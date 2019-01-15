namespace pde.net.control
{
   public partial class fmPdeBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pTitle = new System.Windows.Forms.Panel();
            this.pCaption = new System.Windows.Forms.Panel();
            this.lCaption = new System.Windows.Forms.Label();
            this.pLogoIcon = new System.Windows.Forms.PictureBox();
            this.pSystemButtons = new System.Windows.Forms.Panel();
            this.piCloseBox = new System.Windows.Forms.PictureBox();
            this.piMaximizeBox = new System.Windows.Forms.PictureBox();
            this.piMinimizeBox = new System.Windows.Forms.PictureBox();
            this.pdeBaseTip = new System.Windows.Forms.ToolTip(this.components);
            this.pTitle.SuspendLayout();
            this.pCaption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).BeginInit();
            this.pSystemButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pTitle
            // 
            this.pTitle.BackColor = System.Drawing.Color.Transparent;
            this.pTitle.Controls.Add(this.pCaption);
            this.pTitle.Controls.Add(this.pSystemButtons);
            this.pTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTitle.Location = new System.Drawing.Point(0, 0);
            this.pTitle.Margin = new System.Windows.Forms.Padding(4);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(535, 30);
            this.pTitle.TabIndex = 7;
            this.pTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pTitle_MouseDoubleClick);
            this.pTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTitle_MouseMove);
            // 
            // pCaption
            // 
            this.pCaption.Controls.Add(this.lCaption);
            this.pCaption.Controls.Add(this.pLogoIcon);
            this.pCaption.Location = new System.Drawing.Point(0, 0);
            this.pCaption.Margin = new System.Windows.Forms.Padding(4);
            this.pCaption.Name = "pCaption";
            this.pCaption.Size = new System.Drawing.Size(294, 30);
            this.pCaption.TabIndex = 6;
            // 
            // lCaption
            // 
            this.lCaption.AutoSize = true;
            this.lCaption.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lCaption.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lCaption.Location = new System.Drawing.Point(29, 7);
            this.lCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new System.Drawing.Size(32, 17);
            this.lCaption.TabIndex = 2;
            this.lCaption.Text = "标题";
            this.lCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pLogoIcon
            // 
            this.pLogoIcon.BackColor = System.Drawing.Color.Transparent;
            this.pLogoIcon.Location = new System.Drawing.Point(6, 5);
            this.pLogoIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pLogoIcon.Name = "pLogoIcon";
            this.pLogoIcon.Size = new System.Drawing.Size(21, 20);
            this.pLogoIcon.TabIndex = 1;
            this.pLogoIcon.TabStop = false;
            // 
            // pSystemButtons
            // 
            this.pSystemButtons.BackColor = System.Drawing.Color.Transparent;
            this.pSystemButtons.Controls.Add(this.piCloseBox);
            this.pSystemButtons.Controls.Add(this.piMaximizeBox);
            this.pSystemButtons.Controls.Add(this.piMinimizeBox);
            this.pSystemButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pSystemButtons.Location = new System.Drawing.Point(375, 0);
            this.pSystemButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pSystemButtons.Name = "pSystemButtons";
            this.pSystemButtons.Size = new System.Drawing.Size(160, 30);
            this.pSystemButtons.TabIndex = 5;
            // 
            // piCloseBox
            // 
            this.piCloseBox.BackColor = System.Drawing.Color.Transparent;
            this.piCloseBox.Location = new System.Drawing.Point(88, 4);
            this.piCloseBox.Margin = new System.Windows.Forms.Padding(4);
            this.piCloseBox.Name = "piCloseBox";
            this.piCloseBox.Size = new System.Drawing.Size(29, 22);
            this.piCloseBox.TabIndex = 2;
            this.piCloseBox.TabStop = false;
            this.piCloseBox.Click += new System.EventHandler(this.piCloseBox_Click);
            this.piCloseBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piCloseBox_Paint);
            this.piCloseBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.piCloseBox_MouseDown);
            this.piCloseBox.MouseEnter += new System.EventHandler(this.piCloseBox_MouseEnter);
            this.piCloseBox.MouseLeave += new System.EventHandler(this.piCloseBox_MouseLeave);
            this.piCloseBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.piCloseBox_MouseUp);
            // 
            // piMaximizeBox
            // 
            this.piMaximizeBox.BackColor = System.Drawing.Color.Transparent;
            this.piMaximizeBox.Location = new System.Drawing.Point(46, 4);
            this.piMaximizeBox.Margin = new System.Windows.Forms.Padding(4);
            this.piMaximizeBox.Name = "piMaximizeBox";
            this.piMaximizeBox.Size = new System.Drawing.Size(26, 22);
            this.piMaximizeBox.TabIndex = 1;
            this.piMaximizeBox.TabStop = false;
            this.piMaximizeBox.Click += new System.EventHandler(this.piMaximizeBox_Click);
            this.piMaximizeBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piMaximizeBox_Paint);
            // 
            // piMinimizeBox
            // 
            this.piMinimizeBox.BackColor = System.Drawing.Color.Transparent;
            this.piMinimizeBox.Location = new System.Drawing.Point(4, 4);
            this.piMinimizeBox.Margin = new System.Windows.Forms.Padding(4);
            this.piMinimizeBox.Name = "piMinimizeBox";
            this.piMinimizeBox.Size = new System.Drawing.Size(26, 22);
            this.piMinimizeBox.TabIndex = 0;
            this.piMinimizeBox.TabStop = false;
            this.piMinimizeBox.Click += new System.EventHandler(this.piMinimizeBox_Click);
            this.piMinimizeBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piMinimizeBox_Paint);
            this.piMinimizeBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.piMinimizeBox_MouseDown);
            this.piMinimizeBox.MouseEnter += new System.EventHandler(this.piMinimizeBox_MouseEnter);
            this.piMinimizeBox.MouseLeave += new System.EventHandler(this.piMinimizeBox_MouseLeave);
            this.piMinimizeBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.piMinimizeBox_MouseUp);
            // 
            // fmPdeBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(535, 269);
            this.Controls.Add(this.pTitle);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmPdeBase";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fmPdeBase";
            this.Load += new System.EventHandler(this.fmPdeBase_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fmPdeBase_KeyPress);
            this.pTitle.ResumeLayout(false);
            this.pCaption.ResumeLayout(false);
            this.pCaption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).EndInit();
            this.pSystemButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pTitle;
        public System.Windows.Forms.Panel pCaption;
        public System.Windows.Forms.Label lCaption;
        public System.Windows.Forms.PictureBox pLogoIcon;
        public System.Windows.Forms.Panel pSystemButtons;
        public System.Windows.Forms.PictureBox piMaximizeBox;
        public System.Windows.Forms.PictureBox piMinimizeBox;
        private System.Windows.Forms.ToolTip pdeBaseTip;
        public System.Windows.Forms.PictureBox piCloseBox;
    }
}