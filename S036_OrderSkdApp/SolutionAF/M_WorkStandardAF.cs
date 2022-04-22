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
    /// 部品調達.dbo.M_WorkStandard（作業基準マスタ）に関する処理をまとめる
    /// </summary>
    class M_WorkStandardAF
    {
        /// <summary>
        /// 作業基準マスタ　取得
        /// </summary>
        /// <param name="prodCode">機種コード</param>
        public (bool IsOk, DataTable Table) GetMWorkStandard(string prodCode)
        {
            string sql = "SELECT " +
                         "ProcessNo AS 順番, " +
                         "[Work] AS 作業, " +
                         "WorkContents AS 作業内容, " +
                         "TIME AS 完了時間 " +
                         "FROM " +
                         "M_WorkStandard " +
                         "WHERE " +
                        $"(ProdCode = '{ prodCode }') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 作業基準マスタ　取得
        /// </summary>
        /// <param name="prodCode">機種コード</param>
        /// <param name="processNo">工程順</param>
        public (bool IsOk, DataTable Table) GetMWorkStandard(string prodCode, int processNo)
        {
            string sql = "SELECT " +
                         "WorkNo AS 順番, " +
                         "[Work] AS 作業, " +
                         "WorkContents AS 作業内容, " +
                         "Jig AS 治具, " +
                         "ImportantPoint AS 注意点, " +
                         "Time AS 完了時間 " +
                         "FROM " +
                         "M_WorkStandard " +
                         "WHERE " +
                        $"(ProdCode = '{ prodCode }') AND " +
                        $"(ProcessNo = { processNo }) " +
                         "ORDER BY 順番";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 標準時間マスタ　新規更新
        /// </summary>
        /// <param name="controlList"></param>
        /// <param name="secTime"></param>
        /// <returns></returns>
        public bool UpdateMWorkStandard(List<ControlParam> controlList, DataTable dt)
        {
            string sql = "";

            foreach (var v in dt.AsEnumerable().Select(v => v))
            {
                sql = "MERGE INTO 部品調達.dbo.M_WorkStandard AS A " +
                    "USING (SELECT " +
                    "'" + controlList.SGetText("productCodeC1TextBox") + "' AS ProductCode, " +
                    "'" + controlList.SGetText("processNoC1NumericEdit") + "' AS ProcessNo, " +
                    "" + v.Field<int>("順番") + " AS Turn " +
                    " ) AS B " +
                    "on (" +
                    "A.ProdCode = B.ProductCode AND " +
                    "A.ProcessNo = B.ProcessNo AND " +
                    "A.WorkNo = B.Turn " +
                    ") " +
                    "WHEN MATCHED " +
                    "THEN " +
                    "UPDATE SET " +
                    "Work = '" + v.Field<string>("作業") + "'," +
                    "WorkContents = '" + v.Field<string>("作業内容") + "'," +
                    "ImportantPoint = '" + v.Field<string>("注意点") + "'," +
                    "Jig = '" + v.Field<string>("治具") + "'," +
                    "Time = " + v.Field<int>("完了時間") + "," +
                    //"SGH = '" + v.Field<string>("SGH") + "'," +
                    "UpdateID = '" + LoginInfo.Instance.UserId + "'," +
                    "UpdateDate = '" + DateTime.Now + "'" +
                    "WHEN NOT MATCHED " +
                    "THEN " +
                    "INSERT(ProdCode, Line, Num, ProcessNo, WorkNo, Work, WorkContents, ImportantPoint, Jig, Time, " +
                    "CreateDate, CreateID)" +
                    "VALUES " +
                    "('" + controlList.SGetText("productCodeC1TextBox") + "'," +
                    "'" + controlList.SGetText("lineC1ComboBox") + "'," +
                    "" + controlList.SGetText("numC1ComboBox") + "," +
                    "" + controlList.SGetText("processNoC1NumericEdit") + "," +
                    "" + v.Field<int>("順番") + "," +
                    "'" + v.Field<string>("作業") + "'," +
                    "'" + v.Field<string>("作業内容") + "'," +
                    "'" + v.Field<string>("注意点") + "'," +
                    "'" + v.Field<string>("治具") + "'," +
                    "" + v.Field<int>("完了時間") + "," +
                    //"'" + v.Field<string>("SGH") + "'," +
                    "'" + DateTime.Now + "'," +
                    "'" + LoginInfo.Instance.UserId + "'); ";

                var af = CommonAF.ExecutUpdateSQL(sql);
                if (af.IsOk == false)
                {
                    return af.IsOk;
                }
            }
            return true;
        }

        /// <summary>
        /// 作業基準表　取得
        /// </summary>
        /// <param name="prodCode">機種コード</param>
        /// <param name="processNo">工程順</param>
        public (bool IsOk, DataTable Table) GetWorkStandardList(string prodCode, int processNo)
        {
            string sql = "SELECT " +
                         "A.ProdCode AS 機種コード, " +
                         "B.機種名, " +
                         "A.WorkNo AS 順番, " +
                         "A.[Work] AS 作業, " +
                         "A.WorkContents AS 作業内容, " +
                         "A.Jig AS 治具, " +
                         "A.ImportantPoint AS 注意点, " +
                         "A.TIME AS 時間, " +
                         "A.UpdateDate AS 更新日 " +
                         "FROM " +
                         "M_WorkStandard AS A " +
                         "LEFT OUTER JOIN 三相メイン.dbo.機種マスタ AS B " +
                         "ON A.ProdCode = B.機種コード " +
                         "WHERE " +
                        $"(A.ProdCode = '{ prodCode }') AND " +
                        $"(A.ProcessNo = { processNo }) " +
                         "ORDER BY " +
                         "順番 ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }
    }
}
