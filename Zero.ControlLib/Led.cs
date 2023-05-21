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

            //���ÿؼ���ʽ
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

        //����������󾭳�ʹ�ã��Ϳ��Դ���һ����Ա����
        private Graphics graphics;
        private Pen pen;
        private Timer blinkTimer;


        private Color defaultColor = Color.Gray;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡĬ����ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�⻷��϶")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�⻷���")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�Ƿ��б߿�")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ɫ���鼯��")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰֵ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ������ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�Ƿ������ʾ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�Ƿ���˸")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��˸���")]
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

            //��ȡ����
            graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            //�Ȼ�Բ

            Color color = GetColor();

            int width = Math.Min(this.Width, this.Height);

            SolidBrush solidBrush = new SolidBrush(color);
            Rectangle rectangle = new Rectangle(1, 1, width - 2, width - 2);

            graphics.FillEllipse(solidBrush, rectangle);

            float temp = 1 + gapWidth + borderWidth * 0.5f;

            //���Ʊ߿�
            if (isBorder)
            {
                pen = new Pen(this.BackColor, borderWidth);

                RectangleF rectangleF = new RectangleF(temp, temp, width - 2 * temp, width - 2 * temp);

                graphics.DrawEllipse(pen, rectangleF);

            }


            //��������

            if (isHighLight)
            {
                //����һ��GraphicsPath
                GraphicsPath graphicsPath = new GraphicsPath();

                //����һ��RectangleF
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

                //�����Բ��״
                graphicsPath.AddEllipse(rectangle1);

                //����һ��PathGradientBrush����
                PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);

                pathGradientBrush.CenterColor = this.centerColor;
                pathGradientBrush.SurroundColors = new Color[] { color };

                graphics.FillPath(pathGradientBrush, graphicsPath);
            }
        }


        /// <summary>
        /// ��ȡ������ɫ
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
