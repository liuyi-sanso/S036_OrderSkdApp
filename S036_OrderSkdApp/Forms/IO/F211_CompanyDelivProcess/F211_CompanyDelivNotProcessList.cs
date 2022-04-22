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
using System.Diagnostics.Contracts;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 関係会社 未処理一覧（検収）
    /// </summary>
    public partial class F211_CompanyDelivNotProcessList : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F211/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 組立部門
        /// </summary>
        private string groupCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        /// <param name="groupCode">組立部門</param>
        public F211_CompanyDelivNotProcessList(string fId, string groupCode) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "関係会社 未処理一覧（検収）";
            this.groupCode = groupCode;
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F211_CompanyDelivNotProcessList_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(groupCodeC1TextBox, null, groupCode, true, enumCate.無し);
                AddControlListII(batchCodeC1TextBox, null, "%", true, enumCate.無し);

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
                defButtomMessage = "一覧の「納期」と「納入数」と「納入単価」(薄い黄色列)は直接変更できます。　   " +
                   "「削除」ボタン押下後、該当行のデータが削除されます。\n" +
                   "※エラー表示のある物は納入受付できません。　　E：エラーあり";

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
            TopMenuEnable("F6", false);

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

            // 初期設定
            DrawC1TrueDBGrid();
        }

        #endregion  ＜クリア処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// c1TrueDBGrid　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                ClearTopMessage();
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName != "削除")
                {
                    return;
                }

                var autoNo = grid[row, "autoNo"].ToString();

                // Ｗ関係会社ファイルを削除
                apiParam.RemoveAll();
                apiParam.Add("autoNo", new JValue(autoNo));
                var result = ApiCommonUpdate(apiUrl + "DeleteWCompanyFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0007", "");
                    return;
                }

                DrawC1TrueDBGrid();
                ChangeTopMessage("I0003", "Ｗ関係会社ファイル");
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
                case "削除":
                    e.Value = "削除";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 一覧画面　検証している処理
        /// </summary>
        private void c1TrueDBGrid_BeforeColUpdate(object sender, C1.Win.C1TrueDBGrid.BeforeColUpdateEventArgs e)
        {
            try
            {
                ClearTopMessage();
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int rowIndex = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;
                var caption = e.Column.DataColumn.Caption;
                var text = e.Column.DataColumn.Text.TrimEnd();

                if (caption != "納期" && caption != "納入数" && caption != "納入単価")
                {
                    ChangeTopMessage(1, "ERR", "（「納期」と「納入数」と「納入単価」（薄黄色列）以外は更新できません");
                    e.Cancel = true;
                    return;
                }

                // 未入力チェック
                if (text == "")
                {
                    ChangeTopMessage("W0007", caption);
                    e.Cancel = true;
                    return;
                }

                if (caption == "納期")
                {
                    var isOk = ErrorCheckDelivDate(text, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (caption == "納入数")
                {
                    var isOk = ErrorCheckDelivNum(text, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (caption == "納入単価")
                {
                    var isOk = ErrorCheckDelivUnitPrice(text, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    // 処理なし
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 一覧画面　列を更新した後処理
        /// </summary>
        private void c1TrueDBGrid_AfterColUpdate(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int rowIndex = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;
                var caption = e.Column.DataColumn.Caption;
                var text = e.Column.DataColumn.Text.TrimEnd();

                if (caption != "納期" && caption != "納入数" && caption != "納入単価")
                {
                    return;
                }

                // 更新行の情報を取得
                var autoNo = grid[rowIndex, "autoNo"].ToString().TrimEnd();

                // 未入力チェック
                if (text == "")
                {
                    ChangeTopMessage("W0007", caption);
                    return;
                }

                // Ｗ関係会社ファイルを修正
                apiParam.RemoveAll();
                apiParam.Add("autoNo", new JValue(autoNo));
                apiParam.Add("fieldName", new JValue(caption));
                apiParam.Add("value", new JValue(text));
                var result = ApiCommonUpdate(apiUrl + "UpdateWCompanyFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0001", "Ｗ関係会社ファイル");
                    return;
                }

                DrawC1TrueDBGrid();
                ChangeTopMessage("I0002", "Ｗ関係会社ファイル");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜イベント処理 END＞

        #region ＜メイン処理＞ 


        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// c1TrueDBGrid　描画
        /// </summary>
        private void DrawC1TrueDBGrid()
        {
            // クリア
            c1TrueDBGrid.SetDataBinding(null, "", true);

            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(groupCodeC1TextBox.Text));

            // 送受信履歴ファイル
            var result = ApiCommonGet(apiUrl + "GetCompanyNotProcessList", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("I0005");
                return;
            }

            c1TrueDBGrid.SetDataBinding(result.Table, "", true);
            ChangeTopMessage("I0011", result.Table.Rows.Count.ToString("#,###"));
        }

        /// <summary>
        /// エラーチェック  納期
        /// </summary>
        /// <param name="dateTxt">納期</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivDate(string dateTxt, string name)
        {
            // 未入力チェック
            if (dateTxt == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            var chk = Check.IsDate(dateTxt);
            if (chk.Result == false)
            {
                ChangeTopMessage("W0019", chk.Msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  納入数
        /// </summary>
        /// <param name="numTxt">納入数</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivNum(string numTxt, string name)
        {
            // 未入力チェック
            if (numTxt == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            // 数値か
            var chk1 = Check.IsNumeric(numTxt);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", name + "には");
                return false;
            }

            decimal value = decimal.Parse(numTxt);

            // 正数か
            if (value < 0m)
            {
                ChangeTopMessage("W0006", name);
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 7, 0);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", name + "の" + chk2.Msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  納入単価
        /// </summary>
        /// <param name="numTxt">納入単価</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivUnitPrice(string numTxt, string name)
        {
            // 未入力チェック
            if (numTxt == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            // 数値か
            var chk1 = Check.IsNumeric(numTxt);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", name + "には");
                return false;
            }

            decimal value = decimal.Parse(numTxt);

            // 正数か
            if (value < 0m)
            {
                ChangeTopMessage("W0006", name);
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 6, 2);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", name + "の" + chk2.Msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0])</returns>
        private (bool IsOk, int Count) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
        {
            var webApi = new WebAPI();
            var result = webApi.PostRequest(apiUrl, apiParam, LoginInfo.Instance.Token);
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
                        return (false, 0);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0);
            }

            if ((bool)result["isOk"] == false)
            {
                return (false, 0);
            }

            return (
                true,
                (int)(result["count"])
            );
        }

        /// <summary>
        /// WEBAPI側共通取得処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(isOk：実行成否、count：取得数、data：検索結果)</returns>
        private (bool IsOk, int Count, DataTable Table) ApiCommonGet(string apiUrl, JObject apiParam = null)
        {
            var webApi = new WebAPI();
            var result = webApi.PostRequest(apiUrl, apiParam, LoginInfo.Instance.Token);
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
                        return (false, 0, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, null);
            }

            if ((bool)result["isOk"] == false)
            {
                return (false, 0, null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, 0, null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());

            return (
                true,
                (int)(result["count"]),
                table
            );
        }

        #endregion  ＜その他処理 END＞
    }
}