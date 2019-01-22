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

namespace CompareMSSQL.SubForm
{
    public partial class CompareFunction : CommonWin
    {
        //public CompareView()
        //{
        //    InitializeComponent();
        //}
        /// <summary>
        /// 所有函数
        /// </summary>
        private List<DBFunctionView> allFunctions = new List<DBFunctionView>();

        /// <summary>
        /// 当前选择操作的是否为源数据库
        /// </summary>
        private bool currentIsSource;

        public CompareFunction(string sourceDB, string targetDB):base()
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

                foreach (UserDefinedFunction sFunction in sourceDatabase.UserDefinedFunctions)
                {
                    if (sFunction.IsSystemObject)
                    {
                        continue;
                    }

                    if (!targetDatabase.Views.Contains(sFunction.Name))
                    {
                        allFunctions.Add(new DBFunctionView(sFunction, true, DifferencesType.unique));

                        allFunctions.Add(new DBFunctionView(sFunction, false, DifferencesType.lack));
                    }
                    else if (IsEqualTowFunction(sFunction, targetDatabase.UserDefinedFunctions[sFunction.Name]))
                    {
                        allFunctions.Add(new DBFunctionView(sFunction, true, DifferencesType.common));

                        allFunctions.Add(new DBFunctionView(targetDatabase.UserDefinedFunctions[sFunction.Name], false, DifferencesType.common));
                    }
                    else
                    {
                        allFunctions.Add(new DBFunctionView(sFunction, true, DifferencesType.differences));

                        allFunctions.Add(new DBFunctionView(targetDatabase.UserDefinedFunctions[sFunction.Name], false, DifferencesType.differences));
                    }

                }

                foreach (UserDefinedFunction tFunction in targetDatabase.UserDefinedFunctions)
                {
                    if (!tFunction.IsSystemObject && !sourceDatabase.UserDefinedFunctions.Contains(tFunction.Name))
                    {
                        allFunctions.Add(new DBFunctionView(tFunction, false, DifferencesType.unique));

                        allFunctions.Add(new DBFunctionView(tFunction, true, DifferencesType.lack));
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
                var functions = allFunctions.Where(tb => tb.IsSourceDB == isSource).OrderBy(tb => tb.DBFunction.Name);
                CustomTreeNode startNode = new CustomTreeNode();
                startNode.IsParent = true;
                startNode.CanMenu = false;
                startNode.Text = "所有函数";

                tv.Nodes.Add(startNode);

                var tempNode = new CustomTreeNode();
                var color = Color.Black;
                foreach (var function in functions)
                {
                    switch (function.Differences)
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
                    tempNode.Text = function.DBFunction.Name;
                    tempNode.Function = function.DBFunction;
                    tempNode.Differences = function.Differences;
                    tempNode.IsSourceDB = function.IsSourceDB;
                    tempNode.ForeColor = color;
                    startNode.Nodes.Add(tempNode);
                }
                startNode.Expand();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
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
                    e.Node.ContextMenuStrip = cmsFunction;
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
                    e.Node.ContextMenuStrip = cmsFunction;
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
        /// 是否相同
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        private bool IsEqualTowFunction(UserDefinedFunction sf, UserDefinedFunction tf)
        {
            bool isEqual = false;
            if (sf.TextHeader == tf.TextHeader && sf.TextBody == tf.TextBody)
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
        private void cmsFunctionCurrentDiff_Click(object sender, EventArgs e)
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
                            txtSql.Text = getFunctionAlterSql(targetDB, node.Function.Name);
                        }
                        else
                        {
                            txtSql.Text = getFunctionAlterSql(sourceDB, node.Function.Name);
                        }
                        break;
                    case DifferencesType.lack:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getFunctionCreateSql(targetDB, node.Function.Name);
                        }
                        else
                        {
                            txtSql.Text = getFunctionCreateSql(sourceDB, node.Function.Name);
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
        private void cmsFunctionCurrentCreate_Click(object sender, EventArgs e)
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
                            txtSql.Text = getFunctionCreateSql(sourceDB, node.Function.Name);
                        }
                        else
                        {
                            txtSql.Text = getFunctionCreateSql(targetDB, node.Function.Name);
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
        private void cmsFunctionAllDiff_Click(object sender, EventArgs e)
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
                List<UserDefinedFunction> functions = new List<UserDefinedFunction>();
                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getFunctionAlterSql(targetdb.UserDefinedFunctions[node.Function.Name]));
                                break;
                            case DifferencesType.lack:
                                functions.Add(targetdb.UserDefinedFunctions[node.Function.Name]);
                                break;
                        }
                    }
                    if (functions.Count > 0)
                    {
                        sql.Append(getFunctionCreateSql(targetServer, functions));
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getFunctionAlterSql(sourcedb.UserDefinedFunctions[node.Function.Name]));
                                break;
                            case DifferencesType.lack:
                                functions.Add(sourcedb.UserDefinedFunctions[node.Function.Name]);
                                break;
                        }
                    }
                    if (functions.Count > 0)
                    {
                        sql.Append(getFunctionCreateSql(sourceServer, functions));
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
        private void cmsFunctionAllCreate_Click(object sender, EventArgs e)
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
                List<UserDefinedFunction> sourceFunctions = new List<UserDefinedFunction>();
                List<UserDefinedFunction> targetFunctions = new List<UserDefinedFunction>();

                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.lack:
                                targetFunctions.Add(targetdb.UserDefinedFunctions[node.Function.Name]);
                                break;
                            default:
                                sourceFunctions.Add(node.Function);
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
                                sourceFunctions.Add(sourcedb.UserDefinedFunctions[node.Function.Name]);
                                break;
                            default:
                                targetFunctions.Add(node.Function);
                                break;
                        }
                    }
                }

                if (sourceFunctions.Count > 0)
                {
                    sql.Append(getFunctionCreateSql(sourceServer, sourceFunctions));
                }
                if (targetFunctions.Count > 0)
                {
                    sql.Append(getFunctionCreateSql(targetServer, targetFunctions));
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
        private void cmsFunctionColorAllDiff_Click(object sender, EventArgs e)
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
                List<UserDefinedFunction> functions = new List<UserDefinedFunction>();
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
                                    sql.Append(getFunctionAlterSql(targetdb.UserDefinedFunctions[node.Function.Name]));
                                    break;
                                case DifferencesType.lack:
                                    functions.Add(targetdb.UserDefinedFunctions[node.Function.Name]);
                                    break;
                            }
                        }
                    }
                    if (functions.Count > 0)
                    {
                        sql.Append(getFunctionCreateSql(targetServer, functions));
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
                                    sql.Append(getFunctionAlterSql(sourcedb.UserDefinedFunctions[node.Function.Name]));
                                    break;
                                case DifferencesType.lack:
                                    functions.Add(sourcedb.UserDefinedFunctions[node.Function.Name]);
                                    break;
                            }
                        }
                    }
                    if (functions.Count > 0)
                    {
                        sql.Append(getFunctionCreateSql(sourceServer, functions));
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
        private void cmsFunctionColorAllCreate_Click(object sender, EventArgs e)
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
                List<UserDefinedFunction> sourceFunctions = new List<UserDefinedFunction>();
                List<UserDefinedFunction> targetFunctions = new List<UserDefinedFunction>();

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
                                    targetFunctions.Add(targetdb.UserDefinedFunctions[node.Function.Name]);
                                    break;
                                default:
                                    sourceFunctions.Add(node.Function);
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
                                    sourceFunctions.Add(sourcedb.UserDefinedFunctions[node.Function.Name]);
                                    break;
                                default:
                                    targetFunctions.Add(node.Function);
                                    break;
                            }
                        }
                    }
                }

                if (sourceFunctions.Count > 0)
                {
                    sql.Append(getFunctionCreateSql(sourceServer, sourceFunctions));
                }
                if (targetFunctions.Count > 0)
                {
                    sql.Append(getFunctionCreateSql(targetServer, targetFunctions));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        private string getFunctionAlterSql(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            //Server srv = new Server(".");
            var dbName = sourceConn.Database;
            // Reference the database.
            Database db = srv.Databases[dbName];

            string sql = "";
            sql += string.Format("--script for function {0}\r\n", name);
            if (db.UserDefinedFunctions.Contains(name))
            {
                sql += createToAlter(db.UserDefinedFunctions[name].TextHeader) + db.UserDefinedFunctions[name].TextBody + "\r\n";
            }
            sql += "--\r\n";
            sourceConn.Dispose();
            return sql.ToString();
        }

        private string getFunctionAlterSql(UserDefinedFunction function)
        {
            string sql = "";
            sql += string.Format("--script for function {0}\r\n", function.Name);
            sql += createToAlter(function.TextHeader) + function.TextBody + "\r\n" + "--\r\n";
            return sql.ToString();
        }

        private string getFunctionCreateSql(string connect, string name)
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

            if (db.UserDefinedFunctions.Contains(name))
            {
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { db.UserDefinedFunctions[name].Urn });
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

        private string getFunctionCreateSql(Server srv, List<UserDefinedFunction> functions)
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
            foreach (UserDefinedFunction function in functions)
            {
                urnCollection.Add(function.Urn);
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

        private string createToAlter(string s)
        {
            var ns = s.TrimStart();
            var index = ns.IndexOf(' ');
            ns = ns.Remove(0, index);
            ns = "Alter " + ns;
            return ns;
        }
    }
}
