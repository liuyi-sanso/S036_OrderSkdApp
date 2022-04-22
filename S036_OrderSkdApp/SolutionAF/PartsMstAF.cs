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
    class PartsMstAF
    {

        /// <summary>
        /// 部品マスタ抽出
        /// </summary>
        /// <param name="searchCode"> 検索コード(完全一致) </param>
        /// <remarks>検索コードは 部品コード → 部品名の順で検索 </remarks>
        /// <remarks>searchCodeを指定しない場合は、全件を抽出 </remarks>
        public (bool IsOk, DataTable Table) GetPartsMST(string searchCode = "")
        {
            string sql = "SELECT " +
                        "部品コード, 部品名, 図面番号, 部品区分, 備考, 収容種別, 収容数, 客先部品番号, " +
                        "部品休止Ｆ, 棚卸除外Ｆ, 作成者, 変更者, " +
                        "C.StaffName AS 作成者,CONVERT(VARCHAR,A.作成日付,120) AS 作成日付, " +
                        "D.StaffName AS 変更者,CONVERT(VARCHAR,A.変更日付,120) AS 変更日付 " +
                        "FROM 製造調達.dbo.部品マスタ AS A " +
                        "left join 三相メイン.dbo.M_STAFF AS C on A.作成者 = C.ID and C.CurrentGroupFLG = '1' " +
                        "left join 三相メイン.dbo.M_STAFF AS D on A.変更者 = D.ID and D.CurrentGroupFLG = '1' ";
            string whereSql = "";

            if (searchCode == "")
            {
                //全件抽出
                var af = CommonAF.ExecutSelectSQL(sql);
                if (af.IsOk == true && af.Table.Rows.Count != 0)
                {
                    return (true, af.Table);
                }
            }
            else
            {
                // 部品コードで検索
                whereSql = "where 部品コード = '" + searchCode + "'";
                var af = CommonAF.ExecutSelectSQL(sql + whereSql);
                if (af.IsOk == true)
                {
                    return (true, af.Table);
                }

                // 図面番号コードで検索
                whereSql = "where 図面番号 = '" + searchCode + "'";
                af = CommonAF.ExecutSelectSQL(sql + whereSql);
                if (af.IsOk == true)
                {
                    return (true, af.Table);
                }
            }

            return (false, null);

        }
    }
}
