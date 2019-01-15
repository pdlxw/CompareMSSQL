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

namespace CompareMSSQL.SubForm
{
    public partial class CompareTable : Form
    {
        /// <summary>
        /// 源数据库连接字
        /// </summary>
        private string sourceDB;
        /// <summary>
        /// 目标数据库连接字
        /// </summary>
        private string targetDB;
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

        public CompareTable(string sourceDB, string targetDB)
        {
            this.sourceDB = sourceDB;
            this.targetDB = targetDB;  
            
            InitializeComponent();

            setTableView();

            setTreeView(source_table_tv, true);
            setTreeView(target_table_tv, false);
        }


        private void setTableView()
        {
            try
            {
                var sourceDatabase = getDB(sourceDB) ?? new Database();
                var targetDatabase = getDB(targetDB) ?? new Database();

                foreach (Table sdb in sourceDatabase.Tables)
                {
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
                    if (!sourceDatabase.Tables.Contains(ttb.Name))
                    {
                        allTables.Add(new DBTableView(ttb, false, DifferencesType.unique));

                        allTables.Add(new DBTableView(ttb, true, DifferencesType.lack));
                    }
                }

            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        [Obsolete]
        private void setTableView_obsolete()
        {
            try
            {
                List<Table> sourceTable = getDBTable(sourceDB, false);
                List<Table> targetTable = getDBTable(targetDB, false);
                //var tableView = new DBTableView();
                //目标数据库独有的
                var tUnique = targetTable.Where(t => !sourceTable.Select(s => s.Name).ToList().Contains(t.Name));
                foreach (var tu in tUnique)
                {
                    allTables.Add(new DBTableView(tu, false, DifferencesType.unique, targetDB, sourceDB));

                    allTables.Add(new DBTableView(tu, true, DifferencesType.lack));
                }
                ///遍历源数据库，如匹配则比较表结构
                foreach (Table tb in sourceTable)
                {
                    Table matchTable = targetTable.Where(t => t.Name == tb.Name).FirstOrDefault();
                    if (matchTable == null)
                    {
                        allTables.Add(new DBTableView(tb, true, DifferencesType.unique));

                        allTables.Add(new DBTableView(tb, false, DifferencesType.lack));
                    }
                    else if (IsEqualTowTable(tb, matchTable))
                    {
                        allTables.Add(new DBTableView(tb, true, DifferencesType.common));

                        allTables.Add(new DBTableView(matchTable, false, DifferencesType.common));
                    }
                    else
                    {
                        allTables.Add(new DBTableView(tb, true, DifferencesType.differences));

                        allTables.Add(new DBTableView(matchTable, false, DifferencesType.differences));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        private void setTreeView(TreeView tv, bool isSource)
        {
            try
            {
                var tables = allTables.Where(tb => tb.IsSourceDB == isSource).OrderBy(tb => tb.DBTable.Name);
                CustomTreeNode startNode = new CustomTreeNode();
                startNode.IsParent = true;
                startNode.CanMenu = true;
                startNode.Text = "所有表([黑:同][绿:独有][黄:差异][灰:缺])";
                startNode.Tag = new TableInfoTag
                {
                    IsParent = true,
                    CanMenu = true
                };
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
                    tempNode.IsTable = true;
                    tempNode.Table = table.DBTable;
                    tempNode.Text = table.DBTable.Name;
                    tempNode.Tag = new TableInfoTag
                    {
                        IsParent = true,
                        CanMenu = true,
                        IsTable = true,
                        Table = table.DBTable,
                        TableView = table
                    };
                    tempNode.TableView = table;
                    tempNode.ForeColor = color;
                    startNode.Nodes.Add(tempNode);

                    var colNode = new CustomTreeNode();
                    foreach (Column col in table.DBTable.Columns)
                    {
                        colNode = new CustomTreeNode();
                        colNode.IsColumn = true;
                        colNode.Column = col;
                        colNode.Text = string.Format("{0}({1}{2}{3})", col.Name, col.InPrimaryKey ? "PK," : "", col.DataType.IsStringType ? string.Format("{0}({1}),", col.DataType.Name, col.DataType.MaximumLength) : col.DataType.Name + ",", col.Nullable ? "null" : "not null");
                        colNode.ForeColor = color;
                        colNode.Tag = new TableInfoTag
                        {
                            IsColumn = true
                        };
                        tempNode.Nodes.Add(colNode);
                    }
                }
                startNode.Expand();
            }
            catch (Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
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
            if (!string.IsNullOrEmpty(tb_Sql.Text))
            {
                Clipboard.SetDataObject(tb_Sql.Text);
            }         
        }

        private void source_table_tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                source_table_tv.SelectedNode = e.Node;
                //var tag = (TableInfoTag)e.Node.Tag;
                if (((CustomTreeNode)e.Node).IsTable)
                {
                    currentIsSource = true;
                    e.Node.ContextMenuStrip = cms_table;
                    //cms_top.Show(source_table_tv, e.X, e.Y);
                }               
            }
        }

        private void target_table_tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                target_table_tv.SelectedNode = e.Node;
                //var tag = (TableInfoTag)e.Node.Tag;
                if (((CustomTreeNode)e.Node).IsTable)
                {
                    currentIsSource = false;
                    e.Node.ContextMenuStrip = cms_table;
                    //cms_top.Show(source_table_tv, e.X, e.Y);
                }
            }
        }

        /// <summary>
        /// 当前差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_currentdiff_Click(object sender, EventArgs e)
        {
            try
            {
                //var tag = (TableInfoTag)source_table_tv.SelectedNode.Tag;
                CustomTreeNode node;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    node = (CustomTreeNode)source_table_tv.SelectedNode;
                }
                else
                {
                    node = (CustomTreeNode)target_table_tv.SelectedNode;
                }

                if (!node.IsTable)
                {
                    tb_Sql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }
                //差异及缺表获取sql
                switch (node.TableView.Differences)
                {
                    case DifferencesType.differences:
                        var table = new Table();
                        if (node.TableView.IsSourceDB)
                        {
                            table = getTableByName(targetDB, node.Table.Name);
                        }
                        else
                        {
                            table = getTableByName(sourceDB, node.Table.Name);
                        }
                        tb_Sql.Text = getTowTableDiffSql(node.Table, table);
                        break;
                    case DifferencesType.lack:
                        if (node.TableView.IsSourceDB)
                        {
                            tb_Sql.Text = getTableCreateSql(targetDB, node.Table.Name);
                        }
                        else
                        {
                            tb_Sql.Text = getTableCreateSql(sourceDB, node.Table.Name);
                        }
                        break;
                    case DifferencesType.common:
                    case DifferencesType.unique:
                    default:
                        tb_Sql.Text = "--消息：无差异脚本。";
                        break;
                }
            }
            catch (Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当前建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_currentcreate_Click(object sender, EventArgs e)
        {
            try
            {
                CustomTreeNode node;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    node = (CustomTreeNode)source_table_tv.SelectedNode;
                }
                else
                {
                    node = (CustomTreeNode)target_table_tv.SelectedNode;
                }

                if (!node.IsTable)
                {
                    tb_Sql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }
                //缺表从对方数据库获取建表sql，否则自身数据库获取sql
                switch (node.TableView.Differences)
                {
                    case DifferencesType.lack:
                        var table = new Table();
                        if (node.TableView.IsSourceDB)
                        {
                            tb_Sql.Text = getTableCreateSql(targetDB, node.Table.Name);
                        }
                        else
                        {
                            tb_Sql.Text = getTableCreateSql(sourceDB, node.Table.Name);
                        }
                        break;
                    case DifferencesType.differences:
                    case DifferencesType.common:
                    case DifferencesType.unique:
                        if (node.TableView.IsSourceDB)
                        {
                            tb_Sql.Text = getTableCreateSql(sourceDB, node.Table.Name);
                        }
                        else
                        {
                            tb_Sql.Text = getTableCreateSql(targetDB, node.Table.Name);
                        }
                        break;
                    default:
                        tb_Sql.Text = "--消息：无脚本。";
                        break;
                }
            }
            catch (Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 所有差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_alldiff_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNodeCollection nodes = null;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    nodes = source_table_tv.Nodes;
                }
                else
                {
                    nodes = target_table_tv.Nodes;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    tb_Sql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                var sql = string.Empty;
                //差异获取差异sql，却表获取建表sql
                foreach (CustomTreeNode node in nodes[0].Nodes)
                {
                    switch (node.TableView.Differences)
                    {
                        case DifferencesType.differences:
                            if (node.TableView.IsSourceDB)
                            {
                                sql += getTowTableDiffSql(node.Table, targetdb.Tables[node.Table.Name]);
                            }
                            else
                            {
                                sql += getTowTableDiffSql(node.Table, sourcedb.Tables[node.Table.Name]);
                            }
                            break;
                        case DifferencesType.lack:
                            if (node.TableView.IsSourceDB)
                            {
                                sql += getTableCreateSql(targetServer, targetdb.Tables[node.Table.Name]);
                            }
                            else
                            {
                                sql += getTableCreateSql(sourceServer, sourcedb.Tables[node.Table.Name]);
                            }
                            break;
                    }
                }

                tb_Sql.Text = sql == "" ? "--消息：无生成脚本。" : sql;
            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }

        }

        /// <summary>
        /// 全部建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_allcreate_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNodeCollection nodes = null;
                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    nodes = source_table_tv.Nodes;
                }
                else
                {
                    nodes = target_table_tv.Nodes;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    tb_Sql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                var sql = string.Empty;
                //缺少的表从对方数据库获取建表sql，否则从本身数据库获取sql
                foreach (CustomTreeNode node in nodes[0].Nodes)
                {
                    switch (node.TableView.Differences)
                    {
                        case DifferencesType.lack:
                            if (node.TableView.IsSourceDB)
                            {
                                sql += getTableCreateSql(targetServer, targetdb.Tables[node.Table.Name]);
                            }
                            else
                            {
                                sql += getTableCreateSql(sourceServer, sourcedb.Tables[node.Table.Name]);
                            }
                            break;
                        default:
                            if (node.TableView.IsSourceDB)
                            {
                                sql += getTableCreateSql(sourceServer, node.Table);
                            }
                            else
                            {
                                sql += getTableCreateSql(targetServer, node.Table);
                            }
                            break;
                    }
                }

                tb_Sql.Text = sql == "" ? "--消息：无生成脚本。" : sql;
            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当前颜色全部差异脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_coloralldiff_Click(object sender, EventArgs e)
        {
            try
            {
                CustomTreeNode selectnode;
                TreeNodeCollection nodes = null;

                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    selectnode = (CustomTreeNode)source_table_tv.SelectedNode;
                    nodes = source_table_tv.Nodes;
                }
                else
                {
                    selectnode = (CustomTreeNode)target_table_tv.SelectedNode;
                    nodes = target_table_tv.Nodes;
                }
                if (!selectnode.IsTable)
                {
                    tb_Sql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }
                //相同或者独有的无差异脚本
                if (selectnode.TableView.Differences == DifferencesType.common || selectnode.TableView.Differences == DifferencesType.unique)
                {
                    tb_Sql.Text = "--消息：无生成脚本。";
                    return;
                }

                if (nodes.Count <= 0 || nodes[0].Nodes.Count <= 0)
                {
                    tb_Sql.Text = "--消息：无生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                var sql = string.Empty;
                //差异获取差异sql，却表获取建表sql
                foreach (CustomTreeNode node in nodes[0].Nodes)
                {
                    if (node.TableView.Differences == selectnode.TableView.Differences)
                    {
                        switch (node.TableView.Differences)
                        {
                            case DifferencesType.differences:
                                if (node.TableView.IsSourceDB)
                                {
                                    sql += getTowTableDiffSql(node.Table, targetdb.Tables[node.Table.Name]);
                                }
                                else
                                {
                                    sql += getTowTableDiffSql(node.Table, sourcedb.Tables[node.Table.Name]);
                                }
                                break;
                            case DifferencesType.lack:
                                if (node.TableView.IsSourceDB)
                                {
                                    sql += getTableCreateSql(targetServer, targetdb.Tables[node.Table.Name]);
                                }
                                else
                                {
                                    sql += getTableCreateSql(sourceServer, sourcedb.Tables[node.Table.Name]);
                                }
                                break;
                        }
                    }
                }

                tb_Sql.Text = sql == "" ? "--消息：无生成脚本。" : sql;
            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当前颜色全部建表脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_colorallcreate_Click(object sender, EventArgs e)
        {
            try
            {
                CustomTreeNode selectnode;
                TreeNodeCollection nodes = null;

                //根据右键的树，获取树的节点
                if (currentIsSource)
                {
                    selectnode = (CustomTreeNode)source_table_tv.SelectedNode;
                    nodes = source_table_tv.Nodes;
                }
                else
                {
                    selectnode = (CustomTreeNode)target_table_tv.SelectedNode;
                    nodes = target_table_tv.Nodes;
                }
                if (!selectnode.IsTable)
                {
                    tb_Sql.Text = "--消息：右键表才能生成脚本。";
                    return;
                }

                Server sourceServer, targetServer;
                Database sourcedb, targetdb;
                getServer(sourceDB, out sourceServer, out sourcedb);
                getServer(targetDB, out targetServer, out targetdb);
                var sql = string.Empty;
                //缺少的表从对方数据库获取建表sql，否则从本身数据库获取sql
                foreach (CustomTreeNode node in nodes[0].Nodes)
                {
                    if (node.TableView.Differences == selectnode.TableView.Differences)
                    {
                        switch (node.TableView.Differences)
                        {
                            case DifferencesType.lack:
                                if (node.TableView.IsSourceDB)
                                {
                                    sql += getTableCreateSql(targetServer, targetdb.Tables[node.Table.Name]);
                                }
                                else
                                {
                                    sql += getTableCreateSql(sourceServer, sourcedb.Tables[node.Table.Name]);
                                }
                                break;
                            default:
                                if (node.TableView.IsSourceDB)
                                {
                                    sql += getTableCreateSql(sourceServer, node.Table);
                                }
                                else
                                {
                                    sql += getTableCreateSql(targetServer, node.Table);
                                }
                                break;
                        }
                    }
                }

                tb_Sql.Text = sql == "" ? "--消息：无生成脚本。" : sql;

            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
            }

        }

        /// <summary>
        /// 隐藏/显示缺少的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cms_table_hidden_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                tb_Sql.Text = string.Format("--消息：{0}", ex.Message);
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
            string diff = string.Empty;
            diff += string.Format("--script for table {0}\r\n", source.Name);
            foreach (Column col in target.Columns)
            {
                if (source.Columns.Contains(col.Name))
                {
                    if (!IsEqualTowCol(source.Columns[col.Name], col))
                    {
                        diff += string.Format("alter table {0}\r\nalter column {1} {2} {3} \r\n", source.Name, col.Name, col.DataType.IsStringType ? string.Format("{0}({1})", col.DataType.Name, col.DataType.MaximumLength == -1 ? "MAX" : col.DataType.MaximumLength.ToString()) : col.DataType.Name, col.Nullable ? "null" : "not null");
                    }
                }
                else
                {
                    diff += string.Format("alter table {0}\r\nadd {1} {2} {3} \r\n", source.Name, col.Name, col.DataType.IsStringType ? string.Format("{0}({1})", col.DataType.Name, col.DataType.MaximumLength == -1 ? "MAX" : col.DataType.MaximumLength.ToString()) : col.DataType.Name, col.Nullable ? "null" : "not null");
                }
            }
            diff += "--\r\n";
            return diff;
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
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true;
            scrp.Options.DriAllConstraints = true;
            var sql = string.Empty;

            System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { table.Urn });
            sql += string.Format("--script for table {0}\r\n", table.Name);
            foreach (string st in sc)
            {
                sql += string.Format("{0}\r\n", st);
            }
            sql += "--\r\n";

            return sql;
        }

        private string getTableCreateSql(string connect, string name)
        {
            var sourceConn = new SqlConnection(connect);
            Server srv = new Server(new ServerConnection(sourceConn));
            //Server srv = new Server(".");
            var dbName = sourceConn.Database;
            // Reference the database.
            Database db = srv.Databases[dbName];
            var sql = string.Empty;

            // Define a Scripter object and set the required scripting options.   
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true;   // To include indexes  
            scrp.Options.DriAllConstraints = true;   // to include referential constraints in the script  

            // Iterate through the tables in database and script each one. Display the script.

            if (db.Tables.Contains(name))
            {
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { db.Tables[name].Urn });
                sql += string.Format("--script for table {0}\r\n", name);
                foreach (string st in sc)
                {
                    sql += string.Format("{0}\r\n", st);
                }
                sql += "--\r\n";
            }
            sourceConn.Dispose();
            return sql;               
        }

        /// <summary>
        /// server、db
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="srv"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private bool getServer(string connect, out Server srv, out Database db)
        {
            var sourceConn = new SqlConnection(connect);
            srv = new Server(new ServerConnection(sourceConn));

            var dbName = sourceConn.Database;

            db = srv.Databases[dbName];
            sourceConn.Dispose();
            return true;
        }

    }
}
