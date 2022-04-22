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

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 納入予定一覧
    /// </summary>
    class F002_DelivScheduleListAF
    {
        /// <summary>
        /// 納入予定一覧を取得
        /// </summary>
        /// <param name="date1">当日－２</param>
        /// <param name="date2">当日－１</param>
        /// <param name="date3">当日</param>
        /// <param name="date4">当日＋１</param>
        /// <param name="date5">当日＋２</param>
        /// <param name="date6">当日＋３</param>
        /// <param name="date7">当日＋４</param>
        /// <returns>納入予定一覧</returns>
        public (bool IsOk, DataTable Table) GetDelivScheduleList(DateTime date1, DateTime date2, DateTime date3, DateTime date4,
            DateTime date5, DateTime date6, DateTime date7)
        {
            var sql = "SELECT " +
                    "部門コード, " +
                    "MAX(部門名) AS 部門名, " +

                    // 日付1
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date1 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date1 + "' THEN 件数予定 ELSE 0 END)) AS 件数1, " +
                    "SUM(CASE WHEN 日付 = '" + date1 + "' THEN 件数率 ELSE 0 END) AS 件数率1, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date1 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date1 + "' THEN 数量予定 ELSE 0 END))) AS 数量1, " +
                    "SUM(CASE WHEN 日付 = '" + date1 + "' THEN 数量率 ELSE 0 END) AS 数量率1, " +

                    // 日付2
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date2 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date2 + "' THEN 件数予定 ELSE 0 END)) AS 件数2, " +
                    "SUM(CASE WHEN 日付 = '" + date2 + "' THEN 件数率 ELSE 0 END) AS 件数率2, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date2 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date2 + "' THEN 数量予定 ELSE 0 END))) AS 数量2, " +
                    "SUM(CASE WHEN 日付 = '" + date2 + "' THEN 数量率 ELSE 0 END) AS 数量率2, " +

                    // 日付3
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date3 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date3 + "' THEN 件数予定 ELSE 0 END)) AS 件数3, " +
                    "SUM(CASE WHEN 日付 = '" + date3 + "' THEN 件数率 ELSE 0 END) AS 件数率3, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date3 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date3 + "' THEN 数量予定 ELSE 0 END))) AS 数量3, " +
                    "SUM(CASE WHEN 日付 = '" + date3 + "' THEN 数量率 ELSE 0 END) AS 数量率3, " +

                    // 日付4
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date4 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date4 + "' THEN 件数予定 ELSE 0 END)) AS 件数4, " +
                    "SUM(CASE WHEN 日付 = '" + date4 + "' THEN 件数率 ELSE 0 END) AS 件数率4, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date4 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date4 + "' THEN 数量予定 ELSE 0 END))) AS 数量4, " +
                    "SUM(CASE WHEN 日付 = '" + date4 + "' THEN 数量率 ELSE 0 END) AS 数量率4, " +

                    // 日付5
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date5 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date5 + "' THEN 件数予定 ELSE 0 END)) AS 件数5, " +
                    "SUM(CASE WHEN 日付 = '" + date5 + "' THEN 件数率 ELSE 0 END) AS 件数率5, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date5 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date5 + "' THEN 数量予定 ELSE 0 END))) AS 数量5, " +
                    "SUM(CASE WHEN 日付 = '" + date5 + "' THEN 数量率 ELSE 0 END) AS 数量率5, " +

                    // 日付6
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date6 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date6 + "' THEN 件数予定 ELSE 0 END)) AS 件数6, " +
                    "SUM(CASE WHEN 日付 = '" + date6 + "' THEN 件数率 ELSE 0 END) AS 件数率6, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date6 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date6 + "' THEN 数量予定 ELSE 0 END))) AS 数量6, " +
                    "SUM(CASE WHEN 日付 = '" + date6 + "' THEN 数量率 ELSE 0 END) AS 数量率6, " +

                    // 日付7
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date7 + "' THEN 件数 ELSE 0 END)) +'/' + " +
                    "CONVERT(nvarchar, SUM(CASE WHEN 日付 = '" + date7 + "' THEN 件数予定 ELSE 0 END)) AS 件数7, " +
                    "SUM(CASE WHEN 日付 = '" + date7 + "' THEN 件数率 ELSE 0 END) AS 件数率7, " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date7 + "' THEN 数量 ELSE 0 END))) +'/' + " +
                    "CONVERT(nvarchar, CONVERT(float, SUM(CASE WHEN 日付 = '" + date7 + "' THEN 数量予定 ELSE 0 END))) AS 数量7, " +
                    "SUM(CASE WHEN 日付 = '" + date7 + "' THEN 数量率 ELSE 0 END) AS 数量率7, " +
                    "MAX(表示順) AS 表示順, " +
                    "ROW_NUMBER() OVER(ORDER BY MAX(表示順)) 行番 " +
                    "FROM " +
                    "( " +
                    "( " +
                    "SELECT 部門コード, 係名 AS 部門名, NULL AS 日付, 0 AS 件数, 0 AS 件数予定, 0 AS 件数率, 0 AS 数量, 0 AS 数量予定, 0 AS 数量率, 表示順 " +
                    "FROM 三相メイン.dbo.部門マスタ " +
                    "WHERE " +
                    "(ISNULL(製販区分, '') = 'S1' AND ISNULL(部門区分, '') <> '') " +
                    "OR 部門コード = '3510' " +
                    ") " +
                    "UNION ALL " +
                    "(" +
                    "SELECT AA.部門コード, AA.部門名, AA.日付, AA.件数, AA.件数予定, AA.件数率, AA.数量, AA.数量予定, AA.数量率, AA.表示順 " +
                    "FROM(" +
                    "SELECT " +
                    "B.部門コード, " +
                    "'' AS 部門名, " +
                    "A.DELIVERY_DATE AS 日付, " +
                    "SUM((CASE WHEN A.STATUS = 2 THEN 1 ELSE 0 END)) AS 件数, " +
                    "COUNT(A.DO_NO) AS 件数予定, " +
                    "ROUND((SUM((CASE WHEN A.STATUS = 2 THEN 1 ELSE 0 END)) / CONVERT(float, COUNT(A.DO_NO))) *100, 2) AS 件数率, " +
                    "SUM((CASE WHEN A.STATUS = 2 THEN ISNULL(A.DELIVERY_QTY, 0) ELSE 0 END)) AS 数量, " +
                    "SUM(ISNULL(A.DELIVERY_QTY, 0)) AS 数量予定, " +
                    "ROUND((SUM((CASE WHEN A.STATUS = 2 THEN ISNULL(A.DELIVERY_QTY, 0) ELSE 0 END)) / CONVERT(float, SUM(ISNULL(A.DELIVERY_QTY, 0)))) *100, 2) AS 数量率, " +
                    "NULL AS 表示順 " +
                    "FROM " +
                    "(" +
                    "SELECT * " +
                    "FROM EDIDATA.dbo.T_SHIPMENT_DATA " +
                    "WHERE DELIVERY_DATE BETWEEN '" + date1 + "' AND '" + date7 + "' " +
                    ") AS A " +
                    "LEFT OUTER JOIN " +
                    "(SELECT ISNULL(PO_NO, '') AS PO_NO, ISNULL(FIELD11, '') AS 部門コード FROM EDIDATA.dbo.T_SD01 GROUP BY ISNULL(PO_NO, ''), ISNULL(FIELD11, '')) AS B " +
                    "ON A.PO_NO = B.PO_NO " +
                    "GROUP BY B.部門コード, A.DELIVERY_DATE " +
                    ") AS AA " +
                    "LEFT OUTER JOIN 三相メイン.dbo.部門マスタ AS BB ON AA.部門コード = BB.部門コード " +
                    "WHERE(ISNULL(BB.製販区分, '') = 'S1' AND ISNULL(BB.部門区分, '') <> '') " +
                    "OR BB.部門コード = '3510' " +
                    ") " +
                    ") AS AAA " +
                    "GROUP BY " +
                    "部門コード " +
                    "ORDER BY 表示順 ";
            var result = CommonAF.ExecutSelectSQL(sql);
            if (result.IsOk == false)
            {
                return (false, null);
            }
            return (true, result.Table);
        }
    }
}