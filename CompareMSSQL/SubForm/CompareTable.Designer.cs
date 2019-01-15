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
            this.source_table = new System.Windows.Forms.GroupBox();
            this.source_table_tv = new System.Windows.Forms.TreeView();
            this.target_table_tv = new System.Windows.Forms.TreeView();
            this.target_table = new System.Windows.Forms.GroupBox();
            this.gb_sql = new System.Windows.Forms.GroupBox();
            this.bt_copySql = new System.Windows.Forms.Button();
            this.tb_Sql = new System.Windows.Forms.TextBox();
            this.cms_table = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_table_currentdiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_table_currentcreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_table_sp1 = new System.Windows.Forms.ToolStripSeparator();
            this.cms_table_alldiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_table_allcreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_table_sp2 = new System.Windows.Forms.ToolStripSeparator();
            this.cms_table_coloralldiff = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_table_colorallcreate = new System.Windows.Forms.ToolStripMenuItem();
            this.source_table.SuspendLayout();
            this.target_table.SuspendLayout();
            this.gb_sql.SuspendLayout();
            this.cms_table.SuspendLayout();
            this.SuspendLayout();
            // 
            // source_table
            // 
            this.source_table.Controls.Add(this.source_table_tv);
            this.source_table.Location = new System.Drawing.Point(4, 6);
            this.source_table.Name = "source_table";
            this.source_table.Size = new System.Drawing.Size(310, 350);
            this.source_table.TabIndex = 0;
            this.source_table.TabStop = false;
            this.source_table.Text = "源数据库";
            // 
            // source_table_tv
            // 
            this.source_table_tv.Location = new System.Drawing.Point(5, 21);
            this.source_table_tv.Name = "source_table_tv";
            this.source_table_tv.Size = new System.Drawing.Size(300, 320);
            this.source_table_tv.TabIndex = 0;
            this.source_table_tv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.source_table_tv_NodeMouseClick);
            // 
            // target_table_tv
            // 
            this.target_table_tv.Location = new System.Drawing.Point(5, 21);
            this.target_table_tv.Name = "target_table_tv";
            this.target_table_tv.Size = new System.Drawing.Size(300, 320);
            this.target_table_tv.TabIndex = 0;
            this.target_table_tv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.target_table_tv_NodeMouseClick);
            // 
            // target_table
            // 
            this.target_table.Controls.Add(this.target_table_tv);
            this.target_table.Location = new System.Drawing.Point(318, 6);
            this.target_table.Name = "target_table";
            this.target_table.Size = new System.Drawing.Size(310, 350);
            this.target_table.TabIndex = 1;
            this.target_table.TabStop = false;
            this.target_table.Text = "目标数据库";
            // 
            // gb_sql
            // 
            this.gb_sql.Controls.Add(this.bt_copySql);
            this.gb_sql.Controls.Add(this.tb_Sql);
            this.gb_sql.Location = new System.Drawing.Point(4, 355);
            this.gb_sql.Name = "gb_sql";
            this.gb_sql.Size = new System.Drawing.Size(624, 195);
            this.gb_sql.TabIndex = 2;
            this.gb_sql.TabStop = false;
            this.gb_sql.Text = "脚本/消息";
            // 
            // bt_copySql
            // 
            this.bt_copySql.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.bt_copySql.Location = new System.Drawing.Point(579, 165);
            this.bt_copySql.Name = "bt_copySql";
            this.bt_copySql.Size = new System.Drawing.Size(39, 23);
            this.bt_copySql.TabIndex = 1;
            this.bt_copySql.Text = "复制";
            this.bt_copySql.UseVisualStyleBackColor = true;
            this.bt_copySql.Click += new System.EventHandler(this.bt_copySql_Click);
            // 
            // tb_Sql
            // 
            this.tb_Sql.Location = new System.Drawing.Point(5, 16);
            this.tb_Sql.Multiline = true;
            this.tb_Sql.Name = "tb_Sql";
            this.tb_Sql.ReadOnly = true;
            this.tb_Sql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_Sql.Size = new System.Drawing.Size(614, 173);
            this.tb_Sql.TabIndex = 0;
            // 
            // cms_table
            // 
            this.cms_table.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_table_currentdiff,
            this.cms_table_currentcreate,
            this.cms_table_sp1,
            this.cms_table_alldiff,
            this.cms_table_allcreate,
            this.cms_table_sp2,
            this.cms_table_coloralldiff,
            this.cms_table_colorallcreate});
            this.cms_table.Name = "cms_top";
            this.cms_table.Size = new System.Drawing.Size(197, 148);
            // 
            // cms_table_currentdiff
            // 
            this.cms_table_currentdiff.Name = "cms_table_currentdiff";
            this.cms_table_currentdiff.Size = new System.Drawing.Size(196, 22);
            this.cms_table_currentdiff.Text = "当前差异脚本";
            this.cms_table_currentdiff.Click += new System.EventHandler(this.cms_table_currentdiff_Click);
            // 
            // cms_table_currentcreate
            // 
            this.cms_table_currentcreate.Name = "cms_table_currentcreate";
            this.cms_table_currentcreate.Size = new System.Drawing.Size(196, 22);
            this.cms_table_currentcreate.Text = "当前建表脚本";
            this.cms_table_currentcreate.Click += new System.EventHandler(this.cms_table_currentcreate_Click);
            // 
            // cms_table_sp1
            // 
            this.cms_table_sp1.Name = "cms_table_sp1";
            this.cms_table_sp1.Size = new System.Drawing.Size(193, 6);
            // 
            // cms_table_alldiff
            // 
            this.cms_table_alldiff.Name = "cms_table_alldiff";
            this.cms_table_alldiff.Size = new System.Drawing.Size(196, 22);
            this.cms_table_alldiff.Text = "全部差异脚本";
            this.cms_table_alldiff.Click += new System.EventHandler(this.cms_table_alldiff_Click);
            // 
            // cms_table_allcreate
            // 
            this.cms_table_allcreate.Name = "cms_table_allcreate";
            this.cms_table_allcreate.Size = new System.Drawing.Size(196, 22);
            this.cms_table_allcreate.Text = "全部建表脚本";
            this.cms_table_allcreate.Click += new System.EventHandler(this.cms_table_allcreate_Click);
            // 
            // cms_table_sp2
            // 
            this.cms_table_sp2.Name = "cms_table_sp2";
            this.cms_table_sp2.Size = new System.Drawing.Size(193, 6);
            // 
            // cms_table_coloralldiff
            // 
            this.cms_table_coloralldiff.Name = "cms_table_coloralldiff";
            this.cms_table_coloralldiff.Size = new System.Drawing.Size(196, 22);
            this.cms_table_coloralldiff.Text = "当前颜色全部差异脚本";
            this.cms_table_coloralldiff.Click += new System.EventHandler(this.cms_table_coloralldiff_Click);
            // 
            // cms_table_colorallcreate
            // 
            this.cms_table_colorallcreate.Name = "cms_table_colorallcreate";
            this.cms_table_colorallcreate.Size = new System.Drawing.Size(196, 22);
            this.cms_table_colorallcreate.Text = "当前颜色全部建表脚本";
            this.cms_table_colorallcreate.Click += new System.EventHandler(this.cms_table_colorallcreate_Click);
            // 
            // CompareTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 557);
            this.Controls.Add(this.gb_sql);
            this.Controls.Add(this.target_table);
            this.Controls.Add(this.source_table);
            this.Name = "CompareTable";
            this.Text = "CompareTable";
            this.source_table.ResumeLayout(false);
            this.target_table.ResumeLayout(false);
            this.gb_sql.ResumeLayout(false);
            this.gb_sql.PerformLayout();
            this.cms_table.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox source_table;
        private System.Windows.Forms.TreeView source_table_tv;
        private System.Windows.Forms.TreeView target_table_tv;
        private System.Windows.Forms.GroupBox target_table;
        private System.Windows.Forms.GroupBox gb_sql;
        private System.Windows.Forms.TextBox tb_Sql;
        private System.Windows.Forms.Button bt_copySql;
        private System.Windows.Forms.ContextMenuStrip cms_table;
        private System.Windows.Forms.ToolStripMenuItem cms_table_currentdiff;
        private System.Windows.Forms.ToolStripMenuItem cms_table_currentcreate;
        private System.Windows.Forms.ToolStripSeparator cms_table_sp1;
        private System.Windows.Forms.ToolStripMenuItem cms_table_alldiff;
        private System.Windows.Forms.ToolStripMenuItem cms_table_allcreate;
        private System.Windows.Forms.ToolStripSeparator cms_table_sp2;
        private System.Windows.Forms.ToolStripMenuItem cms_table_coloralldiff;
        private System.Windows.Forms.ToolStripMenuItem cms_table_colorallcreate;
    }
}