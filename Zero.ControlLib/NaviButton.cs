using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    [DefaultEvent("NaviButtonClick")]
    public partial class NaviButton : UserControl
    {
        public NaviButton()
        {
            InitializeComponent();
        }

        private Image naviImage=Properties.Resources.main;

        public Image NaviImage
        {
            get { return naviImage; }
            set { 
                naviImage = value;
                pic_Main.Image = naviImage;
            }
        }

        private string naviName="主界面";

        public string NaviName
        {
            get { return naviName; }
            set { 
                naviName = value;
                lbl_NaviName.Text = naviName;
            }
        }

        // property isActive
        private bool isActive = false;
        [Browsable(true)]
        [Description("是否激活")]
        [Category("自定义属性")]
        public bool IsActive
        { get { return isActive; } set
            {
                isActive = value;
                this.Invalidate();
            }
        }

        //property activeColor
        private Color activeColor= Color.FromArgb(236, 240, 243);
        [Browsable(true)]
        [Description("激活时的颜色")]
        [Category("自定义属性")]
        public Color ActiveColor
        {
            get { return activeColor; }
            set { 
                activeColor = value;
                this.Invalidate();
            }
        }
        //property activeHeight
        private int activeHeight = 4;
        [Browsable(true)]
        [Description("激活方块高度")]
        [Category("自定义属性")]
        public int ActiveHeight
        {
            get { return activeHeight; }
            set
            {
                activeHeight = value;
                this.Invalidate();
            }
        }

        private int activeGap = 0;
        [Browsable(true)]
        [Description("激活方块边距")]
        [Category("自定义属性")]
        public int ActiveGap
        {
            get { return activeGap; }
            set
            {
                activeGap = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Description("悬浮渐变色系数")]
        [Category("自定义属性")]
        public float ColorDepth { get; set; } = -0.2f;

        //重写 OnPaint 方法
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (isActive)
            {
                //画激活方块
                Brush brush = new SolidBrush(activeColor);
                e.Graphics.FillRectangle(brush, activeGap, this.Height - activeHeight, this.Width-2*activeGap, activeHeight);
            }
            else
            {
                //画悬浮渐变方块
                Brush brush = new SolidBrush(this.BackColor);
                e.Graphics.FillRectangle(brush, activeGap, this.Height - activeHeight, this.Width - 2 * activeGap, activeHeight);
            }
        }

        // 自定义单击事件
        public event EventHandler NaviButtonClick;

        private void lbl_NaviName_Click(object sender, EventArgs e)
        {
            NaviButtonClick?.Invoke(this, e);
        }
    }
}
