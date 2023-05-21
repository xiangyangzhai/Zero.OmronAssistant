using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Zero.ControlLib
{
    public partial class Led : UserControl
    {
        public Led()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            blinkTimer = new Timer();
            blinkTimer.Interval = 200;
            blinkTimer.Tick += BlinkTimer_Tick;

        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            currentValue++;

            if (currentValue >= colorList.Length)
            {
                currentValue = 0;
            }

            this.Invalidate();
        }

        //如果画布对象经常使用，就可以创建一个成员变量
        private Graphics graphics;
        private Pen pen;
        private Timer blinkTimer;


        private Color defaultColor = Color.Gray;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取默认颜色")]
        public Color DefaultColor
        {
            get { return defaultColor; }
            set
            {
                defaultColor = value;
                this.Invalidate();
            }
        }


        private int gapWidth = 5;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取外环间隙")]
        public int GapWidth
        {
            get { return gapWidth; }
            set
            {
                gapWidth = value;
                this.Invalidate();
            }
        }



        private int borderWidth = 5;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取外环宽度")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                this.Invalidate();
            }
        }

        private bool isBorder = true;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取是否有边框")]
        public bool IsBorder
        {
            get { return isBorder; }
            set
            {
                isBorder = value;
                this.Invalidate();
            }
        }

        private Color[] colorList = new Color[] { Color.Gray, Color.Green };
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取颜色数组集合")]
        public Color[] ColorList
        {
            get { return colorList; }
            set
            {
                colorList = value;
                this.Invalidate();
            }
        }

        private int currentValue;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前值")]
        public int CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;
                this.Invalidate();
            }
        }

        private Color centerColor = Color.White;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取中心颜色")]
        public Color CenterColor
        {
            get { return centerColor; }
            set
            {
                centerColor = value;
                this.Invalidate();
            }
        }

        private bool isHighLight = false;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取是否高亮显示")]
        public bool IsHighLight
        {
            get { return isHighLight; }
            set
            {
                isHighLight = value;
                this.Invalidate();
            }
        }

        private bool isBlink = false;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取是否闪烁")]
        public bool IsBlink
        {
            get { return isBlink; }
            set
            {
                isBlink = value;

                currentValue = 0;
                blinkTimer.Enabled = isBlink;

                this.Invalidate();
            }
        }


        private int blinkVelocity = 200;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取闪烁间隔")]
        public int BlinkVelocity
        {
            get { return blinkVelocity; }
            set
            {
                blinkVelocity = value;

                if (blinkVelocity <= 0) return;
                blinkTimer.Interval = blinkVelocity;
                this.Invalidate();

            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //获取画布
            graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            //先画圆

            Color color = GetColor();

            int width = Math.Min(this.Width, this.Height);

            SolidBrush solidBrush = new SolidBrush(color);
            Rectangle rectangle = new Rectangle(1, 1, width - 2, width - 2);

            graphics.FillEllipse(solidBrush, rectangle);

            float temp = 1 + gapWidth + borderWidth * 0.5f;

            //绘制边框
            if (isBorder)
            {
                pen = new Pen(this.BackColor, borderWidth);

                RectangleF rectangleF = new RectangleF(temp, temp, width - 2 * temp, width - 2 * temp);

                graphics.DrawEllipse(pen, rectangleF);

            }


            //高亮处理

            if (isHighLight)
            {
                //创建一个GraphicsPath
                GraphicsPath graphicsPath = new GraphicsPath();

                //创建一个RectangleF
                RectangleF rectangle1;

                if (isBorder)
                {
                    temp = 1 + gapWidth + borderWidth;
                    rectangle1 = new RectangleF(temp, temp, width - 2 * temp, width - 2 * temp);
                }
                else
                {
                    rectangle1 = new RectangleF(1, 1, width - 2, width - 2);
                }

                //添加椭圆形状
                graphicsPath.AddEllipse(rectangle1);

                //创建一个PathGradientBrush对象
                PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);

                pathGradientBrush.CenterColor = this.centerColor;
                pathGradientBrush.SurroundColors = new Color[] { color };

                graphics.FillPath(pathGradientBrush, graphicsPath);
            }
        }


        /// <summary>
        /// 获取绘制颜色
        /// </summary>
        /// <returns></returns>
        private Color GetColor()
        {
            if (colorList != null && colorList.Length > currentValue)
            {
                return colorList[currentValue];
            }
            else
            {
                return defaultColor;
            }
        }


    }
}
