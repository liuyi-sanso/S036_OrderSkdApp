using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using C1.Win.C1Input;
using C1.Win.Calendar;
using System.Threading.Tasks;

namespace S036_OrderSkdApp
{
    public class ProcessCodeMstAF
    {
        /// <summary>
        /// 工程番号取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetProcessCode()
        {
            string sql = "SELECT * "
                        + "FROM 部品調達.dbo.工程番号 ORDER BY 工程番号";
            var result = CommonAF.ExecutSelectSQL(sql);

            return (result.IsOk, result.Table);
        }
    }
}
