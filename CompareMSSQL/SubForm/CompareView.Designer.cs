namespace CompareMSSQL.SubForm
{
    partial class CompareView
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
            this.cmsView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsViewCurrentDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewCurrentCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewSp1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsViewAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewSp2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsViewColorAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewColorAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSource.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.grpSql.SuspendLayout();
            this.cmsView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwSource
            // 
            this.tvwSource.LineColor = System.Drawing.Color.Black;
            // 
            // tvwTarget
            // 
            this.tvwTarget.LineColor = System.Drawing.Color.Black;
            // 
            // cmsView
            // 
            this.cmsView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsViewCurrentDiff,
            this.cmsViewCurrentCreate,
            this.cmsViewSp1,
            this.cmsViewAllDiff,
            this.cmsViewAllCreate,
            this.cmsViewSp2,
            this.cmsViewColorAllDiff,
            this.cmsViewColorAllCreate});
            this.cmsView.Name = "cms_top";
            this.cmsView.Size = new System.Drawing.Size(197, 170);
            // 
            // cmsViewCurrentDiff
            // 
            this.cmsViewCurrentDiff.Name = "cmsViewCurrentDiff";
            this.cmsViewCurrentDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsViewCurrentDiff.Text = "当前差异脚本";
            this.cmsViewCurrentDiff.Click += new System.EventHandler(this.cmsViewCurrentDiff_Click);
            // 
            // cmsViewCurrentCreate
            // 
            this.cmsViewCurrentCreate.Name = "cmsViewCurrentCreate";
            this.cmsViewCurrentCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsViewCurrentCreate.Text = "当前创建脚本";
            this.cmsViewCurrentCreate.Click += new System.EventHandler(this.cmsViewCurrentCreate_Click);
            // 
            // cmsViewSp1
            // 
            this.cmsViewSp1.Name = "cmsViewSp1";
            this.cmsViewSp1.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsViewAllDiff
            // 
            this.cmsViewAllDiff.Name = "cmsViewAllDiff";
            this.cmsViewAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsViewAllDiff.Text = "全部差异脚本";
            this.cmsViewAllDiff.Click += new System.EventHandler(this.cmsViewAllDiff_Click);
            // 
            // cmsViewAllCreate
            // 
            this.cmsViewAllCreate.Name = "cmsViewAllCreate";
            this.cmsViewAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsViewAllCreate.Text = "全部创建脚本";
            this.cmsViewAllCreate.Click += new System.EventHandler(this.cmsViewAllCreate_Click);
            // 
            // cmsViewSp2
            // 
            this.cmsViewSp2.Name = "cmsViewSp2";
            this.cmsViewSp2.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsViewColorAllDiff
            // 
            this.cmsViewColorAllDiff.Name = "cmsViewColorAllDiff";
            this.cmsViewColorAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsViewColorAllDiff.Text = "当前颜色全部差异脚本";
            this.cmsViewColorAllDiff.Click += new System.EventHandler(this.cmsViewColorAllDiff_Click);
            // 
            // cmsViewColorAllCreate
            // 
            this.cmsViewColorAllCreate.Name = "cmsViewColorAllCreate";
            this.cmsViewColorAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsViewColorAllCreate.Text = "当前颜色全部创建脚本";
            this.cmsViewColorAllCreate.Click += new System.EventHandler(this.cmsViewColorAllCreate_Click);
            //
            //Handler
            //
            this.tvwTarget.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwTarget_NodeMouseClick);
            this.tvwSource.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwSource_NodeMouseClick);
            // 
            // CompareView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(632, 557);
            this.Name = "CompareView";
            this.Text = "CompareView";
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.grpTarget, 0);
            this.Controls.SetChildIndex(this.grpSql, 0);
            this.grpSource.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.grpSql.ResumeLayout(false);
            this.grpSql.PerformLayout();
            this.cmsView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsView;
        private System.Windows.Forms.ToolStripMenuItem cmsViewCurrentDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsViewCurrentCreate;
        private System.Windows.Forms.ToolStripSeparator cmsViewSp1;
        private System.Windows.Forms.ToolStripMenuItem cmsViewAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsViewAllCreate;
        private System.Windows.Forms.ToolStripSeparator cmsViewSp2;
        private System.Windows.Forms.ToolStripMenuItem cmsViewColorAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsViewColorAllCreate;
    }
}