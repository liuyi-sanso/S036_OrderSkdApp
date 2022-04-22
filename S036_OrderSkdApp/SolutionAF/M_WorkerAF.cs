using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using C1.Win.C1Input;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 部品調達.dbo.M_Work（作業者マスタ）に関する処理をまとめる
    /// </summary>
    public class M_WorkerAF
    {
        /// <summary>
        /// 作業者マスタ　抽出　（workerId）
        /// </summary>
        /// <param name="workerId">作業者ID</param>
        /// <returns></returns>
        public (bool IsOk, DataTable Table) GetWorkerMst(string workerId)
        {
            string sql = "SELECT " +
                "A.WorkerID,A.WorkerName,A.WorkerLastName,A.WorkerFirstName,A.WorkerNameKana,A.PositionCate," +
                "B.CateName AS PositionCateName, A.GroupCode,C.部門名 AS GroupCodeName,A.Remarks," +
                "CASE A.EnableCate WHEN '1' THEN '使用可能' WHEN '0' THEN '使用不可' ELSE '使用不可' END AS 使用可否, " +
                "CASE A.ResponsiblePersonCate WHEN 1 THEN '可' ELSE '不可' END AS ResponsiblePersonCate, " +
                "A.CreateDate,A.CreateID,A.UpdateDate,A.UpdateID," +
                "D.StaffName AS 作成者,CONVERT(VARCHAR,A.CreateDate,120) AS 作成日付, " +
                "E.StaffName AS 変更者,CONVERT(VARCHAR,A.UpdateDate,120) AS 変更日付 " +
                "FROM 部品調達.dbo.M_Worker AS A " +
                "left join SANSODB.dbo.M_CateName AS B on A.PositionCate = B.CateCode and " +
                "B.GroupCode = '0000' and B.DivisionCode = '17' " +
                "left join 三相メイン.dbo.部門マスタ AS C on A.GroupCode = C.部門コード " +
                "left join 三相メイン.dbo.M_STAFF AS D on A.CreateID = D.ID and D.CurrentGroupFLG = '1' " +
                "left join 三相メイン.dbo.M_STAFF AS E on A.UpdateID = E.ID and E.CurrentGroupFLG = '1' " +
                "where " +
                "WorkerID = '" + workerId+ "'";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 作業者マスタ　新規更新
        /// </summary>
        /// <returns></returns>
        public bool UpdateMWorker(List<ControlParam> controlList)
        {
            // 使用可否区分取得
            var c1 = controlList.Where(v => v.Control.Name == "enableCateC1ComboBox").ToList()[0];
            string enableCate = ((C1ComboBox)c1.Control).SGetText(1);

            // 責任者区分取得
            c1 = controlList.Where(v => v.Control.Name == "responsiblePersonCateC1ComboBox").ToList()[0];
            string responsiblePersonCate = ((C1ComboBox)c1.Control).SGetText(1);


            string sql =
                "MERGE INTO 部品調達.dbo.M_Worker AS A " +
                "USING (SELECT " +
                "'" + controlList.SGetText("workerIDC1TextBox") + "' AS WorkerID " +
                " ) AS B " +
                "on (" +
                "A.WorkerID = B.WorkerID " +
                ") " +
                "WHEN MATCHED " +
                "THEN " +
                "UPDATE SET " +
                "WorkerName = '" + controlList.SGetText("workerNameC1TextBox") + "'," +
                "WorkerLastName = '" + controlList.SGetText("workerLastNameC1TextBox") + "'," +
                "WorkerFirstName = '" + controlList.SGetText("workerFirstNameC1TextBox") + "'," +
                "WorkerNameKana = '" + controlList.SGetText("workerNameKanaC1TextBox") + "'," +
                "PositionCate = " + (controlList.SGetText("positionCateC1ComboBox") == "" ?
                                                                "null" : controlList.SGetText("positionCateC1ComboBox")) + "," +
                "GroupCode = '" + controlList.SGetText("groupCodeC1ComboBox") + "'," +
                "EnableCate = '" + enableCate + "'," +
                "ResponsiblePersonCate = " + responsiblePersonCate + "," +
                "Remarks = '" + controlList.SGetText("remarksC1TextBox") + "'," +
                "UpdateID = '" + LoginInfo.Instance.UserId + "'," +
                "UpdateDate = '" + DateTime.Now + "'" +
                "WHEN NOT MATCHED " +
                "THEN " +
                "INSERT(WorkerID,WorkerName,WorkerLastName,WorkerFirstName,WorkerNameKana," +
                "PositionCate,GroupCode,EnableCate,ResponsiblePersonCate,Remarks,CreateDate,CreateID)" +
                "VALUES " +
                "(" +
                "'" + controlList.SGetText("workerIDC1TextBox") + "'," +
                "'" + controlList.SGetText("workerNameC1TextBox") + "'," +
                "'" + controlList.SGetText("workerLastNameC1TextBox") + "'," +
                "'" + controlList.SGetText("workerFirstNameC1TextBox") + "'," +
                "'" + controlList.SGetText("workerNameKanaC1TextBox") + "'," +
                "" + (controlList.SGetText("positionCateC1ComboBox") == "" ?
                                                    "null" : controlList.SGetText("positionCateC1ComboBox")) + "," +
                "'" + controlList.SGetText("groupCodeC1ComboBox") + "'," +
                "'" + enableCate + "'," +
                "" + responsiblePersonCate + "," +
                "'" + controlList.SGetText("remarksC1TextBox") + "'," +
                "'" + DateTime.Now + "'," +
                "'" + LoginInfo.Instance.UserId + "'); ";
            var af = CommonAF.ExecutUpdateSQL(sql);
            if (af.IsOk == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 作業者マスタ削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteMWorker(string workerID)
        {
            string sql = "delete from 部品調達.dbo.M_Worker " +
                "where " +
                "WorkerID = '" + workerID + "'";
            var af = CommonAF.ExecutDeleteSQL(sql);
            if (af.IsOk == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 作業者マスタ全件
        /// </summary>
        /// <returns></returns>
        public (bool IsOk, DataTable Table) GetMWorkerAll()
        {
            string sql = "SELECT " +
                "A.WorkerID AS 職番,A.WorkerName AS 社員名," +
                "A.WorkerNameKana AS 社員名カナ," +
                "B.CateName AS 役職区分名, A.GroupCode AS 部門コード,C.部門名 AS 部門名,A.Remarks AS 備考," +
                "CASE A.EnableCate WHEN '1' THEN '使用可能' WHEN '0' THEN '使用不可' ELSE '使用不可' END AS 使用可否, " +
                "CASE A.ResponsiblePersonCate WHEN 1 THEN '可' ELSE '不可' END AS 責任者区分, " +
                "D.StaffName AS 作成者,CONVERT(VARCHAR,A.CreateDate,120) AS 作成日付, " +
                "E.StaffName AS 変更者,CONVERT(VARCHAR,A.UpdateDate,120) AS 変更日付 " +
                "FROM 部品調達.dbo.M_Worker AS A " +
                "left join SANSODB.dbo.M_CateName AS B on A.PositionCate = B.CateCode " +
                "and B.GroupCode = '0000' and B.DivisionCode = '17' " +
                "left join 三相メイン.dbo.部門マスタ AS C on A.GroupCode = C.部門コード " +
                "left join 三相メイン.dbo.M_STAFF AS D on A.CreateID = D.ID and D.CurrentGroupFLG = '1' " +
                "left join 三相メイン.dbo.M_STAFF AS E on A.UpdateID = E.ID and E.CurrentGroupFLG = '1' " +
                "order by WorkerID";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 作業者マスタ　抽出　（groupCode）
        /// </summary>
        /// <param name="groupCode">組立部門</param>
        public (bool IsOk, DataTable Table) GetWorkerMstGroupCode(string groupCode)
        {
            string sql = "SELECT " +
                         "A.WorkerID, " +
                         "A.WorkerName, " +
                         "A.GroupCode, " +
                         "C.部門名 AS GroupCodeName " +
                         "FROM " +
                         "M_Worker AS A " +
                         "LEFT OUTER JOIN SANSODB.dbo.M_CateName AS B " +
                         "ON A.PositionCate = B.CateCode AND " +
                         "B.GroupCode = '0000' AND " +
                         "B.DivisionCode = '17' " +
                         "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS C " +
                         "ON A.GroupCode = C.部門コード " +
                         "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS D " +
                         "ON A.CreateID = D.ID AND " +
                         "D.CurrentGroupFLG = '1' " +
                         "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS E " +
                         "ON A.UpdateID = E.ID AND " +
                         "E.CurrentGroupFLG = '1' " +
                         "WHERE " +
                         "(A.EnableCate = '1') AND " +
                         "(A.GroupCode = '" + groupCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 作業者マスタ教育者　抽出
        /// </summary>
        public (bool IsOk, DataTable Table) GetWorkerMstTrainer( )
        {
            string sql = "SELECT " +
                         "A.WorkerID, " +
                         "A.WorkerName " +
                         "FROM " +
                         "M_Worker AS A " +
                         "LEFT OUTER JOIN SANSODB.dbo.M_CateName AS B " +
                         "ON A.PositionCate = B.CateCode AND " +
                         "B.GroupCode = '0000' AND " +
                         "B.DivisionCode = '17' " +
                         "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS C " +
                         "ON A.GroupCode = C.部門コード " +
                         "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS D " +
                         "ON A.CreateID = D.ID AND " +
                         "D.CurrentGroupFLG = '1' " +
                         "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS E " +
                         "ON A.UpdateID = E.ID AND " +
                         "E.CurrentGroupFLG = '1' " +
                         "WHERE " +
                         "(A.PositionCate >= 10) " +
                         "ORDER BY A.PositionCate DESC, A.WorkerID ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 作業者マスタ責任者　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetWorkerMstResponsiblePerson(string groupCode)
        {
            string sql = "SELECT " +
                         "WorkerID, " +
                         "WorkerName, " +
                         "PositionCate, " +
                         "GroupCode " +
                         "FROM " +
                         "M_Worker " +
                         "WHERE " +
　　　　　　　　　　　　 "(EnableCate = '1') AND " +
　　　　　　　　　　　　 "(ResponsiblePersonCate = 1) AND " +
　　　　　　　　　　　　 "(PositionCate >= 30) OR " +
　　　　　　　　　　　　 "(EnableCate = '1') AND " +
                         "(ResponsiblePersonCate = 1) AND " +
                        $"(GroupCode = '{ groupCode }') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }
    }
}
