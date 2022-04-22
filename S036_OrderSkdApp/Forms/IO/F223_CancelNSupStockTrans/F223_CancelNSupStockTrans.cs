using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SansoBase;
using SansoBase.Common;
using C1.Win.C1Input;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 返品有償受付
    /// </summary>
    public partial class F223_CancelNSupStockTrans : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// 処理年月　開始日
        /// </summary>
        private DateTime startDate = SansoBase.DatetimeFC.GetBeginOfMonth(DateTime.Today);

        /// <summary>
        /// 処理年月　終了日
        /// </summary>
        private DateTime endDate = SansoBase.DatetimeFC.GetEndOfMonth(DateTime.Today);

        /// <summary>
        /// 変更前 部品コード
        /// </summary>
        private string stPartsCode = "";

        /// <summary>
        /// 変更前 出庫先
        /// </summary>
        private string stSupCode = "";

        /// <summary>
        /// 需要予測 保管
        /// </summary>
        private string jyuyoyosoku = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F223_CancelNSupStockTrans(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "返品有償受付";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F223_CancelNSupStockTrans_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1ComboBox, groupNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDateC1DateEdit, null, null, true, enumCate.無し);
                AddControlListII(outNumC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(outPriceC1NumericEdit, null, null, false, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supOsrcCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(thisMonthInNumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(thisMonthOutNumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(thisMonthStockNumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(transCateC1ComboBox, transCateC1TextBox, "", true, enumCate.無し);
                AddControlListII(transCateC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(unitPriceC1NumericEdit, null, "", true, enumCate.無し);

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
                SetStockCateC1ComboBox();
                SetTransCateC1ComboBox();

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
        /// 課別コード　コンボボックスセット
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
                    ChangeTopMessage(1, "WARN", result.Msg);
                    return;
                }
            }

            var dt = result.Table;
            if (dt == null || dt.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "ユーザ", "マスタ");
                return;
            }

            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(groupCodeC1ComboBox, dt);
        }

        /// <summary>
        /// 在庫P　コンボボックスセット
        /// </summary>
        private void SetStockCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("", "素材在庫");
            dt.Rows.Add("Z", "完成品在庫");

            ControlAF.SetC1ComboBox(stockCateC1ComboBox, dt, stockCateC1ComboBox.Width,
                stockCateNameC1TextBox.Width, "ID", "NAME", false);
        }

        /// <summary>
        /// 支給区分　コンボボックスセット
        /// </summary>
        private void SetTransCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("", "");
            dt.Rows.Add("Y", "有償");
            dt.Rows.Add("M", "無償");

            ControlAF.SetC1ComboBox(transCateC1ComboBox, dt, transCateC1ComboBox.Width,
                transCateC1ComboBox.Width, "ID", "NAME", false);
        }
        #endregion  ＜コンボボックス設定処理 END＞

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

            c1TrueDBGrid.SetDataBinding(null, "", true);
            outDateC1DateEdit.Text = DateTime.Today.ToShortDateString();
            groupCodeC1ComboBox.SelectedIndex = 0;
            SetExecuteDate(groupCodeC1ComboBox.Text);
            stSupCode = "";
            stPartsCode = "";

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

                    case "supCodeC1TextBox":
                        supSearchBt_Click(sender, e);
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
                        ActiveControl = ctl;
                        ChangeTopMessage("W0018");
                        returnFlg = false;
                    }
                    // コンボボックスリストの存在チェック
                    else if (ControlAF.CheckComboBoxList(ctl,
                        (v.SubControl != null ? ((C1TextBox)v.SubControl) : null)) == false)
                    {
                        ActiveControl = ctl;
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
        /// 部品コード検索ボタン押下時
        /// </summary>
        private void partsSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        partsCodeC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString().TrimEnd();
                        partsNameC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                        ClearTopMessage();
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
        /// 出庫先検索ボタン押下時
        /// </summary>
        private void supSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        supNameC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
                        ClearTopMessage();
                    }
                }
                ActiveControl = supCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 需要予測番号 検証時
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = JyuyoyosokuCodeErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                jyuyoyosoku = jyuyoyosokuCodeC1TextBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コード 検証時
        /// </summary>
        private void partsCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = PartsCodeErrorCheck();
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
        /// 部品コード 検証後
        /// </summary>
        private void partsCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false
                    || stPartsCode == t.Text)
                {
                    return;
                }

                partsNameC1TextBox.Text = "";

                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                var param = new SansoBase.PartsMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, t.Text));
                param.SetDBName("製造調達");
                var result1 = CommonAF.ExecutSelectSQL(param);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部品マスタ検索時に");
                    ActiveControl = t;
                    return;
                }
                partsNameC1TextBox.Text = result1.Table.Rows[0].Field<string>("部品名") ?? "";

                stockCateC1ComboBox.SelectedIndex = -1;
                transCateC1ComboBox.SelectedIndex = 1;
                entryDateC1TextBox.Text = DateTime.Today.ToShortDateString();

                // 入出工程単価情報設定
                SetGridViewInfo();

                // 在庫情報設定
                SetStockInfo();

                // 仕入先入力後に部品変更した場合
                if (string.IsNullOrEmpty(supCodeC1TextBox.Text) == false)
                {
                    // 単価マスタ取得
                    var result2 = GetUnitPrice();
                    if (result2.IsOk == false)
                    {
                        return;
                    }
                    unitPriceC1NumericEdit.Text = result2.Table.Rows[0]["TransUnitPrice"].ToString();
                }

                stPartsCode = t.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 出庫先 検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 出庫先チェック
                var isOk = SupCodeErrorCheck();
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
        /// 出庫先 検証後
        /// </summary>
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false
                    || stSupCode == t.Text)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    supNameC1TextBox.Text = "";
                    return;
                }

                // 支給区分をY(有償)に設定
                transCateC1ComboBox.SelectedIndex = 1;

                // 単価マスタ取得
                var result = GetUnitPrice();
                if (result.IsOk == false)
                {
                    return;
                }
                unitPriceC1NumericEdit.Text = result.Table.Rows[0]["TransUnitPrice"].ToString();

                // 在庫P取得
                var result2 = GetStockCate();
                if (result2.IsOk == false)
                {
                    if (result2.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "在庫P取得 " + result2.Msg);
                        return;
                    }
                }
                if (result2.Table != null && result2.Table.Rows.Count >= 1)
                {
                    stockCateC1ComboBox.Text = result2.Table.Rows[0].Field<string>("stockCate") ?? "";
                }

                if (groupCodeC1ComboBox.Text == "3623")
                {
                    // 在庫PをZ(完成品在庫)に設定
                    stockCateC1ComboBox.SelectedIndex = 1;
                }

                // 在庫情報設定
                SetStockInfo();

                stSupCode = t.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 返品単価 検証時
        /// </summary>
        private void unitPriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = UnitPriceErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = unitPriceC1NumericEdit;
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
        /// 返品数量、返品単価 検証後
        /// </summary>
        private void C1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                // 返品金額の算出
                var outNum = outNumC1NumericEdit.Text;
                var unitPrice = unitPriceC1NumericEdit.Text;
                decimal d1 = outNum == "" ? 0 : System.Convert.ToDecimal(outNum);
                decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
                decimal d = d1 * d2;

                var round = Math.Round(d);
                var check = SansoBase.Check.IsPointNumberRange(round, 11, 0);
                if (check.Result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    ChangeTopMessage(1, "WARN", "返品数量 × 返品単価の" + check.Msg);
                    return;
                }

                outPriceC1NumericEdit.Value = round;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 伝票番号 検証時
        /// </summary>
        private void doCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = DoCodeErrorCheck();
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
        /// 課別コード 検証後
        /// </summary>
        private void groupCodeC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var c = groupCodeC1ComboBox;
                if (string.IsNullOrEmpty(c.Text))
                {
                    return;
                }

                SetExecuteDate(c.Text);

                ComboBoxValidated(sender, e);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 在庫P検証後
        /// </summary>
        private void stockCateC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                // 在庫情報取得
                SetStockInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 返品数量　検証時
        /// </summary>
        private void outNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                ClearTopMessage();

                if (isRunValidating == false)
                {
                    return;
                }

                var t = (C1TextBox)sender;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                var outNum = t.Text;
                decimal d1 = outNum == "" ? 0 : System.Convert.ToDecimal(outNum);
                if (d1 >= 0)
                {
                    ChangeTopMessage(1, "WARN", "数量がプラスまたはゼロで入力されています。" +
                                                "返品処理の相殺以外はマイナスで入力してください。");
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
        /// 実行（F10）必須チェック
        /// </summary>
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        private bool RequirF10()
        {
            // 必須チェック
            var control = controlListII?
                          .Where(v => v.Required && v.Control.Name != "stockCateC1ComboBox")
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

            var isOk1 = JyuyoyosokuCodeErrorCheck();
            if (isOk1 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
                return false;
            }

            var isOk2 = PartsCodeErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk3 = SupCodeErrorCheck();
            if (isOk3 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            var isOk4 = UnitPriceErrorCheck();
            if (isOk4 == false)
            {
                ActiveControl = unitPriceC1NumericEdit;
                return false;
            }

            var isOk5 = OutDateErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = outDateC1DateEdit;
                return false;
            }

            var isOk6 = DoCodeErrorCheck();
            if (isOk6 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            // 出庫金額の確認
            var outNum = outNumC1NumericEdit.Text;
            var unitPrice = unitPriceC1NumericEdit.Text;
            decimal d1 = outNum == "" ? 0 : System.Convert.ToDecimal(outNum);
            decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
            decimal d = d1 * d2;
            var round = Math.Round(d);
            var outPrice = System.Convert.ToDecimal(outPriceC1NumericEdit.Text);

            if (round != outPrice)
            {
                ChangeTopMessage(1, "WARN", "返品金額が返品数量 × 返品単価と異なります");
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
                
                // 入出庫ファイル、在庫マスタ、素材在庫マスタ更新
                var result = UpdateIOFileAPI(controlListII);
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "返品処理 " + result.Msg);
                        return;
                    }
                }

                DisplayClear();
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0001", "返品処理");

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
        /// 需要予測番号チェック
        /// </summary>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool JyuyoyosokuCodeErrorCheck()
        {
            var t = jyuyoyosokuCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var param = new SansoBase.ManufactCommandFile();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.JyuyoyosokuCode, t.Text.Replace("-", "")));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "製造指令ファイル検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "製造指令ファイル");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 部品コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PartsCodeErrorCheck()
        {
            var t = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var param = new SansoBase.PartsMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, t.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "部品マスタ");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 出庫先チェック
        /// </summary>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool SupCodeErrorCheck()
        {
            var t = supCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                supNameC1TextBox.Text = "";
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 仕入先マスタ
            var paramSup = new SansoBase.SupMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.SupCode, t.Text));
            paramSup.SetDBName("製造調達");
            var afSup = CommonAF.ExecutSelectSQL(paramSup);
            var dtSup = afSup.Table;
            if (dtSup.Rows.Count >= 1)
            {
                var supCate = dtSup.Rows[0].Field<string>("仕入先区分") ?? "";
                if (supCate.Trim() != "S" && supCate.Trim() != "G")
                {
                    ChangeTopMessage("W0016", "外注・一般仕入先以外");
                    return false;
                }

                supNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? "";
                supOsrcCateC1TextBox.Text = supCate;
            }
            else
            {
                supNameC1TextBox.Text = "仕入先マスタ未登録";
                supOsrcCateC1TextBox.Text = "";
            }

            if (supOsrcCateC1TextBox.Text == "G")
            {
                dataCateC1TextBox.Text = "6"; 
                accountCodeC1TextBox.Text = "083";
            }

            if (supOsrcCateC1TextBox.Text == "S")
            {
                dataCateC1TextBox.Text = "1";
                accountCodeC1TextBox.Text = "060";
            }

            return true;
        }

        /// <summary>
        /// 単価チェック
        /// </summary>
        /// <returns>True：エラー無し False：エラーあり</returns>
        private bool UnitPriceErrorCheck()
        {
            var t = unitPriceC1NumericEdit;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            decimal.TryParse(unitPriceC1NumericEdit.Text, out decimal unitPrice);
            if (unitPrice <= 0)
            {
                ChangeTopMessage("W0016", "単価に0以下");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 出庫日付チェック
        /// </summary>
        /// <returns>true：チェックＯＫ、false：チェックＮＧ</returns>
        private bool OutDateErrorCheck()
        {
            var d = outDateC1DateEdit;

            // 空欄の場合
            var isOk = DateTime.TryParse(d.Text, out var date);
            if (isOk == false)
            {
                ChangeTopMessage("W0007", d.Label.Text);
                return false;
            }

            // 処理年月と同じ日付のみ入力可能
            if (date < startDate || date > endDate)
            {
                ChangeTopMessage("W0016", "処理年月と異なる日付");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 伝票番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool DoCodeErrorCheck()
        {
            var t = doCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 在庫情報設定
        /// </summary>
        private void SetStockInfo()
        {
            try
            {
                stockInfoC1TextBox.Text = "";
                prevMonthStockNumC1TextBox.Text = "";
                thisMonthInNumC1TextBox.Text = "";
                thisMonthOutNumC1TextBox.Text = "";
                thisMonthStockNumC1TextBox.Text = "";

                // 部品コード、課別コードのどちらかが入力ない場合は何もしない
                if (string.IsNullOrEmpty(partsCodeC1TextBox.Text) == true
                    || string.IsNullOrEmpty(groupCodeC1ComboBox.Text) == true)
                {
                    return;
                }

                (bool IsOk, DataTable Table, string Sql) result;

                if (stockCateC1ComboBox.Text == "Z")
                {
                    stockInfoC1TextBox.Text = "完成品在庫";

                    // 在庫マスタ
                    var param = new StockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1ComboBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);
                }
                else if(stockCateC1ComboBox.Text.Trim() == "")
                {
                    stockInfoC1TextBox.Text = "素材在庫";

                    // 素材在庫マスタ
                    var param = new MaterialStockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1ComboBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);
                }
                else
                {
                    stockInfoC1TextBox.Text = "";
                    return;
                }

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                    return;
                }

                if (result.Table.Rows.Count >= 1)
                {
                    prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
                    thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
                    thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
                    thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 入出工程単価情報設定
        /// </summary>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool SetGridViewInfo()
        {
            try
            {
                c1TrueDBGrid.SetDataBinding(null, "", true);

                var result = GetGridViewData();
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return false;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", result.Msg);
                        return false;
                    }
                }

                var dt = result.Table;
                if (dt == null || dt.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "部品コード", "工程マスタ");
                    return false;
                }

                c1TrueDBGrid.SetDataBinding(result.Table, "", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }

        /// <summary>
        /// 処理年月設定
        /// </summary>
        private void SetExecuteDate(string groupCode)
        {
            // 処理年月取得
            var result = GetExecuteDate();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", "部門マスタ検索 " + result.Msg);
                    return;
                }
            }

            if (result.Table == null && result.Table.Rows.Count <= 0)
            {
                executeDateValueLabel.Visible = false;
                executeDateLabel.Visible = false;
                ChangeTopMessage("W0001", "部門マスタ");
                return;
            }
            else
            {
                executeDateValueLabel.Visible = true;
                executeDateLabel.Visible = true;

                var date = result.Table.Rows[0].Field<string>("executeDate") ?? "";

                var isOk = DateTime.TryParse(date, out var date1);
                if (isOk == false)
                {
                    ChangeTopMessage("W0002", "処理年月", "部門マスタ");
                    return;
                }

                startDate = date1;
                executeDateValueLabel.Text = startDate.ToString("yyyy/MM");
                endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);
            }
        }

        /// <summary>
        /// 単価マスタ取得
        /// </summary>
        /// <returns>True：エラー無しかつデータ有り False：エラー有りもしくは0件、
        /// 取得データ(falseの場合はnull)
        /// </returns>
        private (bool IsOk, DataTable Table) GetUnitPrice()
        {
            var result = GetUnitPriceMst();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return (false, null);
                }
                else
                {
                    ChangeTopMessage(1, "WARN", "単価マスタ検索 " + result.Msg);
                    return (false, null);
                }
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "単価マスタ");
                return (false, null);
            }

            return (true, result.Table);
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

        #endregion  ＜その他処理 END＞

        #region ＜API処理＞ 

        /// <summary>
        /// 仕入先ファイル、入出庫ファイル、在庫マスタ、素材在庫マスタ更新
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        /// <returns>IsOk：エラーが無し：True　エラーがある：False</returns>
        /// <returns>再ログインしたかどうか：再ログインした場合:true</returns>
        /// <returns>Msg：エラーメッセージ</returns>
        public (bool IsOk, bool ReLogin, string Msg) UpdateIOFileAPI(List<ControlParam> controlList)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += "IOFile/CreateIOFile";

            JObject param = new JObject();

            //項目要見直し
            var dataCate = controlList.SGetText("dataCateC1TextBox");
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var supCode = controlList.SGetText("supCodeC1TextBox");
            var jyuyoyosokuCode = controlList.SGetText("jyuyoyosokuCodeC1TextBox");
            var unitPrice = controlList.SGetText("unitPriceC1NumericEdit").Replace(",", "");
            var acceptDate = controlList.SGetText("outDateC1DateEdit");
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            var accountCode = controlList.SGetText("accountCodeC1TextBox");
            var doCode = controlList.SGetText("doCodeC1TextBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");
            var nSupCate = "I";
            var password = LoginInfo.Instance.UserNo;
            var machineName = LoginInfo.Instance.MachineCode;
            var createDate = DateTime.Now.ToString();
            var createStaffCode = LoginInfo.Instance.UserNo;
            var createID = LoginInfo.Instance.UserId;
            var price = "";

            if (dataCate == "1") // 一般仕入
            {
                int.TryParse(controlList.SGetText("outNumC1NumericEdit").Replace(",", ""), out int inNum);                
                int.TryParse(controlList.SGetText("outPriceC1NumericEdit").Replace(",", ""), out int temp);
                price = (temp * -1).ToString();

                // 入庫の返品時は入庫数を増やす
                param.Add("inNum", new JValue(inNum * -1).ToString());
            }
            else // 6(有償支給)
            {
                int.TryParse(controlList.SGetText("outNumC1NumericEdit").Replace(",", ""), out int outNum);
                price = controlList.SGetText("outPriceC1NumericEdit").Replace(",", "");

                param.Add("outNum", new JValue(outNum));
            }
           
            param.Add("dbName", new JValue("製造調達"));
            param.Add("dataCate", new JValue(dataCate));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCode));
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
            param.Add("createDate", new JValue(createDate));
            param.Add("createStaffCode", new JValue(createStaffCode));
            param.Add("createID", new JValue(createID));
            param.Add("isEOM", new JValue(false));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
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

            return (true, false, (string)result["doCode"]);

        }
        
        /// <summary>
        /// 入出工程単価情報取得
        /// </summary>
        /// <param name="id">ユーザID</param>
        public (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGridViewData()
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F223/GetIOProcessUnitPrice?sid={solutionIdShort}&fid={formIdShort}";

            var partsCode = partsCodeC1TextBox.Text ?? "";

            JObject param = new JObject();
            param.Add("partsCode", new JValue(partsCode));

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
        /// 課別コード　取得
        /// </summary>
        /// <param name="id">ユーザID</param>
        public (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGroupManufactDirect()
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F223/GetGroupComboListByUser?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("id", new JValue(LoginInfo.Instance.UserId.ToUpper() ?? ""));

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
        /// FC前工程在庫Ｐ　取得
        /// </summary>
        public (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetStockCate()
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F223/GetStockCate?sid={solutionIdShort}&fid={formIdShort}";

            var partsCode = partsCodeC1TextBox.Text ?? "";
            var supCode = supCodeC1TextBox.Text ?? "";

            JObject param = new JObject();
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));

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
        /// 処理年月 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetExecuteDate()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F223/GetExecuteDate?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            var groupCode = controlListII.SGetText("groupCodeC1ComboBox");
            param.Add("groupCode", new JValue(groupCode));

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
        /// 単価マスタ 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetUnitPriceMst()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F223/GetUnitPriceMst?sid={solutionIdShort}&fid={formIdShort}";

            var partsCode = partsCodeC1TextBox.Text ?? "";
            var supCode = supCodeC1TextBox.Text ?? "";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));

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


        #endregion  ＜API処理 END＞
    }
}
