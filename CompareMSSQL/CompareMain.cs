﻿using System;
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
using CompareMSSQL.Services;

namespace CompareMSSQL
{
    public partial class CompareMain : Form
    {
        private bool canSelectMenu = true;

        private bool wait = false;

        public CompareMain()
        {
            
            InitializeComponent();

            setMenuTree();

        }

        private void setMenuTree()
        {
            string[] menuText = { "表", "视图", "存储过程", "函数" };
            this.tvwMenu.ImageList = imgMenuTree;
            this.tvwMenu.ShowNodeToolTips = true;
            foreach (var menu in menuText)
            {
                CustomTreeNode node = new CustomTreeNode();
                node.Text = menu;
                node.ToolTipText = menu;
                node.IsParent = true;
                node.ImageIndex = 7;
                node.SelectedImageIndex = 3;
                tvwMenu.Nodes.Add(node);
            }
            tvwMenu.ExpandAll();

            //树的节点绘画事件
            this.tvwMenu.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvwMenu.DrawNode += new DrawTreeNodeEventHandler(tvwMenu_DrawNode);
        }

        private void Menu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                
                lblMsg.Visible = true;
                lblMsg.Refresh();
                if (tvwMenu.SelectedNode != null)
                {
                    tvwMenu.SelectedNode.ToolTipText = tvwMenu.SelectedNode.Text;
                }
                tvwMenu.SelectedNode = e.Node;
                tvwMenu.SelectedNode.ToolTipText = "点击刷新";
                //var maskLayer = new MaskLayerForm(new Size(this.ClientRectangle.Width, this.ClientRectangle.Height), new Point(this.Left, this.Top + this.Height - this.ClientRectangle.Height), "正在加载...");
                //maskLayer.ShowDialog(this);
                
                tvwMenu.Enabled = false;
                tvwMenu.Cursor = Cursors.WaitCursor;
                var label = new Label();
                label.Text = "正在加载...";
                label.Location = new Point(10, 10);
                splMenu.Panel2.Controls.Clear();   
                splMenu.Panel2.Controls.Add(label);
                splMenu.Panel2.Refresh();
                switch (e.Node.Text)
                {
                    case "表":    
                        var compareTable = new CompareTable(txtSourceDB.Text, txtTargetDB.Text);
                        splMenu.Panel2.Controls.Remove(label);
                        showSubForm(compareTable);
                        //compareTable.Dock = DockStyle.Fill;
                        //compareTable.TopLevel = false;
                        //compareTable.WindowState = FormWindowState.Maximized;
                        //compareTable.FormBorderStyle = FormBorderStyle.None;
                        //compareTable.Parent = splMenu.Panel2;
                        //compareTable.Show();
                        break;
                    case "视图":
                        //splMenu.Panel2.Controls.Clear();
                        var compareView = new CompareView(txtSourceDB.Text, txtTargetDB.Text);
                        splMenu.Panel2.Controls.Remove(label);
                        showSubForm(compareView);
                        //compareView.Dock = DockStyle.Fill;
                        //compareView.TopLevel = false;
                        //compareView.WindowState = FormWindowState.Maximized;
                        //compareView.FormBorderStyle = FormBorderStyle.None;
                        //compareView.Parent = splMenu.Panel2;
                        //compareView.Show();
                        break;
                    case "存储过程":
                        //splMenu.Panel2.Controls.Clear();
                        var compareProcedure = new CompareProcedure(txtSourceDB.Text, txtTargetDB.Text);
                        splMenu.Panel2.Controls.Remove(label);
                        showSubForm(compareProcedure);
                        //compareProcedure.Dock = DockStyle.Fill;
                        //compareProcedure.TopLevel = false;
                        //compareProcedure.WindowState = FormWindowState.Maximized;
                        //compareProcedure.FormBorderStyle = FormBorderStyle.None;
                        //compareProcedure.Parent = splMenu.Panel2;
                        //compareProcedure.Show();
                        break;
                    case "函数":
                        //splMenu.Panel2.Controls.Clear();
                        var compareFunction = new CompareFunction(txtSourceDB.Text, txtTargetDB.Text);
                        splMenu.Panel2.Controls.Remove(label);
                        showSubForm(compareFunction);
                        //compareFunction.Dock = DockStyle.Fill;
                        //compareFunction.TopLevel = false;
                        //compareFunction.WindowState = FormWindowState.Maximized;
                        //compareFunction.FormBorderStyle = FormBorderStyle.None;
                        //compareFunction.Parent = splMenu.Panel2;
                        //compareFunction.Show();
                        break;
                    default: break;
                }
            }
            catch
            {

            }
            finally
            {
                lblMsg.Visible = false;
                lblMsg.Refresh();
                tvwMenu.Enabled = true;
                tvwMenu.Cursor = Cursors.Hand;
                //wait = false;
            }
        }

        private void showSubForm(CommonWin win)
        {
            win.Dock = DockStyle.Fill;
            win.TopLevel = false;
            win.WindowState = FormWindowState.Maximized;
            win.FormBorderStyle = FormBorderStyle.None;
            win.Top = 0;
            win.Left = 0;
            win.Parent = splMenu.Panel2;
            win.Show();
        }

        /// <summary>
        /// 非焦点也保持选中背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwMenu_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {

            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                // Draw the background of the selected node. The NodeBounds
                // method makes the highlight rectangle large enough to
                // include the text of a node tag, if one is present.
                e.Graphics.FillRectangle(Brushes.Orange, e.Node.Bounds);

                // Retrieve the node font. If the node font has not been set,
                // use the TreeView font.
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                
                // Draw the node text.
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                    Rectangle.Inflate(e.Bounds, 2, 0));
            }

            // Use the default background and node text.
            else
            {
                e.DrawDefault = true;
            }

            // If the node has focus, draw the focus rectangle large, making
            // it large enough to include the text of the node tag, if present.
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = e.Node.Bounds;
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }

        }




        private void Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lblMsg.Visible = true;
            lblMsg.Refresh();
            switch (e.Node.Text)
            {
                case "表":
                    //MenuSplitContainer.Panel2.Controls.Clear();
                    var compareTable = new CompareTable(txtSourceDB.Text, txtTargetDB.Text);
                    splMenu.Panel2.Controls.Clear();
                    compareTable.Dock = DockStyle.Fill;
                    compareTable.TopLevel = false;
                    compareTable.WindowState = FormWindowState.Maximized;
                    compareTable.FormBorderStyle = FormBorderStyle.None;
                    compareTable.Parent = splMenu.Panel2;
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
            lblMsg.Visible = false;
            lblMsg.Refresh();
        }

    }
}
