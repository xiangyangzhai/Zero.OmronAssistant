namespace Zero.OmronAssistant
{
    partial class FrmHelpInfo
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
            this.rtxtHelpInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtxtHelpInfo
            // 
            this.rtxtHelpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtHelpInfo.Location = new System.Drawing.Point(0, 0);
            this.rtxtHelpInfo.Name = "rtxtHelpInfo";
            this.rtxtHelpInfo.Size = new System.Drawing.Size(1366, 648);
            this.rtxtHelpInfo.TabIndex = 0;
            this.rtxtHelpInfo.Text = "";
            // 
            // FrmHelpInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 648);
            this.Controls.Add(this.rtxtHelpInfo);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmHelpInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "帮助信息";
            this.Load += new System.EventHandler(this.FrmHelpInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtHelpInfo;
    }
}