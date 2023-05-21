using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class TextShow : UserControl
    {
        public TextShow()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);


            stringFormat = new StringFormat();
            stringFormat.LineAlignment= StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
        }


        private int borderWidth=2;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取边框宽度")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set {
                borderWidth = value;

                this.Invalidate();
            }
        }


        private Color borderColor = Color.White;
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


        private string currentValue="12.34";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前值")]
        public string CurrentValue
        {
            get { return currentValue; }
            set { 
                currentValue = value; 
                this.Invalidate(); 
            }
        }

        private string unit="℃";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前单位")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.Invalidate();
            }
        }
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取绑定变量名称")]
        public string BindVarName { get; set; }

        //文本的对齐方式
        private StringFormat stringFormat;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;


            //绘制边框

            Rectangle rectangle = new Rectangle(borderWidth / 2, borderWidth / 2, this.Width - borderWidth, this.Height - borderWidth);

            graphics.DrawRectangle(new Pen(borderColor, borderWidth), rectangle);


            //绘制文本

            graphics.DrawString(currentValue + " " + unit, this.Font, new SolidBrush(this.ForeColor), rectangle, stringFormat);
        }

    }
}
