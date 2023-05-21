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

            //���ÿؼ���ʽ
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private Color defaultColor = Color.FromArgb(0, 235, 177);
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

        private Color waveBackColor = Color.FromArgb(9, 96, 115);
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ���α���ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡԲ������ϵ��[0-0.2]")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�������ֵ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ������Сֵ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰֵ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ɫ����")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ֵ����")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ֵ����")]
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


            //��ȡ����
            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


            //ת������ϵ
            graphics.TranslateTransform(this.Width / 2.0f, 0.0f);

            //����ͷ��

            SolidBrush solidBrush = new SolidBrush(this.waveBackColor);

            float r = radianRate * this.Height;

            graphics.FillEllipse(solidBrush, -this.Width * 0.5f, 1.0f, Width, r * 2);

            //�����в�

            graphics.FillRectangle(solidBrush, -this.Width * 0.5f, r + 1.0f, this.Width, this.Height - 2 * (r + 1.0f));

            //Һ����ɫ

            float scale = (currentValue - rangeMin) / (rangeMax - rangeMin);

            Color waveColor = GetColorFromValue(scale);

            float ypos = (1.0f - scale) * (this.Height - 2 * (r + 1.0f));

            GraphicsPath graphicsPath = new GraphicsPath();

            //��ӱ���������
            if (scale > radianRate)
            {
                PointF point1 = new PointF(-Width * 0.5f, ypos + r + 1.0f);
                PointF point2 = new PointF(-Width * 0.25f, ypos + 3 * r + 1.0f);
                PointF point3 = new PointF(Width * 0.25f, ypos - r + 1.0f);
                PointF point4 = new PointF(Width * 0.5f, ypos + r + 1.0f);
                graphicsPath.AddBezier(point1, point2, point3, point4);
            }

            //���ֱ��
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
        /// ͨ����ֵ��ȡ��Ӧ�������ɫ
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
