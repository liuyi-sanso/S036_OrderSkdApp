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
    /// 月末出庫処理 有償支給発行（外注）
    /// </summary>
    public partial class F203E_OutProcessOsrc : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] 
            + "Solution/S036/F203E/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 処理年月　開始日
        /// </summary>
        private DateTime startDate = SansoBase.DatetimeFC.GetBeginOfMonth(DateTime.Today);

        /// <summary>
        /// 処理年月　終了日
        /// </summary>
        private DateTime endDate = SansoBase.DatetimeFC.GetEndOfMonth(DateTime.Today);

        /// <summary>
        /// 変更前 出庫先
        /// </summary>
        private string stSupCode = "";

        /// <summary>
        /// 需要予測 保管
        /// </summary>
        private string jyuyoyosoku = "";

        /// <summary>
        /// 課別コード保管（ログインユーザから取得）
        /// </summary>
        private DataTable groupCodeDt = new DataTable();


        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F203E_OutProcessOsrc(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "月末有償支給発行（外注）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F203E_OutProcessOsrc_Load(object sender, EventArgs e)
        {
            try
            {
                // 月末
                IsEOMTitleBackColor = true;

                // コントロールリストIIの要素
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDateC1DateEdit, null, endDate.ToString(), true, enumCate.無し);
                AddControlListII(outNumC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(outPriceC1NumericEdit, null, null, false, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supOsrcCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(transCateC1ComboBox, transCateC1TextBox, "", true, enumCate.無し);
                AddControlListII(transCateC1TextBox, null, null, false, enumCate.無し);
                AddControlListII(unitPriceC1NumericEdit, null, "0", true, enumCate.無し);
                AddControlListII(prevMonthStockNumEOMC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthInNumEOMC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthOutNumEOMC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthStockNumEOMC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthInNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthOutNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(thisMonthStockNumC1TextBox, null, "", false, enumCate.Key);

                
                // 印刷用プリンタ（OutsideTrans）のドライバがインストールされているかどうかをチェック
                string outsideTransPrinterName = System.Configuration.ConfigurationManager.AppSettings["OutsideTrans"];
                var pjudge = false;
                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == outsideTransPrinterName) { pjudge = true; }
                }

                if (pjudge == false)
                {
                    MessageBox.Show("OutsideTrans（プリンタ）がありません。設定してください。" +
                                    "\r\n処理を中止します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                
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

            // 初期設定
            doCodeC1CheckBox.Checked = false;
            c1TrueDBGrid.SetDataBinding(null, "", true);
            outDateC1DateEdit.Value = endDate;
            stSupCode = "";

            var groupCode = groupCodeDt.Rows[0].Field<string>("groupCode") ?? "";
            var groupName = groupCodeDt.Rows[0].Field<string>("groupName") ?? "";
            groupCodeC1TextBox.Text = groupCode;
            groupNameC1TextBox.Text = groupName;

            stockCateC1ComboBox.SelectedIndex = 0;
            stockCateNameC1TextBox.Text = stockCateC1ComboBox.SGetText(1);
            transCateC1ComboBox.SelectedIndex = 0;
            transCateC1TextBox.Text = transCateC1ComboBox.SGetText(1);

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // 需要予測はクリアしない
            jyuyoyosokuCodeC1TextBox.Text = jyuyoyosoku;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = jyuyoyosokuCodeC1TextBox;
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
                        stSupCode = supCodeC1TextBox.Text;
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
                if (isRunValidating == false)
                {
                    return;
                }

                partsNameC1TextBox.Text = "";

                var t = (C1TextBox)sender;
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

                    if (result2.Table == null || result2.Table.Rows.Count <= 0)
                    {
                        // 処理なし
                    }
                    else
                    {
                        unitPriceC1NumericEdit.Text = result2.Table.Rows[0]["支給単価"].ToString();
                    }

                }

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
                unitPriceC1NumericEdit.Text = result.Table.Rows[0]["支給単価"].ToString();

                // 在庫P取得
                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                var result2 = ApiCommonGet(apiUrl + "GetStockCate", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result2.Table != null && result2.Table.Rows.Count >= 1)
                {
                    stockCateC1ComboBox.Text = DBNull.Value.Equals(result2.Table.Rows[0].Field<string>("stockCate")) ? "" : result2.Table.Rows[0].Field<string>("stockCate").ToString();
                    stockCateNameC1TextBox.Text = "";
                    var dv1 = (System.Data.DataView)stockCateC1ComboBox.ItemsDataSource;
                    dv1.RowFilter = dv1.Table.Columns[0].ColumnName + " = '" + stockCateC1ComboBox.Text + "' ";
                    if (dv1.Count != 0) { stockCateNameC1TextBox.Text = dv1.ToTable().Rows[0][1].ToString(); }

                }

                if (groupCodeC1TextBox.Text == "3623")
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
        /// 出庫単価 検証時
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
        /// 出庫数量、出庫単価 検証後
        /// </summary>
        private void C1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                // 出庫金額の算出
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
                    ChangeTopMessage(1, "WARN", "出庫数量 × 出庫単価の" + check.Msg);
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
        /// 伝票番号チェック 変更時
        /// </summary>
        private void doCodeC1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (doCodeC1CheckBox.Checked == true)
            {
                doCodeC1TextBox.Text = "";
                doCodeC1TextBox.Enabled = true;
                doCodeC1TextBox.BackColor = Color.White;
            }
            else
            {
                doCodeC1TextBox.Text = "";
                doCodeC1TextBox.Enabled = false;
                doCodeC1TextBox.BackColor = Color.PeachPuff;
            }
        }

        /// <summary>
        /// 伝票番号 検証時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 備考 検証時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remarksC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = RemarksErrorCheck(remarksC1TextBox, 50);
                if (isOk == false)
                {
                    ActiveControl = remarksC1TextBox;
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

                // 在庫情報名 セット
                if (stockCateC1ComboBox.Text == "Z")
                {
                    stockInfoC1TextBox.Text = "完成品在庫";
                }
                else if (stockCateC1ComboBox.Text.Trim() == "")
                {
                    stockInfoC1TextBox.Text = "素材在庫";
                }
                else
                {
                    stockInfoC1TextBox.Text = "";
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

            var isOk6 = RemarksErrorCheck(remarksC1TextBox, 50);
            if (isOk6 == false)
            {
                ActiveControl = remarksC1TextBox;
                return false;
            }

            if (doCodeC1CheckBox.Checked)
            {
                if (doCodeC1TextBox.Text == "")
                {
                    ChangeTopMessage("W0007", doCodeC1TextBox.Label.Text);
                    ActiveControl = doCodeC1TextBox;
                    return false;
                }
            }

            var isOk7 = DoCodeErrorCheck();
            if (isOk7 == false)
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
                ChangeTopMessage(1, "WARN", "出庫金額が出庫数量 × 出庫単価と異なります");
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
                var doCodeC1Check = doCodeC1CheckBox.Checked;

                apiParam.RemoveAll();
                apiParam.Add("dbName", new JValue("製造調達"));
                apiParam.Add("dataCate", new JValue(dataCateC1TextBox.Text));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                apiParam.Add("outNum", new JValue(outNumC1NumericEdit.Value));
                apiParam.Add("unitPrice", new JValue(unitPriceC1NumericEdit.Value));
                apiParam.Add("price", new JValue(outPriceC1NumericEdit.Value));
                apiParam.Add("acceptDate", new JValue(outDateC1DateEdit.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1TextBox.Text));
                apiParam.Add("accountCode", new JValue(accountCodeC1TextBox.Text));
                apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.Text));
                apiParam.Add("nSupCate", new JValue(transCateC1ComboBox.Text));
                apiParam.Add("password", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("nSupStockTransDataFlg", new JValue(doCodeC1Check == true ? "" : "M"));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString()));
                apiParam.Add("remarks", new JValue(remarksC1TextBox.Text));
                apiParam.Add("createStaffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("createID", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("doCodeCheck", new JValue(doCodeC1Check));
                apiParam.Add("isEOM", new JValue(true));
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("staffId", new JValue(LoginInfo.Instance.UserId));

                var result = ApiIOFileUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0001", "入出庫ファイル");
                    return;
                }

                // チェックが入っていない場合、印刷処理
                if (doCodeC1CheckBox.Checked == false)
                {
                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage(1, "WARN", "伝票番号のチェックは無しですが、印刷データがありませんでした。");
                    }
                    else
                    {
                        var isOk = PrintProc(result.Table);
                        if (isOk == false)
                        {
                            return;
                        }
                    }
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

        /// <summary>
        /// 印刷処理
        /// </summary>
        private bool PrintProc(DataTable dt)
        {
            // マウスカーソル待機状態
            Cursor = Cursors.WaitCursor;

            // 機種名取得
            var result2 = GetProductName();
            var productName = result2.productName;

            // レポート印刷
            var report = new C1.Win.FlexReport.C1FlexReport();
            report.Load(EXE_DIRECTORY + @"\Reports\R012_IONSupStockTrans.flxr", "R012_IONSupStockTrans");

            // データソース設定
            var ds = new C1.Win.FlexReport.DataSource
            {
                Name = " ",
                ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                Recordset = dt
            };
            report.DataSources.Add(ds);
            report.DataSourceName = ds.Name;

            // 個別設定            
            var visible = (supCodeC1TextBox.Text == "3730" ? true : false);
            var barCode = dt.Rows[0]["barCode"].ToString();
            ((C1.Win.FlexReport.Field)report.Fields["バーコード１"]).Visible = visible;
            ((C1.Win.FlexReport.Field)report.Fields["バーコード２"]).Visible = visible;
            ((C1.Win.FlexReport.Field)report.Fields["機種名１"]).Text = productName;
            ((C1.Win.FlexReport.Field)report.Fields["機種名２"]).Text = productName;
            ((C1.Win.FlexReport.Field)report.Fields["機種名３"]).Text = productName;

            // 即印刷
            var p = new System.Drawing.Printing.PrinterSettings();
            p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutsideTrans"];
            report.Render();
            var print = PrintReport(report, false, p);
            if (print.IsOk == false)
            {
                ChangeTopMessage("E0008", "印刷処理で");
                return false;
            }

            // マウスカーソル待機状態を解除
            Cursor = Cursors.Default;

            return true;
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
                if (supCate.Trim() != "G")
                {
                    ChangeTopMessage("W0016", "外注以外");
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

            dataCateC1TextBox.Text = "6"; // 有償       
            accountCodeC1TextBox.Text = "083"; // 外注

            // 単価マスタ存在チェック
            var result = GetUnitPrice();
            if (result.IsOk == false)
            {
                return false;
            }

            // 住所録マスタチェック
            var result2 = SelectDBAF.GetSansoMainAddressMst(t.Text);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "住所録マスタ検索時に");
                return false;
            }
            if (result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "住所録マスタ");
                return false;
            }

            var stopCate = result2.Table.Rows[0].Field<string>("取引停止区分").Substring(1, 1) ?? "";
            if (stopCate == "D" || stopCate == "K")
            {
                ChangeTopMessage("W0016", "取引停止仕入先");
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

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 備考チェック
        /// </summary>
        /// <param name="byteNum">最大バイト数</param>
        /// <param name="text">チェックをする文字列</param>
        /// <returns>true：チェックＯＫ、false：チェックＮＧ</returns>
        private bool RemarksErrorCheck(C1TextBox t, int byteNum)
        {
            if (isRunValidating == false)
            {
                return true;
            }

            // 未入力時処理
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            // 使用禁止文字のチェック
            var c = Check.HasBanChar(t.Text).Result;
            if (c == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, _) = Check.IsByteRange(t.Text, 0, byteNum);
            if (isOK == false)
            {
                ChangeTopMessage("W0014", t.Label.Text, $"　バイト数{byteNum}");
                return false;
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
            prevMonthStockNumC1TextBox.Text = "0";
            thisMonthInNumC1TextBox.Text = "0";
            thisMonthOutNumC1TextBox.Text = "0";
            thisMonthStockNumC1TextBox.Text = "0";
            prevMonthStockNumEOMC1TextBox.Text = "0";
            thisMonthInNumEOMC1TextBox.Text = "0";
            thisMonthOutNumEOMC1TextBox.Text = "0";
            thisMonthStockNumEOMC1TextBox.Text = "0";

            // 部品コード、課別コードのどちらかが入力ない場合は何もしない
            if (string.IsNullOrEmpty(partsCodeC1TextBox.Text) == true
                || string.IsNullOrEmpty(groupCodeC1TextBox.Text) == true)
            {
                return;
            }

            (bool IsOk, DataTable Table, string Sql) result;
            (bool IsOk, DataTable Table, string Sql) resultE;

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

                // 月末在庫マスタ
                var paramE = new StockMstE();
                paramE.SelectStr = "*";
                paramE.WhereColuList.Add((paramE.PartsCode, partsCodeC1TextBox.Text));
                paramE.WhereColuList.Add((paramE.GroupCode, groupCodeC1TextBox.Text));
                paramE.SetDBName("製造調達");
                resultE = CommonAF.ExecutSelectSQL(paramE);
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

                // 月末素材在庫マスタ
                var paramE = new MaterialStockMstE();
                paramE.SelectStr = "*";
                paramE.WhereColuList.Add((paramE.PartsCode, partsCodeC1TextBox.Text));
                paramE.WhereColuList.Add((paramE.GroupCode, groupCodeC1TextBox.Text));
                paramE.SetDBName("製造調達");
                resultE = CommonAF.ExecutSelectSQL(paramE);
            }

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                return;
            }
            if (resultE.IsOk == false)
            {
                ChangeTopMessage("E0008", "月末在庫マスタ/月末素材在庫マスタ検索時に");
                return;
            }

            if (result.Table.Rows.Count >= 1)
            {
                // 当月にセット
                prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
                thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
                thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
                thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
            }

            if (resultE.Table.Rows.Count >= 1)
            {
                // 月末にセット
                prevMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
                thisMonthInNumEOMC1TextBox.Text = resultE.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
                thisMonthOutNumEOMC1TextBox.Text = resultE.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
                thisMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
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
                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                var result = ApiCommonGet(apiUrl + "GetIOProcessUnitPrice", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return false;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
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
        /// 単価マスタ取得
        /// </summary>
        /// <returns>True：エラー無しかつデータ有り False：エラー有りもしくは0件、
        /// 取得データ(falseの場合はnull)
        /// </returns>
        private (bool IsOk, DataTable Table) GetUnitPrice()
        {

            var param = new SansoBase.UnitPriceMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
            param.WhereColuList.Add((param.SupCode, supCodeC1TextBox.Text));
            param.SetDBName("製造調達");
            var result1 = CommonAF.ExecutSelectSQL(param);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return (false, null);
            }

            if (result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "単価マスタ");
                return (false, null);
            }

            return (result1.IsOk, result1.Table);
        }

        /// <summary>
        /// レポート機種名取得
        /// </summary>
        /// <returns>IsOk:エラー無し False:エラー有り、機種名</returns>
        private (bool IsOk, string productName) GetProductName()
        {
            var productName = "";
            if (jyuyoyosokuCodeC1TextBox.Text.Trim() == "")
            {
                // 需要予測番号が入力無い場合、部品構成表から取得
                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                var result = ApiCommonGet(apiUrl + "GetBOMMst", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部品構成表検索時に");
                    return (false, "");
                }

                if (result.Table != null && result.Table.Rows.Count >= 1)
                {
                    productName = result.Table.Rows[0]["productName"].ToString();
                }

                return (true, productName);
            }
            else
            {
                // 需要予測番号が入力ある場合、製造指令ファイルから取得
                apiParam.RemoveAll();
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text.Replace("-", "")));
                var result = ApiCommonGet(apiUrl + "GetManufactFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "製造指令ファイル検索時に");
                    return (false, "");
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    productName = "需要予測番号エラー";
                }
                else
                {
                    var temp = result.Table.Rows[0]["productName"].ToString();
                    productName = temp == "" ? "機種マスタ無し" : temp;
                }

                return (true, productName);
            }
        }

        /// <summary>
        /// 課別コード、処理年月取得
        /// </summary>
        /// <returns></returns>
        private void GetGroupCode()
        {
            // 課別コード取得
            apiParam.RemoveAll();
            apiParam.Add("id", new JValue(LoginInfo.Instance.UserId.ToUpper()));
            var result1 = ApiCommonGet(apiUrl + "GetGroupComboListByUser", apiParam);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }

            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "ユーザ", "マスタ");
                return;
            }

            groupCodeDt = result1.Table;
            var groupCode = groupCodeDt.Rows[0].Field<string>("groupCode") ?? "";
            if (groupCode == "3623")
            {
                // 在庫P = Z にする
                stockCateC1ComboBox.SelectedIndex = 1;
            }

            // 月末処理の処理年月取得
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(groupCode));
            var result2 = ApiCommonGet(apiUrl + "GetEOMExecuteDate", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }

            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "課別コード", "部門マスタ");
                return;
            }

            var isOk = DateTime.TryParse(result2.Table.Rows[0].Field<string>("executeDate"), out var date);
            if (isOk == false)
            {
                ChangeTopMessage("W0002", "処理年月", "部門マスタ");
                return;
            }

            startDate = date;
            endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);
            executeDateValueLabel.Text = startDate.ToString("yyyy/MM");
            executeDateValueLabel.Visible = true;
            executeDateLabel.Visible = true;
        }

        /// <summary>
        /// WebAPI側IOFile更新処理(データ取得有り)
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 取得データ, エラーメッセージ)</returns>
        private (bool IsOk, DataTable Table, string Msg) ApiIOFileUpdate(string apiUrl, JObject apiParam = null)
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
                        return (false, null, (string)(result["msg"]));
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, null, (string)(result["msg"]));
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
                return (false, null, (string)(result["msg"]));
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());

            return (true, table, "");
        }

        /// <summary>
        /// WebAPI側共通取得処理
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
