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
    /// 入出庫チェックリスト
    /// </summary>
    public partial class F224_IOCheckList : BaseForm
    {
        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F224_IOCheckList(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "入出庫チェックリスト";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F224_IOCheckList_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(groupCodeC1ComboBox, groupCodeNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupCodeNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(userCodeC1ComboBox, userNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(userNameC1TextBox, null, "", false, enumCate.無し);
                var today = DateTime.Today;
                AddControlListII(dateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);

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

                SetGroupCodeC1ComboBox();
                SetUserCodeC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "右側のボタンを押すと、レポートが出力されます。";

                // クリア処理
                DisplayClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜起動処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 組立部門　コンボボックスセット
        /// </summary>
        private void SetGroupCodeC1ComboBox()
        {
            var result = GetGroupManufactDirect();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", "出庫先検索 " + result.Msg);
                    return;
                }
            }

            var dt = result.Table;
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(groupCodeC1ComboBox, dt);

        }

        /// <summary>
        /// 担当者　コンボボックスセット
        /// </summary>
        private void SetUserCodeC1ComboBox()
        {
            var result = GetStaffList();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", "担当者検索 " + result.Msg);
                    return;
                }
            }

            var dt = result.Table;
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "_";
            dr[1] = "すべて";
            dt.Rows.InsertAt(dr, 0);
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(userCodeC1ComboBox, dt);
        }

        #endregion  ＜コンボボックス設定処理 END＞


        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {

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
            userCodeC1ComboBox.SelectedIndex = 0;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = groupCodeC1ComboBox;
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

        #region ＜メイン処理＞ 

        /// <summary>
        /// HT処理分　印刷
        /// </summary>
        private void inputOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                var g = groupCodeErrorCheck();
                if (!g)
                {
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                if (groupCodeC1ComboBox.Text == "")
                {
                    ChangeTopMessage("W0007", "組立部門");
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                // コンボのチェック
                var isOk1 = CheckComboBoxListAll();
                if (isOk1 == false)
                {
                    ActiveControl = userCodeC1ComboBox;
                    return;
                }

                Cursor = Cursors.WaitCursor;

                ClearTopMessage();

                // HT処理分チェックリスト取得
                var result = GetInputOrder(false);
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "HT処理分 " + result.Msg);
                        return;
                    }
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "印刷");
                    return;
                }

                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R015_AccountsPayableCheckList.flxr"
                            , "R015_AccountsPayableCheckList");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (!print.IsOk)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// HT処理分　印刷
        /// </summary>
        private void inputOrderButton2_Click(object sender, EventArgs e)
        {
            try
            {
                var g = groupCodeErrorCheck();
                if (!g)
                {
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                if (groupCodeC1ComboBox.Text == "")
                {
                    ChangeTopMessage("W0007", "組立部門");
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                // コンボのチェック
                var isOk1 = CheckComboBoxListAll();
                if (isOk1 == false)
                {
                    ActiveControl = userCodeC1ComboBox;
                    return;
                }

                Cursor = Cursors.WaitCursor;

                ClearTopMessage();

                // HT処理分チェックリスト取得
                var result = GetInputOrder(true);
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "HT処理分 " + result.Msg);
                        return;
                    }
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "印刷");
                    return;
                }

                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R015_AccountsPayableCheckList.flxr"
                            , "R015_AccountsPayableCheckList");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (!print.IsOk)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 買掛チェックリスト　印刷
        /// </summary>
        private void accountsPayableButton_Click(object sender, EventArgs e)
        {
            try
            {
                var g = groupCodeErrorCheck();
                if (!g)
                {
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                if (groupCodeC1ComboBox.Text == "")
                {
                    ChangeTopMessage("W0007", "組立部門");
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                // コンボのチェック
                var isOk1 = CheckComboBoxListAll();
                if (isOk1 != false)
                {
                    ActiveControl = userCodeC1ComboBox;
                    return;
                }

                Cursor = Cursors.WaitCursor;

                ClearTopMessage();

                // 買掛チェックリスト取得
                var result = GetAccountsPayable();
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "買掛チェックリスト " + result.Msg);
                        return;
                    }
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "印刷");
                    return;
                }

                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R015_AccountsPayableCheckList.flxr", "R015_AccountsPayableCheckList");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // フィールド値設定
                var v = result.Table.Rows[0].Field<string>("dataCate") ?? "";
                if (v == "1")
                {
                    ((C1.Win.FlexReport.Field)report.Fields["データ区分名"]).Text = "一般仕入";
                }
                else if (v == "3")
                {
                    ((C1.Win.FlexReport.Field)report.Fields["データ区分名"]).Text = "外注仕入";
                }
                else if (v == "6")
                {
                    ((C1.Win.FlexReport.Field)report.Fields["データ区分名"]).Text = "有償支給";
                }
                else if (v == "5")
                {
                    ((C1.Win.FlexReport.Field)report.Fields["データ区分名"]).Text = "客先無償支給";
                }
                else
                {
                    ((C1.Win.FlexReport.Field)report.Fields["データ区分名"]).Text = "";
                }

                ((C1.Win.FlexReport.Field)report.Fields["作成日付"]).Format = "yy/MM/dd";
                ((C1.Win.FlexReport.TextField)report.Fields["入力順ラベル"]).Visible = false;

                Cursor = Cursors.Default;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (!print.IsOk)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// オンライン入力済原紙　印刷
        /// </summary>
        private void onlineInputStencilBt_Click(object sender, EventArgs e)
        {
            try
            {
                var g = groupCodeErrorCheck();
                if (!g)
                {
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                if (groupCodeC1ComboBox.Text == "")
                {
                    ChangeTopMessage("W0007", "組立部門");
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                Cursor = Cursors.WaitCursor;

                ClearTopMessage();

                using (var report = new C1.Win.FlexReport.C1FlexReport())
                {
                    report.Load(EXE_DIRECTORY + @"\Reports\R013_OnlineCompletedDocuStaff.flxr", "R013_OnlineCompletedDocuStaff");

                    var dt = new DataTable();
                    dt.Columns.Add("ダミー");
                    dt.Rows.Add("");

                    // データソース設定
                    var ds = new C1.Win.FlexReport.DataSource
                    {
                        Name = " ",
                        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                        Recordset = dt
                    };
                    report.DataSources.Add(ds);
                    report.DataSourceName = ds.Name;

                    // フィールド値設定
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門1"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名1"]).Text = groupCodeNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門2"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名2"]).Text = groupCodeNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門3"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名3"]).Text = groupCodeNameC1TextBox.Text;

                    Cursor = Cursors.Default;

                    // プレビュー印刷
                    report.Render();
                    var print = PrintReport(report);
                    if (!print.IsOk)
                    {
                        ChangeTopMessage("E0008", "印刷処理で");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// オンライン入力済原紙（担当者別）　印刷
        /// </summary>
        private void onlineInputPersonBt_Click(object sender, EventArgs e)
        {
            try
            {
                var g = groupCodeErrorCheck();
                if (!g)
                {
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                if (groupCodeC1ComboBox.Text == "")
                {
                    ChangeTopMessage("W0007", "組立部門");
                    ActiveControl = groupCodeC1ComboBox;
                    return;
                }

                // コンボのチェック
                var isOk1 = CheckComboBoxListAll();
                if (isOk1 == false)
                {
                    ActiveControl = userCodeC1ComboBox;
                    return;
                }

                if (userCodeC1ComboBox.Text == "_")
                {
                    ActiveControl = userCodeC1ComboBox;
                    ChangeTopMessage(1,"WARN", "「すべて」以外の担当者を選択してください");
                    return;
                }

                // 日付空欄時処理
                var dateEdit = dateC1DateEdit;
                var isOk2 = DateTime.TryParse(dateEdit.Text, out var dt1);
                if (!isOk2)
                {
                    ActiveControl = dateEdit;
                    ChangeTopMessage("W0013", "日付");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                ClearTopMessage();

                using (var report = new C1.Win.FlexReport.C1FlexReport())
                {
                    report.Load(EXE_DIRECTORY + @"\Reports\R013_OnlineCompletedDocuStaff.flxr", "R013_OnlineCompletedDocuStaff");

                    var dt = new DataTable();
                    dt.Columns.Add("ダミー");
                    dt.Rows.Add("");

                    // データソース設定
                    var ds = new C1.Win.FlexReport.DataSource
                    {
                        Name = " ",
                        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                        Recordset = dt
                    };
                    report.DataSources.Add(ds);
                    report.DataSourceName = ds.Name;

                    // フィールド値設定
                    string y = DateTime.Parse(dateC1DateEdit.Text).ToString("yyyy");
                    string m = DateTime.Parse(dateC1DateEdit.Text).ToString("MM");
                    string d = DateTime.Parse(dateC1DateEdit.Text).ToString("dd");
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門1"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名1"]).Text = groupCodeNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["担当者1"]).Text = userNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["年1"]).Text = y;
                    ((C1.Win.FlexReport.TextField)report.Fields["月1"]).Text = m;
                    ((C1.Win.FlexReport.TextField)report.Fields["日1"]).Text = d;
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門2"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名2"]).Text = groupCodeNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["担当者2"]).Text = userNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["年2"]).Text = y;
                    ((C1.Win.FlexReport.TextField)report.Fields["月2"]).Text = m;
                    ((C1.Win.FlexReport.TextField)report.Fields["日2"]).Text = d;
                    ((C1.Win.FlexReport.TextField)report.Fields["組立部門3"]).Text = groupCodeC1ComboBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["課別名3"]).Text = groupCodeNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["担当者3"]).Text = userNameC1TextBox.Text;
                    ((C1.Win.FlexReport.TextField)report.Fields["年3"]).Text = y;
                    ((C1.Win.FlexReport.TextField)report.Fields["月3"]).Text = m;
                    ((C1.Win.FlexReport.TextField)report.Fields["日3"]).Text = d;

                    Cursor = Cursors.Default;

                    // プレビュー印刷
                    report.Render();
                    var print = PrintReport(report);
                    if (!print.IsOk)
                    {
                        ChangeTopMessage("E0008", "印刷処理で");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// 組立部門チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool groupCodeErrorCheck()
        {
            var t = groupCodeC1ComboBox;

            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ログイン有効期限切れメッセージ表示
        /// </summary>
        private void ShowLoginMessageBox()
        {
            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                     $"再度、処理を実行してください",
                     "ログイン有効期限切れエラー",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
        }

        /// <summary>
        /// S1製造直接部門 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGroupManufactDirect()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F224/GetGroupManufactDirect?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }

        /// <summary>
        /// S1製造直接部門 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetStaffList()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F224/GetStaffList?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("userId", new JValue(LoginInfo.Instance.UserId.ToUpper() ?? ""));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }

        /// <summary>
        /// HT処理分チェックリスト 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetInputOrder(bool isNonSlip = false)
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F224/GetInputOrder?sid={solutionIdShort}&fid={formIdShort}";

            var groupCode = groupCodeC1ComboBox.Text ?? "";
            var date = dateC1DateEdit.Text ?? "";
            var password = userCodeC1ComboBox.Text == "_" ? "" : userCodeC1ComboBox.Text;

            JObject param = new JObject();
            param.Add("groupCode", new JValue(groupCode));
            param.Add("date", new JValue(date));
            param.Add("password", new JValue(password));
            param.Add("isNonSlip", new JValue(isNonSlip));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }

        /// <summary>
        /// 買掛チェックリスト 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetAccountsPayable()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F224/GetAccountsPayable?sid={solutionIdShort}&fid={formIdShort}";

            // 未使用機能なので一部コメント化。使用する際にはuserCodeC1ComboBoxへ変更し、API側も「すべて」への対応すること
            var groupCode = groupCodeC1ComboBox.Text ?? "";
            var date = dateC1DateEdit.Text ?? "";
            //var password = userCodeC1TextBox.Text ?? "";
            var machineName = LoginInfo.Instance.MachineCode ?? "";

            JObject param = new JObject();
            param.Add("groupCode", new JValue(groupCode));
            param.Add("date", new JValue(date));
            //param.Add("password", new JValue(password));
            param.Add("machineName", new JValue(machineName));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }
        #endregion  ＜その他処理 END＞

    }
}
