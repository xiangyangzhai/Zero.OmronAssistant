using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class TextShow : UserControl
    {
        public TextShow()
        {
            InitializeComponent();

            //���ÿؼ���ʽ
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);


            stringFormat = new StringFormat();
            stringFormat.LineAlignment= StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
        }


        private int borderWidth=2;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�߿���")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set {
                borderWidth = value;

                this.Invalidate();
            }
        }


        private Color borderColor = Color.White;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�߿���ɫ")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;

                this.Invalidate();
            }
        }


        private string currentValue="12.34";
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰֵ")]
        public string CurrentValue
        {
            get { return currentValue; }
            set { 
                currentValue = value; 
                this.Invalidate(); 
            }
        }

        private string unit="��";
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰ��λ")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.Invalidate();
            }
        }
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�󶨱�������")]
        public string BindVarName { get; set; }

        //�ı��Ķ��뷽ʽ
        private StringFormat stringFormat;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;


            //���Ʊ߿�

            Rectangle rectangle = new Rectangle(borderWidth / 2, borderWidth / 2, this.Width - borderWidth, this.Height - borderWidth);

            graphics.DrawRectangle(new Pen(borderColor, borderWidth), rectangle);


            //�����ı�

            graphics.DrawString(currentValue + " " + unit, this.Font, new SolidBrush(this.ForeColor), rectangle, stringFormat);
        }

    }
}
