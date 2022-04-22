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
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 月末有償支給発行（一般）
    /// </summary>
    public class F204E_NSupStockTransAF
    {
        /// <summary>
        ///  製造調達.dbo.部門マスタの処理年月　取得
        /// </summary>
        /// <param name="gruopCode">部門コード</param>
        public (bool IsOk, DataTable Table) GetExecuteDate(string gruopCode)
        {

            string sql =
            "SELECT " +
            "FORMAT(処理年月, 'yyyy/MM') AS 処理年月 " +
            "FROM " +
            "製造調達.dbo.部門マスタ " +
            "WHERE " +
            "部門コード = '" + gruopCode + "' ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);


            /*
            apiParam.RemoveAll();
            apiParam.Add("gruopCode", new JValue(gruopCode));

            var result = ApiCommonGet(apiUrl + "GetExecuteDate", apiParam);

            return (result.IsOk, result.Table);
            */
        }

        /// <summary>
        /// 入出工程単価情報取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        public (bool IsOk, DataTable Table) GetGridViewData(string partsCode)
        {
            string sql =
            "SELECT A.工程番号, A.仕入先コード, C.仕入先名１ AS 仕入先名, " +
            "B.単価区分, B.仕入単価, B.材料費, B.加工費, B.支給単価, A.在庫Ｐ " +
            "FROM 製造調達.dbo.工程マスタ AS A " +
            "INNER JOIN 製造調達.dbo.単価マスタ AS B ON A.部品コード = B.部品コード AND A.仕入先コード = B.仕入先コード " +
            "LEFT OUTER JOIN 製造調達.dbo.仕入先マスタ AS C ON A.仕入先コード = C.仕入先コード " +
           $"WHERE (A.部品コード = '{partsCode}') " +
            "ORDER BY A.工程番号";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// レポート情報（入出有償支給発行一般）取得
        /// </summary>
        /// <param name="groupCode">課別コード</param>
        /// <param name="doCode">伝票番号</param>
        /// <returns></returns>
        public (bool IsOk, DataTable Table) GetIONSupStockTrans(string groupCode, string doCode)
        {
            string sql =
            "SELECT " +
            "入出庫ファイル.課別コード, 仕入先マスタ_1.仕入先名１ AS 課別名, 入出庫ファイル.仕入先コード, " +
            "仕入先マスタ_2.仕入先名１, 部品マスタ.部品名, 入出庫ファイル.工事番号, 入出庫ファイル.注文番号, " +
            "入出庫ファイル.部品コード, 入出庫ファイル.入庫数 * -1 AS 入庫数, 入出庫ファイル.単価, " +
            "入出庫ファイル.金額 * -1 AS 金額, 入出庫ファイル.受払年月日, 入出庫ファイル.伝票番号, " +
            "部品マスタ.図面番号, 入出庫ファイル.有償支給データＦ,  " +
            "CASE 入出庫ファイル.仕入先コード " +
            "    WHEN '4170' THEN " +
            "        CASE ISNULL(入出庫ファイル.備考, '') " +
            "            WHEN 'Z' THEN '材料不良返品有償支給' + CHAR(13) + CHAR(10) + ISNULL(部品マスタ.備考, '') " +
            "            WHEN 'O' THEN '加工応援素材返品有償支給' + CHAR(13) + CHAR(10) + ISNULL(部品マスタ.備考, '') " +
            "            WHEN '' THEN ISNULL(部品マスタ.備考,'') " +
            "            ELSE ISNULL(入出庫ファイル.備考,'') +CHAR(13) + CHAR(10) + ISNULL(部品マスタ.備考, '') " +
            "       END " +
            "    ELSE " +
            "        CASE ISNULL(入出庫ファイル.備考, '') " +
            "        WHEN 'Z' THEN '材料不良返品有償支給' " +
            "        WHEN  'O' THEN '加工応援素材返品有償支給' " +
            "        ELSE 入出庫ファイル.備考 " +
            "    END " +
            "END AS 備考 " +
            ",CASE 入出庫ファイル.仕入先コード " +
            "    WHEN '4170' THEN ISNULL(部品マスタ.備考,'') " +
            "    ELSE '' " +
            "END AS 部品M塗装色 " +
            "FROM " +
            "入出庫ファイル " +
            "LEFT OUTER JOIN 仕入先マスタ AS 仕入先マスタ_1 ON 入出庫ファイル.課別コード = 仕入先マスタ_1.仕入先コード " +
            "LEFT OUTER JOIN 仕入先マスタ AS 仕入先マスタ_2 ON 入出庫ファイル.仕入先コード = 仕入先マスタ_2.仕入先コード " +
            "LEFT OUTER JOIN 部品マスタ ON 入出庫ファイル.部品コード = 部品マスタ.部品コード " +
            "WHERE " +
            $"(入出庫ファイル.課別コード = '{groupCode}') AND (入出庫ファイル.有償支給データＦ = 'M') ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /*
        /// <summary>
        /// 仕入先ファイル、入出庫ファイル、在庫マスタ、素材在庫マスタ更新
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        /// <param name="doCodeC1Check">伝票番号手動入力判定</param>
        /// <returns>IsOk：エラーが無し：True　エラーがある：False</returns>
        /// <returns>再ログインしたかどうか：再ログインした場合:true</returns>
        /// <returns>DoCode：伝票番号</returns>
        public (bool IsOk, bool ReLogin, string DoCode) UpdateIOFileAPI(List<ControlParam> controlList, bool doCodeC1Check)
        {
            var dataCate = controlList.SGetText("dataCateC1TextBox");
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var supCode = controlList.SGetText("supCodeC1TextBox");
            var jyuyoyosokuCode = controlList.SGetText("jyuyoyosokuCodeC1TextBox");
            var inNum = controlList.SGetText("outNumC1NumericEdit").Replace(",", "");
            var unitPrice = controlList.SGetText("unitPriceC1NumericEdit").Replace(",", "");
            var price = controlList.SGetText("outPriceC1NumericEdit").Replace(",", "");
            var acceptDate = controlList.SGetText("outDateC1DateEdit");
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            var accountCode = controlList.SGetText("accountCodeC1TextBox");
            var doCode = controlList.SGetText("doCodeC1TextBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");
            var nSupCate = controlList.SGetText("transCateC1ComboBox");
            var password = LoginInfo.Instance.UserNo;
            var machineName = LoginInfo.Instance.MachineCode;
            var nSupStockTransDataFlg = (doCodeC1Check == true ? "" : "M");
            var createDate = DateTime.Now.ToString();
            var remarks = controlList.SGetText("remarksC1TextBox");
            var createStaffCode = LoginInfo.Instance.UserNo;
            var createID = LoginInfo.Instance.UserId;

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += "IOFile/CreateNSupStockTrans";
            var token = LoginInfo.Instance.Token;

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("dataCate", new JValue(dataCate));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCode));
            param.Add("inNum", new JValue(inNum));
            param.Add("unitPrice", new JValue(unitPrice));
            param.Add("price", new JValue(price));
            param.Add("acceptDate", new JValue(acceptDate));
            param.Add("groupCode", new JValue(groupCode));
            param.Add("accountCode", new JValue(accountCode));
            param.Add("doCode", new JValue(doCode));
            param.Add("stockCate", new JValue(stockCate));
            param.Add("nSupCate", new JValue(nSupCate));
            param.Add("password", new JValue(password));
            param.Add("machineName", new JValue(machineName));
            param.Add("nSupStockTransDataFlg", new JValue(nSupStockTransDataFlg));
            param.Add("createDate", new JValue(createDate));
            param.Add("remarks", new JValue(remarks));
            param.Add("createStaffCode", new JValue(createStaffCode));
            param.Add("createID", new JValue(createID));
            param.Add("doCodeCheck", new JValue(doCodeC1Check));
            param.Add("isEOM", new JValue(true));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "");
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, false, "");
            }
            var aa = result["reLogIn"];
            return (true, false, (string)result["doCode"]);
        }
        */

        /// <summary>
        /// 入出庫ファイル有償支給データF更新
        /// </summary>
        /// <param name="groupCode">課別コード</param>
        /// <param name="doCode">伝票番号</param>
        public bool UpdateNSupStockTransDataFlg(string groupCode, string doCode)
        {
            string sql =
                $"UPDATE 製造調達.dbo.入出庫ファイル SET 有償支給データＦ = 'K' " +
                $"WHERE 課別コード = '{groupCode}' AND 伝票番号 = '{doCode}' AND ISNULL(有償支給データＦ, '') = 'M' ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return result.IsOk;
        }

        /// <summary>
        /// 代表機種名取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        public (bool IsOk, DataTable Table) GetProductNameByParts(string partsCode)
        {
            string sql =
            "SELECT TOP(1) A.部品コード, A.機種コード, ISNULL(B.機種名, '') AS 機種名, C.納期 " +
            "FROM 製造調達.dbo.部品構成表 AS A " +
            "INNER JOIN 製造調達.dbo.機種マスタ AS B ON A.機種コード = B.機種コード " +
            "INNER JOIN 製造調達.dbo.製造指令ファイル AS C ON A.機種コード = C.機種コード " +
           $"WHERE(A.部品コード = '{partsCode}') ORDER BY C.納期 DESC ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

    }
}
