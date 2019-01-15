using pde.net.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pde.pub
{
    public partial class PBarUtil : pdeBaseForm
    {   
        public ProgressUtil2.onClose closeEvent = null; 
        public Form fmOwer = null;
        public bool bStop = false; 


        private Image m_img = null;
        private EventHandler evtHandler = null;
          
        public PBarUtil()
        {
            InitializeComponent();  
        } 
        private void fmProgress_Load(object sender, EventArgs e)
        {
           if (picWait.Visible)
            {
                //为委托关联一个处理方法   
                evtHandler = new EventHandler(OnImageAnimate);
                //获取要加载的gif动画文件   
                m_img = picWait.Image;
                //调用开始动画方法   
                BeginAnimate();
            }
        }

        //开始动画方法   
        private void BeginAnimate()
        {
            if (m_img != null)
            {
                //当gif动画每隔一定时间后，都会变换一帧，那么就会触发一事件，该方法就是将当前image每变换一帧时，都会调用当前这个委托所关联的方法。  
                ImageAnimator.Animate(m_img, evtHandler);
            }
        }

        //获得当前gif动画的下一步需要渲染的帧，当下一步任何对当前gif动画的操作都是对该帧进行操作)   
        private void UpdateImage()
        {
            ImageAnimator.UpdateFrames(m_img);
        }

        //关闭显示动画，该方法可以在winform关闭时，或者某个按钮的触发事件中进行调用，以停止渲染当前gif动画。   
        private void StopAnimate()
        {
            m_img = null;
            ImageAnimator.StopAnimate(m_img, evtHandler);
        }

        //委托所关联的方法   
        private void OnImageAnimate(Object sender, EventArgs e)
        {
            //该方法中，只是使得当前这个winfor重绘，然后去调用该winform的OnPaint（）方法进行重绘)   
            this.Invalidate();
        }

        private void PBarUtil_Paint(object sender, PaintEventArgs e)
        {
         //   base.OnPaint(e);
            if (m_img != null)
            {
                //获得当前gif动画下一步要渲染的帧。   
                UpdateImage();
                //将获得的当前gif动画需要渲染的帧显示在界面上的某个位置。   
                e.Graphics.DrawImage(m_img, new Rectangle(145, 140, m_img.Width, m_img.Height));
            }
        }

        private void PBarUtil_Shown(object sender, EventArgs e)
        {
            if (fmOwer != null)
            {
                Invoke(new Action(() =>
                {
                    this.Owner = fmOwer;
                    Application.DoEvents();
                }));
            }
        }


        private void PBarUtil_FormClosing(object sender, FormClosingEventArgs e)
        {
            bStop = true;
        }

        private void piCloseBox_Click(object sender, EventArgs e)
        {
            if (closeEvent!= null)
            { 
                if (!closeEvent())
                {
                    return;
                }
            }
            Close();
        } 
    }

    public static class ProgressUtil2
    {
        public delegate bool onClose();
        private static PBarUtil _pBar = null;
        private static Thread pThread = null;
        private static volatile bool inCycle = false; 
        private static volatile System.Timers.Timer timerWait;

        private static PBarUtil pBarUtil()
        { 
            if ((_pBar == null) || (_pBar.IsDisposed))
            {
                _pBar = new PBarUtil();
            }
            return _pBar;
        }

        #region 显示进度条（循环进度）
        /// 显示进度条（循环进度）
        /// </summary>
        /// <param name="info">进度信息</param>
        /// <param name="onclose">点击关闭事件</param>
        /// <param name="ower">进度条宿主</param>
        public static void Show(string info, onClose onclose = null, Form ower = null)
        {
            inCycle = false;
            if (pThread != null) { pThread = null; }

            pThread = new Thread(() =>
            {
                pBarUtil().ShowSysTitle = true;
                pBarUtil().picWait.Visible = false;
                pBarUtil().lblInfo.Location = new Point(50, 27);
                pBarUtil().pb.Location = new Point(46, 54);
                pBarUtil().pb.Value = 0;
                pBarUtil().pb.Step = 8;
                pBarUtil().pb.Visible = true;
                pBarUtil().lblProgress.Visible = false;
                pBarUtil().closeEvent = onclose;
                //  pBar.fmOwer = ower;
                pBarUtil().Show();

 
                SetInfo(info);
                inCycle = true;
                while (true)
                {
                    if (!inCycle) return; 
                    if (pBarUtil().IsHandleCreated)
                    {
                        pBarUtil()?.Invoke(new Action(() =>
                        {
                            if (pBarUtil().pb.Value >= pBarUtil().pb.Maximum)
                            {
                                if (timerWait == null)
                                {
                                    timerWait = new System.Timers.Timer();
                                    timerWait.Interval = 600;
                                    timerWait.Elapsed += timerWait_Elapsed;
                                }
                                timerWait.Start();
                            }
                            else
                            {
                                pBarUtil().pb.Value++;
                            }
                            pBarUtil().BringToFront();
                            pBarUtil().pb.Refresh();
                        }));
                    }
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
            });
            pThread.Start();
        }
        #endregion

        private static void timerWait_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerWait.Stop();
            if (pBarUtil().IsHandleCreated)
            {
                pBarUtil()?.Invoke(new Action(() =>
                {
                    pBarUtil().pb.Value = 0;
                }));
            }
        }

        #region 等待显示（无进度条）
        /// 等待显示（无进度条）
        /// </summary>
        /// <param name="info">进度信息</param>
        /// <param name="onclose">点击关闭事件</param>
        /// <param name="ower">进度条宿主</param>
        public static void Wait(string info, onClose onclose = null, Form ower = null)
        {
            inCycle = false;
            if (pThread != null)
            {
                try
                {
                    pThread.Abort();
                }
                catch { }
            }
            pThread = null;

            pThread = new Thread(() =>
            {
                pBarUtil().ShowSysTitle = true;
                pBarUtil().picWait.Location = new Point(50, 32);
                pBarUtil().picWait.Visible = true;
                pBarUtil().lblInfo.Location = new Point(100, 40);
                pBarUtil().pb.Visible = false;
                pBarUtil().lblProgress.Visible = false;
                pBarUtil().closeEvent = onclose;
             //   pBarUtil().fmOwer = ower;
                pBarUtil().Show();
                SetInfo(info);
                inCycle = true;
                while (true)
                {
                    if (!inCycle) return; 
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

            }); 
            pThread.Start();   
        }
        #endregion

        #region 释放进度条
        /// <summary>
        /// 释放进度条
        /// </summary>
        public static void Free()
        {
            if (timerWait != null)
            {
                timerWait.Close();
            }
            inCycle = false;
            Thread.Sleep(200);
            Application.DoEvents();
            if (pThread != null)
            {
                try
                {
                    pThread.Abort();
                }
                catch { }
            }
            pThread = null;
            if (_pBar != null)
            {
                _pBar = null;
            }
            Form fm = Application.OpenForms["PBarUtil"];
            if (fm != null)
            {
                fm = null;
            }
        }
        #endregion

        #region 显示进度条（指定进度）
        /// <summary>
        /// 显示进度条（指定进度）
        /// </summary>
        /// <param name="info">进度信息</param>
        /// <param name="complete">已完成进度</param>
        /// <param name="maxValue">最大进度</param>
        /// <param name="onclose">点击关闭事件</param>
        /// <param name="ower">进度条宿主</param>
        public static void Show(string info, int complete, int maxValue, onClose onclose = null, Form ower = null)
        {
            inCycle = false;
            if (pThread != null) { pThread = null; }

            pThread = new Thread(() =>
            {
                pBarUtil().ShowSysTitle = true;
                pBarUtil().picWait.Visible = false;
                pBarUtil().lblInfo.Location = new Point(50, 27);
                pBarUtil().pb.Location = new Point(46, 54);
                pBarUtil().pb.Value = 0;
                pBarUtil().pb.Step = 8;
                pBarUtil().pb.Visible = true;
                pBarUtil().pb.Value = 0;
                pBarUtil().pb.Step = 1;
                pBarUtil().lblProgress.Visible = true;
                pBarUtil().closeEvent = onclose;
                // pBarUtil().fmOwer = ower;
                pBarUtil().Show();

                SetProgress(info, complete, maxValue);

                inCycle = true;
                while (true)
                {
                    if (!inCycle) return;
                    if (_pBar == null) { return; }  
                    Application.DoEvents();
                } 
            }); 
            pThread.Start();
        }
        #endregion

        #region  设置进度信息
        /// <summary>
        /// 设置进度信息
        /// </summary>
        /// <param name="info">进度信息</param>
        public static void SetInfo(string info)
        {
            if (_pBar == null) return;
            if (_pBar.IsHandleCreated)
            {
                _pBar?.Invoke(new Action(() =>
                {
                    _pBar.lblInfo.Text = info;
                    _pBar.BringToFront();
                    _pBar.pb.Refresh();
                }));
            }
            Application.DoEvents();
            /*
            Thread thread = new Thread(() =>
            { 
               
            });
            thread.IsBackground = true;
            thread.Start(); */
        }
        #endregion

        #region 设置完成进度值
        /// <summary>
        /// 设置完成进度值
        /// </summary>
        /// <param name="value">已完成进度值</param>
        public static void SetValue(int value)
        {  
            if (!pBarUtil().pb.Visible) { return; }
            Thread thread = new Thread(() =>
            {
                if (pBarUtil().IsHandleCreated)
                {
                    pBarUtil()?.Invoke(new Action(() =>
                    {
                        pBarUtil().pb.Value = value > pBarUtil().pb.Maximum ? pBarUtil().pb.Maximum : value;
                        pBarUtil().lblProgress.Text = string.Format("{0} / {1}", value, pBarUtil().pb.Maximum);
                        pBarUtil().BringToFront();
                        pBarUtil().pb.Refresh();
                    }));
                }
                Application.DoEvents();
            });
            thread.IsBackground = true;
            thread.Start();  
        }
        #endregion

        private static void SetProgress(string info, int value, int max)
        {  
            if (!pBarUtil().pb.Visible) { return; }
            Thread thread = new Thread(() =>
            {
                if (pBarUtil().IsHandleCreated)
                {
                    pBarUtil()?.Invoke(new Action(() =>
                    {
                        pBarUtil().lblInfo.Text = info;
                        pBarUtil().pb.Maximum = max;
                        pBarUtil().pb.Value = value > pBarUtil().pb.Maximum ? pBarUtil().pb.Maximum : value;
                        pBarUtil().lblProgress.Text = string.Format("{0} / {1}", value, pBarUtil().pb.Maximum);
                        pBarUtil().BringToFront();
                        pBarUtil().pb.Refresh();
                    }));
                }
                Application.DoEvents();
            });
            thread.IsBackground = true;
            thread.Start();
        }

      

    }

    
}
