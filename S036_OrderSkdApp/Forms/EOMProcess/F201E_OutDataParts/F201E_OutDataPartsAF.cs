using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SansoBase;
using C1.Win.C1Input;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 月末出庫処理（部品）
    /// </summary>
    public class F201E_OutDataPartsAF
    {

        /// <summary>
        /// 入出庫ファイル、在庫マスタ、素材在庫マスタ更新
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        public bool UpdateIOFile(List<ControlParam> controlList)
        {

            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                try
                {

                    var dataCate = controlList.SGetText("dataCateC1TextBox");
                    var partsCode = controlList.SGetText("partsCodeC1TextBox");
                    var supCode = controlList.SGetText("supCodeC1ComboBox");
                    var delivNum = controlList.SGetText("delivNumC1NumericEdit");
                    var delivDate = controlList.SGetText("delivDateC1DateEdit");
                    var groupCode = controlList.SGetText("groupCodeC1TextBox");
                    var outDataCate = controlList.SGetText("outDataCateC1ComboBox");
                    var stockCate = controlList.SGetText("stockCateC1ComboBox");
                    var userNo = LoginInfo.Instance.UserNo;
                    var machineCode = LoginInfo.Instance.MachineCode;
                    var today = DateTime.Today.ToString("yyyy/MM/dd");
                    var remarks = controlList.SGetText("remarksC1TextBox");
                    var code = controlList.SGetText("codeC1TextBox");
                    var jyuyoyosokuCode = controlList.SGetText("jyuyoyosokuCodeC1TextBox");
                    var remarksAll = controlList.SGetText("remarksAllC1TextBox");

                    var userId = LoginInfo.Instance.UserId;


                    string sql = "INSERT INTO " +
                        "入出庫ファイル " +
                        "(" +
                        "データ区分 " +
                        ",部品コード " +
                        ",仕入先コード " +
                        ",工事番号 " +
                        ",出庫数 " +
                        ",単価区分 " +
                        ",単価 " +
                        ",加工単価 " +
                        ",金額 " +
                        ",受払年月日 " +
                        ",課別コード " +
                        ",科目コード " +
                        ",伝票番号 " +
                        ",出庫区分 " +
                        ",在庫P " +
                        ",有償P " +
                        ",パスワード " +
                        ",端末名 " +
                        ",送信済みF " +
                        ",有償データF " +
                        ",実績データF " +
                        ",作成日付 " +
                        ",変更日付 " +
                        ",備考 " +
                        ",作成者 " +
                        ",作成者ID " +
                        ") " +
                        "VALUES( " +
                        "'" + dataCate + "' " +
                        ",'" + partsCode + "' " +
                        ",'" + supCode + "' " +
                        ",'' " +
                        "," + delivNum + " " +
                        ",'' " +
                        ",0 " +
                        ",0 " +
                        ",0 " +
                        ",'" + delivDate + "' " +
                        ",'" + groupCode + "' " +
                        ",'' " +
                        ",'' " +
                        ",'" + outDataCate + "' " +
                        (stockCate == " " ? ",'' " : ",'" + stockCate + "' ") +
                        ",'' " +
                        ",'" + userNo + "' " +
                        ",'" + machineCode + "' " +
                        ",'' " +
                        ",'' " +
                        ",'' " +
                        ",'" + today + "' " +
                        ",NULL " +
                        ",'" + remarksAll + "' " +
                        ",'" + userNo + "' " +
                        ",'" + userId + "' " +
                        ") ";

                    var result = CommonAF.ExecutInsertSQL(sql);
                    if (result.IsOk == false)
                    {
                        return false;
                    }

                    // 在庫マスタ更新
                    if (stockCate == "Z")
                    {
                        using (var cmd = new SqlCommand())
                        {
                            // 月末在庫マスタ更新
                            string existSql1 = "SELECT * FROM 月末在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count <= 0)
                            {
                                return false;
                            }

                            // 出庫区分がXではない時に最終出庫日を更新
                            string strX = "";
                            if (outDataCate != "X")
                            {
                                strX = ", 最終出庫日 = (CASE WHEN 最終出庫日 IS NULL THEN '" + delivDate + "' "
                                     + "WHEN 最終出庫日 < '" + delivDate + "' THEN '" + delivDate + "' "
                                     + "ELSE 最終出庫日 END) ";
                            }
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 月末在庫マスタ "
                                            + "SET 出庫数量 = ISNULL(出庫数量, 0) + " + delivNum
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + delivNum
                                            + strX
                                            + ", 変更日付 = '" + today + "' "
                                            + ", 変更者 = '" + userNo + "' "
                                            + ", 変更者ID = '" + userId + "' "
                                            + "WHERE (課別コード = '" + groupCode + "') "
                                            + "AND (部品コード = '" + partsCode + "') ";

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();

                            // 在庫マスタ更新
                            string existSql2 = "SELECT * FROM 在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult2 = CommonAF.ExecutSelectSQL(existSql2);
                            if (existResult2.Table.Rows.Count <= 0)
                            {
                                return false;
                            }

                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 在庫マスタ "
                                            + "SET 前残数量 = ISNULL(前残数量, 0) - " + delivNum
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + delivNum
                                            + strX
                                            + ", 変更日付 = '" + today + "' "
                                            + ", 変更者 = '" + userNo + "' "
                                            + ", 変更者ID = '" + userId + "' "
                                            + "WHERE (課別コード = '" + groupCode + "') "
                                            + "AND (部品コード = '" + partsCode + "') ";

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();

                        }

                    }
                    // 素材在庫マスタ更新
                    else
                    {
                        using (var cmd = new SqlCommand())
                        {
                            // 月末素材在庫マスタ更新
                            string existSql1 = "SELECT * FROM 月末素材在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count <= 0)
                            {
                                return false;
                            }

                            // 出庫区分がXではない時に最終出庫日を更新
                            string strX = "";
                            if (outDataCate != "X")
                            {
                                strX = ", 最終出庫日 = (CASE WHEN 最終出庫日 IS NULL THEN '" + delivDate + "' "
                                     + "WHEN 最終出庫日 < '" + delivDate + "' THEN '" + delivDate + "' "
                                     + "ELSE 最終出庫日 END) ";
                            }
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 月末素材在庫マスタ "
                                            + "SET 出庫数量 = ISNULL(出庫数量, 0) + " + delivNum
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + delivNum
                                            + strX
                                            + ", 変更日付 = '" + today + "' "
                                            + "WHERE (課別コード = '" + groupCode + "') "
                                            + "AND (部品コード = '" + partsCode + "') ";

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();


                            // 素材在庫マスタ更新
                            string existSql2 = "SELECT * FROM 素材在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult2 = CommonAF.ExecutSelectSQL(existSql2);
                            if (existResult2.Table.Rows.Count <= 0)
                            {
                                return false;
                            }
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 素材在庫マスタ "
                                            + "SET 前残数量 = ISNULL(前残数量, 0) - " + delivNum
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + delivNum
                                            + strX
                                            + ", 変更日付 = '" + today + "' "
                                            + "WHERE (課別コード = '" + groupCode + "') "
                                            + "AND (部品コード = '" + partsCode + "') ";

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();


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

                return true;

            }

        }


        /// <summary>
        /// 在庫情報取得
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetStockInfo(List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var groupCode = controlList.SGetText("groupCodeC1TextBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");

            string sql = "";
            if (stockCate == "Z")
            {
                sql =
                    "SELECT * FROM 在庫マスタ " +
                    "WHERE 部品コード = '" + partsCode + "' " +
                    " AND 課別コード = '" + groupCode + "' ";
            }
            else
            {
                sql =
                    "SELECT * FROM 素材在庫マスタ " +
                    "WHERE 部品コード = '" + partsCode + "' " +
                    " AND 課別コード = '" + groupCode + "' ";
            }

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// 月末在庫情報取得
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetEOMStockInfo(List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var groupCode = controlList.SGetText("groupCodeC1TextBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");

            string sql = "";
            if (stockCate == "Z")
            {
                sql =
                    "SELECT * FROM 月末在庫マスタ " +
                    "WHERE 部品コード = '" + partsCode + "' " +
                    " AND 課別コード = '" + groupCode + "' ";
            }
            else
            {
                sql =
                    "SELECT * FROM 月末素材在庫マスタ " +
                    "WHERE 部品コード = '" + partsCode + "' " +
                    " AND 課別コード = '" + groupCode + "' ";
            }

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// S1製造直接部門　取得
        /// </summary>
        /// <param name="s">作成者ID</param>
        public (bool IsOk, DataTable Table) GetGroupManufactDirect(string s)
        {
            string sql =
            "SELECT " +
            "B.部門コード " +
            ", B.部門名 " +
            "FROM " +
            "三相メイン.dbo.M_SystemUsingGroup AS A " +
            ", 三相メイン.dbo.部門マスタ AS B " +
            "WHERE " +
            "A.STAFFID = '" + s + "' " +
            "AND B.有効部門区分 = '1' AND B.製販区分 = 'S1' " +
            "AND A.GROUPCODE = B.部門コード " +
            "ORDER BY GROUPCODE ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }


        /// <summary>
        /// 月末処理の処理年月　取得
        /// </summary>
        /// <param name="s">部門コード</param>
        public (bool IsOk, DataTable Table) GetEOMExecuteDate(string s)
        {
            string sql =
            "SELECT " +
            "FORMAT(DATEADD(MM, -1, 処理年月), 'yyyy/MM') AS 月末処理年月 " +
            "FROM " +
            "製造調達.dbo.部門マスタ " +
            "WHERE " +
            "部門コード = '" + s + "' ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

    }
}
