namespace CompareMSSQL
{
    partial class CompareMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareMain));
            this.lblSourceDB = new System.Windows.Forms.Label();
            this.txtSourceDB = new System.Windows.Forms.TextBox();
            this.txtTargetDB = new System.Windows.Forms.TextBox();
            this.lblTargetDB = new System.Windows.Forms.Label();
            this.splMenu = new System.Windows.Forms.SplitContainer();
            this.tvwMenu = new System.Windows.Forms.TreeView();
            this.lblMsg = new System.Windows.Forms.Label();
            this.imgMenuTree = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splMenu)).BeginInit();
            this.splMenu.Panel1.SuspendLayout();
            this.splMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSourceDB
            // 
            this.lblSourceDB.AutoSize = true;
            this.lblSourceDB.Location = new System.Drawing.Point(16, 7);
            this.lblSourceDB.Name = "lblSourceDB";
            this.lblSourceDB.Size = new System.Drawing.Size(65, 12);
            this.lblSourceDB.TabIndex = 0;
            this.lblSourceDB.Text = "源数据库：";
            // 
            // txtSourceDB
            // 
            this.txtSourceDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSourceDB.Location = new System.Drawing.Point(78, 4);
            this.txtSourceDB.Name = "txtSourceDB";
            this.txtSourceDB.Size = new System.Drawing.Size(702, 21);
            this.txtSourceDB.TabIndex = 1;
            this.txtSourceDB.Text = "Data Source=.;Initial Catalog=Skyland01;User ID=sa;Password=123456;";
            // 
            // txtTargetDB
            // 
            this.txtTargetDB.Location = new System.Drawing.Point(78, 31);
            this.txtTargetDB.Name = "txtTargetDB";
            this.txtTargetDB.Size = new System.Drawing.Size(702, 21);
            this.txtTargetDB.TabIndex = 3;
            this.txtTargetDB.Text = "Data Source=.;Initial Catalog=Skyland02;User ID=sa;Password=123456;";
            // 
            // lblTargetDB
            // 
            this.lblTargetDB.AutoSize = true;
            this.lblTargetDB.Location = new System.Drawing.Point(4, 34);
            this.lblTargetDB.Name = "lblTargetDB";
            this.lblTargetDB.Size = new System.Drawing.Size(77, 12);
            this.lblTargetDB.TabIndex = 2;
            this.lblTargetDB.Text = "目标数据库：";
            // 
            // splMenu
            // 
            this.splMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splMenu.Location = new System.Drawing.Point(7, 62);
            this.splMenu.Name = "splMenu";
            // 
            // splMenu.Panel1
            // 
            this.splMenu.Panel1.Controls.Add(this.tvwMenu);
            // 
            // splMenu.Panel2
            // 
            this.splMenu.Panel2.AutoScroll = true;
            this.splMenu.Panel2.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.splMenu.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splMenu.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splMenu.Size = new System.Drawing.Size(773, 555);
            this.splMenu.SplitterDistance = 133;
            this.splMenu.TabIndex = 4;
            // 
            // tvwMenu
            // 
            this.tvwMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.tvwMenu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvwMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tvwMenu.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tvwMenu.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tvwMenu.HideSelection = false;
            this.tvwMenu.Indent = 19;
            this.tvwMenu.ItemHeight = 25;
            this.tvwMenu.LabelEdit = true;
            this.tvwMenu.Location = new System.Drawing.Point(0, 15);
            this.tvwMenu.Name = "tvwMenu";
            this.tvwMenu.ShowRootLines = false;
            this.tvwMenu.Size = new System.Drawing.Size(133, 539);
            this.tvwMenu.TabIndex = 0;
            this.tvwMenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Menu_NodeMouseClick);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMsg.Location = new System.Drawing.Point(4, 628);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(59, 12);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "请等待...";
            this.lblMsg.Visible = false;
            // 
            // imgMenuTree
            // 
            this.imgMenuTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMenuTree.ImageStream")));
            this.imgMenuTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMenuTree.Images.SetKeyName(0, "a15.ico");
            this.imgMenuTree.Images.SetKeyName(1, "f2.ico");
            this.imgMenuTree.Images.SetKeyName(2, "g10.png");
            this.imgMenuTree.Images.SetKeyName(3, "g18.png");
            this.imgMenuTree.Images.SetKeyName(4, "file1.ico");
            this.imgMenuTree.Images.SetKeyName(5, "file10.ico");
            this.imgMenuTree.Images.SetKeyName(6, "file16.ico");
            this.imgMenuTree.Images.SetKeyName(7, "g18-1.png");
            // 
            // CompareMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 646);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.splMenu);
            this.Controls.Add(this.txtTargetDB);
            this.Controls.Add(this.lblTargetDB);
            this.Controls.Add(this.txtSourceDB);
            this.Controls.Add(this.lblSourceDB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompareMain";
            this.Text = "demo";
            this.splMenu.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMenu)).EndInit();
            this.splMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSourceDB;
        private System.Windows.Forms.TextBox txtSourceDB;
        private System.Windows.Forms.TextBox txtTargetDB;
        private System.Windows.Forms.Label lblTargetDB;
        private System.Windows.Forms.SplitContainer splMenu;
        private System.Windows.Forms.TreeView tvwMenu;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ImageList imgMenuTree;
    }
}

