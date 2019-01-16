namespace CompareMSSQL.SubForm
{
    partial class CommonWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer CommonComponents = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (CommonComponents != null))
            {
                CommonComponents.Dispose();
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
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.tvwSource = new System.Windows.Forms.TreeView();
            this.tvwTarget = new System.Windows.Forms.TreeView();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.grpSql = new System.Windows.Forms.GroupBox();
            this.btnCopySql = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.grpSource.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.grpSql.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.tvwSource);
            this.grpSource.Location = new System.Drawing.Point(4, 6);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(310, 350);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "源数据库";
            // 
            // tvwSource
            // 
            this.tvwSource.Location = new System.Drawing.Point(5, 21);
            this.tvwSource.Name = "tvwSource";
            this.tvwSource.Size = new System.Drawing.Size(300, 320);
            this.tvwSource.TabIndex = 0;
            // 
            // tvwTarget
            // 
            this.tvwTarget.Location = new System.Drawing.Point(5, 21);
            this.tvwTarget.Name = "tvwTarget";
            this.tvwTarget.Size = new System.Drawing.Size(300, 320);
            this.tvwTarget.TabIndex = 0;
            // 
            // grpTarget
            // 
            this.grpTarget.Controls.Add(this.tvwTarget);
            this.grpTarget.Location = new System.Drawing.Point(318, 6);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(310, 350);
            this.grpTarget.TabIndex = 1;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "目标数据库";
            // 
            // grpSql
            // 
            this.grpSql.Controls.Add(this.btnCopySql);
            this.grpSql.Controls.Add(this.txtSql);
            this.grpSql.Location = new System.Drawing.Point(4, 355);
            this.grpSql.Name = "grpSql";
            this.grpSql.Size = new System.Drawing.Size(624, 195);
            this.grpSql.TabIndex = 2;
            this.grpSql.TabStop = false;
            this.grpSql.Text = "脚本/消息";
            // 
            // btnCopySql
            // 
            this.btnCopySql.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCopySql.Location = new System.Drawing.Point(563, 165);
            this.btnCopySql.Name = "btnCopySql";
            this.btnCopySql.Size = new System.Drawing.Size(39, 23);
            this.btnCopySql.TabIndex = 1;
            this.btnCopySql.Text = "复制";
            this.btnCopySql.UseVisualStyleBackColor = true;
            this.btnCopySql.Click += new System.EventHandler(this.btnCopySql_Click);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(5, 16);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ReadOnly = true;
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(614, 173);
            this.txtSql.TabIndex = 0;
            // 
            // CommonWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 557);
            this.Controls.Add(this.grpSql);
            this.Controls.Add(this.grpTarget);
            this.Controls.Add(this.grpSource);
            this.Name = "CommonWin";
            this.Text = "CommonWin";
            this.grpSource.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.grpSql.ResumeLayout(false);
            this.grpSql.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox grpSource;
        protected System.Windows.Forms.TreeView tvwSource;
        protected System.Windows.Forms.TreeView tvwTarget;
        protected System.Windows.Forms.GroupBox grpTarget;
        protected System.Windows.Forms.GroupBox grpSql;
        protected System.Windows.Forms.TextBox txtSql;
        protected System.Windows.Forms.Button btnCopySql;
    }
}