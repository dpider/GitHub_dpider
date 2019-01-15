using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace pde.log
{
    public class LogContent
    {
        /// <summary>
        /// 日志发生位置信息
        /// </summary>
        public StackFrame StackFrameInfo { get; set; }

        /// <summary>
        /// 日志跟踪信息，单元名、多少行
        /// </summary>
        public string CodeInfo { get; set; }


        /// <summary>
        /// 日志信息文本内容
        /// </summary>
        public string Message { get; set; }
    }
}
