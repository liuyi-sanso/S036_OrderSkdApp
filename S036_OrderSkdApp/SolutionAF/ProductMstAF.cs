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
    /// <summary>
    /// 機種マスタ
    /// </summary>
    public class ProductMstAF
    {
        /// <summary>
        /// 機種マスタ
        /// </summary>
        /// <param name="searchCode">検索条件</param>
        /// <remarks>機種コード、機種名の順に検索します</remarks>
        public (bool IsOk, DataTable Table) GetProductMst(string searchCode)
        {
            string sql = "SELECT * "
                       + "FROM 三相メイン.dbo.機種マスタ AS A ";

            string sqlWhere = $"WHERE A.機種コード = '{ searchCode }' ";

            var result = CommonAF.ExecutSelectSQL(sql + sqlWhere);
            if (result.IsOk == false)
            {
                return (result.IsOk, result.Table);
            }

            if(result.Table.Rows.Count >= 1)
            {
                return (true, result.Table);
            }

            sqlWhere = $"WHERE A.機種名 = '{ searchCode }' ";

            result = CommonAF.ExecutSelectSQL(sql + sqlWhere);

            return (result.IsOk, result.Table);
        }
    }
}
