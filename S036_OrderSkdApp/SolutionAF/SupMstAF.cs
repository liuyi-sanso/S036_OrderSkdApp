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
    class SupMstAF
    {
        /// <summary>
        /// 仕入先マスタ抽出
        /// </summary>
        /// <param name="supCode">仕入先コード</param>
        /// <returns>
        /// IsOk：True エラー無し、False エラー有り
        /// Table：抽出したデータ
        /// </returns>
        public (bool IsOk, DataTable Table) GetSupMST(string supCode)
        {
            string sql = "select * from 製造調達.dbo.仕入先マスタ " +
                "where 仕入先コード = '" + supCode + "'";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 仕入先マスタ抽出(社内のみ)
        /// </summary>
        /// <param name="supCode">仕入先コード</param>
        /// <returns>
        /// IsOk：True エラー無し、False エラー有り
        /// Table：抽出したデータ
        /// </returns>
        public (bool IsOk, DataTable Table) GetInsideSupMST(string supCode)
        {
            string sql = "select * from 製造調達.dbo.仕入先マスタ " +
                "where 仕入先区分 = 'K' and 仕入先コード = '" + supCode + "'";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 仕入先区分　抽出
        /// </summary>
        /// <returns>
        /// IsOk：True エラー無し、False エラー有り
        /// Table：抽出データ
        /// </returns>
        public (bool IsOk, DataTable Table) GetSupCate()
        {
            string sql = "SELECT CateCode AS ID, CateName AS NAME FROM SANSODB.dbo.M_CateName " +
                         "where(GroupCode = '0000') and (DivisionCode = '10') order by DisplayOrder ";

            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }


        /// <summary>
        /// 仕入先マスタ　メンテナンス情報抽出
        /// </summary>
        /// <param name="supCode">仕入先コード</param>
        /// <returns>
        /// IsOk：True エラー無し、False エラー有り
        /// Table：抽出データ
        /// </returns>
        /// <remarks>引数を指定しない場合は、全件を抽出 </remarks>
        public (bool IsOk, DataTable Table) GetSupMstMaint(string supCode)
        {
            string sql = "SELECT 仕入先コード, 仕入先名１, 仕入先名２, 郵便番号, 住所１, 住所２, 電話番号, ＦＡＸ番号, " +
                         "Ｅメールアドレス, 仕入先担当者名, 三相担当者名, 仕入先区分, 納品書発行区分, " +
                         "B.StaffName AS 作成者, FORMAT(A.作成日付,'yyyy/MM/dd HH:mm:ss') AS 作成日時, 作成者 AS 作成者ID, " +
                         "C.StaffName AS 変更者, FORMAT(A.変更日付,'yyyy/MM/dd HH:mm:ss') AS 変更日時, 変更者 AS 変更者ID " +
                         "FROM 製造調達.dbo.仕入先マスタ AS A " +
                         "LEFT JOIN 三相メイン.dbo.M_STAFF AS B ON A.作成者 = B.StaffCode AND B.CurrentGroupFLG = '1' " +
                         "LEFT JOIN 三相メイン.dbo.M_STAFF AS C ON A.変更者 = C.StaffCode AND C.CurrentGroupFLG = '1' " +
                         "WHERE 仕入先コード = '" + supCode + "'";

            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }
    }
}
