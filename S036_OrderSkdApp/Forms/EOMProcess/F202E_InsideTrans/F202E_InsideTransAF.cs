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
    /// 月末社内移行発行
    /// </summary>
    public class F202E_InsideTransAF
    {
        /*
        /// <summary>
        /// 仕入先マスタ、入出庫ファイル、在庫マスタ、素材在庫マスタ、部品手配マスタ更新
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
                    var unitPrice = controlList.SGetText("unitPriceC1NumericEdit");
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


                    // 入出庫ファイル更新
                    string machineSend = "";
                    if (dataCate == "8" && supCode == "3630")
                    {
                        machineSend = "1";
                    }

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
                        ",機械送信F " +
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
                        ",'" + machineSend + "' " +
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
                            // 月末在庫マスタ更新
                            string existSql1 = "SELECT * FROM 月末在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count <= 0)
                            {
                                return false;

                            }

                            string strX = ", 最終出庫日 = (CASE WHEN 最終出庫日 IS NULL THEN '" + outDate + "' "
                                        + "WHEN 最終出庫日 < '" + outDate + "' THEN '" + outDate + "' "
                                        + "ELSE 最終出庫日 END) ";
                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 月末在庫マスタ "
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
                                            + "SET 前残数量 = ISNULL(前残数量, 0) - " + outNum
                                            + ", 前残金額 = ISNULL(前残金額, 0) - " + outPrice
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
                            // 月末素材在庫マスタ更新
                            string existSql1 = "SELECT * FROM 月末素材在庫マスタ " +
                                              "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult1 = CommonAF.ExecutSelectSQL(existSql1);
                            if (existResult1.Table.Rows.Count <= 0)
                            {
                                return false;
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


                            // 素材在庫マスタ更新
                            string existSql2 = "SELECT * FROM 素材在庫マスタ " +
                                               "WHERE 課別コード = '" + groupCode + "' AND 部品コード = '" + partsCode + "'";
                            var existResult2 = CommonAF.ExecutSelectSQL(existSql2);
                            if (existResult2.Table.Rows.Count <= 0)
                            {
                                return false;
                            }

                            cmd.Connection = con;
                            cmd.CommandText = "UPDATE 在庫マスタ "
                                            + "SET 前残数量 = ISNULL(前残数量, 0) - " + outNum
                                            + ", 前残金額 = ISNULL(前残金額, 0) - " + outPrice
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

        /// <summary>
        /// 月末処理の処理年月　取得
        /// </summary>
        /// <param name="s">部門コード</param>
        public (bool IsOk, DataTable Table) GetExecuteDate(string s)
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

            /*
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("groupCode", new JValue(s));

            var result = ApiCommonGet(apiUrl + "GetExecuteDate", apiParam);

            return (result.IsOk, result.Table);
            */
        }

        /// <summary>
        /// 入出工程単価情報取得
        /// </summary>
        /// <param name="s">部品コード</param>
        public (bool IsOk, DataTable Table) GetGridViewData(string s)
        {
            string sql =
            "SELECT " +
            "dbo.工程マスタ.工程番号 AS 工程番号 " +
            ",dbo.工程マスタ.仕入先コード AS 仕入先コード " +
            ",dbo.仕入先マスタ.仕入先名１ AS 仕入先名 " +
            ",dbo.単価マスタ.単価区分 AS 単価区分 " +
            ",dbo.単価マスタ.仕入単価 AS 仕入単価 " +
            ",dbo.単価マスタ.材料費 AS 材料費 " +
            ",dbo.単価マスタ.加工費 AS 加工費 " +
            ",dbo.単価マスタ.支給単価 AS 支給単価 " +
            ",dbo.工程マスタ.在庫Ｐ AS 在庫Ｐ " +
            "FROM " +
            "dbo.工程マスタ " +
            "INNER JOIN dbo.単価マスタ " +
            "ON dbo.工程マスタ.[部品コード] = dbo.単価マスタ.[部品コード] " +
            "AND dbo.工程マスタ.[仕入先コード] = dbo.単価マスタ.[仕入先コード] " +
            "LEFT OUTER JOIN dbo.仕入先マスタ " +
            "ON dbo.工程マスタ.[仕入先コード] = dbo.仕入先マスタ.[仕入先コード] " +
            "WHERE (dbo.工程マスタ.[部品コード] = '" + s + "') " +
            "ORDER BY dbo.工程マスタ.工程番号 ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);

        }


    }
}
