using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pde.log
{
    /// <summary>
    /// 日志类型：用于记录日志所属类型，方便进一步定位日志发生位置
    /// </summary>
    public class LogType
    {
        /// <summary>
        /// 平台日志
        /// </summary>
        public const string Plat = "Plat";

        /// <summary>
        /// 系统接口
        /// </summary>
        public const string Interface = "Interface";

        /// <summary>
        /// 应用系统日志
        /// </summary>
        public const string System = "System";

        /// <summary>
        /// 操作系统日志
        /// </summary>
        public const string OS = "OS";

        /// <summary>
        /// 用户操作相关日志
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// 数据库相关日志
        /// </summary>
        public const string DBase = "DBase";

        /// <summary>
        /// 硬件相关日志
        /// </summary>
        public const string HardWare = "HardWare";

        /// <summary>
        /// 其他类型日志
        /// </summary>
        public const string Other = "Other";

        private string _type = System;
 
    }
}
