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
    /// 内部受付（部品）
    /// </summary>
    public partial class F207_InsideAcceptanceParts : BaseForm
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
        /// 課別コード保管（ログインユーザから取得）
        /// </summary>
        private DataTable groupCodeDt = new DataTable();

        /// <summary>
        /// 変更前 部品コード
        /// </summary>
        private string stPartsCode = "";

        /// <summary>
        /// 変更前 仕入先コード
        /// </summary>
        private string stSupCode = "";



        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F207_InsideAcceptanceParts(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "内部受付（部品）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F207_InsideAcceptanceParts_Load(object sender, EventArgs e)
        {
            try
            {
                // 左パネル
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.Key);

                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(inNumC1NumericEdit, null, "", true, enumCate.Key);
                AddControlListII(unitPriceC1NumericEdit, null, "", true, enumCate.Key);
                AddControlListII(unitPriceCateC1ComboBox, unitPriceNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(unitPriceNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(processUnitPriceC1NumericEdit, null, "", false, enumCate.Key);
                AddControlListII(inPriceC1NumericEdit, null, "", true, enumCate.Key);
                AddControlListII(inDateC1DateEdit, null, null, true, enumCate.Key);
                AddControlListII(doCodeC1TextBox, null, "", true, enumCate.Key);
                AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", true, enumCate.Key);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.Key);

                // 右パネル
                AddControlListII(partsCateC1TextBox, partsCateNameC1TextBox, "", false, enumCate.Key);
                AddControlListII(partsCateNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(entryDateC1TextBox, null, null, false, enumCate.Key);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", false, enumCate.Key);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(transCateC1ComboBox, transCateC1TextBox, "", false, enumCate.Key);
                AddControlListII(transCateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(supOsrcCateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(ProcessMstC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(unitPriceMstC1TextBox, null, "", false, enumCate.Key);

                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthInNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthOutNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthStockNumC1TextBox, null, "", false, enumCate.Key);

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
                SetStockCateC1ComboBox();
                SetTransCateC1ComboBox();
                SetUnitPriceCateC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後に実行（F10）を押してください。";

                // 課別コード、処理年月取得
                GetGroupCode();

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
        /// 単価区分　コンボボックスセット
        /// </summary>
        private void SetUnitPriceCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("1", "決");
            dt.Rows.Add("9", "未");
            dt.Rows.Add("K", "仮");

            ControlAF.SetC1ComboBox(unitPriceCateC1ComboBox, dt, unitPriceCateC1ComboBox.Width,
                unitPriceCateC1ComboBox.Width, "ID", "NAME", false);
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
                    //((C1.Win.Calendar.C1DateEdit)c).Value = v.Initial;
                    ((C1.Win.Calendar.C1DateEdit)c).Text = v.Initial;
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

            // 初期処理
            stPartsCode = "";
            stSupCode = "";
            unitPriceCateC1ComboBox.SelectedIndex = -1;
            stockCateC1ComboBox.SelectedIndex = -1;
            transCateC1ComboBox.SelectedIndex = -1;
            partsCateNameC1TextBox.ForeColor = Color.Red;
            partsCateNameC1TextBox.ForeColor = Color.PeachPuff;

            var groupCode = groupCodeDt.Rows[0].Field<string>("groupCode") ?? "";
            var groupName = groupCodeDt.Rows[0].Field<string>("groupName") ?? "";
            groupCodeC1TextBox.Text = groupCode;
            groupNameC1TextBox.Text = groupName;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

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
                if (isRunValidating == false)
                {
                    return;
                }

                var t = (C1TextBox)sender;
                if (string.IsNullOrEmpty(t.Text))
                {
                    partsNameC1TextBox.Text = "";
                    partsCateC1TextBox.Text = "";
                    partsCateNameC1TextBox.Text = "";
                    dataCateC1TextBox.Text = "";
                    accountCodeC1TextBox.Text = "";
                    supOsrcCateC1TextBox.Text = "";
                    ProcessMstC1TextBox.Text = "";
                    unitPriceMstC1TextBox.Text = "";
                    return;
                }

                var paramParts = new SansoBase.PartsMst();
                paramParts.SelectStr = "*";
                paramParts.WhereColuList.Add((paramParts.PartsCode, t.Text));
                paramParts.SetDBName("製造調達");
                var resultParts = CommonAF.ExecutSelectSQL(paramParts);
                if (resultParts.IsOk == false)
                {
                    ActiveControl = t;
                    ChangeTopMessage("E0008", "部品マスタ検索時に");                    
                    return;
                }
                if (resultParts.Table.Rows.Count <= 0)
                {
                    ActiveControl = t;
                    ChangeTopMessage("W0002", t.Label.Text, "部品マスタ");                    
                    return;
                }

                partsNameC1TextBox.Text = resultParts.Table.Rows[0].Field<string>("部品名") ?? "";
                var partsCate = resultParts.Table.Rows[0].Field<string>("部品区分") ?? "";
                partsCateC1TextBox.Text = partsCate;

                var isOk = PartsCateBranch();
                if (isOk == false)
                {
                    ActiveControl = t;
                    return;
                }

                inDateC1DateEdit.Value = DateTime.Today.ToShortDateString();
                entryDateC1TextBox.Text = DateTime.Now.ToShortDateString();

                stPartsCode = t.Text;

                // 仕入先入力後に部品変更した場合
                if (string.IsNullOrEmpty(supCodeC1TextBox.Text) == false)
                {
                    // 単価情報取得
                    var isOk2 = GetUnitPrice();
                    if (isOk2 == false)
                    {
                        ActiveControl = t;
                        return;
                    }
                }

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
                if (isRunValidating == false)
                {
                    return;
                }

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
        /// 仕入先コード 検証後
        /// </summary> 
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = supCodeC1TextBox;

                if (isRunValidating == false
                    || stSupCode == t.Text)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    supNameC1TextBox.Text = "";
                }

                // 単価情報取得
                var isOk = GetUnitPrice();
                if (isOk == false)
                {
                    ActiveControl = t;
                    return;
                }

                // 在庫情報設定
                SetStockInfo();

                stSupCode = supCodeC1TextBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 納入数量 検証時
        /// </summary>
        private void inNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = InNumErrorCheck();
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
        /// 納入単価 検証時
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
        /// 納入数量、納入単価 検証後
        /// </summary>
        private void numPriceC1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                // 納入金額計算
                var inNum = inNumC1NumericEdit.Text;
                var unitPrice = unitPriceC1NumericEdit.Text;
                decimal d1 = inNum == "" ? 0 : System.Convert.ToDecimal(inNum);
                decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
                decimal d = d1 * d2;

                var round = Math.Round(d);
                var check = SansoBase.Check.IsPointNumberRange(round, 11, 0);
                if (check.Result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    ChangeTopMessage(1, "WARN", "出庫数量 × 出庫単価の" + check.Msg);
                    return;
                }

                inPriceC1NumericEdit.Value = round;

                return;
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
        /// 在庫P 検証後
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
            var isOk1 = PartsCodeErrorCheck();
            if (isOk1 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk2 = JyuyoyosokuCodeErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
                return false;
            }

            var isOk3 = SupCodeErrorCheck();
            if (isOk3 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            var isOk4 = InNumErrorCheck();
            if (isOk4 == false)
            {
                ActiveControl = inNumC1NumericEdit;
                return false;
            }

            var isOk5 = UnitPriceErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = unitPriceC1NumericEdit;
                return false;
            }

            var isOk6 = InDateErrorCheck();
            if (isOk6 == false)
            {
                ActiveControl = inDateC1DateEdit;
                return false;
            }

            var isOk7 = DoCodeErrorCheck();
            if (isOk7 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            // 納入金額の確認
            var inNum = inNumC1NumericEdit.Text;
            var unitPrice = unitPriceC1NumericEdit.Text;
            decimal d1 = inNum == "" ? 0 : System.Convert.ToDecimal(inNum);
            decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
            decimal d = d1 * d2;
            var round = Math.Round(d);
            var inPrice = System.Convert.ToDecimal(inPriceC1NumericEdit.Text);

            var check = SansoBase.Check.IsPointNumberRange(round, 11, 0);
            if (check.Result == false)
            {
                ChangeTopMessage(1, "WARN", "出庫数量 × 出庫単価の" + check.Msg);
                return false;
            }

            if (round != inPrice)
            {
                ChangeTopMessage(1, "WARN", "納入金額が納入数量 × 納入単価と異なります");
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
                var result = UpdateIOFileAPI();
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

                DisplayClear();
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0009", "内部受付処理が");
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
        /// 課別コード、処理年月取得
        /// </summary>
        /// <returns></returns>
        private void GetGroupCode()
        {
            // 課別コード取得
            var result1 = GetGroupComboListByUser();
            if (result1.IsOk == false)
            {
                if (result1.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", result1.Msg);
                    return;
                }
            }

            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "ユーザ", "マスタ");
                return;
            }

            groupCodeDt = result1.Table;
            var groupCode = groupCodeDt.Rows[0].Field<string>("groupCode") ?? "";
            var groupName = groupCodeDt.Rows[0].Field<string>("groupName") ?? "";
            if (groupCode == "3623")
            {
                // 在庫P = Z にする
                stockCateC1ComboBox.SelectedIndex = 1;
            }

            // 処理年月取得
            var af2 = new GroupMstAF();
            var result = af2.GetExecuteDate(groupCode);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部門マスタ検索時に");
                return;
            }

            if (result.Table.Rows.Count <= 0)
            {
                executeDateValueLabel.Visible = false;
                executeDateLabel.Visible = false;
                ChangeTopMessage("W0001", "部門マスタ");
                return;
            }

            executeDateValueLabel.Text = result.Table.Rows[0].Field<string>("処理年月") ?? "";
            executeDateValueLabel.Visible = true;
            executeDateLabel.Visible = true;

            var isOk = DateTime.TryParse(executeDateValueLabel.Text, out var date);
            if (isOk == false)
            {
                ChangeTopMessage("W0002", "処理年月", "部門マスタ");
                return;
            }

            startDate = date;
            endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);

        }

        /// <summary>
        /// 部品コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PartsCodeErrorCheck()
        {
            var t = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text) || t.Text == stPartsCode)
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
        /// 需要予測番号コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
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
            param.WhereColuList.Add((param.JyuyoyosokuCode, t.Text));
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
        /// 仕入先コード エラーチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SupCodeErrorCheck()
        {
            var t = supCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text) || t.Text == stSupCode)
            {
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var param = new SansoBase.SupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.SupCode, t.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "製造指令ファイル検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "仕入先マスタ");
                return false;
            }

            var supCate = result.Table.Rows[0].Field<string>("仕入先区分") ?? "";

            if (supCodeC1TextBox.Text != "3731" && supCate != "K")
            {
                ChangeTopMessage("W0016", "社内以外");
                return false;
            }

            supNameC1TextBox.Text = result.Table.Rows[0].Field<string>("仕入先名１") ?? "";
            supOsrcCateC1TextBox.Text = result.Table.Rows[0].Field<string>("仕入先区分") ?? "";

            // 住所録マスタ取得
            var resultAddress = SelectDBAF.GetSansoMainAddressMst(supCodeC1TextBox.Text);
            if (resultAddress.IsOk == false)
            {
                ChangeTopMessage("E0008", "住所録マスタ検索時に");
                return　false;
            }

            // 仕入外注区分 判定
            switch (supOsrcCateC1TextBox.Text)
            {
                case "G": // 外注仕入先
                    dataCateC1TextBox.Text = "3";
                    accountCodeC1TextBox.Text = "083";
                    break;


                case "K": // 社内部門
                    dataCateC1TextBox.Text = "2";
                    accountCodeC1TextBox.Text = "";
                    break;

                default: // 一般仕入先
                    dataCateC1TextBox.Text = "1";

                    // 住所録マスタ
                    if (resultAddress.Table.Rows.Count >= 1)
                    {
                        var temp = resultAddress.Table.Rows[0].Field<string>("仕外区分詳細") ?? "";
                        accountCodeC1TextBox.Text = temp == "N" ? "062" : "060";
                    }
                    break;
            }

            var isOk2 = PartsCateBranch();
            if (isOk2 == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 納入数量チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool InNumErrorCheck()
        {
            var n = inNumC1NumericEdit;

            if (string.IsNullOrEmpty(n.Text))
            {
                return true;
            }

            decimal num = decimal.Parse(n.Value.ToString());
            var result = Check.IsPointNumberRange(num, 7, 0);
            if (result.Result == false)
            {
                ChangeTopMessage("W0006", "納入数量");
                return false;
            }

            if (num == 0)
            {
                ChangeTopMessage("W0016", "納入数量に0");
                return false;
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

            decimal.TryParse(t.Text, out decimal unitPrice);

            if (partsCateC1TextBox.Text == "M01" || partsCateC1TextBox.Text == "M04")
            {
                if (unitPrice != 0)
                {
                    ChangeTopMessage("W0016", "無償支給の時、0以外");
                    return false;
                }
            }
            else
            {
                if (unitPrice == 0)
                {
                    ChangeTopMessage("W0007", t.Label.Text);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 納入日付チェック
        /// </summary>
        /// <returns>true：チェックＯＫ、false：チェックＮＧ</returns>
        private bool InDateErrorCheck()
        {
            var d = inDateC1DateEdit;

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

            if (date >= DateTime.Today)
            {
                ChangeTopMessage("W0016", "当日以降の日付");
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

            var isOk1 = Check.HasBanChar(t.Text).Result;
            if (isOk1 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var isOk2 = Check.IsStringRange(t.Text, 4, 4);
            if (isOk2.Result == false)
            {
                ChangeTopMessage("W0009", t.Label.Text, "4", "4");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 単価情報取得
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool GetUnitPrice()
        {
            var partsCode = partsCodeC1TextBox.Text;
            var supCode = supCodeC1TextBox.Text;            

            if (string.IsNullOrEmpty(partsCode) || string.IsNullOrEmpty(supCode))
            {
                supNameC1TextBox.Text = "";
                stockCateC1ComboBox.SelectedIndex = 0;
                transCateC1ComboBox.SelectedIndex = 0;
                unitPriceC1NumericEdit.Value = 0;
                processUnitPriceC1NumericEdit.Value = 0;
                ProcessMstC1TextBox.Text = "";
                unitPriceMstC1TextBox.Text = "";
            }

            if ( partsCode == stPartsCode && supCode == stSupCode)
            {
                return true;
            }

            if (partsCode != "M01" || partsCode != "M04")
            {
                // 工程マスタ参照
                var paramProcess = new SansoBase.ProcessMst();
                paramProcess.SelectStr = "*";
                paramProcess.WhereColuList.Add((paramProcess.PartsCode, partsCode));
                paramProcess.WhereColuList.Add((paramProcess.SupCode, supCode));
                paramProcess.SetDBName("製造調達");
                var resultProcess = CommonAF.ExecutSelectSQL(paramProcess);
                if (resultProcess.IsOk == false)
                {
                    ChangeTopMessage("E0008", "工程マスタ検索時に");
                    return false;
                }

                var dtProcess = resultProcess.Table;
                if (dtProcess.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "部品コード、仕入先コード", "工程マスタ");
                    ProcessMstC1TextBox.Text = "無し";
                    stockCateC1ComboBox.SelectedIndex = 0;
                    transCateC1ComboBox.SelectedIndex = 0;
                }
                else
                {
                    ProcessMstC1TextBox.Text = "有り";
                    stockCateC1ComboBox.Text = dtProcess.Rows[0].Field<string>("在庫Ｐ") ?? "";
                    stockCateC1ComboBox_Validated(stockCateC1ComboBox, new EventArgs());
                    transCateC1ComboBox.Text = dtProcess.Rows[0].Field<string>("有償Ｐ") ?? "";
                    ComboBoxValidated(transCateC1ComboBox, new EventArgs());
                }
            }

            // 単価マスタ参照
            var paramUnitPrice = new SansoBase.UnitPriceMst();
            paramUnitPrice.SelectStr = "*";
            paramUnitPrice.WhereColuList.Add((paramUnitPrice.PartsCode, partsCode));
            paramUnitPrice.WhereColuList.Add((paramUnitPrice.SupCode, supCode));
            paramUnitPrice.SetDBName("製造調達");
            var resultUnitPrice = CommonAF.ExecutSelectSQL(paramUnitPrice);
            if (resultUnitPrice.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return false;
            }

            var dtUnitPrice = resultUnitPrice.Table;
            if (dtUnitPrice.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "部品コード、仕入先コード", "単価マスタ");
                unitPriceMstC1TextBox.Text = "無し";
            }
            else
            {
                unitPriceMstC1TextBox.Text = "有り";
                unitPriceCateC1ComboBox.Text = dtUnitPrice.Rows[0].Field<string>("単価区分") ?? "";
                ComboBoxValidated(unitPriceCateC1ComboBox, new EventArgs());
                unitPriceC1NumericEdit.Value = dtUnitPrice.Rows[0].Field<decimal?>("仕入単価") ?? 0;
                processUnitPriceC1NumericEdit.Value = dtUnitPrice.Rows[0].Field<decimal?>("加工費") ?? 0;
            }

            var isOk = PartsCateBranch();
            if (isOk == false)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 部品区分判定
        /// </summary>
        /// <returns>True：エラー無し False：エラーあり</returns>
        private bool PartsCateBranch()
        {
            switch (partsCateC1TextBox.Text)
            {
                case "M01":
                    partsCateNameC1TextBox.Text = "無償支給";
                    dataCateC1TextBox.Text = "5";
                    unitPriceC1NumericEdit.Value = 0;
                    processUnitPriceC1NumericEdit.Value = 0;
                    unitPriceCateC1ComboBox.SelectedIndex = 0;
                    break;

                case "M02":
                    partsCateNameC1TextBox.Text = "有償支給";
                    dataCateC1TextBox.Text = "1";
                    accountCodeC1TextBox.Text = "060";
                    break;

                case "M04":
                    partsCateNameC1TextBox.Text = "無償支給(加工有)";
                    dataCateC1TextBox.Text = "5";
                    unitPriceC1NumericEdit.Value = 0;
                    processUnitPriceC1NumericEdit.Value = 0;
                    unitPriceCateC1ComboBox.SelectedIndex = 0;
                    break;

                default:
                    var paramCate = new SansoBase.PartsCateMst();
                    paramCate.SelectStr = "*";
                    paramCate.WhereColuList.Add((paramCate.PartsCate, partsCateC1TextBox.Text));
                    paramCate.SetDBName("製造調達");
                    var resultCate = CommonAF.ExecutSelectSQL(paramCate);
                    if (resultCate.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "部品区分マスタ検索時に");
                        return false;
                    }

                    if (resultCate.Table.Rows.Count <= 0)
                    {
                        partsCateNameC1TextBox.Text = "";
                    }
                    else
                    {
                        partsCateNameC1TextBox.Text = resultCate.Table.Rows[0].Field<string>("部品区分名") ?? "";
                    }
                    break;
            }

            return true;

        }


        /// <summary>
        /// 在庫情報設定
        /// </summary>
        /// <returns>True：エラー無し False：エラーあり</returns>
        private void SetStockInfo()
        {
            stockInfoC1TextBox.Text = "";
            prevMonthStockNumC1TextBox.Text = "";
            thisMonthInNumC1TextBox.Text = "";
            thisMonthOutNumC1TextBox.Text = "";
            thisMonthStockNumC1TextBox.Text = "";

            // 部品コード、課別コードのどちらかが入力ない場合は何もしない
            if (string.IsNullOrEmpty(partsCodeC1TextBox.Text) == true
                || string.IsNullOrEmpty(groupCodeC1TextBox.Text) == true)
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
                param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                param.SetDBName("製造調達");
                result = CommonAF.ExecutSelectSQL(param);
            }
            else
            {
                stockInfoC1TextBox.Text = "素材在庫";

                // 素材在庫マスタ
                var param = new MaterialStockMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                param.SetDBName("製造調達");
                result = CommonAF.ExecutSelectSQL(param);
            }

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                return;
            }

            if (result.Table.Rows.Count <= 0)
            {
                return;
            }

            prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
            thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
            thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
            thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
        }

        /// <summary>
        /// 課別コード取得処理 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGroupComboListByUser()
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F207/GetGroupComboListByUser?sid={solutionIdShort}&fid={formIdShort}";

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
        /// 入出庫ファイル更新処理WebAPI呼び出し
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg) UpdateIOFileAPI()
        {

            // 必要なパラメータ設定済
            var dataCate = controlListII.SGetText("dataCateC1TextBox");
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");
            var supCode = controlListII.SGetText("supCodeC1TextBox");
            var jyuyoyosokuCode = controlListII.SGetText("jyuyoyosokuCodeC1TextBox");
            var inNum = controlListII.SGetText("inNumC1NumericEdit").Replace(",", "");
            var unitPriceCate = controlListII.SGetText("unitPriceCateC1ComboBox");
            var unitPrice = controlListII.SGetText("unitPriceC1NumericEdit").Replace(",", "");
            var processUnitPrice = controlListII.SGetText("processUnitPriceC1NumericEdit").Replace(",", "");
            var price = controlListII.SGetText("inPriceC1NumericEdit").Replace(",", "");
            var acceptDate = controlListII.SGetText("inDateC1DateEdit");
            var groupCode = controlListII.SGetText("groupCodeC1TextBox");
            var accountCode = controlListII.SGetText("accountCodeC1TextBox");
            var doCode = controlListII.SGetText("doCodeC1TextBox");
            var stockCate = controlListII.SGetText("stockCateC1ComboBox");
            var nSupCate = controlListII.SGetText("transCateC1ComboBox");
            var password = LoginInfo.Instance.UserNo;
            var machineName = LoginInfo.Instance.MachineCode;           
            var createDate = DateTime.Now.ToString();
            var den_NO = "";
            var createStaffCode = LoginInfo.Instance.UserNo;
            var createID = LoginInfo.Instance.UserId;

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("dataCate", new JValue(dataCate));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCode));
            param.Add("inNum", new JValue(inNum));
            param.Add("unitPriceCate", new JValue(unitPriceCate));
            param.Add("unitPrice", new JValue(unitPrice));
            param.Add("processUnitPrice", new JValue(processUnitPrice));
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
            param.Add("den_NO", new JValue(den_NO));
            param.Add("createStaffCode", new JValue(createStaffCode));
            param.Add("createID", new JValue(createID));
            param.Add("isEOM", new JValue(false));
            param.Add("num", new JValue(inNum));

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F207/CreateIOFile?sid={solutionIdShort}&fid={formIdShort}";

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
                return (false, false, (string)result["msg"]);
            }

            return (true, false, "");
        }

        #endregion  ＜その他処理 END＞
    }
}
