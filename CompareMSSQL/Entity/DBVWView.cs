using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareMSSQL.Enum;
using Microsoft.SqlServer.Management.Smo;

namespace CompareMSSQL.Entity
{
    class DBVWView : CompareSourceView
    {
        public DBVWView()
        {

        }

        public DBVWView(View view, bool isSource, DifferencesType dt, string connectStr = "", string compareConnectStr = "")
        {
            DBView = view;
            IsSourceDB = isSource;
            Differences = dt;
            ConnectString = connectStr;
            CompareConnectString = compareConnectStr;
        }

        /// <summary>
        /// 视图
        /// </summary>
        public View DBView { get; set; }
    }
}
