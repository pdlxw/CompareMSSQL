using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Common;
using CompareMSSQL.Entity;
using CompareMSSQL.Enum;
using System.Threading;

namespace CompareMSSQL.SubForm
{
    public partial class CompareTable : CommonWin
    {
        
        /// <summary>
        /// 所有表
        /// </summary>
        private List<DBTableView> allTables = new List<DBTableView>();

        /// <summary>
        /// 当前选择操作的是否为源数据库
        /// </summary>
        private bool currentIsSource;

        //public CompareTable()
        //{
        //    InitializeComponent();
        //}

        public CompareTable(string sourceDB, string targetDB):base()
        {
            this.sourceDB = sourceDB;
            this.targetDB = targetDB;
            InitializeComponent();
            setTableView();
            setTreeView(tvwSource, true);
            setTreeView(tvwTarget, false);
        }


        private void setTableView()
        {
            try
            {
                Console.WriteLine(string.Format("1:{0}", DateTime.Now.ToString("hh:mm:ss ffff")));
                var sourceDatabase = getDB(sourceDB) ?? new Database();
                var targetDatabase = getDB(targetDB) ?? new Database();

                foreach (Table sdb in sourceDatabase.Tables)
                {
                    if (sdb.IsSystemObject)
                    {
                        continue;
                    }

                    if (!targetDatabase.Tables.Contains(sdb.Name))
                    {
                        allTables.Add(new DBTableView(sdb, true, DifferencesType.unique));

                        allTables.Add(new DBTableView(sdb, false, DifferencesType.lack));
                    }
                    else if(IsEqualTowTable(sdb, targetDatabase.Tables[sdb.Name]))
                    {
                        allTables.Add(new DBTableView(sdb, true, DifferencesType.common));

                        allTables.Add(new DBTableView(targetDatabase.Tables[sdb.Name], false, DifferencesType.common));
                    }
                    else
                    {
                        allTables.Add(new DBTableView(sdb, true, DifferencesType.differences));

                        allTables.Add(new DBTableView(targetDatabase.Tables[sdb.Name], false, DifferencesType.differences));
                    }
                    
                }

                foreach (Table ttb in targetDatabase.Tables)
                {
                    if (!ttb.IsSystemObject && !sourceDatabase.Tables.Contains(ttb.Name))
                    {
                        allTables.Add(new DBTableView(ttb, false, DifferencesType.unique));

                        allTables.Add(new DBTableView(ttb, true, DifferencesType.lack));
                    }
                }

            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
            Console.WriteLine(string.Format("2:{0}", DateTime.Now.ToString("hh:mm:ss ffff")));
        }

        private void setTreeView(TreeView tv, bool isSource)
        {
            try
            {
                Console.WriteLine(string.Format("3:{0}", DateTime.Now.ToString("hh:mm:ss ffff")));
                var tables = allTables.Where(tb => tb.IsSourceDB == isSource).OrderBy(tb => tb.DBTable.Name);
                CustomTreeNode startNode = new CustomTreeNode();
                startNode.IsParent = true;
                startNode.CanMenu = true;
                startNode.Text = "所有表([黑:同][绿:独有][黄:差异][灰:缺])";
                tv.Nodes.Add(startNode);

                var tempNode = new CustomTreeNode();
                var color = Color.Black;
                foreach (var table in tables)
                {
                    switch (table.Differences)
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
                    tempNode.Table = table.DBTable;
                    tempNode.Text = table.DBTable.Name;
                    tempNode.Differences = table.Differences;
                    tempNode.IsSourceDB = table.IsSourceDB;
                    tempNode.ForeColor = color;
                    startNode.Nodes.Add(tempNode);

                    var colNode = new CustomTreeNode();
                    foreach (Column col in table.DBTable.Columns)
                    {
                        colNode = new CustomTreeNode();
                        colNode.Column = col;
                        colNode.Text = string.Format("{0}({1}{2}{3})", col.Name, col.InPrimaryKey ? "PK," : "", col.DataType.IsStringType ? string.Format("{0}({1}),", col.DataType.Name, col.DataType.MaximumLength) : col.DataType.Name + ",", col.Nullable ? "null" : "not null");
                        colNode.ForeColor = color;
                        tempNode.Nodes.Add(colNode);
                    }
                }
                startNode.Expand();
            }
            catch (Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
            Console.WriteLine(string.Format("4:{0}", DateTime.Now.ToString("hh:mm:ss ffff")));
        }

        private List<Table> getDBTable(string dbStr, bool includeSys)
        {
            var sourceConn = new SqlConnection(dbStr);
            Server srv = new Server(new ServerConnection(sourceConn));
            var dbName = sourceConn.Database;
            Database db = srv.Databases[dbName];
            List<Table> tables = new List<Table>();
            foreach (Table tb in db.Tables)
            {
                if (tb.IsSystemObject && !includeSys)
                {
                    continue;
                }
                tables.Add(tb);
            }
            //sourceConn.Close();
            sourceConn.Dispose();
            return tables;
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
        /// 表比较
        /// </summary>
        /// <param name="st"></param>
        /// <param name="tt"></param>
        /// <returns></returns>
        private bool IsEqualTowTable(Table st, Table tt)
        {
            bool equal = true;

            if (st.Columns.Count != tt.Columns.Count)
            {
                return false;
            }

            foreach (Column col in st.Columns)
            {
                if (!tt.Columns.Contains(col.Name) || !IsEqualTowCol(col, tt.Columns[col.Name]))
                {
                    return false;
                }
            }

            return equal;
        }

        /// <summary>
        /// 列比较
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsEqualTowCol(Column sc, Column tc)
        {
            bool equal = true;

            if (sc.Nullable != tc.Nullable || sc.DataType.Name != tc.DataType.Name || sc.DataType.NumericPrecision != tc.DataType.NumericPrecision || sc.DataType.NumericScale != tc.DataType.NumericScale || sc.DataType.MaximumLength != tc.DataType.MaximumLength)
            {
                return false;
            }

            return equal;
        }

        /// <summary>
        /// 复制脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_copySql_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSql.Text))
            {
                Clipboard.SetDataObject(txtSql.Text);
            }         
        }

        private void tvwSource_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvwSource.SelectedNode = e.Node;
                //var tag = (TableInfoTag)e.Node.Tag;
                if (((CustomTreeNode)e.Node).Table != null)
                {
                    currentIsSource = true;
                    e.Node.ContextMenuStrip = cmsTable;
                    //cms_top.Show(source_table_tv, e.X, e.Y);
                }           
            }
        }

        private void tvwTarget_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvwTarget.SelectedNode = e.Node;
                //var tag = (TableInfoTag)e.Node.Tag;
                if (((CustomTreeNode)e.Node).Table != null)
                {
                    currentIsSource = false;
                    e.Node.ContextMenuStrip = cmsTable;
                    //cms_top.Show(source_table_tv, e.X, e.Y);
                }
            }
        }

        /// <summary>
        /// 当前差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableCurrentDiff_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                //var tag = (TableInfoTag)source_table_tv.SelectedNode.Tag;
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

                if (node.Table == null)
                {
                    txtSql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }
                //差异及缺表获取sql
                switch (node.Differences)
                {
                    case DifferencesType.differences:
                        var table = new Table();
                        if (node.IsSourceDB)
                        {
                            table = getTableByName(targetDB, node.Table.Name);
                        }
                        else
                        {
                            table = getTableByName(sourceDB, node.Table.Name);
                        }
                        txtSql.Text = getTowTableDiffSql(node.Table, table);
                        break;
                    case DifferencesType.lack:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getTableCreateSql(targetDB, node.Table.Name);
                        }
                        else
                        {
                            txtSql.Text = getTableCreateSql(sourceDB, node.Table.Name);
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
        /// 当前建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableCurrentCreate_Click(object sender, EventArgs e)
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

                if (node.Table == null)
                {
                    txtSql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }
                //缺表从对方数据库获取建表sql，否则自身数据库获取sql
                switch (node.Differences)
                {
                    case DifferencesType.lack:
                        var table = new Table();
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getTableCreateSql(targetDB, node.Table.Name);
                        }
                        else
                        {
                            txtSql.Text = getTableCreateSql(sourceDB, node.Table.Name);
                        }
                        break;
                    case DifferencesType.differences:
                    case DifferencesType.common:
                    case DifferencesType.unique:
                        if (node.IsSourceDB)
                        {
                            txtSql.Text = getTableCreateSql(sourceDB, node.Table.Name);
                        }
                        else
                        {
                            txtSql.Text = getTableCreateSql(targetDB, node.Table.Name);
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
        /// 所有差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableAllDiff_Click(object sender, EventArgs e)
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
                List<Table> tables = new List<Table>();
                StringBuilder sql = new StringBuilder(100);

                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getTowTableDiffSql(node.Table, targetdb.Tables[node.Table.Name]));
                                break;
                            case DifferencesType.lack:
                                tables.Add(targetdb.Tables[node.Table.Name]);
                                break;
                        }
                    }
                    if (tables.Count > 0)
                    {
                        sql.Append(getTableCreateSql(targetServer, tables));
                    }
                }
                else
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.differences:
                                sql.Append(getTowTableDiffSql(node.Table, sourcedb.Tables[node.Table.Name]));
                                break;
                            case DifferencesType.lack:
                                tables.Add(sourcedb.Tables[node.Table.Name]);
                                break;
                        }
                    }
                    if (tables.Count > 0)
                    {
                        sql.Append(getTableCreateSql(sourceServer, tables));
                    }
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }

        }

        /// <summary>
        /// 全部建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableAllCreate_Click(object sender, EventArgs e)
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
                List<Table> sourceTables = new List<Table>();
                List<Table> targetTables = new List<Table>();
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                StringBuilder sql = new StringBuilder(100);

                //缺少的表从对方数据库获取建表sql，否则从本身数据库获取sql
                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        switch (node.Differences)
                        {
                            case DifferencesType.lack:
                                targetTables.Add(targetdb.Tables[node.Table.Name]);
                                break;
                            default:
                                sourceTables.Add(node.Table);
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
                                sourceTables.Add(sourcedb.Tables[node.Table.Name]);
                                break;
                            default:
                                targetTables.Add(node.Table);
                                break;
                        }
                    }
                }

                if (sourceTables.Count > 0)
                {
                    sql.Append(getTableCreateSql(sourceServer, sourceTables));
                }
                if (targetTables.Count > 0)
                {
                    sql.Append(getTableCreateSql(targetServer, targetTables));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当前颜色全部差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableColorAllDiff_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                CustomTreeNode selectnode;
                TreeNodeCollection nodes = null;

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
                if (selectnode.Table == null)
                {
                    txtSql.Text = "--消息：右键表才能生成脚本。";
                    return;
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
                StringBuilder sql = new StringBuilder(100);
                List<Table> tables = new List<Table>();
                //差异获取差异sql，却表获取建表sql
                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.differences:
                                    sql.Append(getTowTableDiffSql(node.Table, targetdb.Tables[node.Table.Name]));
                                    break;
                                case DifferencesType.lack:
                                    tables.Add(targetdb.Tables[node.Table.Name]);
                                    break;
                            }
                        }
                    }
                    if (tables.Count > 0)
                    {
                        sql.Append(getTableCreateSql(targetServer, tables));
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
                                    sql.Append(getTowTableDiffSql(node.Table, sourcedb.Tables[node.Table.Name]));
                                    break;
                                case DifferencesType.lack:
                                    tables.Add(sourcedb.Tables[node.Table.Name]);
                                    break;
                            }
                        }
                    }
                    if (tables.Count > 0)
                    {
                        sql.Append(getTableCreateSql(sourceServer, tables));
                    }
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();
            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当前颜色全部建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableColorAllCreate_Click(object sender, EventArgs e)
        {
            try
            {
                showMessage("正在执行...");
                CustomTreeNode selectnode;
                TreeNodeCollection nodes = null;

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
                if (selectnode.Table == null)
                {
                    txtSql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                List<Table> sourceTables = new List<Table>();
                List<Table> targetTables = new List<Table>();
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                StringBuilder sql = new StringBuilder(100);
                //缺少的表从对方数据库获取建表sql，否则从本身数据库获取sql
                if (((CustomTreeNode)nodes[0].Nodes[0]).IsSourceDB)
                {
                    foreach (CustomTreeNode node in nodes[0].Nodes)
                    {
                        if (node.Differences == selectnode.Differences)
                        {
                            switch (node.Differences)
                            {
                                case DifferencesType.lack:
                                    targetTables.Add(targetdb.Tables[node.Table.Name]);
                                    break;
                                default:
                                    sourceTables.Add(node.Table);
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
                                    sourceTables.Add(sourcedb.Tables[node.Table.Name]);
                                    break;
                                default:
                                    targetTables.Add(node.Table);
                                    break;
                            }
                        }
                    }
                }

                if (sourceTables.Count > 0)
                {
                    sql.Append(getTableCreateSql(sourceServer, sourceTables));
                }
                if (targetTables.Count > 0)
                {
                    sql.Append(getTableCreateSql(targetServer, targetTables));
                }

                txtSql.Text = sql.Length == 0 ? "--消息：无生成脚本。" : sql.ToString();

            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }

        }

        /// <summary>
        /// 隐藏/显示缺少的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsTableHidden_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                txtSql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 比较源数据库比目标数据库差了哪些字段
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private string getTowTableDiffSql(Table source, Table target)
        {
            StringBuilder diff = new StringBuilder(100);
            diff.AppendFormat("--script for table {0}\r\n", source.Name);
            foreach (Column col in target.Columns)
            {
                if (source.Columns.Contains(col.Name))
                {
                    if (!IsEqualTowCol(source.Columns[col.Name], col))
                    {
                        diff.AppendFormat("alter table {0}\r\nalter column {1} {2} {3} \r\n", source.Name, col.Name, col.DataType.IsStringType ? string.Format("{0}({1})", col.DataType.Name, col.DataType.MaximumLength == -1 ? "MAX" : col.DataType.MaximumLength.ToString()) : col.DataType.Name, col.Nullable ? "null" : "not null");
                    }
                }
                else
                {
                    diff.AppendFormat("alter table {0}\r\nadd {1} {2} {3} \r\n", source.Name, col.Name, col.DataType.IsStringType ? string.Format("{0}({1})", col.DataType.Name, col.DataType.MaximumLength == -1 ? "MAX" : col.DataType.MaximumLength.ToString()) : col.DataType.Name, col.Nullable ? "null" : "not null");
                }
            }
            diff.Append("--\r\n");
            return diff.ToString();
        }

        private Table getTableByName(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            var dbName = sourceConn.Database;
            Database db = srv.Databases[dbName];
            Table table = new Table();

            if (db.Tables.Contains(name))
            {
                table = db.Tables[name];
            }
            sourceConn.Dispose();
            return table;
        }

        private string getTableCreateSql(Server srv, Table table)
        {
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            //不显示依赖
            scrp.Options.WithDependencies = false;
            scrp.Options.Indexes = true;
            scrp.Options.DriAllConstraints = true;
            scrp.Options.IncludeHeaders = true;
            scrp.Options.IncludeIfNotExists = true;
            StringBuilder sql = new StringBuilder();

            System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { table.Urn });
            //sql.AppendFormat("--script for table {0}\r\n", table.Name);
            foreach (string st in sc)
            {
                sql.AppendFormat("{0}\r\n", st);
            }
            //sql.Append("--\r\n");

            return sql.ToString();
        }

        private string getTableCreateSql(Server srv, List<Table> tables)
        {
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true;
            scrp.Options.DriAllConstraints = true;
            scrp.Options.IncludeHeaders = true;
            scrp.Options.IncludeIfNotExists = true;
            StringBuilder sql = new StringBuilder();
            UrnCollection urnCollection = new UrnCollection();

            foreach (Table tb in tables)
            {
                urnCollection.Add(tb.Urn);
            }

            System.Collections.Specialized.StringCollection sc = scrp.Script(urnCollection);
            foreach (string st in sc)
            {
                sql.AppendFormat("{0}\r\n", st);
            }
            //sql.Append("--\r\n");

            return sql.ToString();
        }

        private string getTableCreateSql(string connect, string name)
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

            // Iterate through the tables in database and script each one. Display the script.

            if (db.Tables.Contains(name))
            {
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { db.Tables[name].Urn});
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

    }
}
