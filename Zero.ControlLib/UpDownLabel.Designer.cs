namespace Zero.ControlLib
{
    partial class UpDownLabel
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
            this.btn_Dec = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.lbl_Data = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Dec
            // 
            this.btn_Dec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(161)))), ((int)(((byte)(226)))));
            this.btn_Dec.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Dec.FlatAppearance.BorderSize = 0;
            this.btn_Dec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Dec.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Dec.ForeColor = System.Drawing.Color.White;
            this.btn_Dec.Location = new System.Drawing.Point(0, 0);
            this.btn_Dec.Name = "btn_Dec";
            this.btn_Dec.Size = new System.Drawing.Size(37, 37);
            this.btn_Dec.TabIndex = 0;
            this.btn_Dec.Text = "-";
            this.btn_Dec.UseVisualStyleBackColor = false;
            this.btn_Dec.Click += new System.EventHandler(this.btn_Dec_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(161)))), ((int)(((byte)(226)))));
            this.btn_Add.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Add.FlatAppearance.BorderSize = 0;
            this.btn_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Add.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Add.ForeColor = System.Drawing.Color.White;
            this.btn_Add.Location = new System.Drawing.Point(89, 0);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(37, 37);
            this.btn_Add.TabIndex = 1;
            this.btn_Add.Text = "+";
            this.btn_Add.UseVisualStyleBackColor = false;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // lbl_Data
            // 
            this.lbl_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Data.Location = new System.Drawing.Point(37, 0);
            this.lbl_Data.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Data.Name = "lbl_Data";
            this.lbl_Data.Size = new System.Drawing.Size(52, 37);
            this.lbl_Data.TabIndex = 2;
            this.lbl_Data.Text = "0";
            this.lbl_Data.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpDownLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lbl_Data);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.btn_Dec);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UpDownLabel";
            this.Size = new System.Drawing.Size(126, 37);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Dec;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Label lbl_Data;
    }
}
