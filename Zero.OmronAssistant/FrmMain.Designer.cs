namespace Zero.OmronAssistant
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.naviBtn_CloseWindow = new Zero.ControlLib.NaviButton();
            this.naviBtn_Info = new Zero.ControlLib.NaviButton();
            this.naviBtn_Settings = new Zero.ControlLib.NaviButton();
            this.naviBtn_Main = new Zero.ControlLib.NaviButton();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.TopPanel.Controls.Add(this.naviBtn_CloseWindow);
            this.TopPanel.Controls.Add(this.naviBtn_Info);
            this.TopPanel.Controls.Add(this.naviBtn_Settings);
            this.TopPanel.Controls.Add(this.naviBtn_Main);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1366, 80);
            this.TopPanel.TabIndex = 0;
            this.TopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDown);
            this.TopPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseMove);
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 728);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1366, 40);
            this.BottomPanel.TabIndex = 1;
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 80);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1366, 648);
            this.MainPanel.TabIndex = 2;
            // 
            // naviBtn_CloseWindow
            // 
            this.naviBtn_CloseWindow.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(243)))));
            this.naviBtn_CloseWindow.ActiveGap = 0;
            this.naviBtn_CloseWindow.ActiveHeight = 4;
            this.naviBtn_CloseWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.naviBtn_CloseWindow.ColorDepth = -0.2F;
            this.naviBtn_CloseWindow.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.naviBtn_CloseWindow.IsActive = false;
            this.naviBtn_CloseWindow.Location = new System.Drawing.Point(1295, 0);
            this.naviBtn_CloseWindow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.naviBtn_CloseWindow.Name = "naviBtn_CloseWindow";
            this.naviBtn_CloseWindow.NaviImage = global::Zero.OmronAssistant.Properties.Resources.exit;
            this.naviBtn_CloseWindow.NaviName = "退出系统";
            this.naviBtn_CloseWindow.Size = new System.Drawing.Size(68, 80);
            this.naviBtn_CloseWindow.TabIndex = 0;
            // 
            // naviBtn_Info
            // 
            this.naviBtn_Info.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(243)))));
            this.naviBtn_Info.ActiveGap = 0;
            this.naviBtn_Info.ActiveHeight = 4;
            this.naviBtn_Info.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.naviBtn_Info.ColorDepth = -0.2F;
            this.naviBtn_Info.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.naviBtn_Info.IsActive = false;
            this.naviBtn_Info.Location = new System.Drawing.Point(1219, 0);
            this.naviBtn_Info.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.naviBtn_Info.Name = "naviBtn_Info";
            this.naviBtn_Info.NaviImage = global::Zero.OmronAssistant.Properties.Resources.information;
            this.naviBtn_Info.NaviName = "帮助信息";
            this.naviBtn_Info.Size = new System.Drawing.Size(68, 80);
            this.naviBtn_Info.TabIndex = 0;
            // 
            // naviBtn_Settings
            // 
            this.naviBtn_Settings.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(243)))));
            this.naviBtn_Settings.ActiveGap = 0;
            this.naviBtn_Settings.ActiveHeight = 4;
            this.naviBtn_Settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.naviBtn_Settings.ColorDepth = -0.2F;
            this.naviBtn_Settings.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.naviBtn_Settings.IsActive = false;
            this.naviBtn_Settings.Location = new System.Drawing.Point(1143, 0);
            this.naviBtn_Settings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.naviBtn_Settings.Name = "naviBtn_Settings";
            this.naviBtn_Settings.NaviImage = global::Zero.OmronAssistant.Properties.Resources.setting;
            this.naviBtn_Settings.NaviName = "参数设置";
            this.naviBtn_Settings.Size = new System.Drawing.Size(68, 80);
            this.naviBtn_Settings.TabIndex = 0;
            // 
            // naviBtn_Main
            // 
            this.naviBtn_Main.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(243)))));
            this.naviBtn_Main.ActiveGap = 0;
            this.naviBtn_Main.ActiveHeight = 4;
            this.naviBtn_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.naviBtn_Main.ColorDepth = -0.2F;
            this.naviBtn_Main.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.naviBtn_Main.IsActive = false;
            this.naviBtn_Main.Location = new System.Drawing.Point(1067, 0);
            this.naviBtn_Main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.naviBtn_Main.Name = "naviBtn_Main";
            this.naviBtn_Main.NaviImage = global::Zero.OmronAssistant.Properties.Resources.main;
            this.naviBtn_Main.NaviName = "主界面";
            this.naviBtn_Main.Size = new System.Drawing.Size(68, 80);
            this.naviBtn_Main.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMain";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.TopPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel MainPanel;
        private ControlLib.NaviButton naviBtn_Main;
        private ControlLib.NaviButton naviBtn_Settings;
        private ControlLib.NaviButton naviBtn_CloseWindow;
        private ControlLib.NaviButton naviBtn_Info;
    }
}

