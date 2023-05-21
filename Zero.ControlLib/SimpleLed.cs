using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ControlLib
{
    public partial class SimpleLed : UserControl
    {
        public SimpleLed()
        {
            InitializeComponent();

            //���ÿؼ���ʽ
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private bool state=true;

        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡָʾ�Ƶ�״̬")]
        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                this.BackgroundImage = state ? Properties.Resources.Green : Properties.Resources.Red;
            }
        }

        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡָʾ�ư󶨵ı�������")]
        public string BindVarName { get; set; }
    }
}
