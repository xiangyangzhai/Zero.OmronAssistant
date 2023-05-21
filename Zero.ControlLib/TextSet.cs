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
    [DefaultEvent("ControlDoubleClick")]
    public partial class TextSet : UserControl
    {
        public TextSet()
        {
            InitializeComponent();
        }


        private int titleScale=50;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取标题所占比例")]
        public int TitleScale
        {
            get { return titleScale; }
            set { titleScale = value;

                this.MainTableLayoutControl.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, titleScale);
            }
        }


        private int valueScale = 30;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取数据所占比例")]
        public int ValueScale
        {
            get { return valueScale; }
            set
            {
                valueScale = value;

                this.MainTableLayoutControl.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, valueScale);
            }
        }

        private string titleName = "参数名称设置";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取参数名称")]
        public string TitleName
        {
            get { return titleName; }
            set
            {
                titleName = value;

                this.lbl_Title.Text = titleName;
            }
        }


        private string currentValue = "0.0";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前数值")]
        public string CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;

                this.lbl_Value.Text = currentValue;
            }
        }


        private string unit = "℃";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前单位")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;

                this.lbl_Unit.Text = unit;
            }
        }

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取绑定变量名称")]
        public string BindVarName { get; set; }

        [Browsable(true)]
        [Category("自定义事件")]
        [Description("控件双击事件")]
        public event EventHandler ControlDoubleClick;

        private void lbl_Value_DoubleClick(object sender, EventArgs e)
        {
            if (ControlDoubleClick != null)
            {
                ControlDoubleClick(this, e);
            }
        }
    }
}
