using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using pde.log;

namespace pde.pub
{
    public class FTPUtil
    { 
        private static int BLOCK_SIZE = 64 * 1024;    // 缓冲大小 
        private byte[] buffer = new byte[BLOCK_SIZE]; 
        private FtpWebRequest ftpReq;
        private int state = 0;                 //上传下载状态  0 正常   1 暂停  2 停止

        #region 属性 
        public bool MakeDirIfNotExist { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string RootPath { get; set; }
        public bool UsePasv { get; set; }
        public bool UseSSL { get; set; }
        public bool KeepAlive { get; set; }
        public bool UseBinary { get; set; }
        public int TimeOut { get; set; }
        public long TotalSize { get; set; }
        public long CompleteSize { get; set; }
        public double Speed { get; set; }
        public object Owner { get; set; }
        #endregion

        #region 构造方法
        public FTPUtil(string host, int port, string user, string password, string rootPath = "", bool PasvMode = true, bool EnableSSL = false, bool keepAlive = true, bool useBinary = true, int timeOut = 0)
        {
            Host = host;
            Port = port;
            UserName = user;
            UserPassword = password;
            RootPath = rootPath;
            UsePasv = PasvMode;                            //主被动模式，默认被动模式
            UseSSL = UseSSL;                               //使用SSL加密传输
            KeepAlive = keepAlive;                         // 默认为true，连接不会被关闭 
            UseBinary = useBinary;                         // 指定数据传输类型是否为二进制 
            TimeOut = timeOut > 0 ? timeOut : 600000;      //FTP超时设置
            MakeDirIfNotExist = true;
        }
        #endregion

        #region 初始化连接
        private void init(string ftpFile)
        {
            string uri = string.Format(@"ftp://{0}:{1}/", Host, Port);

            if (RootPath.Equals(""))
            {
                uri += ftpFile;
            }
            else
            {
                uri += RootPath + "/" + ftpFile;
            }
            
            // 根据uri创建FtpWebRequest对象 
            ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            ftpReq.Credentials = new NetworkCredential(UserName, UserPassword); // ftp用户名和密码 
            ftpReq.KeepAlive = KeepAlive;
            ftpReq.UsePassive = UsePasv;
            ftpReq.EnableSsl = UseSSL;
            ftpReq.UseBinary = UseBinary;
            ftpReq.Timeout = TimeOut;
            ftpReq.ReadWriteTimeout = TimeOut;
        }

        private void AutoCreateDir(string ftpFile)
        {
            if (!MakeDirIfNotExist) return;
            string folderWhole = RootPath.Equals("") ? ftpFile : RootPath + "/" + ftpFile;
            string[] dirs = folderWhole.Replace("//", "/").Split('/');
            string folder = "";
            for (int i = 0; i<dirs.Length - 1; i++)
            {
                string dir = dirs[i];
                if (dir.Equals("")) continue;
                List<string> lst = GetDirectoryList(folder);
                if (!lst.Contains(dir))
                {
                    MakeDir(folder + "/" + dir);
                }
                folder += "/" + dir;
            }
        }
        #endregion

        #region 获取当前目录下明细(包含文件和文件夹) 
        /// <summary>
        ///  获取当前目录下明细(包含文件和文件夹) 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<string> GetFilesDetailList(string folder)
        {
            try
            {
                init(folder);
                List<string> list = new List<string>();
                ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (FtpWebResponse res = (FtpWebResponse)ftpReq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                string info = "获取文件夹列表明细失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 获取当前目录下所有的文件夹列表(仅文件夹) 
        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹) 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<string> GetDirectoryList(string folder)
        {
            try
            {
                List<string> detail = GetFilesDetailList(folder);
                List<string> drectory = new List<string>();
                foreach (string str in detail)
                {
                    if (str.Trim().Substring(0, 1).ToUpper() == "D")
                    {
                        string[] tmp = str.Split(' ');
                        string strx = tmp[tmp.Length - 1];
                        drectory.Add(strx);
                    }
                }
                return drectory;
            }
            catch (Exception ex)
            {
                string info = ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region  获取当前目录下文件列表(仅文件) 
        /// <summary>
        /// 获取当前目录下文件列表(仅文件) 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<string> GetFileList(string folder)
        {
            try
            {
                List<string> detail = GetFilesDetailList(folder);
                List<string> drectory = new List<string>();
                foreach (string str in detail)
                {
                    if (str.Trim().Substring(0, 1).ToUpper() != "D")
                    {
                        string[] tmp = str.Split(' ');
                        string strx = tmp[tmp.Length - 1];
                        drectory.Add(strx);
                    }
                }
                return drectory;
            }
            catch (Exception ex)
            {
                string info = "获取文件列表失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 获取文件大小 
        /// <summary>
        /// 获取文件大小 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public long GetFileSize(string filename)
        {
            try
            {
                long filesize = 0;
                init(filename);
                ftpReq.Method = WebRequestMethods.Ftp.GetFileSize;
                using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse())
                {
                    filesize = response.ContentLength;
                }
                return filesize;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion

        #region ftp上文件改名
        /// <summary>
        /// ftp上文件改名
        /// </summary>
        /// <param name="oldFilename"></param>
        /// <param name="newFilename"></param>
        public void Rename(string oldFilename, string newFilename)
        {
            try
            {
                init(oldFilename);
                ftpReq.Method = WebRequestMethods.Ftp.Rename;
                ftpReq.RenameTo = newFilename;
                using(FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse())
                {

                } 
            }
            catch (Exception ex)
            {
                string info = "修改文件名失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 删除FTP文件 
        /// <summary>
        /// 删除FTP文件 
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public bool DeletFile(string remoteFile)
        {
            try
            {
                init(remoteFile);
                ftpReq.Method = WebRequestMethods.Ftp.DeleteFile;
                using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse()) { }
                return true;
            }
            catch (Exception ex)
            {
                string info = "删除文件失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 删除FTP上目录
        /// <summary>
        /// 删除FTP上目录
        /// </summary>
        /// <param name="dirName"></param>
        public void DelDir(string dirName)
        {
            try
            {
                init(dirName);
                ftpReq.Method = WebRequestMethods.Ftp.RemoveDirectory;
                using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse()) { } 
            }
            catch (Exception ex)
            {
                string info = "删除文件目录失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 在FTP上创建目录
        /// <summary>
        /// 在FTP上创建目录
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            try
            {
                init(dirName);
                ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;
                using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse()) { } 
            }
            catch (Exception ex)
            {
                string info = "创建目录失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
        }
        #endregion

        #region 上传文件到FTP服务器 
        /// <summary>
        /// 上传文件到FTP服务器(断点续传)
        /// </summary>
        /// <param name="localFile">本地文件全路径名称</param>
        /// <param name="remoteFile">远程文件</param>
        /// <param name="brokenOpen ">是否支持断点续传，默认不使用断点续传</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：拥有者对象，第二个参数：总大小，第三个参数：当前进度，第四个参数：下载速度 /秒)</param>
        /// <returns></returns>       
        public bool UploadFile(string localFile, string remoteFile, bool brokenOpen = false, Action<object, long, long, double> updateProgress = null)
        {
            try
            {
                state = 0;
                AutoCreateDir(remoteFile);
                FileInfo fileInf = new FileInfo(localFile);
                long fileSize = fileInf.Length;
                long remoteFileSize = GetFileSize(remoteFile);
                if (remoteFileSize < 0)
                {
                    remoteFileSize = 0;   //文件不存在
                }
                long startByte = 0;
                if (brokenOpen)
                {
                    if (remoteFileSize > fileSize)
                    {
                        //服务器上文件比本地还要大，说明出异常了
                        throw new Exception("服务器文件大小出现异常，请检查后重新上传！");
                    }
                    startByte = remoteFileSize;
                }
                else
                {
                    if (remoteFileSize > 0)
                    {
                        DeletFile(remoteFile);
                    }
                    remoteFileSize = 0;
                }
                init(remoteFile);
                ftpReq.Method = brokenOpen ? WebRequestMethods.Ftp.AppendFile : WebRequestMethods.Ftp.UploadFile;
                ftpReq.ContentLength = fileInf.Length;

                using (FileStream fs = fileInf.OpenRead())
                {
                    using (Stream strm = ftpReq.GetRequestStream())
                    {
                        DateTime dtBegin = DateTime.Now;
                        fs.Seek(startByte, 0);
                        int contentLen = fs.Read(buffer, 0, BLOCK_SIZE);

                        TotalSize = fileSize;
                        CompleteSize = startByte;

                        if (contentLen == 0)
                        {
                            updateProgress?.Invoke(Owner, fileSize, startByte, 0);//更新进度条  
                        }
                        // 流内容没有结束 
                        while (contentLen != 0)
                        {
                            if (state > 0) break;
                            // 把内容从file stream 写入 upload stream 
                            startByte += contentLen;
                            strm.Write(buffer, 0, contentLen);
                            contentLen = fs.Read(buffer, 0, BLOCK_SIZE);
                            TimeSpan ts = DateTime.Now.Subtract(dtBegin);
                            double speed = 0;
                            if (ts.Seconds > 0)
                            {
                                speed = (startByte - remoteFileSize) / ts.Seconds;
                            }
                            Speed = speed;
                            updateProgress?.Invoke(Owner, fileSize, startByte, speed);//更新进度条  
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string info = "上传文件失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
            finally
            {
                if (state == 2)
                {
                    DeletFile(remoteFile);
                }
            }
        }
        #endregion

        #region 从FTP服务器下载文件
        /// <summary>
        /// 从FTP服务器下载文件，指定本地路径和本地文件名
        /// </summary>
        /// <param name="remoteFile">远程文件名</param>
        /// <param name="localFile">保存本地的文件名（包含路径）</param> 
        /// <param name="brokenOpen">是否断点下载</param>
        /// <param name="overWriteIfExist">如果本地文件存在是否覆盖（只在非断点下载情况下有效）</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：拥有者对象，第二个参数：总大小，第三个参数：当前进度，第四个参数：下载速度 /秒)</param>
        /// <returns>是否下载成功</returns>
        public bool DownloadFile(string remoteFile, string localFile, bool brokenOpen = false, bool overWriteIfExist = true, Action<object, long, long, double> updateProgress = null)
        {
            FileInfo file = new FileInfo(localFile);
            try
            {
                state = 0;
                long size = 0;
                if (brokenOpen)
                {
                    if (file.Exists)
                    {
                        size = file.Length;
                    }
                }
                else
                {
                    if (file.Exists)
                    {
                        if (overWriteIfExist)
                        {
                            file.Delete();
                        }
                        else
                        {
                            throw new Exception("本地文件已存在：" + localFile);
                        }
                    }
                }
                long remoteFileSize = GetFileSize(remoteFile);  //获取服务器上文件大小
                if (remoteFileSize < 0)
                {
                    remoteFileSize = 0;   //文件不存在
                }
                init(remoteFile);
                ftpReq.ContentOffset = size;
                ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
                using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        DateTime dtBegin = DateTime.Now;
                        long totalDownloadedByte = size; 
                        int readCount; 
                        readCount = ftpStream.Read(buffer, 0, BLOCK_SIZE);
                        using (FileStream outputStream = new FileStream(localFile, FileMode.Append))
                            while (readCount > 0)
                            {
                                if (state > 0) break;
                                totalDownloadedByte += readCount;
                                outputStream.Write(buffer, 0, readCount);
                                TimeSpan ts = DateTime.Now.Subtract(dtBegin); 
                                readCount = ftpStream.Read(buffer, 0, BLOCK_SIZE);
                                double speed = 0;
                                if (ts.Seconds>0)
                                {
                                    speed = (totalDownloadedByte - size) / ts.Seconds;
                                }
                                TotalSize = remoteFileSize;
                                CompleteSize = totalDownloadedByte;
                                Speed = speed;
                                updateProgress?.Invoke(Owner, remoteFileSize, totalDownloadedByte, speed);//更新进度条   
                            }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string info = "下载文件失败：" + ex.Message;
                LogLocal.log().SaveLog(new LogEntity(info, LogType.Plat, LogLevel.ERROR));
                throw new Exception(info);
            }
            finally
            {
                if (state == 2)
                {
                    file.Delete();
                }
            }
        }
        #endregion

        #region 暂停上传/下载
        /// <summary>
        /// 暂停上传/下载
        /// </summary>
        public void Pause()
        {
            state = 1;
        }
        #endregion

        #region 停止上传/下载
        /// <summary>
        /// 停止上传/下载
        /// </summary>
        public void Stop()
        {
            state = 2;
        }
        #endregion
    }
}