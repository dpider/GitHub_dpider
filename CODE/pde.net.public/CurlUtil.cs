using CurlSharp;
using pde.log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace pde.pub
{
    public class CurlUtil
    {
        //第一个参数：拥有者对象，第二个参数：总大小，第三个参数：当前进度，第四个参数：下载速度 /秒
        public delegate void OnUpdateProgress(object owner, long totalSize, long complete, double speed);
        private OnUpdateProgress onUpProgress;
        private int DownOrUp = 0;           //1：下载   2：上传
        private long StartDownPosion = 0;         //已下载大小
        private DateTime BeginTime;         //计时开始 
        private string downFileName = "";
        private int state = 0;                 //上传下载状态  0 正常   1 暂停  2 停止
        private FileStream DownFileStream;
        private MemoryStream UploadStream; 
        private CurlEasy easy = null;
        private static int BLOCK_SIZE = 64*1024;    // 缓冲大小

        #region 属性
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int TimeOut { get; set; }
        public bool BrokenOpen { get; set; }    //是否支持断点续传
        public long TotalSize { get; set; }
        public long CompleteSize { get; set; }
        public double Speed { get; set; }
        public object Owner { get; set; }
         
        public long UpLoadedSize { get; set; }//已上传文件大小，断点续传用
        #endregion


        #region 构造方法
        public CurlUtil(object owner = null, string userName = "", string userPassword = "", bool brokenOpen = true, int timeOut = 0)
        {
            Owner = owner;
            UserName = userName;
            UserPassword = userPassword; 
            TimeOut = timeOut > 0 ? timeOut : 600000;      //超时设置
            BrokenOpen = brokenOpen;
            UpLoadedSize = 0;
        }
        #endregion

        #region 必须* 需要在主线程中调用的函数，否则多线程会异常
        public static void InitCurl()
        {
            Curl.GlobalInit(CurlInitFlag.All);
        }

        public static void FreeCurl()
        {
            Curl.GlobalCleanup();
        }
        #endregion

        #region
        private void initCurl(string url)
        {
            easy = new CurlEasy();
            easy.Url = url;
            easy.Timeout = TimeOut;
            easy.ConnectTimeout = TimeOut;
            easy.BufferSize = BLOCK_SIZE;
            if ((!UserName.Equals("")) && (!UserPassword.Equals("")))
            {
                easy.SetOpt(CurlOption.UserPwd, UserName + ":" + UserPassword);
            }

            // easy.fun
            easy.DebugFunction = OnDebug;
            easy.ProgressFunction = onProgress;

            easy.UserAgent = "curl";
            easy.Verbose = true; 

            // HTTP/2 please
          //  easy.HttpVersion = CurlHttpVersion.Http2_0;

            // skip SSL verification during debugging
            easy.SslVerifyPeer = false;
            easy.SslVerifyhost = false;

            easy.SetOpt(CurlOption.HttpHeader, "Content-Type: application/octet-stream");
            easy.SetOpt(CurlOption.HttpHeader, "Expect:");
             
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="aURL">url文件地址</param>
        /// <param name="outFile">下载文件地址</param>
        /// <param name="updateProgress">下载进度过程</param>
        public bool DownloadFile(string aURL, string outFile, OnUpdateProgress updateProgress = null)
        {
            try
            {
                DownOrUp = 1;  //下载标识
                if ((!BrokenOpen) && (File.Exists(outFile)))
                {
                    File.Delete(outFile);
                }
                downFileName = outFile;
                
                DownFileStream = new FileStream(outFile, FileMode.OpenOrCreate);
                StartDownPosion = DownFileStream.Length;
                DownFileStream.Seek(DownFileStream.Length, SeekOrigin.Current);

                onUpProgress = updateProgress;
                BeginTime = DateTime.Now;
                initCurl(aURL);
                easy.WriteFunction = OnWriteData;
                easy.ResumeFrom = DownFileStream.Length;
                easy.Perform();
                if (easy.ResponseCode == 200)
                {
                    return true;
                }
                else
                {
                    throw new Exception(string.Format("HTTP返回：{0}", easy.ResponseCode));
                }
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity("文件下载失败：" + ex.Message, LogType.Plat, LogLevel.ERROR));
                throw ex;
            }
            finally
            {
                easy = null;
                DownFileStream.Flush();
                DownFileStream.Close();
                DownFileStream.Dispose();
                DownOrUp = 0;
            }
        }
        #endregion
        
        #region 写入数据 (下载数据）
        private int OnWriteData(byte[] buf, int size, int nmemb, object extraData)
        {
            try
            {
                DownFileStream.Write(buf, 0, buf.Length);

                /*
                using (FileStream FStream = new FileStream(downFileName, FileMode.OpenOrCreate))
                {
                   // {
                        SPosition = FStream.Length;     //获取已经下载的长度
                   // }
                    FStream.Seek(SPosition, SeekOrigin.Current);
                    DownedLen += buf.Length;
                    if (SPosition <= DownedLen)
                    {
                        FStream.Write(buf, 0, buf.Length);
                    }
                }*/
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("OnReadData发生错误：{1}", ex.Message), LogType.Plat, LogLevel.ERROR));
                throw ex;
            }
            return size * nmemb;
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// HTTP上传文件 
        /// </summary>
        /// <param name="aURL">上传URL地址</param>
        /// <param name="upFile">待上传文件</param>
        /// <param name="uploadedSize">已上传文件大小（字节）</param>
        /// <param name="updateProgress">下载进度过程</param>
        public bool UploadFile(string aURL, string upFile, long uploadedSize = 0, OnUpdateProgress updateProgress = null)
        {  
            try
            {
                DownOrUp = 2;  //上传标识
                initCurl(aURL);
                UpLoadedSize = uploadedSize;
                easy.ReadFunction = OnReadData;
                byte[] UploadData = File.ReadAllBytes(upFile);
                UploadStream = new MemoryStream(UploadData);
                UploadStream.Seek(UpLoadedSize, SeekOrigin.Current);
                easy.ReadData = UploadData;
                onUpProgress = updateProgress;
                BeginTime = DateTime.Now;
                easy.Upload = true;
                easy.InfileSize = UploadData.Length;
                easy.Post = true;
                easy.Perform();
                if (easy.ResponseCode == 200)
                {
                    return true;
                }
                else
                {
                    throw new Exception(string.Format("HTTP返回：{0}", easy.ResponseCode));
                } 
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("文件上传URL：\"{0}\"  上传失败：{1}", aURL, ex.Message), LogType.Plat, LogLevel.ERROR));
                throw ex;
            }
            finally
            {
                easy = null;
                UploadStream.Flush();
                UploadStream.Close();
                UploadStream.Dispose();
                DownOrUp = 0;
            }
        }
        #endregion

        #region 读取数据（上传数据）
        private int OnReadData(byte[] buf, int size, int nmemb, object extraData)
        { 
            long SPosition = 0;
            try
            {
              //  UploadStream.Seek(UpLoadedSize, SeekOrigin.Current);
                UpLoadedSize += buf.Length;
                return UploadStream.Read(buf, 0, size * nmemb);

                /*
                byte[] data = (byte[])extraData;

                using (MemoryStream FStream = new MemoryStream(data))
                {
                    if (BrokenOpen) //支持断点续传
                    {
                        SPosition = upLoadedSize;     //已经上传的字节数
                    }
                    FStream.Seek(SPosition, SeekOrigin.Current);
                    return FStream.Read(buf, 0, size * nmemb); 
                } */
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("OnReadData：上传起始字节：{0}，发生错误：{1}", SPosition, ex.Message), LogType.Plat, LogLevel.ERROR));
                throw ex;
            } 
        }
        #endregion

        #region 调试模式方法
        private void OnDebug(CurlInfoType infoType, string msg, int size, object extraData)
        {
            LogLocal.log().SaveLog(new LogEntity("CURL-DEBUG：" + msg, LogType.Plat, LogLevel.DEBUG));
        }
        #endregion

        #region 上传下载过程方法
        private int onProgress(object extraData, double dlTotal, double dlNow, double ulTotal, double ulNow)
        {
            try
            { 
                double speed = 0;
                TimeSpan ts = DateTime.Now.Subtract(BeginTime);
                 
                if ((DownOrUp == 1) && (state == 0))     //下载
                {
                    if (ts.Seconds > 0)
                    {
                        speed = dlNow / ts.Seconds;
                    }
                    Speed = speed;
                    TotalSize = Convert.ToInt64(dlTotal + StartDownPosion);
                    CompleteSize = Convert.ToInt64(dlNow + StartDownPosion);
                    if ((dlNow + dlTotal) > 0)
                    {
                        onUpProgress(Owner, TotalSize, CompleteSize, speed);
                    } 
                }
                else if ((DownOrUp == 2) && (state == 0)) //上传
                {
                    if (ts.Seconds > 0)
                    {
                        speed = ulNow / ts.Seconds;
                    }
                    Speed = speed;
                    TotalSize = UploadStream.Length; // Convert.ToInt64(ulTotal);
                    CompleteSize = Convert.ToInt64(ulNow);

                    if ((ulNow + ulTotal) > 0)
                    {
                        onUpProgress(Owner, TotalSize, CompleteSize, speed);
                    }
                }

                if (state == 2) //停止
                {
                    if (DownOrUp == 1)
                    {
                        easy.Reset();
                    }
                    if (DownOrUp == 2)
                    {
                        easy.Pause();
                    }
                }

                System.Windows.Forms.Application.DoEvents();
            }
            catch(Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("CURL-onProgress：总大小：{0}，已完成传输大小：{1}，发生错误：{2}", TotalSize, CompleteSize, ex.Message), LogType.Plat, LogLevel.ERROR));
                throw ex;
            }
            return 0;
        }
        #endregion

        #region 暂停上传/下载
        /// <summary>
        /// 暂停上传/下载
        /// </summary>
        public void Pause()
        {
            state = 1;
            if (easy != null)
            {
                easy.Pause();
            }
        }

        public void Resume()
        {
            state = 0;
            if (easy != null)
            {
                BeginTime = DateTime.Now;
                easy.Continue();
            }
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
