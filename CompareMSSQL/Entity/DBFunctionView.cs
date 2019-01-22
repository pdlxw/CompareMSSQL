using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareMSSQL.Enum;
using Microsoft.SqlServer.Management.Smo;

namespace CompareMSSQL.Entity
{
    class DBFunctionView : CompareSourceView
    {
        public DBFunctionView(UserDefinedFunction function, bool isSource, DifferencesType dt, string connectStr = "", string compareConnectStr = "")
        {
            DBFunction = function;
            IsSourceDB = isSource;
            Differences = dt;
            ConnectString = connectStr;
            CompareConnectString = compareConnectStr;
        }

        /// <summary>
        /// 函数
        /// </summary>
        public UserDefinedFunction DBFunction { get; set; }
    }
}
