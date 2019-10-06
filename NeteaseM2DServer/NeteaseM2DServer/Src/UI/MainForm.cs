﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeteaseM2DServer.Src.Service;
using System.Threading;
using NeteaseM2DServer.Src.Model;
using NeteaseM2DServer.Src.Api;
using System.Text.RegularExpressions;
using NeteaseM2DServer.Src.Util;
using System.Runtime.CompilerServices;
using System.Net;
using System.Net.Sockets;
using QRCoder;

namespace NeteaseM2DServer.Src.UI {

    public partial class MainForm : Form {

        private static MainForm Instance;

        public static MainForm GetInstance() {
            if (Instance == null)
                Instance = new MainForm();
            return Instance;
        }

        private MainForm() {
            InitializeComponent();
        }

        // StartThread SocketMetaDataCb SocketPlaybackStateCb StopThread
        #region 线程服务

        private Thread socketThread;
        private SocketService socketService;

        /// <summary>
        /// 开始线程
        /// </summary>
        /// <param name="port">端口号</param>
        private void StartThread(int port) {
            socketService = new SocketService();
            socketService.port = port;

            socketService.listenCb = (ok) => {
                // コントロールが作成されたスレッド以外のスレッドからコントロール 'buttonListen' がアクセスされました
                this.Invoke(new Action(() => {
                    Global.isListening = ok;
                    numericUpDownPort.Enabled = !ok;
                    if (ok) {
                        // 监听成功

                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(Global.qrCodeMagic + "://" +
                            textBoxIP.Text + ":" + socketService.port, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Global.qrCodeImage = qrCode.GetGraphic(7);
                        buttonQrCode.Visible = true;

                        labelSongDuration.Text = "正在等待歌曲...";
                        buttonListen.Text = "取消监听";

                        labelSongTitle.Text = "未知歌曲";
                        labelSongArtist.Text = "未知歌手";
                        labelSongAlbum.Text = "未知专辑";
                    } else {
                        labelSongDuration.Text = "未监听...";
                        buttonListen.Text = "监听端口";
                        timerGlobal.Enabled = false;
                        MessageBox.Show("端口监听失败，可能是被占用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
            };

            socketService.pingCb = (ok) => {
                Console.WriteLine("ping");
                this.Invoke(new Action(() => {
                    QrCodeform.Close();
                }));
            };

            socketService.metaDataCb = SocketMetaDataCb;
            socketService.playbackStateCb = SocketPlaybackStateCb;

            socketThread = new Thread(new ThreadStart(socketService.RunThread));
            socketThread.IsBackground = true;
            socketThread.Start();
        }

        /// <summary>
        /// Socket 获取 MetaData 歌曲信息 回调
        /// </summary>
        /// <param name="obj">Metadata</param>
        private void SocketMetaDataCb(Metadata obj) {
            Console.WriteLine(obj.ToString());
            if (!Global.isListening) return;

            this.Invoke(new Action(() => {
                buttonShowLyric.Enabled = buttonOpenWeb.Enabled = true;
                labelSongTitle.Visible = labelSongArtist.Visible = labelSongAlbum.Visible = true;

                if (Global.currentSong != null && Global.currentSong.Equals(obj)) return;
                Global.currentSong = obj;

                labelSongTitle.Text = "标题：" + obj.title;
                labelSongArtist.Text = "歌手：" + obj.artist;
                labelSongAlbum.Text = "专辑：" + obj.album;
                toolTip.SetToolTip(labelSongTitle, labelSongTitle.Text.Substring(3));
                toolTip.SetToolTip(labelSongArtist, labelSongArtist.Text.Substring(3));
                toolTip.SetToolTip(labelSongAlbum, labelSongAlbum.Text.Substring(3));

                double s = Global.currentSong.duration;
                Global.durationStr = ((int)(s / 60.0)).ToString("00") + ":" + ((int)(s % 60.0)).ToString("00");

                // 新线程搜索，防止阻塞 UI
                new Thread(() => {
                    this.Invoke(new Action(() => {
                        // 正在搜索
                        LyricForm.getInstance().updateSongLyric(true);
                    }));
                    Search();
                    this.Invoke(new Action(() => {
                        // 搜索完成
                        LyricForm.getInstance().updateSongLyric(false);
                    }));
                }).Start();

                // 开始全局计时
                timerGlobal.Enabled = true;
            }));
        }

        /// <summary>
        /// Socket 获取 PlatbackState 播放状态 回调
        /// </summary>
        /// <param name="obj">PlaybackState</param>
        private void SocketPlaybackStateCb(PlaybackState obj) {
            Global.stateUpdateMS = CommonUtil.GetTimeStamp();

            Console.WriteLine(obj.ToString());
            if (!Global.isListening) return;

            this.Invoke(new Action(() => {
                labelSongTitle.Visible = labelSongArtist.Visible = labelSongAlbum.Visible = true;
                timerGlobal.Enabled = true;
                Global.currentState = obj;
            }));
        }

        /// <summary>
        /// 结束线程
        /// </summary>
        private void StopThread() {
            if (socketService != null)
                socketService.serverListener.Stop();
            if (socketThread != null)
                socketThread.Abort();
        }

        #endregion // 线程服务

        // Search
        #region 歌曲查找

        /// <summary>
        /// 判断搜索到的结果是否正确，先查专辑再查歌名
        /// </summary>
        private bool checkSearchResult(Song searchRet, Metadata trueRet) {
            if (!searchRet.Al.Name.Equals(trueRet.album)) return false;
            if (searchRet.Name.IndexOf("(") != -1) {
                // 存在括号
                if (trueRet.title.IndexOf("(") == -1) return false;

                string[] searchToken = searchRet.Name.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                string[] trueToken = trueRet.title.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                
                // xxx1 (xxx2)

                // xxx1
                if (!searchToken[0].Trim().Equals(trueToken[0].Trim())) return false;
                // xxx2
                if (!searchToken[1].Trim().Equals(trueToken[1].Trim())) return false;

                else return true;
            } else {
                // 不存在括号
                return searchRet.Name.Trim().Equals(new Regex("\\(.*\\)").Replace(trueRet.title, "").Trim());
            }
        }

        /// <summary>
        /// 列表内搜索
        /// </summary>
        /// <returns>-1: 404</returns>
        private int checkContinueResult(SearchResult searchRet, Metadata trueRet) {
            if (searchRet.Result == null || searchRet.Result.Songs == null || searchRet.Result.Songs.Count == 0) return -1;
            for (int i = 0; i < searchRet.Result.Songs.Count; i++) { // <<< 不能用 searchRet.Result.SongCount
                if (checkSearchResult(searchRet.Result.Songs.ElementAt(i), trueRet)) 
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 查找歌曲 Id 和 歌词
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Search() {
            var api = new NeteaseMusicAPI();

            // 匹配歌曲
            Song resultSong = null;
            // 匹配歌曲索引
            int id = -1;

            // 几种方案搜索
            string titleOfPh = Global.currentSong.title;
            string titleOfNoPh = new Regex("\\(.*\\)").Replace(titleOfPh, "");
            string[] searchToken = {
                titleOfPh,                                                                          // 标题
                titleOfPh + " " + Global.currentSong.artist,                                        // 标题 + 歌手
                titleOfPh + " " + Global.currentSong.artist + " " + Global.currentSong.album,       // 标题 + 歌手 + 专辑
                titleOfNoPh,                                                                        // 去括号标题
                titleOfNoPh + " " + Global.currentSong.artist,                                      // 去括号标题 + 歌手
                titleOfNoPh + " " + Global.currentSong.artist + " " + Global.currentSong.album      // 去括号标题 + 歌手 + 专辑
            };

            SearchResult sRet = null;
            foreach (var stk in searchToken) {
                Console.WriteLine(stk);
                sRet = api.Search(stk);
                if (sRet.Code == 200 && ((id = checkContinueResult(sRet, Global.currentSong)) != -1))
                    break;
                else
                    sRet = null;
            }
            if (sRet != null && id != -1) 
                resultSong = sRet.Result.Songs.ElementAt(id);

            // 搜索结束
            if (resultSong != null) {
                Global.MusicId = resultSong.Id;
                LyricResult lyricResult = api.Lyric(Global.MusicId);

                if (lyricResult.Code == 200 && lyricResult.Lrc != null && lyricResult.Lrc.Lyric != null && lyricResult.Lrc.Lyric != "") {
                    Global.MusicLyricPage = LyricPage.parseLrc(lyricResult.Lrc.Lyric);
                    // Console.WriteLine(lyricResult.Lrc.Lyric);
                    Global.MusicLyricState = Global.LyricState.Found;
                } else {
                    Global.MusicLyricPage = null;
                    if (lyricResult.Nolyric == true)
                        Global.MusicLyricState = Global.LyricState.PureMusic;
                    else
                        Global.MusicLyricState = Global.LyricState.NotFound;
                }
            } else {
                Global.MusicId = -1;
                Global.MusicLyricPage = null;
                Global.MusicLyricState = Global.LyricState.NotFound;
            }
        }

        #endregion // 歌曲查找

        // MainForm_Load MainForm_FormClosed buttonListen_Click buttonExit_Click buttonShowLyric_Click buttonOpenWeb_Click
        #region 界面交互

        private void MainForm_Load(object sender, EventArgs e) {

            timeAdjustContextMenu.Renderer = new NativeRenderer(NativeRenderer.ToolbarTheme.MediaToolbar);
            contextMenuStrip.Renderer = new NativeRenderer(NativeRenderer.ToolbarTheme.MediaToolbar);

            // Load Setting
            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;

            string ip = "";
            foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ip = ipa.ToString();

            textBoxIP.Text = ip;

            labelSongDuration.Text = "未监听...";
            Global.MainFormTimer = timerSong_Tick;
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            StopThread();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 监听 / 取消监听
        /// </summary>
        private void buttonListen_Click(object sender, EventArgs e) {
            if ((sender as Button).Text == "监听端口") {
                StartThread(int.Parse(numericUpDownPort.Value.ToString()));
            } else {
                StopThread();

                labelSongDuration.Text = "未监听...";
                buttonListen.Text = "监听端口";

                numericUpDownPort.Enabled = true;
                timerGlobal.Enabled = false;
                labelSongTitle.Visible = labelSongArtist.Visible = labelSongAlbum.Visible = false;

                buttonQrCode.Visible = false;
                Global.isListening = buttonShowLyric.Enabled = buttonOpenWeb.Enabled = false;
                Global.currentSong = null;
                Global.currentState = null;
                Global.isListening = false;
                Global.MusicId = -1;
                Global.qrCodeImage = null;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        private void buttonExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// 打开歌词
        /// </summary>
        private void buttonShowLyric_Click(object sender, EventArgs e) {
            LyricForm lyricForm = LyricForm.getInstance();
            lyricForm.Activate();
            lyricForm.Show();
        }

        /// <summary>
        /// 打开网页
        /// </summary>
        private void buttonOpenWeb_Click(object sender, EventArgs e) {
            if (Global.MusicId != -1) {
                string url = "https://music.163.com/#/song?id=" + Global.MusicId;
                System.Diagnostics.Process.Start(url);
            } else {
                DialogResult ret =
                    MessageBox.Show("未找到歌曲 \"" + Global.currentSong.title + "\"。", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (ret == DialogResult.Retry) {

                    // 阻塞
                    LyricForm.getInstance().updateSongLyric(true);
                    Search();
                    LyricForm.getInstance().updateSongLyric(false);
                    buttonOpenWeb_Click(sender, e);
                }
            }
        }

        /// <summary>
        /// 二维码窗体
        /// </summary>
        private Form QrCodeform;

        /// <summary>
        /// 显示二维码
        /// </summary>
        private void buttonQrCode_Click(object sender, EventArgs e) {

            if (Global.qrCodeImage == null) {
                MessageBox.Show("不存在二维码。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            QrCodeform = new Form();
            QrCodeform.Text = "连接二维码";
            QrCodeform.Name = "QrCodeForm";
            QrCodeform.FormBorderStyle = FormBorderStyle.FixedDialog;
            QrCodeform.MaximizeBox = false;
            QrCodeform.MinimizeBox = false;
            QrCodeform.ShowInTaskbar = false;
            QrCodeform.StartPosition = FormStartPosition.CenterScreen;
            QrCodeform.Size = Global.qrCodeImage.Size;

            PictureBox pictureBox = new PictureBox();
            pictureBox.Name = "pictureBox";
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = Global.qrCodeImage;

            QrCodeform.Controls.Add(pictureBox);
            pictureBox.Dock = DockStyle.Fill;

            QrCodeform.Show(this);
        }

        #endregion // 界面交互

        // buttonTimeAdjust_Click menuItemFasterSlower_Click contextMenuStrip_Opening
        #region 弹出菜单

        /// <summary>
        /// 时间调整 弹出菜单
        /// </summary>
        private void buttonTimeAdjust_Click(object sender, EventArgs e) {
            Point senderPnt = PointToScreen((sender as Button).Location);
            timeAdjustContextMenu.Show(
                senderPnt.X,
                senderPnt.Y + (sender as Button).Height
            );
        }

        /// <summary>
        /// 菜单，时间调整
        /// </summary>
        private void menuItemFasterSlower_Click(object sender, EventArgs e) {
            string tag = (sender as ToolStripMenuItem).Tag.ToString();
            bool isFaster = tag.Substring(0, 1) == "+";
            double it = double.Parse(tag.Substring(1));
            if (isFaster)
                Global.stateUpdateMS -= (long)(it * 1000);
            else
                Global.stateUpdateMS += (long)(it * 1000);
        }

        Control rightClickControl = null;

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e) {
            rightClickControl = (sender as ContextMenuStrip).SourceControl;
        }

        /// <summary>
        /// 右键复制
        /// </summary>
        private void menuItemCopy_Click(object sender, EventArgs e) {
            if (rightClickControl != null)
                Clipboard.SetText(rightClickControl.Text.Substring(3));
        }

        #endregion // 弹出菜单

        /// <summary>
        /// 时间更新
        /// </summary>
        private void timerSong_Tick() {
            long now = CommonUtil.GetTimeStamp();
            if (Global.currentState == null) return;

            // 当前时间
            double s = Global.currentState.currentPosSecond + (double)((now - Global.stateUpdateMS) / 1000.0);

            // 暂停 或 超过
            if (!Global.currentState.isPlay || (Global.currentSong != null && s >= Global.currentSong.duration))
                return;

            Global.currentPos = (int)(s * 1000);
            string currentPos = ((int)(s / 60.0)).ToString("00") + ":" + ((int)(s % 60.0)).ToString("00");
            

            labelSongDuration.Text = currentPos + " / " + (Global.durationStr == "" || Global.durationStr == null ? "未知" : Global.durationStr);
        }

        /// <summary>
        /// 全局计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerGlobal_Tick(object sender, EventArgs e) {
            if (Global.MainFormTimer != null)
                Global.MainFormTimer();
            if (Global.LyricFormTimer != null)
                Global.LyricFormTimer();
        }
    }
}