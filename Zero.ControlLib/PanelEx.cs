using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class PanelEx : Panel
    {
        public PanelEx()
        {
            InitializeComponent();

            //���ÿؼ���ʽ
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

        }

        //GDI+

        //Graphics 

        //Device

        //Interface

        //Plus

        private Color borderColor = Color.FromArgb(35, 255, 253);

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


        private int borderWidth = 2;

        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ�߿���")]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //��ȡGraphics����
            Graphics graphics = e.Graphics;

            //����Pen����
            Pen pen = new Pen(borderColor, borderWidth);

            //����Rectangle����
            Rectangle rectangle = new Rectangle(1, 1, this.Width - 2, this.Height - 2);

            //����Rectangle���α߿�
            graphics.DrawRectangle(pen, rectangle);
        }

    }
}
