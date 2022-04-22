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
    /// 内部受付返品（部品）
    /// </summary>
    public partial class F220E_InsideAcceptanceCancel : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F220E/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 部品コードの変更前保管エリア
        /// </summary>
        private string stPartsCode = "";

        /// <summary>
        /// 仕入先コードの変更前保管エリア
        /// </summary>
        private string stSupCode = "";

        /// <summary>
        /// 属性の変更前保管エリア
        /// </summary>
        private string stOutCate = "";

        /// <summary>
        /// 大分類の変更前保管エリア
        /// </summary>
        private string stMainDivision = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F220E_InsideAcceptanceCancel(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "内部受付返品（部品）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F220E_InsideAcceptanceCancel_Load(object sender, EventArgs e)
        {
            try
            {
                // 月末
                IsEOMTitleBackColor = true;
                this.executeDateValueLabel.Text = DateTime.Parse(this.executeDateValueLabel.Text).AddMonths(-1).ToString("yyyy/MM");

                // 専用プリンタのドライバがインストールされているかどうか
                bool isInsideTransPrinter = false;
                string insideTransPrinterName = System.Configuration.ConfigurationManager.AppSettings["InsideTrans"];

                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == insideTransPrinterName)
                    {
                        isInsideTransPrinter = true;
                    }
                }

                if (isInsideTransPrinter == false)
                {
                    MessageBox.Show("必要なプリンタ(" + insideTransPrinterName +
                        ")がありません。処理を中止します。", "プリンタなしエラー",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                isRunValidating = false;

                // 部門マスタ
                var groupName = "";
                var param = new SansoBase.GroupMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.GroupCode, LoginInfo.Instance.GroupCode));
                param.SetDBName("三相メイン");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部門マスタ検索時に");
                }
                if (result.Table.Rows.Count >= 1)
                {
                    groupName = result.Table.Rows[0]["部門名"].ToString();
                }

                // コントロールリストをセット
                AddControlListII(productCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(productNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsCodeC1ComboBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outCateC1ComboBox, outCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(outCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(mainDivisionC1ComboBox, mainDivisionNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(mainDivisionNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(subDivisionC1ComboBox, subDivisionNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(subDivisionNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dateC1DateEdit, null, DateTime.Today.ToShortDateString(), true, enumCate.無し);
                AddControlListII(numC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(unitPriceC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(unitPriceCateC1ComboBox, unitPriceCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(unitPriceCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(processUnitPriceC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(priceC1NumericEdit, null, null, true, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, 
                    (LoginInfo.Instance.GroupCode == "3623" ? "Z" : ""), false, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, 
                    (LoginInfo.Instance.GroupCode == "3623" ? "仕掛品" : "素材"), false, enumCate.無し);
                AddControlListII(transCateC1ComboBox, transCateNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(transCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(processMstC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(unitPriceMstC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, null, LoginInfo.Instance.GroupCode, true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, groupName, false, enumCate.無し);

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
                SetOutCateC1ComboBox();
                SetMainDivisionC1ComboBox();
                SetUnitPriceCateC1ComboBox();
                SetStockCateC1ComboBox();
                SetTransCateC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後に実行（F10）を押してください。　　";

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

            // 初期設定
            stPartsCode = "";
            stSupCode = "";
            stOutCate = "";
            stMainDivision = "";
            SetPartsCodeC1ComboBox("");
            SetSubDivisionC1ComboBox("");
            partsCodeC1ComboBox.SelectedIndex = -1;
            outCateC1ComboBox.SelectedIndex = -1;
            mainDivisionC1ComboBox.SelectedIndex = -1;
            subDivisionC1ComboBox.SelectedIndex = -1;
            unitPriceCateC1ComboBox.SelectedIndex = -1;
            stockCateC1ComboBox.SelectedIndex = 0;
            transCateC1ComboBox.SelectedIndex = -1;
            DrawC1TrueDBGrid();
            doCodeC1CheckBox.Checked = false;
            doCodeC1TextBox.Enabled = false;
            doCodeC1TextBox.Label.Enabled = true;
            doCodeC1TextBox.BackColor = Color.PeachPuff;
            mainDivisionC1ComboBox.Enabled = false;
            mainDivisionC1ComboBox.Label.Enabled = true;
            mainDivisionC1ComboBox.BackColor = Color.PeachPuff;
            subDivisionC1ComboBox.Enabled = false;
            subDivisionC1ComboBox.Label.Enabled = true;
            subDivisionC1ComboBox.BackColor = Color.PeachPuff;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = productCodeC1TextBox;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 部品コード  コンボボックスセット
        /// </summary>
        /// <param name="productCode">機種コード</param>
        private void SetPartsCodeC1ComboBox(string productCode)
        {
            if (productCode == "")
            {
                partsCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            // 部品構成表
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("productCode", new JValue(productCode));
            var result = ApiCommonGet(apiUrl + "GetPartsList", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品構成表検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                partsCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            partsCodeC1ComboBox.TextDetached = true;
            partsCodeC1ComboBox.ItemsDataSource = result.Table.DefaultView;
            partsCodeC1ComboBox.ItemsDisplayMember = "rowNo";
            partsCodeC1ComboBox.ItemsValueMember = "rowNo";
            partsCodeC1ComboBox.ItemMode = C1.Win.C1Input.ComboItemMode.HtmlPattern;
            partsCodeC1ComboBox.HtmlPattern =
                    "<table>" +
                    "<tr>" +
                    "<td width=100>{partsCode}</td>" +
                    "<td width=250>{partsName}</td>" +
                    "<td width=30>{processNo}</td>" +
                    "<td width=60>{supCode}</td>" +
                    "<td width=250>{supName}</td>" +
                    "<td width=0>{rowNo}</td>" +
                    "</tr>" +
                    "</table>";
        }

        /// <summary>
        /// 属性  コンボボックスセット
        /// </summary>
        private void SetOutCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("1", "不良以外");
            dt.Rows.Add("9", "不良");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(outCateC1ComboBox, dt, outCateC1ComboBox.Width,
                outCateNameC1TextBox.Width, "ID", "NAME");
        }

        /// <summary>
        /// 大分類  コンボボックスセット
        /// </summary>
        private void SetMainDivisionC1ComboBox()
        {
            var result = ApiCommonGet(apiUrl + "GetMainDivisionList", null);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "大分類検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                mainDivisionC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(mainDivisionC1ComboBox, result.Table, mainDivisionC1ComboBox.Width,
                mainDivisionNameC1TextBox.Width, "mainDivision", "mainDivisionName");
        }

        /// <summary>
        /// 中分類  コンボボックスセット
        /// </summary>
        /// <param name="mainDivision">大分類</param>
        private void SetSubDivisionC1ComboBox(string mainDivision)
        {
            if (mainDivision == "")
            {
                subDivisionC1ComboBox.ItemsDataSource = null;
                return;
            }

            apiParam.RemoveAll();
            apiParam.Add("mainDivision", new JValue(mainDivision));
            var result = ApiCommonGet(apiUrl + "GetSubDivisionList", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "中分類検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                subDivisionC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(subDivisionC1ComboBox, result.Table, subDivisionC1ComboBox.Width,
                subDivisionNameC1TextBox.Width, "subDivision", "subDivisionName");
        }

        /// <summary>
        /// 単価区分  コンボボックスセット
        /// </summary>
        private void SetUnitPriceCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("1", "決");
            dt.Rows.Add("9", "未");
            dt.Rows.Add("K", "仮");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(unitPriceCateC1ComboBox, dt, unitPriceCateC1ComboBox.Width,
                unitPriceCateNameC1TextBox.Width, "ID", "NAME");
        }

        /// <summary>
        /// 在庫  コンボボックスセット
        /// </summary>
        private void SetStockCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("", "素材");
            dt.Rows.Add("Z", "仕掛品");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(stockCateC1ComboBox, dt, stockCateC1ComboBox.Width,
                stockCateNameC1TextBox.Width, "ID", "NAME");
        }

        /// <summary>
        /// 支給区分  コンボボックスセット
        /// </summary>
        private void SetTransCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("Y", "有償");
            dt.Rows.Add("M", "無償");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(transCateC1ComboBox, dt, transCateC1ComboBox.Width,
                transCateNameC1TextBox.Width, "ID", "NAME");
        }

        #endregion  ＜コンボボックス設定処理 END＞

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

                    case "partsCodeC1ComboBox":
                        partsSearchBt_Click(sender, e);
                        break;

                    case "supCodeC1TextBox":
                        supSearchButton_Click(sender, e);
                        break;

                    case "jyuyoyosokuCodeC1TextBox":
                        sakubanSearchButton_Click(sender, e);
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
                        productCodeC1TextBox.Text = form.row.Cells["機種コード"].Value.ToString().TrimEnd();
                        productNameC1TextBox.Text = form.row.Cells["機種名"].Value.ToString().TrimEnd();
                    }
                }
                ActiveControl = productCodeC1TextBox;
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
                        partsCodeC1ComboBox.Text = form.row.Cells["部品コード"].Value.ToString().TrimEnd();
                    }
                }

                ActiveControl = partsCodeC1ComboBox;

                var isOk1 = ErrorCheckPartsCode();
                if (isOk1 == false)
                {
                    ActiveControl = (C1ComboBox)sender;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
                {
                    ActiveControl = (C1ComboBox)sender;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 仕入先コード検索ボタン押下時
        /// </summary>
        private void supSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_SupMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                    }
                }

                ActiveControl = supCodeC1TextBox;

                var isOk1 = ErrorCheckSupCode();
                if (isOk1 == false)
                {
                    ActiveControl = supCodeC1TextBox;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
                {
                    ActiveControl = supCodeC1TextBox;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        ///　需要予測番号  検索ボタン押下時
        /// </summary>
        private void sakubanSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F904_SakubanCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        jyuyoyosokuCodeC1TextBox.Text = form.row.Cells["需要予測番号"].Value.ToString();
                    }
                }
                ActiveControl = jyuyoyosokuCodeC1TextBox;
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
                if (stockC1TrueDBGrid.EditActive)
                {
                    ActiveControl = F10Bt;
                }
                if (stockEC1TrueDBGrid.EditActive)
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

                    if (ctl.Label.Text == "部品コード") 
                    {
                        continue;
                    }

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
        /// 機種コード　検証している
        /// </summary>
        private void productCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckProductCode();
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
        /// 機種コード　検証された後
        /// </summary>
        private void productCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var temp = partsCodeC1ComboBox.Text;

                // 未入力時処理     
                var s = (C1TextBox)sender;
                if (string.IsNullOrEmpty(s.Text))
                {
                    if (productNameC1TextBox.Text == "")
                    {
                        SetPartsCodeC1ComboBox("");
                        return;
                    }

                    return;
                }

                SetPartsCodeC1ComboBox(s.Text);

                partsCodeC1ComboBox.Text = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 機種名　検証時
        /// </summary>
        private void productNameC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckProductName();
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
        /// 機種名　検証後
        /// </summary>
        private void productNameC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var temp = partsCodeC1ComboBox.Text;

                // 未入力時処理     
                var s = (C1TextBox)sender;
                if (string.IsNullOrEmpty(s.Text))
                {
                    if (productCodeC1TextBox.Text == "")
                    {
                        SetPartsCodeC1ComboBox("");
                        return;
                    }

                    return;
                }

                SetPartsCodeC1ComboBox(productCodeC1TextBox.Text);

                partsCodeC1ComboBox.Text = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コード　検証時
        /// </summary>
        private void partsCodeC1ComboBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stPartsCode == s.Text)
                {
                    return;
                }
                stPartsCode = s.Text;

                var isOk1 = ErrorCheckPartsCode();
                if (isOk1 == false)
                {
                    ActiveControl = (C1ComboBox)sender;
                    e.Cancel = true;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
                {
                    ActiveControl = (C1ComboBox)sender;
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
        /// 部品コード　選択後
        /// </summary>
        private void partsCodeC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var s = (C1ComboBox)sender;

                if (s.SelectedIndex < 0)
                {
                    return;
                }

                // controlListIIから対象のコントロールの情報を取得
                var SelectList = controlListII.Where(v => v.Control.Name == s.Name).ToList();
                var listCtr = SelectList[0].SubControl;

                // コンボボックスDataSourceをDataViewに変換
                var dv = (System.Data.DataView)((C1ComboBox)SelectList[0].Control).ItemsDataSource;
                if (dv == null)
                {
                    listCtr.Text = "";
                    return;
                }

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
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
                dv.RowFilter = "rowNo = '" + ((C1ComboBox)SelectList[0].Control).Text + "' ";
                listCtr.Text = dv.ToTable().Rows[0][1].ToString();

                partsCodeC1ComboBox.Text = dv.ToTable().Rows[0]["partsCode"].ToString().TrimEnd();
                partsNameC1TextBox.Text = dv.ToTable().Rows[0]["partsName"].ToString().TrimEnd();
                supCodeC1TextBox.Text = dv.ToTable().Rows[0]["supCode"].ToString().TrimEnd();
                supNameC1TextBox.Text = dv.ToTable().Rows[0]["supName"].ToString().TrimEnd();

                // 変更無ければ何もしない
                if (stPartsCode == partsCodeC1ComboBox.Text && stSupCode == supCodeC1TextBox.Text)
                {
                    return;
                }
                stPartsCode = partsCodeC1ComboBox.Text;
                stSupCode = supCodeC1TextBox.Text;

                var isOk1 = ErrorCheckPartsCode();
                if (isOk1 == false)
                {
                    ActiveControl = partsCodeC1ComboBox;
                    return;
                }

                var isOk2 = ErrorCheckSupCode();
                if (isOk2 == false)
                {
                    ActiveControl = supCodeC1TextBox;
                    return;
                }

                var isOk3 = ErrorCheckPartsCodeAndSupCode();
                if (isOk3 == false)
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
        /// 仕入先コード　検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1TextBox)sender;
                if (stSupCode == s.Text)
                {
                    return;
                }
                stSupCode = s.Text;

                var isOk1 = ErrorCheckSupCode();
                if (isOk1 == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
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
        /// 需番　検証時
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckJyuyoyosokuCode();
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
        /// 属性　検証後
        /// </summary>
        private void outCateC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stOutCate == s.Text)
                {
                    return;
                }
                stOutCate = s.Text;

                // クリア
                outCateNameC1TextBox.Text = "";
                mainDivisionC1ComboBox.Enabled = false;
                mainDivisionC1ComboBox.Label.Enabled = true;
                mainDivisionC1ComboBox.BackColor = Color.PeachPuff;
                mainDivisionC1ComboBox.SelectedIndex = -1;
                mainDivisionC1ComboBox.Text = "";
                mainDivisionNameC1TextBox.Text = "";
                subDivisionC1ComboBox.Enabled = false;
                subDivisionC1ComboBox.Label.Enabled = true;
                subDivisionC1ComboBox.BackColor = Color.PeachPuff;
                subDivisionC1ComboBox.SelectedIndex = -1;
                subDivisionC1ComboBox.Text = "";
                subDivisionNameC1TextBox.Text = "";
                SetSubDivisionC1ComboBox("");

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                if (s.Text == "9")
                {
                    mainDivisionC1ComboBox.Enabled = true;
                    mainDivisionC1ComboBox.BackColor = Color.White;
                    subDivisionC1ComboBox.Enabled = true;
                    subDivisionC1ComboBox.BackColor = Color.White;
                }

                if (mainDivisionC1ComboBox.Enabled) 
                {
                    ActiveControl = mainDivisionC1ComboBox;
                }
                else 
                {
                    ActiveControl = remarksC1TextBox;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 属性　選択後
        /// </summary>
        private void outCateC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stOutCate == s.Text)
                {
                    return;
                }
                stOutCate = s.Text;

                // クリア
                outCateNameC1TextBox.Text = "";
                mainDivisionC1ComboBox.Enabled = false;
                mainDivisionC1ComboBox.Label.Enabled = true;
                mainDivisionC1ComboBox.BackColor = Color.PeachPuff;
                mainDivisionC1ComboBox.SelectedIndex = -1;
                mainDivisionC1ComboBox.Text = "";
                mainDivisionNameC1TextBox.Text = "";
                subDivisionC1ComboBox.Enabled = false;
                subDivisionC1ComboBox.Label.Enabled = true;
                subDivisionC1ComboBox.BackColor = Color.PeachPuff;
                subDivisionC1ComboBox.SelectedIndex = -1;
                subDivisionC1ComboBox.Text = "";
                subDivisionNameC1TextBox.Text = "";
                SetSubDivisionC1ComboBox("");

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                if (s.Text == "9")
                {
                    mainDivisionC1ComboBox.Enabled = true;
                    mainDivisionC1ComboBox.BackColor = Color.White;
                    subDivisionC1ComboBox.Enabled = true;
                    subDivisionC1ComboBox.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 大分類　検証後
        /// </summary>
        private void mainDivisionC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stMainDivision == s.Text)
                {
                    return;
                }
                stMainDivision = s.Text;

                // クリア
                mainDivisionNameC1TextBox.Text = "";
                subDivisionC1ComboBox.SelectedIndex = -1;
                subDivisionC1ComboBox.Text = "";
                subDivisionNameC1TextBox.Text = "";
                SetSubDivisionC1ComboBox("");

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                SetSubDivisionC1ComboBox(s.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 大分類　選択後
        /// </summary>
        private void mainDivisionC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stMainDivision == s.Text)
                {
                    return;
                }
                stMainDivision = s.Text;

                // クリア
                mainDivisionNameC1TextBox.Text = "";
                subDivisionC1ComboBox.SelectedIndex = -1;
                subDivisionC1ComboBox.Text = "";
                subDivisionNameC1TextBox.Text = "";
                SetSubDivisionC1ComboBox("");

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                SetSubDivisionC1ComboBox(s.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 備考　検証時
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

        /// <summary>
        /// 納入指示数　検証時
        /// </summary>
        private void numC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckNum();
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
        /// 単価　検証時
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
        /// 加工単価　検証時
        /// </summary>
        private void processUnitPriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckProcessUnitPrice();
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
        /// 在庫P　検証後
        /// </summary>
        private void stockCateC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                DrawC1TrueDBGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 在庫P　選択後
        /// </summary>
        private void stockCateC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                DrawC1TrueDBGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 伝票番号　検証時
        /// </summary>
        private void doCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckDoCode();
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
        /// 伝票番号  チェックボックス
        /// </summary>
        private void doCodeC1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var s = (C1CheckBox)sender;
                if (s.Checked)
                {
                    doCodeC1TextBox.Enabled = true;
                    doCodeC1TextBox.BackColor = Color.White;
                    ActiveControl = doCodeC1TextBox;
                }
                else
                {
                    doCodeC1TextBox.Enabled = false;
                    doCodeC1TextBox.Label.Enabled = true;
                    doCodeC1TextBox.BackColor = Color.PeachPuff;
                    doCodeC1TextBox.Text = "";
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

            if (outCateC1ComboBox.Text == "9")
            {
                if (mainDivisionC1ComboBox.Text == "")
                {
                    ActiveControl = mainDivisionC1ComboBox;
                    ChangeTopMessage("W0007", mainDivisionC1ComboBox.Label.Text);
                    return false;
                }

                if (subDivisionC1ComboBox.Text == "")
                {
                    ActiveControl = subDivisionC1ComboBox;
                    ChangeTopMessage("W0007", subDivisionC1ComboBox.Label.Text);
                    return false;
                }
            }

            if (doCodeC1CheckBox.Checked && doCodeC1TextBox.Text == "")
            {
                ActiveControl = doCodeC1TextBox;
                ChangeTopMessage("W0007", doCodeC1TextBox.Label.Text);
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
            var startDate = DateTime.Parse(executeDateValueLabel.Text + "/01");
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var date = DateTime.Parse(dateC1DateEdit.Text);
            if (date < startDate || date > endDate)
            {
                ChangeTopMessage("W0016", "処理日付外");
                return false;
            }

            // エラーチェック 部品コード
            var isOk1 = ErrorCheckPartsCode();
            if (isOk1 == false)
            {
                ActiveControl = partsCodeC1ComboBox;
                return false;
            }

            // エラーチェック 仕入先コード
            var isOk2 = ErrorCheckSupCode();
            if (isOk2 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            // エラーチェック 部品コード＆仕入先コード
            var isOk3 = ErrorCheckPartsCodeAndSupCode();
            if (isOk3 == false)
            {
                return false;
            }

            // エラーチェック 需番
            var isOk4 = ErrorCheckJyuyoyosokuCode();
            if (isOk4 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
                return false;
            }

            // エラーチェック 備考
            var isOk5 = ErrorCheckRemarks();
            if (isOk5 == false)
            {
                ActiveControl = remarksC1TextBox;
                return false;
            }

            // エラーチェック 数量
            var isOk6 = ErrorCheckNum();
            if (isOk6 == false)
            {
                ActiveControl = numC1NumericEdit;
                return false;
            }

            // エラーチェック 単価
            var isOk7 = ErrorCheckUnitPrice();
            if (isOk7 == false)
            {
                ActiveControl = unitPriceC1NumericEdit;
                return false;
            }

            // エラーチェック 加工単価
            var isOk8 = ErrorCheckProcessUnitPrice();
            if (isOk8 == false)
            {
                ActiveControl = processUnitPriceC1NumericEdit;
                return false;
            }

            // エラーチェック 伝票番号
            var isOk9 = ErrorCheckDoCode();
            if (isOk9 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            // 数量
            if (decimal.Parse(numC1NumericEdit.Text) > 0m)
            {
                var dialog = MessageBox.Show("返品数量にプラスが入力されました、返品の取消ですか？",
                                                 "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    ActiveControl = numC1NumericEdit;
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

                // データ登録
                apiParam.RemoveAll();
                apiParam.Add("isEOM", new JValue(true));
                apiParam.Add("partsCode", new JValue(partsCodeC1ComboBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                apiParam.Add("outCate", new JValue(outCateC1ComboBox.Text));
                apiParam.Add("mainDivision", new JValue(mainDivisionC1ComboBox.Text));
                apiParam.Add("subDivision", new JValue(subDivisionC1ComboBox.Text));
                apiParam.Add("remarks", new JValue(remarksC1TextBox.Text));
                apiParam.Add("date", new JValue(DateTime.Parse(dateC1DateEdit.Text)));
                apiParam.Add("num", new JValue(decimal.Parse(numC1NumericEdit.Text)));
                apiParam.Add("unitPrice", new JValue(decimal.Parse(unitPriceC1NumericEdit.Text)));
                apiParam.Add("unitPriceCate", new JValue(unitPriceCateC1ComboBox.Text));
                apiParam.Add("processUnitPrice", new JValue(
                    decimal.Parse(processUnitPriceC1NumericEdit.Text == "" ? "0" : processUnitPriceC1NumericEdit.Text)));
                apiParam.Add("price", new JValue(decimal.Parse(priceC1NumericEdit.Text)));
                apiParam.Add("doCode", new JValue(doCodeC1CheckBox.Checked ? doCodeC1TextBox.Text : ""));
                apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.Text));
                apiParam.Add("transCate", new JValue(transCateC1ComboBox.Text));
                apiParam.Add("dataCate", new JValue(dataCateC1TextBox.Text));
                apiParam.Add("accountCode", new JValue(accountCodeC1TextBox.Text));
                apiParam.Add("groupCode", new JValue(LoginInfo.Instance.GroupCode));
                apiParam.Add("isDoCodeChecked", new JValue(doCodeC1CheckBox.Checked));
                var result1 = ApiCommonUpdate(apiUrl + "InsideAcceptanceCancelIOFileUpdate", apiParam);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result1.Msg);
                    return;
                }

                bool temp = doCodeC1CheckBox.Checked;

                // 画面クリア
                DisplayClear();
                if (temp)
                {
                    ChangeTopMessage("I0002", "内部受付返品");
                    return;
                }

                ChangeTopMessage(1, "INFO", "内部受付返品の更新が完了しました    伝票番号：" + result1.doCode);

                // レポート
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(LoginInfo.Instance.GroupCode));
                apiParam.Add("doCode", new JValue(result1.doCode));
                var result2 = ApiCommonGet(apiUrl + "GetReportData", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "レポートデータ取得時に");
                    return;
                }
                if (result2.Table != null && result2.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result2.Table);
                    if (v.IsOk == false)
                    {
                        return;
                    }

                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {
                        report.Load(EXE_DIRECTORY + @"\Reports\R005_InsideTransReturn.flxr", "R005_InsideTransReturn");

                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = reportConnectionString,
                            Recordset = v.Table
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

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

                // 伝票発行済処理
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(LoginInfo.Instance.GroupCode));
                apiParam.Add("doCode", new JValue(result1.doCode));
                var result3 = ApiCommonUpdate(apiUrl + "InsideAcceptanceCancelPrinted", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result3.Msg);
                    return;
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
        /// c1TrueDBGrid　描画
        /// </summary>
        private void DrawC1TrueDBGrid()
        {
            // クリア
            stockC1TrueDBGrid.SetDataBinding(null, "", true);
            stockEC1TrueDBGrid.SetDataBinding(null, "", true);

            if (partsCodeC1ComboBox.Text == "") 
            {
                return;
            }

            // 在庫マスタ 素材在庫マスタ
            if (stockCateC1ComboBox.Text == "Z") 
            {
                var param = new SansoBase.StockMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCodeC1ComboBox.Text));
                param.WhereColuList.Add((param.GroupCode, LoginInfo.Instance.GroupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "在庫マスタ検索時に");
                    return;
                }
                if (result.Table.Rows.Count >= 1)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = "在庫P";
                    dc.DefaultValue = "完成品在庫";
                    dc.DataType = typeof(string);
                    result.Table.Columns.Add(dc);
                    stockC1TrueDBGrid.SetDataBinding(result.Table, "", true);
                }

                var param2 = new SansoBase.StockMstE();
                param2.SelectStr = "*";
                param2.WhereColuList.Add((param2.PartsCode, partsCodeC1ComboBox.Text));
                param2.WhereColuList.Add((param2.GroupCode, LoginInfo.Instance.GroupCode));
                param2.SetDBName("製造調達");
                var result2 = CommonAF.ExecutSelectSQL(param2);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "月末在庫マスタ検索時に");
                    return;
                }
                if (result2.Table.Rows.Count >= 1)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = "在庫P";
                    dc.DefaultValue = "完成品在庫";
                    dc.DataType = typeof(string);
                    result2.Table.Columns.Add(dc);
                    stockEC1TrueDBGrid.SetDataBinding(result2.Table, "", true);
                }
            }
            else 
            {
                var param = new SansoBase.MaterialStockMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCodeC1ComboBox.Text));
                param.WhereColuList.Add((param.GroupCode, LoginInfo.Instance.GroupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "素材在庫マスタ検索時に");
                    return;
                }
                if (result.Table.Rows.Count >= 1)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = "在庫P";
                    dc.DefaultValue = "素材在庫";
                    dc.DataType = typeof(string);
                    result.Table.Columns.Add(dc);
                    stockC1TrueDBGrid.SetDataBinding(result.Table, "", true);
                }

                var param2 = new SansoBase.MaterialStockMstE();
                param2.SelectStr = "*";
                param2.WhereColuList.Add((param2.PartsCode, partsCodeC1ComboBox.Text));
                param2.WhereColuList.Add((param2.GroupCode, LoginInfo.Instance.GroupCode));
                param2.SetDBName("製造調達");
                var result2 = CommonAF.ExecutSelectSQL(param2);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "月末素材在庫マスタ検索時に");
                    return;
                }
                if (result2.Table.Rows.Count >= 1)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = "在庫P";
                    dc.DefaultValue = "素材在庫";
                    dc.DataType = typeof(string);
                    result2.Table.Columns.Add(dc);
                    stockEC1TrueDBGrid.SetDataBinding(result2.Table, "", true);
                }
            }
        }

        /// <summary>
        /// エラーチェック  機種コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckProductCode()
        {
            // 未入力時処理
            var s = productCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 機種マスタ
            var param = new SansoBase.ProductMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.ProductCode, s.Text));
            param.SetDBName("三相メイン");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "機種マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "機種コード", "機種マスタ");
                return false;
            }

            productNameC1TextBox.Text = result.Table.Rows[0]["機種名"].ToString();

            return true;
        }

        /// <summary>
        /// エラーチェック  機種名
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckProductName()
        {
            // 未入力時処理
            var s = productNameC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 機種マスタ
            var param = new SansoBase.ProductMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.ProductName, s.Text));
            param.SetDBName("三相メイン");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "機種マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "機種名", "機種マスタ");
                return false;
            }

            productCodeC1TextBox.Text = result.Table.Rows[0]["機種コード"].ToString();

            return true;
        }

        /// <summary>
        /// エラーチェック  部品コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPartsCode()
        {
            partsNameC1TextBox.Text = "";
            entryDateC1TextBox.Text = "";
            stockC1TrueDBGrid.SetDataBinding(null, "", true);
            stockEC1TrueDBGrid.SetDataBinding(null, "", true);

            // 未入力時処理
            var s = partsCodeC1ComboBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 部品マスタ
            var param = new SansoBase.PartsMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, s.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "部品マスタ");
                return false;
            }

            partsNameC1TextBox.Text = result.Table.Rows[0]["部品名"].ToString();
            entryDateC1TextBox.Text = DateTime.Today.ToShortDateString();

            // 在庫情報
            DrawC1TrueDBGrid();

            return true;
        }

        /// <summary>
        /// エラーチェック  仕入先コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSupCode()
        {
            supNameC1TextBox.Text = "";
            supCateC1TextBox.Text = "";
            dataCateC1TextBox.Text = "";
            accountCodeC1TextBox.Text = "";
            
            // 未入力時処理
            var s = supCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 仕入先マスタ
            var param = new SansoBase.SupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.SupCode, s.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return false;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "仕入先マスタ");
                return false;
            }

            var supCate = result.Table.Rows[0]["仕入先区分"].ToString();
            if (supCate != "K")
            {
                ChangeTopMessage("W0016", "社内コード以外");
                return false;
            }

            supNameC1TextBox.Text = result.Table.Rows[0]["仕入先名１"].ToString();
            supCateC1TextBox.Text = supCate;
            dataCateC1TextBox.Text = "2";
            accountCodeC1TextBox.Text = "";

            return true;
        }

        /// <summary>
        /// エラーチェック  部品コード＆仕入先コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPartsCodeAndSupCode()
        {
            processMstC1TextBox.Text = "";
            unitPriceMstC1TextBox.Text = "";

            // 未入力時処理
            if (partsCodeC1ComboBox.Text == "" || supCodeC1TextBox.Text == "")
            {
                return true;
            }

            // 工程マスタ
            var param1 = new SansoBase.ProcessMst();
            param1.SelectStr = "*";
            param1.WhereColuList.Add((param1.PartsCode, partsCodeC1ComboBox.Text));
            param1.WhereColuList.Add((param1.SupCode, supCodeC1TextBox.Text));
            param1.SetDBName("製造調達");
            var result1 = CommonAF.ExecutSelectSQL(param1);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "工程マスタ検索時に");
                return false;
            }
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                processMstC1TextBox.Text = "工程マスタなし";
            }
            else
            {
                processMstC1TextBox.Text = "工程マスタあり";
                stockCateC1ComboBox.Text = result1.Table.Rows[0]["在庫Ｐ"].ToString().TrimEnd();
                stockCateNameC1TextBox.Text = stockCateC1ComboBox.SGetText(1);
                transCateC1ComboBox.Text = result1.Table.Rows[0]["有償Ｐ"].ToString().TrimEnd();
                transCateNameC1TextBox.Text = (transCateC1ComboBox.Text == "" ? "" : transCateC1ComboBox.SGetText(1));
            }

            // 単価マスタ
            var param2 = new SansoBase.UnitPriceMst();
            param2.SelectStr = "*";
            param2.WhereColuList.Add((param2.PartsCode, partsCodeC1ComboBox.Text));
            param2.WhereColuList.Add((param2.SupCode, supCodeC1TextBox.Text));
            param2.SetDBName("製造調達");
            var result2 = CommonAF.ExecutSelectSQL(param2);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return false;
            }
            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                unitPriceMstC1TextBox.Text = "単価マスタなし";
            }
            else
            {
                unitPriceMstC1TextBox.Text = "単価マスタあり";
                unitPriceCateC1ComboBox.Text = result2.Table.Rows[0]["単価区分"].ToString().TrimEnd();
                unitPriceCateNameC1TextBox.Text = (unitPriceCateC1ComboBox.Text == "" ? "" : unitPriceCateC1ComboBox.SGetText(1));
                unitPriceC1NumericEdit.Value = result2.Table.Rows[0].Field<decimal?>("仕入単価") ?? 0m;
                processUnitPriceC1NumericEdit.Value = result2.Table.Rows[0].Field<decimal?>("加工費") ?? 0m;
                ErrorCheckPrice();
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  需番
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckJyuyoyosokuCode()
        {
            // 未入力時処理
            var s = jyuyoyosokuCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
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
            var isOk1 = Check.HasSQLBanChar(s.Text).Result;
            if (isOk1 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            var isOk2 = Check.IsByteRange(s.Text, 0, 50).Result;
            if (isOk2 == false)
            {
                ChangeTopMessage("W0009", "備考", "0", "50");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  数量
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckNum()
        {
            // 未入力チェック
            var s = numC1NumericEdit;
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
            if (value == 0m)
            {
                ChangeTopMessage("W0016", s.Label.Text + "にゼロ");
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 7, 0);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", s.Label.Text + "の" + chk2.Msg);
                return false;
            }

            // エラーチェック  金額
            var isOk = ErrorCheckPrice();
            if (isOk == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  単価
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckUnitPrice()
        {
            // 未入力チェック
            var s = unitPriceC1NumericEdit;
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
            if (value <= 0m)
            {
                ChangeTopMessage("W0006", s.Label.Text);
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 7, 2);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", s.Label.Text + "の" + chk2.Msg);
                return false;
            }

            // エラーチェック  金額
            var isOk = ErrorCheckPrice();
            if (isOk == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  金額
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPrice()
        {
            var price = decimal.Parse(string.IsNullOrEmpty(this.numC1NumericEdit.Text) ? "0" : this.numC1NumericEdit.Text)
                      * decimal.Parse(string.IsNullOrEmpty(this.unitPriceC1NumericEdit.Text) ? "0" : this.unitPriceC1NumericEdit.Text);

            //四捨五入
            price = (price > 0m) ? (((Int64)(System.Math.Abs(price) + 0.5m)) * 1m) : (((Int64)(System.Math.Abs(price) + 0.5m)) * (-1m));

            priceC1NumericEdit.Value = price;

            // 数値項目の桁数範囲チェック
            var isOk = Check.IsPointNumberRange(price, 11, 0).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0014", priceC1NumericEdit.Label.Text, "最大桁数の整数11桁小数0");
                return false;
            }
            return true;
        }

        /// <summary>
        /// エラーチェック  加工単価
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckProcessUnitPrice()
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

        /// <summary>
        /// エラーチェック  伝票番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDoCode()
        {
            // 未入力時処理
            var s = doCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk1 = Check.HasSQLBanChar(s.Text).Result;
            if (isOk1 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (s.Text.Length != 4)
            {
                ChangeTopMessage("W0019", s.Label.Text + "には4桁の");
                return false;
            }

            // 数値か
            var chk1 = Check.IsNumeric(s.Text);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", s.Label.Text + "には");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機種名設定
        /// </summary>
        /// <param name="table">設定用データテーブル</param>
        /// <returns>IsOk[true:設定成功　false：設定時にエラー発生]、Table：設定後のデータテーブル</returns>
        private (bool IsOk, DataTable Table) SetProductName(DataTable table)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                return (true, null);
            }

            foreach (DataRow v in table.Rows)
            {
                var j = v["jyuyoyosokuCode"].ToString().TrimEnd();

                if (j == "")
                {
                    // 部品構成表
                    apiParam.RemoveAll();
                    apiParam.Add("partsCode", new JValue(v["partsCode"].ToString().TrimEnd()));
                    var result = ApiCommonGet(apiUrl + "GetBOMMst", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "部品構成表検索時に");
                        return (false, null);
                    }
                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        continue;
                    }

                    v["productName"] = result.Table.Rows[0]["productName"].ToString();
                }
                else
                {
                    // 製造指令ファイル
                    apiParam.RemoveAll();
                    apiParam.Add("jyuyoyosokuCode", new JValue(j));
                    var result = ApiCommonGet(apiUrl + "GetManufactFile", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "製造指令ファイル検索時に");
                        return (false, null);
                    }
                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        continue;
                    }

                    v["productName"] = result.Table.Rows[0]["productName"].ToString();
                }
            }

            return (true, table);
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

            if ((bool)result["isOk"] == false)
            {
                return (false, 0, (string)(result["msg"]), "");
            }

            return (
                true,
                (int)(result["count"]),
                "",
                (string)(result["doCode"])
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