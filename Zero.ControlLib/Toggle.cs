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
        /// ��Բ
        /// </summary>
        Ellipse,
        /// <summary>
        /// ����
        /// </summary>
        Rectangle,
        /// <summary>
        /// ֱ��
        /// </summary>
        Line

    }

    [DefaultEvent("CheckedChanged")]
    public partial class Toggle : UserControl
    {
        public Toggle()
        {
            InitializeComponent();

            //���ÿؼ���ʽ
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ�����ʽ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ�ѡ�е���ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ�δѡ�е���ɫ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ��Ƿ�ѡ��")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ�ѡ�е��ı�")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�ؼ�δѡ�е��ı�")]
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


            //��ȡ�����ɫ
            Color fillColor = isChecked ? trueColor : falseColor;

            //����·��
            GraphicsPath path = new GraphicsPath();

            //�ı�
            string text = isChecked ? trueText : falseText;

            //�ļ��ߴ�
            SizeF size = graphics.MeasureString(text, this.Font);

            //��϶
            int gap = 2;

            //СԲ���
            int border = 2;

            switch (controlType)
            {
                case ControlType.Ellipse:

                    float r = (this.Height - 2) * 0.5f;

                    //���ֱ��
                    path.AddLine(new Point(this.Height / 2, 1), new Point(this.Width - this.Height / 2, 1));
                    //���Բ��
                    path.AddArc(new RectangleF(this.Width - 2 * r - 1, 1, 2 * r, 2 * r), -90, 180);
                    //���ֱ��
                    path.AddLine(new PointF(this.Width - r - 1, this.Height - 1), new Point(this.Height / 2, this.Height - 1));
                    //���Բ��
                    path.AddArc(new RectangleF(1, 1, 2 * r, 2 * r), 90, 180);

                    graphics.FillPath(new SolidBrush(fillColor), path);

                    //��Բ�뾶
                    float radius = (this.Height - 2 * (1 + gap)) * 0.5f;

                    //���ѡ����λ���
                    if (isChecked)
                    {
                        graphics.FillEllipse(Brushes.White, new RectangleF(this.Width - 2 * radius - gap - 1, gap + 1, 2 * radius, 2 * radius));

                        //����ı�Ϊ�գ���СԲ������д����
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

                        //����ı�Ϊ�գ���СԲ������д����
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

                    //���Բ��1
                    path.AddArc(new RectangleF(1, 1, intRadius * 2, intRadius * 2), 180, 90);
                    //���Բ��2
                    path.AddArc(new RectangleF(this.Width - 2 * intRadius - 1, 1, intRadius * 2, intRadius * 2), 270, 90);
                    //���Բ��3
                    path.AddArc(new RectangleF(this.Width - 2 * intRadius - 1, this.Height - 2 * intRadius - 1, intRadius * 2, intRadius * 2), 0, 90);
                    //���Բ��4
                    path.AddArc(new RectangleF(1, this.Height - 2 * intRadius - 1, intRadius * 2, intRadius * 2), 90, 90);

                    graphics.FillPath(new SolidBrush(fillColor), path);

                    if (isChecked)
                    {
                        path = new GraphicsPath();

                        int inwidth = this.Height - 2 * (gap + 1);

                        //���Բ��1
                        path.AddArc(new RectangleF(this.Width - inwidth - gap - 1, 1 + gap, intRadius * 2, intRadius * 2), 180, 90);
                        //���Բ��2
                        path.AddArc(new RectangleF(this.Width - 1 - gap - 2 * intRadius, 1 + gap, intRadius * 2, intRadius * 2), 270, 90);
                        //���Բ��3
                        path.AddArc(new RectangleF(this.Width - 1 - gap - 2 * intRadius, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 0, 90);
                        //���Բ��4
                        path.AddArc(new RectangleF(this.Width - inwidth - gap - 1, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 90, 90);

                        graphics.FillPath(Brushes.White, path);

                        radius = inwidth * 0.5f;

                        //����ı�Ϊ�գ���СԲ������д����
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

                        //���Բ��1
                        path.AddArc(new RectangleF(1 + gap, 1 + gap, intRadius * 2, intRadius * 2), 180, 90);
                        //���Բ��2
                        path.AddArc(new RectangleF(inwidth + 1 + gap - 2 * intRadius, 1 + gap, intRadius * 2, intRadius * 2), 270, 90);
                        //���Բ��3
                        path.AddArc(new RectangleF(inwidth + 1 + gap - 2 * intRadius, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 0, 90);
                        //���Բ��4
                        path.AddArc(new RectangleF(1 + gap, this.Height - 2 * intRadius - 1 - gap, intRadius * 2, intRadius * 2), 90, 90);

                        graphics.FillPath(Brushes.White, path);

                        radius = inwidth * 0.5f;

                        //����ı�Ϊ�գ���СԲ������д����
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
                    //���ֱ��1
                    path.AddLine(new PointF(1 + bigRadius + smallRadius, 1 + smallRadius), new PointF(this.Width-(1 + bigRadius + smallRadius), 1+smallRadius));
                    //���Բ��1
                    path.AddArc(new RectangleF(this.Width - (1 + bigRadius + smallRadius*2), 1 + smallRadius,smallRadius*2,smallRadius*2),-90,180);
                    //���ֱ��2
                    path.AddLine(new PointF(this.Width - (1 + bigRadius + smallRadius),1+smallRadius+bigRadius), new PointF(1 + bigRadius + smallRadius, 1 + smallRadius + bigRadius));
                    //���Բ��2
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
        [Category("�Զ����¼�")]
        [Description("ѡ�з����仯����")]
        public event EventHandler CheckedChanged;

    }
}
