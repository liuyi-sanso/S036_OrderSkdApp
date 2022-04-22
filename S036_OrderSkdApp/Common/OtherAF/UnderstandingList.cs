using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 理解度のリスト
    /// </summary>
    public class UnderstandingList
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnderstandingList()
        {
        }

        /// <summary>
        /// 教育内容を返す
        /// </summary>
        /// <returns>教育内容のデータテーブルを返す</returns>
        public DataTable ReturnUnderstandingList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("理解度");
            dt.Columns.Add("理解度名");

            dt.Rows.Add("A", "良く理解している");
            dt.Rows.Add("B", "理解している");
            dt.Rows.Add("C", "一部理解していない");
            dt.Rows.Add("D", "再教育が必要");

            return dt;
        }

    }
}
