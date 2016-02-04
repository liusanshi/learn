namespace FireFiddler
{
    partial class UI
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cb_disabled = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tv_view = new System.Windows.Forms.TreeView();
            this.ruleTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ruleTypesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_disabled
            // 
            this.cb_disabled.AutoSize = true;
            this.cb_disabled.Location = new System.Drawing.Point(15, 0);
            this.cb_disabled.Name = "cb_disabled";
            this.cb_disabled.Size = new System.Drawing.Size(72, 16);
            this.cb_disabled.TabIndex = 0;
            this.cb_disabled.Text = "是否启用";
            this.cb_disabled.UseVisualStyleBackColor = true;
            this.cb_disabled.CheckedChanged += new System.EventHandler(this.cb_disabled_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tv_view);
            this.groupBox1.Controls.Add(this.cb_disabled);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1001, 704);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // tv_view
            // 
            this.tv_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_view.Location = new System.Drawing.Point(3, 17);
            this.tv_view.Name = "tv_view";
            this.tv_view.Size = new System.Drawing.Size(995, 684);
            this.tv_view.TabIndex = 1;
            // 
            // ruleTypesBindingSource
            // 
            this.ruleTypesBindingSource.DataSource = typeof(FireFiddler.RuleTypes);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "UI";
            this.Size = new System.Drawing.Size(1001, 704);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ruleTypesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_disabled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.BindingSource ruleTypesBindingSource;
        private System.Windows.Forms.TreeView tv_view;


    }
}
