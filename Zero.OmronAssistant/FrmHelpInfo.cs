using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.OmronAssistant
{
    public partial class FrmHelpInfo : Form
    {
        public FrmHelpInfo()
        {
            InitializeComponent();
        }

        private void FrmHelpInfo_Load(object sender, EventArgs e)
        {
            //在rtxtHelpInfo中显示readme.txt中的内容，并设置rtxtHelpInfo不可编辑
            string readmePath = Application.StartupPath + "\\readme.txt";
            // Optimized 
            string readmeContent = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(readmePath))
                {
                    readmeContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // 弹出异常提示对话框
                MessageBox.Show("帮助信息文件不存在，请检查！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

            this.rtxtHelpInfo.Text = readmeContent;
            this.rtxtHelpInfo.ReadOnly = true;
        }
    }
}
