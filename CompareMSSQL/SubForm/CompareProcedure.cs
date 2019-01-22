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
    public partial class CompareProcedure : CommonWin
    {
        //public CompareView()
        //{
        //    InitializeComponent();
        //}
        /// <summary>
        /// 所有视图
        /// </summary>
        private List<DBProcedureView> allProcedures = new List<DBProcedureView>();

        /// <summary>
        /// 当前选择操作的是否为源数据库
        /// </summary>
        private bool currentIsSource;

        public CompareProcedure(string sourceDB, string targetDB):base()
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

                foreach (StoredProcedure sProcedure in sourceDatabase.StoredProcedures)
                {
                    if (sProcedure.IsSystemObject)
                    {
                        continue;
                    }

                    if (!targetDatabase.Views.Contains(sProcedure.Name))
                    {
                        allProcedures.Add(new DBProcedureView(sProcedure, true, DifferencesType.unique));

                        allProcedures.Add(new DBProcedureView(sProcedure, false, DifferencesType.lack));
                    }
                    else if (IsEqualTowProcedure(sProcedure, targetDatabase.StoredProcedures[sProcedure.Name]))
                    {
                        allProcedures.Add(new DBProcedureView(sProcedure, true, DifferencesType.common));

                        allProcedures.Add(new DBProcedureView(targetDatabase.StoredProcedures[sProcedure.Name], false, DifferencesType.common));
                    }
                    else
                    {
                        allProcedures.Add(new DBProcedureView(sProcedure, true, DifferencesType.differences));

                        allProcedures.Add(new DBProcedureView(targetDatabase.StoredProcedures[sProcedure.Name], false, DifferencesType.differences));
                    }

                }

                foreach (StoredProcedure tProcedure in targetDatabase.StoredProcedures)
                {
                    if (!tProcedure.IsSystemObject && !sourceDatabase.StoredProcedures.Contains(tProcedure.Name))
                    {
                        allProcedures.Add(new DBProcedureView(tProcedure, false, DifferencesType.unique));

                        allProcedures.Add(new DBProcedureView(tProcedure, true, DifferencesType.lack));
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
                var procedures = allProcedures.Where(tb => tb.IsSourceDB == isSource).OrderBy(tb => tb.DBStoredProcedure.Name);
                CustomTreeNode startNode = new CustomTreeNode();
                startNode.IsParent = true;
                startNode.CanMenu = false;
                startNode.Text = "所有存储过程";

                tv.Nodes.Add(startNode);

                var tempNode = new CustomTreeNode();
                var color = Color.Black;
                foreach (var procedure in procedures)
                {
                    switch (procedure.Differences)
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
                    tempNode.Text = procedure.DBStoredProcedure.Name;
                    tempNode.Procedure = procedure.DBStoredProcedure;
                    tempNode.Differences = procedure.Differences;
                    tempNode.IsSourceDB = procedure.IsSourceDB;
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
                    e.Node.ContextMenuStrip = cmsProcedure;
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
                    e.Node.ContextMenuStrip = cmsProcedure;
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
        /// <param name="sp"></param>
        /// <param name="tv"></param>
        /// <returns></returns>
        private bool IsEqualTowProcedure(StoredProcedure sp, StoredProcedure tp)
        {
            bool isEqual = false;
            if (sp.TextHeader == tp.TextHeader && sp.TextBody == tp.TextBody)
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
        private void cmsProcedureCurrentDiff_Click(object sender, EventArgs e)
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
                            txtSql.Text = getProcedureAlterSql(targetDB, node.Procedure.Name);
                        }
                        else
                        {
                            txtSql.Text = getProcedureAlterSql(sourceDB, node.Procedure.Name);
                        }
                        break;
                    case DifferencesType.lack:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getProcedureCreateSql(targetDB, node.Procedure.Name);
                        }
                        else
                        {
                            txtSql.Text = getProcedureCreateSql(sourceDB, node.Procedure.Name);
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
        private void cmsProcedureCurrentCreate_Click(object sender, EventArgs e)
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
                            txtSql.Text = getProcedureCreateSql(sourceDB, node.Procedure.Name);
                        }
                        else
                        {
                            txtSql.Text = getProcedureCreateSql(targetDB, node.Procedure.Name);
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
        private void cmsProcedureAllDiff_Click(object sender, EventArgs e)
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
                List<StoredProcedure> procedures = new List<StoredProcedure>();
                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getProcedureAlterSql(targetdb.StoredProcedures[node.Procedure.Name]));
                                break;
                            case DifferencesType.lack:
                                procedures.Add(targetdb.StoredProcedures[node.Procedure.Name]);
                                break;
                        }
                    }
                    if (procedures.Count > 0)
                    {
                        sql.Append(getProcedureCreateSql(targetServer, procedures));
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getProcedureAlterSql(sourcedb.StoredProcedures[node.Procedure.Name]));
                                break;
                            case DifferencesType.lack:
                                procedures.Add(sourcedb.StoredProcedures[node.Procedure.Name]);
                                break;
                        }
                    }
                    if (procedures.Count > 0)
                    {
                        sql.Append(getProcedureCreateSql(sourceServer, procedures));
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
        private void cmsProcedureAllCreate_Click(object sender, EventArgs e)
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
                List<StoredProcedure> sourceProcedures = new List<StoredProcedure>();
                List<StoredProcedure> targetProcedures = new List<StoredProcedure>();

                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.lack:
                                targetProcedures.Add(targetdb.StoredProcedures[node.Procedure.Name]);
                                break;
                            default:
                                sourceProcedures.Add(node.Procedure);
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
                                sourceProcedures.Add(sourcedb.StoredProcedures[node.Procedure.Name]);
                                break;
                            default:
                                targetProcedures.Add(node.Procedure);
                                break;
                        }
                    }
                }

                if (sourceProcedures.Count > 0)
                {
                    sql.Append(getProcedureCreateSql(sourceServer, sourceProcedures));
                }
                if (targetProcedures.Count > 0)
                {
                    sql.Append(getProcedureCreateSql(targetServer, targetProcedures));
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
        private void cmsProcedureColorAllDiff_Click(object sender, EventArgs e)
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
                List<StoredProcedure> procedures = new List<StoredProcedure>();
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
                                    sql.Append(getProcedureAlterSql(targetdb.StoredProcedures[node.Procedure.Name]));
                                    break;
                                case DifferencesType.lack:
                                    procedures.Add(targetdb.StoredProcedures[node.Procedure.Name]);
                                    break;
                            }
                        }
                    }
                    if (procedures.Count > 0)
                    {
                        sql.Append(getProcedureCreateSql(targetServer, procedures));
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
                                    sql.Append(getProcedureAlterSql(sourcedb.StoredProcedures[node.Procedure.Name]));
                                    break;
                                case DifferencesType.lack:
                                    procedures.Add(sourcedb.StoredProcedures[node.Procedure.Name]);
                                    break;
                            }
                        }
                    }
                    if (procedures.Count > 0)
                    {
                        sql.Append(getProcedureCreateSql(sourceServer, procedures));
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
        private void cmsProcedureColorAllCreate_Click(object sender, EventArgs e)
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
                List<StoredProcedure> sourceProcedures = new List<StoredProcedure>();
                List<StoredProcedure> targetProcedures = new List<StoredProcedure>();

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
                                    targetProcedures.Add(targetdb.StoredProcedures[node.Procedure.Name]);
                                    break;
                                default:
                                    sourceProcedures.Add(node.Procedure);
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
                                    sourceProcedures.Add(sourcedb.StoredProcedures[node.Procedure.Name]);
                                    break;
                                default:
                                    targetProcedures.Add(node.Procedure);
                                    break;
                            }
                        }
                    }
                }

                if (sourceProcedures.Count > 0)
                {
                    sql.Append(getProcedureCreateSql(sourceServer, sourceProcedures));
                }
                if (targetProcedures.Count > 0)
                {
                    sql.Append(getProcedureCreateSql(targetServer, targetProcedures));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        private string getProcedureAlterSql(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            //Server srv = new Server(".");
            var dbName = sourceConn.Database;
            // Reference the database.
            Database db = srv.Databases[dbName];

            string sql = "";
            sql += string.Format("--script for procedure {0}\r\n", name);
            if (db.StoredProcedures.Contains(name))
            {
                sql += createToAlter(db.StoredProcedures[name].TextHeader) + db.StoredProcedures[name].TextBody + "\r\n";
            }
            sql += "--\r\n";
            sourceConn.Dispose();
            return sql.ToString();
        }

        private string getProcedureAlterSql(StoredProcedure procedure)
        {
            string sql = "";
            sql += string.Format("--script for procedure {0}\r\n", procedure.Name);
            sql += createToAlter(procedure.TextHeader) + procedure.TextBody + "\r\n" + "--\r\n";
            return sql.ToString();
        }

        private string getProcedureCreateSql(string connect, string name)
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

            if (db.StoredProcedures.Contains(name))
            {
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { db.StoredProcedures[name].Urn });
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

        private string getProcedureCreateSql(Server srv, List<StoredProcedure> procedures)
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
            foreach (StoredProcedure procedure in procedures)
            {
                urnCollection.Add(procedure.Urn);
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
