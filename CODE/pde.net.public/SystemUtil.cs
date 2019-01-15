using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace pde.pub
{
    public class SystemUtil
    {
        /// <summary>
        /// 获得窗体上控件相对于屏幕的位置
        /// </summary>
        /// <param name="c">控件</param>
        /// <returns></returns>
        public static Point LocationOnClient(Control c)
        {
            Point retval = new Point(0, 0);
            for (; c.Parent != null; c = c.Parent)
            {
                retval.Offset(c.Location);
            }
            return retval;
        }


        /// <summary>
        /// 运行EXE
        /// </summary>
        /// <param name="exePath">EXE文件路径</param>
        /// <param name="param"></param>
        /// <returns>执行的结果</returns>
        public static string runExeWaitExit(string exePath, string param, bool waitForExit = true, bool createNoWindow = true)
        {
            DirectoryInfo di = Directory.GetParent(@exePath);
            string workingDirectory = di.ToString();
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exePath;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.CreateNoWindow = createNoWindow;
            p.StartInfo.Arguments = param;//参数以空格分隔，如果某个参数为空，可以传入””
            p.Start();
            //此处可以返回一个字符串，此例是返回压缩成功之后的一个文件路径
            string output = p.StandardOutput.ReadToEnd();
            if (waitForExit)
            {
                p.WaitForExit();
            }
            return output;
        }

        /// <summary>
        /// 限时调用某个函数，超时退出
        /// </summary>
        /// <param name="action">函数</param>
        /// <param name="timeoutMilliseconds">超时时间，毫秒级</param>
        public static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// 获取程序运行当前目录
        /// </summary>
        /// <returns></returns>
        public static string CurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #region 获取enum注释内容
        /// <summary>
        /// 获取enum注释内容
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="val">枚举值或指定值</param>
        /// <returns></returns>
        public static string getEnumDescription(Type enumType, object val)
        {
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            if (val is string)
            {
                foreach (FieldInfo field in fields)
                {
                    if ((field.FieldType.IsEnum) && (field.Name.Equals(val)))
                    {
                        object[] arr = field.GetCustomAttributes(typeDescription, true);
                        if (arr.Length > 0)
                        {
                            DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                            return aa.Description;
                        }
                    }
                }
            }

            if (val is int)
            {
                foreach (FieldInfo field in fields)
                {
                    if ((field.FieldType.IsEnum) && (field.Name.Equals(Enum.GetName(enumType, val))))
                    {
                        object[] arr = field.GetCustomAttributes(typeDescription, true);
                        if (arr.Length > 0)
                        {
                            DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                            return aa.Description;
                        }
                    }
                }
            }
            return val.ToString();
        }
        #endregion

       
         
    }
}
