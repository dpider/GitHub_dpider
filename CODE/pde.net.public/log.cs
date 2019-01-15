using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace pde.pub
{

    public class Log
    {
        public enum PdeLogLevel
        {
            [System.ComponentModel.Description("debug")]
            llDebug = 0,
            [System.ComponentModel.Description("info")]
            llInfo = 1,
            [System.ComponentModel.Description("warn")]
            llWarn = 2,
            [System.ComponentModel.Description("error")]
            llError = 3
        }
        private static Log pdeLog = null;
        private string logPath = "";
        private List<PdeLogLevel> logLevel = new List<PdeLogLevel>() { PdeLogLevel.llInfo, PdeLogLevel.llError};

        #region  静态方法
        public static void Close()
        {
            if (pdeLog != null)
            {
                pdeLog = null;
            }
        }

        public static Log PDELog1()
        {
            if (pdeLog == null)
            {
                pdeLog = new Log(); 
            }
            return pdeLog;
        }

        public string LogPath
        {
            get { return PDELog1().logPath; }
            set
            {
                if (!"".Equals(value))
                    PDELog1().logPath = value;
            }  
        }

        public List<PdeLogLevel> LogLevel
        { 
            get { return PDELog1().logLevel; }
            set { PDELog1().logLevel = value; }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        public void Write(string message)
        { 
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug)) { Debug(message); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo)) { Info(message); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn)) { Warn(message); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError)) { Error(message); }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        /// <param name="username">当前用户名</param>
        public void Write(string message, string username)
        { 
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug)) { Debug(message, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo)) { Info(message, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn)) { Warn(message, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError)) { Error(message, username); }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="username">当前用户名</param>
        public void Write(string message, string userid, string username)
        { 
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug)) { Debug(message, userid, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo)) { Info(message, userid, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn)) { Warn(message, userid, username); }
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError)) { Error(message, userid, username); }
        }

        #region 具体写日志方法
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug))
            {
                string msg = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ffff"), message);
                writeLog("debug", msg);
            }
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="username">当前用户名</param>
        public void Debug(string message, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug))
            {
                string msg = string.Format("[{0}] {1} {2}", DateTime.Now.ToString("HH:mm:ss ffff"), username, message);
                writeLog("debug", msg);
            }
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="username">当前用户名</param>
        public void Debug(string message, string userid, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llDebug))
            {
                string msg = string.Format("[{0}] {1} {2} {3}", DateTime.Now.ToString("HH:mm:ss ffff"), userid, username, message);
                writeLog("debug", msg);
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        public void Info(string message)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo))
            {
                string msg = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ffff"), message);
                writeLog("info", msg);
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        /// <param name="username">当前用户名</param>
        public void Info(string message, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo))
            {
                string msg = string.Format("[{0}] {1} {2}", DateTime.Now.ToString("HH:mm:ss ffff"), username, message);
                writeLog("info", msg);
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="username">当前用户名</param>
        public void Info(string message, string userid, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llInfo))
            {
                string msg = string.Format("[{0}] {1} {2} {3}", DateTime.Now.ToString("HH:mm:ss ffff"), userid, username, message);
                writeLog("info", msg);
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        public void Warn(string message)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn))
            {
                string msg = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ffff"), message);
                writeLog("warn", msg);
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        /// <param name="username">当前用户名</param>
        public void Warn(string message, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn))
            {
                string msg = string.Format("[{0}] {1} {2}", DateTime.Now.ToString("HH:mm:ss ffff"), username, message);
                writeLog("warn", msg);
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="username">当前用户名</param>
        public void Warn(string message, string userid, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llWarn))
            {
                string msg = string.Format("[{0}] {1} {2} {3}", DateTime.Now.ToString("HH:mm:ss ffff"), userid, username, message);
                writeLog("warn", msg);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        public void Error(string message)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError))
            {
                string msg = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ffff"), message);
                writeLog("error", msg);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">日志信息</param> 
        /// <param name="username">当前用户名</param>
        public void Error(string message, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError))
            {
                string msg = string.Format("[{0}] {1} {2}", DateTime.Now.ToString("HH:mm:ss ffff"), username, message);
                writeLog("error", msg);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="username">当前用户名</param>
        public void Error(string message, string userid, string username)
        {
            if (pdeLog.logLevel.Contains(PdeLogLevel.llError))
            {
                string msg = string.Format("[{0}] {1} {2} {3}", DateTime.Now.ToString("HH:mm:ss ffff"), userid, username, message);
                writeLog("error", msg);
            }
        }
        #endregion

        private void writeLog(string pre, string msg)
        {
            string logfile = string.Format(@"{0}\{1}_{2}.log", PDELog1().logPath, pre, DateTime.Now.ToString("yyyyMMdd"));
            PDELog1().saveLog(logfile, msg);
        }
        #endregion


        public Log()
        {
            logPath = WRSetting.Set().getSettings("LogPath");
            if ("".Equals(logPath))
            {
                logPath = AppDomain.CurrentDomain.BaseDirectory + @"\Log";  //默认文件夹 
            }
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath); //不存在则创建文件夹 
            } 
        }

        private Dictionary<long, long> lockDic = new Dictionary<long, long>();
        private void saveLog(string logfile, string message)
        {
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(logfile))
            {
                fs = new FileStream(logfile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
            }
            else
            {
                fs = new FileStream(logfile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
            }
            /*
            sw = new StreamWriter(fs);
            sw.WriteLine(message);
            sw.Close();
            fs.Close(); */

            Byte[] dataArray = System.Text.Encoding.Default.GetBytes(message + System.Environment.NewLine);
            bool flag = true;
            long slen = dataArray.Length;
            long len = 0;
            while (flag)
            {
                try
                {
                    if (len >= fs.Length)
                    {
                        fs.Lock(len, slen);
                        lockDic[len] = slen;
                        flag = false;
                    }
                    else
                    {
                        len = fs.Length;
                    }
                }
                catch (Exception ex)
                {
                    while (!lockDic.ContainsKey(len))
                    {
                        len += lockDic[len];
                    }
                }
            }
            fs.Seek(len, System.IO.SeekOrigin.Begin);
            fs.Write(dataArray, 0, dataArray.Length);
            fs.Close();
        }
    }
}
