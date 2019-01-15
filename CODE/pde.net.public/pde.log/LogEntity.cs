using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pde.log
{ 
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public string[] Levels { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public LogContent Content { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="logLevel">ERROR：只记录错误日志；WARN：记录错误和警告日志；INFO：记录信息、错误和警告日志；DEBUG：记录调试、信息、错误和警告日志；ALL：记录所有日志；OFF：不记录任何日志</param>
        public LogEntity(string message, string logType, string logLevel)
        {
            Content = new LogContent();
            Content.Message = message;
            Type = logType;
            Levels = new string[] { logLevel };
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="logLevels">ERROR：只记录错误日志；WARN：记录错误和警告日志；INFO：记录信息、错误和警告日志；DEBUG：记录调试、信息、错误和警告日志；ALL：记录所有日志；OFF：不记录任何日志</param>
        public LogEntity(string message, string logType, string[] logLevels)
        {
            Content = new LogContent();
            Content.Message = message;
            Type = logType;
            Levels = logLevels;
        }

        /// <summary>
        /// 日志，默认日志类型为：系统 
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logLevel">ERROR：只记录错误日志；WARN：记录错误和警告日志；INFO：记录信息、错误和警告日志；DEBUG：记录调试、信息、错误和警告日志；ALL：记录所有日志；OFF：不记录任何日志</param>
        public LogEntity(string message, string logLevel)
        { 
            Content = new LogContent();
            Content.Message = message;
            Type = LogType.System;
            Levels = new string[] { logLevel };
        }

        /// <summary>
        /// 日志，默认类型为：系统 
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logLevels">ERROR：只记录错误日志；WARN：记录错误和警告日志；INFO：记录信息、错误和警告日志；DEBUG：记录调试、信息、错误和警告日志；ALL：记录所有日志；OFF：不记录任何日志</param>
        public LogEntity(string message, string[] logLevels)
        {
            Content = new LogContent();
            Content.Message = message;
            Type = LogType.System;
            Levels = logLevels;
        }

        /// <summary>
        /// 日志，默认类型为：系统，日志级别为：错误
        /// </summary>
        /// <param name="message"></param>
        public LogEntity(string message)
        {
            Content = new LogContent();
            Content.Message = message;
            Type = LogType.System;
            Levels = new string[] { LogLevel.ERROR };
        }
    }
}
