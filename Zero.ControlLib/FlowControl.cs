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
    /// <summary>
    /// 管道水平或垂直
    /// </summary>
    public enum DirectionStyle
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// 管道左右/上下转向
    /// </summary>
    public enum PipeTurnDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public partial class FlowControl : UserControl
    {
        public FlowControl()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.updateTimer = new Timer();
            this.updateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.dashOffset += this.moveVelocity;

            if (this.dashOffset > this.pipeLength + this.gapLength || this.dashOffset < (this.pipeLength + this.gapLength) * (-1.0f))
            {
                this.dashOffset = 0.0f;
            }
            this.Invalidate();
        }

        //虚线的起始点
        private float dashOffset = 0.0f;

        private Timer updateTimer;

        private Color moveColor = Color.DodgerBlue;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道的流动颜色")]
        public Color MoveColor
        {
            get { return moveColor; }
            set
            {
                moveColor = value;

                this.Invalidate();
            }
        }

        private Color edgeColor = Color.DimGray;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道的边缘颜色")]
        public Color EdgeColor
        {
            get { return edgeColor; }
            set
            {
                edgeColor = value;

                this.Invalidate();
            }
        }



        private Color centerColor = Color.LightGray;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道的中心颜色")]
        public Color CenterColor
        {
            get { return centerColor; }
            set
            {
                centerColor = value;

                this.Invalidate();
            }
        }


        private PipeTurnDirection pipeTurnLeft = PipeTurnDirection.None;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道左边的转向类型")]
        public PipeTurnDirection PipeTurnLeft
        {
            get { return pipeTurnLeft; }
            set
            {
                pipeTurnLeft = value;
                this.Invalidate();
            }
        }


        private PipeTurnDirection pipeTurnRight = PipeTurnDirection.None;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道右边的转向类型")]
        public PipeTurnDirection PipeTurnRight
        {
            get { return pipeTurnRight; }
            set
            {
                pipeTurnRight = value;
                this.Invalidate();
            }
        }

        private DirectionStyle pipeLineStyle = DirectionStyle.Horizontal;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道水平或垂直")]
        public DirectionStyle PipeLineStyle
        {
            get { return pipeLineStyle; }
            set { pipeLineStyle = value; this.Invalidate(); }
        }


        private bool isActive = false;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道是否流动")]
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                this.updateTimer.Enabled = value;
                this.Invalidate();
            }
        }


        private float moveVelocity = 1.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道的流动速度(-1.0-1.0之间)，正为正向，负为反向")]
        public float MoveVelocity
        {
            get { return moveVelocity; }
            set
            {
                if (value > 1.0f)
                {
                    value = 1.0f;
                }
                if (value < -1.0f)
                {
                    value = 1.0f;
                }
                moveVelocity = value;
                this.Invalidate();
            }
        }

        private int moveInterval = 50;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道的流动间隔")]
        public int MoveInterval
        {
            get { return moveInterval; }
            set
            {
                if (value <= 0) return;
                this.updateTimer.Interval = value;
                moveInterval = value;
                this.Invalidate();
            }
        }

        private int pipeWidth = 5;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道流动条宽度")]
        public int PipeWidth
        {
            get { return pipeWidth; }
            set
            {
                if (value <= 0) return;

                pipeWidth = value;
                this.Invalidate();
            }
        }

        private int pipeLength = 5;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道流动条长度")]
        public int PipeLength
        {
            get { return pipeLength; }
            set
            {
                if (value <= 0) return;

                pipeLength = value;
                this.Invalidate();
            }
        }


        private int gapLength = 5;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取管道流动条之间的间隔")]
        public int GapLength
        {
            get { return gapLength; }
            set
            {
                if (value <= 0) return;

                gapLength = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            ColorBlend blend = new ColorBlend();
            //渐变颜色
            blend.Colors = new Color[] { this.edgeColor, this.centerColor, this.edgeColor };
            //渐变比例
            blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };

            LinearGradientBrush linearGradientBrush;

            Pen pen = new Pen(this.edgeColor, 1.0f);

            //水平管道
            if (this.pipeLineStyle == DirectionStyle.Horizontal)
            {
                linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), this.edgeColor, this.edgeColor);

                linearGradientBrush.InterpolationColors = blend;

                switch (this.pipeTurnLeft)
                {
                    case PipeTurnDirection.Up:
                        PaintEllipse(graphics, blend, pen, new Rectangle(0, -this.Height, this.Height * 2, this.Height * 2), 90, 90);
                        break;
                    case PipeTurnDirection.Down:
                        PaintEllipse(graphics, blend, pen, new Rectangle(0, 0, this.Height * 2, this.Height * 2), 180, 90);
                        break;
                    default:
                        //绘制矩形
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, 0, this.Height, this.Height));
                        break;
                }

                switch (this.pipeTurnRight)
                {
                    case PipeTurnDirection.Up:
                        PaintEllipse(graphics, blend, pen, new Rectangle(this.Width - 2 * this.Height, -this.Height, this.Height * 2, this.Height * 2), 0, 90);
                        break;
                    case PipeTurnDirection.Down:
                        PaintEllipse(graphics, blend, pen, new Rectangle(this.Width - 2 * this.Height, 0, this.Height * 2, this.Height * 2), 270, 90);
                        break;
                    default:
                        //绘制矩形
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(this.Width - this.Height, 0, this.Height, this.Height));
                        break;
                }

                if (this.Width > this.Height * 2)
                {
                    //绘制矩形
                    PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(this.Height - 1, 0, this.Width - 2 * this.Height + 2, this.Height));
                }

                //判断是否流动

                if (isActive)
                {
                    //路径
                    GraphicsPath graphicsPath = new GraphicsPath();

                    //规划路径

                    //左边部分
                    switch (this.pipeTurnLeft)
                    {
                        case PipeTurnDirection.Up:
                            graphicsPath.AddArc(new RectangleF(this.Height * 0.5f, this.Height * 0.5f * (-1), this.Height, this.Height), 180, -90);
                            break;
                        case PipeTurnDirection.Down:
                            graphicsPath.AddArc(new RectangleF(this.Height * 0.5f, this.Height * 0.5f, this.Height, this.Height), 180, 90);
                            break;
                        default:
                            graphicsPath.AddLine(0, this.Height * 0.5f, this.Height, this.Height * 0.5f);
                            break;
                    }

                    //中间部分
                    if (this.Width > this.Height * 2)
                    {
                        graphicsPath.AddLine(this.Height, this.Height * 0.5f, this.Width - this.Height, this.Height * 0.5f);
                    }

                    //右边部分
                    switch (this.pipeTurnRight)
                    {
                        case PipeTurnDirection.Up:
                            graphicsPath.AddArc(new RectangleF(this.Width - this.Height * 1.5f, this.Height * 0.5f * (-1), this.Height, this.Height), 90, -90);
                            break;
                        case PipeTurnDirection.Down:
                            graphicsPath.AddArc(new RectangleF(this.Width - this.Height * 1.5f, this.Height * 0.5f, this.Height, this.Height), 270, 90);
                            break;
                        default:
                            graphicsPath.AddLine(this.Width - this.Height, this.Height * 0.5f, this.Width, this.Height * 0.5f);
                            break;
                    }

                    //对Pen进一步设置
                    //画笔
                    pen = new Pen(this.moveColor, this.pipeWidth);

                    pen.DashStyle = DashStyle.Custom;

                    pen.DashPattern = new float[]
                    {
                        this.pipeLength,this.gapLength
                    };
                    pen.DashOffset = dashOffset;

                    graphics.DrawPath(pen, graphicsPath);
                }
            }
            else
            {
                linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), this.edgeColor, this.edgeColor);

                linearGradientBrush.InterpolationColors = blend;

                switch (this.pipeTurnLeft)
                {
                    case PipeTurnDirection.Left:
                        PaintEllipse(graphics, blend, pen, new Rectangle(-this.Width, 0, this.Width * 2, this.Width * 2), 270, 90);
                        break;
                    case PipeTurnDirection.Right:
                        PaintEllipse(graphics, blend, pen, new Rectangle(0, 0, this.Width * 2, this.Width * 2), 180, 90);
                        break;
                    default:
                        //绘制矩形
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, 0, this.Width, this.Width));
                        break;
                }

                switch (this.pipeTurnRight)
                {
                    case PipeTurnDirection.Left:
                        PaintEllipse(graphics, blend, pen, new Rectangle(-this.Width, this.Height - 2 * this.Width, this.Width * 2, this.Width * 2), 0, 90);
                        break;
                    case PipeTurnDirection.Right:
                        PaintEllipse(graphics, blend, pen, new Rectangle(0, this.Height - 2 * this.Width, this.Width * 2, this.Width * 2), 90, 90);
                        break;
                    default:
                        //绘制矩形
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, this.Height - this.Width, this.Width, this.Width));
                        break;
                }

                if (this.Height > this.Width * 2)
                {
                    //绘制矩形
                    PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, this.Width - 1, this.Width, this.Height - this.Width * 2 + 2));
                }

                if (isActive)
                {
                    //路径
                    GraphicsPath graphicsPath = new GraphicsPath();

                    switch (this.pipeTurnLeft)
                    {
                        case PipeTurnDirection.Left:
                            graphicsPath.AddArc(new RectangleF(this.Width * 0.5f * (-1), this.Width * 0.5f, this.Width, this.Width), 270, 90);
                            break;
                        case PipeTurnDirection.Right:
                            graphicsPath.AddArc(new RectangleF(this.Width * 0.5f, this.Width * 0.5f, this.Width, this.Width), 270, -90);
                            break;
                        default:
                            graphicsPath.AddLine(this.Width * 0.5f, 0, this.Width * 0.5f, this.Width);
                            break;
                    }

                    if (this.Height > this.Width * 2)
                    {
                        graphicsPath.AddLine(this.Width * 0.5f, this.Width, this.Width * 0.5f, this.Height - this.Width);
                    }

                    switch (this.pipeTurnRight)
                    {
                        case PipeTurnDirection.Left:
                            graphicsPath.AddArc(new RectangleF(this.Width * 0.5f * (-1), this.Height - this.Width * 1.5f, this.Width, this.Width), 0, 90);
                            break;
                        case PipeTurnDirection.Right:
                            graphicsPath.AddArc(new RectangleF(this.Width * 0.5f, this.Height - this.Width * 1.5f, this.Width, this.Width), 180, -90);
                            break;
                        default:
                            graphicsPath.AddLine(this.Width * 0.5f, this.Height - this.Width, this.Width * 0.5f, this.Height);
                            break;
                    }

                    //对Pen进一步设置
                    //画笔
                    pen = new Pen(this.moveColor, this.pipeWidth);

                    pen.DashStyle = DashStyle.Custom;

                    pen.DashPattern = new float[]
                    {
                        this.pipeLength,this.gapLength
                    };
                    pen.DashOffset = dashOffset;

                    graphics.DrawPath(pen, graphicsPath);
                }
            }
        }

        private void PaintEllipse(Graphics graphics, ColorBlend blend, Pen pen, Rectangle rectangle, float startAngle, float sweepAngle)
        {
            //创建GraphicsPath
            GraphicsPath graphicsPath = new GraphicsPath();

            graphicsPath.AddEllipse(rectangle);

            //创建PathGradientBrush
            PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);

            pathGradientBrush.CenterPoint = new PointF(rectangle.X + rectangle.Width * 0.5f, rectangle.Y + rectangle.Height * 0.5f);
            pathGradientBrush.InterpolationColors = blend;

            //绘制Pipe

            graphics.FillPie(pathGradientBrush, rectangle, startAngle, sweepAngle);

            //绘制边线

            graphics.DrawArc(pen, rectangle, startAngle, sweepAngle);

        }


        private void PaintRectangle(Graphics graphics, Brush brush, Pen pen, Rectangle rectangle)
        {
            graphics.FillRectangle(brush, rectangle);

            switch (this.pipeLineStyle)
            {
                case DirectionStyle.Horizontal:
                    graphics.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y);
                    graphics.DrawLine(pen, rectangle.X, rectangle.Y + rectangle.Height, rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                    break;
                case DirectionStyle.Vertical:
                    graphics.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X, rectangle.Y + rectangle.Height);
                    graphics.DrawLine(pen, rectangle.X + rectangle.Width, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                    break;
                default:
                    break;
            }

        }
    }
}
