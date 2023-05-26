namespace Zero.OmronAssistant
{
    partial class FrmMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.txtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnFreeze = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.PanelCommunication = new Zero.ControlLib.HeadPanel();
            this.PanelTest = new Zero.ControlLib.HeadPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTagValue = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRegist = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnReadTheTag = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.PanelCommunication.SuspendLayout();
            this.PanelTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToAddRows = false;
            this.dgvVariables.AllowUserToDeleteRows = false;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtName,
            this.DataType,
            this.Address,
            this.Comment,
            this.TagLink,
            this.RW,
            this.POU,
            this.Value});
            this.dgvVariables.Location = new System.Drawing.Point(501, 53);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.RowTemplate.Height = 23;
            this.dgvVariables.Size = new System.Drawing.Size(853, 583);
            this.dgvVariables.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.DataPropertyName = "Name";
            this.txtName.HeaderText = "Name";
            this.txtName.Name = "txtName";
            // 
            // DataType
            // 
            this.DataType.DataPropertyName = "DataType";
            this.DataType.HeaderText = "DataType";
            this.DataType.Name = "DataType";
            // 
            // Address
            // 
            this.Address.DataPropertyName = "Address";
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            // 
            // Comment
            // 
            this.Comment.DataPropertyName = "Comment";
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            // 
            // TagLink
            // 
            this.TagLink.DataPropertyName = "TagLink";
            this.TagLink.HeaderText = "TagLink";
            this.TagLink.Name = "TagLink";
            // 
            // RW
            // 
            this.RW.DataPropertyName = "RW";
            this.RW.HeaderText = "RW";
            this.RW.Name = "RW";
            // 
            // POU
            // 
            this.POU.DataPropertyName = "POU";
            this.POU.HeaderText = "POU";
            this.POU.Name = "POU";
            // 
            // Value
            // 
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(1145, 13);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 34);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始通信";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(1226, 12);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 35);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "停止通信";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnFreeze
            // 
            this.btnFreeze.Location = new System.Drawing.Point(1064, 13);
            this.btnFreeze.Name = "btnFreeze";
            this.btnFreeze.Size = new System.Drawing.Size(75, 34);
            this.btnFreeze.TabIndex = 1;
            this.btnFreeze.Text = "停止刷新";
            this.btnFreeze.UseVisualStyleBackColor = true;
            this.btnFreeze.Click += new System.EventHandler(this.btnFreeze_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(983, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 34);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新表格";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // PanelCommunication
            // 
            this.PanelCommunication.BorderColor = System.Drawing.Color.Black;
            this.PanelCommunication.BorderWidth = 1;
            this.PanelCommunication.Controls.Add(this.btnDisConnect);
            this.PanelCommunication.Controls.Add(this.btnRegist);
            this.PanelCommunication.Controls.Add(this.btnConnect);
            this.PanelCommunication.Controls.Add(this.textBox3);
            this.PanelCommunication.Controls.Add(this.txtIP);
            this.PanelCommunication.Controls.Add(this.txtPort);
            this.PanelCommunication.Controls.Add(this.label2);
            this.PanelCommunication.Controls.Add(this.label3);
            this.PanelCommunication.Controls.Add(this.label1);
            this.PanelCommunication.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PanelCommunication.LinearGradientRate = 0.5F;
            this.PanelCommunication.Location = new System.Drawing.Point(12, 53);
            this.PanelCommunication.Name = "PanelCommunication";
            this.PanelCommunication.Size = new System.Drawing.Size(483, 200);
            this.PanelCommunication.TabIndex = 2;
            this.PanelCommunication.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.PanelCommunication.ThemeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.PanelCommunication.ThemeForeColor = System.Drawing.Color.White;
            this.PanelCommunication.TitleHeight = 30;
            this.PanelCommunication.TitleText = "通信";
            // 
            // PanelTest
            // 
            this.PanelTest.BorderColor = System.Drawing.Color.Black;
            this.PanelTest.BorderWidth = 1;
            this.PanelTest.Controls.Add(this.label5);
            this.PanelTest.Controls.Add(this.label4);
            this.PanelTest.Controls.Add(this.btnReadTheTag);
            this.PanelTest.Controls.Add(this.txtTagValue);
            this.PanelTest.Controls.Add(this.txtTagName);
            this.PanelTest.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PanelTest.LinearGradientRate = 0.5F;
            this.PanelTest.Location = new System.Drawing.Point(12, 287);
            this.PanelTest.Name = "PanelTest";
            this.PanelTest.Size = new System.Drawing.Size(483, 200);
            this.PanelTest.TabIndex = 2;
            this.PanelTest.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.PanelTest.ThemeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.PanelTest.ThemeForeColor = System.Drawing.Color.White;
            this.PanelTest.TitleHeight = 30;
            this.PanelTest.TitleText = "测试";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(48, 79);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(193, 26);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "44818";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(47, 48);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(194, 26);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.1.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "会话句柄";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.Location = new System.Drawing.Point(318, 48);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(162, 26);
            this.textBox3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "标签名称";
            // 
            // txtTagName
            // 
            this.txtTagName.Location = new System.Drawing.Point(75, 47);
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.Size = new System.Drawing.Size(405, 26);
            this.txtTagName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "标签值";
            // 
            // txtTagValue
            // 
            this.txtTagValue.BackColor = System.Drawing.SystemColors.Control;
            this.txtTagValue.Location = new System.Drawing.Point(75, 86);
            this.txtTagValue.Name = "txtTagValue";
            this.txtTagValue.ReadOnly = true;
            this.txtTagValue.Size = new System.Drawing.Size(405, 26);
            this.txtTagValue.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(47, 125);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 34);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnRegist
            // 
            this.btnRegist.Location = new System.Drawing.Point(128, 125);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(75, 34);
            this.btnRegist.TabIndex = 2;
            this.btnRegist.Text = "注册会话";
            this.btnRegist.UseVisualStyleBackColor = true;
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(209, 125);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(75, 34);
            this.btnDisConnect.TabIndex = 2;
            this.btnDisConnect.Text = "断开";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            // 
            // btnReadTheTag
            // 
            this.btnReadTheTag.Location = new System.Drawing.Point(75, 131);
            this.btnReadTheTag.Name = "btnReadTheTag";
            this.btnReadTheTag.Size = new System.Drawing.Size(128, 34);
            this.btnReadTheTag.TabIndex = 2;
            this.btnReadTheTag.Text = "读取当前标签值";
            this.btnReadTheTag.UseVisualStyleBackColor = true;
            // 
            // FrmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 648);
            this.Controls.Add(this.PanelTest);
            this.Controls.Add(this.PanelCommunication);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnFreeze);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dgvVariables);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.PanelCommunication.ResumeLayout(false);
            this.PanelCommunication.PerformLayout();
            this.PanelTest.ResumeLayout(false);
            this.PanelTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnFreeze;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagLink;
        private System.Windows.Forms.DataGridViewTextBoxColumn RW;
        private System.Windows.Forms.DataGridViewTextBoxColumn POU;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private ControlLib.HeadPanel PanelCommunication;
        private ControlLib.HeadPanel PanelTest;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTagName;
        private System.Windows.Forms.TextBox txtTagValue;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnReadTheTag;
    }
}