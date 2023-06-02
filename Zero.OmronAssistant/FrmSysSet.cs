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
using Zero.Models;

namespace Zero.OmronAssistant
{
    public partial class FrmSysSet : Form
    {
        public FrmSysSet()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // 打开文件对话框，默认路径为启动路径下的Settings文件夹下的Variables.txt文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath + "\\Settings";
            // Optimized
            // 过滤txt和所有文件
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "打开变量配置文件";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 清空CommonMethods.PLCVariables以及this.rtxtVariables.Text
                this.rtxtVariables.Text = string.Empty;
                CommonMethods.PLCVariables.Clear();

                // 在rtxtVariables中显示Variables.txt中的内容，并设置rtxtVariables不可编辑
                string variablesPath = openFileDialog.FileName;
                string variablesContent = string.Empty;
                try
                {

                    // 读取Variables.txt文件的所有行
                    var lines = File.ReadAllLines(variablesPath,Encoding.Default);

                    // 解析文件的每一行
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == 0) continue;

                        // 使用制表符（\t）分割每一行的内容
                        var columns = lines[i].Split('\t');

                        // 排除非taglink的标签
                        //if (columns[5] != "TRUE") continue;

                        // 创建一个新的PLC变量实例并设置其属性值
                        PLCVariable plcVariable = new PLCVariable
                        {
                            Name = columns[1],
                            DataType = columns[2],
                            Address = columns[3],
                            Comment = columns[4],
                            TagLink = columns[5],
                            RW = columns[6],
                            POU = columns[7]
                        };

                        // 将PLC变量实例添加到列表中
                        CommonMethods.PLCVariables.Add(plcVariable);

                    }
                    
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(variablesPath, Encoding.Default))
                    {

                        variablesContent = reader.ReadToEnd();
                    }
                    CommonMethods.PLCVariables_Show = CommonMethods.PLCVariables;
                }
                catch (Exception)
                {
                    // 弹出异常提示对话框
                    MessageBox.Show("打开变量配置文件失败，请检查！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.rtxtVariables.Text = variablesContent;
                this.rtxtVariables.ReadOnly = true;


                
            }
        }
    }
}
