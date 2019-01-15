using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeNode = System.Windows.Forms.TreeNode;
using Microsoft.SqlServer.Management.Smo;

namespace CompareMSSQL.Entity
{
    class CustomTreeNode : TreeNode
    {

        public CustomTreeNode()
        {
            
        }

        /// <summary>
        /// 是否父节点
        /// </summary>
        public bool IsParent { get; set; }

        public bool CanMenu { get; set; }

        public bool IsTable { get; set; }

        public bool IsColumn { get; set; }

        public bool IsColType { get; set; }

        public DBTableView TableView { get; set; }

        public Table Table { get; set; }

        public Column Column { get; set; }

        public DataType DataType { get; set; }

    }
}
