﻿
namespace NcmlAtwServer {
    partial class LyricForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tmrShow = new System.Windows.Forms.Timer(this.components);
            this.tmrHide = new System.Windows.Forms.Timer(this.components);
            this.btnOption = new System.Windows.Forms.Button();
            this.cmsOption = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miLockWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdjustOffset = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpacity = new System.Windows.Forms.ToolStripMenuItem();
            this.miRestoreWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.divOption1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFont = new System.Windows.Forms.ToolStripMenuItem();
            this.miForeColor = new System.Windows.Forms.ToolStripMenuItem();
            this.miBackColor = new System.Windows.Forms.ToolStripMenuItem();
            this.miRestoreStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.divOption2 = new System.Windows.Forms.ToolStripSeparator();
            this.miShowMainWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnFaster = new System.Windows.Forms.Button();
            this.btnSlower = new System.Windows.Forms.Button();
            this.lblLyric = new System.Windows.Forms.Label();
            this.tmrText = new System.Windows.Forms.Timer(this.components);
            this.miShowLyric = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrShow
            // 
            this.tmrShow.Interval = 1;
            this.tmrShow.Tick += new System.EventHandler(this.TmrShow_Tick);
            // 
            // tmrHide
            // 
            this.tmrHide.Interval = 1;
            this.tmrHide.Tick += new System.EventHandler(this.TmrHide_Tick);
            // 
            // btnOption
            // 
            this.btnOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOption.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOption.Image = global::NcmlAtwServer.Properties.Resources.Option;
            this.btnOption.Location = new System.Drawing.Point(1320, 5);
            this.btnOption.Name = "btnOption";
            this.btnOption.Size = new System.Drawing.Size(24, 24);
            this.btnOption.TabIndex = 0;
            this.tipMain.SetToolTip(this.btnOption, "左键弹出菜单，右键移动歌词。\r\n注意：仅当解除锁定时才能够移动窗口和调整大小。");
            this.btnOption.UseVisualStyleBackColor = true;
            this.btnOption.Click += new System.EventHandler(this.BtnOption_Click);
            // 
            // cmsOption
            // 
            this.cmsOption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowLyric,
            this.miLockWindow,
            this.miAdjustOffset,
            this.miOpacity,
            this.miRestoreWindow,
            this.divOption1,
            this.miFont,
            this.miForeColor,
            this.miBackColor,
            this.miRestoreStyle,
            this.divOption2,
            this.miShowMainWindow,
            this.miExit});
            this.cmsOption.Name = "cmsOption";
            this.cmsOption.Size = new System.Drawing.Size(198, 280);
            // 
            // miLockWindow
            // 
            this.miLockWindow.Checked = true;
            this.miLockWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miLockWindow.Name = "miLockWindow";
            this.miLockWindow.Size = new System.Drawing.Size(197, 22);
            this.miLockWindow.Text = "锁定窗口(L)";
            this.miLockWindow.Click += new System.EventHandler(this.MiLockWindow_Click);
            // 
            // miAdjustOffset
            // 
            this.miAdjustOffset.Checked = true;
            this.miAdjustOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miAdjustOffset.Name = "miAdjustOffset";
            this.miAdjustOffset.Size = new System.Drawing.Size(197, 22);
            this.miAdjustOffset.Text = "调整时间差(&D)";
            this.miAdjustOffset.Click += new System.EventHandler(this.MiAdjustOffset_Click);
            // 
            // miOpacity
            // 
            this.miOpacity.Name = "miOpacity";
            this.miOpacity.Size = new System.Drawing.Size(197, 22);
            this.miOpacity.Text = "透明度(&O)";
            // 
            // miRestoreWindow
            // 
            this.miRestoreWindow.Name = "miRestoreWindow";
            this.miRestoreWindow.Size = new System.Drawing.Size(197, 22);
            this.miRestoreWindow.Text = "恢复窗口位置和大小(&R)";
            this.miRestoreWindow.Click += new System.EventHandler(this.MiRestoreWindow_Click);
            // 
            // divOption1
            // 
            this.divOption1.Name = "divOption1";
            this.divOption1.Size = new System.Drawing.Size(194, 6);
            // 
            // miFont
            // 
            this.miFont.Name = "miFont";
            this.miFont.Size = new System.Drawing.Size(197, 22);
            this.miFont.Text = "歌词字体(&F)";
            this.miFont.Click += new System.EventHandler(this.MiFont_Click);
            // 
            // miForeColor
            // 
            this.miForeColor.Name = "miForeColor";
            this.miForeColor.Size = new System.Drawing.Size(197, 22);
            this.miForeColor.Text = "歌词颜色(&C)";
            this.miForeColor.Click += new System.EventHandler(this.MiForeColor_Click);
            // 
            // miBackColor
            // 
            this.miBackColor.Name = "miBackColor";
            this.miBackColor.Size = new System.Drawing.Size(197, 22);
            this.miBackColor.Text = "背景颜色(&B)";
            this.miBackColor.Click += new System.EventHandler(this.MiBackColor_Click);
            // 
            // miRestoreStyle
            // 
            this.miRestoreStyle.Name = "miRestoreStyle";
            this.miRestoreStyle.Size = new System.Drawing.Size(197, 22);
            this.miRestoreStyle.Text = "恢复默认样式(S)";
            this.miRestoreStyle.Click += new System.EventHandler(this.MiRestoreStyle_Click);
            // 
            // divOption2
            // 
            this.divOption2.Name = "divOption2";
            this.divOption2.Size = new System.Drawing.Size(194, 6);
            // 
            // miShowMainWindow
            // 
            this.miShowMainWindow.Name = "miShowMainWindow";
            this.miShowMainWindow.Size = new System.Drawing.Size(197, 22);
            this.miShowMainWindow.Text = "显示主界面(&M)";
            this.miShowMainWindow.Click += new System.EventHandler(this.MiShowMainWindow_Click);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(197, 22);
            this.miExit.Text = "关闭歌词(&X)";
            this.miExit.Click += new System.EventHandler(this.MiExit_Click);
            // 
            // btnFaster
            // 
            this.btnFaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFaster.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFaster.Image = global::NcmlAtwServer.Properties.Resources.Faster;
            this.btnFaster.Location = new System.Drawing.Point(1320, 70);
            this.btnFaster.Name = "btnFaster";
            this.btnFaster.Size = new System.Drawing.Size(24, 24);
            this.btnFaster.TabIndex = 0;
            this.tipMain.SetToolTip(this.btnFaster, "歌词加快 0.5 秒");
            this.btnFaster.UseVisualStyleBackColor = true;
            this.btnFaster.Click += new System.EventHandler(this.BtnFaster_Click);
            // 
            // btnSlower
            // 
            this.btnSlower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSlower.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSlower.Image = global::NcmlAtwServer.Properties.Resources.Slower;
            this.btnSlower.Location = new System.Drawing.Point(1293, 70);
            this.btnSlower.Name = "btnSlower";
            this.btnSlower.Size = new System.Drawing.Size(24, 24);
            this.btnSlower.TabIndex = 0;
            this.tipMain.SetToolTip(this.btnSlower, "歌词放慢 0.5 秒");
            this.btnSlower.UseVisualStyleBackColor = true;
            this.btnSlower.Click += new System.EventHandler(this.BtnSlower_Click);
            // 
            // lblLyric
            // 
            this.lblLyric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLyric.Font = new System.Drawing.Font("Yu Gothic UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLyric.Location = new System.Drawing.Point(0, 0);
            this.lblLyric.Name = "lblLyric";
            this.lblLyric.Size = new System.Drawing.Size(1350, 100);
            this.lblLyric.TabIndex = 1;
            this.lblLyric.Text = "未找到歌詞";
            this.lblLyric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrText
            // 
            this.tmrText.Interval = 1;
            this.tmrText.Tick += new System.EventHandler(this.TmrText_Tick);
            // 
            // miShowLyric
            // 
            this.miShowLyric.Name = "miShowLyric";
            this.miShowLyric.Size = new System.Drawing.Size(197, 22);
            this.miShowLyric.Text = "显示完整歌词(&M)";
            this.miShowLyric.Click += new System.EventHandler(this.MiShowLyric_Click);
            // 
            // LyricForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1350, 100);
            this.Controls.Add(this.btnSlower);
            this.Controls.Add(this.btnFaster);
            this.Controls.Add(this.btnOption);
            this.Controls.Add(this.lblLyric);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 62);
            this.Name = "LyricForm";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "NcmlAtw Lyric";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LyricForm_FormClosed);
            this.Load += new System.EventHandler(this.LyricForm_Load);
            this.cmsOption.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrShow;
        private System.Windows.Forms.Timer tmrHide;
        private System.Windows.Forms.Button btnOption;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.ContextMenuStrip cmsOption;
        private System.Windows.Forms.ToolStripMenuItem miRestoreWindow;
        private System.Windows.Forms.ToolStripSeparator divOption2;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miLockWindow;
        private System.Windows.Forms.ToolStripMenuItem miShowMainWindow;
        private System.Windows.Forms.ToolStripMenuItem miOpacity;
        private System.Windows.Forms.Button btnFaster;
        private System.Windows.Forms.Button btnSlower;
        private System.Windows.Forms.ToolStripMenuItem miAdjustOffset;
        private System.Windows.Forms.Label lblLyric;
        private System.Windows.Forms.Timer tmrText;
        private System.Windows.Forms.ToolStripSeparator divOption1;
        private System.Windows.Forms.ToolStripMenuItem miFont;
        private System.Windows.Forms.ToolStripMenuItem miForeColor;
        private System.Windows.Forms.ToolStripMenuItem miBackColor;
        private System.Windows.Forms.ToolStripMenuItem miRestoreStyle;
        private System.Windows.Forms.ToolStripMenuItem miShowLyric;
    }
}