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
        [Category("�Զ�������")]
        [Description("���û��ȡ������ռ����")]
        public int TitleScale
        {
            get { return titleScale; }
            set { titleScale = value;

                this.MainTableLayoutControl.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, titleScale);
            }
        }


        private int valueScale = 30;
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ������ռ����")]
        public int ValueScale
        {
            get { return valueScale; }
            set
            {
                valueScale = value;

                this.MainTableLayoutControl.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, valueScale);
            }
        }

        private string titleName = "������������";
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ��������")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰ��ֵ")]
        public string CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;

                this.lbl_Value.Text = currentValue;
            }
        }


        private string unit = "��";
        [Browsable(true)]
        [Category("�Զ�������")]
        [Description("���û��ȡ��ǰ��λ")]
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
        [Category("�Զ�������")]
        [Description("���û��ȡ�󶨱�������")]
        public string BindVarName { get; set; }

        [Browsable(true)]
        [Category("�Զ����¼�")]
        [Description("�ؼ�˫���¼�")]
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
