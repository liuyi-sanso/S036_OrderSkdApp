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
    public class F228_GoodsTagPrintAF
    {
        /// <summary>
        /// M_TagCodeテーブル作成、現品票ファイル抽出
        /// DataTableを使用するためWebAPIよりWindowsFormで更新処理を行う
        /// </summary>
        /// <returns></returns>
        public (bool IsOk, string Msg, DataTable Table) CreateMTagCode(DataTable table,string inspectionMsg)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                string sql = "";
                var now = DateTime.Now;
                List<string> printTagCodeList = new List<string>();

                DataTable printDt = new DataTable();

                try
                {
                    int packingBoxSerial = table.Rows.Count;
                    for (int i = 0; i < table.Rows.Count; i++) 
                    {
                        var dr = table.Rows[i];
                        // M_TagCode作成
                        sql = "INSERT INTO SANSODB.dbo.M_TagCode " +
                        "(BizCode,TagCode,SkdCode,PartsCode,SkdNum,PackingBoxSerial,PackingBoxNum,PoCode,Sakuban,LocCode,CusCode," +
                        "LineCode,NxtCusCode,GroupCode,StateCate,DoCode,InsMtdCate,Remarks,CreateDate,CreateID) " +
                        "VALUES (" +
                        "'" + dr.Field<string>("bizCode") + "'," +
                        "'" + dr.Field<string>("tagCode") + (i + 1).ToString() + "'," +
                        "'" + dr.Field<string>("skdCode") + "'," +
                        "'" + dr.Field<string>("partsCode") + "'," +
                        "" + dr.Field<string>("delivNum") + "," +
                        "" + packingBoxSerial + "," +
                        "" + (i + 1) + "," +
                        "'" + dr.Field<string>("poCode") + "'," +
                        "'" + (dr.Field<string>("sakuban") == "01" ? "" : dr.Field<string>("sakuban")) + "'," +
                        "''," +
                        "'" + dr.Field<string>("groupCode") + "'," +
                        "''," +
                        "'" + dr.Field<string>("nxtCusCode") + "'," +
                        "'" + dr.Field<string>("groupCode") + "'," +
                        "'0'," +
                        "'" + dr.Field<string>("doCode") + "'," +
                        "'0'," +
                        "''," +
                        "'" + now + "'," +
                        "'" + LoginInfo.Instance.UserId + "')";
                        var result2 = CommonAF.ExecutUpdateSQL(sql, tran, con);
                        if (result2.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "M_TagCode作成時にエラーが発生しました。",null);
                        }

                        if (dr.Field<string>("nxtCusCode") != null && dr.Field<string>("nxtCusCode") != "") 
                        {
                            // 次工程マスタ（M_NxtCusCode）を作成
                            sql = "insert into SANSODB.dbo.M_NxtCusCode " +
                                "(InsDate,InsStaffID,TagCode,NxtCusCode,DeviceID,CreateDate,CreateID) " +
                                "values " +
                                "(" +
                                "'" + now + "'," +
                                "'" + LoginInfo.Instance.UserId + "'," +
                                "'" + dr.Field<string>("tagCode") + (i + 1).ToString() + "'," +
                                "'" + dr.Field<string>("nxtCusCode") + "'," +
                                "'" + LoginInfo.Instance.MachineCode + "'," +
                                "'" + now + "'," +
                                "'" + LoginInfo.Instance.UserId + "')";
                            var result3 = CommonAF.ExecutUpdateSQL(sql, tran, con);
                            if (result3.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "M_NxtCusCode作成時にエラーが発生しました。", null);
                            }

                        }

                        printTagCodeList.Add(dr.Field<string>("tagCode") + (i + 1).ToString());
                    }

                    // 印刷データ取得
                    string doCode = table.Rows[0].Field<string>("doCode");
                    string tagCode = table.Rows[0].Field<string>("tagCode");

                    sql = "select CONVERT(int, RAND() * (select  max(ID) - min(ID) + 1 from SANSODB.dbo.M_Note AS M)) AS coefficient";

                    var result1 = CommonAF.ExecutSelectSQL(sql);
                    if (result1.IsOk == false)
                    {
                        return (false, "印刷データ取得時にエラーが発生しました。", null);
                    }
                    int coefficient = result1.Table.Rows[0].Field<int>("coefficient");

                    sql = "select " +
                        "A.TagCode, A.PoCode, A.PartsCode, D.部品名 AS PartsName, D.図面番号 AS DrawingCode, A.SkdNum, " +
                        "A.PackingBoxSerial, A.PackingBoxNum,A.DoCode, " +
                        "B.DELIVERY_DATE AS SkdDate, C.KISYU_CD AS ProductCode, E.機種名 AS ProductName, A.GroupCode, " +
                        "F.部門名 AS GroupName, " +
                        "CASE WHEN isnull(NxtCusCode , '') <> '' THEN NxtCusCode ELSE GroupCode END AS NxtCusCode, CASE WHEN isnull(NxtCusCode , '') <> '' THEN (SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ WHERE (仕入先コード = A.NxtCusCode)) ELSE (SELECT 部門名 FROM 三相メイン.dbo.部門マスタ WHERE (部門コード = GroupCode)) END AS NxtCusName, " +
                        "A.CusCode, " +
                        "(SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ AS 仕入先マスタ_1 WHERE (A.CusCode = 仕入先コード)) AS CusName, " +
                        "CASE isnull(A.UpdateDate,'2000-01-01') when '2000-01-01' then A.CreateDate else A.UpdateDate end AS CreateDate, " +
                        "G.Note AS Message, A.sakuban," +
                        "H.ハンガー, H.吊り数,H.マスキング, H.マスキングNO, H.洗浄, H.梱包箱, H.最大入数,I.塗装色,J.仕様 AS 備考  " +
                        "FROM SANSODB.dbo.M_TagCode AS A " +
                        "left JOIN EDIDATA.dbo.T_SHIPMENT_DATA AS B ON (A.SkdCode = B.DO_NO) " +
                        "left join SANSODB.dbo.AT_SAKUBAN AS C ON (A.Sakuban = C.SAKUBAN) " +
                        "left join 製造調達.dbo.部品マスタ AS D on (A.PartsCode = D.部品コード) " +
                        "left join 三相メイン.dbo.機種マスタ AS E on (C.KISYU_CD = E.機種コード) " +
                        "left join 三相メイン.dbo.部門マスタ AS F on (A.GroupCode = F.部門コード) " +
                        "left join SANSODB.dbo.M_Note AS G on (G.ID = " + coefficient + ") " +
                        "left join 塗装管理.dbo.作業標準マスタ AS H ON (H.部品コード = A.PartsCode) " +
                        "LEFT OUTER JOIN 製造調達.dbo.Ｖ塗装色参照 AS I ON A.GroupCode = I.仕入先コード AND A.PartsCode = I.部品コード " +
                        "left join 塗装管理.dbo.部品マスタ AS J ON A.PartsCode = J.部品コード  " +
                        "WHERE " +
                        "(A.DoCode = '" + doCode + "') AND (A.TagCode like '" + tagCode + "%') AND " +
                        "(ISNULL(A.StateCate, '0') = '0') " +
                        "order by A.PackingBoxSerial ";

                    var result4 = CommonAF.ExecutSelectSQL(sql, tran, con);
                    if (result4.IsOk == false)
                    {
                        tran.Rollback();
                        return (false, "印刷データ抽出時にエラーが発生しました。",null);
                    }

                    printDt = result4.Table;

                    // 初品検査情報をprintDtに追加
                    printDt.Columns.Add("InspectionMsg", typeof(string));

                    for (int i = 0; i < printDt.Rows.Count; i++) 
                    {
                        printDt.Rows[i]["InspectionMsg"] = inspectionMsg;
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
                return (true, "", printDt);
            }
        }


        /// <summary>
        /// M_TagCodeテーブル更新、M_DelivSlipを削除
        /// DataTableを使用するためWebAPIよりWindowsFormで更新処理を行う        
        /// </summary>
        /// <param name="table">M_TagCode用のDataTable</param>
        /// <param name="direct">直送の場合、True。デフォルトはFalse</param>
        /// <returns></returns>
        public (bool IsOk, string Msg) UpdateMTagCode(DataTable table,bool direct = false)
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
                    var doCode = table.Rows[0].Field<string>("DoCode");

                    // 全数発行済みのM_DelivSlipを削除
                    sql = $"SELECT A.DoCode, A.DelivNumTotal " +
                              $"FROM (SELECT DoCode, SUM(DelivNum) AS DelivNumTotal " +
                              $"FROM SANSODB.dbo.M_DelivSlip " +
                              $"GROUP BY DoCode) AS A INNER JOIN " +
                              $"(SELECT SkdCode, SUM(SkdNum) AS TagNumTotal " +
                              $"FROM SANSODB.dbo.M_TagCode " +
                              $"GROUP BY SkdCode) AS B ON A.DoCode = B.SkdCode AND A.DelivNumTotal <= B.TagNumTotal " +
                              $"WHERE (A.DoCode = '" + doCode + "')";
                    var result5 = CommonAF.ExecutSelectSQL(sql, tran, con);
                    if (result5.IsOk == false)
                    {
                        tran.Rollback();
                        return (false, "M_DelivSlip抽出時にエラーが発生しました。");
                    }

                    if (result5.Table.Rows.Count > 0)
                    {
                        sql = $"DELETE FROM SANSODB.dbo.M_DelivSlip " +
                                  $"WHERE (DoCode = '{doCode}')";
                        var result6 = CommonAF.ExecutDeleteSQL(sql, tran, con);
                        if (result6.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "全数発行済みのM_DelivSlip削除時にエラーが発生しました。");
                        }
                    }

                    // M_TagCodeのステータスを更新
                    if (direct) 
                    {
                        // 直送の場合は、完了扱いにする
                        sql = "UPDATE SANSODB.dbo.M_TagCode " +
                                  "SET StateCate = '8' " +
                                  "WHERE (TagCode = '" + table.Rows[0].Field<string>("TagCode") + "')";
                        var result7 = CommonAF.ExecutUpdateSQL(sql, tran, con);
                        if (result7.IsOk == false)
                        {
                            tran.Rollback();
                            return (false, "M_TagCodeのステータスを更新時にエラーが発生しました。");
                        }
                    }
                    else 
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            sql = "UPDATE SANSODB.dbo.M_TagCode " +
                                      "SET StateCate = '3' " +
                                      "WHERE (TagCode = '" + table.Rows[i].Field<string>("TagCode") + "')";
                            var result7 = CommonAF.ExecutUpdateSQL(sql, tran, con);
                            if (result7.IsOk == false)
                            {
                                tran.Rollback();
                                return (false, "M_TagCodeのステータスを更新時にエラーが発生しました。");
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
