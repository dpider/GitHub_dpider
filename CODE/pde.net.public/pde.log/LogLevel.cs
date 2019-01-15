using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pde.log
{
    /// <summary>
    /// 日志级别，
    /// </summary>
    public class LogLevel
    { 
        /// <summary>
        /// 级别：0    记录全部日志信息
        /// </summary>
        public const string ALL = "ALL";

        /// <summary>
        /// 级别：1    错误级别，阻止某个功能或者模块不能正常工作的信息 
        /// </summary>
        public const string ERROR = "ERROR";

        /// <summary>
        /// 级别：2    警告级别 
        /// </summary>
        public const string WARN = "WARN";

        /// <summary>
        /// 级别：3    一般信息的日志，最常用 
        /// </summary>
        public const string INFO = "INFO";

        /// <summary>
        /// 级别：4    调试信息的日志，日志信息最多 
        /// </summary>
        public const string DEBUG = "DEBUG";

        /// <summary>
        /// 级别：5    不记录任何信息 
        /// </summary>
        public const string OFF = "OFF";

        /// <summary>
        /// 判断是否需要记录日志，通过不同级别设置，系统记录相应级别及级别以上的日志
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool needLog(string value, string config)
        {
            switch (config.ToUpper())
            {
                case ALL:
                    return true;

                case ERROR:
                    return value.Equals(ERROR, StringComparison.CurrentCultureIgnoreCase); 

                case WARN:
                    return value.Equals(ERROR, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(WARN, StringComparison.CurrentCultureIgnoreCase);

                case INFO:
                    return value.Equals(ERROR, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(WARN, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(INFO, StringComparison.CurrentCultureIgnoreCase);

                case DEBUG:
                    return value.Equals(ERROR, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(WARN, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(INFO, StringComparison.CurrentCultureIgnoreCase) || 
                           value.Equals(DEBUG, StringComparison.CurrentCultureIgnoreCase);

                case OFF:
                    return false;
            }
            return false;
        }

        public static bool needCodeInfo(string value)
        {
            return (value.Equals(ERROR, StringComparison.CurrentCultureIgnoreCase) || 
                    value.Equals(WARN, StringComparison.CurrentCultureIgnoreCase) || 
                    value.Equals(DEBUG, StringComparison.CurrentCultureIgnoreCase));
        }

    }
}
