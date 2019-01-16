using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompareMSSQL.Entity;
using CompareMSSQL.SubForm;
using System.Threading;

namespace CompareMSSQL
{
    public partial class Form1 : Form
    {
        private bool canSelectMenu = true;

        public Form1()
        {
            InitializeComponent();
            setMenuTree();
        }

        private void setMenuTree()
        {
            string[] menuText = { "表", "视图", "存储过程", "函数" };
            foreach (var menu in menuText)
            {
                CustomTreeNode node = new CustomTreeNode();
                node.Text = menu;
                node.ToolTipText = menu;
                node.IsParent = true;
                Menu.Nodes.Add(node);
            }
            Menu.ExpandAll();
        }

        private void Menu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            msg_lb.Visible = true;
            msg_lb.Refresh();
            switch (e.Node.Text)
            {
                case "表":
                    //MenuSplitContainer.Panel2.Controls.Clear();
                    var compareTable = new CompareTable(tb_sourceDB.Text, tb_targetDB.Text);
                    MenuSplitContainer.Panel2.Controls.Clear();
                    compareTable.Dock = DockStyle.Fill;
                    compareTable.TopLevel = false;
                    compareTable.WindowState = FormWindowState.Maximized;
                    compareTable.FormBorderStyle = FormBorderStyle.None;
                    compareTable.Parent = MenuSplitContainer.Panel2;                    
                    compareTable.Show();
                    break;
                case "视图":
                    var compareView = new CompareView(tb_sourceDB.Text, tb_targetDB.Text);
                    MenuSplitContainer.Panel2.Controls.Clear();
                    compareView.Dock = DockStyle.Fill;
                    compareView.TopLevel = false;
                    compareView.WindowState = FormWindowState.Maximized;
                    compareView.FormBorderStyle = FormBorderStyle.None;
                    compareView.Parent = MenuSplitContainer.Panel2;
                    compareView.Show();
                    break;
                case "存储过程":
                    break;
                case "函数":
                    break;
                default: break;
            }
            Thread.Sleep(2000);
            msg_lb.Visible = false;
            msg_lb.Refresh();
        }

        private void Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            msg_lb.Visible = true;
            msg_lb.Refresh();
            switch (e.Node.Text)
            {
                case "表":
                    //MenuSplitContainer.Panel2.Controls.Clear();
                    var compareTable = new CompareTable(tb_sourceDB.Text, tb_targetDB.Text);
                    MenuSplitContainer.Panel2.Controls.Clear();
                    compareTable.Dock = DockStyle.Fill;
                    compareTable.TopLevel = false;
                    compareTable.WindowState = FormWindowState.Maximized;
                    compareTable.FormBorderStyle = FormBorderStyle.None;
                    compareTable.Parent = MenuSplitContainer.Panel2;
                    compareTable.Show();
                    break;
                case "视图":
                    break;
                case "存储过程":
                    break;
                case "函数":
                    break;
                default: break;
            }
            Thread.Sleep(2000);
            msg_lb.Visible = false;
            msg_lb.Refresh();
        }
    }
}
