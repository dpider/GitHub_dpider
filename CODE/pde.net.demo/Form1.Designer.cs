namespace pde.net.demo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tcDemo = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tpFTPUtil = new System.Windows.Forms.TabPage();
            this.lvFTP = new System.Windows.Forms.ListView();
            this.fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tvFTP = new System.Windows.Forms.TreeView();
            this.pnlFTP = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFTPDeleteDir = new System.Windows.Forms.Button();
            this.btnFTPSTOP = new System.Windows.Forms.Button();
            this.btnFTPPause = new System.Windows.Forms.Button();
            this.btnFTPDeleteFile = new System.Windows.Forms.Button();
            this.btnFTPDown = new System.Windows.Forms.Button();
            this.btnFTPGetList = new System.Windows.Forms.Button();
            this.btnFTPUpload = new System.Windows.Forms.Button();
            this.cbxFTPUseBroken = new System.Windows.Forms.CheckBox();
            this.cbxFTPSSL = new System.Windows.Forms.CheckBox();
            this.lblFTPInfo = new System.Windows.Forms.Label();
            this.rbtnFTPPASV = new System.Windows.Forms.RadioButton();
            this.rbtnFTPPASS = new System.Windows.Forms.RadioButton();
            this.edtFtpPassword = new System.Windows.Forms.TextBox();
            this.edtFtpUser = new System.Windows.Forms.TextBox();
            this.edtFTPPort = new System.Windows.Forms.TextBox();
            this.edtFTPHost = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSelCurFile = new System.Windows.Forms.Button();
            this.edtCurlLocalFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCurlUp = new System.Windows.Forms.Button();
            this.btnCurlResume = new System.Windows.Forms.Button();
            this.btnCurlPause = new System.Windows.Forms.Button();
            this.btnCurlStop = new System.Windows.Forms.Button();
            this.lblCURLInfo = new System.Windows.Forms.Label();
            this.btnCurlDown = new System.Windows.Forms.Button();
            this.edtCurlURL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxEx1 = new pde.net.control.TextBoxEx();
            this.label10 = new System.Windows.Forms.Label();
            this.edtCurlUploadedSize = new System.Windows.Forms.TextBox();
            this.tcDemo.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tpFTPUtil.SuspendLayout();
            this.pnlFTP.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDemo
            // 
            this.tcDemo.Controls.Add(this.tabPage2);
            this.tcDemo.Controls.Add(this.tpFTPUtil);
            this.tcDemo.Controls.Add(this.tabPage1);
            this.tcDemo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDemo.ItemSize = new System.Drawing.Size(54, 22);
            this.tcDemo.Location = new System.Drawing.Point(0, 0);
            this.tcDemo.Name = "tcDemo";
            this.tcDemo.SelectedIndex = 0;
            this.tcDemo.Size = new System.Drawing.Size(973, 477);
            this.tcDemo.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gray;
            this.tabPage2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage2.BackgroundImage")));
            this.tabPage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage2.Controls.Add(this.textBoxEx1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(965, 447);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "控件测试";
            // 
            // tpFTPUtil
            // 
            this.tpFTPUtil.Controls.Add(this.lvFTP);
            this.tpFTPUtil.Controls.Add(this.splitter1);
            this.tpFTPUtil.Controls.Add(this.tvFTP);
            this.tpFTPUtil.Controls.Add(this.pnlFTP);
            this.tpFTPUtil.Location = new System.Drawing.Point(4, 26);
            this.tpFTPUtil.Name = "tpFTPUtil";
            this.tpFTPUtil.Padding = new System.Windows.Forms.Padding(3);
            this.tpFTPUtil.Size = new System.Drawing.Size(965, 447);
            this.tpFTPUtil.TabIndex = 0;
            this.tpFTPUtil.Text = "FTP测试";
            this.tpFTPUtil.UseVisualStyleBackColor = true;
            // 
            // lvFTP
            // 
            this.lvFTP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileName,
            this.fileSize});
            this.lvFTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFTP.HideSelection = false;
            this.lvFTP.Location = new System.Drawing.Point(172, 3);
            this.lvFTP.MultiSelect = false;
            this.lvFTP.Name = "lvFTP";
            this.lvFTP.Size = new System.Drawing.Size(554, 441);
            this.lvFTP.TabIndex = 3;
            this.lvFTP.UseCompatibleStateImageBehavior = false;
            this.lvFTP.View = System.Windows.Forms.View.Details;
            // 
            // fileName
            // 
            this.fileName.Text = "文件名";
            this.fileName.Width = 366;
            // 
            // fileSize
            // 
            this.fileSize.Text = "文件大小";
            this.fileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.fileSize.Width = 112;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(169, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 441);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // tvFTP
            // 
            this.tvFTP.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvFTP.HideSelection = false;
            this.tvFTP.Location = new System.Drawing.Point(3, 3);
            this.tvFTP.Name = "tvFTP";
            this.tvFTP.Size = new System.Drawing.Size(166, 441);
            this.tvFTP.TabIndex = 1;
            this.tvFTP.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFTP_AfterSelect);
            this.tvFTP.DoubleClick += new System.EventHandler(this.tvFTP_DoubleClick);
            // 
            // pnlFTP
            // 
            this.pnlFTP.Controls.Add(this.panel1);
            this.pnlFTP.Controls.Add(this.cbxFTPUseBroken);
            this.pnlFTP.Controls.Add(this.cbxFTPSSL);
            this.pnlFTP.Controls.Add(this.lblFTPInfo);
            this.pnlFTP.Controls.Add(this.rbtnFTPPASV);
            this.pnlFTP.Controls.Add(this.rbtnFTPPASS);
            this.pnlFTP.Controls.Add(this.edtFtpPassword);
            this.pnlFTP.Controls.Add(this.edtFtpUser);
            this.pnlFTP.Controls.Add(this.edtFTPPort);
            this.pnlFTP.Controls.Add(this.edtFTPHost);
            this.pnlFTP.Controls.Add(this.label7);
            this.pnlFTP.Controls.Add(this.label6);
            this.pnlFTP.Controls.Add(this.label5);
            this.pnlFTP.Controls.Add(this.label4);
            this.pnlFTP.Controls.Add(this.label3);
            this.pnlFTP.Controls.Add(this.label2);
            this.pnlFTP.Controls.Add(this.label1);
            this.pnlFTP.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFTP.Location = new System.Drawing.Point(726, 3);
            this.pnlFTP.Name = "pnlFTP";
            this.pnlFTP.Size = new System.Drawing.Size(236, 441);
            this.pnlFTP.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFTPDeleteDir);
            this.panel1.Controls.Add(this.btnFTPSTOP);
            this.panel1.Controls.Add(this.btnFTPPause);
            this.panel1.Controls.Add(this.btnFTPDeleteFile);
            this.panel1.Controls.Add(this.btnFTPDown);
            this.panel1.Controls.Add(this.btnFTPGetList);
            this.panel1.Controls.Add(this.btnFTPUpload);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 284);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 132);
            this.panel1.TabIndex = 10;
            // 
            // btnFTPDeleteDir
            // 
            this.btnFTPDeleteDir.Location = new System.Drawing.Point(89, 87);
            this.btnFTPDeleteDir.Name = "btnFTPDeleteDir";
            this.btnFTPDeleteDir.Size = new System.Drawing.Size(81, 23);
            this.btnFTPDeleteDir.TabIndex = 8;
            this.btnFTPDeleteDir.Text = "删除文件夹";
            this.btnFTPDeleteDir.UseVisualStyleBackColor = true;
            this.btnFTPDeleteDir.Click += new System.EventHandler(this.btnFTPDeleteDir_Click);
            // 
            // btnFTPSTOP
            // 
            this.btnFTPSTOP.Location = new System.Drawing.Point(176, 23);
            this.btnFTPSTOP.Name = "btnFTPSTOP";
            this.btnFTPSTOP.Size = new System.Drawing.Size(49, 23);
            this.btnFTPSTOP.TabIndex = 4;
            this.btnFTPSTOP.Text = "停止";
            this.btnFTPSTOP.UseVisualStyleBackColor = true;
            this.btnFTPSTOP.Click += new System.EventHandler(this.btnFTPSTOP_Click);
            // 
            // btnFTPPause
            // 
            this.btnFTPPause.Location = new System.Drawing.Point(121, 23);
            this.btnFTPPause.Name = "btnFTPPause";
            this.btnFTPPause.Size = new System.Drawing.Size(49, 23);
            this.btnFTPPause.TabIndex = 4;
            this.btnFTPPause.Text = "暂停";
            this.btnFTPPause.UseVisualStyleBackColor = true;
            this.btnFTPPause.Click += new System.EventHandler(this.btnFTPPause_Click);
            // 
            // btnFTPDeleteFile
            // 
            this.btnFTPDeleteFile.Location = new System.Drawing.Point(15, 87);
            this.btnFTPDeleteFile.Name = "btnFTPDeleteFile";
            this.btnFTPDeleteFile.Size = new System.Drawing.Size(62, 23);
            this.btnFTPDeleteFile.TabIndex = 7;
            this.btnFTPDeleteFile.Text = "删除文件";
            this.btnFTPDeleteFile.UseVisualStyleBackColor = true;
            this.btnFTPDeleteFile.Click += new System.EventHandler(this.btnFTPDeleteFile_Click);
            // 
            // btnFTPDown
            // 
            this.btnFTPDown.Location = new System.Drawing.Point(15, 58);
            this.btnFTPDown.Name = "btnFTPDown";
            this.btnFTPDown.Size = new System.Drawing.Size(62, 23);
            this.btnFTPDown.TabIndex = 4;
            this.btnFTPDown.Text = "下载文件";
            this.btnFTPDown.UseVisualStyleBackColor = true;
            this.btnFTPDown.Click += new System.EventHandler(this.btnFTPDown_Click);
            // 
            // btnFTPGetList
            // 
            this.btnFTPGetList.Location = new System.Drawing.Point(15, 23);
            this.btnFTPGetList.Name = "btnFTPGetList";
            this.btnFTPGetList.Size = new System.Drawing.Size(75, 23);
            this.btnFTPGetList.TabIndex = 3;
            this.btnFTPGetList.Text = "获取列表";
            this.btnFTPGetList.UseVisualStyleBackColor = true;
            this.btnFTPGetList.Click += new System.EventHandler(this.btnFTPGetList_Click);
            // 
            // btnFTPUpload
            // 
            this.btnFTPUpload.Location = new System.Drawing.Point(87, 58);
            this.btnFTPUpload.Name = "btnFTPUpload";
            this.btnFTPUpload.Size = new System.Drawing.Size(62, 23);
            this.btnFTPUpload.TabIndex = 6;
            this.btnFTPUpload.Text = "上传文件";
            this.btnFTPUpload.UseVisualStyleBackColor = true;
            this.btnFTPUpload.Click += new System.EventHandler(this.btnFTPUpload_Click);
            // 
            // cbxFTPUseBroken
            // 
            this.cbxFTPUseBroken.AutoSize = true;
            this.cbxFTPUseBroken.Location = new System.Drawing.Point(102, 188);
            this.cbxFTPUseBroken.Name = "cbxFTPUseBroken";
            this.cbxFTPUseBroken.Size = new System.Drawing.Size(96, 16);
            this.cbxFTPUseBroken.TabIndex = 9;
            this.cbxFTPUseBroken.Text = "使用断点续传";
            this.cbxFTPUseBroken.UseVisualStyleBackColor = true;
            // 
            // cbxFTPSSL
            // 
            this.cbxFTPSSL.AutoSize = true;
            this.cbxFTPSSL.Location = new System.Drawing.Point(102, 161);
            this.cbxFTPSSL.Name = "cbxFTPSSL";
            this.cbxFTPSSL.Size = new System.Drawing.Size(90, 16);
            this.cbxFTPSSL.TabIndex = 8;
            this.cbxFTPSSL.Text = "使用SSL加密";
            this.cbxFTPSSL.UseVisualStyleBackColor = true;
            // 
            // lblFTPInfo
            // 
            this.lblFTPInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFTPInfo.Location = new System.Drawing.Point(0, 416);
            this.lblFTPInfo.Name = "lblFTPInfo";
            this.lblFTPInfo.Size = new System.Drawing.Size(236, 25);
            this.lblFTPInfo.TabIndex = 5;
            this.lblFTPInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbtnFTPPASV
            // 
            this.rbtnFTPPASV.AutoSize = true;
            this.rbtnFTPPASV.Checked = true;
            this.rbtnFTPPASV.Location = new System.Drawing.Point(157, 132);
            this.rbtnFTPPASV.Name = "rbtnFTPPASV";
            this.rbtnFTPPASV.Size = new System.Drawing.Size(47, 16);
            this.rbtnFTPPASV.TabIndex = 2;
            this.rbtnFTPPASV.TabStop = true;
            this.rbtnFTPPASV.Text = "被动";
            this.rbtnFTPPASV.UseVisualStyleBackColor = true;
            // 
            // rbtnFTPPASS
            // 
            this.rbtnFTPPASS.AutoSize = true;
            this.rbtnFTPPASS.Location = new System.Drawing.Point(104, 132);
            this.rbtnFTPPASS.Name = "rbtnFTPPASS";
            this.rbtnFTPPASS.Size = new System.Drawing.Size(47, 16);
            this.rbtnFTPPASS.TabIndex = 2;
            this.rbtnFTPPASS.Text = "主动";
            this.rbtnFTPPASS.UseVisualStyleBackColor = true;
            // 
            // edtFtpPassword
            // 
            this.edtFtpPassword.Location = new System.Drawing.Point(102, 98);
            this.edtFtpPassword.Name = "edtFtpPassword";
            this.edtFtpPassword.Size = new System.Drawing.Size(100, 21);
            this.edtFtpPassword.TabIndex = 1;
            // 
            // edtFtpUser
            // 
            this.edtFtpUser.Location = new System.Drawing.Point(102, 71);
            this.edtFtpUser.Name = "edtFtpUser";
            this.edtFtpUser.Size = new System.Drawing.Size(100, 21);
            this.edtFtpUser.TabIndex = 1;
            // 
            // edtFTPPort
            // 
            this.edtFTPPort.Location = new System.Drawing.Point(102, 44);
            this.edtFTPPort.Name = "edtFTPPort";
            this.edtFTPPort.Size = new System.Drawing.Size(100, 21);
            this.edtFTPPort.TabIndex = 1;
            // 
            // edtFTPHost
            // 
            this.edtFTPHost.Location = new System.Drawing.Point(102, 17);
            this.edtFTPHost.Name = "edtFTPHost";
            this.edtFTPHost.Size = new System.Drawing.Size(100, 21);
            this.edtFTPHost.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "SSL加密：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "断点续传：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "传输模式：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务器端口：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器地址：";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edtCurlUploadedSize);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.btnSelCurFile);
            this.tabPage1.Controls.Add(this.edtCurlLocalFile);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.btnCurlUp);
            this.tabPage1.Controls.Add(this.btnCurlResume);
            this.tabPage1.Controls.Add(this.btnCurlPause);
            this.tabPage1.Controls.Add(this.btnCurlStop);
            this.tabPage1.Controls.Add(this.lblCURLInfo);
            this.tabPage1.Controls.Add(this.btnCurlDown);
            this.tabPage1.Controls.Add(this.edtCurlURL);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(965, 447);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Curl测试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSelCurFile
            // 
            this.btnSelCurFile.Location = new System.Drawing.Point(809, 71);
            this.btnSelCurFile.Name = "btnSelCurFile";
            this.btnSelCurFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelCurFile.TabIndex = 13;
            this.btnSelCurFile.Text = "选择文件";
            this.btnSelCurFile.UseVisualStyleBackColor = true;
            this.btnSelCurFile.Click += new System.EventHandler(this.btnSelCurFile_Click);
            // 
            // edtCurlLocalFile
            // 
            this.edtCurlLocalFile.Location = new System.Drawing.Point(85, 73);
            this.edtCurlLocalFile.Name = "edtCurlLocalFile";
            this.edtCurlLocalFile.Size = new System.Drawing.Size(718, 21);
            this.edtCurlLocalFile.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(47, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 11;
            this.label9.Text = "文件：";
            // 
            // btnCurlUp
            // 
            this.btnCurlUp.Location = new System.Drawing.Point(174, 166);
            this.btnCurlUp.Name = "btnCurlUp";
            this.btnCurlUp.Size = new System.Drawing.Size(75, 23);
            this.btnCurlUp.TabIndex = 10;
            this.btnCurlUp.Text = "上传";
            this.btnCurlUp.UseVisualStyleBackColor = true;
            this.btnCurlUp.Click += new System.EventHandler(this.btnCurlUp_Click);
            // 
            // btnCurlResume
            // 
            this.btnCurlResume.Location = new System.Drawing.Point(342, 166);
            this.btnCurlResume.Name = "btnCurlResume";
            this.btnCurlResume.Size = new System.Drawing.Size(75, 23);
            this.btnCurlResume.TabIndex = 9;
            this.btnCurlResume.Text = "继续";
            this.btnCurlResume.UseVisualStyleBackColor = true;
            this.btnCurlResume.Click += new System.EventHandler(this.btnCurlResume_Click);
            // 
            // btnCurlPause
            // 
            this.btnCurlPause.Location = new System.Drawing.Point(261, 166);
            this.btnCurlPause.Name = "btnCurlPause";
            this.btnCurlPause.Size = new System.Drawing.Size(75, 23);
            this.btnCurlPause.TabIndex = 8;
            this.btnCurlPause.Text = "暂停";
            this.btnCurlPause.UseVisualStyleBackColor = true;
            this.btnCurlPause.Click += new System.EventHandler(this.btnCurlPause_Click);
            // 
            // btnCurlStop
            // 
            this.btnCurlStop.Location = new System.Drawing.Point(435, 166);
            this.btnCurlStop.Name = "btnCurlStop";
            this.btnCurlStop.Size = new System.Drawing.Size(75, 23);
            this.btnCurlStop.TabIndex = 7;
            this.btnCurlStop.Text = "停止";
            this.btnCurlStop.UseVisualStyleBackColor = true;
            this.btnCurlStop.Click += new System.EventHandler(this.btnCurlStop_Click);
            // 
            // lblCURLInfo
            // 
            this.lblCURLInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCURLInfo.Location = new System.Drawing.Point(3, 419);
            this.lblCURLInfo.Name = "lblCURLInfo";
            this.lblCURLInfo.Size = new System.Drawing.Size(959, 25);
            this.lblCURLInfo.TabIndex = 6;
            this.lblCURLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCurlDown
            // 
            this.btnCurlDown.Location = new System.Drawing.Point(93, 166);
            this.btnCurlDown.Name = "btnCurlDown";
            this.btnCurlDown.Size = new System.Drawing.Size(75, 23);
            this.btnCurlDown.TabIndex = 2;
            this.btnCurlDown.Text = "下载";
            this.btnCurlDown.UseVisualStyleBackColor = true;
            this.btnCurlDown.Click += new System.EventHandler(this.btnCurlDown_Click);
            // 
            // edtCurlURL
            // 
            this.edtCurlURL.Location = new System.Drawing.Point(85, 35);
            this.edtCurlURL.Name = "edtCurlURL";
            this.edtCurlURL.Size = new System.Drawing.Size(718, 21);
            this.edtCurlURL.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(53, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "URL：";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.OverwritePrompt = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.BackAlpha = 0;
            this.textBoxEx1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.textBoxEx1.BorderColor = System.Drawing.Color.Red;
            this.textBoxEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEx1.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxEx1.ForeColor = System.Drawing.Color.White;
            this.textBoxEx1.HotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBoxEx1.Location = new System.Drawing.Point(18, 48);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEx1.Size = new System.Drawing.Size(239, 29);
            this.textBoxEx1.TabIndex = 3;
            this.textBoxEx1.Text = "123地方";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(77, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "上传起始位置（字节）：";
            // 
            // edtCurlUploadedSize
            // 
            this.edtCurlUploadedSize.Location = new System.Drawing.Point(212, 116);
            this.edtCurlUploadedSize.Name = "edtCurlUploadedSize";
            this.edtCurlUploadedSize.Size = new System.Drawing.Size(100, 21);
            this.edtCurlUploadedSize.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 477);
            this.Controls.Add(this.tcDemo);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pde.net.demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tcDemo.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tpFTPUtil.ResumeLayout(false);
            this.pnlFTP.ResumeLayout(false);
            this.pnlFTP.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDemo;
        private System.Windows.Forms.TabPage tpFTPUtil;
        private System.Windows.Forms.Panel pnlFTP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edtFTPHost;
        private System.Windows.Forms.TextBox edtFTPPort;
        private System.Windows.Forms.TextBox edtFtpUser;
        private System.Windows.Forms.TextBox edtFtpPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbtnFTPPASS;
        private System.Windows.Forms.RadioButton rbtnFTPPASV;
        private System.Windows.Forms.Button btnFTPGetList;
        private System.Windows.Forms.TreeView tvFTP;
        private System.Windows.Forms.ListView lvFTP;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ColumnHeader fileName;
        private System.Windows.Forms.Button btnFTPDown;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblFTPInfo;
        private System.Windows.Forms.Button btnFTPSTOP;
        private System.Windows.Forms.Button btnFTPPause;
        private System.Windows.Forms.Button btnFTPUpload;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFTPDeleteFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbxFTPSSL;
        private System.Windows.Forms.CheckBox cbxFTPUseBroken;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFTPDeleteDir;
        private System.Windows.Forms.ColumnHeader fileSize;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox edtCurlURL;
        private System.Windows.Forms.Button btnCurlDown;
        private System.Windows.Forms.Label lblCURLInfo;
        private System.Windows.Forms.Button btnCurlPause;
        private System.Windows.Forms.Button btnCurlStop;
        private System.Windows.Forms.Button btnCurlResume;
        private System.Windows.Forms.Button btnCurlUp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox edtCurlLocalFile;
        private System.Windows.Forms.Button btnSelCurFile;
        private System.Windows.Forms.TabPage tabPage2;
        private control.TextBoxEx textBoxEx1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox edtCurlUploadedSize;
    }
}

