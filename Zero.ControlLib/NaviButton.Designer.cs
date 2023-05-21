namespace Zero.ControlLib
{
    partial class NaviButton
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pic_Main = new System.Windows.Forms.PictureBox();
            this.lbl_NaviName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Main
            // 
            this.pic_Main.Image = global::Zero.ControlLib.Properties.Resources.main;
            this.pic_Main.Location = new System.Drawing.Point(15, 12);
            this.pic_Main.Name = "pic_Main";
            this.pic_Main.Size = new System.Drawing.Size(36, 36);
            this.pic_Main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Main.TabIndex = 0;
            this.pic_Main.TabStop = false;
            this.pic_Main.Click += new System.EventHandler(this.lbl_NaviName_Click);
            // 
            // lbl_NaviName
            // 
            this.lbl_NaviName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_NaviName.ForeColor = System.Drawing.Color.White;
            this.lbl_NaviName.Location = new System.Drawing.Point(0, 0);
            this.lbl_NaviName.Name = "lbl_NaviName";
            this.lbl_NaviName.Size = new System.Drawing.Size(68, 71);
            this.lbl_NaviName.TabIndex = 1;
            this.lbl_NaviName.Text = "主界面";
            this.lbl_NaviName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lbl_NaviName.Click += new System.EventHandler(this.lbl_NaviName_Click);
            // 
            // NaviButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(69)))), ((int)(((byte)(163)))));
            this.Controls.Add(this.pic_Main);
            this.Controls.Add(this.lbl_NaviName);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NaviButton";
            this.Size = new System.Drawing.Size(68, 80);
            this.Click += new System.EventHandler(this.lbl_NaviName_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Main;
        private System.Windows.Forms.Label lbl_NaviName;
    }
}
