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
    /// 部品調達.dbo.M_TrainingHis（教育履歴マスタ）に関する処理をまとめる
    /// </summary>
    class M_TrainingHisAF
    {
        /// <summary>
        /// 教育履歴マスタ　新規
        /// </summary>
        /// <returns></returns>
        public bool CreateMTrainingHis(List<ControlParam> controlList, DataTable workerDt, string cateCode)
        {
            DateTime date = DateTime.Now;

            string sql = "";

            foreach (DataRow dr in workerDt.Rows)
            {
                sql = "INSERT " +
                      "INTO " +
                      "M_TrainingHis( " +
                      "WorkerID, " +
                      "GroupCode, " +
                      "Line, " +
                      "Trainer, " +
                      "TrainingDate, " +
                      "TrainingCate, " +
                      "Understanding, " +
                      "TrainingContents, " +
                      "CreateDate, " +
                      "CreateID) " +
                      "VALUES (" +
                     $"'{ dr.Field<string>("WorkerID") }'," +
                     $"'{ controlList.SGetText("groupCodeC1ComboBox") }', " +
                     $"'{ controlList.SGetText("lineC1ComboBox") }', " +
                     $"'{ controlList.SGetText("trainerC1ComboBox") }', " +
                     $"'{ controlList.SGetText("trainingDateC1DateEdit") }', " +
                     $"'{ cateCode }', " +
                     $"'{ controlList.SGetText("understandingC1ComboBox") }', " +
                     $"'{ controlList.SGetText("trainingContentC1ComboBox") }', " +
                     $"'{ date }', " +
                     $"'{ LoginInfo.Instance.UserId }') ";

                var af = CommonAF.ExecutInsertSQL(sql);
                if (af.IsOk == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 教育履歴マスタ　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetMTrainingHis(List<ControlParam> controlList, string trainingCate, int narrowDownFlg)
        {
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            if (groupCode == "_") { groupCode = ""; }

            var line = controlList.SGetText("lineC1ComboBox");
            if (line == "_") { line = ""; }

            var staffCode = controlList.SGetText("staffCodeC1ComboBox");
            if (staffCode == "_") { staffCode = ""; }

            if (trainingCate == "99") { trainingCate = ""; }

            var trainingContent = controlList.SGetText("trainingContentC1ComboBox");
            if (trainingContent == "すべて") { trainingContent = ""; }

            var understanding = controlList.SGetText("understandingC1ComboBox");
            if (understanding == "_") { understanding = ""; }

            string sql = "SELECT ";

            if (narrowDownFlg == 1)
            {
                sql += "TOP(100) ";
            }

            sql += "C.TrainingDate AS 教育日, " +
                  "C.GroupCode AS 組立部門, " +
                  "E.係名 AS 組立部門名, " +
                  "C.Line AS 組立ライン, " +
                  "D.LineName AS 組立ライン名, " +
                  "C.WorkerID AS 作業者ID, " +
                  "F.WorkerName AS 作業者, " +
                  "A.教育区分, " +
                  "C.TrainingContents AS 教育内容, " +
                  "CASE C.Understanding WHEN 'A' THEN C.Understanding + '：よく理解している' " +
                  "WHEN 'B' THEN C.Understanding + '：理解している'  WHEN 'C' THEN C.Understanding + '：一部理解していない' " +
                  "WHEN 'D' THEN C.Understanding + '：再教育が必要' ELSE '' END AS 理解度, " +
                  "G.StaffName AS 教育者, " +
                  "C.CreateDate AS 作成日付, " +
                  "B.StaffName AS 作成者 " +
                  "FROM " +
                  "M_TrainingHis AS C " +
                  "LEFT OUTER JOIN ( " +
                  "SELECT " +
                  "ID, " +
                  "StaffName " +
                  "FROM " +
                  "三相メイン.dbo.M_STAFF " +
                  "WHERE " +
                  "(CurrentGroupFLG = 1) " +
                  ") AS B " +
                  "ON C.CreateID = B.ID " +
                  "LEFT OUTER JOIN ( " +
                  "SELECT " +
                  "CateCode AS 教育区分コード, " +
                  "CateName AS 教育区分 " +
                  "FROM " +
                  "SANSODB.dbo.M_CateName " +
                  "WHERE " +
                  "(GroupCode = '0000') AND " +
                  "(DivisionCode = 18) " +
                  ") AS A " +
                  "ON C.TrainingCate = A.教育区分コード " +
                  "LEFT OUTER JOIN 三相メイン.dbo.AssemblyLineMst AS D " +
                  "ON C.Line = D.LineCode " +
                  "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS E " +
                  "ON C.GroupCode = E.部門コード " +
                  "LEFT OUTER JOIN M_Worker AS F " +
                  "ON C.WorkerID = F.WorkerID " +
                  "LEFT OUTER JOIN " +
                  "三相メイン.dbo.M_STAFF AS G " +
                  "ON C.Trainer = G.StaffCode ";

            sql += $"WHERE (C.TrainingDate BETWEEN '{ controlList.SGetText("startDateC1DateEdit")}' " +
                   $"AND '{ controlList.SGetText("endDateC1DateEdit") }') ";

            sql += string.IsNullOrEmpty(groupCode) ? "" : $"AND (C.GroupCode = '{ groupCode }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (C.line = '{ line }') ";
            sql += string.IsNullOrEmpty(staffCode) ? "" : $"AND (C.WorkerID = '{ staffCode }') ";
            sql += string.IsNullOrEmpty(trainingCate) ? "" : $"AND (A.教育区分コード = '{ trainingCate }') ";
            sql += string.IsNullOrEmpty(trainingContent) ? "" : $"AND (C.TrainingContents = '{ trainingContent }') ";
            sql += string.IsNullOrEmpty(understanding) ? "" : $"AND (C.Understanding = '{ understanding }') ";

            sql += "ORDER BY C.TrainingDate DESC, " +
                   "C.GroupCode, C.Line, C.WorkerID";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

        /// <summary>
        /// 教育履歴マスタ　取得
        /// </summary>
        /// <param name="groupCode">組立部門</param>
        /// <param name="line">組立ライン</param>
        public (bool IsOk, DataTable Table) GetMTrainingHis(string groupCode, string line)
        {
            string sql = "SELECT " +
                         "C.WorkerID AS 作業者ID, " +
                         "F.WorkerName AS 作業者, " +
                         "C.GroupCode AS 組立部門, " +
                         "E.係名 AS 組立部門名, " +
                         "C.Line AS 組立ライン, " +
                         "D.LineName AS 組立ライン名, " +
                         "C.TrainingDate AS 教育日, " +
                         "A.教育区分, " +
                         "C.TrainingContents AS 教育内容, " +
                  　　　"CASE C.Understanding WHEN 'A' THEN C.Understanding + '：よく理解している' " +
                  　　　"WHEN 'B' THEN C.Understanding + '：理解している'  " +
                     　 "WHEN 'C' THEN C.Understanding + '：一部理解していない' " +
                  　　　"WHEN 'D' THEN C.Understanding + '：再教育が必要' ELSE '' END AS 理解度, " +
                         "G.StaffName AS 教育者, " +
                         "C.CreateDate AS 作成日付, " +
                         "B.StaffName AS 作成者, " +
                         "C.TrainingCate AS 教育区分コード " +
                         "FROM " +
                         "M_TrainingHis AS C " +
                         "LEFT OUTER JOIN ( " +
                         "SELECT " +
                         "ID, " +
                         "StaffName " +
                         "FROM " +
                         "三相メイン.dbo.M_STAFF " +
                         "WHERE " +
                         "(CurrentGroupFLG = 1) " +
                         ") AS B " +
                         "ON C.CreateID = B.ID " +
                         "LEFT OUTER JOIN ( " +
                         "SELECT " +
                         "CateCode AS 教育区分コード, " +
                         "CateName AS 教育区分 " +
                         "FROM " +
                         "SANSODB.dbo.M_CateName " +
                         "WHERE " +
                         "(GroupCode = '0000') AND " +
                         "(DivisionCode = 18) " +
                         ") AS A " +
                         "ON C.TrainingCate = A.教育区分コード " +
                         "LEFT OUTER JOIN 三相メイン.dbo.AssemblyLineMst AS D " +
                         "ON C.Line = D.LineCode " +
                         "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS E " +
                         "ON C.GroupCode = E.部門コード " +
                         "LEFT OUTER JOIN M_Worker AS F " +
                         "ON C.WorkerID = F.WorkerID " +
                         "LEFT OUTER JOIN " +
                         "三相メイン.dbo.M_STAFF AS G " +
                         "ON C.Trainer = G.StaffCode ";

            sql += $"WHERE (C.GroupCode = '{ groupCode }')" +
                   $"AND (C.line = '{ line }') ";

            sql += "ORDER BY C.TrainingDate DESC";


            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 教育履歴マスタ　削除
        /// </summary>
        /// <param name="list">WorkerID, GroupCode, Line, TrainingDate, CreateDate</param>
        /// <returns>True:削除成功　False:削除失敗</returns>
        public bool DleateMTrainingHis(List<string> list)
        {
            string sql = "DELETE FROM " +
                       "M_TrainingHis " +
                       "WHERE " +
                        $"(WorkerID = '{ list[0] }') " +
                        $"AND (GroupCode = '{ list[1] }') " +
                        $"AND (Line = '{ list[2] }') " +
                        $"AND (TrainingDate = '{ list[3] }') " +
                        $"AND (CreateDate = '{ list[4] }') ";

            var af = CommonAF.ExecutDeleteSQL(sql);
            return af.IsOk;
        }

        /// <summary>
        /// 教育履歴マスタ　取得
        /// </summary>
        /// <param name="staffCode">作業者コード</param>
        /// <param name="groupCode">組立部門</param>
        /// <param name="line">組立ライン</param>
        /// <remarks>作業者、組立部門、組立ライン、理解度がB以上の履歴を取得する</remarks>
        public (bool IsOk, DataTable Table) GetMTrainingHis(string groupCode, string line, string staffCode)
        {
            string sql = "SELECT " +
                         "WorkerID, " +
                         "GroupCode, " +
                         "Line, " +
                         "Trainer, " +
                         "TrainingDate, " +
                         "TrainingCate, " +
                         "TrainingContents, " +
                         "Understanding, " +
                         "Remarks, " +
                         "CreateDate, " +
                         "CreateID, " +
                         "UpdateDate, " +
                         "UpdateID " +
                         "FROM " +
                         "M_TrainingHis " +
                         "WHERE " +
                        $"(WorkerID = '{ staffCode }') AND " +
                        $"(GroupCode = '{ groupCode }') AND " +
                        $"(Line = '{ line }') AND " +
                         "(Understanding IN ('A', 'B')) " +
                         "ORDER BY " +
                         "TrainingDate DESC";

            var af = CommonAF.ExecutSelectSQL(sql);
            return (af.IsOk, af.Table);

        }
    }
}
