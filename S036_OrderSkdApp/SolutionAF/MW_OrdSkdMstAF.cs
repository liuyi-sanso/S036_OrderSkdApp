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
    public class MW_OrdSkdMstAF
    {
        /// <summary>
        /// MW_OrdSkdMstの絞込んだデータを取得
        /// </summary>
        /// <param name="searchCate">絞り込み項目</param>
        /// <param name="searchCode">絞り込みコード</param>
        public (bool IsOk, DataTable Table) GetWOrdSkdMst(string searchCate ,string searchCode)
        {
            string sql = "SELECT " +
                        "InstrCode AS 指示番号, " +
                        "PartsName AS 部品名, " +
                        "PartsCode AS 部品コード, " +
                        "SupCode AS 仕入先コード, " +
                        "GroupCode AS 部門コード, " +
                        "BizCode AS 業務コード, " +
                        "DelivDate AS 指示日, " +
                        "DelivNum AS 必要数, " +
                        "OrdStaffID AS 担当者, " +
                        "StockCate AS 在庫P, " +
                        "UnitPriceCate AS 単価区分, " +
                        "UnitPrice AS 単価, " +
                        "ProcUnitPrice AS 加工単価, " +
                        "Price AS 金額, " +
                        "NextPartsCode AS 次工程部品コード, " +
                        "NextProcCode AS 次工程コード, " +
                        "PaintColor AS 塗装色, " +
                        "ReqStaffID AS 依頼者, " +
                        "ReqGroupCode AS 依頼部門, " +
                        "ReqCode AS 依頼番号, " +
                        "KeyFlg AS 重点F, " +
                        "MaintStaffID AS メンテナンス者, " +
                        "MaintDate AS メンテナンス時間, " +
                        "Remarks AS 備考, " +
                        "CreateDate AS 作成日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.CreateID GROUP BY StaffName) AS 作成者, " +
                        "UpdateDate AS 変更日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.UpdateID GROUP BY StaffName) AS 変更者 " +
                        "FROM SANSODB.dbo.MW_OrdSkdMst AS A ";

            switch (searchCate)
            {
                // 発注課別
                case "0":
                    sql += "WHERE GroupCode = '" + searchCode + "' ";
                    break;

                // 仕入先担当者
                case "1":
                    sql += "WHERE OrdStaffID = '" + searchCode + "' ";
                    break;

                // 仕入先コード
                case "2":
                    sql += "WHERE SupCode = '" + searchCode + "' ";
                    break;

                default:
                    return (false, null);
            }

            sql += "ORDER BY InstrCode ";

            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }

        /// <summary>
        /// MW_OrdSkdMstのMaintStaffID（メンテナンス中のスタッフID）のリストを取得
        /// </summary>
        /// <param name="searchCate">絞り込み項目</param>
        /// <param name="searchCode">絞り込みコード</param>
        public (bool IsOk, DataTable Table) GetMaintStaffID(string searchCate, string searchCode)
        {
            string sql = "SELECT " +
                         "MaintStaffID, " +
                         "MaintDate, " + 
                         "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                         "WHERE ID = A.MaintStaffID GROUP BY StaffName) AS メンテ者名 " +
                         "FROM SANSODB.dbo.MW_OrdSkdMst AS A ";

            switch (searchCate)
            {
                // 発注課別
                case "0":
                    sql += "WHERE GroupCode = '" + searchCode + "' ";
                    break;

                // 仕入先担当者
                case "1":
                    sql += "WHERE OrdStaffID = '" + searchCode + "' ";
                    break;

                // 仕入先コード
                case "2":
                    sql += "WHERE SupCode = '" + searchCode + "' ";
                    break;

                default:
                    return (false, null);
            }

            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }

        /// <summary>
        /// MW_OrdSkdMstのMaintStaffID（メンテナンス中のスタッフID）と MaintDate（メンテナンスを初めた日付）を更新する
        /// </summary>
        /// <param name="searchCate">絞り込み項目</param>
        /// <param name="searchCode">絞り込みコード</param>
        /// <param name="maintDate">メンテナンスを初めた日付</param>
        /// <returns>True：更新成功　　 False：更新失敗</returns>
        public bool UpdateMaintStaffID(string searchCate, string searchCode, DateTime maintDate)
        {
            string sql = "UPDATE SANSODB.dbo.MW_OrdSkdMst " +
                  "SET MaintStaffID = '" + LoginInfo.Instance.UserId + "', " +
                  "MaintDate = '" + maintDate + "' ";

            switch (searchCate)
            {
                // 発注課別
                case "0":
                    sql += "WHERE GroupCode = '" + searchCode + "' ";
                    break;

                // 仕入先担当者
                case "1":
                    sql += "WHERE OrdStaffID = '" + searchCode + "' ";
                    break;

                // 仕入先コード
                case "2":
                    sql += "WHERE SupCode = '" + searchCode + "' ";
                    break;

                default:
                    return false;
            }

            var result = CommonAF.ExecutUpdateSQL(sql);
            if (result.IsOk == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// MW_OrdSkdMstのMaintStaffID（メンテナンス中のスタッフID）と MaintDate（メンテナンスを初めた日付）をクリアする
        /// </summary>
        /// <param name="maintStaffID">メンテナンス中のスタッフID</param>
        /// <param name="maintDate">メンテナンスを初めた日付</param>
        /// <returns>True：更新成功　　 False：更新失敗</returns>
        public bool ClearMaintStaffID(string maintStaffID, DateTime maintDate)
        {
            string sql = "UPDATE SANSODB.dbo.MW_OrdSkdMst " +
                  "SET MaintStaffID = NULL, " +
                  "MaintDate = NULL " +
                  "WHERE MaintStaffID = '" + maintStaffID + "' " +
                  "AND MaintDate = '" + maintDate + "' ";
            var result = CommonAF.ExecutUpdateSQL(sql);
            if (result.IsOk == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// MW_OrdSkdMstメンテナンス用のリストを取得
        /// </summary>
        /// <param name="maintStaffID">メンテナンス中のスタッフID</param>
        /// <param name="maintDate">メンテナンスを初めた日付</param>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetWOrdSkdMstList(string maintStaffID, DateTime maintDate, 
                                                              List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var supCode = controlList.SGetText("supCodeC1TextBox");

            string sql = "SELECT " +
                        "InstrCode AS 指示番号, " +
                        "PartsName AS 部品名, " +
                        "PartsCode AS 部品コード, " +
                        "SupCode AS 仕入先コード, " +
                        "GroupCode AS 部門コード, " +
                        "BizCode AS 業務コード, " +
                        "DelivDate AS 指示日, " +
                        "ISNULL(DelivNum, 0) AS 必要数, " +
                        "UnitPriceCate AS 単価区分, " +
                        "ISNULL(UnitPrice, 0) AS 単価, " +
                        "ProcUnitPrice AS 加工単価, " +
                        "ISNULL(Price, 0) AS 金額, " +
                        "OrdStaffID AS 担当者, " +
                        "StockCate AS 在庫P, " +
                        "NextPartsCode AS 次工程部品コード, " +
                        "NextProcCode AS 次工程コード, " +
                        "PaintColor AS 塗装色, " +
                        "ReqStaffID AS 依頼者, " +
                        "ReqGroupCode AS 依頼部門, " +
                        "ReqCode AS 依頼番号, " +
                        "KeyFlg AS 重点F, " +
                        "Remarks AS 備考, " +
                        "CreateDate AS 作成日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.CreateID GROUP BY StaffName) AS 作成者, " +
                        "UpdateDate AS 変更日付, " +
                        "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF " +
                        "WHERE ID = A.UpdateID GROUP BY StaffName) AS 変更者 " +
                        "FROM SANSODB.dbo.MW_OrdSkdMst AS A " +
                        "WHERE MaintStaffID = '" + maintStaffID + "' " +
                        "AND MaintDate = '" + maintDate + "' " +
                        (partsCode == "" ? "" : ("AND PartsCode = '" + partsCode + "' ")) +
                        (supCode == "" ? "" : ("AND SupCode = '" + supCode + "' "));
            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }

        /// <summary>
        /// MW_OrdSkdMstの部品名を取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        public (bool IsOk, DataTable Table) GetPartsName(string partsCode)
        {
            string sql = "SELECT * " +
                         "FROM SANSODB.dbo.MW_OrdSkdMst AS A " +
                         "WHERE PartsCode = '" + partsCode + "' ";
            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }

        /// <summary>
        /// Ｗ発注日程マスタ更新処理
        /// </summary>
        /// <param name="beforeDt">変更前テーブル</param>
        /// <param name="diffDt">変更前と変更後の差分テーブル</param>
        /// <returns></returns>
        public bool UpdateWOrdSkdMst(DataTable beforeDt, DataTable diffDt)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                // 日付を統一する
                var now = DateTime.Now;

                string sql = "";
                try
                {
                    // 差分テーブルを一行ずつ、列を更新する   
                    foreach (DataRow d in diffDt.Rows)
                    {
                        var instrCode = d["指示番号"].ToString().TrimEnd();
                        var partsCode = d["部品コード"].ToString().TrimEnd();
                        var supCode = d["仕入先コード"].ToString().TrimEnd();
                        var groupCode = d["部門コード"].ToString().TrimEnd();
                        var bizCode = d["業務コード"].ToString().TrimEnd();
                        var createDate = d.Field<DateTime>("作成日付");
                        var delivDate = d.Field<DateTime>("指示日");
                        var delivNum = d.Field<decimal?>("必要数") ?? 0m;
                        var unitPrice = d.Field<decimal?>("単価") ?? 0m;

                        // 列を比較し、値不一致の列のみ更新する   
                        var dRows = beforeDt.AsEnumerable()
                                            .Where(v => v.Field<string>("指示番号") == instrCode 
                                                     && v.Field<string>("部品コード") == partsCode
                                                     && v.Field<string>("仕入先コード") == supCode
                                                     && v.Field<string>("部門コード") == groupCode
                                                     && v.Field<string>("業務コード") == bizCode
                                                     && v.Field<DateTime>("作成日付") == createDate).ToArray();
                        var delivNumBefore = dRows[0].Field<decimal>("必要数");

                        // 金額  四捨五入
                        var price = delivNum * unitPrice;
                        price = (price > 0m) ? (((Int64)(System.Math.Abs(price) + 0.5m)) * 1m) :
                            (((Int64)(System.Math.Abs(price) + 0.5m)) * (-1m));

                        sql = "UPDATE SANSODB.dbo.MW_OrdSkdMst " +
                              "SET DelivDate = '" + delivDate + "', " +
                              ((delivNum == delivNumBefore) ? "" : ("DelivNum = " + delivNum + ", ")) +
                              ((delivNum == delivNumBefore) ? "" : ("Price = " + price + ", ")) +
                              "UpdateID = '" + LoginInfo.Instance.UserId + "', " +
                              "UpdateDate = '" + now + "' " +
                              "WHERE InstrCode = '" + instrCode + "' " +
                              "AND PartsCode = '" + partsCode + "' " +
                              "AND SupCode = '" + supCode + "' " +
                              "AND GroupCode = '" + groupCode + "' " +
                              "AND BizCode = '" + bizCode + "' " +
                              "AND CONVERT(DATETIME, CONVERT(VARCHAR, CreateDate, 120)) = '" + createDate + "' ";
                        var result = CommonAF.ExecutUpdateSQL(sql, tran, con);
                        if (result.IsOk == false)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    try
                    {
                        tran.Dispose();
                        if (con != null)
                        {
                            con.Close();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}