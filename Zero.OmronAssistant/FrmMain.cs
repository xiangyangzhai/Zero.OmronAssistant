using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zero.ControlLib;

namespace Zero.OmronAssistant
{
    /// <summary>
    /// 通过临界窗体，将所有的窗体分成两部分，小于临界窗体为固定窗体，大于临界窗体的为非固定窗体
    /// </summary>
    public enum FormNames
    {
        主界面,
        参数设置,
        临界窗体,
        帮助信息,
        退出系统
    }
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            Common_NaviButtonClickBind();
        }


        #region 切换窗体
        private void Common_NaviButtonClickBind()
        {
            foreach(var item in this.TopPanel.Controls.OfType<NaviButton>())
            {
                item.NaviButtonClick += Common_NaviButtonClick;
            }
        }
        

        public void OpenWindow(FormNames formNames)
        {
            int total = this.MainPanel.Controls.Count;

            int closeCount = 0;

            bool isFind = false;

            for (int i = 0; i < total; i++)
            {
                Control ct = this.MainPanel.Controls[i - closeCount];

                if (ct is Form frm)
                {
                    //如果已经找到
                    if (frm.Text == formNames.ToString())
                    {
                        frm.BringToFront();
                        isFind = true;
                        break;
                    }
                    //如果不是我们要打开的窗体，就要判断是不是固定窗体
                    //如果是固定窗体，不处理
                    //如果不是固定窗体，就关闭掉
                    else if ((FormNames)Enum.Parse(typeof(FormNames), frm.Text) > FormNames.临界窗体)
                    {
                        frm.Close();
                        closeCount++;
                    }
                }
            }

            //到底要不要打开新窗体

            //如果是参数设置，是不是要打开

            if (isFind == false)
            {
                Form frm = null;

                switch (formNames)
                {
                    case FormNames.主界面:
                        frm = new FrmMonitor();
                        break;
                    case FormNames.临界窗体:
                        break;
                    case FormNames.参数设置:
                        frm = new FrmSysSet();
                        break;
                    case FormNames.帮助信息:
                        frm = new FrmHelpInfo();
                        break;
                    case FormNames.退出系统:
                        //不切换窗体

                        // 退出系统前，对话框确认
                        DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            Application.Exit();
                        }
                        return;
                    default:
                        break;
                }
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.Parent = this.MainPanel;
                frm.BringToFront();
                frm.Show();
            }
        }

        

        private void Common_NaviButtonClick(object sender, EventArgs e)
        {
            if(sender is NaviButton navi)
            {
                FormNames formNames = (FormNames)Enum.Parse(typeof(FormNames), navi.NaviName);

                //权限控制
                switch (formNames)
                {
                    case FormNames.主界面:
                        break;
                    case FormNames.临界窗体:
                        break;
                    case FormNames.参数设置:
                        break;
                    case FormNames.帮助信息:
                        break;
                    case FormNames.退出系统:
                        break;
                    default:
                        break;
                }

                OpenWindow(formNames);
            
            }
        }


        #endregion

        #region 无边框拖动

        private Point mPoint;

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        #endregion



        private void FrmMain_Load(object sender, EventArgs e)
        {
            // 触发主界面点击事件
            this.OpenWindow(FormNames.主界面);

        }
    }


}
