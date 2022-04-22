using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SansoBase;
using SansoBase.Common;
using C1.Win.C1Input;
using System.Runtime.InteropServices;
using System.IO;

namespace S036_OrderSkdApp
{
    public partial class Sample01 : BaseForm
    {
        public Sample01(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "サンプル画面０１";
        }

        private void Sample01_Load(object sender, EventArgs e)
        {
            string toolTipCSS = 
                "<style type='text/css'>" +
                ".fontRed {color:red;font-weight: bold;}" +
                ".fontGreen {color:green;padding:20px 0px 20px 5px;}" +

                "td {background-color:#ffffff;padding:5px 10px 5px 5px;}" +
                "th {background-color:#C0C0C0;padding:5px 10px 5px 5px;}" +
                ".explanationTitle {margin:5px 0px 0px 0px;border-bottom: solid 1px #C0C0C0;}" +
                ".explanation {margin:10px 0px 10px 0px}" +
                "</style>";


            string toolTipHTML01 = "" +
                "<div class='fontRed'>テストメッセージです</div>" +
                "<div class='fontGreen'>ここにはHTMLとCSSを使用して色々記述できます</div>";
            c1SuperTooltip1.SetToolTip(button1, toolTipCSS + toolTipHTML01);


            string toolTipHTML02 = "" +
                "<div class='explanationTitle'>" +
                "<table border='0'>" +
                "<tr>" +
                "<td align='left' width='25px'><img src='Forms/Sample/img/SuperTooltipInfo.png' width='20' height='20'></td>" +
                "<td align='left'><b>ワイルドカードを使用したあいまい検索ができます。</b></td>" +
                "</tr>" +
                "</table>" +
                "</div>" +
                "<div class='explanation'>" +
                "<table width='100%' border='0' cellspacing='1' bgcolor='#808080' class='explanationTable'>" +
                " <tr'>" +
                " <th>ワイルドカード<br>文字</th>" +
                " <th>説明</th>" +
                " </tr>" +
                " <tr>" +
                " <td>%</td>" +
                " <td>１文字以上の任意の文字列を表します</td>" +
                " </tr>" +
                " <tr> " +
                " <td>_</td>" +
                " <td>任意の１文字を表します</td>" +
                " </tr>" +
                " <tr>" +
                " <td>[&nbsp;]</td>" +
                " <td>[&nbsp;]内に指定した文字列の範囲にある、任意の１文字を表します。<br>範囲の指定方法：[2-6]、2,3,4,5,6のどれか１文字という意味</td>" +
                " </tr>" +
                " <tr>" +
                " <td>[^]</td> " +
                " <td>[^]内に指定した文字列の範囲に無い１文字を表します。<br>範囲の指定方法：[^2-6]、2,3,4,5,6以外の１文字という意味</td>" +
                " </tr> " +
                "</table>" +
                "</div>" +
                "<div>" +
                "例：「999%」：頭3文字が999から始まるという意味<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;「_[5-8]」：2文字目が5か6か7か8という意味" +
                "</div> ";
            c1SuperTooltip1.SetToolTip(productNameC1TextBox, toolTipCSS + toolTipHTML02);


        }

        private void F901Button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F901_ProductMCommonSearch("F901_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        //STProductCode = form.row.Cells["機種コード"].Value.ToString();
                        //STProductName = form.row.Cells["機種名"].Value.ToString();

                        //this.productCodeC1TextBox.Text = form.row.Cells["機種コード"].Value.ToString();
                        //this.productNameC1TextBox.Text = form.row.Cells["機種名"].Value.ToString();
                    }
                }
                //this.ActiveControl = productCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 印刷データ取得
            string tagCode = "";
            DataTable printDt = new DataTable();

            using (var con = new SqlConnection(CommonAF.connectionString))
            {
                // DBへ接続開始
                con.Open();

                //トランザクション開始
                SqlTransaction tran = con.BeginTransaction();

                var sql = "select " +
                "A.TagCode, A.PoCode, A.PartsCode, D.部品名 AS PartsName, D.図面番号 AS DrawingCode, A.SkdNum, " +
                "A.PackingBoxSerial, A.PackingBoxNum,A.DoCode, " +
                "B.DELIVERY_DATE AS SkdDate, CASE WHEN isnull(KISYU_CD, '') <> '' THEN KISYU_CD ELSE work.dbo.FC_代表機種コード(partsCode) end AS ProductCode, " +
                "E.機種名 AS ProductName, A.GroupCode, " +
                "F.部門名 AS GroupName, " +
                "CASE WHEN isnull(NxtCusCode , '') <> '' THEN NxtCusCode ELSE GroupCode END AS NxtCusCode, CASE WHEN isnull(NxtCusCode , '') <> '' THEN (SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ WHERE (仕入先コード = A.NxtCusCode)) ELSE (SELECT 部門名 FROM 三相メイン.dbo.部門マスタ WHERE (部門コード = GroupCode)) END AS NxtCusName, " +
                "A.CusCode, " +
                "(SELECT 仕入先名１ FROM 製造調達.dbo.仕入先マスタ AS 仕入先マスタ_1 WHERE (A.CusCode = 仕入先コード)) AS CusName, " +
                "CASE isnull(A.UpdateDate,'2000-01-01') when '2000-01-01' then A.CreateDate else A.UpdateDate end AS CreateDate, " +
                "G.Note AS Message, A.sakuban," +
                "H.ハンガー, H.吊り数,H.マスキング, H.マスキングNO, H.洗浄, H.梱包箱, H.最大入数,I.塗装色,J.仕様 AS 備考 " +
                "FROM SANSODB.dbo.M_TagCode AS A " +
                "left JOIN EDIDATA.dbo.T_SHIPMENT_DATA AS B ON (A.SkdCode = B.DO_NO) " +
                "left join SANSODB.dbo.AT_SAKUBAN AS C ON (A.Sakuban = C.SAKUBAN) " +
                "left join 製造調達.dbo.部品マスタ AS D on (A.PartsCode = D.部品コード) " +
                "left join 三相メイン.dbo.機種マスタ AS E on (CASE WHEN isnull(KISYU_CD, '') <> '' THEN KISYU_CD ELSE work.dbo.FC_代表機種コード(partsCode) end = E.機種コード) " +
                "left join 三相メイン.dbo.部門マスタ AS F on (A.GroupCode = F.部門コード) " +
                "left join SANSODB.dbo.M_Note AS G on (G.ID = 0) " +
                "left join 塗装管理.dbo.作業標準マスタ AS H ON (H.部品コード = A.PartsCode) " +
                "LEFT OUTER JOIN 製造調達.dbo.Ｖ塗装色参照 AS I ON A.GroupCode = I.仕入先コード AND A.PartsCode = I.部品コード " +
                "left join 塗装管理.dbo.部品マスタ AS J ON A.PartsCode = J.部品コード  " +
                "WHERE (A.TagCode = '30012104141518415') ";



                var result = CommonAF.ExecutSelectSQL(sql, tran, con);
                if (result.IsOk == false)
                {
                }
                printDt = result.Table;
            }

            // 現品票（A6版）印刷処理
            var result4 = PrintGoodsTagA6(printDt,true,true,"", @"\\sserv04\work\CS\AllMenuShortcut\CommonGoodsTagA6_test_kawabata.flxr");
            if (result4.isOk == false)
            {
                ChangeTopMessage(1, "ERR", result4.msg);
                return;
            }
        }
    }
}
