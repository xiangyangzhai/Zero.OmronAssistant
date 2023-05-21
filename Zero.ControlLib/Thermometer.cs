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
    public partial class Thermometer : UserControl
    {
        public Thermometer()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }


        private Color thermometerColor = Color.FromArgb(211, 211, 211);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计的颜色")]
        public Color ThermometerColor
        {
            get { return thermometerColor; }
            set
            {
                thermometerColor = value;
                this.Invalidate();
            }
        }

        private Color liquidColor = Color.FromArgb(255, 77, 59);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计液体的颜色")]
        public Color LiquidColor
        {
            get { return liquidColor; }
            set
            {
                liquidColor = value;
                this.Invalidate();
            }
        }

        private float maxValue = 100.0F;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计量程最大值")]

        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                this.Invalidate();
            }
        }

        private float minValue = 0.0F;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计量程最小值")]

        public float MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                this.Invalidate();
            }
        }


        private float currentValue = 40.0F;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计当前值")]

        public float CurrentValue
        {
            get { return currentValue; }
            set
            {
                if (value > maxValue)
                {
                    value = maxValue;
                }
                if (value < minValue)
                {
                    value = minValue;
                }
                currentValue = value;
                this.Invalidate();
            }
        }


        private int majorSplitCount = 2;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计主刻度数量")]

        public int MajorSplitCount
        {
            get { return majorSplitCount; }
            set
            {
                if (value <= 0) return;

                majorSplitCount = value;
                this.Invalidate();
            }
        }

        private int minorSplitCount = 10;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计次刻度数量")]

        public int MinorSplitCount
        {
            get { return minorSplitCount; }
            set
            {
                if (value <= 0) return;

                minorSplitCount = value;
                this.Invalidate();
            }
        }


        private bool leftVisible = true;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计左刻度是否可见")]

        public bool LeftVisible
        {
            get { return leftVisible; }
            set
            {
                leftVisible = value;
                this.Invalidate();
            }
        }

        private bool rightVisible = true;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计右刻度是否可见")]

        public bool RightVisible
        {
            get { return rightVisible; }
            set
            {
                rightVisible = value;
                this.Invalidate();
            }
        }

        private bool isUnitVisible = false;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计单位是否可见")]

        public bool IsUnitVisible
        {
            get { return isUnitVisible; }
            set
            {
                isUnitVisible = value;
                this.Invalidate();
            }
        }

        private string unit = "℃";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取温度计单位")]

        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.Invalidate();
            }
        }

        private Rectangle recLeft;

        private Rectangle recRight;

        private Rectangle recMiddle;

        private Rectangle recButtom;


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //获取Graphics对象并设置属性
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


            //rectangle
            recMiddle = new Rectangle(this.Width/2-this.Width/8,this.Width/4,this.Width/4,this.Height-this.Width/4*2);

            recLeft = new Rectangle(0,recMiddle.Top+recMiddle.Width/2,this.Width/2-this.Width/8,recMiddle.Height-recMiddle.Width*2);

            recRight = new Rectangle(recMiddle.Right, recMiddle.Top + recMiddle.Width / 2, this.Width / 2 - this.Width / 8, recMiddle.Height - recMiddle.Width * 2);

            //graphics.FillRectangle(Brushes.Black, recMiddle);

            //graphics.FillRectangle(Brushes.Red, recLeft);

            //graphics.FillRectangle(Brushes.Green, recRight);

            //绘制玻璃管
            GraphicsPath graphicsPath = new GraphicsPath();

            //添加第一条直线
            graphicsPath.AddLine(recMiddle.Left,recMiddle.Bottom,recMiddle.Left,recMiddle.Top+recMiddle.Width/2);

            //添加圆弧
            graphicsPath.AddArc(new Rectangle(recMiddle.Left, recMiddle.Top ,recMiddle.Width,recMiddle.Width),180,180);

            //添加第二条直线
            graphicsPath.AddLine(recMiddle.Right, recMiddle.Top + recMiddle.Width / 2, recMiddle.Right, recMiddle.Bottom);

            //闭环
            graphicsPath.CloseAllFigures();

            graphics.FillPath(new SolidBrush(thermometerColor), graphicsPath);

            //recBottom

            //绘制底部
            recButtom = new Rectangle(this.Width/2-recMiddle.Width,this.Height-recMiddle.Width*2-2,recMiddle.Width*2,recMiddle.Width*2);

            graphics.FillEllipse(new SolidBrush(thermometerColor), recButtom);
            
            graphics.FillEllipse(new SolidBrush(liquidColor), new Rectangle(recButtom.Left+4,recButtom.Top+4,recButtom.Width-8,recButtom.Height-8));

            //绘制刻度

            //每个大格子对应的值的范围
            float decSplit = (maxValue - minValue) / majorSplitCount;

            //每个大格子对应的像素范围
            float decSplitHeight = recLeft.Height / majorSplitCount;

            //绘制大刻度
            for (int i = 0; i <= majorSplitCount; i++)
            {
                string value = (minValue + decSplit * i).ToString("0.##") + (isUnitVisible ? unit : "");

                //测量这些值的尺寸
                SizeF size = graphics.MeasureString(value, this.Font);

                //如果左边是可见的
                if (leftVisible)
                {
                    //画直线
                    graphics.DrawLine(new Pen(this.ForeColor, 1), new PointF(recLeft.Left + 2, recLeft.Bottom - i * decSplitHeight), new PointF(recLeft.Right, recLeft.Bottom - i * decSplitHeight));

                    graphics.DrawString(value, this.Font, new SolidBrush(this.ForeColor), new PointF(recLeft.Left + 2, recLeft.Bottom - i * decSplitHeight - size.Height));


                    //绘制小刻度
                    if (i != majorSplitCount)
                    {
                        float decSplitHeight2 = decSplitHeight / minorSplitCount;

                        for (int j = 1; j < minorSplitCount; j++)
                        {
                            float x = j == minorSplitCount / 2 ? recLeft.Right - 10 : recLeft.Right - 5;
                            float y = recLeft.Bottom - i * decSplitHeight - j * decSplitHeight2;

                            graphics.DrawLine(new Pen(this.ForeColor, 1), new PointF(x, y), new PointF(recLeft.Right, y));

                        }
                    } 
                }

                //如果右边是可见的
                if (rightVisible)
                {
                    //画直线
                    graphics.DrawLine(new Pen(this.ForeColor, 1), new PointF(recRight.Left + 2, recRight.Bottom - i * decSplitHeight), new PointF(recRight.Right, recRight.Bottom - i * decSplitHeight));

                    graphics.DrawString(value, this.Font, new SolidBrush(this.ForeColor), new PointF(recRight.Right - 2-size.Width, recRight.Bottom - i * decSplitHeight - size.Height));


                    //绘制小刻度
                    if (i != majorSplitCount)
                    {
                        float decSplitHeight2 = decSplitHeight / minorSplitCount;

                        for (int j = 1; j < minorSplitCount; j++)
                        {
                            float x = j == minorSplitCount / 2 ? recRight.Left + 10 : recRight.Left + 5;
                            float y = recRight.Bottom - i * decSplitHeight - j * decSplitHeight2;

                            graphics.DrawLine(new Pen(this.ForeColor, 1), new PointF(recRight.Left, y), new PointF(x, y));

                        }
                    }
                }


                //绘制液体位置

                float liquidHeight = (currentValue - minValue) / (maxValue - minValue) * recLeft.Height;

                RectangleF rectangleF = new RectangleF(recMiddle.Left + 4, recLeft.Top + (recLeft.Height - liquidHeight), recMiddle.Width - 8, recMiddle.Height - (recLeft.Height - liquidHeight) - recMiddle.Width / 2);

                graphics.FillRectangle(new SolidBrush(liquidColor), rectangleF);

                //绘制数据文字

                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment= StringAlignment.Center;
                stringFormat.Alignment= StringAlignment.Center;

                graphics.DrawString(Convert.ToInt32(currentValue).ToString(), this.Font, new SolidBrush(Color.White), recButtom, stringFormat);


            }
        }
    }
}
