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

    public enum ControlType
    {
        /// <summary>
        /// 椭圆
        /// </summary>
        Ellipse,
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 直线
        /// </summary>
        Line

    }

    [DefaultEvent("CheckedChanged")]
    public partial class Toggle : UserControl
    {
        public Toggle()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.MouseDown += Toggle_MouseDown;
        }

        private void Toggle_MouseDown(object sender, MouseEventArgs e)
        {
            IsChecked = !IsChecked;

            CheckedChanged?.Invoke(this, e);    
        }

        private ControlType controlType = ControlType.Ellipse;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件的样式")]
        public ControlType ControlType
        {
            get { return controlType; }
            set
            {
                controlType = value;

                this.Invalidate();
            }
        }


        private Color trueColor = Color.FromArgb(73, 119, 232);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件选中的颜色")]
        public Color TrueColor
        {
            get { return trueColor; }
            set
            {
                trueColor = value;

                this.Invalidate();
            }
        }


        private Color falseColor = Color.FromArgb(180, 180, 180);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件未选中的颜色")]
        public Color FalseColor
        {
            get { return falseColor; }
            set
            {
                falseColor = value;

                this.Invalidate();
            }
        }


        private bool isChecked = false;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件是否选中")]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.Invalidate();
            }
        }




        private string trueText = string.Empty;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件选中的文本")]
        public string TrueText
        {
            get { return trueText; }
            set
            {
                trueText = value;
                this.Invalidate();
            }
        }

        private string falseText = string.Empty;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取控件未选中的文本")]
        public string FalseText
        {
            get { return falseText; }
            set
            {
                falseText = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


            //获取填充颜色
            Color fillColor = isChecked ? trueColor : falseColor;

            //创建路径
            GraphicsPath path = new GraphicsPath();

            //文本
            string text = isChecked ? trueText : falseText;

            //文件尺寸
            SizeF size = graphics.MeasureString(text, this.Font);

            //间隙
            int gap = 2;

            //小圆宽度
            int border = 2;

            switch (controlType)
            {
                case ControlType.Ellipse:

                    float r = (this.Height - 2) * 0.5f;

                    //添加直线
                    path.AddLine(new Point(this.Height / 2, 1), new Point(this.Width - this.Height / 2, 1));
                    //添加圆弧
                    path.AddArc(new RectangleF(this.Width - 2 * r - 1, 1, 2 * r, 2 * r), -90, 180);
                    //添加直线
                    path.AddLine(new PointF(this.Width - r - 1, this.Height - 1), new Point(this.Height / 2, this.Height - 1));
                    //添加圆弧
                    path.AddArc(new RectangleF(1, 1, 2 * r, 2 * r), 90, 180);

                    graphics.FillPath(new SolidBrush(fillColor), path);

                    //大圆半径
                    float radius = (this.Height - 2 * (1 + gap)) * 0.5f;

                    //如果选中如何绘制
                    if (isChecked)
                    {
                        graphics.FillEllipse(Brushes.White, new RectangleF(this.Width - 2 * radius - gap - 1, gap + 1, 2 * radius, 2 * radius));

                        //如果文本为空，则画小圆，否则，写文字
                        if (string.IsNullOrEmpty(text))
                        {
                            graphics.DrawEllipse(new Pen(Color.White, border), new RectangleF(this.Height * 0.5f - radius * 0.5f, this.Height * 0.5f - radius * 0.5f, radius, radius));
                        }
                        else
                        {
                            graphics.DrawString(text, this.Font, Brushes.White, new PointF(this.Height * 0.5f - size.Height * 0.5f, this.Height * 0.5f - size.Height * 0.5f));
                        }
                    }
                    else
                    {
                        graphics.FillEllipse(Brushes.White, new RectangleF(gap + 1, gap + 1, 2 * radius, 2 * radius));

                        //如果文本为空，则画小圆，否则，写文字
                        if (string.IsNullOrEmpty(text))
                        {
                            graphics.DrawEllipse(new Pen(Color.White, border), new RectangleF(this.Width - (1.5f * radius + 1), this.Height * 0.5f - radius * 0.5f, radius, radius));
                        }
                        else
                        {
                            graphics.DrawString(text, this.Font, Brushes.White, new PointF(this.Width - (this.Height * 0.5f - size.Height * 0.5f) - size.Width, this.Height * 0.5f - size.Height * 0.5f));
                        }
                    }
                    break;
                case ControlType.Rectangle:

                    int intRadius = 3;

                    //添加圆弧1
                    path.AddArc(new RectangleF(1, 1, intRadius * 2, intRadius * 2), 180, 90);
                    //添加圆弧2
                    path.AddArc(new RectangleF(this.Width - 2 * intRadius - 1, 1, intRadius * 2, intRadius * 2), 270, 90);
                    //添加圆弧3
                    path.AddArc(new RectangleF(this.Width - 2 * intRadius - 1, this.Height - 2 * intRadius - 1, intRadius * 2, intRadius * 2), 0, 90);
                    //添加圆弧4
                    path.AddArc(new RectangleF(1, this.Height - 2 * intRadius - 1, intRadius * 2, intRadius * 2), 90, 90);

                    graphics.FillPath(new SolidBrush(fillColor), path);

                    if (isChecked)
                    {
                        path = new GraphicsPath();

                        int inwidth = this.Height - 2 * (gap + 1);

                        //添加圆弧1
                        path.AddArc(new RectangleF(this.Width - inwidth - gap - 1, 1 + gap, intRadius * 2, intRadius * 2), 180, 90);
                        //添加圆弧2
                        path.AddArc(new RectangleF(this.Width - 1 - gap - 2 * intRadius, 1 + gap, intRadius * 2, intRadius * 2), 270, 90);
                        //添加圆弧3
                        path.AddArc(new RectangleF(this.Width - 1 - gap - 2 * intRadius, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 0, 90);
                        //添加圆弧4
                        path.AddArc(new RectangleF(this.Width - inwidth - gap - 1, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 90, 90);

                        graphics.FillPath(Brushes.White, path);

                        radius = inwidth * 0.5f;

                        //如果文本为空，则画小圆，否则，写文字
                        if (string.IsNullOrEmpty(text))
                        {
                            graphics.DrawEllipse(new Pen(Color.White, border), new RectangleF(this.Height * 0.5f - radius * 0.5f, this.Height * 0.5f - radius * 0.5f, radius, radius));
                        }
                        else
                        {
                            graphics.DrawString(text, this.Font, Brushes.White, new PointF(this.Height * 0.5f - size.Height * 0.5f, this.Height * 0.5f - size.Height * 0.5f));
                        }
                    }
                    else
                    {
                        path = new GraphicsPath();

                        int inwidth = this.Height - 2 * (gap + 1);

                        //添加圆弧1
                        path.AddArc(new RectangleF(1 + gap, 1 + gap, intRadius * 2, intRadius * 2), 180, 90);
                        //添加圆弧2
                        path.AddArc(new RectangleF(inwidth + 1 + gap - 2 * intRadius, 1 + gap, intRadius * 2, intRadius * 2), 270, 90);
                        //添加圆弧3
                        path.AddArc(new RectangleF(inwidth + 1 + gap - 2 * intRadius, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 0, 90);
                        //添加圆弧4
                        path.AddArc(new RectangleF(1 + gap, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 90, 90);

                        graphics.FillPath(Brushes.White, path);

                        radius = inwidth * 0.5f;

                        //如果文本为空，则画小圆，否则，写文字
                        if (string.IsNullOrEmpty(text))
                        {
                            graphics.DrawEllipse(new Pen(Color.White, border), new RectangleF(this.Width - (1.5f * radius + 1), this.Height * 0.5f - radius * 0.5f, radius, radius));
                        }
                        else
                        {
                            graphics.DrawString(text, this.Font, Brushes.White, new PointF(this.Width - (this.Height * 0.5f - size.Height * 0.5f) - size.Width, this.Height * 0.5f - size.Height * 0.5f));
                        }
                    }

                    break;
                case ControlType.Line:

                    float bigRadius = (this.Height - 2) * 0.5f;
                    float smallRadius = bigRadius * 0.5f;

                    path = new GraphicsPath();
                    //添加直线1
                    path.AddLine(new PointF(1 + bigRadius + smallRadius, 1 + smallRadius), new PointF(this.Width-(1 + bigRadius + smallRadius), 1+smallRadius));
                    //添加圆弧1
                    path.AddArc(new RectangleF(this.Width - (1 + bigRadius + smallRadius*2), 1 + smallRadius,smallRadius*2,smallRadius*2),-90,180);
                    //添加直线2
                    path.AddLine(new PointF(this.Width - (1 + bigRadius + smallRadius),1+smallRadius+bigRadius), new PointF(1 + bigRadius + smallRadius, 1 + smallRadius + bigRadius));
                    //添加圆弧2
                    path.AddArc(new RectangleF(1 + bigRadius, 1 + smallRadius, smallRadius * 2, smallRadius * 2), 90, 180);

                    graphics.FillPath(new SolidBrush(fillColor), path);

                    if (isChecked)
                    {
                        graphics.FillEllipse(new SolidBrush(fillColor), new RectangleF(this.Width - (1 + bigRadius * 2), 1, bigRadius * 2, bigRadius * 2));

                        graphics.FillEllipse(Brushes.White, new RectangleF(this.Width - (1 + bigRadius + smallRadius), 1 + smallRadius, smallRadius * 2, smallRadius * 2));
                    }
                    else
                    {
                        graphics.FillEllipse(new SolidBrush(fillColor), new RectangleF(1, 1, bigRadius * 2, bigRadius * 2));

                        graphics.FillEllipse(Brushes.White, new RectangleF(1+smallRadius, 1 + smallRadius, smallRadius * 2, smallRadius * 2));
                    }

                    break;
                default:
                    break;
            }
        }

        [Browsable(true)]
        [Category("自定义事件")]
        [Description("选中发生变化触发")]
        public event EventHandler CheckedChanged;

    }
}
