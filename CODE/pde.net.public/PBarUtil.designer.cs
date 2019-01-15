namespace pde.pub
{
    partial class PBarUtil
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PBarUtil));
            this.lblInfo = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).BeginInit();
            this.pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSkin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // piCloseBox
            // 
            this.piCloseBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piCloseBox.BackColor = System.Drawing.Color.White;
            this.piCloseBox.Location = new System.Drawing.Point(482, 0);
            this.piCloseBox.MaximumSize = new System.Drawing.Size(30, 26);
            this.piCloseBox.MinimumSize = new System.Drawing.Size(30, 26);
            this.piCloseBox.Size = new System.Drawing.Size(30, 26);
            this.piCloseBox.Click += new System.EventHandler(this.piCloseBox_Click);
            // 
            // piMaximizeBox
            // 
            this.piMaximizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piMaximizeBox.BackColor = System.Drawing.Color.White;
            this.piMaximizeBox.Location = new System.Drawing.Point(455, 2);
            this.piMaximizeBox.MaximumSize = new System.Drawing.Size(26, 26);
            this.piMaximizeBox.MinimumSize = new System.Drawing.Size(26, 26);
            this.piMaximizeBox.Size = new System.Drawing.Size(26, 26);
            // 
            // piMinimizeBox
            // 
            this.piMinimizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piMinimizeBox.BackColor = System.Drawing.Color.White;
            this.piMinimizeBox.Location = new System.Drawing.Point(456, 0);
            this.piMinimizeBox.MaximumSize = new System.Drawing.Size(26, 26);
            this.piMinimizeBox.MinimumSize = new System.Drawing.Size(26, 26);
            this.piMinimizeBox.Size = new System.Drawing.Size(26, 26);
            // 
            // pTitle
            // 
            this.pTitle.BackColor = System.Drawing.Color.White;
            this.pTitle.Size = new System.Drawing.Size(512, 30);
            // 
            // lCaption
            // 
            this.lCaption.Size = new System.Drawing.Size(0, 17);
            this.lCaption.Text = "";
            // 
            // pLogoIcon
            // 
            this.pLogoIcon.Image = ((System.Drawing.Image)(resources.GetObject("pLogoIcon.Image")));
            // 
            // piSysSkin
            // 
            this.piSysSkin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piSysSkin.BackColor = System.Drawing.Color.White;
            this.piSysSkin.Location = new System.Drawing.Point(456, 0);
            this.piSysSkin.MaximumSize = new System.Drawing.Size(26, 26);
            this.piSysSkin.MinimumSize = new System.Drawing.Size(26, 26);
            this.piSysSkin.Size = new System.Drawing.Size(26, 26);
            // 
            // piSysSet
            // 
            this.piSysSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piSysSet.BackColor = System.Drawing.Color.White;
            this.piSysSet.Location = new System.Drawing.Point(456, 0);
            this.piSysSet.MaximumSize = new System.Drawing.Size(26, 26);
            this.piSysSet.MinimumSize = new System.Drawing.Size(26, 26);
            this.piSysSet.Size = new System.Drawing.Size(26, 26);
            // 
            // piSysMenu
            // 
            this.piSysMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.piSysMenu.BackColor = System.Drawing.Color.White;
            this.piSysMenu.Location = new System.Drawing.Point(404, 0);
            this.piSysMenu.MaximumSize = new System.Drawing.Size(26, 26);
            this.piSysMenu.MinimumSize = new System.Drawing.Size(26, 26);
            this.piSysMenu.Size = new System.Drawing.Size(26, 26);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(50, 27);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(113, 17);
            this.lblInfo.TabIndex = 12;
            this.lblInfo.Text = "正在执行，请稍候...";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(46, 54);
            this.pb.Name = "pb";
            this.pb.RightToLeftLayout = true;
            this.pb.Size = new System.Drawing.Size(410, 18);
            this.pb.Step = 1;
            this.pb.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb.TabIndex = 13;
            this.pb.Value = 100;
            // 
            // lblProgress
            // 
            this.lblProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblProgress.Location = new System.Drawing.Point(357, 27);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(92, 17);
            this.lblProgress.TabIndex = 14;
            this.lblProgress.Text = "50/100";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picWait
            // 
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::pde.net.@public.Properties.Resources.wait;
            this.picWait.Location = new System.Drawing.Point(50, 32);
            this.picWait.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(35, 35);
            this.picWait.TabIndex = 15;
            this.picWait.TabStop = false;
            // 
            // PBarUtil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(512, 99);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pb);
            this.DoubleBuffered = true;
            this.Name = "PBarUtil";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ShowMaximizeBtn = false;
            this.ShowMinimizeBtn = false;
            this.ShowSysMenuBtn = false;
            this.ShowSysSetBtn = false;
            this.ShowSysSkinBtn = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SysTitleBackColor = System.Drawing.Color.White;
            this.Text = "";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PBarUtil_FormClosing);
            this.Load += new System.EventHandler(this.fmProgress_Load);
            this.Shown += new System.EventHandler(this.PBarUtil_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PBarUtil_Paint);
            this.Controls.SetChildIndex(this.pTitle, 0);
            this.Controls.SetChildIndex(this.pb, 0);
            this.Controls.SetChildIndex(this.lblProgress, 0);
            this.Controls.SetChildIndex(this.picWait, 0);
            this.Controls.SetChildIndex(this.lblInfo, 0);
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).EndInit();
            this.pTitle.ResumeLayout(false);
            this.pTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSkin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ProgressBar pb;
        public System.Windows.Forms.Label lblInfo;
        public System.Windows.Forms.Label lblProgress;
        public System.Windows.Forms.PictureBox picWait;
    }
}
