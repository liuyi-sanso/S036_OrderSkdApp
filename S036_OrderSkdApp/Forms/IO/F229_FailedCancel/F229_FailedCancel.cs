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
    /// 不適合品 返品入力画面
    /// </summary>
    public partial class F229_FailedCancel : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F229/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 管理番号の変更前保管エリア
        /// </summary>
        private string stControlNo = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F229_FailedCancel(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "不適合品 返品";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F229_FailedCancel_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(controlNoC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(cancelSupCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(cancelSupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outSupCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDate2C1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, "", false, enumCate.無し);

                AddControlListII(outSupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(productCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(productNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(statusC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(majorClassificatC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(cancelRemarksC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(midClassificatC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(failedCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.無し);
                
                AddControlListII(judgmentC1ComboBox, null, "", true, enumCate.無し);

                AddControlListII(failedNumC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(unitPriceC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(priceC1NumericEdit, null, "", false, enumCate.無し);
                AddControlListII(outNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outUnitPriceC1NumericEdit, null, "", false, enumCate.無し);
                AddControlListII(processUnitPriceC1NumericEdit, null, "", false, enumCate.無し);
                AddControlListII(outPriceC1NumericEdit, null, "", false, enumCate.無し);

                AddControlListII(outDateC1DateEdit, null, "", true, enumCate.無し);

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
                SetjudgmentC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後に実行（F10）を押してください。";

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
        /// 判定  コンボボックスセット
        /// </summary>
        private void SetjudgmentC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("仕入先返品", "0");
            dt.Rows.Add("社内返品", "1");
            ControlAF.SetC1ComboBox(judgmentC1ComboBox, dt, 0, 150, "NAME", "NAME", true);
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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            c1TrueDBGrid.SetDataBinding(null, "", true);
            stControlNo = "";
            outDateC1DateEdit.Value = DateTime.Today;
            judgmentC1ComboBox.SelectedIndex = -1;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = controlNoC1TextBox;
        }

        /// <summary>
        /// 明細部分クリア処理
        /// </summary>
        private void DisplayClearDetail()
        {
            // コントロールの一括クリア処理
            foreach (var v in controlListII.Where(v => v.Control.Name != "controlNoC1TextBox"))
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

            // エクセルファイル用DataTable
            excelDt = null;

            c1TrueDBGrid.SetDataBinding(null, "", true);
            outDateC1DateEdit.Value = DateTime.Today;
            judgmentC1ComboBox.SelectedIndex = -1;
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
                if (Check.HasBanChar(c.Text).Result == false)
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
                    if (Check.HasBanChar(ctl.Text).Result == false)
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
        /// 管理番号 検証時
        /// </summary>
        private void controlNoC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckControlNo();
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
        /// 管理番号 検証後
        /// </summary>
        private void controlNoC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(controlNoC1TextBox.Text))
                {
                    return;
                }

                if (stControlNo == controlNoC1TextBox.Text)
                {
                    return;
                }

                stControlNo = controlNoC1TextBox.Text;

                c1TrueDBGrid.SetDataBinding(null, "", true);
                DisplayClearDetail();

                // 入出庫ファイル（管理データ）抽出
                apiParam.RemoveAll();
                apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));
                var result = CallSansoWebAPI("POST", apiUrl + "GetIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "入出庫ファイル検索時に");
                    return;
                }

                if (result.Table != null && result.Table.Rows.Count >= 1)
                {
                    var dr = result.Table.Rows[0];
                    partsCodeC1TextBox.Text = dr["partsCode"].ToString();
                    partsNameC1TextBox.Text = dr["partsName"].ToString();
                    outSupCodeC1TextBox.Text = dr["supCode"].ToString();
                    outSupNameC1TextBox.Text = dr["supName"].ToString();
                    jyuyoyosokuCodeC1TextBox.Text = dr["jyuyoyosokuCode"].ToString();
                    poCodeC1TextBox.Text = dr["poCode"].ToString();
                    productCodeC1TextBox.Text = dr["productCode"].ToString();
                    productNameC1TextBox.Text = dr["productName"].ToString();
                    outNumC1TextBox.Text = dr["delivNum"].ToString();
                    statusC1TextBox.Text = "9：不良";
                    majorClassificatC1TextBox.Text = dr["majorClassificat"].ToString();
                    midClassificatC1TextBox.Text = dr["midClassificat"].ToString();
                    cancelRemarksC1TextBox.Text = dr["cancelRemarks"].ToString();
                    groupCodeC1TextBox.Text = dr["groupCode"].ToString();
                    groupNameC1TextBox.Text = dr["groupName"].ToString();
                    dataCateC1TextBox.Text = dr["dataCate"].ToString();
                    dataCateNameC1TextBox.Text = dr["dataCateName"].ToString();
                    failedCodeC1TextBox.Text = dr["failedCode"].ToString();
                    outUnitPriceC1NumericEdit.Value = dr["unitPrice"].ToString();
                    processUnitPriceC1NumericEdit.Value = 
                        dr["processUnitPrice"].ToString() == "" ? "0" : dr["processUnitPrice"].ToString();
                    outPriceC1NumericEdit.Value = dr["price"].ToString();
                    stockCateC1TextBox.Text = dr["stockCate"].ToString();
                    stockCateNameC1TextBox.Text = dr["stockCateName"].ToString();
                    outDate2C1TextBox.Text = DateTime.Parse(dr["acceptDate"].ToString()).ToString("yyyy/MM/dd");
                    entryDateC1TextBox.Text = DateTime.Parse(dr["createDate"].ToString()).ToString("yyyy/MM/dd");

                    isRunValidating = false;
                    failedNumC1NumericEdit.Value = dr["delivNum"].ToString();
                    unitPriceC1NumericEdit.Value = dr["unitPrice"].ToString();
                    priceC1NumericEdit.Value = dr["price"].ToString();
                    isRunValidating = true;

                    if (partsCodeC1TextBox.Text != "")
                    {
                        // 仕入先単価抽出
                        apiParam.RemoveAll();
                        apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                        var result2 = CallSansoWebAPI("POST", apiUrl + "GetUnitPriceMst", apiParam);
                        if (result2.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "単価マスタ抽出時に");
                            return;
                        }
                        if (result2.Table != null && result2.Table.Rows.Count >= 1)
                        {
                            c1TrueDBGrid.SetDataBinding(result2.Table, "", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 返品先 検証時
        /// </summary>
        private void cancelSupCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckCancelSupCode();
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
        /// 返品先 検証後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelSupCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                cancelSupNameC1TextBox.Text = "";

                if (string.IsNullOrEmpty(cancelSupCodeC1TextBox.Text))
                {
                    return;
                }

                // 社名、部門名抽出
                if (judgmentC1ComboBox.SGetText(1) == "0")
                {
                    // 仕入先返品
                    var result = CallSansoWebAPI("POST", apiUrl + "GetAddressMst", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "住所録マスタ検索時に");
                        return;
                    }

                    if (result.Table != null && result.Table.Rows.Count > 0)
                    {
                        cancelSupNameC1TextBox.Text = result.Table.Rows[0]["cancelSupName"].ToString();
                    }
                }
                else if (judgmentC1ComboBox.SGetText(1) == "1")
                {
                    // 社内返品
                    var result = CallSansoWebAPI("POST", apiUrl + "GetGroupMst", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "部門マスタ検索時に");
                        return;
                    }

                    if (result.Table != null && result.Table.Rows.Count > 0)
                    {
                        cancelSupNameC1TextBox.Text = result.Table.Rows[0]["cancelSupName"].ToString();
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
        /// 数量 検証後
        /// </summary>
        private void failedNumC1NumericEdit_Validated(object sender, EventArgs e)
        {
            // 金額再計算
            CalculationPrice();
        }

        /// <summary>
        /// 単価 検証後
        /// </summary>
        private void unitPriceC1NumericEdit_Validated(object sender, EventArgs e)
        {
            // 金額再計算
            CalculationPrice();
        }

        /// <summary>
        /// 備考 検証中
        /// </summary>
        private void remarksC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckRemarks();
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
            // 管理番号チェック
            var isOk1 = ErrorCheckControlNo();
            if (isOk1 == false)
            {
                ActiveControl = controlNoC1TextBox;
                return false;
            }

            // 返品先チェック
            var isOk2 = ErrorCheckCancelSupCode();
            if (isOk2 == false)
            {
                ActiveControl = cancelSupCodeC1TextBox;
                return false;
            }

            // 備考チェック
            var isOk3 = ErrorCheckRemarks();
            if (isOk3 == false)
            {
                ActiveControl = remarksC1TextBox;
                return false;
            }

            // 数量チェック
            if (decimal.Parse(failedNumC1NumericEdit.Text) <= 0) 
            {
                ChangeTopMessage("W0016", "数量に０");
                ActiveControl = failedNumC1NumericEdit;
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
                // 入出庫ファイルのデータ区分作成
                string dataCate = dataCateC1TextBox.Text;
                if (judgmentC1ComboBox.SGetText(1) == "0")
                {
                    // 仕入先返品
                    var result2 = CallSansoWebAPI("POST", apiUrl + "GetAddressMst", apiParam);
                    if (result2.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "住所録マスタ検索時に");
                        return;
                    }

                    if (result2.Table != null && result2.Table.Rows.Count > 0)
                    {
                        if (result2.Table.Rows[0]["cancelSupCate"].ToString() == "S")
                        {
                            dataCate = "1";
                        }
                        else if (result2.Table.Rows[0]["cancelSupCate"].ToString() == "G")
                        {
                            dataCate = "3";
                        }
                        else if (result2.Table.Rows[0]["cancelSupCate"].ToString() == "K")
                        {
                            dataCate = "2";
                        }
                        else if (result2.Table.Rows[0]["cancelSupCate"].ToString() == "M")
                        {
                            dataCate = "1";
                        }
                        else 
                        {
                            ChangeTopMessage("E0008", "返品先の仕入先区分検索時に");
                            return;
                        }

                    }
                }
                else if (judgmentC1ComboBox.SGetText(1) == "1")
                {
                    // 社内返品
                    var result2 = CallSansoWebAPI("POST", apiUrl + "GetGroupMst", apiParam);
                    if (result2.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "部門マスタ検索時に");
                        return;
                    }

                    if (result2.Table != null && result2.Table.Rows.Count > 0)
                    {
                        dataCate = "2";
                    }
                }
                else
                {
                    // 処理なし
                }


                isRunValidating = false;
                ClearTopMessage();

                var dialog = MessageBox.Show("入力された内容で、不適合返品登録を行います。よろしいですか？",
                                 "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                 MessageBoxDefaultButton.Button2);
                if (dialog == DialogResult.No)
                {
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                apiParam.RemoveAll();
                apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));
                apiParam.Add("judgment", new JValue(judgmentC1ComboBox.Text));
                apiParam.Add("cancelSupCode", new JValue(cancelSupCodeC1TextBox.Text));
                apiParam.Add("failedNum", new JValue(failedNumC1NumericEdit.Text));
                apiParam.Add("unitPrice", new JValue(unitPriceC1NumericEdit.Text));
                apiParam.Add("price", new JValue(priceC1NumericEdit.Text));
                apiParam.Add("outDate", new JValue(outDateC1DateEdit.Text));
                apiParam.Add("remarks", new JValue(remarksC1TextBox.Text));

                apiParam.Add("dataCate", new JValue(dataCate));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("partsName", new JValue(partsNameC1TextBox.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1TextBox.Text));
                apiParam.Add("failedCode", new JValue(failedCodeC1TextBox.Text));
                apiParam.Add("outSupCode", new JValue(outSupCodeC1TextBox.Text));
                apiParam.Add("processUnitPrice", new JValue(processUnitPriceC1NumericEdit.Text));

                apiParam.Add("majorClassificat", new JValue(majorClassificatC1TextBox.Text));
                apiParam.Add("midClassificat", new JValue(midClassificatC1TextBox.Text));
                apiParam.Add("cancelRemarks", new JValue(cancelRemarksC1TextBox.Text));

                var result = CallSansoWebAPI("POST", apiUrl + "CreateCancelIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "不適合返品登録時に");
                    return;
                }

                if (judgmentC1ComboBox.Text == "仕入先返品" &&
                   (cancelSupCodeC1TextBox.Text != "1962" && cancelSupCodeC1TextBox.Text != "1963" && 
                    cancelSupCodeC1TextBox.Text != "1964"))
                {
                    // 返品納品書データ抽出
                    apiParam.RemoveAll();
                    apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));

                    var result2 = CallSansoWebAPI("POST", apiUrl + "GetReturnDelivSlip", apiParam);
                    if (result2.IsOk == false || result2.Table == null || result2.Table.Rows.Count <= 0)
                    {
                        // 処理なし
                    }
                    else 
                    {
                        // 返品納品書印刷
                        using (var report = new C1.Win.FlexReport.C1FlexReport())
                        {
                            report.Load(EXE_DIRECTORY + @"\Reports\R033_ReturnDelivSlip.flxr", "R033_ReturnDelivSlip");

                            var ds = new C1.Win.FlexReport.DataSource
                            {
                                Name = " ",
                                ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                                Recordset = result2.Table
                            };
                            report.DataSources.Add(ds);
                            report.DataSourceName = ds.Name;

                            // フィールド値設定
                            var dr = result2.Table.Rows[0];

                            if (dr.Field<string>("返品先区分") == "S")
                            {
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル"]).Text = "納　品　書（控）";
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル２"]).Text = "　納　品　書　";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード"]).Text = "060";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード2"]).Text = "060";
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値"]).Text =
                                                                            (decimal.Parse(dr.Field<string>("返品先コード")) +
                                                                             decimal.Parse(dr.Field<string>("入庫数")) +
                                                                             decimal.Parse(dr.Field<string>("単価")) +
                                                                             60).ToString();

                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル1"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル2"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル1"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル2"]).Visible = false;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス522"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス518"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス519"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス592"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス583"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル520"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル521"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル498"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル593"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル516"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線589"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線588"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線587"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コードラベル"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コードラベル"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値"]).Visible = true;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス529"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス525"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス526"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス534"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル528"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル527"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル530"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル535"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル524"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線533"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線532"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線531"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値2"]).Visible = false;
                            }
                            else if (dr.Field<string>("返品先区分") == "G")
                            {
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル"]).Text = "加工外注納品書（控）";
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル２"]).Text = "加工外注納品書";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード"]).Text = "083";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード2"]).Text = "083";
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値2"]).Text =
                                                                        (decimal.Parse(dr.Field<string>("返品先コード")) +
                                                                         decimal.Parse(dr.Field<string>("入庫数")) +
                                                                         decimal.Parse(dr.Field<string>("単価"))).ToString();

                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル1"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル2"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル1"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル2"]).Visible = true;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス522"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス518"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス519"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス592"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス583"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル520"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル521"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル498"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル593"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル516"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線589"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線588"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線587"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コードラベル"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値"]).Visible = false;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス529"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス525"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス526"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス534"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル528"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル527"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル530"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル535"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル524"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線533"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線532"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線531"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値2"]).Visible = true;

                            }
                            else
                            {
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル"]).Text = "納　品　書（控）";
                                ((C1.Win.FlexReport.TextField)report.Fields["タイトル２"]).Text = "　納　品　書　";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード"]).Text = "060";
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コード2"]).Text = "060";
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値"]).Text =
                                                                            (decimal.Parse(dr.Field<string>("返品先コード")) +
                                                                             decimal.Parse(dr.Field<string>("入庫数")) +
                                                                             decimal.Parse(dr.Field<string>("単価")) +
                                                                             60).ToString();

                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル1"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["印ラベル2"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル1"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["御中ラベル2"]).Visible = false;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス522"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス518"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス519"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス592"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス583"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル520"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル521"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル498"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル593"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル516"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線589"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線588"]).Visible = true;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線587"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コードラベル"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["科目コードラベル"]).Visible = true;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値"]).Visible = true;

                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス529"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス525"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス526"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["ボックス534"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル528"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル527"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル530"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル535"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["ラベル524"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線533"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線532"]).Visible = false;
                                ((C1.Win.FlexReport.ShapeField)report.Fields["直線531"]).Visible = false;
                                ((C1.Win.FlexReport.TextField)report.Fields["照合値2"]).Visible = false;
                            }

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

                DisplayClear();

                ChangeTopMessage("I0001", "不適合返品");
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
        /// <summary>
        /// エラーチェック  管理番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckControlNo()
        {
            // 未入力時処理
            var s = controlNoC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            if (stControlNo == s.Text)
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 入出庫ファイルチェック
            apiParam.RemoveAll();
            apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));
            var result = CallSansoWebAPI("POST", apiUrl + "GetIOFile", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "入出庫ファイル検索時に");
                return false;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "入力された管理番号は、入出庫ファイル");
                return false;
            }

            // 不適合在庫マスタ抽出
            apiParam.RemoveAll();
            apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));
            var result2 = CallSansoWebAPI("POST", apiUrl + "GetFailedStockMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "不適合在庫マスタ抽出時に");
                return false;
            }

            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "入力された管理番号は、不適合在庫マスタ");
                return false;
            }
            else
            {
                if (result2.Table.Rows[0]["judgment"].ToString() == "")
                {
                    ChangeTopMessage(1, "ERR", "品管ジャッジがまだされていません。");
                    return false;
                }
            }

            // 不適合在庫マスタ調達抽出
            apiParam.RemoveAll();
            apiParam.Add("controlNo", new JValue(controlNoC1TextBox.Text));
            var result3 = CallSansoWebAPI("POST", apiUrl + "GetFailedStockMstProcurement", apiParam);
            if (result3.IsOk == false)
            {
                ChangeTopMessage("E0008", "不適合在庫マスタ調達抽出時に");
                return false;
            }

            if (result3.Table == null || result3.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "入力された管理番号は、調達の不適合在庫マスタ");
                return false;
            }
            else
            {
                if (result3.Table.Rows[0]["judgment"].ToString() == "1")
                {
                    ChangeTopMessage(1, "ERR", "既に処理が完了しています。");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  返品先
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckCancelSupCode()
        {
            // 未入力時処理
            var s = cancelSupCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 住所録マスタor部門マスタチェック
            apiParam.RemoveAll();
            apiParam.Add("cancelSupCode", new JValue(cancelSupCodeC1TextBox.Text));

            if (judgmentC1ComboBox.Text == "")
            {
                cancelSupCodeC1TextBox.Text = "";
                ChangeTopMessage(1, "ERR", "返品先より先に判定区分を入力してください。");
                return false;
            }
            else if (judgmentC1ComboBox.SGetText(1) == "0")
            {
                // 仕入先返品
                var result = CallSansoWebAPI("POST", apiUrl + "GetAddressMst", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "住所録マスタ検索時に");
                    return false;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "入力された返品先は、住所録マスタ");
                    return false;
                }
            }
            else if (judgmentC1ComboBox.SGetText(1) == "1")
            {
                // 社内返品
                var result = CallSansoWebAPI("POST", apiUrl + "GetGroupMst", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部門マスタ検索時に");
                    return false;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "入力された返品先は、部門マスタ");
                    return false;
                }
            }
            else
            {
                cancelSupCodeC1TextBox.Text = "";
                ChangeTopMessage(1, "ERR", "返品先より先に判定区分を入力してください。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 金額再計算
        /// </summary>
        private void CalculationPrice()
        {
            decimal num1 = 0m;
            decimal num2 = 0m;

            var isOk1 = decimal.TryParse(failedNumC1NumericEdit.Text, out num1);
            if (!isOk1)
            {
                return;
            }

            var isOk2 = decimal.TryParse(unitPriceC1NumericEdit.Text, out num2);
            if (!isOk2)
            {
                return;
            }

            if (num1 != 0 && num2 != 0)
            {
                priceC1NumericEdit.Value = Math.Round((num1 * num2));
            }
        }

        /// <summary>
        /// エラーチェック  備考
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckRemarks()
        {
            // 未入力時処理
            var s = remarksC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }
        #endregion  ＜その他処理 END＞

        /// <summary>
        /// 単価検証中
        /// </summary>
        private void unitPriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckUnitPrice();
                if (isOk == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
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
        /// エラーチェック  単価
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckUnitPrice()
        {
            // 未入力チェック
            var s = processUnitPriceC1NumericEdit;
            if (s.Text == "")
            {
                return true;
            }

            // 数値か
            var chk1 = Check.IsNumeric(s.Text);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", s.Label.Text + "には");
                return false;
            }

            decimal value = decimal.Parse(s.Text);

            // 範囲
            if (value < 0m)
            {
                ChangeTopMessage("W0016", s.Label.Text + "にはマイナス");
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 7, 2);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", s.Label.Text + "の" + chk2.Msg);
                return false;
            }

            return true;
        }
    }
}
