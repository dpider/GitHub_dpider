using pde.pub;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace pde.log
{
    public interface ILogProvider
    {
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="logEntity"></param>
        /// <returns></returns>
        bool SaveLog(LogEntity logEntity);
    }

    public abstract class LogProvider : ILogProvider
    {
        public bool SaveLog(LogEntity logEntity)
        {
            if (logEntity == null) return false;
            if (logEntity.Content == null) return false;
            if (logEntity.Content.Message.Equals("")) return false; 
            FormatLogContent(logEntity);
            return DoSaveLog(logEntity);
        }
          
        /// <summary>
        /// 格式化日志内容
        /// </summary>
        /// <param name="logEntity"></param>
        protected virtual void FormatLogContent(LogEntity logEntity)
        {
            if (logEntity.Content.StackFrameInfo != null)
            { 
                logEntity.Content.CodeInfo = string.Format("文件：[{0}]    方法：[{1}]    行列：[{2},{3}]",
                                             Path.GetFileName(logEntity.Content.StackFrameInfo.GetFileName()),
                                             logEntity.Content.StackFrameInfo.GetMethod().Name,
                                             logEntity.Content.StackFrameInfo.GetFileLineNumber(),
                                             logEntity.Content.StackFrameInfo.GetFileColumnNumber());
            }
            else
            {
                logEntity.Content.CodeInfo = "";
            } 
        }

        /// <summary>
        /// 执行保存
        /// </summary>
        /// <param name="logEntity"></param>
        /// <returns></returns>
        protected abstract bool DoSaveLog(LogEntity logEntity);
    }
}
