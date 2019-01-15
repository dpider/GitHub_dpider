using pde.net.pub.ProgressBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using static pde.net.pub.ProgressBar.fmWaitBar;

namespace pde.pub
{
    public class ProgressUtil
    { 
        private fmWaitBar barForm;//进度条窗体
        private static ProgressUtil _bar;   
         
        public ProgressUtil()
        {
            barForm = new fmWaitBar();
        }

        public static ProgressUtil Bar()
        {
            if (_bar == null)
            {
                _bar = new ProgressUtil();
            }
            if (_bar.barForm.IsDisposed)
            {
                _bar.barForm = new fmWaitBar();
            }
            return _bar;
        }

         
        public static void Free()
        { 
            if (_bar != null)
            {
                _bar.barForm.Close(); 
                _bar = null;
            }
            GC.Collect();
        }
         
        public static void Wait(string info, ClosedEventHandler onClose = null, System.Windows.Forms.Form owner = null)
        {
            Bar().barForm.barType = BarType.btWait;
            Bar().barForm.OnClose = onClose; 
            Bar().barForm.Info = info;
            if (!Bar().barForm.Visible)
            {
                Bar().barForm.Show(owner);
            }
        }

        public static void Show(string info, ClosedEventHandler onClose = null, System.Windows.Forms.Form owner = null)
        {
            Bar().barForm.barType = BarType.btWaitProgress;
            Bar().barForm.OnClose = onClose; 
            Bar().barForm.Info = info;
            if (!Bar().barForm.Visible)
            {
                Bar().barForm.Show(owner);
            } 
        }

        public static void Show(string info, int complete, int maxValue, ClosedEventHandler onClose = null, System.Windows.Forms.Form owner = null)
        {
            Show(info, complete, 0, maxValue, onClose, owner);
        }

        public static void Show(string info, int complete, int minValue, int maxValue, ClosedEventHandler onClose = null, System.Windows.Forms.Form owner = null)
        {
            Bar().barForm.barType = BarType.btProgress;
            Bar().barForm.OnClose = onClose; 
            Bar().barForm.Info = info;
            Bar().barForm.Min = minValue;
            Bar().barForm.Max = maxValue;
            Bar().barForm.Complete = complete;
            if (!Bar().barForm.Visible)
            {
                Bar().barForm.Show(owner);
            }
            /*
            if (threadBar != null)
            {
                threadBar = null;
            }

            threadBar = new Thread(() =>
            {
                Bar().barForm.barType = BarType.btWait; 
                Bar().barForm.OnClose = onClose;
                Bar().barForm.thread = threadBar;
                //   pBarUtil().fmOwer = ower;
                Bar().barForm.Info = info;
                Bar().barForm.Show();
                while (true)
                { 
                    Thread.Sleep(100);
                    System.Windows.Forms.Application.DoEvents();
                }

            });
            threadBar.IsBackground = true;
            threadBar.Start(); */
        }

        public static void SetInfo(string info)
        {
            Bar().barForm.Info = info;
        }

        public static void SetComplete(int complete)
        {
            Bar().barForm.Complete = complete;
        }
    }
     
}
