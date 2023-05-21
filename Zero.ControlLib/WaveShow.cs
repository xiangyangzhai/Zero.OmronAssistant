using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class WaveShow : UserControl
    {
        public WaveShow()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private Color defaultColor = Color.FromArgb(0, 235, 177);
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

        private Color waveBackColor = Color.FromArgb(9, 96, 115);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取波形背景色")]
        public Color WaveBackColor
        {
            get { return waveBackColor; }
            set
            {
                waveBackColor = value;
                this.Invalidate();
            }
        }

        private float radianRate = 0.03f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取圆弧曲线系数[0-0.2]")]
        public float RadianRate
        {
            get { return radianRate; }
            set
            {
                if (value <= 0) return;
                if (value > 0.2) return;
                radianRate = value;
                this.Invalidate();
            }
        }

        private float rangeMax = 100.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取量程最大值")]
        public float RangeMax
        {
            get { return rangeMax; }
            set
            {
                rangeMax = value;
                this.Invalidate();
            }
        }

        private float rangeMin = 0.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取量程最小值")]
        public float RangeMin
        {
            get { return rangeMin; }
            set
            {
                rangeMin = value;
                this.Invalidate();
            }
        }

        private float currentValue = 50.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前值")]
        public float CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;
                this.Invalidate();
            }
        }

        private Color[] colorList = new Color[] { Color.FromArgb(0, 235, 177) };
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取颜色数组")]
        public Color[] ColorList
        {
            get { return colorList; }
            set
            {
                if (value == null) return;
                if (value.Length == 0) return;
                colorList = value;
                this.Invalidate();
            }
        }

        private float[] valueList = new float[] { 1.0f };
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取数值数组")]
        public float[] ValueList
        {
            get { return valueList; }
            set
            {
                if (value == null) return;
                if (value.Length == 0) return;
                valueList = value;
                this.Invalidate();
            }
        }

        private bool isShowValue = true;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取数值数组")]
        public bool IsShowValue
        {
            get { return isShowValue; }
            set
            {
                isShowValue = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            //获取画布
            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


            //转移坐标系
            graphics.TranslateTransform(this.Width / 2.0f, 0.0f);

            //绘制头部

            SolidBrush solidBrush = new SolidBrush(this.waveBackColor);

            float r = radianRate * this.Height;

            graphics.FillEllipse(solidBrush, -this.Width * 0.5f, 1.0f, Width, r * 2);

            //绘制中部

            graphics.FillRectangle(solidBrush, -this.Width * 0.5f, r + 1.0f, this.Width, this.Height - 2 * (r + 1.0f));

            //液体颜色

            float scale = (currentValue - rangeMin) / (rangeMax - rangeMin);

            Color waveColor = GetColorFromValue(scale);

            float ypos = (1.0f - scale) * (this.Height - 2 * (r + 1.0f));

            GraphicsPath graphicsPath = new GraphicsPath();

            //添加贝塞尔曲线
            if (scale > radianRate)
            {
                PointF point1 = new PointF(-Width * 0.5f, ypos + r + 1.0f);
                PointF point2 = new PointF(-Width * 0.25f, ypos + 3 * r + 1.0f);
                PointF point3 = new PointF(Width * 0.25f, ypos - r + 1.0f);
                PointF point4 = new PointF(Width * 0.5f, ypos + r + 1.0f);
                graphicsPath.AddBezier(point1, point2, point3, point4);
            }

            //添加直线
            graphicsPath.AddLine(this.Width * 0.5f, ypos + r + 1.0f, this.Width * 0.5f, this.Height - r - 1.0f);
            graphicsPath.AddArc(new RectangleF(-this.Width*0.5f,this.Height-1.0f-r*2,this.Width,2*r),0,180.0f);
            graphicsPath.AddLine(-this.Width * 0.5f, this.Height - r - 1.0f, -this.Width * 0.5f, ypos + r + 1.0f);

            graphics.FillPath(new SolidBrush(waveColor), graphicsPath);

            if (isShowValue)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment = StringAlignment.Center;    
                stringFormat.Alignment = StringAlignment.Center;    

                graphics.DrawString(Convert.ToInt32(scale * 100) + "%", this.Font, new SolidBrush(this.ForeColor), new RectangleF(-this.Width*0.5f, 0, this.Width, this.Height), stringFormat);
            }
        }

        /// <summary>
        /// 通过数值获取对应的填充颜色
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Color GetColorFromValue(float scale)
        {
            for (int i = 0; i < valueList.Length; i++)
            {
                if (scale < valueList[i])
                {
                    if (colorList.Length > i)
                    {
                        return colorList[i];
                    }
                }
            }

            return defaultColor;
        }

    }
}
