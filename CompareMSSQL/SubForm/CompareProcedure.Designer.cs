namespace CompareMSSQL.SubForm
{
    partial class CompareProcedure
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
            this.components = new System.ComponentModel.Container();
            this.cmsProcedure = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsProcedureCurrentDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureCurrentCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureSp1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsProcedureAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureSp2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsProcedureColorAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureColorAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSource.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.grpSql.SuspendLayout();
            this.cmsProcedure.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwSource
            // 
            this.tvwSource.LineColor = System.Drawing.Color.Black;
            this.tvwSource.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwSource_NodeMouseClick);
            // 
            // tvwTarget
            // 
            this.tvwTarget.LineColor = System.Drawing.Color.Black;
            this.tvwTarget.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwTarget_NodeMouseClick);
            // 
            // cmsProcedure
            // 
            this.cmsProcedure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsProcedureCurrentDiff,
            this.cmsProcedureCurrentCreate,
            this.cmsProcedureSp1,
            this.cmsProcedureAllDiff,
            this.cmsProcedureAllCreate,
            this.cmsProcedureSp2,
            this.cmsProcedureColorAllDiff,
            this.cmsProcedureColorAllCreate});
            this.cmsProcedure.Name = "cms_top";
            this.cmsProcedure.Size = new System.Drawing.Size(197, 170);
            // 
            // cmsProcedureCurrentDiff
            // 
            this.cmsProcedureCurrentDiff.Name = "cmsProcedureCurrentDiff";
            this.cmsProcedureCurrentDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureCurrentDiff.Text = "当前差异脚本";
            this.cmsProcedureCurrentDiff.Click += new System.EventHandler(this.cmsProcedureCurrentDiff_Click);
            // 
            // cmsProcedureCurrentCreate
            // 
            this.cmsProcedureCurrentCreate.Name = "cmsProcedureCurrentCreate";
            this.cmsProcedureCurrentCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureCurrentCreate.Text = "当前创建脚本";
            this.cmsProcedureCurrentCreate.Click += new System.EventHandler(this.cmsProcedureCurrentCreate_Click);
            // 
            // cmsProcedureSp1
            // 
            this.cmsProcedureSp1.Name = "cmsProcedureSp1";
            this.cmsProcedureSp1.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsProcedureAllDiff
            // 
            this.cmsProcedureAllDiff.Name = "cmsProcedureAllDiff";
            this.cmsProcedureAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureAllDiff.Text = "全部差异脚本";
            this.cmsProcedureAllDiff.Click += new System.EventHandler(this.cmsProcedureAllDiff_Click);
            // 
            // cmsProcedureAllCreate
            // 
            this.cmsProcedureAllCreate.Name = "cmsProcedureAllCreate";
            this.cmsProcedureAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureAllCreate.Text = "全部创建脚本";
            this.cmsProcedureAllCreate.Click += new System.EventHandler(this.cmsProcedureAllCreate_Click);
            // 
            // cmsProcedureSp2
            // 
            this.cmsProcedureSp2.Name = "cmsProcedureSp2";
            this.cmsProcedureSp2.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsProcedureColorAllDiff
            // 
            this.cmsProcedureColorAllDiff.Name = "cmsProcedureColorAllDiff";
            this.cmsProcedureColorAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureColorAllDiff.Text = "当前颜色全部差异脚本";
            this.cmsProcedureColorAllDiff.Click += new System.EventHandler(this.cmsProcedureColorAllDiff_Click);
            // 
            // cmsProcedureColorAllCreate
            // 
            this.cmsProcedureColorAllCreate.Name = "cmsProcedureColorAllCreate";
            this.cmsProcedureColorAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsProcedureColorAllCreate.Text = "当前颜色全部创建脚本";
            this.cmsProcedureColorAllCreate.Click += new System.EventHandler(this.cmsProcedureColorAllCreate_Click);
            // 
            // CompareProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(632, 557);
            this.Name = "CompareProcedure";
            this.Text = "CompareProcedure";
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.grpTarget, 0);
            this.Controls.SetChildIndex(this.grpSql, 0);
            this.grpSource.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.grpSql.ResumeLayout(false);
            this.grpSql.PerformLayout();
            this.cmsProcedure.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsProcedure;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureCurrentDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureCurrentCreate;
        private System.Windows.Forms.ToolStripSeparator cmsProcedureSp1;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureAllCreate;
        private System.Windows.Forms.ToolStripSeparator cmsProcedureSp2;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureColorAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsProcedureColorAllCreate;
    }
}