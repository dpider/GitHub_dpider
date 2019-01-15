using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using pde.pub;
using System.Drawing.Imaging;

namespace pde.net.control
{
    public partial class TextBoxEx : System.Windows.Forms.TextBox
    {
        #region 私有变量
        private uPictureBox myPictureBox;
        private bool myUpToDate = false;
        private bool myCaretUpToDate = false;
        private Bitmap myBitmap;
        private Bitmap myAlphaBitmap;
        private int myFontHeight = 10;
        private Timer myTimer1;
        private bool myCaretState = true;
        private bool myPaintedFirstTime = false;
        private Color myBackColor = Color.White;
        private int myBackAlpha = 10;
        private bool _HotTrack = true;   // 是否启用热点效果  
        private Color _BorderColor = Color.Black;  // 边框颜色  
        private Color _HotColor = Color.FromArgb(0x33, 0x5E, 0xA8);  // 热点边框颜色 
        private bool _IsMouseOver = false;// 是否鼠标MouseOver状态 
        #endregion

        public TextBoxEx()
        {
            InitializeComponent();
            this.BackColor = myBackColor;
            this.SetStyle(ControlStyles.UserPaint, false);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            myPictureBox = new uPictureBox();
            this.Controls.Add(myPictureBox);
            myPictureBox.Dock = DockStyle.Fill;
        }


        #region 重写公共方法、属性
        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);
            this.myBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(this.Width,this.Height);
            this.myAlphaBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(this.Width,this.Height);
            myUpToDate = false;
            this.Invalidate();
        }


        //Some of these should be moved to the WndProc later

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            myUpToDate = false;
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            myUpToDate = false;
            this.Invalidate();

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            myUpToDate = false;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Invalidate();
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            base.OnGiveFeedback(gfbevent);
            myUpToDate = false;
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //鼠标状态 
            this._IsMouseOver = true;
            //如果启用HotTrack，则开始重绘 
            //如果不加判断这里不加判断，则当不启用HotTrack， 
            //鼠标在控件上移动时，控件边框会不断重绘， 
            //导致控件边框闪烁。下同 
            //谁有更好的办法？Please tell me , Thanks。 
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //found this code to find the current cursor location
            //at http://www.syncfusion.com/FAQ/WinForms/FAQ_c50c.asp#q597q

            this._IsMouseOver = false;
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            Point ptCursor = Cursor.Position;

            Form f = this.FindForm();
            ptCursor = f.PointToClient(ptCursor);
            if (!this.Bounds.Contains(ptCursor))
                base.OnMouseLeave(e);
        }

        protected override void OnChangeUICues(UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
            myUpToDate = false;
            this.Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            myCaretUpToDate = false;
            myUpToDate = false;
            this.Invalidate();


            myTimer1 = new System.Windows.Forms.Timer(this.components);
            myTimer1.Interval = (int)Win32.GetCaretBlinkTime(); //  usually around 500;

            myTimer1.Tick += new EventHandler(myTimer1_Tick);
            myTimer1.Enabled = true;

        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            myCaretUpToDate = false;
            myUpToDate = false;
            this.Invalidate();
            myTimer1.Dispose();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            if (this.myPaintedFirstTime)
                this.SetStyle(ControlStyles.UserPaint, false);

            base.OnFontChanged(e);

            if (this.myPaintedFirstTime)
                this.SetStyle(ControlStyles.UserPaint, true);


            myFontHeight = GetFontHeight();


            myUpToDate = false;
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            myUpToDate = false;
            this.Invalidate();
        }
         
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);
            // need to rewrite as a big switch

            if (m.Msg == Win32.WM_PAINT)
            {
                myPaintedFirstTime = true;

                if (!myUpToDate || !myCaretUpToDate)
                    GetBitmaps();
                myUpToDate = true;
                myCaretUpToDate = true;

                if (myPictureBox.Image != null) myPictureBox.Image.Dispose();
                myPictureBox.Image = (Image)myAlphaBitmap.Clone();  
            }

            else if (m.Msg == Win32.WM_HSCROLL || m.Msg == Win32.WM_VSCROLL)
            {
                myUpToDate = false;
                this.Invalidate();
            }

            else if (m.Msg == Win32.WM_LBUTTONDOWN
                || m.Msg == Win32.WM_RBUTTONDOWN
                || m.Msg == Win32.WM_LBUTTONDBLCLK
                //  || m.Msg == win32.WM_MOUSELEAVE  ///****
                )
            {
                myUpToDate = false;
                this.Invalidate();
            }

            else if (m.Msg == Win32.WM_MOUSEMOVE)
            {
                if (m.WParam.ToInt32() != 0)  //shift key or other buttons
                {
                    myUpToDate = false;
                    this.Invalidate();
                }
            }
            //System.Diagnostics.Debug.WriteLine("Pro: " + m.Msg.ToString("X")); 
        }

        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set
            {
                if (this.myPaintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, false);

                base.BorderStyle = value;

                if (this.myPaintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, true);

                this.myBitmap = null;
                this.myAlphaBitmap = null;
                myUpToDate = false;
                this.Invalidate();
            }
        }

        public new Color BackColor
        {
            get
            {
                return Color.FromArgb(base.BackColor.R, base.BackColor.G, base.BackColor.B);
            }
            set
            {
                myBackColor = value;
                base.BackColor = value;
                myUpToDate = false;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                myUpToDate = false;
            }
        }

        public override bool Multiline
        {
            get { return base.Multiline; }
            set
            {
                if (this.myPaintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, false);

                base.Multiline = value;

                if (this.myPaintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, true);

                this.myBitmap = null;
                this.myAlphaBitmap = null;
                myUpToDate = false;
                this.Invalidate();
            }
        } 
        #endregion

        #region 私有方法
        private int GetFontHeight()
        {
            Graphics g = this.CreateGraphics();
            SizeF sf_font = g.MeasureString("X", this.Font);
            g.Dispose();
            return (int)sf_font.Height;
        }


        private void GetBitmaps()
        {

            if (myBitmap == null
                || myAlphaBitmap == null
                || myBitmap.Width != Width
                || myBitmap.Height != Height
                || myAlphaBitmap.Width != Width
                || myAlphaBitmap.Height != Height)
            {
                myBitmap = null;
                myAlphaBitmap = null;
            }



            if (myBitmap == null)
            {
                myBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(Width,Height);
                myUpToDate = false;
            }


            if (!myUpToDate)
            {
                //Capture the TextBox control window

                this.SetStyle(ControlStyles.UserPaint, false);

                Win32.CaptureWindow(this, ref myBitmap);

                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.FromArgb(myBackAlpha, myBackColor);
            }
          

            Rectangle r2 = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
            ImageAttributes tempImageAttr = new ImageAttributes();


            //Found the color map code in the MS Help

            ColorMap[] tempColorMap = new ColorMap[1];
            tempColorMap[0] = new ColorMap();
            tempColorMap[0].OldColor = Color.FromArgb(255, myBackColor);
            tempColorMap[0].NewColor = Color.FromArgb(myBackAlpha, myBackColor);

            tempImageAttr.SetRemapTable(tempColorMap);

            if (myAlphaBitmap != null)
                myAlphaBitmap.Dispose();


            myAlphaBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(Width,Height);

            Graphics tempGraphics1 = Graphics.FromImage(myAlphaBitmap);

            tempGraphics1.DrawImage(myBitmap, r2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, GraphicsUnit.Pixel, tempImageAttr);

            #region 绘制边框 
            if (this.BorderStyle == BorderStyle.FixedSingle)
            {
                Pen pen = new Pen(this.BorderColor, 1);
                if (this._HotTrack)
                {
                    if (this.Focused)
                    {
                        pen.Color = this._HotColor;
                    }
                    else
                    {
                        if (this._IsMouseOver)
                        {
                            pen.Color = this._HotColor;
                        }
                        else
                        {
                            pen.Color = this._BorderColor;
                        }
                    }
                }
                tempGraphics1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                tempGraphics1.DrawRectangle(pen, 0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                pen.Dispose();
            }
            #endregion

            tempGraphics1.Dispose();
             
            if (this.Focused && (this.SelectionLength == 0))
            {
                Graphics tempGraphics2 = Graphics.FromImage(myAlphaBitmap);
                if (myCaretState)
                {
                    //Draw the caret
                    Point caret = this.findCaret();
                    Pen p = new Pen(this.ForeColor, 3);
                    tempGraphics2.DrawLine(p, caret.X, caret.Y + 0, caret.X, caret.Y + myFontHeight);
                    tempGraphics2.Dispose();
                }
            }
        }
         
        private Point findCaret()
        {
            /*  Find the caret translated from code at 
			 * http://www.vb-helper.com/howto_track_textbox_caret.html
			 * 
			 * and 
			 * 
			 * http://www.microbion.co.uk/developers/csharp/textpos2.htm
			 * 
			 * Changed to EM_POSFROMCHAR
			 * 
			 * This code still needs to be cleaned up and debugged
			 * */

            Point pointCaret = new Point(0);
            int i_char_loc = this.SelectionStart;
            IntPtr pi_char_loc = new IntPtr(i_char_loc);

            int i_point = Win32.SendMessage(this.Handle, Win32.EM_POSFROMCHAR, pi_char_loc, IntPtr.Zero);
            pointCaret = new Point(i_point);

            if (i_char_loc == 0)
            {
                pointCaret = new Point(0);
            }
            else if (i_char_loc >= this.Text.Length)
            {
                pi_char_loc = new IntPtr(i_char_loc - 1);
                i_point = Win32.SendMessage(this.Handle, Win32.EM_POSFROMCHAR, pi_char_loc, IntPtr.Zero);
                pointCaret = new Point(i_point);

                Graphics g = this.CreateGraphics();
                String t1 = this.Text.Substring(this.Text.Length - 1, 1) + "X";
                SizeF sizet1 = g.MeasureString(t1, this.Font);
                SizeF sizex = g.MeasureString("X", this.Font);
                g.Dispose();
                int xoffset = (int)(sizet1.Width - sizex.Width);
                pointCaret.X = pointCaret.X + xoffset;

                if (i_char_loc == this.Text.Length)
                {
                    String slast = this.Text.Substring(Text.Length - 1, 1);
                    if (slast == "\n")
                    {
                        pointCaret.X = 1;
                        pointCaret.Y = pointCaret.Y + myFontHeight;
                    }
                }
            }
            return pointCaret;
        }

        private void myTimer1_Tick(object sender, EventArgs e)
        {
            //Timer used to turn caret on and off for focused control

            myCaretState = !myCaretState;
            myCaretUpToDate = false;
            this.Invalidate();
        }
        #endregion

        #region 私有类 uPictureBox
        private class uPictureBox : PictureBox
        {
            public uPictureBox()
            {
                this.SetStyle(ControlStyles.Selectable, false);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);

                this.Cursor = null;
                this.Enabled = true;
                this.SizeMode = PictureBoxSizeMode.Normal;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == Win32.WM_LBUTTONDOWN
                    || m.Msg == Win32.WM_RBUTTONDOWN
                    || m.Msg == Win32.WM_LBUTTONDBLCLK
                    || m.Msg == Win32.WM_MOUSELEAVE
                    || m.Msg == Win32.WM_MOUSEMOVE)
                {
                    //Send the above messages back to the parent control
                    Win32.PostMessage(this.Parent.Handle, (uint)m.Msg, m.WParam, m.LParam);
                }

                else if (m.Msg == Win32.WM_LBUTTONUP)
                {
                    //??  for selects and such
                    this.Parent.Invalidate();
                }
                base.WndProc(ref m);
            }
        }
        #endregion

        #region 公共属性

        [
             Category("外观"),
             Description("设置背景颜色的透明度，取值范围从 0 到 255。"),
             Browsable(true),
             DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public int BackAlpha
        {
            get { return myBackAlpha; }
            set
            {
                int v = value;
                if (v > 255)
                    v = 255;
                myBackAlpha = v;
                myUpToDate = false;
                Invalidate();
            }
        }

        [
            Category("行为"),
            Description("获得或设置一个值，指示当鼠标经过控件时控件边框是否发生变化。只在控件的BorderStyle为FixedSingle时有效"),
            Browsable(true),
            DefaultValue(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool HotTrack
        {
            get
            {
                return this._HotTrack;
            }
            set
            {
                this._HotTrack = value;
                //在该值发生变化时重绘控件，下同 
                //在设计模式下，更改该属性时，如果不调用该语句， 
                //则不能立即看到设计试图中该控件相应的变化 
                myUpToDate = false;
                this.Invalidate();
            }
        }
        /// <summary> 
        /// 边框颜色 
        /// </summary> 
        [
            Category("外观"),
            Description("获得或设置控件的边框颜色"),
            Browsable(true),
            DefaultValue(typeof(Color), "#A7A6AA"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public Color BorderColor
        {
            get
            {
                return this._BorderColor;
            }
            set
            {
                this._BorderColor = value;
                myUpToDate = false;
                this.Invalidate();
            }
        }
        /// <summary> 
        /// 热点时边框颜色 
        /// </summary> 
        [
            Category("外观"),
            Description("获得或设置当鼠标经过控件时控件的边框颜色。只在控件的BorderStyle为FixedSingle时有效"),
            Browsable(true),
            DefaultValue(typeof(Color), "#335EA8"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public Color HotColor
        {
            get
            {
                return this._HotColor;
            }
            set
            {
                this._HotColor = value;
                myUpToDate = false;
                this.Invalidate();
            }
        } 
        #endregion 
    }
}
