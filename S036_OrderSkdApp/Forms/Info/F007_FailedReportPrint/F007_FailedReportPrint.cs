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
    /// 不適合品　レポート印刷画面
    /// </summary>
    public partial class F007_FailedReportPrint : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F007/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// c1TrueDBGri用DataTable
        /// </summary>
        private DataTable c1TrueDBGridDT = new DataTable();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F007_FailedReportPrint(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "不適合品　レポート印刷";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F007_FailedReportPrint_Load(object sender, EventArgs e)
        {
            try
            {
                //C1TextBoxをリスト化
                //AddControlListII(productCodeC1TextBox, null, "", true, enumCate.Key);

                //C1CombBoxをリスト化
                //AddControlListII(lineC1ComboBox, lineNameC1TextBox, "", false, enumCate.無し);

                //C1NumericEditをリスト化
                //AddControlListII(ltC1NumericEdit, null, "0", false, enumCate.無し);
                //AddControlListII(ltC1NumericEdit, null, null, false, enumCate.無し);

                //C1DateEditをリスト化
                AddControlListII(startDateC1DateEdit, null, DatetimeFC.GetBeginOfMonth(DateTime.Today).ToString(), false, enumCate.無し);
                AddControlListII(endDateC1DateEdit, null, DatetimeFC.GetEndOfMonth(DateTime.Today).ToString(), false, enumCate.無し);

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
                //SetProcessC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "対象リストの印刷ボタンを押してください。";

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
        /// 処理区分  コンボボックスセット
        /// </summary>
        //private void SetProcessC1ComboBox()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ID", typeof(string));
        //    dt.Columns.Add("NAME", typeof(string));
        //    dt.Rows.Add((int)ConstantProcess.登録ｌ修正, ConstantProcess.登録ｌ修正);
        //    dt.Rows.Add((int)ConstantProcess.削除, ConstantProcess.削除);
        //    ControlAF.SetC1ComboBox(processCateC1ComboBox, dt, 30, 100);
        //}

        #endregion  ＜コンボボックス設定処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            // ファンクションキーの使用可否設定
            TopMenuEnable("F8", false);
            TopMenuEnable("F10", false);
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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = startDateC1DateEdit;
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
                    case "productCodeC1TextBox":
                        productSearchBt_Click(sender, e);
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
        /// 機種コード検索ボタン押下時
        /// </summary>
        private void productSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F901_ProductMCommonSearch("F901_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        //STProductCode = form.row.Cells["機種コード"].Value.ToString();
                        //STProductName = form.row.Cells["機種名"].Value.ToString();

                        //productCodeC1TextBox.Text = form.row.Cells["機種コード"].Value.ToString();
                        //productNameC1TextBox.Text = form.row.Cells["機種名"].Value.ToString();
                    }
                }
                //ActiveControl = productCodeC1TextBox;
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
        /// 印刷前 必須項目チェック
        /// </summary>
        protected override bool Requir_F08(object sender, EventArgs e)
        {
            try
            {
                return RequirF08();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 印刷前 整合性チェック
        /// </summary>
        protected override bool Consis_F08(object sender, EventArgs e)
        {
            try
            {
                return ConsisF08();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 印刷前 エラーチェック
        /// </summary>
        protected override bool ErrCK_F08(object sender, EventArgs e)
        {
            try
            {
                return ErrCKF08();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 印刷（F8）
        /// </summary>
        protected override void F8Bt_Click(object sender, EventArgs e)
        {
            try
            {
                PrintProc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;
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
                ActionProc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Excelボタンクリック
        /// </summary>
        protected override void F12Bt_Click(object sender, EventArgs e)
        {
            try
            {
                EXCELProc();
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

        /// <summary>
        /// タイマー起動処理
        /// </summary>
        protected override void ActuationTimer(string timerName)
        {
            try
            {
                switch (timerName)
                {
                    case "sampleTimer1":
                        // イベント1
                        break;
                    case "sampleTimer2":
                        // イベント2
                        break;
                    case "sampleTimer3":
                        // イベント3
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 
        /// <summary>
        /// 不適合品 社内移行　印刷ボタン押下時
        /// </summary>
        private void printButton1_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // 期間チェック
                if (startDateC1DateEdit == null || startDateC1DateEdit.Text == "")
                {
                    this.ActiveControl = startDateC1DateEdit;
                    ChangeTopMessage("W0007", "不適合品社内移行印刷に期間");
                    return;
                }

                if (endDateC1DateEdit == null || endDateC1DateEdit.Text == "")
                {
                    this.ActiveControl = endDateC1DateEdit;
                    ChangeTopMessage("W0007", "不適合品社内移行印刷に期間");
                    return;
                }

                if (DateTime.Parse(startDateC1DateEdit.Text) > DateTime.Parse(endDateC1DateEdit.Text))
                {
                    ChangeTopMessage("W0021", "検索期間");
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // データ抽出
                apiParam.RemoveAll();
                apiParam.Add("startDate", new JValue(DateTime.Parse(startDateC1DateEdit.Text + " 0:00:00")));
                apiParam.Add("endDate", new JValue(DateTime.Parse(endDateC1DateEdit.Text + " 23:59:59")));

                var result = CallSansoWebAPI("POST", apiUrl + "GetInsideTransFailedList", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "社内移行データ一覧（不適合）抽出時に");
                    return;
                }

                if ((result.Table == null) || (result.Table.Rows.Count <= 0))
                {
                    ChangeTopMessage("I0005");
                    return;
                }

                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R034_InsideTransFailedList.flxr", "R034_InsideTransFailedList");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (print.IsOk == false)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return;
                }

                ChangeTopMessage("I0009", "印刷処理が");
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
        /// 不適合品 未処理一覧　印刷ボタン押下時
        /// </summary>
        private void printButton2_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // データ抽出
                apiParam.RemoveAll();

                var result = CallSansoWebAPI("POST", apiUrl + "GetUntreatedFailedList", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "不適合未処理一覧抽出時に");
                    return;
                }

                if ((result.Table == null) || (result.Table.Rows.Count <= 0))
                {
                    ChangeTopMessage("I0005");
                    return;
                }

                // レポート印刷
                var report = new C1.Win.FlexReport.C1FlexReport();
                report.Load(EXE_DIRECTORY + @"\Reports\R035_UntreatedFailedList.flxr", "R035_UntreatedFailedList");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (print.IsOk == false)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return;
                }

                ChangeTopMessage("I0009", "印刷処理が");
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
        #endregion  ＜イベント処理 END＞

        #region ＜実行前チェック＞ 

        /// <summary>
        /// 印刷（F08）必須チェック
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        /// </summary>
        private bool RequirF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）整合性チェック
        /// </summary>
        /// <returns>True：整合性ＯＫ False：整合性を満たしていない項目がある</returns>
        private bool ConsisF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF08()
        {
            return true;
        }


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
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {
            if ((excelDt == null) || (excelDt.Rows.Count <= 0))
            {
                ChangeTopMessage("I0007");
                excelDt = null;
                return;
            }

            var param = new List<(int ColumnsNum, string Format, int? Width)>();
            //param.Add((1, "", 2400));
            //param.Add((4, "yyyy/m/d;@", 1200));
            //param.Add((6, "yyyy/m/d;@", 1200));

            var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, excelDt);
            var result = cef.CreateSaveExcelFile(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "エクセルデータ検索時に");
                return;
            }
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        private void PrintProc()
        {
            // マウスカーソル待機状態
            Cursor = Cursors.WaitCursor;




            // マウスカーソル待機状態を解除
            Cursor = Cursors.Default;
        }


        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        #endregion  ＜その他処理 END＞

    }
}
