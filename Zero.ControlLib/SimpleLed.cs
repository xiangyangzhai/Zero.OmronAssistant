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

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private bool state=true;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取指示灯的状态")]
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
        [Category("自定义属性")]
        [Description("设置或获取指示灯绑定的变量名称")]
        public string BindVarName { get; set; }
    }
}
