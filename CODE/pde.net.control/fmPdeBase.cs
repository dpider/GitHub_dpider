using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace pde.net.control
{
    /// <summary>
    /// 窗体系统按钮，fsbMin 最小化按钮, fsMax 最大化按钮， fsClose 关闭按钮 
    /// </summary>
    public enum FormSysButtons { fsbMin, fsbMax, fsbClose };

    public partial class fmPdeBase : Form
    {
        public fmPdeBase()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
        }

        #region ==========属性============
        private Color sysbtncolor = Color.Black;
        /// <summary>
        /// 系统按钮颜色
        /// </summary>
        public Color SysButtonColor
        {
            get { return sysbtncolor; }
            set { sysbtncolor = value; }
        }

        /// <summary>
        /// 标题栏背景色
        /// </summary>
        private Color systitlebackcolor = Color.FromArgb(100, Color.DodgerBlue);
        public Color SysTitleBackColor
        {
            get { return systitlebackcolor; }
            set { systitlebackcolor = value; }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
                lCaption.Text = value;
            }
        }

        /// <summary>
        /// 是否可移动窗体
        /// </summary>
        private bool canmoveform = true;
        public bool CanMoveForm
        {
            get { return canmoveform; }
            set { canmoveform = value; }
        }

        #endregion

        private void fmPdeBase_Load(object sender, EventArgs e)
        {
            pTitle.BackColor = SysTitleBackColor;
            pCaption.BackColor = Color.Transparent;
            pSystemButtons.BackColor = Color.Transparent;
            pLogoIcon.BackColor = Color.Transparent;
            lCaption.BackColor = Color.Transparent;
            this.DoubleBuffered = true;
            //使用 SetStyle 方法可以为 Windows 窗体和所创作的 Windows 控件启用默认双缓冲。 
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;

            #region 事件绑定
            piMaximizeBox.MouseEnter += piMinimizeBox_MouseEnter;
            piMaximizeBox.MouseLeave += piMinimizeBox_MouseLeave;
            piMaximizeBox.MouseDown += piMinimizeBox_MouseDown;
            piMaximizeBox.MouseUp += piMinimizeBox_MouseUp;

            pSystemButtons.MouseMove += pTitle_MouseMove;
            pSystemButtons.MouseDoubleClick += pTitle_MouseDoubleClick;
            pCaption.MouseMove += pTitle_MouseMove;
            pCaption.MouseDoubleClick += pTitle_MouseDoubleClick;
            pLogoIcon.MouseMove += pTitle_MouseMove;
            pLogoIcon.MouseDoubleClick += pTitle_MouseDoubleClick;
            lCaption.MouseMove += pTitle_MouseMove;
            lCaption.MouseDoubleClick += pTitle_MouseDoubleClick;
            #endregion
            
            initSysButtons(new FormSysButtons[] { FormSysButtons.fsbMin, FormSysButtons.fsbMax, FormSysButtons.fsbClose });
            lCaption.Text = this.Text;
        }


        #region ======== 系统按钮排列，可继承 ========
        public virtual void initSysButtons(FormSysButtons[] btns)
        {
            piMinimizeBox.Visible = false;
            piMaximizeBox.Visible = false;
            piCloseBox.Visible = false;

            int nTop = 2;
            int nWidth = 0;
            if (btns.Contains(FormSysButtons.fsbMin))
            {
                piMinimizeBox.Visible = true;
                piMinimizeBox.Location = new Point(nWidth, nTop);
                nWidth += piMinimizeBox.Width;
            }
            if (btns.Contains(FormSysButtons.fsbMax))
            {
                piMaximizeBox.Visible = true;
                piMaximizeBox.Location = new Point(nWidth, nTop);
                nWidth += piMaximizeBox.Width;
            }
            if (btns.Contains(FormSysButtons.fsbClose))
            {
                piCloseBox.Visible = true;
                piCloseBox.Location = new Point(nWidth, nTop);
                nWidth += piCloseBox.Width;
            }
            pSystemButtons.Width = nWidth + nTop;
        } 
        #endregion

        #region ======== 无边框窗体点击任务栏图标正常最小化或还原 ========
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;    //  窗口阴影
                return cp;
            }
        } 
        #endregion

        #region ======== 按钮MouseEnter，MouseLeave，MouseDown，MouseUp，Click事件 ========
        private void piMinimizeBox_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = Color.FromArgb(50, Color.White);
        }

        private void piMinimizeBox_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = Color.Transparent;
        }

        private void piMinimizeBox_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as PictureBox).BackColor = Color.FromArgb(50, Color.Silver);
        }

        private void piMinimizeBox_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as PictureBox).BackColor = Color.Transparent;
        }

        private void piCloseBox_MouseEnter(object sender, EventArgs e)
        {
            piCloseBox.BackColor = Color.Red;
        }

        private void piCloseBox_MouseLeave(object sender, EventArgs e)
        {
            piCloseBox.BackColor = Color.Transparent;
        }

        private void piCloseBox_MouseDown(object sender, MouseEventArgs e)
        {
            piCloseBox.BackColor = Color.DarkRed;
        }

        private void piCloseBox_MouseUp(object sender, MouseEventArgs e)
        {
            piCloseBox.BackColor = Color.Transparent;
        }

        //关闭
        private void piCloseBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //最小化
        private void piMinimizeBox_Click(object sender, EventArgs e)
        {
            if (!this.ShowInTaskbar)
            {
                this.Hide();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        //最大化
        private void piMaximizeBox_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized; 

                this.piMaximizeBox.Invalidate();
            }
            else
            {
                this.WindowState = FormWindowState.Normal; 
            }
        }
        #endregion

        #region ======== 绘画最大化、最小化等按钮, 图片大小30*24 ======== 
        private void drawMinimize(Graphics map)
        {
            //使绘图质量最高，即消除锯齿  
            map.SmoothingMode = SmoothingMode.AntiAlias;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 1);//颜色、宽度      
            SolidBrush sb = new SolidBrush(Color.Black); //定义笔刷   
            map.DrawLine(p, 9, 15, 21, 15);//在画板上画直线      
        }

        private void drawMaximize(Graphics map)
        {
            map.SmoothingMode = SmoothingMode.AntiAlias;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 1);
            SolidBrush sb = new SolidBrush(Color.Black);
            map.DrawRectangle(p, 9, 8, 10, 10);
        }

        private void drawRestore(Graphics map)
        {
            int iWidth = 8;  //矩形边长
            int iLen = 3;    //相隔距离 
            int iBegin = 10;  //起始位置，等距
            map.SmoothingMode = SmoothingMode.AntiAlias;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 1);
            SolidBrush sb = new SolidBrush(Color.Black);
            map.DrawRectangle(p, iBegin, iBegin, iWidth, iWidth);
            map.DrawLine(p, iBegin + iLen, iBegin - iLen, iBegin + iLen, iBegin);
            map.DrawLine(p, iBegin + iLen, iBegin - iLen, iBegin + iLen + iWidth, iBegin - iLen);
            map.DrawLine(p, iBegin + iLen + iWidth, iBegin - iLen, iBegin + iLen + iWidth, iBegin - iLen + iWidth);
            map.DrawLine(p, iBegin + iLen + iWidth, iBegin - iLen + iWidth, iBegin + iWidth, iBegin - iLen + iWidth);
        }

        private void drawClose(Graphics map)
        {
            int nLeft = 9;
            map.SmoothingMode = SmoothingMode.HighQuality;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 2);
            SolidBrush sb = new SolidBrush(Color.Black);
            map.DrawLine(p, nLeft, 7, nLeft + 10, 17);
            map.DrawLine(p, nLeft, 17, nLeft + 10, 7);
        }

        private void piMinimizeBox_Paint(object sender, PaintEventArgs e)
        {
            drawMinimize(e.Graphics);
            this.pdeBaseTip.SetToolTip(this.piMinimizeBox, "最小化");
        }

        private void piMaximizeBox_Paint(object sender, PaintEventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {
                drawMaximize(e.Graphics);
                this.pdeBaseTip.SetToolTip(this.piMaximizeBox, "最大化");
            }
            else
            {
                drawRestore(e.Graphics);
                this.pdeBaseTip.SetToolTip(this.piMaximizeBox, "还原");
            }
        }

        private void piCloseBox_Paint(object sender, PaintEventArgs e)
        {
            drawClose(e.Graphics);
            this.pdeBaseTip.SetToolTip(this.piCloseBox, "关闭");
        }
        #endregion

        #region ======== 拖动无边框窗体 改变无边框窗体尺寸  窗体阴影效果 ========
        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    if (piMaximizeBox.Visible)
                    {
                        Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                        vPoint = PointToClient(vPoint);
                        base.WndProc(ref m);
                        if (vPoint.X <= 5)
                            if (vPoint.Y <= 5)
                                m.Result = (IntPtr)Guying_HTTOPLEFT;
                            else if (vPoint.Y >= ClientSize.Height - 5)
                                m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                            else m.Result = (IntPtr)Guying_HTLEFT;
                        else if (vPoint.X >= ClientSize.Width - 5)
                            if (vPoint.Y <= 5)
                                m.Result = (IntPtr)Guying_HTTOPRIGHT;
                            else if (vPoint.Y >= ClientSize.Height - 5)
                                m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                            else m.Result = (IntPtr)Guying_HTRIGHT;
                        else if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOP;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOM;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                case 0x0085:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;

                /* 
              case 0x0201: //鼠标左键按下的消息 
                  m.Msg = 0x00A1; //更改消息为非客户区按下鼠标
                  m.LParam = IntPtr.Zero; //默认值
                  m.WParam = new IntPtr(2);//鼠标放在标题栏内
                  base.WndProc(ref m);
                  break;
                 */
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
         
        private void pTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (CanMoveForm)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //常量  
                    int WM_SYSCOMMAND = 0x0112;

                    //窗体移动  
                    int SC_MOVE = 0xF010;
                    int HTCAPTION = 0x0002;
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                }
            }
        }

        private void pTitle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (piMaximizeBox.Visible)
            {
                piMaximizeBox_Click(sender, e);
            }
        }
        #endregion

        private void fmPdeBase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.Modal)
            {
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.Close();
                }
            }
        }
    }
}
