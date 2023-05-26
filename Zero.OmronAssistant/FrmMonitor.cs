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

        private CancellationTokenSource _tokenSource;

        public void StartTask()
        {
            // 如果存在旧的 CancellationTokenSource，首先释放它
            _tokenSource?.Dispose();
            // 创建新的 CancellationTokenSource
            _tokenSource = new CancellationTokenSource();

            var token = _tokenSource.Token;

            Task.Run(() =>
            {
                //对CommonMethods
                // 这是一个耗时的任务，一直进行通信
                              
                
            }, token);

        }

        public void CancelTask()
        {
            // 请求取消任务
            _tokenSource?.Cancel();
        }
        private void FrmMonitor_Load(object sender, EventArgs e)
        {
            //开启多线程任务，与plc保持连接，持续更新全局变量的值
            this.StartTask();
            //启动定时器更新
            this.StartTick();
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
    }
}
