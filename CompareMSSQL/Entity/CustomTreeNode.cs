using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeNode = System.Windows.Forms.TreeNode;
using Microsoft.SqlServer.Management.Smo;
using CompareMSSQL.Enum;

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

        public DifferencesType Differences { get; set; }

        public bool IsSourceDB { get; set; }

        public Table Table { get; set; }

        public View View { get; set; }

        public StoredProcedure Procedure { get; set; }

        public UserDefinedFunction Function { get; set; }

        public Column Column { get; set; }

        public DataType DataType { get; set; }

    }
}
