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
using SansoBase.Common;
using C1.Win.C1Input;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 伝票再発行
    /// </summary>
    public partial class F219_DocuReprint : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F219/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F219_DocuReprint(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "伝票再発行";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F219_DocuReprint_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // 専用プリンタのドライバがインストールされているかどうか
                bool isInsideTransPrinter = false;
                bool isOutsideTransPrinter = false;
                bool isDelivSlipPrinter = false;

                string insideTransPrinterName = System.Configuration.ConfigurationManager.AppSettings["InsideTrans"];
                string outsideTransPrinterName = System.Configuration.ConfigurationManager.AppSettings["OutsideTrans"];
                string delivSlipPrinterName = System.Configuration.ConfigurationManager.AppSettings["DelivSlip"];

                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == insideTransPrinterName)
                    {
                        isInsideTransPrinter = true;
                    }
                }

                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == outsideTransPrinterName)
                    {
                        isOutsideTransPrinter = true;
                    }
                }

                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == delivSlipPrinterName)
                    {
                        isDelivSlipPrinter = true;
                    }
                }

                if (isInsideTransPrinter == false || isOutsideTransPrinter == false || isDelivSlipPrinter == false)
                {
                    MessageBox.Show("必要なプリンタ(" + insideTransPrinterName +
                        ")または(" + outsideTransPrinterName + ")または(" +
                        delivSlipPrinterName + ")がありません。処理を中止します。", "プリンタなしエラー", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // コントロールリストをセット
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, null, "", false, enumCate.無し);

                // 共通の設定変更
                foreach (var v in controlListII)
                {
                    var t = (C1TextBox)v.Control;
                    // 全コントロールのShowErrorMessageをfalseに変更
                    t.ErrorInfo.ShowErrorMessage = false;

                    // 色替えの場合には「BorderStyle」は「FixedSingle」でないと動かない
                    t.BorderStyle = BorderStyle.FixedSingle;

                    // 必須項目を赤枠に変更（不要ならデザイナ側でやってもよい）
                    if (v.Required == true)
                    {
                        t.BorderColor = Color.Red;
                    }
                    else
                    {
                        t.BorderColor = SystemColors.WindowFrame;
                    }

                    // ラベルがグレーになってしまうため、EnabledをTrueに戻す
                    if (t.Label != null)
                    {
                        t.Label.Enabled = true;
                    }
                }

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後に実行（F10）を押してください。\r\n" +
                    "「再発行」押下後、社内移行伝票(D=8)・社内移行伝票返品(D=2)・有償支給伝票外注(D=6)・有償支給伝票一般(D=1)・返品納品書が再発行されます。";

                // クリア処理
                DisplayClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                isRunValidating = true;
            }
        }

        #endregion  ＜起動処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            // ファンクションキーの使用可否設定
            TopMenuEnable("F10", true);

            // コントロールの一括クリア処理
            foreach (var v in controlListII)
            {
                var c = v.Control;
                var type = c.GetType();
                if (type == typeof(C1.Win.Calendar.C1DateEdit))
                {
                    ((C1.Win.Calendar.C1DateEdit)c).Value = v.Initial;
                }
                else if (type == typeof(C1NumericEdit))
                {
                    ((C1NumericEdit)c).Value = v.Initial;
                }
                else
                {
                    ((C1TextBox)c).Text = v.Initial;
                }
            }

            // 初期設定
            c1TrueDBGrid.SetDataBinding(null, "", true);

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = partsCodeC1TextBox;     
        }

        #endregion  ＜クリア処理 END＞

        #region ＜共通イベント処理＞ 

        /// <summary>
        /// F4ボタン押下時処理
        /// </summary>
        /// <remarks>現在アクティブなコントロールによって共通検索画面を起動する</remarks>
        protected override void F4Bt_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ActiveControl.Name)
                {
                    case "partsCodeC1TextBox":
                        partsSearchBt_Click(sender, e);
                        break;

                    default:
                        ChangeTopMessage("I0010");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コード  検索ボタン押下時
        /// </summary>
        private void partsSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        partsCodeC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString();
                        partsNameC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                    }
                }
                ActiveControl = partsCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        protected override void F6Bt_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 実行前 必須項目チェック
        /// </summary>
        protected override bool Requir_F10(object sender, EventArgs e)
        {
            try
            {
                return RequirF10();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 実行前 エラーチェック
        /// </summary>
        protected override bool ErrCK_F10(object sender, EventArgs e)
        {
            try
            {
                return ErrCKF10();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 実行（F10）
        /// </summary>
        protected override void F10Bt_Click(object sender, EventArgs e)
        {
            try
            {
                if (c1TrueDBGrid.EditActive)
                {
                    ActiveControl = F10Bt;
                }

                ActionProc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// コントロールの背景色を黄色に変える
        /// </summary>
        private void ChangeCtlBkColEnter(object sender, EventArgs e)
        {
            ChangeControlBackColor.ChangeControlBackColorMethod(sender, 1);

            string tag = (((TextBox)sender).Tag ?? "").ToString();
            buttomMessageLabel.Text = (tag == "") ? defButtomMessage : tag;
        }

        /// <summary>
        /// コントロールの背景色を白に変える
        /// </summary>
        private void ChangeCtlBkColLeave(object sender, EventArgs e)
        {
            ChangeControlBackColor.ChangeControlBackColorMethod(sender, 0);

            // トップメッセージクリアは各メソッドでは行わず、Leaveで統一する
            ClearTopMessage();
        }

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 部品コード　検証している
        /// </summary>
        private void partsCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 部品コード
                var isOk = ErrorCheckPartsCode();
                if (isOk == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// c1TrueDBGrid　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName != "再発行")
                {
                    return;
                }

                string jyuyoyosokuCode = grid[row, "jyuyoyosokuCode"].ToString().TrimEnd();
                string insideTransF = grid[row, "insideTransDataF"].ToString().TrimEnd();
                string outsideTransF = grid[row, "nSupStockTransDataF"].ToString().TrimEnd();
                string delivSlipF = grid[row, "delivSlipDataF"].ToString().TrimEnd();
                string dataCate = grid[row, "dataCate"].ToString().TrimEnd();
                Int64 autoNo = Int64.Parse(grid[row, "autoNo"].ToString().TrimEnd());
                string supCode = grid[row, "supCode"].ToString().TrimEnd();

                var webApi = new WebAPI();

                // 調達かどうか
                bool isOrder = false;

                // パラメータ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(LoginInfo.Instance.GroupCode));

                // 部門マスタ
                var result1 = webApi.PostRequest(apiUrl + "GetGroupMst", apiParam, LoginInfo.Instance.Token);
                if ((result1 == null) || ((int)result1["Status"] != (int)HttpStatusCode.OK))
                {
                    if (result1["reLogIn"] != null)
                    {
                        if ((bool)result1["reLogIn"])
                        {
                            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                                            $"再度、処理を実行してください",
                                            "ログイン有効期限切れエラー",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }

                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if ((bool)(result1["isOk"]) == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result1["data"] != null || (int)(result1["count"]) >= 1)
                {
                    var p = result1["data"]["departmentName"].ToString();
                    if (p == "資材部")
                    {
                        isOrder = true;
                    }
                }

                // パラメータ
                apiParam.RemoveAll();
                apiParam.Add("autoNo", new JValue(autoNo));

                // レポートデータ    
                var result2 = webApi.PostRequest(apiUrl + "GetReportData", apiParam, LoginInfo.Instance.Token);
                if ((result2 == null) || ((int)result2["Status"] != (int)HttpStatusCode.OK))
                {
                    if (result2["reLogIn"] != null)
                    {
                        if ((bool)result2["reLogIn"])
                        {
                            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                                            $"再度、処理を実行してください",
                                            "ログイン有効期限切れエラー",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }

                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if ((bool)(result2["isOk"]) == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result2["data"] == null || (int)(result2["count"]) <= 0)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result2["data"]).ToString());

                // 機種名設定
                foreach (DataRow v in table.Rows)
                {
                    var j = v["jyuyoyosokuCode"].ToString().TrimEnd();

                    if (j == "")
                    {
                        // パラメータ
                        apiParam.RemoveAll();
                        apiParam.Add("partsCode", new JValue(v["partsCode"].ToString().TrimEnd()));

                        // 部品構成表
                        var result3 = webApi.PostRequest(apiUrl + "GetBOMMst", apiParam, LoginInfo.Instance.Token);
                        if ((result3 == null) || ((int)result3["Status"] != (int)HttpStatusCode.OK))
                        {
                            if (result3["reLogIn"] != null)
                            {
                                if ((bool)result3["reLogIn"])
                                {
                                    MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                                                    $"再度、処理を実行してください",
                                                    "ログイン有効期限切れエラー",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                    return;
                                }
                                else
                                {
                                    Application.Exit();
                                }
                            }

                            ChangeTopMessage("E0008", "検索時に");
                            return;
                        }
                        if ((bool)(result3["isOk"]) == false)
                        {
                            ChangeTopMessage("E0008", "検索時に");
                            return;
                        }
                        if (result3["data"] == null || (int)(result3["count"]) <= 0)
                        {
                            continue;
                        }

                        v["productName"] = ((JArray)(result3["data"])).First()["productName"].ToString();
                    }
                    else
                    {
                        // パラメータ
                        apiParam.RemoveAll();
                        apiParam.Add("jyuyoyosokuCode", new JValue(j));

                        // 製造指令ファイル
                        var result3 = webApi.PostRequest(apiUrl + "GetManufactFile", apiParam, LoginInfo.Instance.Token);
                        if ((result3 == null) || ((int)result3["Status"] != (int)HttpStatusCode.OK))
                        {
                            if (result3["reLogIn"] != null)
                            {
                                if ((bool)result3["reLogIn"])
                                {
                                    MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                                                    $"再度、処理を実行してください",
                                                    "ログイン有効期限切れエラー",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                    return;
                                }
                                else
                                {
                                    Application.Exit();
                                }
                            }

                            ChangeTopMessage("E0008", "検索時に");
                            return;
                        }
                        if ((bool)(result3["isOk"]) == false)
                        {
                            ChangeTopMessage("E0008", "検索時に");
                            return;
                        }
                        if (result3["data"] == null || (int)(result3["count"]) <= 0)
                        {
                            continue;
                        }

                        v["productName"] = ((JArray)(result3["data"])).First()["productName"].ToString();
                    }
                }


                // 社内移行伝票
                if (insideTransF == "K" && dataCate == "8")
                {
                    if (supCode == "3630")
                    {
                        // レポート
                        using (var report = new C1.Win.FlexReport.C1FlexReport())
                        {
                            report.Load(EXE_DIRECTORY + @"\Reports\R004_InsideTransPaint.flxr", "R004_InsideTransPaint");

                            // データソース設定
                            var ds = new C1.Win.FlexReport.DataSource
                            {
                                Name = " ",
                                ConnectionString = reportConnectionString,
                                Recordset = table
                            };
                            report.DataSources.Add(ds);
                            report.DataSourceName = ds.Name;

                            // プレビュー印刷
                            report.Render();
                            var print = PrintReport(report);
                            if (print.IsOk == false)
                            {
                                ChangeTopMessage("E0008", "印刷処理で");
                                return;
                            }
                        }
                    }
                    else
                    {
                        // レポート
                        using (var report = new C1.Win.FlexReport.C1FlexReport())
                        {
                            report.Load(EXE_DIRECTORY + @"\Reports\R003_InsideTrans.flxr", "R003_InsideTrans");

                            // データソース設定
                            var ds = new C1.Win.FlexReport.DataSource
                            {
                                Name = " ",
                                ConnectionString = reportConnectionString,
                                Recordset = table
                            };
                            report.DataSources.Add(ds);
                            report.DataSourceName = ds.Name;
                            report.Fields["テキスト_denNo"].Visible = isOrder;

                            // プレビュー印刷
                            report.Render();
                            var print = PrintReport(report);
                            if (print.IsOk == false)
                            {
                                ChangeTopMessage("E0008", "印刷処理で");
                                return;
                            }
                        }            
                    }
                }

                // 社内移行伝票 返品
                if (insideTransF == "K" && dataCate == "2")
                {
                    // レポート
                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {
                        report.Load(EXE_DIRECTORY + @"\Reports\R005_InsideTransReturn.flxr", "R005_InsideTransReturn");

                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = reportConnectionString,
                            Recordset = table
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

                        // プレビュー印刷
                        report.Render();
                        var print = PrintReport(report);
                        if (print.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "印刷処理で");
                            return;
                        }
                    }
                }

                // 有償支給伝票　バーコード
                if (outsideTransF == "K" && dataCate == "6")
                {
                    // レポート
                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {
                        report.Load(EXE_DIRECTORY + @"\Reports\R007_NSupStockTransDocuBarcode.flxr", "R007_NSupStockTransDocuBarcode");

                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = reportConnectionString,
                            Recordset = table
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

                        // プレビュー印刷
                        report.Render();
                        var print = PrintReport(report);
                        if (print.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "印刷処理で");
                            return;
                        }
                    }
                }

                // 有償支給伝票　一般
                if (outsideTransF == "K" && dataCate == "1")
                {
                    // レポート
                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {
                        report.Load(EXE_DIRECTORY + @"\Reports\R006_NSupStockTransDocu.flxr", "R006_NSupStockTransDocu");

                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = reportConnectionString,
                            Recordset = table
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

                        // プレビュー印刷
                        report.Render();
                        var print = PrintReport(report);
                        if (print.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "印刷処理で");
                            return;
                        }
                    }
                }

                // 
                if (delivSlipF == "K")
                {
                    // レポート
                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {
                        report.Load(EXE_DIRECTORY + @"\Reports\R008_DelivSlip.flxr", "R008_DelivSlip");

                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = reportConnectionString,
                            Recordset = table
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

                        // プレビュー印刷
                        report.Render();
                        var print = PrintReport(report);
                        if (print.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "印刷処理で");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                isRunValidating = true;
            }
        }

        /// <summary>
        /// 非連携列のテキストを設定
        /// </summary>
        private void c1TrueDBGrid_UnboundColumnFetch(object sender, C1.Win.C1TrueDBGrid.UnboundColumnFetchEventArgs e)
        {
            switch (e.Column.Caption)
            {
                case "再発行":
                    e.Value = "再発行";
                    break;

                default:
                    break;
            }
        }

        #endregion  ＜イベント処理 END＞

        #region ＜実行前チェック＞ 

        /// <summary>
        /// 実行（F10）必須チェック
        /// </summary>
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        private bool RequirF10()
        {
            // 必須チェック
            var control = controlListII?
                          .Where(v => v.Required)
                          .FirstOrDefault(v => string.IsNullOrEmpty(v.Control.Text));

            if (control != null)
            {
                var ctl = ((C1TextBox)control.Control);
                this.ActiveControl = ctl;
                ChangeTopMessage("W0007", ctl.Label.Text);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 実行（F10）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF10()
        {
            // エラーチェック 部品コード
            var isOk = ErrorCheckPartsCode();
            if (isOk == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            return true;
        }

        #endregion  ＜実行前チェック END＞

        #region ＜メイン処理＞ 

        /// <summary>
        /// 実行処理
        /// </summary>
        private void ActionProc()
         {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // クリア
                c1TrueDBGrid.SetDataBinding(null, "", true);

                // パラメータ
                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));

                // 入出庫ファイル
                var webApi = new WebAPI();
                var result = webApi.PostRequest(apiUrl + "GetIOList", apiParam, LoginInfo.Instance.Token);
                if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK))
                {
                    if (result["reLogIn"] != null)
                    {
                        if ((bool)result["reLogIn"])
                        {
                            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                                            $"再度、処理を実行してください",
                                            "ログイン有効期限切れエラー",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }

                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if ((bool)(result["isOk"]) == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result["data"] == null || (int)(result["count"]) <= 0)
                {
                    ChangeTopMessage("I0005");
                    return;
                }

                var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
                c1TrueDBGrid.SetDataBinding(table, "", true);
                groupCodeC1TextBox.Text = DBNull.Value.Equals(table.Rows[0]["staffGroupCode"]) ? "" : table.Rows[0]["staffGroupCode"].ToString();

                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0011", table.Rows.Count.ToString("#,###"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;

                isRunValidating = true;
            }
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// エラーチェック  部品コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPartsCode()
        {
            partsNameC1TextBox.Text = "";

            // 未入力時処理
            var s = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 部品マスタ
            var param = new SansoBase.PartsMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, s.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "部品マスタ");
                return false;
            }
            partsNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";

            return true;
        }

        #endregion  ＜その他処理 END＞
    }
}