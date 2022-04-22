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
    public class M_SystemUsingMenuAF
    {
        /// <summary>
        /// 使用可能メニュー抽出
        /// </summary>
        /// <returns></returns>
        public (bool IsOk, DataTable Table) GetMSystemUsingMenu(string staffID, string solutionID)
        {
            string sql = "SELECT " +
                "A.StaffID,A.SolutionID,A.MenuID,E.CateName AS MenuName,A.EnableCate,A.CreateDate,A.CreateID,A.UpdateDate," +
                "A.UpdateID, " +
                "C.StaffName AS 作成者,CONVERT(VARCHAR,A.CreateDate,120) AS 作成日付, " +
                "D.StaffName AS 変更者,CONVERT(VARCHAR,A.UpdateDate,120) AS 変更日付 " +
                "FROM 三相メイン.dbo.M_SystemUsingMenu AS A " +
                "left join 三相メイン.dbo.M_STAFF AS C on A.CreateID = C.ID and C.CurrentGroupFLG = '1' " +
                "left join 三相メイン.dbo.M_STAFF AS D on A.UpdateID = D.ID and D.CurrentGroupFLG = '1' " +
                "left join SANSODB.dbo.M_CateName AS E on A.MenuID = E.CateCode and E.GroupCode = '0000' and E.DivisionCode = '19' " +
                "where " +
                "A.StaffID = '" + staffID + "' and " +
                "A.SolutionID = '" + solutionID + "' and " +
                "A.EnableCate = '1' " +
                "";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

    }
}
