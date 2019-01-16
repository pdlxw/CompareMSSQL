namespace CompareMSSQL.SubForm
{
    partial class CompareTable
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
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsTableCurrentDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTableCurrentCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTableSp1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsTableAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTableAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTableSp2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsTableColorAllDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTableColorAllCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSource.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.grpSql.SuspendLayout();
            this.cmsTable.SuspendLayout();
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
            // cmsTable
            // 
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsTableCurrentDiff,
            this.cmsTableCurrentCreate,
            this.cmsTableSp1,
            this.cmsTableAllDiff,
            this.cmsTableAllCreate,
            this.cmsTableSp2,
            this.cmsTableColorAllDiff,
            this.cmsTableColorAllCreate});
            this.cmsTable.Name = "cms_top";
            this.cmsTable.Size = new System.Drawing.Size(197, 170);
            // 
            // cmsTableCurrentDiff
            // 
            this.cmsTableCurrentDiff.Name = "cmsTableCurrentDiff";
            this.cmsTableCurrentDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsTableCurrentDiff.Text = "当前差异脚本";
            this.cmsTableCurrentDiff.Click += new System.EventHandler(this.cmsTableCurrentDiff_Click);
            // 
            // cmsTableCurrentCreate
            // 
            this.cmsTableCurrentCreate.Name = "cmsTableCurrentCreate";
            this.cmsTableCurrentCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsTableCurrentCreate.Text = "当前创建脚本";
            this.cmsTableCurrentCreate.Click += new System.EventHandler(this.cmsTableCurrentCreate_Click);
            // 
            // cmsTableSp1
            // 
            this.cmsTableSp1.Name = "cmsTableSp1";
            this.cmsTableSp1.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsTableAllDiff
            // 
            this.cmsTableAllDiff.Name = "cmsTableAllDiff";
            this.cmsTableAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsTableAllDiff.Text = "全部差异脚本";
            this.cmsTableAllDiff.Click += new System.EventHandler(this.cmsTableAllDiff_Click);
            // 
            // cmsTableAllCreate
            // 
            this.cmsTableAllCreate.Name = "cmsTableAllCreate";
            this.cmsTableAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsTableAllCreate.Text = "全部创建脚本";
            this.cmsTableAllCreate.Click += new System.EventHandler(this.cmsTableAllCreate_Click);
            // 
            // cmsTableSp2
            // 
            this.cmsTableSp2.Name = "cmsTableSp2";
            this.cmsTableSp2.Size = new System.Drawing.Size(193, 6);
            // 
            // cmsTableColorAllDiff
            // 
            this.cmsTableColorAllDiff.Name = "cmsTableColorAllDiff";
            this.cmsTableColorAllDiff.Size = new System.Drawing.Size(196, 22);
            this.cmsTableColorAllDiff.Text = "当前颜色全部差异脚本";
            this.cmsTableColorAllDiff.Click += new System.EventHandler(this.cmsTableColorAllDiff_Click);
            // 
            // cmsTableColorAllCreate
            // 
            this.cmsTableColorAllCreate.Name = "cmsTableColorAllCreate";
            this.cmsTableColorAllCreate.Size = new System.Drawing.Size(196, 22);
            this.cmsTableColorAllCreate.Text = "当前颜色全部创建脚本";
            this.cmsTableColorAllCreate.Click += new System.EventHandler(this.cmsTableColorAllCreate_Click);

            //
            //base handler
            //
            this.tvwTarget.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwTarget_NodeMouseClick);
            this.tvwSource.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwSource_NodeMouseClick);
            // 
            // CompareTable
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(632, 557);
            this.Name = "CompareTable";
            this.Text = "CompareTable";
            //this.Controls.SetChildIndex(this.grpSource, 0);
            //this.Controls.SetChildIndex(this.grpTarget, 0);
            //this.Controls.SetChildIndex(this.grpSql, 0);
            //this.grpSource.ResumeLayout(false);
            //this.grpTarget.ResumeLayout(false);
            //this.grpSql.ResumeLayout(false);
            //this.grpSql.PerformLayout();
            this.cmsTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem cmsTableCurrentDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsTableCurrentCreate;
        private System.Windows.Forms.ToolStripSeparator cmsTableSp1;
        private System.Windows.Forms.ToolStripMenuItem cmsTableAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsTableAllCreate;
        private System.Windows.Forms.ToolStripSeparator cmsTableSp2;
        private System.Windows.Forms.ToolStripMenuItem cmsTableColorAllDiff;
        private System.Windows.Forms.ToolStripMenuItem cmsTableColorAllCreate;
    }
}