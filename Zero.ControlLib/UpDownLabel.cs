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
    [DefaultEvent("ValueChanged")]
    public partial class UpDownLabel : UserControl
    {
        public UpDownLabel()
        {
            InitializeComponent();
        }

        private Color themeColor=Color.FromArgb(27, 161, 226);
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取主题颜色")]
        public Color ThemeColor
        {
            get { return themeColor; }
            set { themeColor = value;

                this.btn_Add.BackColor = themeColor;
                this.btn_Dec.BackColor = themeColor;
            }
        }



        private Color themeForeColor = Color.White;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取主题前景色")]
        public Color ThemeForeColor
        {
            get { return themeForeColor; }
            set
            {
                themeForeColor = value;

                this.btn_Add.ForeColor = themeForeColor;
                this.btn_Dec.ForeColor = themeForeColor;
            }
        }



        private int textWidth= 52;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取文本框的宽度")]
        public int TextWidth
        {
            get { return textWidth; }
            set { textWidth = value;

                this.btn_Add.Width = (this.Width - this.textWidth) / 2;
                this.btn_Dec.Width = (this.Width - this.textWidth) / 2;
            }
        }


        private float maxValue = 1000.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取最大值")]
        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
            }
        }


        private float minValue = -1000.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取最小值")]
        public float MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
            }
        }

        private float stepValue = 1.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取步进值")]
        public float StepValue
        {
            get { return stepValue; }
            set
            {
                stepValue = value;
            }
        }


        private float currentValue = 0.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置或获取当前值")]
        public float CurrentValue
        {
            get { return currentValue; }
            set
            {
                if (value > maxValue)
                {
                    return;
                }

                if (value < minValue)
                {
                    return;
                }

                if (currentValue == value)
                {
                    return;
                }

                currentValue = value;

                ValueChanged?.Invoke(this, null);

                this.lbl_Data.Text = currentValue.ToString();
            }
        }

        private void btn_Dec_Click(object sender, EventArgs e)
        {
            CurrentValue -= stepValue;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            CurrentValue += stepValue;
        }
        [Browsable(true)]
        [Category("自定义事件")]
        [Description("当前数值发生变化触发")]
        public event EventHandler ValueChanged;
    }
}
