using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class PanelEx : Panel
    {
        public PanelEx()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

        }

        //GDI+

        //Graphics 

        //Device

        //Interface

        //Plus

        private Color borderColor = Color.FromArgb(35, 255, 253);

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }


        private int borderWidth = 2;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取边框宽度")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //获取Graphics对象
            Graphics graphics = e.Graphics;

            //创建Pen对象
            Pen pen = new Pen(borderColor, borderWidth);

            //创建Rectangle对象
            Rectangle rectangle = new Rectangle(1, 1, this.Width - 2, this.Height - 2);

            //绘制Rectangle矩形边框
            graphics.DrawRectangle(pen, rectangle);
        }

    }
}
