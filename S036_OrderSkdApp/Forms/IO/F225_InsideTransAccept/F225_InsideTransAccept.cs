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
using System.Data.SqlClient;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 社内移行受付（現品票番号）
    /// </summary>
    public partial class F225_InsideTransAccept : BaseForm
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

        private DateTime todayDate = DateTime.Today;

        /// <summary>
        /// 変更前 部品コード
        /// </summary>
        private string stPartsCode = "";

        /// <summary>
        /// 変更前 仕入先
        /// </summary>
        private string stSupCode = "";

        /// <summary>
        /// 現品票　読込時更新時比較用
        /// </summary>
        private DataTable dtGoodsTag;

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"]
                                + "Solution/S036/F225/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F225_InsideTransAccept(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "社内移行受付（現品票番号）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F225_InsideTransAccept_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1ComboBox, groupNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(goodsTagCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(sakubanC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(delivDateC1DateEdit, null, null, true, enumCate.無し);
                AddControlListII(delivNumC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(delivPriceC1NumericEdit, null, null, false, enumCate.無し);
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
                AddControlListII(delivUnitPriceC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(mfgC1NumericEdit, null, null, false, enumCate.無し);
                AddControlListII(salesDateC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(goodsSumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(delivSumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(remainNumC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(ordPriceC1NumericEdit, null, null, false, enumCate.無し); 


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

            delivDateC1DateEdit.Text = todayDate.ToShortDateString();
            groupCodeC1ComboBox.SelectedIndex = 0;
            SetExecuteDate(groupCodeC1ComboBox.Text);
            stSupCode = "";
            stPartsCode = "";

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = goodsTagCodeC1TextBox;
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
        /// 仕入先検索ボタン押下時
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
        /// 現品票番号　検証時
        /// </summary>
        private void goodsTagCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = GoodsTagCodeErrorCheck();
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
        /// 現品票番号　検証後
        /// </summary>
        private void goodsTagCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }


                var result1 = GetGoodsTag(t.Text);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "現品票ファイル検索時に");
                    ActiveControl = t;
                    return;
                }

                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "現品票番号", "現品票ファイル");
                    ActiveControl = t;
                    return;
                }

                ordPriceC1NumericEdit.Value = result1.Table.Rows[0].Field<double>("salesUnitPrice").ToString() ?? "";
                delivSumC1TextBox.Text = "0";

                var salesDate = DBNull.Value.Equals(result1.Table.Rows[0]["salesDate"]) ? "" :
                                        result1.Table.Rows[0]["salesDate"].ToString();

                var isOk = DateTime.TryParse(salesDate, out var date);
                if (isOk == false)
                {
                    ChangeTopMessage("E0008", "現品票ファイルの売上年月日参照時に");
                    ActiveControl = t;
                    return;
                }

                if (date.ToString() == "")
                {
                    delivDateC1DateEdit.Value = todayDate.ToShortDateString();
                }
                else
                {
                    delivDateC1DateEdit.Value = date;
                }

                outDataCateC1TextBox.Text = "";
                entryDateC1TextBox.Text = todayDate.ToShortDateString();

                delivUnitPriceC1NumericEdit.Value = result1.Table.Rows[0].Field<double>("salesUnitPrice").ToString() ?? "";
                mfgC1NumericEdit.Value = result1.Table.Rows[0].Field<double>("salesProcessUnitPrice").ToString() ?? "";
                delivNumC1NumericEdit.Value = result1.Table.Rows[0].Field<double>("delivInstructionNum").ToString() ?? "";
                delivPriceC1NumericEdit.Value = result1.Table.Rows[0].Field<double>("salesPrice").ToString() ?? "";
                doCodeC1TextBox.Text = result1.Table.Rows[0].Field<string>("salesDoCode") ?? "";
                var mfgGroup = result1.Table.Rows[0].Field<string>("salesDoCode") ?? "";
                if (mfgGroup == "4345")
                {
                    dataCateC1TextBox.Text = "3";
                }
                else
                {
                    dataCateC1TextBox.Text = "2";
                }

                sakubanC1TextBox.Text = result1.Table.Rows[0].Field<string>("sakuban") ?? "";
                poCodeC1TextBox.Text = result1.Table.Rows[0].Field<string>("poCode") ?? "";
                salesDateC1TextBox.Text = date.ToShortDateString();

                // 読込時datatable保持
                dtGoodsTag = result1.Table;


                var param = new SansoBase.ProcessMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                param.WhereColuList.Add((param.SupCode, supCodeC1TextBox.Text));
                param.SetDBName("製造調達");
                var result2 = CommonAF.ExecutSelectSQL(param);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "工程マスタ検索時に");
                    return;
                }

                if (result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", t.Label.Text, "工程マスタ");
                    return;
                }
                else
                {
                    stockCateC1ComboBox.Text = result2.Table.Rows[0].Field<string>("在庫P") ?? "";
                }


                partsCodeC1TextBox_Validated(partsCodeC1TextBox, e);
                supCodeC1TextBox_Validated(supCodeC1TextBox, e);
                stockCateC1ComboBox_Validated(stockCateC1ComboBox, e);

                SetStockInfo();


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

                if (result1.Table.Rows.Count <= 0)
                {
                    partsNameC1TextBox.Text = "部品マスタ未登録";
                }
                else
                {
                    partsNameC1TextBox.Text = result1.Table.Rows[0].Field<string>("部品名") ?? "";
                }
                
                stPartsCode = t.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 仕入先 検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 仕入先チェック
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
        /// 仕入先 検証後
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

                // 仕入先マスタ
                var paramSup = new SansoBase.SupMst();
                paramSup.SelectStr = "*";
                paramSup.WhereColuList.Add((paramSup.SupCode, t.Text));
                paramSup.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(paramSup);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                    ActiveControl = t;
                    return;
                }

                if (result.Table.Rows.Count <= 0)
                {
                    supNameC1TextBox.Text = "仕入先マスタ未登録";
                    supOsrcCateC1TextBox.Text = "";

                }
                else
                {
                    supNameC1TextBox.Text = result.Table.Rows[0].Field<string>("仕入先名１") ?? "";
                    supOsrcCateC1TextBox.Text = result.Table.Rows[0].Field<string>("仕入先区分") ?? "";
                }

                stSupCode = t.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 単価 検証時
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
                    ActiveControl = delivUnitPriceC1NumericEdit;
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
        /// 数量、単価 検証後
        /// </summary>
        private void C1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                // 金額の算出
                var outNum = delivNumC1NumericEdit.Text;
                var unitPrice = delivUnitPriceC1NumericEdit.Text;
                decimal d1 = outNum == "" ? 0 : System.Convert.ToDecimal(outNum);
                decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
                decimal d = d1 * d2;

                var round = Math.Round(d);
                var check = SansoBase.Check.IsPointNumberRange(round, 11, 0);
                if (check.Result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    ChangeTopMessage(1, "WARN", "数量 × 単価の" + check.Msg);
                    return;
                }

                delivPriceC1NumericEdit.Value = round;
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
        /// 数量　検証時
        /// </summary>
        private void delivNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
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
                if (d1 == 0)
                {
                    ChangeTopMessage("W0016", "数量に０");
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
            var isOk1 = GoodsTagCodeErrorCheck();
            if (isOk1 == false)
            {
                ActiveControl = goodsTagCodeC1TextBox;
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
                ActiveControl = delivUnitPriceC1NumericEdit;
                return false;
            }

            var isOk5 = delivDateErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = delivDateC1DateEdit;
                return false;
            }

            var isOk6 = DoCodeErrorCheck();
            if (isOk6 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            // 金額の確認
            var outNum = delivNumC1NumericEdit.Text;
            var unitPrice = delivUnitPriceC1NumericEdit.Text;
            decimal d1 = outNum == "" ? 0 : System.Convert.ToDecimal(outNum);
            decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
            decimal d = d1 * d2;
            var round = Math.Round(d);
            var outPrice = System.Convert.ToDecimal(delivPriceC1NumericEdit.Text);

            if (round != outPrice)
            {
                ChangeTopMessage(1, "WARN", "金額が数量 × 単価と異なります");
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

                // 現品票ファイル更新確認
                var result1 = GetGoodsTag(goodsTagCodeC1TextBox.Text);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "現品票ファイル検索時に");
                    return;
                }

                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "現品票番号", "現品票ファイル");
                    return;
                }

                bool judge = dtGoodsTag.AsEnumerable().SequenceEqual(result1.Table.AsEnumerable(), DataRowComparer.Default);
                if (!judge)
                {
                    ChangeTopMessage(1, "WARN", "他の端末で納入指示が修正されています。もう一度やり直してください。");
                    return;
                }


                // VSANSODB_AT_GENPIN、入出庫ファイル、在庫マスタ、素材在庫マスタ更新
                apiParam.RemoveAll();
                apiParam.Add("dbName", new JValue("製造調達"));
                apiParam.Add("dataCate", new JValue(dataCateC1TextBox.Text));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(sakubanC1TextBox.Text.Substring(0, 10)));
                apiParam.Add("poCode", new JValue(poCodeC1TextBox.Text));
                apiParam.Add("inNum", new JValue(delivNumC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("unitPrice", new JValue(delivUnitPriceC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("processUnitPrice", new JValue(mfgC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("price", new JValue(delivPriceC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("acceptDate", new JValue(delivDateC1DateEdit.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.Text));
                apiParam.Add("nSupCate", new JValue(transCateC1ComboBox.Text));
                apiParam.Add("password", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("sakuban", new JValue(sakubanC1TextBox.Text));
                apiParam.Add("den_NO", new JValue(goodsTagCodeC1TextBox.Text));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString()));
                apiParam.Add("createStaffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("createID", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("isEOM", new JValue(false));

                var result = ApiCommonUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result.Msg);
                    return;
                }

                DisplayClear();
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0001", "出庫処理");

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
        /// 現品票番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool GoodsTagCodeErrorCheck()
        {
            var t = goodsTagCodeC1TextBox;
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


            var result1 = GetGoodsTag(t.Text);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "現品票ファイル検索時に");
                ActiveControl = t;
                return false;
            }

            //if (result1.Table.Rows.Count <= 0)
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "現品票番号", "現品票ファイル");
                ActiveControl = t;
                return false;
            }

            sakubanC1TextBox.Text = result1.Table.Rows[0].Field<string>("sakuban") ?? "";
            poCodeC1TextBox.Text = result1.Table.Rows[0].Field<string>("poCode") ?? "";
            partsCodeC1TextBox.Text = result1.Table.Rows[0].Field<string>("partsCode") ?? "";
            supCodeC1TextBox.Text = result1.Table.Rows[0].Field<string>("processGroupCode") ?? "";
            goodsSumC1TextBox.Text = result1.Table.Rows[0].Field<double>("delivInstructionNum").ToString() ?? "";

            outDataCateC1TextBox.Text = "";
            entryDateC1TextBox.Text = todayDate.ToShortDateString();

            var salesFlg = result1.Table.Rows[0].Field<object>("salesCompFlg").ToString() ?? "";
            var outFlg = result1.Table.Rows[0].Field<string>("outFlg") ?? "";

            if (salesFlg == "1" && outFlg == "0")
            {
                // 処理なし
            }
            else
            {
                ChangeTopMessage(1, "WARN", "入力済みか、処理できない現品票です。");
                ActiveControl = t;
                return false;
                
            }

            var groupAssembly = result1.Table.Rows[0].Field<string>("groupCode") ?? "";
            bool existFlg = false;
            for (int i = 0; i < groupCodeC1ComboBox.Items.Count; i++)
            {
                var vvv = groupCodeC1ComboBox.Items[i].ToString();
                if (groupCodeC1ComboBox.Items[i].ToString() == groupAssembly)
                {
                    groupCodeC1ComboBox.SelectedIndex = i;
                    existFlg = true;
                }
            }
            if (!existFlg)
            {
                ChangeTopMessage(1, "WARN", "所属と現品票データの部門が一致していません。");
                ActiveControl = t;
                return false;
            }

            var groupMfg = result1.Table.Rows[0].Field<string>("processGroupCode") ?? "";
            var result2 = GetAddress(groupMfg);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "住所録マスタ検索時に");
                ActiveControl = t;
                return false;
            }

            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                var param3 = new SansoBase.SupMst();
                param3.SelectStr = "*";
                param3.WhereColuList.Add((param3.SupCode, groupMfg));
                param3.WhereColuList.Add((param3.SupCate, "K"));
                param3.SetDBName("製造調達");
                var result3 = CommonAF.ExecutSelectSQL(param3);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                    ActiveControl = t;
                    return false;
                }

                if (result3.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "課別コード", "住所録、部門マスタ");
                    ActiveControl = t;
                    return false;
                }
                else
                {
                    supOsrcCateC1TextBox.Text = "K";
                }
            }
            else
            {
                var transStopCate = result2.Table.Rows[0].Field<string>("取引停止区分") ?? "";
                //var transStopCate = result2.Table.Rows[0].Field<string>("tradeStopCate") ?? "";
                if (transStopCate == "D" || transStopCate == "K")
                {
                    ChangeTopMessage(1, "WARN", "取引停止仕入先です。");
                    ActiveControl = t;
                    return false;
                }
            }

            //var salesDate = DBNull.Value.Equals(result1.Table.Rows[0]["売上年月日"]) ? "" :
            //                                    result1.Table.Rows[0]["売上年月日"].ToString();
            var salesDate = DBNull.Value.Equals(result1.Table.Rows[0]["salesDate"]) ? "" :
                                                  result1.Table.Rows[0]["salesDate"].ToString();

            var isOk2 = DateTime.TryParse(salesDate, out var date);
            if (isOk2 == false)
            {
                ChangeTopMessage("E0008", "現品票ファイルの売上年月日参照時に");
                ActiveControl = t;
                return false;
            }
            
            // 処理年月と同じ日付のみ入力可能
            if (date < startDate || date > endDate)
            {
                ChangeTopMessage("W0016", "処理年月と異なる日付");
                return false;
            }

            if (todayDate < date)
            {
                ChangeTopMessage("W0016", "処理年月と異なる日付");
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
        /// 仕入先チェック
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
            var result = CommonAF.ExecutSelectSQL(paramSup);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "仕入先マスタ");
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
            var t = delivUnitPriceC1NumericEdit;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            decimal.TryParse(delivUnitPriceC1NumericEdit.Text, out decimal unitPrice);
            if (unitPrice == 0)
            {
                ChangeTopMessage("W0016", "単価に０");
                return false;
            }

            // 単価マスタ
            var paramSup = new SansoBase.UnitPriceMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.PartsCode, partsCodeC1TextBox.Text));
            paramSup.WhereColuList.Add((paramSup.SupCode, supCodeC1TextBox.Text));
            paramSup.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(paramSup);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                // 処理なし
            }
            else
            {
                var supUnitPrice = result.Table.Rows[0].Field<decimal>("仕入単価").ToString() ?? "";
                if (supUnitPrice != ordPriceC1NumericEdit.Text.Replace(",", ""))
                {
                    ChangeTopMessage(1, "WARN", "発注単価と異なります。注意してください。");
                }
            }

            return true;
        }

        /// <summary>
        /// 日付チェック
        /// </summary>
        /// <returns>true：チェックＯＫ、false：チェックＮＧ</returns>
        private bool delivDateErrorCheck()
        {
            var d = delivDateC1DateEdit;

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
                else if (stockCateC1ComboBox.Text.Trim() == "")
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
                    ChangeTopMessage(1, "WARN", "部門マスタ検索 " + result.Msg);
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
        
        /// <summary>
        /// 課別コード　取得
        /// </summary>
        /// <param name="id">ユーザID</param>
        public (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGroupManufactDirect()
        {
            
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F225/GetGroupComboListByUser?sid={solutionIdShort}&fid={formIdShort}";

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
           
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F225/GetExecuteDate?sid={solutionIdShort}&fid={formIdShort}";

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
            
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F225/GetUnitPriceMst?sid={solutionIdShort}&fid={formIdShort}";

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


        /// <summary>
        /// 現品票ファイル取得
        /// </summary>
        /// <param name="goodsTagCode">現品票番号</param>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGoodsTag(string goodsTagCode)
        {

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F225/GetTagFile?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("tagCode", new JValue(goodsTagCode));

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
        /// 住所録マスタ取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetAddress(string supCode)
        {
            string sql = "SELECT * FROM Ｖ三相メイン＿住所録マスタ WHERE 会社コード = '" + supCode + "'";
            var af = CommonAF.ExecutSelectSQL(sql);
            return (af.IsOk, af.Table);

            //apiParam.RemoveAll();
            //apiParam.Add("supCode", new JValue(supCode));
            //var result = ApiCommonGet(apiUrl + "GetAdressMst", apiParam);
            //return (result.IsOk, result.Table);
            
        }


        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0], エラーメッセージ)</returns>
        private (bool IsOk, int Count, string Msg, string doCode) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
        {
            apiUrl += $"?sid={solutionIdShort}&fid={formIdShort}";

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
                        return (false, 0, (string)(result["msg"]), "");
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, (string)(result["msg"]), "");
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
                return (false, 0, (string)(result["msg"]), "");
            }
            
            //return (
            //    true,
            //    (int)(result["count"]),
            //    "",
            //    (string)(result["doCode"])
            //);
            
            return (
                true,
                System.Convert.ToInt32(result["count"]),
                "",
                (result["doCode"] == null) ? "null" : (string)result["doCode"]
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
            apiUrl += $"?sid={solutionIdShort}&fid={formIdShort}";

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
