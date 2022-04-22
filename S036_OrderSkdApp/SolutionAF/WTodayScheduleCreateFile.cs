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
    ///  製造調達.dbo.W当日日程ファイルに関する処理をまとめる
    /// </summary>
    public class WTodayScheduleCreateFile
    {
        /// <summary>
        /// W当日日程ファイル　取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetWTodayScheduleCreateFileToday(List<ControlParam> controlList)
        {
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            if (groupCode == "_") { groupCode = ""; }

            var line = controlList.SGetText("lineC1ComboBox");
            if (line == "_") { line = ""; }

            var sakuban = controlList.SGetText("sakubanC1TextBox");

            string sql = "SELECT " +
                         "* " +
                         "FROM " +
                         "製造調達.dbo.W当日日程ファイル作成 " +
                         "WHERE " +
                         "(0 = 0) ";

            sql += string.IsNullOrEmpty(groupCode) ? "" : $"AND (組立部門 = '{ groupCode }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (組立ライン = '{ line }') ";
            sql += string.IsNullOrEmpty(line) ? "" : $"AND (作番 = '{ sakuban }') ";

            var af = CommonAF.ExecutSelectSQL(sql);

            return (af.IsOk, af.Table);
        }
    }
}
