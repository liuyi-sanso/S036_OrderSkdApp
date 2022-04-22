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
using System.Configuration;


namespace S036_OrderSkdApp
{
    public partial class F227_DocumentPrint : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F227/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        /// 
        private JObject apiParam = new JObject();

        /// <summary>
        /// c1TrueDBGrid用のDataTable
        /// </summary>
        DataTable c1TrueDBGridDT;

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        public F227_DocumentPrint(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "伝票発行処理";
        }

        private void F227_DocumentPrint_Load(object sender, EventArgs e)
        {
            try
            {
                //C1TextBoxをリスト化
                AddControlListII(nextCusCodeNameC1TextBox, null, "", false, enumCate.無し);

                //C1CombBoxをリスト化
                AddControlListII(nextCusCodeC1ComboBox, nextCusCodeNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(reportCateC1ComboBox, null, "", true, enumCate.無し);

                //C1DateEditをリスト化
                AddControlListII(startDateC1DateEdit, null, DateTime.Today.AddMonths(-1).ToString(), true, enumCate.無し);
                AddControlListII(endDateC1DateEdit, null, DateTime.Today.AddDays(7).ToString(), true, enumCate.無し);
                AddControlListII(acceptDateC1DateEdit, null, DateTime.Today.ToString(), true, enumCate.無し);
                
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

                // コンボボックスセット、コンボボックスの内容をセットするメソッドは１コントロールずつに分ける
                SetNextCusCodeC1ComboBox();
                SetreportCateC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "印刷対象の現品票にチェックを入れ、実行（F10）を押してください。";

                // クリア処理
                DisplayClear();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // 初期表示
                SearchProc();
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

        #endregion  ＜起動処理 END＞    


        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 次工程  コンボボックスセット
        /// </summary>
        private void SetNextCusCodeC1ComboBox()
        {
            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("dummy", new JValue(""));

            // 次工程一覧
            var webApi = new WebAPI();
            string url = apiUrl + "GetNxtCusCodeList" + "?sid=" + solutionIdShort + "&fid=" + formIdShort;
            var result = webApi.PostRequest(url, apiParam, LoginInfo.Instance.Token);
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

            DataRow dr;
            dr = table.NewRow();
            dr[0] = "_";
            dr[1] = "すべて";
            table.Rows.InsertAt(dr, 0);

            ControlAF.SetC1ComboBox(nextCusCodeC1ComboBox, table, 50, 200);

            nextCusCodeC1ComboBox.SelectedIndex = 0;
            nextCusCodeNameC1TextBox.Text = "すべて";
        }

        /// <summary>
        /// 伝票種類　コンボボックスセット
        /// </summary>
        private void SetreportCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("すべて");
            dt.Rows.Add("社内移行伝票");
            dt.Rows.Add("有償支給伝票");
            ControlAF.SetC1ComboBox(reportCateC1ComboBox, dt, 0, 200, "NAME", "NAME", true);
        }


        #endregion  ＜コンボボックス設定処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            // ファンクションキーの使用可否設定
            TopMenuEnable("F8", false);
            TopMenuEnable("F10", true);
            TopMenuEnable("F12", false);

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

            c1TrueDBGrid.SetDataBinding(null, "", true);
            c1TrueDBGridDT = null;

            reportCateC1ComboBox.SelectedIndex = 0;
            SetNextCusCodeC1ComboBox();

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = nextCusCodeC1ComboBox;
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
        /// 実行前 整合性チェック
        /// </summary>
        protected override bool Consis_F10(object sender, EventArgs e)
        {
            try
            {
                return ConsisF10();
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

        /// <summary>
        /// コンボボックスの共通Validating処理
        /// </summary>
        private void ComboBoxValidating(object sender, CancelEventArgs e)
        {
            try
            {
                // コンボボックスの共通Validating処理（戻り値あり）
                if (IsOkComboBoxValidating(sender, e) == false)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// コンボボックスの共通Validating処理（戻り値あり）
        /// </summary>
        /// <returns>
        /// エラー無し：true
        /// エラーチェックを実行せず：False
        /// エラー有り：False & e.Cancel = true
        /// </returns>
        private bool IsOkComboBoxValidating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return false;
                }

                var c = (C1ComboBox)sender;

                // 未入力時処理
                if (string.IsNullOrEmpty(c.Text))
                {
                    return false;
                }

                // 使用禁止文字
                if (Check.HasSQLBanChar(c.Text).Result == false)
                {
                    ChangeTopMessage("W0018");
                    ActiveControl = c;
                    e.Cancel = true;
                    return false;
                }

                // ComboBoxリスト存在チェック
                Func<bool> isError = () => {
                    ChangeTopMessage("W0013", c.Label.Text);
                    ActiveControl = c;
                    e.Cancel = true;
                    return false;
                };

                var dv = (DataView)c.ItemsDataSource;
                if (dv == null)
                {
                    return isError();
                }

                dv.RowFilter = $"{dv.Table.Columns[0].ColumnName} = '{c.Text}' ";
                if (dv.Count <= 0)
                {
                    return isError();

                    // ２列目のチェックを外す。問題あれば河野まで連絡してください
                    //dv.RowFilter = $"{dv.Table.Columns[1].ColumnName} = '{c.Text}' ";
                    //if (dv.Count <= 0)
                    //{
                    //    return isError();
                    //}
                }

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// コンボボックスの共通Validated処理
        /// </summary>
        private void ComboBoxValidated(object sender, EventArgs e)
        {
            try
            {
                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// コンボボックスの共通Validated処理（戻り値あり）
        /// </summary>
        /// <returns>
        /// エラー無し：True
        /// Validated処理を実行せず：False
        /// </returns>
        private bool IsOkComboBoxValidated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return false;
                }

                var c = (C1ComboBox)sender;

                // controlListIIから対象のコントロールの情報を取得
                var SelectList = controlListII.Where(v => v.Control.Name == c.Name).ToList();
                var listCtr = SelectList[0].SubControl;

                // コンボボックスに対応したテキストボックスがない場合は何もしない
                if (listCtr == null) { return true; }

                // コンボボックスDataSourceをDataViewに変換
                var dv = (System.Data.DataView)c.ItemsDataSource;
                if (dv == null)
                {
                    listCtr.Text = "";
                    return true;
                }

                // 未入力時処理
                if (string.IsNullOrEmpty(c.Text))
                {
                    listCtr.Text = "";
                    dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '' ";
                    if (dv.Count >= 1)
                    {
                        listCtr.Text = dv.ToTable().Rows[0][1].ToString();
                    }
                    return true;
                }

                // ComboBoxの内容を名称テキストに反映
                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + c.Text + "' ";
                if (dv.Count == 0)
                {
                    return false;
                }
                else
                {
                    listCtr.Text = dv.ToTable().Rows[0][1].ToString();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// コンボボックスの共通SelectedIndexChanged処理
        /// </summary>
        private void ComboBoxSelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var c = (C1ComboBox)sender;

                if (c.SelectedIndex < 0)
                {
                    return;
                }

                // controlListIIから対象のコントロールの情報を取得
                var SelectList = controlListII.Where(v => v.Control.Name == c.Name).ToList();
                var listCtr = SelectList[0].SubControl;

                // コンボボックスDataSourceをDataViewに変換
                var dv = (System.Data.DataView)((C1ComboBox)SelectList[0].Control).ItemsDataSource;
                if (dv == null)
                {
                    listCtr.Text = "";
                    return;
                }

                // 未入力時処理
                if (string.IsNullOrEmpty(c.Text))
                {
                    dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '' ";
                    if (dv.Count >= 1)
                    {
                        listCtr.Text = dv.ToTable().Rows[0][1].ToString();
                        return;
                    }
                    else
                    {
                        listCtr.Text = "";
                        return;
                    }
                }

                // ComboBoxの内容を名称テキストに反映
                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ((C1ComboBox)SelectList[0].Control).Text + "' ";
                listCtr.Text = dv.ToTable().Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// ComboBoxリスト存在チェックと名称テキストに反映（全て）
        /// </summary>
        private bool CheckComboBoxListAll()
        {
            try
            {
                bool returnFlg = true;
                var SelectControlListII = ControlListII.Where(v => v.Control.GetType() == typeof(C1ComboBox));
                foreach (var v in SelectControlListII)
                {
                    var ctl = (C1ComboBox)v.Control;

                    // 使用禁止文字
                    if (Check.HasSQLBanChar(ctl.Text).Result == false)
                    {
                        ChangeTopMessage("W0018");
                        returnFlg = false;
                    }
                    // コンボボックスリストの存在チェック
                    else if (ControlAF.CheckComboBoxList(ctl,
                        (v.SubControl != null ? ((C1TextBox)v.SubControl) : null)) == false)
                    {
                        ChangeTopMessage("W0013", ctl.Label.Text);
                        returnFlg = false;
                    }
                    else
                    {
                        // 処理なし
                    }
                }
                return returnFlg;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                SearchProc();
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

        /// <summary>
        /// 全てにチェックを入れるボタンを押下
        /// </summary>
        private void allChoiceButton_Click(object sender, EventArgs e)
        {
            if (c1TrueDBGridDT == null)
            {
                return;
            }

            for (int i = 0; i < c1TrueDBGridDT.Rows.Count; i++)
            {
                this.c1TrueDBGrid[i, "選択"] = "True";
            }
        }

        /// <summary>
        /// 全てのチェックを外すボタンを押下
        /// </summary>
        private void clearChoiceButton_Click(object sender, EventArgs e)
        {
            if (c1TrueDBGridDT == null)
            {
                return;
            }

            for (int i = 0; i < c1TrueDBGridDT.Rows.Count; i++)
            {
                this.c1TrueDBGrid[i, "選択"] = "False";
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
        /// 実行（F10）整合性チェック
        /// </summary>
        /// <returns>True：整合性ＯＫ False：整合性を満たしていない項目がある</returns>
        private bool ConsisF10()
        {
            // ComboBoxリスト存在チェックと名称テキストに反映（全て）
            if (CheckComboBoxListAll() == false)
            {
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
            // データ有無チェック
            if (c1TrueDBGridDT == null || c1TrueDBGridDT.Rows.Count <= 0) 
            {
                ChangeTopMessage("I0008", "データが選択されていません。処理を");
                return false;
            }

            // チェック有り抽出
            var selectTable = c1TrueDBGridDT.AsEnumerable()
                                .Where(v => v.Field<string>("Choice") == "True")
                                .ToList();

            if (selectTable.Count <= 0) 
            {
                ChangeTopMessage("I0008", "データが選択されていません。処理を");
                return false;
            }

            foreach (var v in selectTable) 
            {
                // 単価登録をチェック、内部依頼は除く
                if (string.IsNullOrEmpty(v.Field<string>("SUPPLY_COST")) || decimal.Parse(v.Field<string>("SUPPLY_COST")) == 0)
                {
                    if (v.Field<string>("InRequestGroupCode") == "")
                    {
                        ChangeTopMessage(1, "ERR", "現品票番号「" + v.Field<string>("TagCode") + "」の単価が登録されていません。");
                        return false;
                    }
                }
            }

            var a = Cursor;

            if (Cursor == Cursors.WaitCursor)
            {
                ChangeTopMessage("I0008", "実行中のため。処理を");
                return false;
            }


            return true;
        }

        #endregion  ＜実行前チェック END＞

        #region ＜メイン処理＞ 

        /// <summary>
        /// 実行処理（入出庫ファイル作成、伝票印刷処理）
        /// </summary>
        private void ActionProc()
        {
            try
            {

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;


                isRunValidating = false;
                ClearTopMessage();

                // チェック有り抽出
                var selectCount = c1TrueDBGridDT.AsEnumerable()
                                    .Where(v => v.Field<string>("Choice") == "True")
                                    .Count();

                var af = new F227_DocumentPrintAF();
                // 伝票データ入出庫ファイル更新
                var result = af.CreateDocumentData(c1TrueDBGridDT, DateTime.Parse(acceptDateC1DateEdit.Text));
                if (result.isOk == false)
                {
                    ChangeTopMessage("E0005", result.msg);
                    return;
                }

                // 伝票用データ抽出
                var result2 = af.GetPrintDT(c1TrueDBGridDT);
                if (result2.isOk == false)
                {
                    ChangeTopMessage("E0005", result2.msg);
                    return;
                }

                // 伝票印刷
                var result3 = PrintDocument(result2.printDtG, result2.printDtS, result2.printDtK, result2.printDtK3630);
                if (result3.isOk == false)
                {
                    ChangeTopMessage("E0005", result3.msg);
                    return;
                }

                // 印刷済フラグ更新
                var result4 = af.UpdatePrintFlg(c1TrueDBGridDT);
                if (result4.isOk == false)
                {
                    ChangeTopMessage("E0005", result4.msg);
                    return;
                }

                // クリア処理
                DisplayClear();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // 初期表示
                SearchProc();

                ChangeTopMessage("I0009", "伝票発行処理が");
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
        /// 検索処理
        /// </summary>
        private void SearchProc()
        {
            isRunValidating = false;
            ClearTopMessage();

            // クリア
            c1TrueDBGrid.SetDataBinding(null, "", true);
            c1TrueDBGridDT = null;

            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("startDate", new JValue(startDateC1DateEdit.Text));
            apiParam.Add("endDate", new JValue(endDateC1DateEdit.Text));
            apiParam.Add("nxtCusCode", new JValue(nextCusCodeC1ComboBox.Text));
            apiParam.Add("reportCate", new JValue(reportCateC1ComboBox.Text));

            // 次工程が設定されている伝票未発行の一覧を取得
            var webApi = new WebAPI();
            string url = apiUrl + "GetList" + "?sid=" + solutionIdShort + "&fid=" + formIdShort;
            var result = webApi.PostRequest(url, apiParam, LoginInfo.Instance.Token);
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

            c1TrueDBGridDT = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            c1TrueDBGrid.SetDataBinding(c1TrueDBGridDT, "", true);

            ChangeTopMessage("I0011", c1TrueDBGridDT.Rows.Count.ToString("#,###"));
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        /// <returns></returns>
        private (bool isOk, string msg) PrintDocument(DataTable tableG, DataTable tableS, DataTable tableK, DataTable tableK3630)
        {
            // 専用プリンタのドライバ（OutsideTrans、InsideTrans）がインストールされているかどうか
            // インストールされている場合は、即印刷。
            // インストールされてない場合は、プレビューを表示しプリンタを選択できるようにする
            bool isPrinterOutsideTrans = false;
            string printerName = ConfigurationManager.AppSettings["OutsideTrans"];
            foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (p == printerName) { isPrinterOutsideTrans = true; }
            }
            bool isPrinterInsideTrans = false;
            printerName = ConfigurationManager.AppSettings["InsideTrans"];
            foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (p == printerName) { isPrinterInsideTrans = true; }
            }

            /// <summary>
            /// 社外有償支給発行（外注）
            /// </summary>
            if (tableG.Rows.Count > 0)
            {
                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R029_NSupStockTrans.flxr", "R029_NSupStockTrans");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = tableG
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // 個別設定      
                ((C1.Win.FlexReport.TextField)report.Fields["材料売り1"]).Visible = false;
                ((C1.Win.FlexReport.TextField)report.Fields["材料売り2"]).Visible = false;
                ((C1.Win.FlexReport.ShapeField)report.Fields["割印"]).Visible = false;
                ((C1.Win.FlexReport.TextField)report.Fields["発行日1"]).Text = "発行日";
                ((C1.Win.FlexReport.TextField)report.Fields["発行日2"]).Text = "発行日";
                ((C1.Win.FlexReport.TextField)report.Fields["表示価格は税抜き価格です1"]).Visible = false;
                ((C1.Win.FlexReport.TextField)report.Fields["表示価格は税抜き価格です2"]).Visible = false;

                if (isPrinterOutsideTrans)
                {
                    // 即印刷
                    var bf = new BaseForm();
                    var p = new System.Drawing.Printing.PrinterSettings();
                    p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                    report.Render();
                    var print = bf.PrintReport(report, false, p);
                    if (print.IsOk == false)
                    {
                        return (false, "有償支給伝票印刷時にエラーが発生しました。");
                    }
                }
                else
                {
                    // プレビュー印刷
                    var bf = new BaseForm();
                    report.Render();
                    var print = bf.PrintReport(report);
                    if (print.IsOk == false)
                    {
                        return (false, "有償支給伝票印刷時にエラーが発生しました。");
                    }
                }
            }

            /// <summary>
            /// 社外有償支給発行（一般仕入先）
            /// </summary>
            if (tableS.Rows.Count > 0)
            {
                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R029_NSupStockTrans.flxr", "R029_NSupStockTrans");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = tableS
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // 個別設定            
                ((C1.Win.FlexReport.TextField)report.Fields["材料売り1"]).Visible = true;
                ((C1.Win.FlexReport.TextField)report.Fields["材料売り2"]).Visible = true;
                ((C1.Win.FlexReport.TextField)report.Fields["発行日1"]).Text = "支給年月日";
                ((C1.Win.FlexReport.TextField)report.Fields["発行日2"]).Text = "支給年月日";
                ((C1.Win.FlexReport.TextField)report.Fields["表示価格は税抜き価格です1"]).Visible = true;
                ((C1.Win.FlexReport.TextField)report.Fields["表示価格は税抜き価格です2"]).Visible = true;

                if (isPrinterOutsideTrans)
                {
                    // 即印刷
                    var bf = new BaseForm();
                    var p = new System.Drawing.Printing.PrinterSettings();
                    p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                    report.Render();
                    var print = bf.PrintReport(report, false, p);
                    if (print.IsOk == false)
                    {
                        return (false, "有償支給伝票印刷時にエラーが発生しました。");
                    }
                }
                else
                {
                    // プレビュー印刷
                    var bf = new BaseForm();
                    report.Render();
                    var print = bf.PrintReport(report);
                    if (print.IsOk == false)
                    {
                        return (false, "有償支給伝票印刷時にエラーが発生しました。");
                    }
                }
            }

            /// <summary>
            /// 社内移行発行（塗装）
            /// </summary>
            if (tableK3630.Rows.Count > 0)
            {
                // プレビューを表示するかどうかを判定
                bool isPreview = true;
                string printerNames = System.Configuration.ConfigurationManager.AppSettings["GoodsTagA6"];
                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == printerNames) { isPreview = false; }
                }

                // 現品票（A6版）印刷処理
                var result4 = PrintGoodsTagA6(tableK3630, isPreview);
                if (result4.isOk == false)
                {
                    return (false, "社内移行伝票印刷時にエラーが発生しました。");
                }

                //// レポート印刷
                //var report = new C1.Win.FlexReport.C1FlexReport();
                //report.Load(EXE_DIRECTORY + @"\Reports\R032_InsideTransPaint.flxr", "R032_InsideTransPaint");
                ////report.Load(@"\\sserv04\WORK\CS\AllMenuShortcut\CommonGoodsTag.flxr", "CommonGoodsTag");

                //// データソース設定
                //var ds = new C1.Win.FlexReport.DataSource
                //{
                //    Name = " ",
                //    ConnectionString = ConfigurationManager.AppSettings["C1ReportConnectionString"],
                //    Recordset = tableK3630
                //};
                //report.DataSources.Add(ds);
                //report.DataSourceName = ds.Name;

                ////// 個別設定            
                ////((C1.Win.FlexReport.Field)report.Fields["機種名１"]).Text = productName;
                ////((C1.Win.FlexReport.Field)report.Fields["機種名２"]).Text = productName;
                ////((C1.Win.FlexReport.Field)report.Fields["機種名３"]).Text = productName;

                //if (isPrinterInsideTrans)
                //{
                //    // 即印刷
                //    var bf = new BaseForm();
                //    var p = new System.Drawing.Printing.PrinterSettings();
                //    p.PrinterName = ConfigurationManager.AppSettings["InsideTrans"];
                //    report.Render();
                //    var print = bf.PrintReport(report, false, p);
                //    if (print.IsOk == false)
                //    {
                //        return (false, "社内移行伝票印刷時にエラーが発生しました。");
                //    }
                //}
                //else
                //{
                //    // プレビュー印刷
                //    var bf = new BaseForm();
                //    report.Render();
                //    var print = bf.PrintReport(report);
                //    if (print.IsOk == false)
                //    {
                //        return (false, "社内移行伝票印刷時にエラーが発生しました。");
                //    }
                //}
            }

            /// <summary>
            /// 社内移行発行（塗装以外）
            /// </summary>
            if (tableK.Rows.Count > 0)
            {
                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R030_InsideTrans.flxr", "R030_InsideTrans");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = tableK
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // 個別設定            
                //((C1.Win.FlexReport.Field)report.Fields["機種名１"]).Text = productName;
                //((C1.Win.FlexReport.Field)report.Fields["機種名２"]).Text = productName;
                //((C1.Win.FlexReport.Field)report.Fields["機種名３"]).Text = productName;

                if (isPrinterInsideTrans)
                {
                    // 即印刷
                    var bf = new BaseForm();
                    var p = new System.Drawing.Printing.PrinterSettings();
                    p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["InsideTrans"];
                    report.Render();
                    var print = bf.PrintReport(report, false, p);
                    if (print.IsOk == false)
                    {
                        return (false, "社内移行伝票印刷時にエラーが発生しました。");
                    }
                }
                else
                {
                    // プレビュー印刷
                    var bf = new BaseForm();
                    report.Render();
                    var print = bf.PrintReport(report);
                    if (print.IsOk == false)
                    {
                        return (false, "社内移行伝票印刷時にエラーが発生しました。");
                    }
                }
            }
            return (true,"");
        }



        #endregion  ＜その他処理 END＞




    }
}
