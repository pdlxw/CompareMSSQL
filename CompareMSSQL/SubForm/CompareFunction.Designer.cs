namespace CompareMSSQL.SubForm
{
    partial class CompareFunction
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
            this.cmsFunction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsFunctionCurrentDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFunctionCurrentCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureSp1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFunctionAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFunctionAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProcedureSp2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFunctionColorAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFunctionColorAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSource.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.grpSql.SuspendLayout();
            this.cmsFunction.SuspendLayout();
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
            // cmsFunction
            // 
            this.cmsFunction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsFunctionCurrentDiff,
            this.cmsFunctionCurrentCreate,
            this.cmsProcedureSp1,
            this.cmsFunctionAllDiff,
            this.cmsFunctionAllCreate,
            this.cmsProcedureSp2,
            this.cmsFunctionColorAllDiff,
            this.cmsFunctionColorAllCreate});
            this.cmsFunction.Name = "cms_top";
            this.cmsFunction.Size = new System.Drawing.Size(197, 148);
            // 
            // cmsFunctionCurrentDiff
            // 
            this.cmsFunctionCurrentDiff.Name = "cmsFunctionCurrentDiff";
            this.cmsFunctionCurrentDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionCurrentDiff.Text = "当前差异脚本";
            this.cmsFunctionCurrentDiff.Click += new System.EventHandler(this.cmsFunctionCurrentDiff_Click);
            // 
            // cmsFunctionCurrentCreate
            // 
            this.cmsFunctionCurrentCreate.Name = "cmsFunctionCurrentCreate";
            this.cmsFunctionCurrentCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionCurrentCreate.Text = "当前创建脚本";
            this.cmsFunctionCurrentCreate.Click += new System.EventHandler(this.cmsFunctionCurrentCreate_Click);
            // 
            // cmsProcedureSp1
            // 
            this.cmsProcedureSp1.Name = "cmsProcedureSp1";
            this.cmsProcedureSp1.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsFunctionAllDiff
            // 
            this.cmsFunctionAllDiff.Name = "cmsFunctionAllDiff";
            this.cmsFunctionAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionAllDiff.Text = "全部差异脚本";
            this.cmsFunctionAllDiff.Click += new System.EventHandler(this.cmsFunctionAllDiff_Click);
            // 
            // cmsFunctionAllCreate
            // 
            this.cmsFunctionAllCreate.Name = "cmsFunctionAllCreate";
            this.cmsFunctionAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionAllCreate.Text = "全部创建脚本";
            this.cmsFunctionAllCreate.Click += new System.EventHandler(this.cmsFunctionAllCreate_Click);
            // 
            // cmsProcedureSp2
            // 
            this.cmsProcedureSp2.Name = "cmsProcedureSp2";
            this.cmsProcedureSp2.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsFunctionColorAllDiff
            // 
            this.cmsFunctionColorAllDiff.Name = "cmsFunctionColorAllDiff";
            this.cmsFunctionColorAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionColorAllDiff.Text = "当前颜色全部差异脚本";
            this.cmsFunctionColorAllDiff.Click += new System.EventHandler(this.cmsFunctionColorAllDiff_Click);
            // 
            // cmsFunctionColorAllCreate
            // 
            this.cmsFunctionColorAllCreate.Name = "cmsFunctionColorAllCreate";
            this.cmsFunctionColorAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsFunctionColorAllCreate.Text = "当前颜色全部创建脚本";
            this.cmsFunctionColorAllCreate.Click += new System.EventHandler(this.cmsFunctionColorAllCreate_Click);
            // 
            // CompareFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(632, 557);
            this.Name = "CompareFunction";
            this.Text = "CompareFunction";
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.grpTarget, 0);
            this.Controls.SetChildIndex(this.grpSql, 0);
            this.grpSource.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.grpSql.ResumeLayout(false);
            this.grpSql.PerformLayout();
            this.cmsFunction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsFunction;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionCurrentDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionCurrentCreate;
        private System.Windows.Forms.ToolStripSeparator cmsProcedureSp1;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionAllCreate;
        private System.Windows.Forms.ToolStripSeparator cmsProcedureSp2;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionColorAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsFunctionColorAllCreate;
    }
}