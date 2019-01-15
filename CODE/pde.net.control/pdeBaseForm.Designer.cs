namespace pde.net.control
{
    partial class pdeBaseForm
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
            this.piCloseBox = new System.Windows.Forms.PictureBox();
            this.piMaximizeBox = new System.Windows.Forms.PictureBox();
            this.piMinimizeBox = new System.Windows.Forms.PictureBox();
            this.pdeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pTitle = new System.Windows.Forms.Panel();
            this.lCaption = new System.Windows.Forms.Label();
            this.pLogoIcon = new System.Windows.Forms.PictureBox();
            this.piSysMenu = new System.Windows.Forms.PictureBox();
            this.piSysSet = new System.Windows.Forms.PictureBox();
            this.piSysSkin = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).BeginInit();
            this.pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSkin)).BeginInit();
            this.SuspendLayout();
            // 
            // piCloseBox
            // 
            this.piCloseBox.BackColor = System.Drawing.Color.Transparent;
            this.piCloseBox.Location = new System.Drawing.Point(654, 4);
            this.piCloseBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piCloseBox.MaximumSize = new System.Drawing.Size(30, 25);
            this.piCloseBox.MinimumSize = new System.Drawing.Size(30, 25);
            this.piCloseBox.Name = "piCloseBox";
            this.piCloseBox.Size = new System.Drawing.Size(30, 25);
            this.piCloseBox.TabIndex = 5;
            this.piCloseBox.TabStop = false;
            this.piCloseBox.Click += new System.EventHandler(this.piCloseBox_Click);
            this.piCloseBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piCloseBox_Paint);
            // 
            // piMaximizeBox
            // 
            this.piMaximizeBox.BackColor = System.Drawing.Color.Transparent;
            this.piMaximizeBox.Location = new System.Drawing.Point(614, 4);
            this.piMaximizeBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piMaximizeBox.MaximumSize = new System.Drawing.Size(25, 25);
            this.piMaximizeBox.MinimumSize = new System.Drawing.Size(25, 25);
            this.piMaximizeBox.Name = "piMaximizeBox";
            this.piMaximizeBox.Size = new System.Drawing.Size(25, 25);
            this.piMaximizeBox.TabIndex = 4;
            this.piMaximizeBox.TabStop = false;
            this.piMaximizeBox.Click += new System.EventHandler(this.piMaximizeBox_Click);
            this.piMaximizeBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piMaximizeBox_Paint);
            // 
            // piMinimizeBox
            // 
            this.piMinimizeBox.BackColor = System.Drawing.Color.Transparent;
            this.piMinimizeBox.Location = new System.Drawing.Point(574, 4);
            this.piMinimizeBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piMinimizeBox.MaximumSize = new System.Drawing.Size(25, 25);
            this.piMinimizeBox.MinimumSize = new System.Drawing.Size(25, 25);
            this.piMinimizeBox.Name = "piMinimizeBox";
            this.piMinimizeBox.Size = new System.Drawing.Size(25, 25);
            this.piMinimizeBox.TabIndex = 3;
            this.piMinimizeBox.TabStop = false;
            this.piMinimizeBox.Click += new System.EventHandler(this.piMinimizeBox_Click);
            this.piMinimizeBox.Paint += new System.Windows.Forms.PaintEventHandler(this.piMinimizeBox_Paint);
            // 
            // pTitle
            // 
            this.pTitle.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.pTitle.Controls.Add(this.piCloseBox);
            this.pTitle.Controls.Add(this.lCaption);
            this.pTitle.Controls.Add(this.pLogoIcon);
            this.pTitle.Controls.Add(this.piSysMenu);
            this.pTitle.Controls.Add(this.piMaximizeBox);
            this.pTitle.Controls.Add(this.piSysSet);
            this.pTitle.Controls.Add(this.piSysSkin);
            this.pTitle.Controls.Add(this.piMinimizeBox);
            this.pTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTitle.Location = new System.Drawing.Point(0, 0);
            this.pTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(697, 30);
            this.pTitle.TabIndex = 6;
            this.pTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PTitleMouseDoubleClick);
            this.pTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PdeFormMouseMove);
            // 
            // lCaption
            // 
            this.lCaption.AutoSize = true;
            this.lCaption.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lCaption.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lCaption.Location = new System.Drawing.Point(30, 7);
            this.lCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new System.Drawing.Size(32, 17);
            this.lCaption.TabIndex = 6;
            this.lCaption.Text = "标题";
            this.lCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pLogoIcon
            // 
            this.pLogoIcon.BackColor = System.Drawing.Color.Transparent;
            this.pLogoIcon.Location = new System.Drawing.Point(7, 5);
            this.pLogoIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pLogoIcon.MaximumSize = new System.Drawing.Size(20, 20);
            this.pLogoIcon.MinimumSize = new System.Drawing.Size(20, 20);
            this.pLogoIcon.Name = "pLogoIcon";
            this.pLogoIcon.Size = new System.Drawing.Size(20, 20);
            this.pLogoIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pLogoIcon.TabIndex = 5;
            this.pLogoIcon.TabStop = false;
            // 
            // piSysMenu
            // 
            this.piSysMenu.BackColor = System.Drawing.Color.Transparent;
            this.piSysMenu.Location = new System.Drawing.Point(459, 4);
            this.piSysMenu.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piSysMenu.MaximumSize = new System.Drawing.Size(25, 25);
            this.piSysMenu.MinimumSize = new System.Drawing.Size(25, 25);
            this.piSysMenu.Name = "piSysMenu";
            this.piSysMenu.Size = new System.Drawing.Size(25, 25);
            this.piSysMenu.TabIndex = 11;
            this.piSysMenu.TabStop = false;
            this.piSysMenu.Click += new System.EventHandler(this.piSysMenu_Click);
            this.piSysMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.piSysMenu_Paint);
            // 
            // piSysSet
            // 
            this.piSysSet.BackColor = System.Drawing.Color.Transparent;
            this.piSysSet.Location = new System.Drawing.Point(494, 4);
            this.piSysSet.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piSysSet.MaximumSize = new System.Drawing.Size(25, 25);
            this.piSysSet.MinimumSize = new System.Drawing.Size(25, 25);
            this.piSysSet.Name = "piSysSet";
            this.piSysSet.Size = new System.Drawing.Size(25, 25);
            this.piSysSet.TabIndex = 9;
            this.piSysSet.TabStop = false;
            this.piSysSet.Paint += new System.Windows.Forms.PaintEventHandler(this.piSysSet_Paint);
            // 
            // piSysSkin
            // 
            this.piSysSkin.BackColor = System.Drawing.Color.Transparent;
            this.piSysSkin.Location = new System.Drawing.Point(534, 4);
            this.piSysSkin.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.piSysSkin.MaximumSize = new System.Drawing.Size(25, 25);
            this.piSysSkin.MinimumSize = new System.Drawing.Size(25, 25);
            this.piSysSkin.Name = "piSysSkin";
            this.piSysSkin.Size = new System.Drawing.Size(25, 25);
            this.piSysSkin.TabIndex = 10;
            this.piSysSkin.TabStop = false;
            this.piSysSkin.Paint += new System.Windows.Forms.PaintEventHandler(this.piSysSkin_Paint);
            // 
            // pdeBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(697, 462);
            this.Controls.Add(this.pTitle);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "pdeBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "pdeBaseForm";
            this.Load += new System.EventHandler(this.pdeBaseForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fmPdeBase_KeyPress);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PdeFormMouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.piCloseBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMaximizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piMinimizeBox)).EndInit();
            this.pTitle.ResumeLayout(false);
            this.pTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pLogoIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.piSysSkin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox piCloseBox;
        public System.Windows.Forms.PictureBox piMaximizeBox;
        public System.Windows.Forms.PictureBox piMinimizeBox;
        private System.Windows.Forms.ToolTip pdeToolTip;
        public System.Windows.Forms.Panel pTitle;
        public System.Windows.Forms.Label lCaption;
        public System.Windows.Forms.PictureBox pLogoIcon;
        public System.Windows.Forms.PictureBox piSysSkin;
        public System.Windows.Forms.PictureBox piSysSet;
        public System.Windows.Forms.PictureBox piSysMenu;
    }
}