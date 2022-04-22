using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using C1.Win.C1Input;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 単価マスタメンテナンス
    /// </summary>
    public class F601_UintPriceMasterMaintAF
    {
        /// <summary>
        /// 単価マスタ　新規更新
        /// </summary>
        /// <param name="controlListII">controlListII</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool UpdateUnitPriceMST(List<ControlParam> controlListII)
        {
            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    var date = DateTime.Now;

                    var partsCode = controlListII.SGetText("partsCodeC1TextBox");
                    var supCode = controlListII.SGetText("supCodeC1TextBox");
                    var materialPrice = controlListII.SGetText("materialPriceC1NumericEdit").Replace(",", "");
                    var processPrice = controlListII.SGetText("processPriceC1NumericEdit").Replace(",", "");
                    var transUnitPrice = controlListII.SGetText("transUnitPriceC1NumericEdit").Replace(",", "");
                    var lot = controlListII.SGetText("lotC1NumericEdit").Replace(",", "");
                    var applicationCode = controlListII.SGetText("applicationCodeC1TextBox");

                    // COSTDB.dbo.CM_PARTS 存在チェック
                    string sql1 = $"SELECT * FROM COSTDB.dbo.CM_PARTS WHERE (BUHIN_CD = '{partsCode}') ";
                    var result1 = CommonAF.ExecutSelectSQL(sql1);
                    if (result1.IsOk == false)
                    {
                        tran.Rollback();
                        return false;
                    }

                    // COSTDB.dbo.CM_PARTSに部品コードが存在しない場合、部品コードを追加する
                    if (result1.Table.Rows.Count <= 0)
                    {
                        string sql2 =
                            "INSERT INTO COSTDB.dbo.CM_PARTS " +
                            "(BUHIN_CD, BUHIN_NM, ZUMEN_CD, WEIGHT, MATERIAL, DIMENSION, DRESSED, BUHIN_KEY_CD, CRT_DATE " +
                            ", CRTSYAIN_CD) " +
                            "SELECT 部品マスタ.部品コード, 部品マスタ.部品名, 部品マスタ.図面番号, 部品マスタ_1.重量" +
                            ", 部品マスタ_1.材質, 部品マスタ_1.寸度, 部品マスタ_1.仕上 " +
                            ", (SELECT MAX(BUHIN_KEY_CD) AS BKC FROM COSTDB.dbo.CM_PARTS) + 1 AS BKC " +
                            ", '" + date + "', '" + LoginInfo.Instance.UserNo + "'" +
                            "FROM 製造調達.dbo.部品マスタ AS A " +
                            "LEFT OUTER JOIN 三相メイン.dbo.部品マスタ AS B " +
                            "ON A.部品コード = B.部品コード " +
                            "WHERE (A.部品コード = '" + partsCode + "') ";

                        var result2 = CommonAF.ExecutInsertSQL(sql2, tran, con);
                        if (result2.IsOk == false)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }

                    // 単価マスタ（COSTDB.dbo.CT_PRICE）存在チェック
                    string sql3 = $"SELECT * FROM COSTDB.dbo.CT_PRICE " +
                        $"WHERE (BUHIN_CD = '{partsCode}') AND (PROCESS_CD = '{supCode}')";
                    var result3 = CommonAF.ExecutSelectSQL(sql3);
                    if (result3.IsOk == false)
                    {
                        tran.Rollback();
                        return false;
                    }

                    decimal materialPriceNum = 0;
                    decimal.TryParse(materialPrice, out materialPriceNum);
                    decimal processPriceNum = 0;
                    decimal.TryParse(processPrice, out processPriceNum);
                    decimal transUnitPriceNum = 0;
                    decimal.TryParse(transUnitPrice, out transUnitPriceNum);
                    decimal sum = materialPriceNum + processPriceNum + transUnitPriceNum;

                    // COSTDB.dbo.CT_PRICEにデータが存在しなかったら
                    if (result3.Table.Rows.Count <= 0)
                    {
                        string sql4 =
                            "INSERT INTO COSTDB.dbo.CT_PRICE " +
                            "(BUHIN_CD, BUHIN_KEY_CD, PRIORITY, PROCESS_CD, SHINSEISYO_NO, MATERIAL_COST, WORK_COST, " +
                            "SUPPLY_COST, CRTSYAIN_CD, CRT_DATE, LOT_SU, SALE_PRICE) " +
                            "SELECT " +
                            "A.BUHIN_CD, " +
                            "A.BUHIN_KEY_CD, " +
                            "1, " +
                            "'" + supCode + "', " +
                            "'" + applicationCode + "', " +
                            (materialPrice == "" ? "0, " : materialPrice + ", ") +
                            (processPrice == "" ? "0, " : processPrice + ", ") +
                            (transUnitPrice == "" ? "0, " : transUnitPrice + ", ") +
                            "'" + LoginInfo.Instance.UserNo + "', " +
                            "'" + date + "', " +
                            (lot == "" ? "0, " : lot + ", ") +
                            "'" + sum.ToString() + "' " +
                            "FROM COSTDB.dbo.CM_PARTS AS A " +
                            "WHERE (A.BUHIN_CD ='" + partsCode + "') ";

                        var result4 = CommonAF.ExecutInsertSQL(sql4, tran, con);
                        if (result4.IsOk == false)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }

                    // COSTDB.dbo.CT_PRICEにデータが存在したら
                    else
                    {
                        // 仕入単価等を旧仕入単価等にセットする
                        string sql5 =
                                "UPDATE COSTDB.dbo.CT_PRICE SET " +
                                "O_MATERIAL_COST = MATERIAL_COST, " +
                                "O_OUT_MATERIAL_COST = OUT_MATERIAL_COST, " +
                                "O_SUPPLY_COST = SUPPLY_COST, " +
                                "O_WORK_COST = WORK_COST, " +
                                "O_WORK_TIME = WORK_TIME, " +
                                "O_OUT_WORK_COST = OUT_WORK_COST, " +
                                "O_ADJUSTING = ADJUSTING, " +
                                "O_DEPRECIATION = DEPRECIATION " +
                                "WHERE(BUHIN_CD = '" + partsCode + "') AND (PROCESS_CD = '" + supCode + "') ";

                        var result5 = CommonAF.ExecutUpdateSQL(sql5, tran, con);
                        if (result5.IsOk == false)
                        {
                            tran.Rollback();
                            return false;
                        }

                        string sql6 =
                                "UPDATE COSTDB.dbo.CT_PRICE SET " +
                                "STATUS = 0, PRIORITY = 1, " +
                                "MATERIAL_COST = " + (materialPrice == "" ? "0, " : materialPrice + ", ") +
                                "SALE_PRICE = " + sum.ToString() + ", " +
                                "OUT_MATERIAL_COST = 0, " +
                                "SUPPLY_COST = " + (transUnitPrice == "" ? "0, " : transUnitPrice + ", ") +
                                "WORK_COST = 0, " +
                                "WORK_TIME = 0, " +
                                "OUT_WORK_COST = " + (processPrice == "" ? "0, " : processPrice + ", ") +
                                "ADJUSTING = 0, " +
                                "DEPRECIATION = 0, " +
                                "UPD_DATE = '" + date + "', " +
                                "UPDSYAIN_CD = '" + LoginInfo.Instance.UserNo + "', " +
                                "SHINSEISYO_NO = '" + applicationCode + "', " +
                                "LOT_SU = " + (lot == "" ? "0 " : lot + " ") +
                                "WHERE (BUHIN_CD = '" + partsCode + "') AND (PROCESS_CD = '" + supCode + "') ";

                        var result6 = CommonAF.ExecutUpdateSQL(sql6, tran, con);
                        if (result6.IsOk == false)
                        {
                            tran.Rollback();
                            return false;
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
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
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 単価マスタ　削除
        /// </summary>
        /// <param name="controlListII">controlListII</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool DeleteUnitPriceMST(List<ControlParam> controlListII)
        {
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");
            var supCode = controlListII.SGetText("supCodeC1TextBox");

            string sql = "DELETE FROM COSTDB.dbo.CT_PRICE " +
            "WHERE (BUHIN_CD = '" + partsCode + "') AND (PROCESS_CD = '" + supCode + "')";

            var result = CommonAF.ExecutDeleteSQL(sql);
            if (result.IsOk == false)
            {
                return false;
            }
            return true;
        }
    }
}
