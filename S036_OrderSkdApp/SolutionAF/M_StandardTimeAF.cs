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
    /// 部品調達.dbo.M_StandardTime（標準時間マスタ）に関する処理をまとめる
    /// </summary>
     public class M_StandardTimeAF
    {
        /// <summary>
        /// 標準時間マスタ　新規更新
        /// </summary>
        public bool UpdateMStandardTime(List<ControlParam> controlList, int secTime, int firstSecTime)
        {
            string sql = "MERGE INTO 部品調達.dbo.M_StandardTime AS A " +
                         "USING (SELECT " +
                         "'" + controlList.SGetText("productCodeC1TextBox") + "' AS ProductCode " +
                         " ) AS B " +
                         "on (" +
                         "A.ProdCode = B.ProductCode " +
                         ") " +
                         "WHEN MATCHED " +
                         "THEN " +
                         "UPDATE SET " +
                         "GroupCode = '" + controlList.SGetText("groupCodeC1ComboBox") + "'," +
                         "Line = '" + controlList.SGetText("lineC1ComboBox") + "'," +
                         "Num = " + controlList.SGetText("numC1ComboBox") + "," +
                         "StaffCode1 = '" + controlList.SGetText("staffCode1C1ComboBox") + "'," +
                         "StaffCode2 = '" + controlList.SGetText("staffCode2C1ComboBox") + "'," +
                         "StaffCode3 = '" + controlList.SGetText("staffCode3C1ComboBox") + "'," +
                         "StaffCode4 = '" + controlList.SGetText("staffCode4C1ComboBox") + "'," +
                         "StaffCode5 = '" + controlList.SGetText("staffCode5C1ComboBox") + "'," +
                         "StaffCode6 = '" + controlList.SGetText("staffCode6C1ComboBox") + "'," +
                         "StaffCode7 = '" + controlList.SGetText("staffCode7C1ComboBox") + "'," +
                         "StaffCode8 = '" + controlList.SGetText("staffCode8C1ComboBox") + "'," +
                         "StaffCode9 = '" + controlList.SGetText("staffCode9C1ComboBox") + "'," +
                         "StaffCode10 = '" + controlList.SGetText("staffCode10C1ComboBox") + "'," +
                         "ResponsiblePerson = '" + controlList.SGetText("responsiblePersonC1ComboBox") + "'," +
                         "Certifier = '" + controlList.SGetText("certifierC1ComboBox") + "'," +
                         "Time = " + secTime + "," +
                         "FirstTime = " + firstSecTime + "," +
                         "Remarks = '" + controlList.SGetText("remarksC1TextBox") + "'," +
                         "UpdateID = '" + LoginInfo.Instance.UserId + "'," +
                         "UpdateDate = '" + DateTime.Now + "'" +
                         "WHEN NOT MATCHED " +
                         "THEN " +
                         "INSERT(ProdCode,GroupCode,Line,Num,StaffCode1,StaffCode2,StaffCode3,StaffCode4,StaffCode5," +
                         "StaffCode6,StaffCode7,StaffCode8,StaffCode9,StaffCode10,ResponsiblePerson,Certifier,Time, FirstTime," +
                         "Remarks,CreateDate, CreateID)" +
                         "VALUES " +
                         "('" + controlList.SGetText("productCodeC1TextBox") + "'," +
                         "'" + controlList.SGetText("groupCodeC1ComboBox") + "'," +
                         "'" + controlList.SGetText("lineC1ComboBox") + "'," +
                         "" + controlList.SGetText("numC1ComboBox") + "," +
                         "'" + controlList.SGetText("staffCode1C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode2C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode3C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode4C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode5C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode6C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode7C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode8C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode9C1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode10C1ComboBox") + "'," +
                         "'" + controlList.SGetText("responsiblePersonC1ComboBox") + "'," +
                         "'" + controlList.SGetText("certifierC1ComboBox") + "'," +
                         "" + secTime + "," +
                         "" + firstSecTime + "," +
                         "'" + controlList.SGetText("remarksC1TextBox") + "'," +
                         "'" + DateTime.Now + "'," +
                         "'" + LoginInfo.Instance.UserId + "'); ";

            var af = CommonAF.ExecutUpdateSQL(sql);

            return af.IsOk;
        }

        /// <summary>
        /// 作業基準マスタ　取得
        /// </summary>
        /// <param name="prodCode">機種コード</param>
        public (bool IsOk, DataTable Table) GetMStandardTime(string prodCode)
        {
            string sql = "SELECT " +
                         "*" +
                         "FROM " +
                         "M_StandardTime " +
                         "WHERE " +
                        $"(ProdCode = '{ prodCode }') ";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

        /// <summary>
        /// 作業基準マスタ履歴　取得
        /// </summary>
        /// <param name="prodCode">機種コード</param>
        public (bool IsOk, DataTable Table) GetMHisStandardTime(string prodCode)
        {
            string sql = "SELECT " +
                         "A.*, " +
                         "B.WorkerName AS StaffName " +
                         "FROM " +
                         "M_His_StandardTime AS A " +
                         "LEFT OUTER JOIN M_Worker AS B " +
                         "ON A.StaffCode1 = B.WorkerID " +
                         "WHERE " +
                        $"(A.ProdCode = '{ prodCode }') " +
                        $"ORDER BY A.UpdateDate DESC";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

        /// <summary>
        /// 作業基準マスタ　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetMStandardTime(List<ControlParam> controlList)
        {
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            if (groupCode == "_") { groupCode = ""; }

            var line = controlList.SGetText("lineC1ComboBox");
            if (line == "_") { line = ""; }

            string sql = "SELECT " +
                         "A.GroupCode AS 組立部門, " +
                         "B.係名 AS 組立部門名, " +
                         "A.Line AS 組立ライン, " +
                         "C.LineName AS 組立ライン名, " +
                         "A.ProdCode AS 機種コード, " +
                         "三相メイン.dbo.機種マスタ.機種名, " +
                         "A.TIME, " +
                         "A.Num AS 人数, " +
                         "S1.WorkerName AS 作業者1, " +
                         "S2.WorkerName AS 作業者2, " +
                         "S3.WorkerName AS 作業者3, " +
                         "S4.WorkerName AS 作業者4, " +
                         "S5.WorkerName AS 作業者5, " +
                         "S6.WorkerName AS 作業者6, " +
                         "S7.WorkerName AS 作業者7, " +
                         "S8.WorkerName AS 作業者8, " +
                         "S9.WorkerName AS 作業者9, " +
                         "S10.WorkerName AS 作業者10, " +
                         "作成者.StaffName AS 作成者, " +
                         "A.CreateDate AS 作成日付, " +
                         "更新者.StaffName AS 更新者, " +
                         "A.UpdateDate AS 更新日付 " +
                         "FROM " +
                         "M_StandardTime AS A " +
                         "LEFT OUTER JOIN ( " +
                         "SELECT " +
                         "StaffName, " +
                         "ID, " +
                         "CurrentGroupFLG " +
                         "FROM " +
                         "三相メイン.dbo.M_STAFF " +
                         "WHERE " +
                         "(CurrentGroupFLG = 1) " +
                         ") AS 更新者 " +
                         "ON A.UpdateID = 更新者.ID " +
                         "LEFT OUTER JOIN ( " +
                         "SELECT " +
                         "StaffName, " +
                         "ID, " +
                         "CurrentGroupFLG " +
                         "FROM " +
                         "三相メイン.dbo.M_STAFF AS M_STAFF_1 " +
                         "WHERE " +
                         "(CurrentGroupFLG = 1) " +
                         ") AS 作成者 " +
                         "ON A.CreateID = 作成者.ID " +
                         "LEFT OUTER JOIN M_Worker AS S4 " +
                         "ON A.StaffCode4 = S4.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S10 " +
                         "ON A.StaffCode10 = S10.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S9 " +
                         "ON A.StaffCode9 = S9.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S8 " +
                         "ON A.StaffCode8 = S8.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S7 " +
                         "ON A.StaffCode7 = S7.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S6 " +
                         "ON A.StaffCode6 = S6.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S3 " +
                         "ON A.StaffCode3 = S3.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S2 " +
                         "ON A.StaffCode2 = S2.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S5 " +
                         "ON A.StaffCode5 = S5.WorkerID " +
                         "LEFT OUTER JOIN M_Worker AS S1 " +
                         "ON A.StaffCode1 = S1.WorkerID " +
                         "LEFT OUTER JOIN 三相メイン.dbo.機種マスタ " +
                         "ON A.ProdCode = 三相メイン.dbo.機種マスタ.機種コード " +
                         "LEFT OUTER JOIN 三相メイン.dbo.AssemblyLineMst AS C " +
                         "ON A.Line = C.LineCode " +
                         "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS B " +
                         "ON A.GroupCode = B.部門コード ";

            sql += $"WHERE (0 = 0) ";

            sql += string.IsNullOrEmpty(groupCode) ? "" : $"AND (A.GroupCode = '{ groupCode }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (A.line = '{ line }') ";
            sql += string.IsNullOrEmpty(controlList.SGetText("productCodeC1TextBox")) ? "" 
                                        : $"AND (A.ProdCode = '{ controlList.SGetText("productCodeC1TextBox") }') ";

            sql += "ORDER BY A.GroupCode, A.Line, A.ProdCode";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

        /// <summary>
        /// 標準時間履歴　削除
        /// </summary>
        /// <param name="autoNo">オートNo</param>
        /// <returns>True:削除成功　False:削除失敗</returns>
        public bool DeleteMHisStandardTime(string autoNo, string prodCode, int rowIndex)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string sql = "";

                    sql = "DELETE FROM " +
                          "M_His_StandardTime " +
                          "WHERE " +
                         $"(AutoNo = '{ autoNo }') ";

                    var af = CommonAF.ExecutDeleteSQL(sql, tran, con);

                    if (af.IsOk == false)
                    {
                        tran.Rollback();
                        return af.IsOk;
                    }

                    // rowIndex = 0 = 最新の標準時間の場合、その1つ前の標準時間を新しい標準時間にする
                    if (rowIndex != 0)
                    {
                        tran.Commit();
                        return af.IsOk;
                    }

                    // その1つ前の標準時間にするときは履歴不要のため、トリガーを止める
                    sql = "ALTER TABLE M_StandardTime DISABLE TRIGGER ALL;";

                    af = CommonAF.ExecutUpdateSQL(sql, tran, con);
                    if (af.IsOk == false)
                    {
                        tran.Rollback();
                        return af.IsOk;
                    }

                    sql = "UPDATE M_StandardTime " +
                        "SET " +
                        "ProdCode = B.ProdCode, " +
                        "GroupCode = B.GroupCode, " +
                        "Line = B.Line, " +
                        "Num = B.Num, " +
                        "StaffCode1 = B.StaffCode1, " +
                        "StaffCode2 = B.StaffCode2, " +
                        "StaffCode3 = B.StaffCode3, " +
                        "StaffCode4 = B.StaffCode4, " +
                        "StaffCode5 = B.StaffCode5, " +
                        "StaffCode6 = B.StaffCode6, " +
                        "StaffCode7 = B.StaffCode7, " +
                        "StaffCode8 = B.StaffCode8, " +
                        "StaffCode9 = B.StaffCode9, " +
                        "StaffCode10 = B.StaffCode10, " +
                        "ResponsiblePerson = B.ResponsiblePerson, " +
                        "Certifier = B.Certifier, " +
                        "TIME = B.TIME, " +
                        "FirstTime = B.FirstTime, " +
                        "Remarks = B.Remarks, " +
                        "CreateDate = B.CreateDate, " +
                        "CreateID = B.CreateID, " +
                        $"UpdateDate = '{ DateTime.Now }', " +
                        $"UpdateID = '{ LoginInfo.Instance.UserId }' " +
                        "FROM " +
                        "M_StandardTime " +
                        "INNER JOIN ( " +
                        "SELECT " +
                        "TOP(1) AutoNo, " +
                        "Status, " +
                        "ProdCode, " +
                        "GroupCode, " +
                        "Line, " +
                        "Num, " +
                        "StaffCode1, " +
                        "StaffCode2, " +
                        "StaffCode3, " +
                        "StaffCode4, " +
                        "StaffCode5, " +
                        "StaffCode6, " +
                        "StaffCode7, " +
                        "StaffCode8, " +
                        "StaffCode9, " +
                        "StaffCode10, " +
                        "ResponsiblePerson, " +
                        "Certifier, " +
                        "TIME, " +
                        "FirstTime, " +
                        "Remarks, " +
                        "CreateDate, " +
                        "CreateID, " +
                        "UpdateDate, " +
                        "UpdateID " +
                        "FROM " +
                        "M_His_StandardTime " +
                        "WHERE " +
                        $"(ProdCode = '{ prodCode }') " +
                        "ORDER BY " +
                        "UpdateDate DESC " +
                        ") AS B " +
                        "ON M_StandardTime.ProdCode = B.ProdCode " +
                        "WHERE " +
                        $"(M_StandardTime.ProdCode = '{ prodCode }') ";

                    af = CommonAF.ExecutUpdateSQL(sql, tran, con);
                    if (af.IsOk == false)
                    {
                        tran.Rollback();
                        return af.IsOk;
                    }

                    // トリガーを有効化
                    sql = "ALTER TABLE M_StandardTime ENABLE TRIGGER ALL;";

                    af = CommonAF.ExecutUpdateSQL(sql, tran, con);
                    if (af.IsOk == false)
                    {
                        tran.Rollback();
                    }
                    else
                    {
                        tran.Commit();
                    }

                    return af.IsOk;
                }
                catch
                {
                    tran.Rollback();
                    return false;
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

        /// <summary>
        /// 標準時間マスタ　コピー新規
        /// </summary>
        /// <returns></returns>
        public bool CopyCreateMStandardTimeDate(List<ControlParam> controlList)
        {
            string sql = "";
            DateTime now = DateTime.Now;

            // 標準時間マスタ
            sql = "INSERT " +
                  "INTO M_StandardTime " +
                  "(ProdCode, " +
                  "GroupCode, " +
                  "Line, " +
                  "Num,ResponsiblePerson,Certifier,Time, FirstTime, " +
                  "Remarks," +
                  "CreateDate, " +
                  "CreateID)" +
                  "SELECT " +
                  "'" + controlList.SGetText("newProductCodeC1TextBox") + "'," +
                  "'" + controlList.SGetText("newGroupCodeC1ComboBox") + "'," +
                  "'" + controlList.SGetText("newLineC1ComboBox") + "'," +
                  "Num, ResponsiblePerson, Certifier, Time, FirstTime, " +
                 "'" + controlList.SGetText("checkProductCodeC1TextBox") + "よりコピー作成'," +
                  "'" + now + "'," +
                  "'" + LoginInfo.Instance.UserId + "' " +
                  "FROM M_StandardTime AS A " +
                  "WHERE " +
                 " (ProdCode = '" + controlList.SGetText("checkProductCodeC1TextBox") + "')";

            var af = CommonAF.ExecutInsertSQL(sql);

            return af.IsOk;
        }
    }
}
