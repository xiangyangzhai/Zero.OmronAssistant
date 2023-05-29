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

            // 更新进度条的最大值 最小值 当前值
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Maximum = CommonMethods.PLCVariables.Count;
                    progressBar1.Minimum = 0;
                    progressBar1.Value = 0;
                }));
            }
            else
            {
                progressBar1.Maximum = CommonMethods.PLCVariables.Count;
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
            }

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

            // 批量读取PLC变量
            foreach (var item in CommonMethods.PLCVariables)
            {
                // 利用linq获取 CommonMethods.PLCVariables_Show 中的变量
                var variable = CommonMethods.PLCVariables_Show.Where(v => v.Name == item.Name).FirstOrDefault();
                // 读取PLC变量
                variable.Value = cip.ReadSingleTag(item.Name, item.DataType).Content;

                // 更新进度条的当前值
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

                // 任务被取消
                token.ThrowIfCancellationRequested();
            }

            // 断开连接
            disConnect();

            // 隐藏进度条
            disProgressBar();
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
            this.CancelTask();
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
            // 从query中取值
            tagType = query.FirstOrDefault();
            cip.WriteSingleTag(tagName, tagType, tagValue);
        }
    }
}
