using pde.pub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pde.net.demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getSet(tcDemo);
            int index = 0;
            int.TryParse(WRSetting.Set().getSettings("ActivePage", "0"), out index);
            tcDemo.SelectTab(index); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            setSet(tcDemo);
            WRSetting.Set().setSettings("ActivePage", tcDemo.SelectedIndex.ToString());
        }

        #region 设置读写
        private void getSet(Control control)
        { 
            foreach (Control ctl in control.Controls)
            {
                if (ctl is TextBox)
                {
                    (ctl as TextBox).Text = WRSetting.Set().getSettings(ctl.Name);
                }
                if (ctl is RadioButton)
                {
                    (ctl as RadioButton).Checked = WRSetting.Set().getSettings(ctl.Name).Equals("true");
                }
                if (ctl is CheckBox)
                {
                    (ctl as CheckBox).Checked = WRSetting.Set().getSettings(ctl.Name).Equals("true");
                }
                getSet(ctl);
            }
        }

        private void setSet(Control control)
        {
            foreach (Control ctl in control.Controls)
            {
                if (ctl is TextBox)
                {
                    WRSetting.Set().setSettings(ctl.Name, (ctl as TextBox).Text);
                }
                if (ctl is RadioButton)
                {
                    WRSetting.Set().setSettings(ctl.Name, (ctl as RadioButton).Checked ? "true" : "false");
                }
                if (ctl is CheckBox)
                {
                    WRSetting.Set().setSettings(ctl.Name, (ctl as CheckBox).Checked ? "true" : "false");
                }
                setSet(ctl);
            }
        } 
        #endregion

        #region FTP相关测试
        private FTPUtil initFTP()
        {
            string host = edtFTPHost.Text;
            int port = 21;
            int.TryParse(edtFTPPort.Text, out port);
            string user = edtFtpUser.Text;
            string psd = edtFtpPassword.Text;
            FTPUtil ftp = new FTPUtil(host, port, user, psd, "", rbtnFTPPASV.Checked, cbxFTPSSL.Checked);
            return ftp;
        }

        private void btnFTPGetList_Click(object sender, EventArgs e)
        {
            try
            {
                FTPUtil ftp = initFTP();
                List<string> lst = ftp.GetDirectoryList("");
                tvFTP.Nodes.Clear();
                foreach (string dir in lst)
                {
                    tvFTP.Nodes.Add(dir);
                }

                lst = ftp.GetFileList("");
                lvFTP.Items.Clear();
                foreach (string file in lst)
                {
                    lvFTP.Items.Add(file);
                }
                ftp = null;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string getDir(TreeNode node)
        {
            string dir = "";
            if (node == null) return dir;
            dir = node.Text;
            while (node.Parent != null)
            {
                dir = node.Parent.Text + @"/" + dir;
                node = node.Parent;
            }
            return dir;
        }

        private void tvFTP_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string folder = getDir(tvFTP.SelectedNode);
                if (folder.Equals("")) return;

                FTPUtil ftp = initFTP();
                List<string> lst = ftp.GetDirectoryList(folder);
                tvFTP.SelectedNode.Nodes.Clear();
                foreach (string dir in lst)
                {
                    tvFTP.SelectedNode.Nodes.Add(dir);
                }
                tvFTP.SelectedNode.ExpandAll();
               /* lst = ftp.GetFileList(folder);
                lvFTP.Items.Clear();
                foreach (string file in lst)
                {
                    lvFTP.Items.Add(file);
                }
                */
               ftp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvFTP_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string folder = getDir(tvFTP.SelectedNode); 
                FTPUtil ftp = initFTP();
                List<string> lst = ftp.GetFileList(folder);
                lvFTP.Items.Clear();
                foreach (string file in lst)
                {
                   ListViewItem item = lvFTP.Items.Add(file);
                    string size = StringUtil.formatBytes(ftp.GetFileSize(folder + "/" + file));
                    item.SubItems.Add(ftp.GetFileSize(folder + "/" + file).ToString() + " (" + size + ")");
                }
                ftp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //下载文件
        FTPUtil ftp;
        private void btnFTPDown_Click(object sender, EventArgs e)
        {
            if (lvFTP.SelectedItems.Count == 0) return;
            string ftpfile = getDir(tvFTP.SelectedNode); 
            ftpfile += "/" + lvFTP.SelectedItems[0].Text;
            saveFileDialog1.FileName = lvFTP.SelectedItems[0].Text; 
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            string file = saveFileDialog1.FileName;
            try
            {
                ftpMethod = "下载";
                ftp = initFTP();
                bool ok = ftp.DownloadFile(ftpfile, file,brokenOpen:cbxFTPUseBroken.Checked, updateProgress: setFtpInfo);
                ftp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string ftpMethod = "";
        private void setFtpInfo(object obj, long totle, long complate, double speed)
        {
            string t = StringUtil.formatBytes(totle);
            string c = StringUtil.formatBytes(complate);
            string s = StringUtil.formatBytes(speed);
            lblFTPInfo.Text = string.Format("总大小：{0}  已{1}：{2}  {1}速度：{3}/s", t, ftpMethod, c, s);
            Application.DoEvents();
        }

        private void btnFTPPause_Click(object sender, EventArgs e)
        {
            if (ftp == null) return;
            ftp.Pause();
        }

        private void btnFTPSTOP_Click(object sender, EventArgs e)
        {
            if (ftp == null) return;
            ftp.Stop();
        }

        //上传文件
        private void btnFTPUpload_Click(object sender, EventArgs e)
        { 
            string ftpfile = getDir(tvFTP.SelectedNode); 
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            string file = openFileDialog1.FileName;
            FileInfo info = new FileInfo(file);
            ftpfile += "/" + info.Name;

            try
            {
                ftpMethod = "上传";
                ftp = initFTP();
                bool ok = ftp.UploadFile(file, ftpfile, brokenOpen: cbxFTPUseBroken.Checked, updateProgress: setFtpInfo);
                ftp = null;
                if (ok)
                {
                    tvFTP_AfterSelect(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //删除文件
        private void btnFTPDeleteFile_Click(object sender, EventArgs e)
        {
            if (lvFTP.SelectedItems.Count == 0) return;
            string ftpfile = getDir(tvFTP.SelectedNode); 
            ftpfile += "/" + lvFTP.SelectedItems[0].Text;

            try
            { 
                ftp = initFTP();
                bool ok = ftp.DeletFile(ftpfile);
                ftp = null;
                if (ok)
                {
                    tvFTP_AfterSelect(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //删除文件夹
        private void btnFTPDeleteDir_Click(object sender, EventArgs e)
        { 
            string ftpfile = getDir(tvFTP.SelectedNode); 
            try
            {
                ftp = initFTP();
                ftp.DelDir(ftpfile);
                ftp = null;
                tvFTP.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Curl 相关测试
        private void btnSelCurFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = edtCurlLocalFile.Text;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            edtCurlLocalFile.Text = openFileDialog1.FileName;
        }

        CurlUtil util = null;
        string httpMethod = "";
        //下载
        private void btnCurlDown_Click(object sender, EventArgs e)
        {
            if (util != null) return; 
            httpMethod = "下载";
            util = new CurlUtil();
            CurlUtil.InitCurl();
            util.DownloadFile(edtCurlURL.Text, edtCurlLocalFile.Text, onProgress);
            CurlUtil.FreeCurl();
            util = null;
            lblCURLInfo.Text = "下载完成！";
        }

        //上传
        long CurlUploadedSize = 0;
        private void btnCurlUp_Click(object sender, EventArgs e)
        {
            if (util != null) return; 
            httpMethod = "上传";
            try
            {
                util = new CurlUtil();
                CurlUtil.InitCurl();
                if (!long.TryParse(edtCurlUploadedSize.Text, out CurlUploadedSize))
                {
                    CurlUploadedSize = 0;
                }
                util.UploadFile(edtCurlURL.Text, edtCurlLocalFile.Text, CurlUploadedSize, onProgress); 
                edtCurlUploadedSize.Text = util.UpLoadedSize.ToString();
                CurlUtil.FreeCurl();
                lblCURLInfo.Text = "上传完成！";
            }
            catch(Exception ex)
            {
                lblCURLInfo.Text = "上传失败：" + ex.Message;
            }
            finally
            {
                util = null;
            }
        }


        private void onProgress(object obj, long total, long complete, double speed)
        {
            try
            { 
                lblCURLInfo.Text = string.Format("正在{0}：{1}/{2}    {0}速度：{3}/s", httpMethod, StringUtil.formatBytes(complete), StringUtil.formatBytes(total), StringUtil.formatBytes(speed));
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        //暂停
        private void btnCurlPause_Click(object sender, EventArgs e)
        {
            if (util != null)
            {
                util.Pause();
            }
        }

        //继续
        private void btnCurlResume_Click(object sender, EventArgs e)
        {
            if (util != null)
            {
                util.Resume();
            }
        }

        //停止
        private void btnCurlStop_Click(object sender, EventArgs e)
        {
            if (util != null)
            {
                util.Stop(); 
                lblCURLInfo.Text = "";
            }
        }



        #endregion

        
    }
}
