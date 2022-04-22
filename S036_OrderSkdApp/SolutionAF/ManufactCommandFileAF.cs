using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 製造調達.dbo.製造指令ファイルに関する処理をまとめる
    /// </summary>
    public class ManufactCommandFileAF
    {
        /// <summary>
        /// 需要予測番号から製造調達.dbo.製造指令ファイルのデータを取得
        /// </summary>
        public (bool IsOk, DataTable Table) GetByJyuyoyosoku(string jyuyoyosokuCode)
        {
            string sql = $" SELECT * "
                       + $" FROM 製造調達.dbo.製造指令ファイル "
                       + $" WHERE (工事番号 = '{jyuyoyosokuCode}')";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }
    }
}
