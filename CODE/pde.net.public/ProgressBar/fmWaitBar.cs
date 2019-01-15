using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace pde.net.pub.ProgressBar
{
    public partial class fmWaitBar : pde.net.control.pdeBaseForm
    {
        public enum BarType { btWait, btWaitProgress, btProgress }
        // 定义事件使用的委托
        public delegate bool ClosedEventHandler(); 

        private event ClosedEventHandler onClose;
        private BarType _barType = BarType.btWaitProgress;

        private Image m_img = null;
        private EventHandler evtHandler = null;  

        public fmWaitBar()
        {
            InitializeComponent();
            lblInfo.MouseMove += PdeFormMouseMove;
            lblProgress.MouseMove += PdeFormMouseMove;
        }

        private void fmWaitBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void piCloseBox_Click(object sender, EventArgs e)
        {
            if (onClose != null)
            {
                if (!onClose())
                {
                    return;
                }
            }

            if (_barType == BarType.btWait)
            {
                StopAnimate();
            } 
            Close();
        }

        #region 进度条类型属性  btWait-无进度等待； btWaitProgress-进度条等待；  btProgress-有进度显示等待
        public BarType barType
        {
            get { return _barType; }
            set
            {
                _barType = value;
                MethodInvoker invoker = () =>
                {
                    switch (_barType)
                    {
                        case BarType.btWait:
                            picWait.Location = new Point(50, 32);
                            lblInfo.Location = new Point(100, 40);
                            picWait.Visible = true;
                            pb.Visible = false;
                            lblProgress.Visible = false;
                            //为委托关联一个处理方法   
                            this.Paint += PBar_Paint;
                            evtHandler = new EventHandler(OnImageAnimate);
                            //获取要加载的gif动画文件   
                            m_img = picWait.Image;
                            //调用开始动画方法   
                            BeginAnimate();
                            break;

                        case BarType.btWaitProgress:
                            picWait.Visible = false;
                            lblInfo.Location = new Point(50, 27);
                            pb.Location = new Point(46, 54);
                            pb.Value = 0;
                            pb.Step = 10;
                            pb.Style = ProgressBarStyle.Marquee;
                            pb.Visible = true;
                            lblProgress.Visible = false;
                            break;

                        case BarType.btProgress:
                            picWait.Visible = false;
                            lblInfo.Location = new Point(50, 27);
                            pb.Location = new Point(46, 54);
                            pb.Value = 0;
                            pb.Step = 8;
                            pb.Style = ProgressBarStyle.Continuous;
                            pb.Visible = true;
                            lblProgress.Visible = true;
                            break;
                    }
                };

                if (this.InvokeRequired)
                {
                    this.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
        }
        #endregion

        #region 进度等待信息属性
        public string Info
        {
            get { return lblInfo.Text; }
            set
            {
                MethodInvoker invoker = () =>
                {
                    lblInfo.Text = value;
                    Application.DoEvents();
                };

                if (this.lblInfo.InvokeRequired)
                {
                    this.lblInfo.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
        }
        #endregion

        #region 进度完成值属性
        public int Complete
        {
            get { return pb.Value; }
            set
            {
                MethodInvoker invoker = () =>
                { 
                    if (value < pb.Minimum)
                    {
                        pb.Value = pb.Minimum;
                    }
                    else if (value >= pb.Maximum)
                    {
                        pb.Value = pb.Maximum;
                    }
                    else
                    {
                        pb.Value = value;
                    } 
                    lblProgress.Text = string.Format("{0} / {1}", pb.Value, pb.Maximum);
                    Application.DoEvents();
                    if (pb.Value == pb.Maximum)
                    {
                        Thread.Sleep(500);
                    }
                };

                if (this.pb.InvokeRequired)
                {
                    this.pb.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
        } 
        #endregion

        #region 总进度值最小值属性
        public int Min
        {
            get { return pb.Minimum; }
            set
            {
                MethodInvoker invoker = () =>
                {
                    pb.Minimum = value;
                };

                if (this.pb.InvokeRequired)
                {
                    this.pb.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
        }
        #endregion

        #region 总进度值属性
        public int Max
        {
            get { return pb.Maximum; }
            set
            {
                MethodInvoker invoker = () =>
                { 
                    pb.Maximum = value;
                };

                if (this.pb.InvokeRequired)
                {
                    this.pb.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
        }
        #endregion

        #region 进度条关闭方法属性
        public ClosedEventHandler OnClose
        {
            get { return onClose; }
            set { onClose = value; }
        }
        #endregion

        #region 无进度动画相关代码
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

        private void PBar_Paint(object sender, PaintEventArgs e)
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



        #endregion

        private void fmWaitBar_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Enabled = false;
            }
        }

        private void fmWaitBar_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Enabled = true;
            }
        }
    }
}
