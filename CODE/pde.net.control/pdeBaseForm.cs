using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace pde.net.control
{
    public partial class pdeBaseForm : Form
    {
        /// <summary>
        /// 窗体系统按钮，fsbMin 最小化按钮, fsMax 最大化按钮， fsClose 关闭按钮 
        /// </summary>
        public enum FormSysButtons { fsbMenu, fsbSet, fsbSkin, fsbMin, fsbMax, fsbClose };

        public pdeBaseForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
            pLogoIcon.BackColor = Color.Transparent;
            lCaption.BackColor = Color.Transparent;
        }

        #region ==========属性============
        private Color sysBtnColor = Color.Black;
        /// <summary>
        /// 系统按钮颜色
        /// </summary>
        public Color SysButtonColor
        {
            get { return sysBtnColor; }
            set { sysBtnColor = value; }
        }

        /// <summary>
        /// 标题栏前景色
        /// </summary>
        private Color sysTitleColor = Color.FromArgb(100, Color.DodgerBlue);
        public Color SysTitleColor
        {
            get { return sysTitleColor; }
            set
            {
                sysTitleColor = value;
                lCaption.ForeColor = sysTitleColor; 
            }
        }

        /// <summary>
        /// 标题栏背景色
        /// </summary>
        private Color sysTitleBackColor = Color.FromArgb(100, Color.DodgerBlue);
        public Color SysTitleBackColor
        {
            get { return sysTitleBackColor; }
            set
            {
                sysTitleBackColor = value;
                pTitle.BackColor = sysTitleBackColor;
            }
        }

        /// <summary>
        /// 标题栏背景图
        /// </summary>
        private Image sysTitleBackImage = null;
        public Image SysTitleBackImage
        {
            get { return sysTitleBackImage; }
            set
            {
                sysTitleBackImage = value;
                pTitle.BackgroundImage = sysTitleBackImage;
            }
        }

        /// <summary>
        /// 是否显示标题栏
        /// </summary>
        private bool showSysTitle = true;
        public bool ShowSysTitle
        {
            get { return showSysTitle; }
            set
            {
                showSysTitle = value;
                pTitle.Visible = showSysTitle;
                piSysMenu.Visible = showSysMenuBtn & ShowSysTitle;
                piSysSkin.Visible = showSysSkinBtn & ShowSysTitle;
                piSysSet.Visible = showSysSetBtn & ShowSysTitle;
                piMinimizeBox.Visible = showMinimizeBtn & ShowSysTitle;
                piMaximizeBox.Visible = showMaximizeBtn & ShowSysTitle;
                piCloseBox.Visible = showCloseBtn & ShowSysTitle;
                initSysBtns();
            }
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
        private bool canMoveForm = true;
        public bool CanMoveForm
        {
            get { return canMoveForm; }
            set { canMoveForm = value; }
        }

        /// <summary>
        /// 系统菜单
        /// </summary>
        private ContextMenuStrip sysMenuStrip = null;
        public ContextMenuStrip SysMenuStrip
        {
            get { return sysMenuStrip; }
            set
            {
                sysMenuStrip = value;
                initSysBtns();
            }
        }

        /// <summary>
        /// 系统按钮显示与否
        /// </summary>

        private bool showSysMenuBtn = true;
        public bool ShowSysMenuBtn
        {
            get { return showSysMenuBtn; }
            set
            {
                showSysMenuBtn = value;
                piSysMenu.Visible = showSysMenuBtn & ShowSysTitle;
                initSysBtns();
            }
        }

        private bool showSysSkinBtn = true;
        public bool ShowSysSkinBtn
        {
            get { return showSysSkinBtn; }
            set
            {
                showSysSkinBtn = value;
                piSysSkin.Visible = showSysSkinBtn & ShowSysTitle;
                initSysBtns();
            }
        }

        private bool showSysSetBtn = true;
        public bool ShowSysSetBtn
        {
            get { return showSysSetBtn; }
            set
            {
                showSysSetBtn = value;
                piSysSet.Visible = showSysSetBtn & ShowSysTitle;
                initSysBtns();
            }
        }

        private bool showMinimizeBtn = true;
        public bool ShowMinimizeBtn
        {
            get { return showMinimizeBtn; }
            set
            {
                showMinimizeBtn = value;
                piMinimizeBox.Visible = showMinimizeBtn & ShowSysTitle;
                MinimizeBox = showMinimizeBtn;
                initSysBtns();
            }
        }

        private bool showMaximizeBtn = true;
        public bool ShowMaximizeBtn
        {
            get { return showMaximizeBtn; }
            set
            {
                showMaximizeBtn = value;
                piMaximizeBox.Visible = showMaximizeBtn & ShowSysTitle;
                MaximizeBox = showMaximizeBtn;
                initSysBtns();
            }
        }

        private bool showCloseBtn = true;
        public bool ShowCloseBtn
        {
            get { return showCloseBtn; }
            set
            {
                showCloseBtn = value;
                piCloseBox.Visible = showCloseBtn & ShowSysTitle;
                initSysBtns();
            }
        }
        #endregion
 
        private void pdeBaseForm_Load(object sender, EventArgs e)
        {  
            this.DoubleBuffered = true;
            //使用 SetStyle 方法可以为 Windows 窗体和所创作的 Windows 控件启用默认双缓冲。 
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
             
            #region 事件绑定
            piSysMenu.MouseEnter += piBtn_MouseEnter;
            piSysMenu.MouseLeave += piBtn_MouseLeave;
            piSysMenu.MouseDown += piBtn_MouseDown;
            piSysMenu.MouseUp += piBtn_MouseUp;

            piSysSkin.MouseEnter += piBtn_MouseEnter;
            piSysSkin.MouseLeave += piBtn_MouseLeave;
            piSysSkin.MouseDown += piBtn_MouseDown;
            piSysSkin.MouseUp += piBtn_MouseUp;

            piSysSet.MouseEnter += piBtn_MouseEnter;
            piSysSet.MouseLeave += piBtn_MouseLeave;
            piSysSet.MouseDown += piBtn_MouseDown;
            piSysSet.MouseUp += piBtn_MouseUp;

            piMinimizeBox.MouseEnter += piBtn_MouseEnter;
            piMinimizeBox.MouseLeave += piBtn_MouseLeave;
            piMinimizeBox.MouseDown += piBtn_MouseDown;
            piMinimizeBox.MouseUp += piBtn_MouseUp;

            piMaximizeBox.MouseEnter += piBtn_MouseEnter;
            piMaximizeBox.MouseLeave += piBtn_MouseLeave;
            piMaximizeBox.MouseDown += piBtn_MouseDown;
            piMaximizeBox.MouseUp += piBtn_MouseUp;


            piCloseBox.MouseEnter += piBtn_MouseEnter;
            piCloseBox.MouseLeave += piBtn_MouseLeave;
            piCloseBox.MouseDown += piBtn_MouseDown;
            piCloseBox.MouseUp += piBtn_MouseUp;

            pLogoIcon.MouseMove += PdeFormMouseMove;
            pLogoIcon.MouseDoubleClick += PTitleMouseDoubleClick;
            lCaption.MouseMove += PdeFormMouseMove;
            lCaption.MouseDoubleClick += PTitleMouseDoubleClick;
            #endregion

            lCaption.Text = this.Text;
            pLogoIcon.Visible = ShowIcon;
            if (pLogoIcon.Visible)
            {
                pLogoIcon.Size = new Size(20, 20);
                pLogoIcon.MaximumSize = new Size(20, 20);
                pLogoIcon.MinimumSize = new Size(20, 20);
                pLogoIcon.Location = new Point(7, 5);
                if (pLogoIcon.Image == null)
                {
                    pLogoIcon.Image = Image.FromHbitmap(Icon.ToBitmap().GetHbitmap());
                }
                lCaption.Location = new Point(30, 7);
            }
            else
            {
                lCaption.Location = new Point(7, 7);
            }
            initSysBtns();
        }

        #region ======== 系统按钮排列 ========
        private void initSysBtns()
        { 
            List<PictureBox> btns = new List<PictureBox>();
            /* if ((showCloseBtn) && (ShowSysTitle)) { btns.Add(piCloseBox); } 
             if ((showMaximizeBtn) && (ShowSysTitle)) { btns.Add(piMaximizeBox); }  
             if ((showMinimizeBtn) && (ShowSysTitle)) { btns.Add(piMinimizeBox); }  
             if ((showSysSetBtn) && (ShowSysTitle)) { btns.Add(piSysSet); }  
             if ((showSysSkinBtn) && (ShowSysTitle)) { btns.Add(piSysSkin); } 
             if ((showSysMenuBtn) && (ShowSysTitle)) { btns.Add(piSysMenu); }  */
            if (piCloseBox.Visible) { btns.Add(piCloseBox); }
            if (piMaximizeBox.Visible) { btns.Add(piMaximizeBox); }
            if (piMinimizeBox.Visible) { btns.Add(piMinimizeBox); }
            if (piSysSet.Visible) { btns.Add(piSysSet); }
            if (piSysSkin.Visible) { btns.Add(piSysSkin); }
            if (piSysMenu.Visible) { btns.Add(piSysMenu); }
            int nTop = 0;
            int nLeft = pTitle.Width - 0;
            for (int i = 0; i < btns.Count(); i++)
            {
                PictureBox pbtn = btns[i]; 
                pbtn.Visible = true;
                if (pbtn == piCloseBox)
                {
                    pbtn.Size = new Size(30, 26);
                    pbtn.MaximumSize = new Size(30, 26);
                    pbtn.MinimumSize = new Size(30, 26);
                }
                else
                {
                    pbtn.Size = new Size(26, 26);
                    pbtn.MaximumSize = new Size(26, 26);
                    pbtn.MinimumSize = new Size(26, 26);
                }
                nLeft -= pbtn.Width;
                pbtn.Location = new Point(nLeft, nTop);
                pbtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                pbtn.BringToFront();
            }
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
        PictureBox pbtn = null;

        private void piBtn_MouseEnter(object sender, EventArgs e)
        {
            pbtn = (sender as PictureBox);
            if (pbtn == piCloseBox)
            {
                piCloseBox.BackColor = Color.Red;
            }
            else
            {
                (sender as PictureBox).BackColor = Color.FromArgb(30, (sender as PictureBox).BackColor);
            }
        }

        private void piBtn_MouseLeave(object sender, EventArgs e)
        {
            pbtn = null;
            (sender as PictureBox).BackColor = Color.Transparent;
        }

        private void piBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (pbtn == piCloseBox)
            {
                (sender as PictureBox).BackColor = Color.DarkRed;  
            }
            else
            {
                (sender as PictureBox).BackColor = Color.FromArgb(50, (sender as PictureBox).BackColor);
            } 
        }

        private void piBtn_MouseUp(object sender, MouseEventArgs e)
        {
            pbtn = null;
            (sender as PictureBox).BackColor = Color.Transparent;
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
            pbtn = null;
        }

        //最大化
        private void piMaximizeBox_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            { 
                Rectangle area = Screen.FromHandle(this.Handle).WorkingArea; 
                area.X = 0; 
                this.MaximizedBounds = area; 
                this.MaximumSize = area.Size;

                this.WindowState = FormWindowState.Maximized;
                this.piMaximizeBox.Invalidate();
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            pbtn = null;
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
            map.DrawLine(p, 5, 15, 18, 15);//在画板上画直线      
        }

        private void drawMaximize(Graphics map)
        {
            map.SmoothingMode = SmoothingMode.AntiAlias;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 1); 
            map.DrawRectangle(p, 7, 8, 10, 10);
        }

        private void drawRestore(Graphics map)
        {
            int iWidth = 8;  //矩形边长
            int iLen = 3;    //相隔距离 
            int iBeginL = 6;  //起始左位置 
            int iBeginT = 10;  //起始上位置
            map.SmoothingMode = SmoothingMode.AntiAlias;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            map.CompositingQuality = CompositingQuality.HighQuality;
            map.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen p = new Pen(SysButtonColor, 1); 
            map.DrawRectangle(p, iBeginL, iBeginT, iWidth, iWidth);
            map.DrawLine(p, iBeginL + iLen, iBeginT - iLen, iBeginL + iLen, iBeginT);
            map.DrawLine(p, iBeginL + iLen, iBeginT - iLen, iBeginL + iLen + iWidth, iBeginT - iLen);
            map.DrawLine(p, iBeginL + iLen + iWidth, iBeginT - iLen, iBeginL + iLen + iWidth, iBeginT - iLen + iWidth);
            map.DrawLine(p, iBeginL + iLen + iWidth, iBeginT - iLen + iWidth, iBeginL + iWidth, iBeginT - iLen + iWidth);
        }

        private void drawClose(Graphics map)
        {
            //19*17
            int nLeft = 9;
            map.SmoothingMode = SmoothingMode.HighQuality;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic; 
            Pen p = new Pen(SysButtonColor, 2); 
            map.DrawLine(p, nLeft, 7, nLeft + 10, 17);
            map.DrawLine(p, nLeft, 17, nLeft + 10, 7);
        }

        private void drawSysSet(Graphics map)
        { 
            map.SmoothingMode = SmoothingMode.HighQuality;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic; 
            Pen p = new Pen(SysButtonColor, 1); 
            map.DrawEllipse(p, new Rectangle(10, 11, 4, 4));
            int b = 7;  //边长
            int x0 = 6;
            int x1 = x0 + (int)Math.Round(b * Math.Sin(60* Math.PI/180));
            int x2 = x0 + (int)Math.Round(2 * b*Math.Sin(60 * Math.PI / 180));
            int y0 = 6;
            int y1 = y0+ (int)Math.Round(0.5 * b, 0); 
            int y2 = y1 + b;
            int y3 = y0 + 2 * b; 
            map.DrawPolygon(p, new Point[] { new Point(x0, y1), new Point(x1, y0) , new Point(x2, y1) , new Point(x2, y2) , new Point(x1, y3) , new Point(x0, y2), new Point(x0, y1) } );
        }

        private void drawSysSkin(Graphics map)
        { 
            map.SmoothingMode = SmoothingMode.HighQuality;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic; 
            Pen p = new Pen(SysButtonColor, 1); 
            int b = 3;  //最小边长
            int c = 7;
            int x0 = 5;
            int x1 = x0 + b;
            int x2 = x1 + c;
            int x3 = x2 + b;
            int y0 = 7;
            int y1 = y0 + (int)Math.Round(2*b * Math.Sin(60 * Math.PI / 180));
            int y2 = y1 + 6; 
            map.DrawPolygon(p, new Point[] { new Point(x0, y1), new Point(x1, y0), new Point(x2, y0), new Point(x3, y1), new Point(x2, y1), new Point(x2, y2), new Point(x1, y2), new Point(x1, y1), new Point(x0, y1) });
        }

        private void drawSysMenu(Graphics map)
        {
            map.SmoothingMode = SmoothingMode.HighQuality;
            map.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Pen p = new Pen(SysButtonColor, 1); 
            int x = 6;
            int y = 8;
            map.DrawLine(p, x, y, x + 11, y);
            map.DrawLine(p, x, y + 3, x + 11, y + 3);
            map.DrawLine(p, x, y + 6, x + 11, y + 6);
            map.DrawLine(p, x, y + 9, x + 11, y+9);
        }

        private void piMinimizeBox_Paint(object sender, PaintEventArgs e)
        {
            drawMinimize(e.Graphics);
            if (pbtn != piMinimizeBox)
            {
                piMinimizeBox.BackColor = SysTitleBackColor;
            }
            this.pdeToolTip.SetToolTip(this.piMinimizeBox, "最小化");
        }

        private void piMaximizeBox_Paint(object sender, PaintEventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {
                drawMaximize(e.Graphics);
                this.pdeToolTip.SetToolTip(this.piMaximizeBox, "最大化");
            }
            else
            {
                drawRestore(e.Graphics);
                this.pdeToolTip.SetToolTip(this.piMaximizeBox, "还原");
            }
            if (pbtn != piMaximizeBox)
            {
                piMaximizeBox.BackColor = SysTitleBackColor;
            }
        }

        private void piCloseBox_Paint(object sender, PaintEventArgs e)
        {
            drawClose(e.Graphics);
            if (pbtn != piCloseBox)
            {
                piCloseBox.BackColor = SysTitleBackColor;
            } 
            this.pdeToolTip.SetToolTip(this.piCloseBox, "关闭");
        }

        private void piSysSet_Paint(object sender, PaintEventArgs e)
        {
            drawSysSet(e.Graphics);
            if (pbtn != piSysSet)
            {
                piSysSet.BackColor = SysTitleBackColor;
            }
            this.pdeToolTip.SetToolTip(this.piSysSet, "设置");
        }

        private void piSysSkin_Paint(object sender, PaintEventArgs e)
        {
            drawSysSkin(e.Graphics);
            if (pbtn != piSysSkin)
            {
                piSysSkin.BackColor = SysTitleBackColor;
            }
            this.pdeToolTip.SetToolTip(this.piSysSkin, "皮肤");
        }

        private void piSysMenu_Paint(object sender, PaintEventArgs e)
        {
            drawSysMenu(e.Graphics);
            if (pbtn != piSysMenu)
            {
                piSysMenu.BackColor = SysTitleBackColor;
            }
            this.pdeToolTip.SetToolTip(this.piSysMenu, "菜单");
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

        public void PdeFormMouseMove(object sender, MouseEventArgs e)
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

        private void PTitleMouseDoubleClick(object sender, MouseEventArgs e)
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

        private void piSysMenu_Click(object sender, EventArgs e)
        {
            if (sysMenuStrip != null)
            {
                Point p = new Point(0, piSysMenu.Height);  
                sysMenuStrip.Show(piSysMenu, p, ToolStripDropDownDirection.Default); 
            }
        }

       
    }
}
