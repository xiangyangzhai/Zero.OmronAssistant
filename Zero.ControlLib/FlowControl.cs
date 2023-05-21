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
    /// �ܵ�ˮƽ��ֱ
    /// </summary>
    public enum DirectionStyle
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// �ܵ�����/����ת��
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

            //���ÿؼ���ʽ
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

        //���ߵ���ʼ��
        private float dashOffset = 0.0f;

        private Timer updateTimer;

        private Color moveColor = Color.DodgerBlue;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ���������ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ��ı�Ե��ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ���������ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ���ߵ�ת������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ��ұߵ�ת������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ�ˮƽ��ֱ")]
        public DirectionStyle PipeLineStyle
        {
            get { return pipeLineStyle; }
            set { pipeLineStyle = value; this.Invalidate(); }
        }


        private bool isActive = false;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ��Ƿ�����")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ��������ٶ�(-1.0-1.0֮��)����Ϊ���򣬸�Ϊ����")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ����������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ����������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ�����������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ܵ�������֮��ļ��")]
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
            //������ɫ
            blend.Colors = new Color[] { this.edgeColor, this.centerColor, this.edgeColor };
            //�������
            blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };

            LinearGradientBrush linearGradientBrush;

            Pen pen = new Pen(this.edgeColor, 1.0f);

            //ˮƽ�ܵ�
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
                        //���ƾ���
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
                        //���ƾ���
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(this.Width - this.Height, 0, this.Height, this.Height));
                        break;
                }

                if (this.Width > this.Height * 2)
                {
                    //���ƾ���
                    PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(this.Height - 1, 0, this.Width - 2 * this.Height + 2, this.Height));
                }

                //�ж��Ƿ�����

                if (isActive)
                {
                    //·��
                    GraphicsPath graphicsPath = new GraphicsPath();

                    //�滮·��

                    //��߲���
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

                    //�м䲿��
                    if (this.Width > this.Height * 2)
                    {
                        graphicsPath.AddLine(this.Height, this.Height * 0.5f, this.Width - this.Height, this.Height * 0.5f);
                    }

                    //�ұ߲���
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

                    //��Pen��һ������
                    //����
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
                        //���ƾ���
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
                        //���ƾ���
                        PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, this.Height - this.Width, this.Width, this.Width));
                        break;
                }

                if (this.Height > this.Width * 2)
                {
                    //���ƾ���
                    PaintRectangle(graphics, linearGradientBrush, pen, new Rectangle(0, this.Width - 1, this.Width, this.Height - this.Width * 2 + 2));
                }

                if (isActive)
                {
                    //·��
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

                    //��Pen��һ������
                    //����
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
            //����GraphicsPath
            GraphicsPath graphicsPath = new GraphicsPath();

            graphicsPath.AddEllipse(rectangle);

            //����PathGradientBrush
            PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);

            pathGradientBrush.CenterPoint = new PointF(rectangle.X + rectangle.Width * 0.5f, rectangle.Y + rectangle.Height * 0.5f);
            pathGradientBrush.InterpolationColors = blend;

            //����Pipe

            graphics.FillPie(pathGradientBrush, rectangle, startAngle, sweepAngle);

            //���Ʊ���

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
