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
    public partial class NumKeyboard : UserControl
    {
        public NumKeyboard()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        [Category("自定义属性")]
        [Description("最终的结果值")]
        public string ResultValue
        {
            get { return this.lbl_Result.Text; }
        }

        private string initialValue;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("初始值")]
        public string InitialValue
        {
            get { return initialValue; }
            set
            {
                initialValue = value;
                this.lbl_Result.Text = value;
            }
        }

        private float minValue = 0.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置的最小值")]
        public float MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                this.lbl_Min.Text = value.ToString();
            }
        }


        private float maxValue = 0.0f;
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("设置的最大值")]
        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                this.lbl_Max.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [Category("自定义属性")]
        [Description("关联控件")]
        public Control RelateControl { get; set; }

        [Browsable(true)]
        [Category("自定义事件")]
        [Description("Enter按下事件")]
        public event EventHandler EnterClick;


        [Browsable(true)]
        [Category("自定义事件")]
        [Description("ESC按下事件")]
        public event EventHandler ESCClick;

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            string temp = this.lbl_Result.Text;

            if (float.TryParse(temp, out float value))
            {
                if (value >= this.minValue && value <= this.maxValue)
                {
                    EnterClick?.Invoke(this, e);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

        }

        private void btn_Esc_Click(object sender, EventArgs e)
        {
            ESCClick?.Invoke(this, e);
        }

        private void btn_Common_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Text == ".")
                {
                    //处理小数点
                    if (this.lbl_Result.Text.Contains('.'))
                    {
                        return;
                    }
                    else
                    {
                        this.lbl_Result.Text += button.Text;
                    }
                }
                else
                {
                    string temp = this.lbl_Result.Text + button.Text;

                    if (float.TryParse(temp, out float value))
                    {
                        if (value >= this.minValue && value <= this.maxValue)
                        {
                            this.lbl_Result.Text = temp;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            if (this.lbl_Result.Text.Length == 0) return;

            this.lbl_Result.Text = this.lbl_Result.Text.Substring(0, this.lbl_Result.Text.Length - 1);
        }

        private void btn_Minus_Click(object sender, EventArgs e)
        {
            string temp = this.lbl_Result.Text;

            if (temp.StartsWith("-"))
            {
                temp = temp.Remove(0, 1);
            }
            else
            {
                temp = temp.Insert(0, "-");
            }

            if (float.TryParse(temp, out float value))
            {
                if (value >= this.minValue && value <= this.maxValue)
                {
                    this.lbl_Result.Text = temp;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }


        #region 无边框拖动

        private Point mPoint;
        private void lbl_Result_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void lbl_Result_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        #endregion

    }
}
