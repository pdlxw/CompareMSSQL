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
using Microsoft.SqlServer.Management.Smo;
using CompareMSSQL.Enum;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Common;
using SMODBView = Microsoft.SqlServer.Management.Smo.View;

namespace CompareMSSQL.SubForm
{
    public partial class CompareView : CommonWin
    {
        //public CompareView()
        //{
        //    InitializeComponent();
        //}
        /// <summary>
        /// 所有视图
        /// </summary>
        private List<DBVWView> allViews = new List<DBVWView>();

        /// <summary>
        /// 当前选择操作的是否为源数据库
        /// </summary>
        private bool currentIsSource;

        public CompareView(string sourceDB, string targetDB):base()
        {
            this.sourceDB = sourceDB;
            this.targetDB = targetDB;

            InitializeComponent();

            setView();

            setTreeView(tvwSource, true);
            setTreeView(tvwTarget, false);
        }

        private void setView()
        {
            try
            {
                var sourceDatabase = getDB(sourceDB) ?? new Database();
                var targetDatabase = getDB(targetDB) ?? new Database();

                foreach (SMODBView sview in sourceDatabase.Views)
                {
                    if (sview.IsSystemObject)
                    {
                        continue;
                    }

                    if (!targetDatabase.Views.Contains(sview.Name))
                    {
                        allViews.Add(new DBVWView(sview, true, DifferencesType.unique));

                        allViews.Add(new DBVWView(sview, false, DifferencesType.lack));
                    }
                    else if (IsEqualTowView(sview, targetDatabase.Views[sview.Name]))
                    {
                        allViews.Add(new DBVWView(sview, true, DifferencesType.common));

                        allViews.Add(new DBVWView(targetDatabase.Views[sview.Name], false, DifferencesType.common));
                    }
                    else
                    {
                        allViews.Add(new DBVWView(sview, true, DifferencesType.differences));

                        allViews.Add(new DBVWView(targetDatabase.Views[sview.Name], false, DifferencesType.differences));
                    }

                }

                foreach (SMODBView tview in targetDatabase.Views)
                {
                    if (!tview.IsSystemObject && !sourceDatabase.Views.Contains(tview.Name))
                    {
                        allViews.Add(new DBVWView(tview, false, DifferencesType.unique));

                        allViews.Add(new DBVWView(tview, true, DifferencesType.lack));
                    }
                }

            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }


        private void setTreeView(TreeView tv, bool isSource)
        {
            try
            {
                var views = allViews.Where(tb => tb.IsSourceDB == isSource).OrderBy(tb => tb.DBView.Name);
                CustomTreeNode startNode = new CustomTreeNode();
                startNode.IsParent = true;
                startNode.CanMenu = false;
                startNode.Text = "所有视图";

                tv.Nodes.Add(startNode);

                var tempNode = new CustomTreeNode();
                var color = Color.Black;
                var tip = "";
                foreach (var view in views)
                {
                    switch (view.Differences)
                    {
                        case DifferencesType.unique:
                            color = Color.Green;
                            break;
                        case DifferencesType.differences:
                            color = Color.Orange;
                            break;
                        case DifferencesType.common:
                            color = Color.Black;
                            break;
                        case DifferencesType.lack:
                            color = Color.Gray;
                            break;
                        default:
                            color = Color.Black;
                            break;
                    }
                    tempNode = new CustomTreeNode();
                    tempNode.IsParent = true;
                    tempNode.CanMenu = true;
                    tempNode.Text = view.DBView.Name;
                    tempNode.View = view.DBView;
                    tempNode.Differences = view.Differences;
                    tempNode.IsSourceDB = view.IsSourceDB;
                    tempNode.ForeColor = color;
                    startNode.Nodes.Add(tempNode);
                }
                startNode.Expand();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
                throw ex;
            }
        }

        private void tvwSource_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvwSource.SelectedNode = e.Node;
                
                if (((CustomTreeNode)e.Node).CanMenu)
                {
                    currentIsSource = true;
                    e.Node.ContextMenuStrip = cmsView;                  
                }
            }
        }

        private void tvwTarget_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvwTarget.SelectedNode = e.Node;

                if (((CustomTreeNode)e.Node).CanMenu)
                {
                    currentIsSource = false;
                    e.Node.ContextMenuStrip = cmsView;
                }
            }
        }

        /// <summary>
        /// 取得数据库
        /// </summary>
        /// <param name="dbStr"></param>
        /// <param name="includeSys"></param>
        /// <returns></returns>
        private Database getDB(string dbStr)
        {
            var sourceConn = new SqlConnection(dbStr);
            Server srv = new Server(new ServerConnection(sourceConn));
            var dbName = sourceConn.Database;
            var db = srv.Databases[dbName];

            sourceConn.Dispose();
            return db;
        }

        /// <summary>
        /// 是否相同视图
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="tv"></param>
        /// <returns></returns>
        private bool IsEqualTowView(SMODBView sv, SMODBView tv)
        {
            bool isEqual = false;
            if (sv.TextHeader == tv.TextHeader && sv.TextBody == tv.TextBody)
            {
                isEqual = true;
            }
            return isEqual;
        }

        /// <summary>
        /// 当前差异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewCurrentDiff_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                CustomTreeNode node;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    node = (CustomTreeNode)tvwSource.SelectedNode;
                }
                else
                {
                    node = (CustomTreeNode)tvwTarget.SelectedNode;
                }

                //差异及缺视图获取sql
                switch (node.Differences)
                {
                    case DifferencesType.differences:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getViewAlterSql(targetDB, node.View.Name);
                        }
                        else
                        {
                            txtSql.Text = getViewAlterSql(sourceDB, node.View.Name);
                        }
                        break;
                    case DifferencesType.lack:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getViewCreateSql(targetDB, node.View.Name);
                        }
                        else
                        {
                            txtSql.Text = getViewCreateSql(sourceDB, node.View.Name);
                        }
                        break;
                    case DifferencesType.common:
                    case DifferencesType.unique:
                    default:
                        txtSql.Text = "--消息：无差异脚本。";
                        break;
                }
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 当前创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewCurrentCreate_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                CustomTreeNode node;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    node = (CustomTreeNode)tvwSource.SelectedNode;
                }
                else
                {
                    node = (CustomTreeNode)tvwTarget.SelectedNode;
                }

                //获取sql
                switch (node.Differences)
                {
                    case DifferencesType.differences:
                    case DifferencesType.common:
                    case DifferencesType.unique:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getViewCreateSql(sourceDB, node.View.Name);
                        }
                        else
                        {
                            txtSql.Text = getViewCreateSql(targetDB, node.View.Name);
                        }
                        break;
                    default:
                        txtSql.Text = "--消息：无脚本。";
                        break;
                }
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 全部差异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewAllDiff_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                TreeNodeCollection nodes = null;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    nodes = tvwSource.Nodes;
                }
                else
                {
                    nodes = tvwTarget.Nodes;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    txtSql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                List<SMODBView> views = new List<SMODBView>();
                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getViewAlterSql(targetdb.Views[node.View.Name]));
                                break;
                            case DifferencesType.lack:
                                views.Add(targetdb.Views[node.View.Name]);
                                break;
                        }
                    }
                    if (views.Count > 0)
                    {
                        sql.Append(getViewCreateSql(targetServer, views));
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getViewAlterSql(sourcedb.Views[node.View.Name]));
                                break;
                            case DifferencesType.lack:
                                views.Add(sourcedb.Views[node.View.Name]);
                                break;
                        }
                    }
                    if (views.Count > 0)
                    {
                        sql.Append(getViewCreateSql(sourceServer, views));
                    }
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 全部创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewAllCreate_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                TreeNodeCollection nodes = null;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    nodes = tvwSource.Nodes;
                }
                else
                {
                    nodes = tvwTarget.Nodes;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    txtSql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                List<SMODBView> sourceViews = new List<SMODBView>();
                List<SMODBView> targetViews = new List<SMODBView>();

                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.lack:
                                targetViews.Add(targetdb.Views[node.View.Name]);
                                break;
                            default:
                                sourceViews.Add(node.View);
                                break;
                        }
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.lack:
                                sourceViews.Add(sourcedb.Views[node.View.Name]);
                                break;
                            default:
                                targetViews.Add(node.View);
                                break;
                        }
                    }
                }

                if (sourceViews.Count > 0)
                {
                    sql.Append(getViewCreateSql(sourceServer, sourceViews));
                }
                if (targetViews.Count > 0)
                {
                    sql.Append(getViewCreateSql(targetServer, targetViews));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 当前颜色差异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewColorAllDiff_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                TreeNodeCollection nodes = null;
                CustomTreeNode selectnode;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    selectnode = (CustomTreeNode)tvwSource.SelectedNode;
                    nodes = tvwSource.Nodes;
                }
                else
                {
                    selectnode = (CustomTreeNode)tvwTarget.SelectedNode;
                    nodes = tvwTarget.Nodes;
                }

                //相同或者独有的无差异脚本
                if (selectnode.Differences == DifferencesType.common || selectnode.Differences == DifferencesType.unique)
                {
                    txtSql.Text = "--消息：无生成脚本。";
                    return;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    txtSql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                List<SMODBView> views = new List<SMODBView>();
                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.differences:
                                    sql.Append(getViewAlterSql(targetdb.Views[node.View.Name]));
                                    break;
                                case DifferencesType.lack:
                                    views.Add(targetdb.Views[node.View.Name]);
                                    break;
                            }
                        }
                    }
                    if (views.Count > 0)
                    {
                        sql.Append(getViewCreateSql(targetServer, views));
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.differences:
                                    sql.Append(getViewAlterSql(sourcedb.Views[node.View.Name]));
                                    break;
                                case DifferencesType.lack:
                                    views.Add(sourcedb.Views[node.View.Name]);
                                    break;
                            }
                        }
                    }
                    if (views.Count > 0)
                    {
                        sql.Append(getViewCreateSql(sourceServer, views));
                    }
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 当前颜色创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsViewColorAllCreate_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                TreeNodeCollection nodes = null;
                CustomTreeNode selectnode;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    selectnode = (CustomTreeNode)tvwSource.SelectedNode;
                    nodes = tvwSource.Nodes;
                }
                else
                {
                    selectnode = (CustomTreeNode)tvwSource.SelectedNode;
                    nodes = tvwTarget.Nodes;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    txtSql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                List<SMODBView> sourceViews = new List<SMODBView>();
                List<SMODBView> targetViews = new List<SMODBView>();

                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.lack:
                                    targetViews.Add(targetdb.Views[node.View.Name]);
                                    break;
                                default:
                                    sourceViews.Add(node.View);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.lack:
                                    sourceViews.Add(sourcedb.Views[node.View.Name]);
                                    break;
                                default:
                                    targetViews.Add(node.View);
                                    break;
                            }
                        }
                    }
                }

                if (sourceViews.Count > 0)
                {
                    sql.Append(getViewCreateSql(sourceServer, sourceViews));
                }
                if (targetViews.Count > 0)
                {
                    sql.Append(getViewCreateSql(targetServer, targetViews));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        private string getViewAlterSql(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            //Server srv = new Server(".");
            var dbName = sourceConn.Database;
            // Reference the database.
            Database db = srv.Databases[dbName];

            string sql = "";
            sql += string.Format("--script for view {0}\r\n", name);
            if (db.Views.Contains(name))
            {
                sql += "ALTER VIEW [dbo].[" + Name + "] \r\nAS\r\n" + db.Views[name].TextBody + "\r\n";
            }
            sql += "--\r\n";
            sourceConn.Dispose();
            return sql.ToString();
        }

        private string getViewAlterSql(SMODBView view)
        {
            string sql = "";
            sql += string.Format("--script for view {0}\r\n", view.Name);
            sql += "ALTER VIEW [dbo].[" + view.Name + "] \r\nAS\r\n" + view.TextBody + "\r\n" + "--\r\n";
            return sql.ToString();
        }

        private string getViewCreateSql(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            //Server srv = new Server(".");
            var dbName = sourceConn.Database;
            // Reference the database.
            Database db = srv.Databases[dbName];
            StringBuilder sql = new StringBuilder();

            // Define a Scripter object and set the required scripting options.   
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true;   // To include indexes  
            scrp.Options.DriAllConstraints = true;   // to include referential constraints in the script  
            scrp.Options.IncludeHeaders = true;
            scrp.Options.IncludeIfNotExists = true;
            //scrp.Options.ScriptForAlter = true;

            // Iterate through the tables in database and script each one. Display the script.

            if (db.Views.Contains(name))
            {
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { db.Views[name].Urn });
                //sql.AppendFormat("--script for table {0}\r\n", name);
                foreach (string st in sc)
                {
                    sql.AppendFormat("{0}\r\n", st);
                }
                //sql.Append("--\r\n");
            }
            sourceConn.Dispose();
            return sql.ToString();
        }

        private string getViewCreateSql(Server srv, List<SMODBView> views)
        {
            StringBuilder sql = new StringBuilder();

            // Define a Scripter object and set the required scripting options.   
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true;   // To include indexes  
            scrp.Options.DriAllConstraints = true;   // to include referential constraints in the script  
            scrp.Options.IncludeHeaders = true;
            scrp.Options.IncludeIfNotExists = true;

            UrnCollection urnCollection = new UrnCollection();
            foreach (SMODBView view in views)
            {
                urnCollection.Add(view.Urn);
            }
            // Iterate through the tables in database and script each one. Display the script.

            System.Collections.Specialized.StringCollection sc = scrp.Script(urnCollection);
            //sql.AppendFormat("--script for table {0}\r\n", name);
            foreach (string st in sc)
            {
                sql.AppendFormat("{0}\r\n", st);
            }
            //sql.Append("--\r\n");

            return sql.ToString();
        }
    }
}
