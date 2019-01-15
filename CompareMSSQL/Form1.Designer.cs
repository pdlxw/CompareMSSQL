namespace CompareMSSQL
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_sourceDB = new System.Windows.Forms.Label();
            this.tb_sourceDB = new System.Windows.Forms.TextBox();
            this.tb_targetDB = new System.Windows.Forms.TextBox();
            this.lb_targetDB = new System.Windows.Forms.Label();
            this.MenuSplitContainer = new System.Windows.Forms.SplitContainer();
            this.Menu = new System.Windows.Forms.TreeView();
            this.msg_lb = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MenuSplitContainer)).BeginInit();
            this.MenuSplitContainer.Panel1.SuspendLayout();
            this.MenuSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_sourceDB
            // 
            this.lb_sourceDB.AutoSize = true;
            this.lb_sourceDB.Location = new System.Drawing.Point(16, 7);
            this.lb_sourceDB.Name = "lb_sourceDB";
            this.lb_sourceDB.Size = new System.Drawing.Size(65, 12);
            this.lb_sourceDB.TabIndex = 0;
            this.lb_sourceDB.Text = "源数据库：";
            // 
            // tb_sourceDB
            // 
            this.tb_sourceDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_sourceDB.Location = new System.Drawing.Point(78, 4);
            this.tb_sourceDB.Name = "tb_sourceDB";
            this.tb_sourceDB.Size = new System.Drawing.Size(702, 21);
            this.tb_sourceDB.TabIndex = 1;
            this.tb_sourceDB.Text = "Data Source=.;Initial Catalog=Skyland01;User ID=sa;Password=123456;";
            // 
            // tb_targetDB
            // 
            this.tb_targetDB.Location = new System.Drawing.Point(78, 31);
            this.tb_targetDB.Name = "tb_targetDB";
            this.tb_targetDB.Size = new System.Drawing.Size(702, 21);
            this.tb_targetDB.TabIndex = 3;
            this.tb_targetDB.Text = "Data Source=.;Initial Catalog=Skyland02;User ID=sa;Password=123456;";
            // 
            // lb_targetDB
            // 
            this.lb_targetDB.AutoSize = true;
            this.lb_targetDB.Location = new System.Drawing.Point(4, 34);
            this.lb_targetDB.Name = "lb_targetDB";
            this.lb_targetDB.Size = new System.Drawing.Size(77, 12);
            this.lb_targetDB.TabIndex = 2;
            this.lb_targetDB.Text = "目标数据库：";
            // 
            // MenuSplitContainer
            // 
            this.MenuSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuSplitContainer.Location = new System.Drawing.Point(7, 62);
            this.MenuSplitContainer.Name = "MenuSplitContainer";
            // 
            // MenuSplitContainer.Panel1
            // 
            this.MenuSplitContainer.Panel1.Controls.Add(this.Menu);
            // 
            // MenuSplitContainer.Panel2
            // 
            this.MenuSplitContainer.Panel2.AutoScroll = true;
            this.MenuSplitContainer.Panel2.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.MenuSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.MenuSplitContainer.Size = new System.Drawing.Size(773, 555);
            this.MenuSplitContainer.SplitterDistance = 133;
            this.MenuSplitContainer.TabIndex = 4;
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.SystemColors.Window;
            this.Menu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Menu.LabelEdit = true;
            this.Menu.Location = new System.Drawing.Point(0, -1);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(133, 555);
            this.Menu.TabIndex = 0;
            this.Menu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Menu_NodeMouseClick);
            // 
            // msg_lb
            // 
            this.msg_lb.AutoSize = true;
            this.msg_lb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msg_lb.Location = new System.Drawing.Point(4, 628);
            this.msg_lb.Name = "msg_lb";
            this.msg_lb.Size = new System.Drawing.Size(59, 12);
            this.msg_lb.TabIndex = 0;
            this.msg_lb.Text = "请等待...";
            this.msg_lb.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 646);
            this.Controls.Add(this.msg_lb);
            this.Controls.Add(this.MenuSplitContainer);
            this.Controls.Add(this.tb_targetDB);
            this.Controls.Add(this.lb_targetDB);
            this.Controls.Add(this.tb_sourceDB);
            this.Controls.Add(this.lb_sourceDB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MenuSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MenuSplitContainer)).EndInit();
            this.MenuSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_sourceDB;
        private System.Windows.Forms.TextBox tb_sourceDB;
        private System.Windows.Forms.TextBox tb_targetDB;
        private System.Windows.Forms.Label lb_targetDB;
        private System.Windows.Forms.SplitContainer MenuSplitContainer;
        private System.Windows.Forms.TreeView Menu;
        private System.Windows.Forms.Label msg_lb;
    }
}

