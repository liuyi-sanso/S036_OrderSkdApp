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
    public class GroupMstAF
    {
        /// <summary>
        /// 三相メイン.dbo.部門マスタのデータを取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetSansoMainGroupMst(string groupCode)
        {
            string sql = " SELECT       部門コード, 部名, グループ名, 係名, 部門名, " +
                         "              本社営業所区分, 住所, 電話番号, 作成日付, 変更日付, (ISNULL(製販区分, '') as 製販区分,有効部門区分 " +
                         " FROM         三相メイン.dbo.部門マスタ " +
                         " WHERE        (部門コード = '" + groupCode + "') AND (有効部門区分 = '1') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        ///  製造調達.dbo.部門マスタの処理年月　取得
        /// </summary>
        /// <param name="gruopCode">部門コード</param>
        public (bool IsOk, DataTable Table) GetExecuteDate(string gruopCode)
        {
            string sql =
            "SELECT " +
            "FORMAT(処理年月, 'yyyy/MM') AS 処理年月 " +
            "FROM " +
            "製造調達.dbo.部門マスタ " +
            "WHERE " +
            "部門コード = '" + gruopCode + "' ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }
    }
}
