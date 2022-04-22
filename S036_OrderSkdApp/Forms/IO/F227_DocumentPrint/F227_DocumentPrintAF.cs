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
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace S036_OrderSkdApp
{
    class F227_DocumentPrintAF
    {
        /// <summary>
        /// 実行中のexeファイルのディレクトリ(exeファイル名は含まず)
        /// </summary>
        protected readonly static string EXE_DIRECTORY = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        /// <summary>
        /// 伝票データ入出庫ファイル更新
        /// </summary>
        /// <returns></returns>
        public (bool isOk ,string msg) CreateDocumentData(DataTable table,DateTime acceptDate)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                DataTable printDtG = new DataTable();
                DataTable printDtS = new DataTable();
                DataTable printDtK = new DataTable();
                DataTable printDtK3630 = new DataTable();

                string sql = "";
                var now = DateTime.Now;
                string mTagStatus = "";

                try
                {
                    // チェック有り抽出
                    var selectTable = table.AsEnumerable()
                                        .Where(v => v.Field<string>("Choice") == "True")
                                        .ToList();
                    int i = 0;
                    foreach (var v in selectTable) 
                    {
                        // 仕入先区分抽出
                        string supCate = v.Field<string>("SupCate");

                        // 伝票番号作成
                        sql = "SELECT 番号管理 " +
                            "FROM 製造調達.dbo.部門マスタ " +
                            "where " +
                            "部門コード = '" + v.Field<string>("GroupCode") + "' ";
                        var result6 = CommonAF.ExecutSelectSQL(sql, tran, con);
                        if (result6.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "部門マスタ抽出時にエラーが発生しました。");
                        }

                        // 相手先区分によって伝票番号の取得場所を変更する
                        if (supCate == "K")
                        {
                            sql = "SELECT * FROM 製造調達.dbo.仕入先マスタ " +
                                "WHERE " +
                                "仕入先コード = '9998' ";
                        }
                        else if ((supCate == "S") || (supCate == "G"))
                        {
                            sql = "SELECT * FROM 製造調達.dbo.仕入先マスタ " +
                                "WHERE " +
                                "仕入先コード = '9997' ";
                        }
                        else
                        {
                            tran.Rollback();
                            return (false, "次工程コード「" + v.Field<string>("NxtCusCode") + "」の" +
                                           "相手先区分「" + supCate + "」は" +
                                           "間違っています。");
                        }

                        var result7 = CommonAF.ExecutSelectSQL(sql, tran, con);
                        if (result7.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "仕入先マスタ抽出時にエラーが発生しました。");
                        }

                        string newDoCode = "";
                        switch (result6.Table.Rows[0].Field<decimal>("番号管理"))
                        {
                            case 0:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号"] = decimal.Parse(newDoCode);
                                break;
                            case 1:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号１") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号１"] = decimal.Parse(newDoCode);
                                break;
                            case 2:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号２") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号２"] = decimal.Parse(newDoCode);
                                break;
                            case 3:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号３") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号３"] = decimal.Parse(newDoCode);
                                break;
                            case 4:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号４") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号４"] = decimal.Parse(newDoCode);
                                break;
                            case 5:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号６") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号５"] = decimal.Parse(newDoCode);
                                break;
                            default:
                                newDoCode = (result7.Table.Rows[0].Field<decimal>("伝票番号") + 1).ToString("0000");
                                result7.Table.Rows[0]["伝票番号"] = decimal.Parse(newDoCode);
                                break;
                        }

                        // 入出庫ファイル更新
                        var ioFile = new IOFileEditor();
                        ioFile.UnitPriceCate = v.Field<string>("UnitPriceCate");
                        ioFile.PartsCode = v.Field<string>("PartsCode");
                        ioFile.SupCode = v.Field<string>("NxtCusCode");
                        ioFile.UnitPrice = decimal.Parse(v.Field<string>("SUPPLY_COST"));
                        ioFile.ProcessUnitPrice = 0;
                        ioFile.AcceptDate = acceptDate;
                        ioFile.GroupCode = v.Field<string>("GroupCode");
                        ioFile.DoCode = newDoCode;
                        ioFile.PoCode = v.Field<string>("PoCode");
                        ioFile.JyuyoyosokuCode = v.Field<string>("jyuyoyosokuCode");
                        ioFile.Sakuban = v.Field<string>("Sakuban");
                        ioFile.Remarks = "";
                        ioFile.StockCate = v.Field<string>("StockCate").TrimEnd();
                        ioFile.CreateDate = now;

                        if (supCate == "K")
                        {
                            // 社内移行として入出庫ファイル作成
                            ioFile.DataCate = "8";
                            ioFile.InsideTransDataFlg = "M";
                            ioFile.OutNum = decimal.Parse(v.Field<string>("SkdNum"));
                            var result5 = ioFile.InsertIOFileInsideSale(con, ioFile, false, tran);
                            if (result5.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル作成時：" + result5.Msg + "。");
                            }
                            mTagStatus = "2";

                            // 営業支援.dbo.部品手配マスタ更新
                            if (v.Field<string>("NxtCusCode") == "2930") 
                            {
                                sql = "select top 1 isnull(工事番号,'') AS 需要予測番号 " +
                                    "from 製造調達.dbo.発注明細 AS A " +
                                    "where " +
                                    "(注文番号 = '" + v.Field<string>("PoCode") + "') and " +
                                    "(部品コード = '" + v.Field<string>("PartsCode") +  "') and " +
                                    "(isnull(納入指示フラッグ,'') = '1') ";
                                var result9 = CommonAF.ExecutSelectSQL(sql, tran, con);
                                if (result9.IsOk == false)
                                {
                                    tran.Rollback();
                                    return (false, "需要予測番号抽出時にエラーが発生しました。");
                                }

                                sql = "update 営業支援.dbo.部品手配マスタ " +
                                        "set " +
                                        "依頼先納入数 = isnull(依頼先納入数,0) + " + decimal.Parse(v.Field<string>("SkdNum")) + " " +
                                        "where " +
                                        "(注文番号 = '"+ result9.Table.Rows[0].Field<string>("需要予測番号") + "') and " +
                                        "(部品コード = '" + v.Field<string>("PartsCode") + "')";
                                var result10 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                                if (result10.IsOk == false)
                                {
                                    tran.Rollback();
                                    return (false, "営業支援の部品手配マスタ更新時にエラーが発生しました。");
                                }
                            }
                        }
                        else if (supCate == "G")
                        {
                            // 社外有償支給として入出庫ファイル作成
                            ioFile.DataCate = "6";
                            ioFile.NSupCate = "Y";
                            ioFile.NSupStockTransDataFlg = "M";
                            ioFile.InNum = 0;
                            ioFile.OutNum = decimal.Parse(v.Field<string>("SkdNum"));
                            var result5 = ioFile.InsertIOFileNSupStockTrans(con, ioFile, false, tran);
                            if (result5.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル作成時：" + result5.Msg + "。");
                            }
                            mTagStatus = "8";
                        }
                        else if (supCate == "S")
                        {
                            // 社外有償支給（一般仕入先）として入出庫ファイル作成
                            ioFile.DataCate = "1";
                            ioFile.NSupCate = "Y";
                            ioFile.NSupStockTransDataFlg = "M";
                            ioFile.OutNum = 0;
                            ioFile.InNum = decimal.Parse(v.Field<string>("SkdNum")) * -1;
                            var result5 = ioFile.InsertIOFileCommonBuy(con, ioFile, false, tran);
                            if (result5.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル作成時：" + result5.Msg + "。");
                            }
                            mTagStatus = "8";
                        }
                        else 
                        {
                            // 処理なし
                        }

                        // M_TagCodeのNxtCusCode更新
                        sql = "update SANSODB.dbo.M_TagCode " +
                            "set " +
                            "NxtCusCode = '" + v.Field<string>("NxtCusCode") + "', " +
                            "StateCate = '" + mTagStatus + "', " +
                            "DoCode = '" + newDoCode + "' " +
                            "where " +
                            "TagCode = '" + v.Field<string>("TagCode") + "'";
                        var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                        if (result3.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, " M_TagCodeのNxtCusCode更新時にエラーが発生しました。");
                        }

                        // 次工程マスタ（M_NxtCusCode）削除
                        sql = "DELETE FROM SANSODB.dbo.M_NxtCusCode " +
                            "where " +
                            "(TagCode = '" + v.Field<string>("TagCode") + "')";

                        var result2 = CommonAF.ExecutDeleteSQL(sql, tran, con);

                        if (result2.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "次工程マスタ削除時にエラーが発生しました。");
                        }

                        // 伝票番号更新
                        selectTable[i]["DoCode"] = newDoCode;

                        if (supCate == "K")
                        {
                            sql = "update 製造調達.dbo.仕入先マスタ " +
                                "set " +
                                "伝票番号 =  " + result7.Table.Rows[0].Field<decimal>("伝票番号") + ", " +
                                "伝票番号１ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号１") + ", " +
                                "伝票番号２ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号２") + ", " +
                                "伝票番号３ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号３") + ", " +
                                "伝票番号４ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号４") + ", " +
                                "伝票番号５ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号５") + " " +
                                "WHERE " +
                                "仕入先コード = '9998' ";
                        }
                        else 
                        {
                            sql = "update 製造調達.dbo.仕入先マスタ " +
                                "set " +
                                "伝票番号 =  " + result7.Table.Rows[0].Field<decimal>("伝票番号") + ", " +
                                "伝票番号１ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号１") + ", " +
                                "伝票番号２ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号２") + ", " +
                                "伝票番号３ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号３") + ", " +
                                "伝票番号４ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号４") + ", " +
                                "伝票番号５ =  " + result7.Table.Rows[0].Field<decimal>("伝票番号５") + " " +
                                "WHERE " +
                                "仕入先コード = '9997' ";
                        }
                        var result8 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                        if (result8.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, " 仕入先マスタ更新時にエラーが発生しました。");
                        }

                        i++;
                    }

                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    try
                    {
                        if (con != null)
                        {
                            con.Close();
                        }

                        tran.Dispose();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return (true,"");
            }
        }

        /// <summary>
        /// 伝票データ抽出
        /// </summary>
        /// <returns></returns>
        public (bool isOk, string msg, DataTable printDtG, DataTable printDtS, DataTable printDtK, DataTable printDtK3630)
            GetPrintDT(DataTable table)
        {
            DataTable printDtG = new DataTable();
            DataTable printDtS = new DataTable();
            DataTable printDtK = new DataTable();
            DataTable printDtK3630 = new DataTable();

            string sql = "";

            // チェック有り抽出
            var selectTable = table.AsEnumerable()
                                .Where(v => v.Field<string>("Choice") == "True")
                                .ToList();
            /// <summary>
            /// 社外有償支給抽出（外注）
            /// </summary>
            var selectTableG = selectTable.AsEnumerable()
                                .Where(v => v.Field<string>("SupCate") == "G")
                                .ToList();

            if (selectTableG.Count > 0)
            {
                int k = 0;
                foreach (var v in selectTableG)
                {
                    sql = "SELECT " +
                        "A.課別コード, B.仕入先名１ AS 課別名, A.仕入先コード, C.仕入先名１, D.部品名, A.工事番号, " +
                        "A.注文番号, A.部品コード, " +
                        "case when データ区分 = '1' then A.入庫数 * -1 else A.入庫数 end as 入庫数, " +
                        "A.出庫数 AS 数量, A.単価, " +
                        "case when データ区分 = '1' then  A.金額 * -1 else A.金額 end as 金額, " +
                        "A.受払年月日, A.伝票番号, " +
                        "D.図面番号, A.有償支給データＦ, " +
                        "CASE A.仕入先コード WHEN '4170' THEN CASE ISNULL(A.備考, '') WHEN 'Z' THEN '材料不良返品有償支給' + CHAR(13) + CHAR(10) + ISNULL(D.備考,'') WHEN 'O' THEN '加工応援素材返品有償支給' + CHAR(13) + CHAR(10) + ISNULL(D.備考,'') WHEN '' THEN ISNULL(D.備考,'') ELSE ISNULL(A.備考,'') + CHAR(13) + CHAR(10) + ISNULL(D.備考,'') END ELSE CASE ISNULL(A.備考, '') WHEN 'Z' THEN '材料不良返品有償支給' WHEN 'O' THEN '加工応援素材返品有償支給' ELSE A.備考 END END AS 備考 ," +
                        "CASE A.仕入先コード WHEN '4170' THEN ISNULL(D.備考,'') ELSE '' END AS 部品M塗装色 , " +
                        "A.バッチ処理Ｆ, A.パスワード, A.端末名, " +
                        "case isnull(A.工事番号,'') when '' then (select work.dbo.FC_代表機種(A.部品コード)) else (select top 1 機種名 from 生産計画.dbo.製造指令マスタ AS E left join 三相メイン.dbo.機種マスタ AS F on E.機種コード = F.機種コード where E.工事番号 = isnull(A.工事番号,'')) end AS 代表機種名 " +
                        "FROM 製造調達.dbo.入出庫ファイル AS A " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS B ON A.課別コード = B.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.仕入先コード = C.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS D ON A.部品コード = D.部品コード " +
                        "WHERE " +
                        "(A.伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                        "(A.課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                        "(A.有償支給データＦ = 'M')";
                    var result4 = CommonAF.ExecutSelectSQL(sql);
                    if (result4.IsOk == false)
                    {
                        return (false, "入出庫ファイル抽出時にエラーが発生しました。", null, null, null, null);
                    }

                    if (k == 0)
                    {
                        printDtG = result4.Table.Clone();
                        k++;
                    }

                    if (result4.Table.Rows.Count > 0)
                    {
                        printDtG.Merge(result4.Table);
                    }
                }
            }

            /// <summary>
            /// 社外有償支給抽出（一般仕入先）
            /// </summary>
            var selectTableS = selectTable.AsEnumerable()
                                .Where(v => v.Field<string>("SupCate") == "S")
                                .ToList();

            if (selectTableS.Count > 0)
            {
                int k = 0;
                foreach (var v in selectTableS)
                {
                    sql = "SELECT " +
                        "A.課別コード, B.仕入先名１ AS 課別名, A.仕入先コード, C.仕入先名１, D.部品名, A.工事番号, " +
                        "A.注文番号, A.部品コード, " +
                        "case when A.データ区分 = '1' then A.入庫数 * -1 else A.入庫数 end as 数量," +
                        "A.単価, " +
                        "case when A.データ区分 = '1' then  A.金額 * -1 else A.金額 end as 金額, " +
                        "A.受払年月日, " +
                        "A.伝票番号, D.図面番号, A.有償支給データＦ, " +
                        "CASE A.仕入先コード WHEN '4170' THEN ISNULL(A.備考,'') + CHAR(13) + CHAR(10) + ISNULL(D.備考,'') ELSE A.備考 END AS 備考 , " +
                        "CASE A.仕入先コード WHEN '4170' THEN ISNULL(D.備考,'') ELSE '' END AS 部品M塗装色, " +
                        "A.バッチ処理Ｆ, A.パスワード, A.端末名, " +
                        "case isnull(A.工事番号,'') when '' then (select work.dbo.FC_代表機種(A.部品コード)) else (select top 1 機種名 from 生産計画.dbo.製造指令マスタ AS E left join 三相メイン.dbo.機種マスタ AS F on E.機種コード = F.機種コード where E.工事番号 = isnull(A.工事番号,'')) end AS 代表機種名 " +
                        "FROM 製造調達.dbo.入出庫ファイル AS A " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS B ON A.課別コード = B.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.仕入先コード = C.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS D ON A.部品コード = D.部品コード " +
                        "WHERE " +
                        "(A.伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                        "(A.課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                        "(A.有償支給データＦ = 'M')";
                    var result4 = CommonAF.ExecutSelectSQL(sql);
                    if (result4.IsOk == false)
                    {
                        return (false, "入出庫ファイル抽出時にエラーが発生しました。", null, null, null, null);
                    }

                    if (k == 0)
                    {
                        printDtS = result4.Table.Clone();
                        k++;
                    }

                    if (result4.Table.Rows.Count > 0)
                    {
                        printDtS.Merge(result4.Table);
                    }
                }
            }

            /// <summary>
            /// 社内移行抽出（塗装）
            /// </summary>
            var selectTableK3630 = selectTable.AsEnumerable()
                                .Where(v => v.Field<string>("SupCate") == "K" && v.Field<string>("NxtCusCode") == "3630")
                                .ToList();

            if (selectTableK3630.Count > 0)
            {
                int k = 0;
                foreach (var v in selectTableK3630)
                {
                    // ランダムメッセージ用番号取得
                    sql = "select CONVERT(int, RAND() * (select  max(ID) - min(ID) + 1 from SANSODB.dbo.M_Note AS M)) AS coefficient";

                    var result1 = CommonAF.ExecutSelectSQL(sql);
                    if (result1.IsOk == false)
                    {
                        return (false, "印刷データ取得時にエラーが発生しました。", null, null, null, null);
                    }

                    int coefficient = result1.Table.Rows[0].Field<int>("coefficient");


                    // 入出庫ファイル抽出
                    sql = "SELECT " +
                          "A.TagCode, A.PoCode, A.PartsCode, D.部品名 AS PartsName, D.図面番号 AS DrawingCode, " +
                          "A.SkdNum, A.PackingBoxSerial, A.PackingBoxNum, A.DoCode, B.DELIVERY_DATE AS SkdDate, " +
                          "C.KISYU_CD AS ProductCode, E.機種名 AS ProductName, A.GroupCode, F.部門名 AS GroupName, " +
                          "CASE WHEN isnull(NxtCusCode, '') <> '' THEN NxtCusCode ELSE GroupCode END AS NxtCusCode, " +
                          "CASE WHEN isnull(NxtCusCode, '') <> '' THEN " +
                          "(SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ WHERE (仕入先コード = A.NxtCusCode)) " +
                          "ELSE (SELECT 部門名 FROM 三相メイン.dbo.部門マスタ WHERE (部門コード = GroupCode)) END AS NxtCusName, " +
                          "A.CusCode, " +
                          "(SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ AS 仕入先マスタ_1 " +
                          "WHERE (A.CusCode = 仕入先コード)) AS CusName, " +
                          "CASE isnull(A.UpdateDate, '2000-01-01') WHEN '2000-01-01' THEN A.CreateDate " +
                          "ELSE A.UpdateDate END AS CreateDate, " +
                          "G.Note AS Message, A.Sakuban, H.ハンガー, H.吊り数, H.マスキング, " +
                          "H.マスキングNO, H.洗浄, H.梱包箱, H.最大入数, I.塗装色, J.仕様 AS 備考 " +
                          "FROM " +
                          "SANSODB.dbo.M_TagCode AS A " +
                          "LEFT OUTER JOIN EDIDATA.dbo.T_SHIPMENT_DATA AS B " +
                          "ON A.SkdCode = B.DO_NO " +
                          "LEFT OUTER JOIN SANSODB.dbo.AT_SAKUBAN AS C " +
                          "ON A.Sakuban = C.SAKUBAN " +
                          "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS D " +
                          "ON A.PartsCode = D.部品コード " +
                          "LEFT OUTER JOIN 三相メイン.dbo.機種マスタ AS E " +
                          "ON C.KISYU_CD = E.機種コード " +
                          "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS F " +
                          "ON A.GroupCode = F.部門コード " +
                          "LEFT OUTER JOIN SANSODB.dbo.M_Note AS G " +
                          "ON G.ID = " + coefficient + " " +
                          "LEFT OUTER JOIN 塗装管理.dbo.作業標準マスタ AS H " +
                          "ON H.部品コード = A.PartsCode " +
                          "LEFT OUTER JOIN 製造調達.dbo.Ｖ塗装色参照 AS I " +
                          "ON A.GroupCode = I.仕入先コード AND " +
                          "A.PartsCode = I.部品コード " +
                          "LEFT OUTER JOIN 塗装管理.dbo.部品マスタ AS J " +
                          "ON A.PartsCode = J.部品コード " +
                          "WHERE " +
                          "(ISNULL(A.StateCate, '0') <> '9') AND (A.TagCode = '" + v.Field<string>("TagCode") + "')";

                    //sql = "SELECT " +
                    //    "A.課別コード, E.仕入先名１ AS 課別名, A.仕入先コード, F.仕入先名１, G.部品名, " +
                    //    "A.工事番号, A.注文番号, A.部品コード, A.出庫数, A.単価, A.金額, A.受払年月日, A.伝票番号, " +
                    //    "G.図面番号, D.塗装色, A.社内移行データＦ, " +
                    //    "'K' + SUBSTRING(CONVERT(CHAR(10), A.受払年月日, 111), 3, 2) + SUBSTRING(CONVERT(CHAR(10), A.受払年月日, 111), 6, 2) + RIGHT('00000' + A.伝票番号, 5) AS 現品票番号, " +
                    //    "RIGHT('0000000000' + 'K' + SUBSTRING(CONVERT(CHAR(10), A.受払年月日, 111), 3, 2) + SUBSTRING(CONVERT(CHAR(10), A.受払年月日, 111), 6, 2) + RIGHT('00000' + A.伝票番号, 5), 10) AS バーコード, " +
                    //    "A.パスワード, A.端末名, A.バッチ処理Ｆ, ISNULL(A.DEN_NO, '0') AS DEN_NO, D.マスキングＦ, D.Uマーク有Ｆ, C.仕様 AS 備考, " +
                    //    "LEFT(A.部品コード, 3) + ' ' + SUBSTRING(A.部品コード, 4, 3) + ' ' + SUBSTRING(A.部品コード, 7, 3) AS 部品コード表示, " +
                    //    "C.過去トラ01, C.過去トラ02, C.過去トラ03, C.過去トラ04, C.過去トラ05, B.ハンガー, B.吊り数, " +
                    //    "B.マスキング, B.マスキングNO, B.洗浄, B.梱包箱, B.最大入数, " +
                    //    "case isnull(A.工事番号,'') when '' then (select work.dbo.FC_代表機種(A.部品コード)) else (select top 1 機種名 from 生産計画.dbo.製造指令マスタ AS E left join 三相メイン.dbo.機種マスタ AS F on E.機種コード = F.機種コード where E.工事番号 = isnull(A.工事番号,'')) end AS 代表機種名 " +
                    //    "FROM 製造調達.dbo.入出庫ファイル AS A " +
                    //    "LEFT OUTER JOIN 塗装管理.dbo.作業標準マスタ AS B ON A.部品コード = B.部品コード " +
                    //    "LEFT OUTER JOIN 塗装管理.dbo.部品マスタ AS C ON A.部品コード = C.部品コード " +
                    //    "LEFT OUTER JOIN 製造調達.dbo.Ｖ塗装色参照 AS D ON A.課別コード = D.仕入先コード AND A.部品コード = D.部品コード " +
                    //    "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS E ON A.課別コード = E.仕入先コード " +
                    //    "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS F ON A.仕入先コード = F.仕入先コード " +
                    //    "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS G ON A.部品コード = G.部品コード " +
                    //    "WHERE " +
                    //    "(A.伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                    //    "(A.仕入先コード = '3630') and " +
                    //    "(A.課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                    //    "(A.社内移行データF = 'M')";

                    var result4 = CommonAF.ExecutSelectSQL(sql);
                    if (result4.IsOk == false)
                    {
                        return (false, "入出庫ファイル抽出時にエラーが発生しました。", null, null, null, null);
                    }

                    if (k == 0)
                    {
                        printDtK3630 = result4.Table.Clone();
                        k++;
                    }

                    if (result4.Table.Rows.Count > 0)
                    {
                        printDtK3630.Merge(result4.Table);
                    }
                }
            }

            /// <summary>
            /// 社内移行抽出（塗装以外）
            /// </summary>
            var selectTableK = selectTable.AsEnumerable()
                                .Where(v => v.Field<string>("SupCate") == "K" && v.Field<string>("NxtCusCode") != "3630")
                                .ToList();

            if (selectTableK.Count > 0)
            {
                int k = 0;
                foreach (var v in selectTableK)
                {
                    // 入出庫ファイル抽出
                    sql = "SELECT " +
                        "A.課別コード, B.仕入先名１ AS 課別名, A.仕入先コード, C.仕入先名１, D.部品名, A.工事番号, " +
                        "A.注文番号, A.部品コード, A.出庫数, A.単価, A.金額, A.受払年月日, A.伝票番号, D.図面番号, " +
                        "A.パスワード, A.端末名, A.バッチ処理Ｆ, ISNULL(A.DEN_NO, '0') AS DEN_NO," +
                        "case isnull(A.工事番号,'') when '' then (select work.dbo.FC_代表機種(A.部品コード)) else (select top 1 機種名 from 生産計画.dbo.製造指令マスタ AS E left join 三相メイン.dbo.機種マスタ AS F on E.機種コード = F.機種コード where E.工事番号 = isnull(A.工事番号,'')) end AS 代表機種名 " +
                        "FROM 製造調達.dbo.入出庫ファイル AS A " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS B ON A.課別コード = B.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.仕入先コード = C.仕入先コード " +
                        "LEFT OUTER JOIN 製造調達.dbo.部品マスタ AS D ON A.部品コード = D.部品コード " +
                        "WHERE " +
                        "(A.伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                        "(A.課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                        "(A.社内移行データF = 'M')";
                    var result4 = CommonAF.ExecutSelectSQL(sql);
                    if (result4.IsOk == false)
                    {
                        return (false, "入出庫ファイル抽出時にエラーが発生しました。", null, null, null, null);
                    }

                    if (k == 0)
                    {
                        printDtK = result4.Table.Clone();
                        k++;
                    }

                    if (result4.Table.Rows.Count > 0)
                    {
                        printDtK.Merge(result4.Table);
                    }
                }
            }

            return (true, "", printDtG, printDtS, printDtK, printDtK3630);
        }

        /// <summary>
        /// 印刷済フラグ更新
        /// </summary>
        /// <returns></returns>
        public (bool isOk, string msg) UpdatePrintFlg(DataTable table)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                string sql = "";
                var now = DateTime.Now;

                try
                {
                    // チェック有り抽出
                    var selectTable = table.AsEnumerable()
                                        .Where(v => v.Field<string>("Choice") == "True")
                                        .ToList();

                    /// <summary>
                    /// 社外有償支給発行（外注）
                    /// </summary>
                    var selectTableG = selectTable.AsEnumerable()
                                        .Where(v => v.Field<string>("SupCate") == "G")
                                        .ToList();

                    if (selectTableG.Count > 0)
                    {
                        // 伝票発行済みフラグをKに更新
                        foreach (var v in selectTableG)
                        {
                            sql = "update 製造調達.dbo.入出庫ファイル " +
                                "set 有償支給データＦ = 'K'  " +
                                "WHERE " +
                                "(伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                                "(課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                                "(有償支給データＦ = 'M')";
                            var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                            if (result3.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル更新時にエラーが発生しました。");
                            }
                        }
                    }

                    /// <summary>
                    /// 社外有償支給発行（一般仕入先）
                    /// </summary>
                    var selectTableS = selectTable.AsEnumerable()
                                        .Where(v => v.Field<string>("SupCate") == "S")
                                        .ToList();

                    if (selectTableS.Count > 0)
                    {
                        // 伝票発行済みフラグをKに更新
                        foreach (var v in selectTableS)
                        {
                            sql = "update 製造調達.dbo.入出庫ファイル " +
                                "set 有償支給データＦ = 'K'  " +
                                "WHERE " +
                                "(伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                                "(課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                                "(有償支給データＦ = 'M')";
                            var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                            if (result3.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル更新時にエラーが発生しました。");
                            }
                        }
                    }

                    /// <summary>
                    /// 社内移行発行（塗装）
                    /// </summary>
                    var selectTableK3630 = selectTable.AsEnumerable()
                                        .Where(v => v.Field<string>("SupCate") == "K" && v.Field<string>("NxtCusCode") == "3630")
                                        .ToList();

                    if (selectTableK3630.Count > 0)
                    {
                        // 伝票発行済みフラグをKに更新
                        foreach (var v in selectTableK3630)
                        {
                            sql = "update 製造調達.dbo.入出庫ファイル " +
                                "set 社内移行データF = 'K'  " +
                                "WHERE " +
                                "(伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                                "(仕入先コード = '3630') and " +
                                "(課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                                "(社内移行データF = 'M')";
                            var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                            if (result3.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル更新時にエラーが発生しました。");
                            }
                        }
                    }

                    /// <summary>
                    /// 社内移行発行（塗装以外）
                    /// </summary>
                    var selectTableK = selectTable.AsEnumerable()
                                        .Where(v => v.Field<string>("SupCate") == "K" && v.Field<string>("NxtCusCode") != "3630")
                                        .ToList();

                    if (selectTableK.Count > 0)
                    {
                        // 伝票発行済みフラグをKに更新
                        foreach (var v in selectTableK)
                        {
                            sql = "update 製造調達.dbo.入出庫ファイル " +
                                "set 社内移行データF = 'K'  " +
                                "WHERE " +
                                "(伝票番号 = '" + v.Field<string>("DoCode") + "') and " +
                                "(課別コード = '" + v.Field<string>("GroupCode") + "') and " +
                                "(社内移行データF = 'M')";
                            var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);

                            if (result3.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "入出庫ファイル更新時にエラーが発生しました。");
                            }
                        }
                    }
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    try
                    {
                        if (con != null)
                        {
                            con.Close();
                        }

                        tran.Dispose();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return (true, "");
            }
        }
    }
}
