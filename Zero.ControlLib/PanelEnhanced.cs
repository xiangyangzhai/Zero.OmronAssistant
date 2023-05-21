using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;

namespace Zero.ControlLib
{
    public partial class PanelEnhanced : Panel
    {
        public PanelEnhanced()
        {
            InitializeComponent();

            
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            return;  
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //…Ë÷√ π”√À´ª∫≥Â
            this.DoubleBuffered = true;

            if (this.BackgroundImage != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.DrawImage(this.BackgroundImage, new Rectangle(0, 0, this.Width, this.Height), new Rectangle(0, 0, this.BackgroundImage.Width, this.BackgroundImage.Height), GraphicsUnit.Pixel);     
            }

            base.OnPaint(e);    
        }
    }
}
