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

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 単価マスタメンテナンス
    /// </summary>
    public partial class F601_UintPriceMasterMaint : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// 処理区分の変更前保管エリア
        /// </summary>
        private string stProcessCate = "0";

        /// <summary>
        /// 部品コードの変更前保管エリア
        /// </summary>
        private string stPartsCode = "";

        /// <summary>
        /// 仕入先コードの変更前保管エリア
        /// </summary>
        private string stSupCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F601_UintPriceMasterMaint(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "単価マスタメンテナンス";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F601_UintPriceMasterMaint_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(createStaffNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(updateStaffNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(createDateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(updateDateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(applicationCodeC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(processCateC1ComboBox, null, "", true, enumCate.無し);
                AddControlListII(supUnitPriceC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(materialPriceC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(processPriceC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(lotC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(transUnitPriceC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(prevMaterialPriceC1NumericEdit, null, "0", false, enumCate.無し);
                AddControlListII(prevProcessPriceC1NumericEdit, null, "0", false, enumCate.無し);
                AddControlListII(prevTransUnitPriceC1NumericEdit, null, "0", false, enumCate.無し);
                AddControlListII(prevSupUnitPriceC1NumericEdit, null, "0", false, enumCate.無し);

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

                SetProcessC1ComboBox();

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
        /// 処理区分  コンボボックスセット
        /// </summary>
        private void SetProcessC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("新規｜修正", "0");
            dt.Rows.Add("削除", "1");
            ControlAF.SetC1ComboBox(processCateC1ComboBox, dt, 0, 150, "NAME", "NAME", true);
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

            // キー以外の入力項目編集を不可にする
            EditEnable(false);

            processCateC1ComboBox.SelectedIndex = 0;
            stProcessCate = "0";
            stSupCode = "";
            stPartsCode = "";

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = processCateC1ComboBox;
        }

        /// <summary>
        /// クリア処理  ボディ
        /// </summary>
        private void DisplayClearBody()
        {
            // パネルの一括クリア処理
            foreach (Control c in panel2.Controls)
            {
                var type = c.GetType();
                if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).Text = "";
                }
                else if (type == typeof(C1NumericEdit))
                {
                    ((C1NumericEdit)c).Value = 0;
                }
                else
                {
                    // 処理なし
                }
            }

            // キー以外の入力項目編集を不可にする
            EditEnable(false);
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
                        partsCodeC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString();
                        partsNameC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                        stPartsCode = form.row.Cells["部品コード"].Value.ToString();
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
        /// 処理区分 検証後
        /// </summary>
        private void processCateC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var s = processCateC1ComboBox.SGetText(1);
                if (isRunValidating == false
                    || stProcessCate == s)
                {
                    return;
                }

                stProcessCate = s;

                DisplayClearBody();
                SetUnitPriceColumn();

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
                var obj = (C1TextBox)sender;

                if (isRunValidating == false
                    || string.IsNullOrEmpty(obj.Text)
                    || stPartsCode == obj.Text)
                {
                    return;
                }

                var result = PartsCodeCheck(obj);
                if (result.IsOk == false)
                {
                    ActiveControl = obj;
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
                var obj = (C1TextBox)sender;

                if (isRunValidating == false
                    || stPartsCode == obj.Text)
                {
                    return;
                }

                if (string.IsNullOrEmpty(obj.Text))
                {
                    partsNameC1TextBox.Text = "";
                    stPartsCode = "";
                    return;
                }

                var result = PartsCodeCheck(obj);
                if (result.IsOk == false)
                {
                    ActiveControl = obj;
                    return;
                }

                if (result.Table.Rows.Count >= 1)
                {
                    partsNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名");
                }

                SetUnitPriceColumn();

                stPartsCode = obj.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 仕入先コード 検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var obj = (C1TextBox)sender;

                if (isRunValidating == false
                    || string.IsNullOrEmpty(obj.Text)
                    || stSupCode == obj.Text)
                {
                    return;
                }

                var result = SupCodeCK(obj);
                if (result.IsOk == false)
                {
                    ActiveControl = obj;
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
        /// 仕入先コード 検証後
        /// </summary>
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var obj = (C1TextBox)sender;

                if (isRunValidating == false
                    || stSupCode == obj.Text)
                {
                    return;
                }

                supNameC1TextBox.Text = "";
                stSupCode = "";

                if (string.IsNullOrEmpty(obj.Text))
                {
                    return;
                }

                var result = SupCodeCK(obj);
                if (result.IsOk == false)
                {
                    ActiveControl = obj;
                    return;
                }

                if (result.Table.Rows.Count >= 1)
                {
                    supNameC1TextBox.Text = result.Table.Rows[0].Field<string>("仕入先名１");
                }
                
                SetUnitPriceColumn();

                stSupCode = obj.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 材料費 検証後
        /// </summary>
        private void materialPriceC1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(materialPriceC1NumericEdit.Text)
                    || string.IsNullOrEmpty(processPriceC1NumericEdit.Text))
                {
                    return;
                }

                var result = CalculateSupUnitPrice();
                if (result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 加工費検証後
        /// </summary>
        private void processPriceC1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(materialPriceC1NumericEdit.Text)
                    || string.IsNullOrEmpty(processPriceC1NumericEdit.Text))
                {
                    return;
                }

                var result = CalculateSupUnitPrice();
                if (result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 見積ロット数 検証時
        /// </summary>
        private void lotC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var obj = (C1NumericEdit)sender;
                var chk = LotCK(obj);
                if (chk == false)
                {
                    ActiveControl = obj;
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
        /// 申請書番号 検証時
        /// </summary>
        private void applicationCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var obj = (C1TextBox)sender;
                var chk = ApplicationCodeCK(obj);
                if (chk == false)
                {
                    ActiveControl = obj;
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
            var t0 = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(t0.Text) == false)
            {
                var result0 = PartsCodeCheck(t0);
                if (result0.IsOk == false)
                {
                    ActiveControl = t0;
                    return false;
                }
            }

            var t1 = supCodeC1TextBox;
            if (string.IsNullOrEmpty(t1.Text) == false)
            {
                var result1 = SupCodeCK(t1);
                if (result1.IsOk == false)
                {
                    ActiveControl = t1;
                    return false;
                }
            }

            var s = processCateC1ComboBox.SGetText(1);

            // 新規修正時
            if (s == "0")
            {
                var t2 = materialPriceC1NumericEdit;
                if (string.IsNullOrEmpty(t2.Text) == false)
                {
                    var result2 = PriceCK(t2);
                    if (result2 == false)
                    {
                        ActiveControl = t2;
                        return false;
                    }
                }

                var t3 = processPriceC1NumericEdit;
                if (string.IsNullOrEmpty(t3.Text) == false)
                {
                    var result3 = PriceCK(t3);
                    if (result3 == false)
                    {
                        ActiveControl = t3;
                        return false;
                    }
                }

                var t8 = transUnitPriceC1NumericEdit;
                if (string.IsNullOrEmpty(t8.Text) == false)
                {
                    var result8 = PriceCK(t8);
                    if (result8 == false)
                    {
                        ActiveControl = t8;
                        return false;
                    }
                }

                var result4 = CalculateSupUnitPrice();
                if (result4 == false)
                {
                    ActiveControl = t2;
                    return false;
                }

                var t5 = supUnitPriceC1NumericEdit;
                if (string.IsNullOrEmpty(t5.Text) == false)
                {
                    var result5 = PriceCK(t5);
                    if (result5 == false)
                    {
                        ActiveControl = t5;
                        return false;
                    }
                }

                var t6 = lotC1NumericEdit;
                if (string.IsNullOrEmpty(t6.Text) == false)
                {
                    var result6 = LotCK(t6);
                    if (result6 == false)
                    {
                        ActiveControl = t6;
                        return false;
                    }
                }

                var t7 = applicationCodeC1TextBox;
                if (string.IsNullOrEmpty(t7.Text) == false)
                {
                    var result7 = ApplicationCodeCK(t7);
                    if (result7 == false)
                    {
                        ActiveControl = t7;
                        return false;
                    }
                }

            }
            // 削除時
            else
            {

                var partsCode = partsCodeC1TextBox.Text;
                var supCode = supCodeC1TextBox.Text;

                var af = new UnitPriceMstAF();
                (bool isOk, DataTable dt) = af.GetPartsUnitPriceMstBySup(partsCode, supCode);
                if (isOk == false)
                {
                    ChangeTopMessage("E0008", "単価マスタの検索時に");
                    return false;
                }

                if (dt.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "単価マスタ");
                    return false;
                }
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


                var af = new F601_UintPriceMasterMaintAF();

                // 新規修正
                if (processCateC1ComboBox.SGetText(1) == "0")
                {
                    var result = af.UpdateUnitPriceMST(controlListII);
                    if (result == false)
                    {
                        ChangeTopMessage("E0008", "単価マスタ登録修正時に");
                        return;
                    }

                    DisplayClear();
                    ChangeTopMessage("I0002", "単価マスタ");

                }
                // 削除
                else
                {
                    
                    DialogResult d = MessageBox.Show(
                        "部品コード「" + partsCodeC1TextBox.Text + "」、仕入先コード「" + supCodeC1TextBox.Text + "」の" +
                        "単価マスタを削除してよろしいですか？",
                        "削除確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2);
                    if (d == DialogResult.No)
                    {
                        return;
                    }

                    var result = af.DeleteUnitPriceMST(controlListII);
                    if (result == false)
                    {
                        ChangeTopMessage("E0008", "単価マスタ削除時に");
                        return;
                    }

                    DisplayClear();
                    ChangeTopMessage("I0003", "単価マスタ");

                }

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
        /// 単価、金額 検証時
        /// </summary>
        private void PriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var obj = (C1NumericEdit)sender;
                var chk = PriceCK(obj);
                if (chk == false)
                {
                    ActiveControl = obj;
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
        /// 部品コードチェック・抽出
        /// </summary>
        /// <param name="obj">チェック対象のプロパティ</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private (bool IsOk, DataTable Table) PartsCodeCheck(C1TextBox obj)
        {
            if (Check.HasBanChar(obj.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return (false, null);
            }

            var af = new PartsMstAF();
            var result = af.GetPartsMST(obj.Text);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return (false, null);
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "部品コード", "部品マスタ");
                return (false, null);
            }

            return result;
        }

        /// <summary>
        /// 仕入先コードチェック・抽出
        /// </summary>
        /// <param name="obj">チェック対象コントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private (bool IsOk, DataTable Table) SupCodeCK(C1TextBox obj)
        {
            if (Check.HasBanChar(obj.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return (false, null);
            }

            var af = new SupMstAF();
            var result = af.GetInsideSupMST(obj.Text);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return (false, null);
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "仕入先コード", "仕入先マスタ");
                return (false, null);
            }

            return result;
        }

        /// <summary>
        /// エラーチェック 単価、金額
        /// </summary>
        /// <param name="obj">チェック対象コントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool PriceCK(C1NumericEdit obj)
        {
            if (string.IsNullOrEmpty(obj.Text))
            {
                return true;
            }

            var chk1 = Check.IsNumeric(obj.Text).Result;
            if (chk1 == false)
            {
                ChangeTopMessage("W0019", obj.Label.Text + "には");
                return false;
            }

            var dec = decimal.Parse(obj.Text);

            var chk2 = Check.IsPointNumberRange(dec, 9, 2).Result;
            if (chk2 == false)
            {
                ChangeTopMessage("W0004", obj.Label.Text, "整数9桁以下、小数2");
                return false;
            }

            var chk3 = Check.IsNumberRange(dec, "0", null).Result;
            if (chk3 == false)
            {
                ChangeTopMessage("W0019", obj.Label.Text + "は、0以上の");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック ロット数
        /// </summary>
        /// <param name="obj">チェック対象コントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool LotCK(C1NumericEdit obj)
        {
            if (string.IsNullOrEmpty(obj.Text))
            {
                return true;
            }

            var chk1 = Check.IsNumeric(obj.Text).Result;
            if (chk1 == false)
            {
                ChangeTopMessage("W0019", obj.Label.Text + "には");
                return false;
            }

            var dec = decimal.Parse(obj.Text);

            var chk2 = Check.IsPointNumberRange(dec, 7, 2).Result;
            if (chk2 == false)
            {
                ChangeTopMessage("W0004", obj.Label.Text, "整数7桁以下、小数2");
                return false;
            }

            var chk3 = Check.IsNumberRange(dec, "0", null).Result;
            if (chk3 == false)
            {
                ChangeTopMessage("W0019", obj.Label.Text + "は、0以上の");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック 申請書番号
        /// </summary>
        /// <param name="obj">チェック対象コントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool ApplicationCodeCK(C1TextBox obj)
        {
            if (string.IsNullOrEmpty(obj.Text))
            {
                return true;
            }

            var chk1 = Check.HasBanChar(obj.Text).Result;
            if (chk1 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var chk2 = Check.IsStringRange(obj.Text, 8, 12).Result;
            if (chk2 == false)
            {
                ChangeTopMessage("W0003", obj.Label.Text, "8", "12");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 仕入単価計算
        /// </summary>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool CalculateSupUnitPrice()
        {
            supUnitPriceC1NumericEdit.Value = 0;
            var materialPrice = decimal.Parse(materialPriceC1NumericEdit.Text);
            var processPrice = decimal.Parse(processPriceC1NumericEdit.Text);
            var transUnitPrice = decimal.Parse(transUnitPriceC1NumericEdit.Text);
            var supPrice = materialPrice + processPrice + transUnitPrice;

            var chk = Check.IsPointNumberRange(supPrice, 9, 2).Result;
            if (chk == false)
            {
                ChangeTopMessage("W0004", "仕入単価", "整数9桁以下、小数2");
                return false;
            }

            supUnitPriceC1NumericEdit.Value = supPrice.ToString();
            return true;
        }

        /// <summary>
        /// キー以外の入力項目の編集可否設定
        /// </summary>
        /// <param name="enable">True:入力可能にする False:入力不可にする</param>
        private void EditEnable(bool edit)
        {
            // コントロールEnable用フラグ
            bool readOnly = false;
            bool enable = true;
            Color backColor = SColDef;

            if (edit == false)
            {
                readOnly = true;
                enable = false;
                backColor = SColReadOnly;
            }

            // キー以外の入力項目の設定
            foreach (var v in controlListII.Where(v => v.Control.Name == "materialPriceC1NumericEdit" ||
                                            v.Control.Name == "processPriceC1NumericEdit" ||
                                            v.Control.Name == "transUnitPriceC1NumericEdit" ||
                                            v.Control.Name == "lotC1NumericEdit" ||
                                            v.Control.Name == "applicationCodeC1TextBox"))
            {
                var c = v.Control;
                var type = c.GetType();
                if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).ReadOnly = readOnly;
                    c.Enabled = enable;
                    if (((C1TextBox)c).Label != null)
                    {
                        ((C1TextBox)c).Label.Enabled = true;
                    }
                    c.BackColor = backColor;
                }
                else if (type == typeof(C1NumericEdit))
                {
                    ((C1NumericEdit)c).ReadOnly = readOnly;
                    c.Enabled = enable;
                    if (((C1NumericEdit)c).Label != null)
                    {
                        ((C1NumericEdit)c).Label.Enabled = true;
                    }
                    c.BackColor = backColor;
                }
                else
                {
                    // 処理なし
                }
            }
        }

        /// <summary>
        /// 単価マスタ情報設定
        /// </summary>
        private void SetUnitPriceColumn()
        {
            ClearTopMessage();

            var s = processCateC1ComboBox.SGetText(1);
            var supCode = supCodeC1TextBox.Text;
            var partsCode = partsCodeC1TextBox.Text;

            // 処理区分に変更がない、かつ仕入先コード、部品コードのどちらかが未入力
            if (stProcessCate == s && (partsCode == "" || supCode == ""))
            {
                return;
            }

            // 処理区分に変更がない、かつ部品コ―ド、仕入先コードに変更がない場合は何もしない
            if (stProcessCate == s && partsCode == stPartsCode && stSupCode == supCode)
            {
                return;
            }

            var af = new UnitPriceMstAF();

            var result = af.GetPartsUnitPriceMstBySup(partsCode, supCode);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタの検索時に");
                return;
            }
            var dt = result.Table;

            // 削除の場合は単価マスタの存在チェック          
            if ((dt.Rows.Count <= 0) && (s == "1"))
            {
                ChangeTopMessage("W0001", "単価マスタ");
                return;
            }

            // 新規の場合は画面クリア
            if (dt.Rows.Count <= 0)
            {
                DisplayClearBody();

                // キー以外の入力項目編集を可能にする
                EditEnable(true);
                ActiveControl = materialPriceC1NumericEdit;

                return;
            }

            // 修正か削除の場合は単価マスタの値を表示する
            // DataTableの内容をコントロールにセット
            var list = controlListII
                        .Where(v => (((C1TextBox)v.Control).Label != null) &&
                        (v.Control.Name != "partsStopC1ComboBox") && (v.Control.Name != "delivPlaceC1ComboBox"))
                        .ToList();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var ml = list.Where(v => ((C1TextBox)v.Control).Label.Text == dt.Columns[i].ColumnName).ToList();
                // 列名とラベル名が違うものを除外
                if (ml.Count != 0)
                {
                    if (ml[0].Control.GetType() == typeof(C1.Win.Calendar.C1DateEdit))
                    {
                        ((C1.Win.Calendar.C1DateEdit)ml[0].Control).Value =
                            dt.Rows[0].Field<DateTime?>(((C1.Win.Calendar.C1DateEdit)ml[0].Control).Label.Text) ?? null;
                    }
                    else if (ml[0].Control.GetType() == typeof(C1NumericEdit))
                    {
                        ((C1NumericEdit)ml[0].Control).Value =
                            dt.Rows[0].Field<decimal?>(((C1NumericEdit)ml[0].Control).Label.Text) ?? null;
                    }
                    else if ((ml[0].Control.GetType() == typeof(C1TextBox)) && ((ml[0].Control).Name.Contains("DateEdit")))
                    {
                        ((C1TextBox)ml[0].Control).Text =
                            (dt.Rows[0].Field<DateTime?>(((C1TextBox)ml[0].Control).Label.Text) ?? null).ToString();
                    }
                    else if ((ml[0].Control.GetType() == typeof(C1TextBox)) && ((ml[0].Control).Name.Contains("NumericEdit")))
                    {
                        ((C1TextBox)ml[0].Control).Text = (
                            dt.Rows[0].Field<decimal?>(((C1TextBox)ml[0].Control).Label.Text) ?? null).ToString();
                    }
                    else
                    {
                        ml[0].Control.Text = dt.Rows[0].Field<string>(((C1TextBox)ml[0].Control).Label.Text) ?? string.Empty;
                    }
                }
            }

            // 修正の場合
            if (s == "0")
            {
                // キー以外の入力項目編集を可能にする
                EditEnable(true);
                ActiveControl = materialPriceC1NumericEdit;
            }

            return;
        }

        #endregion  ＜その他処理 END＞

    }
}
