using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareMSSQL.Enum;
using Microsoft.SqlServer.Management.Smo;

namespace CompareMSSQL.Entity
{
    class DBProcedureView : CompareSourceView
    {
        public DBProcedureView()
        {

        }

        public DBProcedureView(StoredProcedure procedure, bool isSource, DifferencesType dt, string connectStr = "", string compareConnectStr = "")
        {
            DBStoredProcedure = procedure;
            IsSourceDB = isSource;
            Differences = dt;
            ConnectString = connectStr;
            CompareConnectString = compareConnectStr;
        }

        /// <summary>
        /// 存储过程
        /// </summary>
        public StoredProcedure DBStoredProcedure { get; set; }
    }
}
