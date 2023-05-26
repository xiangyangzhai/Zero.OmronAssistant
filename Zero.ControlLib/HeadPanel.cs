using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class HeadPanel : Panel
    {
        public HeadPanel()
        {
            InitializeComponent();

            this.stringFormat.Alignment = StringAlignment.Center;
            this.stringFormat.LineAlignment = StringAlignment.Center;
        }


        private StringFormat stringFormat = new StringFormat();

        private Color themeColor=Color.LimeGreen;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取主题颜色")]
        public Color ThemeColor
        {
            get { return themeColor; }
            set {
                themeColor = value;
                this.Invalidate();
            }
        }

        private string   titleText = "HeadPanel";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取标题文本")]
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                this.Invalidate();
            }
        }


        private Color themeForeColor = Color.White;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取主题前景色")]
        public Color ThemeForeColor
        {
            get { return themeForeColor; }
            set
            {
                themeForeColor = value;
                this.Invalidate();
            }
        }


        private int titleHeight = 30;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取标题高度")]
        public int TitleHeight
        {
            get { return titleHeight; }
            set
            {
                titleHeight = value;
                this.Invalidate();
            }
        }

        private int borderWidth = 1;
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

        private Color borderColor = Color.Black;
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

        private float linearGradientRate = 0.5f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取渐变系数")]
        public float LinearGradientRate
        {
            get { return linearGradientRate; }
            set
            {
                linearGradientRate = value;
                this.Invalidate();
            }
        }


        private ContentAlignment textAlignment =ContentAlignment.MiddleCenter;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取文本位置")]
        public ContentAlignment TextAlignment
        {
            get { return textAlignment; }
            set
            {
                textAlignment = value;

                switch (textAlignment)
                {
                    case ContentAlignment.TopLeft:
                        this.stringFormat.Alignment = StringAlignment.Near;
                        this.stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopCenter:
                        this.stringFormat.Alignment = StringAlignment.Center;
                        this.stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopRight:
                        this.stringFormat.Alignment = StringAlignment.Far;
                        this.stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleLeft:
                        this.stringFormat.Alignment = StringAlignment.Near;
                        this.stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleCenter:
                        this.stringFormat.Alignment = StringAlignment.Center;
                        this.stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleRight:
                        this.stringFormat.Alignment = StringAlignment.Far;
                        this.stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomLeft:
                        this.stringFormat.Alignment = StringAlignment.Near;
                        this.stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomCenter:
                        this.stringFormat.Alignment = StringAlignment.Center;
                        this.stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomRight:
                        this.stringFormat.Alignment = StringAlignment.Far;
                        this.stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    default:
                        break;
                }

                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rectangle = new Rectangle(0, 0, this.Width, this.titleHeight);

            //绘制标题栏

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, GetColorLight(this.themeColor,linearGradientRate),this.themeColor,LinearGradientMode.Horizontal);

            graphics.FillRectangle(linearGradientBrush, rectangle);

            //绘制标题文本

            graphics.DrawString(this.titleText, this.Font, new SolidBrush(this.themeForeColor), rectangle, stringFormat);


            //绘制边框
            graphics.DrawRectangle(new Pen(borderColor, borderWidth), borderWidth * 0.5f, borderWidth * 0.5f,  this.Width - borderWidth, this.Height - borderWidth);

            //绘制一条线
            graphics.DrawLine(new Pen(borderColor, 1), 0, this.titleHeight, this.Width, this.titleHeight);
        }


        private Color GetColorLight(Color color, float linearGradientRate)
        {
            return Color.FromArgb(Convert.ToInt32(color.R + (255 - color.R) * linearGradientRate), Convert.ToInt32(color.G + (255 - color.G) * linearGradientRate), Convert.ToInt32(color.B + (255 - color.B) * linearGradientRate));
        }

    }
}
