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
    public class CalendarAF
    {
        /// <summary>
        /// 三相メイン.dbo.カレンダファイルの祝日を取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetHoliday()
        {
            string sql = " SELECT * "
                       + " FROM 三相メイン.dbo.カレンダファイル "
                       + " WHERE (休日フラッグ = '#')";
            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }


        /// <summary>
        /// 製造調達カレンダファイル取得
        /// </summary>
        /// <param name="startDay">休日設定日の月初日</param>
        /// <param name="endDay">休日設定日の月末日</param>
        public (bool IsOk, DataTable Table) GetGridViewData(string startDay, string endDay)
        {

            string sql = "SELECT " +
                             "FORMAT(西暦日付,'yyyy/MM/dd') AS 休日設定日, " +
                             "('（' + SUBSTRING(DATENAME(WEEKDAY, 西暦日付), 1, 1) + '）') AS 曜日, " +
                             "FORMAT(作成日付,'yyyy/MM/dd HH:mm:ss') AS 作成日付, " +
                             "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF WHERE ID = UPPER(作成者ID) " +
                             "GROUP BY StaffCode) AS 作成者, " +
                             "FORMAT(変更日付,'yyyy/MM/dd HH:mm:ss') AS 変更日付, " +
                             "(SELECT MAX(StaffName) FROM 三相メイン.dbo.M_STAFF WHERE ID = UPPER(変更者ID) " +
                             "GROUP BY StaffCode) AS 変更者 " +
                             "FROM " +
                             "製造調達.dbo.カレンダファイル " +
                             "WHERE " +
                             "西暦日付 BETWEEN '" + startDay + "' AND '" + endDay + "' " +
                             "ORDER BY " +
                             "西暦日付 ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 製造調達カレンダファイル追加
        /// </summary>
        /// <param name="s">休日設定日</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool InsertCalendarFile(string strDate)
        {
            var now = DateTime.Now;
            var userId = LoginInfo.Instance.UserId;

            string sql = "INSERT INTO " +
                         "製造調達.dbo.カレンダファイル " +
                         "(西暦日付, 作成日付, 作成者ID) " +
                         "VALUES('" + strDate + "', '" + now + "', '" + userId + "') ";

            var result = CommonAF.ExecutUpdateSQL(sql);
            return result.IsOk;
        }


        /// <summary>
        /// 製造調達カレンダファイル削除
        /// </summary>
        /// <param name="s">休日設定日</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool DeleteCalendarFile(string strDate)
        {
            string sql = "DELETE FROM " +
                         "製造調達.dbo.カレンダファイル " +
                         "WHERE (西暦日付 = '" + strDate + "') ";

            var result = CommonAF.ExecutUpdateSQL(sql);
            return result.IsOk;

        }

        /// <summary>
        /// 製造調達カレンダファイル確認
        /// </summary>
        /// <param name="s">休日設定日</param>
        public (bool IsOk, DataTable Table) ExistCheck(string strDate)
        {

            string sql = "SELECT " +
                         "西暦日付 " +
                         "FROM " +
                         "製造調達.dbo.カレンダファイル " +
                         "WHERE " +
                         "西暦日付 = '" + strDate + "' ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);

        }
    }
}
