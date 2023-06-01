using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using Zero.ControlLib;
using Zero.CommunicationLib;
using Zero.CommunicationLib.Library;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using Zero.Models;
using System.Configuration.Assemblies;

namespace Zero.OmronAssistant
{
    public partial class FrmMonitor : Form
    {
        public FrmMonitor()
        {
            InitializeComponent();

            dgvVariables.AutoGenerateColumns = false;
            //将CommonMethods.PLCVariables作为dgvVariables的数据源
            this.dgvVariables.DataSource = CommonMethods.PLCVariables_Show;
            this.updateTimer.Interval = 2000;
            this.updateTimer.Tick += UpdateTimer_Tick;

            this.Load += FrmMonitor_Load;
        }

        private CIP cip = new CIP();

        private CancellationTokenSource _tokenSource;

        private CancellationTokenSource _writeTokenSource;

        public void StartTask()
        {
            // 如果存在旧的 CancellationTokenSource，首先释放它
            _tokenSource?.Dispose();
            // 创建新的 CancellationTokenSource
            _tokenSource = new CancellationTokenSource();

            var token = _tokenSource.Token;
            var task = Task.Run(() =>
            {
                ReadVariablesData(token);

            }, token);

            task.ContinueWith(t =>
            {
                // 通信完成，利用对话框提示
                MessageBox.Show("通信完成");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // 任务取消时，对进度条进行隐藏并清零，并调用btnDisConnect_Click
            task.ContinueWith(t =>
            {
                disProgressBar();

                disConnect();
            }, TaskContinuationOptions.OnlyOnCanceled);
        }

        private void ReadVariablesData(CancellationToken token)
        {
            // 显示进度条
            ShowProgressBar();

            // 更新进度条的最大值 最小值 当前值
            InitionalizeProgressBar(CommonMethods.PLCVariables);

            //断开之前的连接
            disConnect();

            // 连接PLC
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect_Click(null, null);
                }));
            }
            else
            {
                btnConnect.PerformClick();
            }

            //注册会话
            if (btnRegist.InvokeRequired)
            {
                btnRegist.Invoke(new MethodInvoker(delegate
                {
                    btnRegist_Click(null, null);
                }));
            }
            else
            {
                btnRegist.PerformClick();
            }
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directoryPath = System.IO.Path.GetDirectoryName(assemblyPath);
            string logFilePath = directoryPath + "\\log\\Read.log";

            string logEntry = string.Empty;
            // 批量读取PLC变量
            foreach (var item in CommonMethods.PLCVariables)
            {
                // 利用linq获取 CommonMethods.PLCVariables_Show 中的变量
                var variable = CommonMethods.PLCVariables_Show.Where(v => v.Name == item.Name).FirstOrDefault();

                // 对变量进行判断，判断是否为数组
                if (item.DataType.Contains("["))
                {
                    var dataType = CIP.ParseTagType(item.DataType);

                    string result = string.Empty;
                    // 读取PLC数组变量
                    for (int i = dataType.Content2; i <= dataType.Content3; i++)
                    {
                        var res = cip.ReadSingleTag($"{item.Name}" + "[" + i + "]", dataType.Content1).Content;
                        result = result + res + ",";
                        if(res == "读取失败")
                        {
                            logEntry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $", {item.Name}" + "[" + i + "]" + ", " + res;
                            System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                        }
                        
                    }
                    variable.Value = result;
                    continue;
                }
                // 读取PLC变量
                variable.Value = cip.ReadSingleTag(item.Name, item.DataType).Content;
                if (variable.Value == "读取失败")
                {
                    logEntry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +", " + item.Name + ", " + variable.Value;
                    System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
                // 更新进度条的当前值
                UpdateProgressBar();

                // 任务被取消
                token.ThrowIfCancellationRequested();
            }

            // 断开连接
            disConnect();

            // 隐藏进度条
            disProgressBar();
        }

        private void InitionalizeProgressBar(List<PLCVariable> lists)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Maximum = lists.Count;
                    progressBar1.Minimum = 0;
                    progressBar1.Value = 0;
                }));
            }
            else
            {
                progressBar1.Maximum = lists.Count;
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
            }
        }

        private void ShowProgressBar()
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Visible = true;
                }));
            }
            else
            {
                progressBar1.Visible = true;
            }
        }

        private void UpdateProgressBar()
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Value++;
                }));
            }
            else
            {
                progressBar1.Value++;
            }
        }

        private void disProgressBar()
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }));
            }
            else
            {
                progressBar1.Visible = false;
                progressBar1.Value = 0;
            }
        }

        private void disConnect()
        {
            if (btnDisConnect.InvokeRequired)
            {
                btnDisConnect.Invoke(new MethodInvoker(delegate
                {
                    btnDisConnect_Click(null, null);
                }));
            }
            else
            {
                btnDisConnect.PerformClick();
            }
        }

        public void CancelTask()
        {
            // 请求取消任务
            _tokenSource?.Cancel();
        }
        private void FrmMonitor_Load(object sender, EventArgs e)
        {
            //开启多线程任务，与plc保持连接，持续更新全局变量的值
            //this.StartTask();
            //启动定时器更新
            this.StartTick();
            this.btnRegist.Enabled = false;
            this.btnDisConnect.Enabled = false;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            //对dgv的数据源进行更新
            //if(this.dgvVariables.DataSource != null)
            //{
            //    this.dgvVariables.Refresh();
            //}
            //else
            //{
            //    this.dgvVariables.DataSource = null;
            //    this.dgvVariables.DataSource = CommonMethods.PLCVariables;
            //}
            this.dgvVariables.DataSource = null;
            this.dgvVariables.DataSource = CommonMethods.PLCVariables_Show;

        }

        // 添加一个定时器，通过CIP协议定时更新CommonMethods.PLCVariables的值
        private Timer updateTimer = new Timer();

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(CommonMethods.PLCVariables.Count == 0)
            {
                MessageBox.Show("请先添加变量");
                return;
            }
            this.StartTask();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            // 弹窗确认是否进行取消任务
            var res = MessageBox.Show("是否取消任务", "提示", MessageBoxButtons.OKCancel);
            if(res == DialogResult.OK)
            {
                this.CancelTask();
            }
        }

        private void StartTick()
        {
            this.updateTimer.Start();
        }

        private void StopTick()
        {
            this.updateTimer.Stop();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.StartTick();
        }

        private void btnFreeze_Click(object sender, EventArgs e)
        {
            this.StopTick();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //
            if( cip.Connect(txtIP.Text.Trim(),Int32.Parse(txtPort.Text)))
            {
                btnConnect.Enabled = false;
                btnRegist.Enabled = true;
                btnDisConnect.Enabled = true;
            }

        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            cip.Regist();
            txtSessionHandle.Text = string.Empty;
            // 将 cip.SessionHandle 转换为 空格隔开的字符串
            foreach (var item in cip.SessionHandle)
            {
                txtSessionHandle.Text += item.ToString("X2") + " ";
            }

        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (!cip.Connected) return;
            cip.Unregist();
            cip.DisConnect();
            cip.SessionHandle = null;
            txtSessionHandle.Text = string.Empty;
            btnConnect.Enabled = true;
            btnRegist.Enabled = false;
            btnDisConnect.Enabled = false;
        }

        private void btnReadTheTag_Click(object sender, EventArgs e)
        {
            txtTagValue.Text = cip.ReadSingleTag(txtTagName.Text.Trim(), string.Empty).Content;
        }

        private void btnWriteTagValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (CommonMethods.PLCVariables_Show.Count != 0)
                {
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请先配置全局变量，再进行写入操作！");
                return;
                
            }


            
            string tagName = txtTagNameForWrite.Text.Trim();
            string tagValue = txtTagValueForWrite.Text.Trim().ToUpper();
            string tagType = string.Empty;
            // 根据 tagName 在 CommonMethods.PLCVariables_Show 中查找对应的 tagType，使用linq
            var query = from item in CommonMethods.PLCVariables_Show
                        where item.Name == tagName
                        select item.DataType;
            if (tagName.Contains("["))
            {
                query = from item in CommonMethods.PLCVariables_Show
                        where item.Name == tagName.Substring(0, tagName.IndexOf("["))
                        select item.DataType;
                tagType = query.FirstOrDefault();
                // 从query中取值
                if (!string.IsNullOrEmpty(tagType))
                {
                    if (tagType.Contains("["))
                    {
                        tagType = tagType.Substring(0, tagType.IndexOf("["));

                    }
                    if (tagType.Contains("("))
                    {
                        tagType = tagType.Substring(0, tagType.IndexOf("("));
                    }
                }
            }
            else
            {
                tagType = query.FirstOrDefault();
                
            }
            if (tagType.StartsWith("STRING"))
            {
                tagType = "STRING";
                //for(int i=0;i<tagValue.Length;i++)
                //{
                //    cip.WriteSingleTag(tagName+"["+i+"]", "BYTE", tagValue[i].ToString());
                //}
            }
            cip.WriteSingleTag(tagName, tagType, tagValue);
        }

        private void btnSaveJson_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, CommonMethods.PLCVariables_Show);
                }
            }
            //提示保存成功
            MessageBox.Show("保存成功");
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    CommonMethods.PLCVariables_JSON = (List<PLCVariable>)serializer.Deserialize(file, typeof(List<PLCVariable>));
                }
            }

            // 将 CommonMethods.PLCVariables_JSON 写入到 PLC
            // 如果存在旧的 CancellationTokenSource，首先释放它
            _writeTokenSource?.Dispose();
            // 创建新的 CancellationTokenSource
            _writeTokenSource = new CancellationTokenSource();

            var token = _writeTokenSource.Token;
            var task = Task.Run(() =>
            {
                WriteVariablesData(token);

            }, token);

            task.ContinueWith(t =>
            {
                // 通信完成，利用对话框提示
                MessageBox.Show("通信完成");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // 任务取消时，对进度条进行隐藏并清零，并调用btnDisConnect_Click
            task.ContinueWith(t =>
            {
                disProgressBar();

                disConnect();
            }, TaskContinuationOptions.OnlyOnCanceled);
        }

        private void WriteVariablesData(CancellationToken token)
        {
            // 显示进度条
            ShowProgressBar();

            // 更新进度条的最大值 最小值 当前值
            InitionalizeProgressBar(CommonMethods.PLCVariables_JSON);

            //断开之前的连接
            disConnect();

            // 连接PLC
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(new MethodInvoker(delegate
                {
                    btnConnect_Click(null, null);
                }));
            }
            else
            {
                btnConnect.PerformClick();
            }

            //注册会话
            if (btnRegist.InvokeRequired)
            {
                btnRegist.Invoke(new MethodInvoker(delegate
                {
                    btnRegist_Click(null, null);
                }));
            }
            else
            {
                btnRegist.PerformClick();
            }
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directoryPath = System.IO.Path.GetDirectoryName(assemblyPath);
            string logFilePath = directoryPath + "\\log\\Write.log";
            string result = string.Empty;
            string logEntry = string.Empty;
            // 批量写入PLC变量
            foreach (var item in CommonMethods.PLCVariables_JSON)
            {
                // 更新进度条的当前值
                UpdateProgressBar();
                // 利用linq获取 CommonMethods.PLCVariables_JSON 中的变量
                var variable = CommonMethods.PLCVariables_JSON.Where(v => v.Name == item.Name).FirstOrDefault();

                if (variable.Value == "读取失败")
                {
                    continue;
                }

                if (item.Name.Contains("[") && item.Name[item.Name.Length - 1] != ']' && !item.DataType.Contains("["))
                {
                    result = cip.WriteSingleTag(item.Name, item.DataType, item.Value).Content;
                    logEntry = LogToText2(logFilePath, result, logEntry, item);
                    continue;
                }
                // 对变量进行判断，判断是否为数组
                if (item.DataType.Contains("["))
                {
                    var dataType = CIP.ParseTagType(item.DataType);

                    //判断 tagName最后一个字符是否是"]"


                    string[] values = item.Value.Split(',');
                    // 写入PLC数组变量
                    for (int i = dataType.Content2; i <= dataType.Content3; i++)
                    {
                        result = cip.WriteSingleTag($"{item.Name}" + "[" + i + "]", dataType.Content1, values[i - dataType.Content2]).Content;
                        logEntry = LogToText(logFilePath, result, logEntry, item, i);

                    }
                    continue;
                }
                // 写入PLC变量
                result = cip.WriteSingleTag(item.Name, item.DataType, item.Value).Content;
                logEntry = LogToText2(logFilePath, result, logEntry, item);
                

                // 任务被取消
                token.ThrowIfCancellationRequested();
            }

            // 断开连接
            disConnect();

            // 隐藏进度条
            disProgressBar();
        }

        private static string LogToText2(string logFilePath, string result, string logEntry, PLCVariable item)
        {
            if (result != "写入成功")
            {
                logEntry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", " + item.Name + ", " + "写入失败";
                System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }

            return logEntry;
        }

        private static string LogToText(string logFilePath, string result, string logEntry, PLCVariable item, int i)
        {
            if (result != "写入成功")
            {
                logEntry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $", {item.Name}" + "[" + i + "]" + ", " + "写入失败";
                System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }

            return logEntry;
        }

        private void btnStopRestore_Click(object sender, EventArgs e)
        {
            // 弹窗确认是否请求取消任务
            if (MessageBox.Show("是否取消任务？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            // 取消任务
            _writeTokenSource?.Cancel();
        }
    }
}
