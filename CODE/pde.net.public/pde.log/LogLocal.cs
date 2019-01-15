using pde.pub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace pde.log
{
    public class LogLocal : LogProvider
    {
        private static LogLocal _log = null;
        private string logPath = "";

        #region  静态方法
        public static void Close()
        {
            if (_log != null)
            {
                _log = null;
            }
        }

        public static LogLocal log()
        {
            if (_log == null)
            {
                _log = new LogLocal();
            }
            return _log;
        }
        #endregion

        protected LogLocal()
        {
            logPath = WRSetting.Set().getSettings("LogPath");
            if ("".Equals(logPath))
            {
                logPath = SystemUtil.CurrentDirectory() + @"Log";  //默认文件夹 
            } 
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath); //不存在则创建文件夹 
            }
        }

        protected override void FormatLogContent(LogEntity logEntity)
        {
            base.FormatLogContent(logEntity);
            //格式化字符串
            logEntity.Content.Message = string.Format("{0}  [{1}]  {2}", DateTime.Now.ToString("HH:mm:ss ffff"), logEntity.Type.ToString(), logEntity.Content.Message);
            if (!logEntity.Content.CodeInfo.Equals(""))
            { 
                logEntity.Content.Message += string.Format("{0}                       [位置]  {1}", 
                                             Environment.NewLine, logEntity.Content.CodeInfo);
            }
        }

        private Dictionary<long, long> lockDic = new Dictionary<long, long>();
        protected override bool DoSaveLog(LogEntity logEntity)
        {
            try
            {
                //读取配置文件固定位置的日志级别
                string logLevel = WRSetting.Set().getSettings("LogLevel");
                if (logLevel.Equals(""))
                {
                    logLevel = LogLevel.ERROR;
                }

                foreach (string level in logEntity.Levels)
                {                    
                    if (!LogLevel.needLog(level, logLevel))
                    {
                        continue;
                    }

                    string logfile = string.Format(@"{0}\{1}", logPath, level);
                    if (!Directory.Exists(logfile))
                    {
                        Directory.CreateDirectory(logfile); //不存在则创建文件夹 
                    }

                    logfile = string.Format(@"{0}\{1}.log", logfile, DateTime.Now.ToString("yyyyMMdd"));

                    FileStream fs;
                    if (File.Exists(logfile))
                    {
                        fs = new FileStream(logfile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
                    }
                    else
                    {
                        fs = new FileStream(logfile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
                    }

                    Byte[] dataArray = Encoding.Default.GetBytes(logEntity.Content.Message + Environment.NewLine);
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
                        catch
                        {
                            while (!lockDic.ContainsKey(len))
                            {
                                len += lockDic[len];
                            }
                        }
                    }
                    fs.Seek(len, SeekOrigin.Begin);
                    fs.Write(dataArray, 0, dataArray.Length);
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
