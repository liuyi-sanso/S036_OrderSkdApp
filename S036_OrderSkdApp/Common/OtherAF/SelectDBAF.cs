using System;
using SansoBase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 各ソリューション毎のデータの取得を行うクラス
    /// </summary>
    /// <remarks>対象DB：生産管理、生産計画、WORK、TEST …</remarks>
    public static class SelectDBAF 
    {
        #region< 生産管理 >

        /// <summary>
        /// 産管理.dbo.機種組替ファイル
        /// </summary>
        /// <param name="requestCode">依頼番号</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetProductChangeFile(string requestCode)
        {
            string sql = "SELECT 依頼番号, 組替日, 変更前機種コード, 変更前機種名, 変更後機種コード, 変更後機種名, 台数, 実績数, 依頼部門, 組替部門, 検査フラッグ, "
                       + "検査者, 完了フラッグ, 作番, 作番２, 作番３, 作番４, 作番５, 理由 "
                       + "FROM 生産管理.dbo.機種組替ファイル "
                       + "WHERE (依頼番号 = '" + requestCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 三相メイン.dbo.製品倉庫別在庫マスタ
        /// </summary>
        /// <param name="productCode">機種コード</param>
        /// <param name="storeHouseCode">倉庫コード</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetProductStoreHouseMst(string productCode, string storeHouseCode)
        {
            string sql = "SELECT 機種コード, 倉庫コード, 前残数量, 入庫数量, 出庫数量, 当残数量, 最終入庫日, 最終出庫日 "
                       + "FROM 三相メイン.dbo.製品倉庫別在庫マスタ "
                       + "WHERE (機種コード = '" + productCode + "') "
                       + "AND (倉庫コード = '" + storeHouseCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 生産管理.dbo.組立予定ファイル
        /// </summary>
        /// <param name="sakuban">作番</param>
        /// <param name="date">組立予定日</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetAssemblyScheduleFile(string sakuban, DateTime date)
        {
            string sql = "SELECT * "
                       + "FROM 生産管理.dbo.組立予定ファイル "
                       + "WHERE (作番 = '" + sakuban + "') "
                       + "AND (組立予定日 = '" + date + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                throw new Exception("SQL実行が失敗しました");
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 生産管理.dbo.組立予定ファイル
        /// </summary>
        /// <param name="juyoyosokuCode">工事番号</param>
        /// <param name="date">組立予定日</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetAssemblyScheduleFileJuyoyosoku(string juyoyosokuCode, DateTime date)
        {
            string sql = "SELECT * "
                       + "FROM 生産管理.dbo.組立予定ファイル "
                       + "WHERE (工事番号 = '" + juyoyosokuCode + "') "
                       + "AND (組立予定日 = '" + date + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                throw new Exception("SQL実行が失敗しました");
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 生産管理.dbo.組立予定ファイル
        /// </summary>
        /// <param name="juyoyosokuCode">工事番号</param>
        /// <param name="groupCode">組立部門</param>
        /// <param name="lineCode">組立ライン</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetAssemblyScheduleFileJuyoyosoku(string juyoyosokuCode, string groupCode, string lineCode)
        {
            string sql = "SELECT * "
                       + "FROM 生産管理.dbo.組立予定ファイル "
                       + "WHERE (工事番号 = '" + juyoyosokuCode + "') "
                       + "AND (組立部門 = '" + groupCode + "') "
                       + "AND (組立ライン = '" + lineCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                throw new Exception("SQL実行が失敗しました");
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 生産管理.dbo.Ｖ機種組替依頼番号参照
        /// </summary>
        /// <param name="sakuban">作番</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetAllSakuban(string sakuban)
        {
            string sql = "SELECT * "
                       + "FROM 生産管理.dbo.Ｖ機種組替依頼番号参照 "
                       + "WHERE (作番 = '" + sakuban + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                throw new Exception("SQL実行が失敗しました");
            }
            return (true, af.Table);
        }

        #endregion

        #region< 生産計画 >

        public static (bool IsOk, DataTable Table) GetJyuyoyosoku(string JyuyoyosokuCode)
        {
            string sql = "SELECT * "
                       + "FROM 生産計画.dbo.製造指令マスタ "
                       + "WHERE (工事番号 = '" + JyuyoyosokuCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }


        public static (bool IsOk, int Count) GetCountProdPlanWSakubanSchedule()
        {
            string sql = "select count(*) AS Count from 生産計画.dbo.Ｗ作番日程ワーク";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true, af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        public static (bool IsOk, int Count) GetCountProdPlanWSakubanScheduleNot4510()
        {
            string sql = "select count(*) AS Count from 生産計画.dbo.Ｗ作番日程ワーク WHERE ISNULL(組立部門, '') <> '4510'";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true, af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        /// <summary>
        /// 生産計画.dbo.ラインＭ
        /// </summary>
        /// <param name="groupCode">課別CD</param>
        /// <param name="lineCode">ライン（省略可能）</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetLineMst(string groupCode, string lineCode = "")
        {
            string sql = "SELECT ライン, ライン名 "
                       + "FROM 生産計画.dbo.ラインＭ "
                       + "WHERE 課別CD = '" + groupCode + "' "
                       + "AND ISNULL(ライン, '') <> '%%%' "
                       + (lineCode == "" ? "" : ("AND ライン = '" + lineCode + "' "));
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 生産計画.dbo.ラインＭ
        /// </summary>
        /// <param name="groupCode">課別CD</param>
        /// <param name="lineCode">ライン（省略可能）</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetLineMstParts()
        {
            string sql = "SELECT ライン, ライン名 "
                       + "FROM 生産計画.dbo.ラインＭ "
                       + "WHERE ライン IN('219', '249', '269', '389') "
                       + "GROUP BY ライン, ライン名 "
                       + "ORDER BY ライン ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 生産計画.dbo.作番日程ファイル
        /// </summary>
        /// <param name="sakuban">作番</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetSakubanScheduleFile(string sakuban)
        {
            string sql = "SELECT * FROM 生産計画.dbo.作番日程ファイル WHERE (作番 = '" + sakuban + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 三相メイン >

        public static (bool IsOk, DataTable Table) CheckProduct(string productCode, string productName)
        {
            string sql = "SELECT 機種コード,機種名 "
                       + "FROM 三相メイン.dbo.機種マスタ "
                       + "WHERE (機種コード = '" + productCode + "') "
                       + "AND (機種名 = '" + productName + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        public static (bool IsOk, int Count) GetCountProductMstProductCode(string productCode)
        {
            string sql = "SELECT count(*) AS Count "
                       + "FROM 三相メイン.dbo.機種マスタ "
                       + "WHERE (機種コード = '" + productCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true , af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        public static (bool IsOk, int Count) GetCountPartsMst(string partsCode)
        {
            string sql = "SELECT count(*) AS Count "
                       + "FROM 三相メイン.dbo.部品マスタ "
                       + "WHERE (部品コード = '" + partsCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true, af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        public static (bool IsOk, int Count) GetCountProductMstProductName(string ProductName)
        {
            string sql = "SELECT count(*) AS Count " +
                         "FROM 三相メイン.dbo.機種マスタ " +
                         "WHERE (機種名 = '" + ProductName + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true, af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        public static (bool IsOk, int Count) GetCountSansoMainAddressMst(string cusCode)
        {
            string sql = "SELECT count(*) AS Count " +
                         "FROM 三相メイン.dbo.住所録マスタ " +
                         "WHERE (会社コード = '" + cusCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, 0);
            }

            return (true, af.Table.Rows[0].Field<int?>("Count") ?? 0);
        }

        public static (bool IsOk, DataTable Table) GetSansoMainAddressMst(string cusCode)
        {
            string sql = "SELECT * " +
                         "FROM 三相メイン.dbo.住所録マスタ " +
                         "WHERE (会社コード = '" + cusCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true , af.Table);
        }


        public static (bool IsOk, DataTable Table) CheckGroupLine(string GroupCode, string LineCode)
        {
            string sql = "SELECT GroupCode,LineCode " +
                         "FROM 三相メイン.dbo.AssemblyLineMst " +
                         "WHERE " +
                         "(GroupCode = '" + GroupCode + "') AND " +
                         "(LineCode = '" + LineCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// ラインマスタから全件取得
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetGroupLine()
        {
            string sql = "select LineCode,LineName,GroupCode,部門名 " +
                         "from 三相メイン.dbo.AssemblyLineMst AS A " +
                         "left join 三相メイン.dbo.部門マスタ AS B on A.GroupCode = B.部門コード " +
                         "where EnableCate = '1' " +
                         "order by GroupCode,LineCode ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// ラインマスタのラインコードから、部門名を取得
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetLineGroup(string line)
        {
            string sql = "select * from 三相メイン.dbo.AssemblyLineMst AS A " +
                         "left join 三相メイン.dbo.部門マスタ AS B on A.GroupCode = B.部門コード " +
                         "where LineCode = '" + line + "' ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        public static (bool IsOk, DataTable Table) GetSansoMainGroupMst(string groupCode)
        {
            string sql = "SELECT 部門コード, 部名, グループ名, 係名, 部門名, 本社営業所区分, 住所, 電話番号, 作成日付, 変更日付,有効部門区分 " +
                         "FROM 三相メイン.dbo.部門マスタ " +
                         "WHERE (部門コード = '" + groupCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// カレンダーファイル抽出
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetCalendarFile(DateTime date)
        {
            string sql = "select * from 三相メイン.dbo.カレンダファイル " +
                         "where (西暦日付 = '" + date + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 三相メイン.dbo.M_STAFF
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetStaffMst(string staffCode)
        {
            string sql = "SELECT * "
                       + "FROM 三相メイン.dbo.M_STAFF "
                       + "WHERE (StaffCode = '" + staffCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 三相メイン.dbo.月末機種マスタ
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetSansoMainProdMstE(string productCode)
        {
            string sql = "SELECT * "
                       + "FROM 三相メイン.dbo.月末機種マスタ "
                       + "WHERE (機種コード = '" + productCode + "') ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< SANSODB >
        /// <summary>
        /// リードタイム取得
        /// </summary>
        /// <param name="date">基準日</param>
        /// <param name="lt">加減日数</param>
        /// <returns></returns>
        public static (bool IsOk, DateTime LT) GetLeadTime(DateTime date,int lt)
        {
            string sql = "select SANSODB.dbo.FC_LT2('" + date + "'," + lt + ") AS LT ";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, date);
            }

            return (true, af.Table.Rows[0].Field<DateTime>("LT"));
        }

        /// <summary>
        /// 製品区分抽出（製品区分全てを抽出する場合は、SansoBaseにあります）
        /// </summary>
        /// <returns></returns>
        public static (bool isOk, DataTable table) GetProductCate(string CateCode)
        {
            try
            {
                string SQL = "select CateCode,CateName,DisplayOrder " +
                    "FROM [SANSODB].[dbo].[M_CateName] " +
                    "where " +
                    "(GroupCode = '0000') and " +
                    "(DivisionCode = '1') and " +
                    "(CateCode = '" + CateCode + "') " +
                    "order by DisplayOrder ";

                var af = CommonAF.ExecutSelectSQL(SQL);


                if (af.IsOk == false)
                {
                    throw new Exception();
                }
                return (true, af.Table);
            }
            catch
            {
                return (false, null);
            }
        }

        #endregion

        #region< 原価管理 >

        /// <summary>
        /// 原価管理.dbo.移行マスタ
        /// </summary>
        /// <param name="productCode">機種コード</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetTransMst(string productCode)
        {
            string sql = "SELECT * FROM 原価管理.dbo.移行マスタ "
                       + "WHERE (機種コード = '" + productCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 原価管理.dbo.加工単価マスタ
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetManufactureUnitPriceMst(string partsCode)
        {
            string sql = "SELECT * FROM 原価管理.dbo.加工単価マスタ "
                       + "WHERE (部品コード = '" + partsCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 原価管理.dbo.レートマスタ
        /// </summary>
        /// <param name="groupCode">組立部門</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetRateMst(string groupCode)
        {
            string sql = "SELECT * FROM 原価管理.dbo.レートマスタ "
                       + "WHERE (組立部門 = '" + groupCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 原価管理.dbo.移行加工履歴ファイル
        /// </summary>
        /// <param name="batchCode">バッチ番号</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetTransProcessHistoryFile(string batchCode)
        {
            string sql = "SELECT * FROM 原価管理.dbo.移行加工履歴ファイル "
                       + "WHERE (バッチ番号 = '" + batchCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 原価管理システムで特に使用する、組立部門コードを抽出
        /// </summary>
        /// <returns>組立部門コード、組立部門名のDataTable</returns>
        public static (bool IsOk, DataTable Table) GetCostAssemblyGroupCode()
        {
            string sql = "SELECT '_' AS 部門コード, 'すべて' AS 部門名 UNION ALL SELECT 部門コード, 部門名 FROM 三相メイン.dbo.部門マスタ "
                       + "WHERE (LEFT(ISNULL(製販区分, ''), 1) = 'S') AND (LEFT(ISNULL(部門区分, ''), 1) = 'S') "
                       + "ORDER BY 部門コード ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 原価管理システムで特に使用する、営業部門コードを抽出
        /// </summary>
        /// <returns>営業部門コード、営業部門名のDataTable</returns>
        public static (bool IsOk, DataTable Table) GetCostSalesGroupCode()
        {
            string sql = "SELECT '_' AS 部門コード, 'すべて' AS 部門名 UNION ALL SELECT 部門コード, 部門名 FROM 三相メイン.dbo.部門マスタ "
                       + "WHERE (LEFT(ISNULL(部門区分, ''), 1) = 'U') "
                       + "ORDER BY 部門コード ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 製造調達 >
        /// <summary>
        /// 調達問合せシステムで特に使用する課別コードを抽出
        /// </summary>
        /// <returns>課別コード、課別名のDataTable</returns>
        public static (bool IsOk, DataTable Table) GetProcurementGroupCode()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("_", "すべて");
            dt.Rows.Add("3621", "組立課 組立一係");
            dt.Rows.Add("3624", "組立課 組立三係（精密）");
            dt.Rows.Add("3626", "組立課 組立二係");
            dt.Rows.Add("3638", "組立課 組立五係（汎用）");

            return (true, dt);
        }

        /// <summary>
        /// 調達問合せシステムで特に使用する課別コードを抽出（すべて無し）
        /// </summary>
        /// <returns>課別コード、課別名のDataTable</returns>
        public static (bool IsOk, DataTable Table) GetProcurementGroupCode2()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("3621", "組立課 組立一係");
            dt.Rows.Add("3624", "組立課 組立三係（精密）");
            dt.Rows.Add("3626", "組立課 組立二係");
            dt.Rows.Add("3638", "組立課 組立五係（汎用）");

            return (true, dt);
        }

        /// <summary>
        /// 製造調達.dbo.仕入先マスタ
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetProcurementSupStaffName()
        {
            string sql = "SELECT 三相担当者名 FROM 製造調達.dbo.仕入先マスタ WHERE ISNULL(三相担当者名, '') <> '' GROUP BY 三相担当者名 ORDER BY 三相担当者名 ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 製造熊山 >

        /// <summary>
        /// 製造熊山.dbo.熊山有償単価Ｍ
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetKumayamaUnitPriceMst(string partsCode)
        {
            string sql = "SELECT * FROM 製造熊山.dbo.熊山有償単価Ｍ "
                       + "WHERE (部品コード = '" + partsCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 荏原管理 >

        /// <summary>
        /// 荏原管理.dbo.Ｐ番管理マスタ
        /// </summary>
        /// <param name="pCode">Ｐ番</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetPCodeMst(string pCode)
        {
            string sql = "SELECT * FROM 荏原管理.dbo.Ｐ番管理マスタ "
                       + "WHERE (Ｐ番 = '" + pCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 荏原管理.dbo.Ｐ番ピッキングシート履歴
        /// </summary>
        /// <param name="pCode">Ｐ番</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetPCodePickingSheetHistory(string pCode, string partsCode = "")
        {
            string sql = "SELECT * FROM 荏原管理.dbo.Ｐ番ピッキングシート履歴 "
                       + "WHERE (Ｐ番 = '" + pCode + "') "
                       + (partsCode == "" ? "" : ("AND (部品コード = '" + partsCode + "') "));
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 発注管理システム >

        /// <summary>
        /// 発注管理システム.dbo.課員
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetHaccyuStaffMst()
        {
            string sql = "SELECT 課員コード, 社員名 FROM 発注管理システム.dbo.課員 "
                       + "ORDER BY 課員コード ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        /// <summary>
        /// 発注管理システム.dbo.仕入先担当者
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetHaccyuSupStaffMst()
        {
            string sql = "SELECT DISTINCT 発注管理システム.dbo.仕入先担当者.課員コード, 三相メイン.dbo.職番マスタ.漢字名 "
                       + "FROM　発注管理システム.dbo.仕入先担当者 INNER JOIN 三相メイン.dbo.職番マスタ ON 発注管理システム.dbo.仕入先担当者.課員コード = 三相メイン.dbo.職番マスタ.職番 "
                       + "WHERE (発注管理システム.dbo.仕入先担当者.課員コード <> '9999') "
                       + "ORDER BY 発注管理システム.dbo.仕入先担当者.課員コード ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion

        #region< 物流管理 >

        /// <summary>
        /// 物流管理.dbo.営業部門マスタ
        /// </summary>
        /// <param name="groupCode">営業部門</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetLogisticsSalesGroupMst(string groupCode)
        {
            string sql = "SELECT * FROM 物流管理.dbo.営業部門マスタ "
                       + "WHERE (営業部門 = '" + groupCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion


        #region< 塗装管理 >

        /// <summary>
        /// 塗装管理.dbo.受注マスタ
        /// </summary>
        /// <param name="ordCode">注文番号</param>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) GetPaintRoMst(string ordCode)
        {
            string sql = "SELECT * FROM 塗装管理.dbo.受注マスタ "
                       + "WHERE (注文番号 = '" + ordCode + "') ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }

            return (true, af.Table);
        }

        #endregion
    }
}
