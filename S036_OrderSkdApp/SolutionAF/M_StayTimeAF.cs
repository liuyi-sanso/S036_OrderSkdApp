using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 部品調達.dbo.M_StayTime（在場時間マスタ）に関する処理をまとめる
    /// </summary>
    public class M_StayTimeAF
    {
        /// <summary>
        /// 在場時間マスタ　新規更新
        /// </summary>
        public bool UpdateMStayTime(List<ControlParam> controlList)
        {
            DateTime now = DateTime.Now;

            string sql = "MERGE INTO 部品調達.dbo.M_StayTime AS A " +
                         "USING (" +
                         "SELECT " +
                         "CASE WHEN AutoNoCount = 0 THEN 0 ELSE MinAutoNo END AS AutoNo " +
                         "FROM " +
                         "(SELECT " +
                         "COUNT(AutoNo) AS AutoNoCount, " +
                         "MIN(AutoNo) AS MinAutoNo " +
                         "FROM " +
                         "部品調達.dbo.M_StayTime " +
                         "WHERE " +
                         "(ISNULL(EndTime, CONVERT(DATETIME, '2000-01-01 00:00:00', 102)) " +
                         "= CONVERT(DATETIME, '2000-01-01 00:00:00', 102)) AND " +
                         "(GroupCode = '" + controlList.SGetText("groupCodeC1ComboBox") + "') AND " +
                         "(Line = '" + controlList.SGetText("lineC1ComboBox") + "') AND " +
                         "(StaffCode = '" + controlList.SGetText("staffCode1C1ComboBox") + "') AND " +
                         "(CONVERT(NVARCHAR, CreateDate, 111) = CONVERT(NVARCHAR, GETDATE(), 111)) " +
                         ") AS C " +
                         ") AS B " +
                         "on (" +
                         "A.AutoNo = B.AutoNo " +
                         ") " +
                         "WHEN MATCHED " +
                         "THEN " +
                         "UPDATE SET " +
                         "EndTime = '" + now + "'," +
                         "UpdateID = '" + LoginInfo.Instance.UserId + "'," +
                         "UpdateDate = '" + now + "' " +
                         "WHEN NOT MATCHED " +
                         "THEN " +
                         "INSERT " +
                         "(GroupCode,Line,StaffCode,StartTime, CreateDate, CreateID) " +
                         "VALUES " +
                         "('" + controlList.SGetText("groupCodeC1ComboBox") + "'," +
                         "'" + controlList.SGetText("lineC1ComboBox") + "'," +
                         "'" + controlList.SGetText("staffCode1C1ComboBox") + "'," +
                         "'" + now + "'," +
                         "'" + now + "'," +
                         "'" + LoginInfo.Instance.UserId + "'); ";

            var af = CommonAF.ExecutUpdateSQL(sql);

            return af.IsOk;
        }

        /// <summary>
        /// 作業基準マスタ　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetMStayTimeToday(List<ControlParam> controlList)
        {
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            if (groupCode == "_") { groupCode = ""; }

            var line = controlList.SGetText("lineC1ComboBox");
            if (line == "_") { line = ""; }

            string sql = "SELECT " +
　　　                 　"M_StayTime.AutoNo, " +
　　　                 　"M_StayTime.GroupCode, " +
　　　                 　"M_StayTime.Line, " +
　　　                 　"M_StayTime.StaffCode, " +
　　　                 　"M_Worker.WorkerName AS StaffName, " +
　　　                 　"M_StayTime.StartTime, " +
　　　                 　"M_StayTime.EndTime " +
　　　                 　"FROM " +
　　　                 　"M_StayTime " +
　　　                 　"LEFT OUTER JOIN M_Worker " +
　　　                 　"ON M_StayTime.StaffCode = M_Worker.WorkerID " +
　　　                 　"WHERE " +
　　　                 　"(CONVERT(NVARCHAR, M_StayTime.CreateDate, 111) " +
　　　                 　"= CONVERT(NVARCHAR, GETDATE(), 111)) ";

            sql += string.IsNullOrEmpty(groupCode) ? "" : $"AND (M_StayTime.GroupCode = '{ groupCode }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (M_StayTime.Line = '{ line }') ";

            sql += "ORDER BY M_StayTime.CreateDate";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

        /// <summary>
        /// 作業基準マスタ　Excel出力データ　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetMStayTimeExcelOutput(List<ControlParam> controlList)
        {
            var groupCode = controlList.SGetText("searchGroupCodeC1ComboBox");
            if (groupCode == "_") { groupCode = ""; }

            var line = controlList.SGetText("searchLineC1ComboBox");
            if (line == "_") { line = ""; }

            var staffCode = controlList.SGetText("searchStaffCode1C1ComboBox");
            if (staffCode == "_") { staffCode = ""; }

            string sql = "SELECT " +
                         "A.GroupCode AS 組立部門, " +
                         "C.部門名, " +
                         "A.Line AS 組立ライン, " +
                         "D.LineName AS 組立ライン名, " +
                         "A.StaffCode AS 作業者コード, " +
                         "B.WorkerName AS 作業者名, " +
                         "A.StartTime AS 開始時間, " +
                         "A.EndTime AS 終了時間 " +
                         "FROM " +
                         "M_StayTime AS A " +
                         "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS C " +
                         "ON A.GroupCode = C.部門コード " +
                         "LEFT OUTER JOIN ( " +
                         "SELECT " +
                         "LineCode, " +
                         "LineName " +
                         "FROM " +
                         "三相メイン.dbo.AssemblyLineMst " +
                         "WHERE " +
                         "(EnableCate = 1) " +
                         ") AS D " +
                         "ON A.Line = D.LineCode " +
                         "LEFT OUTER JOIN M_Worker AS B " +
                         "ON A.StaffCode = B.WorkerID " +
                         "WHERE " +
                         "(CONVERT(NVARCHAR, A.StartTime, 111) BETWEEN " +
                         "'" + controlList.SGetText("startDateC1DateEdit") + "' AND " +
                         "'" + controlList.SGetText("endDateC1DateEdit") + "') ";

            sql += string.IsNullOrEmpty(groupCode) ? "" : $"AND (A.GroupCode = '{ groupCode }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (A.Line = '{ line }') ";
            sql += string.IsNullOrEmpty(staffCode) ? "" : $"AND (A.StaffCode = '{ staffCode }') ";

            sql += "ORDER BY A.StartTime, A.GroupCode, A.Line, A.StaffCode";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }

    }
}
