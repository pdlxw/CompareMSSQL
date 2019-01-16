using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareMSSQL.Enum;

namespace CompareMSSQL.Entity
{
    class CompareSourceView
    {
        /// <summary>
        /// 是否列到源数据库
        /// </summary>
        public bool IsSourceDB { get; set; }

        /// <summary>
        /// 差异类型
        /// </summary>
        public DifferencesType Differences { get; set; }

        /// <summary>
        /// 数据库连接字
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// 被比较的数据库连接字
        /// </summary>
        public string CompareConnectString { get; set; }
    }
}
