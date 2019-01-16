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

namespace CompareMSSQL.SubForm
{
    public partial class CommonWin : Form
    {

        /// <summary>
        /// 源数据库连接字
        /// </summary>
        protected string sourceDB;
        /// <summary>
        /// 目标数据库连接字
        /// </summary>
        protected string targetDB;

        public CommonWin()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 复制脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopySql_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSql.Text))
            {
                Clipboard.SetDataObject(txtSql.Text);
            }
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        protected void showMessage(string msg)
        {
            txtSql.Text = msg;
            txtSql.Refresh();
        }

        /// <summary>
        /// server、db
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="srv"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        protected bool getServer(string connect, out Server srv, out Database db)
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
