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
    /// 社内移行発行
    /// </summary>
    public class F202_InsideTransAF
    {
        /*
        /// <summary>
        /// 仕入先マスタ、変数表、入出庫ファイル、在庫マスタ、素材在庫マスタ、部品手配マスタ更新
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        /// <param name="columnName">仕入先マスタ更新項目名</param>
        /// <param name="doCodeValue">仕入先マスタ更新値</param>
        /// <param name="doCodeCheck">伝票番号手動入力判定</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        public bool UpdateIOFile(List<ControlParam> controlList, string columnName, double doCodeValue, bool doCodeCheck)
        {

            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    var partsCode = controlList.SGetText("partsCodeC1TextBox");
                    var jyuyoyosokuCode = controlList.SGetText("jyuyoyosokuCodeC1TextBox");
                    var poCode = controlList.SGetText("poCodeC1TextBox");
                    var supCode = controlList.SGetText("supCodeC1TextBox");
                    var outNum = controlList.SGetText("outNumC1NumericEdit");
                    var boxNum = controlList.SGetText("boxNumC1NumericEdit");
                    var unitPrice = controlList.SGetText("unitPriceC1NumericEdit");
                    var sumOut = controlList.SGetText("sumOutNumC1TextBox");
                    var outPrice = controlList.SGetText("outPriceC1TextBox");
                    var outDate = controlList.SGetText("outDateC1DateEdit");
                    var doCode = controlList.SGetText("doCodeC1TextBox");
                    var groupCode = controlList.SGetText("groupCodeC1ComboBox");
                    var dataCate = controlList.SGetText("dataCateC1TextBox");
                    var outDataCate = controlList.SGetText("outDataCateC1TextBox");
                    var stockCate = controlList.SGetText("stockCateC1ComboBox");
                    var supCate = controlList.SGetText("supCateC1TextBox");
                    var ordRemainNum = controlList.SGetText("ordRemainNumC1TextBox"); 
                    var userNo = LoginInfo.Instance.UserNo;
                    var machineCode = LoginInfo.Instance.MachineCode;
                    var dateNow = DateTime.Today;
                    var today = dateNow.ToString("yyyy/MM/dd");
                    var userId = LoginInfo.Instance.UserId;

                    // 仕入先マスタ更新
                    if (doCodeCheck)
                    {
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 製造調達.dbo.仕入先マスタ "
                                            + " SET " + columnName + " = " + doCodeValue.ToString()
                                            + " WHERE 仕入先コード = '9998'";

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();
                        }

                    }

                    // 変数表更新
                    string strDEN_NO = "";
                    using (var cmd = new SqlCommand())
                    {
                        string existSql1 = 
                        "SELECT " +
                        "内容２num " +
                        ",内容１chr " +
                        "FROM " +
                        "三相メイン.dbo.変数表 " +
                        "WHERE " +
                        "区分 = 'GP" + groupCode + "' ";

                        var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                        if (existResult1.Table.Rows.Count >= 1)
                        {
                            var v = existResult1.Table.Rows[0]["内容２num"].ToString();
                            double d = v == "" ? 0 : System.Convert.ToDouble(v);
                            if (d < 99999)
                            {
                                d = d + 1;
                            }
                            else
                            {
                                d = 0;
                            }

                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 三相メイン.dbo.変数表 " +
                                              "SET 内容２num = " + d.ToString() +
                                              "WHERE " +
                                              "区分 = 'GP" + groupCode + "' ";

                            string s = existResult1.Table.Rows[0]["内容２num"].ToString();
                            double d1 = s == "" ? 0 : System.Convert.ToDouble(s);
                            string ss = d1.ToString("00000");
                            strDEN_NO = existResult1.Table.Rows[0]["内容１chr"].ToString()
                                      + dateNow.ToString("yyMM")
                                      + ss;

                            if (tran != null)
                            {
                                cmd.Transaction = tran;
                            }
                            cmd.ExecuteNonQuery();
                            
                        }
                        
                    }

                    // 入出庫ファイル更新
                    string sql = "INSERT INTO " +
                        "入出庫ファイル " +
                        "(" +
                        "データ区分 " +
                        ",部品コード " +
                        ",仕入先コード " +
                        ",工事番号 " +
                        ",注文番号" +
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
                        ",社内移行データF " +
                        ",現品票番号 " + 
                        ",DEN_NO " +
                        ",作成日付 " +
                        ",変更日付 " +
                        ",作成者 " +
                        ",作成者ID " +
                        ") " +
                        "VALUES( " +
                        "'" + dataCate + "' " +
                        ",'" + partsCode + "' " +
                        ",'" + supCode + "' " +
                        ",'" + jyuyoyosokuCode + "' " +
                        ",'" + poCode + "' " +
                        "," + outNum + " " +
                        ",'' " +
                        "," + unitPrice + " " +
                        ",NULL " +
                        "," + outPrice + " " + 
                        ",'" + outDate + "' " +
                        ",'" + groupCode + "' " +
                        ",'' " +
                        ",'" + doCode + "' " +
                        ",'' " +
                        (stockCate == " " ? ",'' " : ",'" + stockCate + "' ") +
                        ",'' " +
                        ",'" + userNo + "' " +
                        ",'" + machineCode + "' " +
                        ",'' " +
                        ",'' " +
                        ",'' " +
                        (doCodeCheck == true ? ",'' " : ",'M' ") + 
                        ",'' " +
                        ",'" + strDEN_NO + "' " +
                        ",'" + today + "' " +
                        ",NULL " +
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
                            string existSql1 = "SELECT * FROM 在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count <= 0)
                            {
                                string insertSql1 = "INSERT INTO 在庫マスタ(部品コード, 課別コード,前残数量,前残金額,入庫数量 " +
                                                   ", 入庫金額, 出庫数量, 出庫金額, 当残数量, 当残金額 " +
                                                   ", 作成日付, 作成者, 作成者ID) " +
                                                   " VALUES('" + partsCode + "', '" + groupCode + "', 0, 0, 0" +
                                                   ", 0, 0, 0, 0, 0" +
                                                   ", '" + today + "', '" + userNo + "', '" + userId + "')";

                                var insertResult1 = CommonAF.ExecutInsertSQL(insertSql1);
                                if (insertResult1.IsOk == false)
                                {
                                    return false;
                                }
                            }


                            string strX = ", 最終出庫日 = (CASE WHEN 最終出庫日 IS NULL THEN '" + outDate + "' "
                                        + "WHEN 最終出庫日 < '" + outDate + "' THEN '" + outDate + "' "
                                        + "ELSE 最終出庫日 END) ";
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 在庫マスタ "
                                            + "SET 出庫数量 = ISNULL(出庫数量, 0) + " + outNum
                                            + ", 出庫金額 = ISNULL(出庫金額, 0) + " + outPrice
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + outNum
                                            + ", 当残金額 = ISNULL(当残金額, 0) - " + outPrice
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
                            string existSql2 = "SELECT * FROM 素材在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult2 = CommonAF.ExecutSelectSQL(existSql2);
                            if (existResult2.Table.Rows.Count <= 0)
                            {
                                string insertSql2 = "INSERT INTO 素材在庫マスタ(部品コード, 課別コード, 前残数量, 前残金額" +
                                                   ", 入庫数量, 入庫金額, 出庫数量, 出庫金額, 当残数量, 当残金額, 作成日付) " +
                                                   " VALUES('" + partsCode + "', '" + groupCode + "', 0, 0" +
                                                   ", 0, 0, 0, 0, 0, 0, '" + today + "')";

                                var insertResult2 = CommonAF.ExecutInsertSQL(insertSql2);
                                if (insertResult2.IsOk == false)
                                {
                                    return false;
                                }
                            }

                            string strX = ", 最終出庫日 = (CASE WHEN 最終出庫日 IS NULL THEN '" + outDate + "' "
                                        + "WHEN 最終出庫日 < '" + outDate + "' THEN '" + outDate + "' "
                                        + "ELSE 最終出庫日 END) ";
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 素材在庫マスタ "
                                            + "SET 出庫数量 = ISNULL(出庫数量, 0) + " + outNum
                                            + ", 出庫金額 = ISNULL(出庫金額, 0) + " + outPrice
                                            + ", 当残数量 = ISNULL(当残数量, 0) - " + outNum
                                            + ", 当残金額 = ISNULL(当残金額, 0) - " + outPrice
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

                    // 部品手配マスタ更新
                    if (supCode == "2930" && poCode != "")
                    {
                        using (var cmd = new SqlCommand())
                        {
                            string existSql1 =
                            "SELECT " +
                            "* " +
                            "FROM " +
                            "Ｖ営業支援＿部品手配マスタ " +
                            "WHERE " +
                            "注文番号 = '" + poCode + "' " +
                            "AND 部品コード = '" + partsCode + "' ";

                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count >= 1)
                            {
                                var v = existResult1.Table.Rows[0]["依頼先納入数"].ToString();
                                var v2 = outNum;
                                double d1 = v == "" ? 0 : System.Convert.ToDouble(v);
                                double d2 = v2 == "" ? 0 : System.Convert.ToDouble(v2);
                                double d = d1 + d2;

                                cmd.Connection = con;
                                cmd.CommandText = "UPDATE Ｖ営業支援＿部品手配マスタ " +
                                                  "SET 依頼先納入数 = " + d.ToString() +
                                                  "WHERE " +
                                                  "注文番号 = '" + poCode + "' " +
                                                  "AND 部品コード = '" + partsCode + "' ";
                                if (tran != null)
                                {
                                    cmd.Transaction = tran;
                                }
                                cmd.ExecuteNonQuery();

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

                return true;

            }

        }

    */

    }
}
