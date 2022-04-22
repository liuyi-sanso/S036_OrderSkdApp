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
    /// 納品書受付処理
    /// </summary>
    public partial class F226_DelivSlipReception : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];

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
        public F226_DelivSlipReception(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "納品書受付処理";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F226_DelivSlipReception_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(codeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(stateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(webEDICateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(webEDICateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(skdCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(delivNumC1NumericEdit, null, null, false, enumCate.無し);
                AddControlListII(delivDateC1DateEdit, null, DateTime.Today.ToShortDateString(), true, enumCate.無し);

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

                // DefaultButtomMessageをセット
                defButtomMessage = "コード入力し、エンターキー押下後に、画面に情報が表示されます。　" +
                    "必須項目入力後に実行（F10）を押してください。　  ";

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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = codeC1TextBox;     
        }

        /// <summary>
        /// クリア処理  ボディ
        /// </summary>
        private void DisplayClearBody()
        {
            foreach (Control c in panel2.Controls)
            {
                var type = c.GetType();
                if (type == typeof(C1NumericEdit))
                {
                    ((C1NumericEdit)c).Value = null;
                }
                else if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).Text = "";
                }
                else
                {
                    // 処理なし
                }
            }

            foreach (Control c in panel3.Controls)
            {
                var type = c.GetType();
                if (type == typeof(C1NumericEdit))
                {
                    ((C1NumericEdit)c).Value = null;
                }
                else if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).Text = "";
                }
                else
                {
                    // 処理なし
                }
            }
        }

        #endregion  ＜クリア処理 END＞

        #region ＜共通イベント処理＞ 

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
        /// 入力コード　検証している
        /// </summary>
        private void codeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 入力コード
                var isOk = ErrorCheckCode();
                if (isOk == false)
                {
                    // クリア
                    DisplayClearBody();

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
        /// 入力コード　検証された後
        /// </summary>
        private void codeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                CodeValidated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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

            CodeValidated();

            return true;
        }

        /// <summary>
        /// 実行（F10）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF10()
        {
            // エラーチェック 入力コード
            var isOk = ErrorCheckCode();
            if (isOk == false)
            {
                ActiveControl = codeC1TextBox;
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

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                var code = codeC1TextBox.Text;
                var num = decimal.Parse(delivNumC1NumericEdit.Text);
                var date = DateTime.Parse(delivDateC1DateEdit.Text);

                apiParam.RemoveAll();
                apiParam.Add("changeCate", new JValue("1"));
                apiParam.Add("doCode", new JValue(code));
                apiParam.Add("num", new JValue(num));
                apiParam.Add("acceptDate", new JValue(date));
                var result = ApiCommonUpdate(apiUrl + "Task/ReceiptDelivSlip", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result.Msg);
                    return;
                }

                DisplayClear();
                ChangeTopMessage("I0009", code + " の納品書受領処理が");
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
        /// エラーチェック  入力コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckCode()
        {
            // 未入力時処理
            var s = codeC1TextBox;
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

            if (s.Text.Length < 13)
            {
                ChangeTopMessage("W0005", s.Label.Text, "13");
                return false;
            }

            // T_SHIPMENT_DATA
            var result1 = ApiCommonGet(apiUrl + "DeliveryNote/State/" + s.Text, null, true);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "T_SHIPMENT_DATA検索時");
                return false;
            }

            if (result1.Obj == null || result1.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "T_SHIPMENT_DATA");
                return false;
            }

            var state = result1.Obj["state"].ToString().TrimEnd();
            if (state == "2")
            {
                ChangeTopMessage(1, "WARN", "この納品書番号は既に取込済みです");
                return false;
            }

            var poCode = result1.Obj["orderCode"].ToString().TrimEnd();
            var doCode = result1.Obj["code"].ToString().TrimEnd();
            var skdCode = result1.Obj["code"].ToString().TrimEnd();
            var num = decimal.Parse(result1.Obj["remain"].ToString().TrimEnd());

            if ((poCode == "") || (skdCode == "") || (doCode == ""))
            {
                ChangeTopMessage("W0007", poCodeC1TextBox.Label.Text + "と" 
                    + doCodeC1TextBox.Label.Text + "と" + skdCodeC1TextBox.Label.Text);
                return false;
            }

            if (num == 0m) 
            {
                ChangeTopMessage("W0016", delivNumC1NumericEdit.Label.Text + "には数値0");
                return false;
            }

            // 発注明細
            apiParam.RemoveAll();
            apiParam.Add("poCode", new JValue(poCode));
            var result2 = ApiCommonGet(apiUrl + "Solution/S036/F226/GetOrdDetailMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注明細マスタ検索時に");
                return false;
            }
            if (result2.Table == null || result2.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "発注明細マスタ");
                return false;
            }

            var count1 = decimal.Parse(result2.Table.Rows[0]["count"].ToString());
            if (count1 <= 0m) 
            {
                ChangeTopMessage("W0002", s.Label.Text, "発注明細マスタ");
                return false;
            }

            // M_DelivSlip
            apiParam.RemoveAll();
            apiParam.Add("doCode", new JValue(doCode));
            var result3 = ApiCommonGet(apiUrl + "Solution/S036/F226/GetDelivSlipFile", apiParam);
            if (result3.IsOk == false)
            {
                ChangeTopMessage("E0008", "納品書ファイル検索時に");
                return false;
            }
            if (result3.Table == null || result3.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "納品書ファイル");
                return false;
            }

            var count2 = decimal.Parse(result3.Table.Rows[0]["count"].ToString());
            if (count2 >= 1m)
            {
                ChangeTopMessage(1, "WARN", "この納品書番号は既に取込済みです");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 入力コードの情報を画面に表示
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private void CodeValidated()
        {
            // 変更無ければ何もしない
            var s = codeC1TextBox;

            // クリア
            DisplayClearBody();

            // 未入力時処理
            if (string.IsNullOrEmpty(s.Text))
            {
                return;
            }

            // T_SHIPMENT_DATA
            var result1 = ApiCommonGet(apiUrl + "DeliveryNote/State/" + s.Text, null, true);
            if (result1.Obj == null || result1.Count <= 0)
            {
                return;
            }

            stateC1TextBox.Text = result1.Obj["state"].ToString().TrimEnd();
            webEDICateC1TextBox.Text = result1.Obj["ediState"].ToString().TrimEnd();
            webEDICateNameC1TextBox.Text = result1.Obj["ediStateName"].ToString().TrimEnd();

            poCodeC1TextBox.Text = result1.Obj["orderCode"].ToString().TrimEnd();
            doCodeC1TextBox.Text = result1.Obj["code"].ToString().TrimEnd();
            skdCodeC1TextBox.Text = result1.Obj["code"].ToString().TrimEnd();
            delivNumC1NumericEdit.Value = decimal.Parse(result1.Obj["remain"].ToString().TrimEnd());

            // T_SHIPMENT_DATA
            apiParam.RemoveAll();
            apiParam.Add("doCode", new JValue(s.Text));
            var result2 = ApiCommonGet(apiUrl + "Solution/S036/F226/GetShipmentData", apiParam, false);
            if (result2.Table == null || result2.Count <= 0)
            {
                return;
            }

            delivDateC1DateEdit.Value = result2.Table.Rows[0].Field<DateTime>("delivDate");
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], エラーメッセージ)</returns>
        private (bool IsOk, string Msg) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
        {
            apiUrl += $"?sid={solutionIdShort}&fid={formIdShort}";

            var webApi = new WebAPI();
            var result = webApi.PostRequest(apiUrl, apiParam ?? (new JObject() { }), LoginInfo.Instance.Token);
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
                        return (false, (string)(result["msg"]));
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, (string)(result["msg"]));
            }

            bool isOk;

            if (result["isOk"] == null) 
            {
                isOk = (bool)result["isok"];
            }
            else 
            {
                isOk = (bool)result["isOk"];
            }

            if (isOk == false)
            {
                return (false, (string)(result["msg"]));
            }

            return (true, (string)(result["msg"]));
        }

        /// <summary>
        /// WEBAPI側共通取得処理
        /// </summary>
        /// <param name="apiUrl">URL</param>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="isHttpGet">true：WEBAPIのメソッドが[HttpGet]、false：WEBAPIのメソッドが[HttpPost]</param>
        /// <returns>(isOk：実行成否、count：取得数、data：検索結果)</returns>
        private (bool IsOk, int Count, JObject Obj, DataTable Table) ApiCommonGet(string apiUrl, 
            JObject apiParam = null, bool isHttpGet = false)
        {
            apiUrl += $"?sid={solutionIdShort}&fid={formIdShort}";
            JObject result;

            if (isHttpGet) 
            {
                result = ControlAF.GetRequest(apiUrl, apiParam ?? (new JObject() { }), LoginInfo.Instance.Token);
            }
            else 
            {
                var webApi = new WebAPI();
                result = webApi.PostRequest(apiUrl, apiParam ?? (new JObject() { }), LoginInfo.Instance.Token);
            }

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
                        return (false, 0, null, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, null, null);
            }

            bool isOk;

            if (result["isOk"] == null)
            {
                isOk = (bool)result["isok"];
            }
            else
            {
                isOk = (bool)result["isOk"];
            }

            if (isOk == false)
            {
                return (false, 0, null, null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, 0, null, null);
            }

            if (result["data"].Type.ToString() == "Array") 
            {
                var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());

                return (
                    true,
                    (int)(result["count"]), 
                    null,
                    table);
            }
            else 
            {
                return (
                    true, 
                    (int)(result["count"]), 
                    (JObject)(result["data"]),
                    null);
            }
        }

        #endregion  ＜その他処理 END＞
    }
}