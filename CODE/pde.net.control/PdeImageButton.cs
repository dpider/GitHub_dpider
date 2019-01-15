using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace pde.net.control
{
    [DefaultEvent("Click")]
    public partial class PdeImageButton : UserControl
    { 
        public enum ImageButtonState {ibsNormal, ibsHover, ibsMouseDown, ibsSelected};
        public enum ImagePostion { Left, Top, Right, Bottom };

        public PdeImageButton()
        {
            InitializeComponent();
            BorderStyle = BorderStyle.None;
            ButtonImagePostion = ImagePostion.Top;
            picture.BackColor = Color.Transparent;
            buttonimagelayout = PictureBoxSizeMode.CenterImage;
            //默认值属性
            ButtonText = "Caption"; 
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }
   
        private void PdeImageButton_Load(object sender, EventArgs e)
        {
            //事件绑定 
            picture.Click += btn_Click;
            picture.MouseEnter += btn_MouseEnter;
            picture.MouseLeave += btn_MouseLeave;
            picture.MouseDown += btn_MouseDown;
            picture.MouseUp += btn_MouseUp;

            iCaption.Click += btn_Click;
            iCaption.MouseEnter += btn_MouseEnter;
            iCaption.MouseLeave += btn_MouseLeave;
            iCaption.MouseDown += btn_MouseDown;
            iCaption.MouseUp += btn_MouseUp; 
        }
 

        #region ---属性---

        #region 按钮文字
        private int txtHeight;
        private string buttontext;
        [Category("Appearance"), Description("按钮文字")]
        public string ButtonText
        {
            get { return buttontext; }
            set
            {
                buttontext = value;
                iCaption.Text = buttontext;
                iCaption.AutoSize = true;
                txtHeight = iCaption.Height;
                iCaption.AutoSize = false; 
            }
        }
        #endregion

        #region 字体
        private Font font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))); 
        [Category("Appearance"), Description("按钮文字字体")] 
        public Font TextFont
        {
            get { return font; }
            set
            {
                font = value;
                iCaption.Font = font;  
            }
        }
        #endregion
  
        #region 是否显示文字
        private bool showtext = true;
        [Category("Appearance"), Description("是否显示文字")]
        public bool ShowText
        {
            get { return showtext; }
            set
            {
                showtext = value;
                iCaption.Visible = showtext;
            }
        }
        #endregion

        #region 是否显示图片
        private bool showImage = true;
        [Category("Appearance"), Description("是否显示图片")]
        public bool ShowImage
        {
            get { return showImage; }
            set
            {
                showImage = value;
                picture.Visible = showImage;
            }
        }
        #endregion

        #region 按钮图片
        private Image buttonimage;
        [Category("Appearance"), Description("按钮图片")]
        public Image ButtonImage
        {
            get { return buttonimage; }
            set
            {
                buttonimage = value;
                picture.Image = buttonimage;
            }
        }
        #endregion
    
        #region 按钮图片背景颜色
        private Color buttonImagebackColor;
        [Category("Appearance"), Description("按钮图片背景颜色")]
        public Color ButtonImageBackColor
        {
            get { return buttonImagebackColor; }
            set
            {
                buttonImagebackColor = value;
                picture.BackColor = buttonImagebackColor;
            }
        } 
        #endregion
 
        #region 按钮图片布局
        private PictureBoxSizeMode buttonimagelayout;
        [Category("Appearance"), Description("按钮图片布局")]
        public PictureBoxSizeMode ButtonImageLayout
        {
            get { return buttonimagelayout; }
            set
            {
                buttonimagelayout = value;
                picture.SizeMode = buttonimagelayout;
            }
        }
        #endregion

        #region 按钮图片的背景图片
        private Image buttonimageBackgroundImage;
        [Category("Appearance"), Description("按钮图片的背景图片")]
        public Image ButtonImageBackgroundImage
        {
            get { return buttonimageBackgroundImage; }
            set
            {
                buttonimageBackgroundImage = value; 
                picture.BackgroundImage = buttonimageBackgroundImage; 
            }
        }
        #endregion

        #region 按钮图片的背景图片布局
        private ImageLayout buttonImageBackgroundImageLayout;
        [Category("Appearance"), Description("按钮图片的背景图片布局")]
        public ImageLayout ButtonImageBackgroundImageLayout
        {
            get { return buttonImageBackgroundImageLayout; }
            set
            {
                buttonImageBackgroundImageLayout = value;
                picture.BackgroundImageLayout = buttonImageBackgroundImageLayout;
            }
        }
        #endregion

        #region 按钮选中状态
        private bool buttonSelected = false;
        [Category("Appearance"), Description("按钮选中状态")]
        public bool ButtonSelected
        {
            get { return buttonSelected; }
            set { buttonSelected = value; }
        }
        #endregion

        #region 图片相对于文字的位置
        [Category("Appearance"), Description("图片相对于文字的位置")] 
        private ImagePostion buttonimagepostion = ImagePostion.Top;
        public ImagePostion ButtonImagePostion
        {
            get { return buttonimagepostion; }
            set
            {
                buttonimagepostion = value;  
                switch (buttonimagepostion)
                {
                    case ImagePostion.Left:
                        picture.Dock = DockStyle.Left;
                        break;

                    case ImagePostion.Right:
                        picture.Dock = DockStyle.Right;
                        break;

                    case ImagePostion.Bottom:
                        picture.Dock = DockStyle.Bottom;
                        break;

                    default:
                        picture.Dock = DockStyle.Top;
                        break;
                }
                picture.Size = buttonimagesize;
            }
        }
        #endregion

        #region 图片大小
        private Size buttonimagesize = new Size(40, 40);
        [Category("Appearance"), Description("按钮图片大小")]
        public Size ButtonImageSize
        {
            get { return buttonimagesize; }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Width > Width) value.Width = Width;
                if (value.Height < 0) value.Height = 0;
                if (value.Height > Height) value.Height = Height;

                buttonimagesize = value;
                picture.Size = buttonimagesize; 
            }
        }
        #endregion

        #region 按钮文字提示
        private string tip;
        [Category("Appearance"), Description("按钮文字提示")]
        public string ToolTip
        {
            get { return tip; }
            set
            {
                tip = value;
                if (!"".Equals(tip))
                {
                    toolTip1.SetToolTip(iCaption, tip);
                    toolTip1.SetToolTip(picture, tip);
                    toolTip1.SetToolTip(this, tip);
                }
                else
                {
                    toolTip1.RemoveAll();
                }
            }
        }
        #endregion

        #endregion


        #region  事件

        //建立一个函数  
        private void callObjectEvent(Object obj, string EventName, EventArgs e = null)
        {
            //建立一个类型      
            //Type t = typeof(obj.GetType);  
            Type t = Type.GetType(obj.GetType().AssemblyQualifiedName);
            //产生方法      
            MethodInfo m = t.GetMethod(EventName, BindingFlags.NonPublic | BindingFlags.Instance);
            //参数赋值。传入函数      
            //获得参数资料  
            ParameterInfo[] para = m.GetParameters();
            //根据参数的名字，拿参数的空值。  
            //参数对象      
            object[] p = new object[1];
            if (e == null)
                p[0] = Type.GetType(para[0].ParameterType.BaseType.FullName).GetProperty("Empty");
            else
                p[0] = e;
            //调用  
            m.Invoke(obj, p);
            return;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            callObjectEvent(this, "OnClick");
        }

        bool isEnter = false;
        private void btn_MouseEnter(object sender, EventArgs e)
        {
            if (!isEnter)
            {
                isEnter = true;
                callObjectEvent(this, "OnMouseEnter");
            }
            
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            isEnter = false;
            callObjectEvent(this, "OnMouseLeave");
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            callObjectEvent(this, "OnMouseDown");
        }
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            callObjectEvent(this, "OnMouseUp");
        } 
        #endregion
         
    }

}
