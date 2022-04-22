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
    public class MW_OrdSkdDMstAF
    {
        /// <summary>
        /// MW_OrdSkdDMstのデータを取得
        /// </summary>
        /// <param name="instrCode">指示番号</param>
        public (bool IsOk, DataTable Table) GetWOrdSkdDMstList(string instrCode)
        {
            string sql = "SELECT " +
                        "A.InstrCode AS 指示番号, " +
                        "A.PoCode AS 注文番号, " +
                        "A.SkdCode AS 日程番号, " +
                        "CASE WHEN LEN(ISNULL(A.Sakuban, '')) = 12 " +
                        "THEN SUBSTRING(A.Sakuban, 1, 4) + '-' + SUBSTRING(A.Sakuban, 5, 6) + '-' + " +
                        "SUBSTRING(A.Sakuban, 11, 2)  ELSE A.Sakuban END AS 作番," +
                        "A.PartsCode AS 部品コード, " +
                        "B.PartsName AS 部品名, " +
                        "A.SupCode AS 仕入先コード, " +
                        "C.仕入先名１ AS 仕入先名, " +
                        "A.GroupCode AS 部門コード, " +
                        "A.BizCode AS 業務コード, " +
                        "A.DelivDate AS 納入日付, " +
                        "SUM(ISNULL(A.DelivNum, 0)) AS 納入数量, " +
                        "A.OrdStaffID AS 担当者, " +
                        "A.StockCate AS 在庫P, " +
                        "A.LT AS リードタイム, " +
                        "A.MinLotNum AS 最低ロット数, " +
                        "A.LotNum AS ロット数, " +
                        "ISNULL(A.RqmtNum, 0) AS 所要量, " +
                        "ISNULL(A.AllocNum, 0) AS 実引当数, " +
                        "ISNULL(A.PlanAllocNum, 0) AS 予定引当数, " +
                        "A.ReqStaffID AS 依頼者, " +
                        "A.ReqGroupCode AS 依頼部門, " +
                        "A.ReqCode AS 依頼番号, " +
                        "A.Remarks AS 備考, " +
                        "A.CreateDate AS 作成日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.CreateID GROUP BY StaffName) AS 作成者, " +
                        "A.UpdateDate AS 変更日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.UpdateID GROUP BY StaffName) AS 変更者 " +
                        "FROM SANSODB.dbo.MW_OrdSkdDMst AS A " +
                        "LEFT OUTER JOIN SANSODB.dbo.MW_OrdSkdMst AS B ON A.InstrCode = B.InstrCode " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.SupCode = C.仕入先コード " +
                        "WHERE A.InstrCode = '" + instrCode + "' " +
                        "GROUP BY " +
                        "A.InstrCode, " +
                        "A.PoCode, " +
                        "A.SkdCode, " +
                        "A.Sakuban, " +
                        "A.PartsCode, " +
                        "B.PartsName, " +
                        "A.SupCode, " +
                        "C.仕入先名１, " +
                        "A.GroupCode, " +
                        "A.BizCode, " +
                        "A.DelivDate, " +
                        "A.OrdStaffID, " +
                        "A.StockCate, " +
                        "A.LT, " +
                        "A.MinLotNum, " +
                        "A.LotNum, " +
                        "ISNULL(A.RqmtNum, 0), " +
                        "ISNULL(A.AllocNum, 0), " +
                        "ISNULL(A.PlanAllocNum, 0), " +
                        "A.ReqStaffID, " +
                        "A.ReqGroupCode, " +
                        "A.ReqCode, " +
                        "A.Remarks, " +
                        "A.CreateDate, " +
                        "A.CreateID, " +
                        "A.UpdateDate, " +
                        "A.UpdateID ";
            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }
    }
}