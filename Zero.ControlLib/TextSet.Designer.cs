namespace Zero.ControlLib
{
    partial class TextSet
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
            this.MainTableLayoutControl = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Unit = new System.Windows.Forms.Label();
            this.lbl_Value = new System.Windows.Forms.Label();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.MainTableLayoutControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutControl
            // 
            this.MainTableLayoutControl.ColumnCount = 3;
            this.MainTableLayoutControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.MainTableLayoutControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.MainTableLayoutControl.Controls.Add(this.lbl_Unit, 2, 0);
            this.MainTableLayoutControl.Controls.Add(this.lbl_Value, 1, 0);
            this.MainTableLayoutControl.Controls.Add(this.lbl_Title, 0, 0);
            this.MainTableLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutControl.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutControl.Name = "MainTableLayoutControl";
            this.MainTableLayoutControl.RowCount = 1;
            this.MainTableLayoutControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutControl.Size = new System.Drawing.Size(252, 41);
            this.MainTableLayoutControl.TabIndex = 0;
            // 
            // lbl_Unit
            // 
            this.lbl_Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Unit.Location = new System.Drawing.Point(204, 3);
            this.lbl_Unit.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Unit.Name = "lbl_Unit";
            this.lbl_Unit.Size = new System.Drawing.Size(45, 35);
            this.lbl_Unit.TabIndex = 2;
            this.lbl_Unit.Text = "℃";
            this.lbl_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Unit.DoubleClick += new System.EventHandler(this.lbl_Value_DoubleClick);
            // 
            // lbl_Value
            // 
            this.lbl_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Value.Location = new System.Drawing.Point(129, 3);
            this.lbl_Value.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Value.Name = "lbl_Value";
            this.lbl_Value.Size = new System.Drawing.Size(69, 35);
            this.lbl_Value.TabIndex = 1;
            this.lbl_Value.Text = "0.0";
            this.lbl_Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Value.DoubleClick += new System.EventHandler(this.lbl_Value_DoubleClick);
            // 
            // lbl_Title
            // 
            this.lbl_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Title.Location = new System.Drawing.Point(3, 3);
            this.lbl_Title.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(120, 35);
            this.lbl_Title.TabIndex = 0;
            this.lbl_Title.Text = "参数名称设置";
            this.lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Title.DoubleClick += new System.EventHandler(this.lbl_Value_DoubleClick);
            // 
            // TextSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutControl);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TextSet";
            this.Size = new System.Drawing.Size(252, 41);
            this.MainTableLayoutControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutControl;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.Label lbl_Unit;
        private System.Windows.Forms.Label lbl_Value;
    }
}
