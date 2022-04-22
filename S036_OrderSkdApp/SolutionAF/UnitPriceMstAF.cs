using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 製造調達.dbo.単価マスタに関する処理をまとめる
    /// </summary>
    public class UnitPriceMstAF
    {
        /// <summary>
        /// 単価情報取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetUnitPrice(string partsCode, string supCode)
        {
            string sql =
            "SELECT " +
            "* " +
            "FROM " +
            "製造調達.dbo.単価マスタ " +
            "WHERE " +
            "部品コード = '" + partsCode + "' AND 仕入先コード = '" + supCode + "'";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// 部品単価マスタ取得（部品コード、仕入先コード）
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        /// <remarks>引数を指定しない場合は、全件を抽出 </remarks>
        public (bool IsOk, DataTable Table) GetPartsUnitPriceMstBySup(string partsCode = "", string supCode = "")
        {
            string sql = "SELECT "
                       + "A.部品コード, "
                       + "B.部品名, "
                       + "A.仕入先コード, "
                       + "C.仕入先名１ AS 仕入先名, "
                       + "ISNULL(A.仕入単価, 0) AS 仕入単価, "
                       + "ISNULL(A.材料費, 0) AS 材料費, "
                       + "ISNULL(A.加工費, 0) AS 加工費, "
                       + "ISNULL(A.支給単価, 0) AS 支給単価, "
                       + "ISNULL(A.見積ロット数, 0) AS 見積ロット数, "
                       + "ISNULL(A.管理番号, '') AS 申請書番号, "
                       + "ISNULL(A.旧仕入単価, 0) AS 旧仕入単価, "
                       + "ISNULL(A.旧材料費, 0) AS 旧材料費, "
                       + "ISNULL(A.旧加工費, 0) AS 旧加工費, "
                       + "ISNULL(A.旧支給単価, 0) AS 旧支給単価, "
                       + "E.StaffName AS 作成者, FORMAT(A.作成日付, 'yyyy/MM/dd HH:mm:ss') AS 作成日時, "
                       + "F.StaffName AS 変更者, FORMAT(A.変更日付, 'yyyy/MM/dd HH:mm:ss') AS 変更日時 "
                       + "FROM 製造調達.dbo.単価マスタ AS A "
                       + "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS B ON A.部品コード = B.部品コード "
                       + "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.仕入先コード = C.仕入先コード "
                       + "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS E on A.CRTSYAIN_CD = E.StaffCode and E.CurrentGroupFLG = '1' "
                       + "LEFT OUTER JOIN 三相メイン.dbo.M_STAFF AS F on A.UPDSYAIN_CD = F.StaffCode and F.CurrentGroupFLG = '1' "
                       ;

            (bool IsOk, DataTable Table, string Sql) result;

            if (partsCode == "" && supCode == "")
            {
                sql += "order by A.部品コード, A.仕入先コード";

                //全件抽出
                result = CommonAF.ExecutSelectSQL(sql);
                if (result.IsOk == false)
                {
                    return (false, null);
                }
            }
            else
            {
                sql += "where A.部品コード = '" + partsCode + "' and A.仕入先コード = '" + supCode + "'";

                result = CommonAF.ExecutSelectSQL(sql);
                if (result.IsOk == false)
                {
                    return (false, null);
                }
            }

            return (true, result.Table);
        }
    }
}
