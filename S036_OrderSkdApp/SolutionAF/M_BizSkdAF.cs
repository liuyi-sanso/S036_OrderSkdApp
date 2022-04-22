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
    public class M_BizSkdAF
    {
        /// <summary>
        /// 加工日程マスタにW発注日程マスタのデータを追加
        /// </summary>
        /// <param name="searchCate">絞り込み項目</param>
        /// <param name="searchCode">絞り込みコード</param>
        /// <returns>True：更新成功　　 False：更新失敗</returns>
        public (bool IsOk, DataTable Table) InsertMBizSkd(string searchCate, string searchCode)
        {
            string sql = "INSERT INTO SANSODB.dbo.M_BizSkd( " +
                        "BizCode, SkdCode, SkdDate, PartsCode, SkdNum, Sakuban, " +
                        "CusCode, GroupCode, StateCate, Remarks, CreateDate, CreateID) " +
                        "SELECT " +
                        "A.BizCode, B.SkdCode, A.DelivDate, A.PartsCode, A.DelivNum, B.Sakuban, " +
                        "A.GroupCode, A.SupCode, '0', A.Remarks, '" + DateTime.Now + "', '" + LoginInfo.Instance.UserId + "' " +
                        "FROM SANSODB.dbo.MW_OrdSkdMst AS A " +
                        "LEFT OUTER JOIN (SELECT InstrCode, SkdCode, Sakuban " +
                        "                 FROM SANSODB.dbo.MW_OrdSkdDMst " +
                        "                 GROUP BY InstrCode, SkdCode, Sakuban) AS B ON A.InstrCode = B.InstrCode " +
                        "WHERE NOT EXISTS (SELECT BizCode, SkdCode " +
                        "                  FROM SANSODB.dbo.M_BizSkd AS C " +
                        "                  WHERE A.BizCode = C.BizCode AND B.SkdCode = C.SkdCode) " +
                        "AND ISNULL(B.SkdCode, '') <> '' ";

            switch (searchCate)
            {
                // 発注課別
                case "0":
                    sql += "AND A.GroupCode = '" + searchCode + "' ";
                    break;

                // 仕入先担当者
                case "1":
                    sql += "AND A.OrdStaffID = '" + searchCode + "' ";
                    break;

                // 仕入先コード
                case "2":
                    sql += "AND A.SupCode = '" + searchCode + "' ";
                    break;

                default:
                    return (false, null);
            }

            // 更新した件数を返す
            sql += "SELECT ROWCOUNT_BIG() ";

            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }
    }
}