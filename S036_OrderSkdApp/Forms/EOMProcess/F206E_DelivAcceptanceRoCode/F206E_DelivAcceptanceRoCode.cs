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
    /// 月末処理　納入受付（注番）
    /// </summary>
    public partial class F206E_DelivAcceptanceRoCode : BaseForm
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
        private List<string> groupCodeList = new List<string>();

        /// <summary>
        /// 特別後工程コード
        /// </summary>
        private string specialNextProcessCode = "";

        /// <summary>
        /// 次工程エラー
        /// </summary>
        private string nextProcessError = "";

        /// <summary>
        /// 初品検査フラッグ
        /// </summary>
        private string firstProdCheckFlg = "";

        /// <summary>
        /// 最終判定結果
        /// </summary>
        private string finalResult = "";

        /// <summary>
        /// 管理番号
        /// </summary>
        private string manageCode = "";

        /// <summary>
        /// DTCheckクラス(排他制御)　発注マスタ
        /// </summary>
        DTCheck ordMstDTCheck = new DTCheck();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F206E_DelivAcceptanceRoCode(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "月末納入受付（注番）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F206E_DelivAcceptanceRoCode_Load(object sender, EventArgs e)
        {
            // 月末
            IsEOMTitleBackColor = true;

            // 左パネル
            AddControlListII(poCode1C1TextBox, null, "", false, enumCate.Key);
            AddControlListII(poCode2C1TextBox, null, "", false, enumCate.Key);
            AddControlListII(applicationCodeC1TextBox, null, "", false, enumCate.Key);

            AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", false, enumCate.Key);
            AddControlListII(partsNameC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(drawingCodeC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", false, enumCate.Key);
            AddControlListII(supNameC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(inNumC1NumericEdit, null, "", true, enumCate.Key);
            AddControlListII(unitPriceC1NumericEdit, null, "", true, enumCate.Key);
            AddControlListII(unitPriceCateC1ComboBox, unitPriceNameC1TextBox, "", true, enumCate.Key);
            AddControlListII(unitPriceNameC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(processUnitPriceC1NumericEdit, null, "", false, enumCate.Key);
            AddControlListII(inPriceC1NumericEdit, null, "", false, enumCate.Key);
            AddControlListII(inDateC1DateEdit, null, null, true, enumCate.Key);
            AddControlListII(doCodeC1TextBox, null, "", true, enumCate.Key);
            AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", false, enumCate.Key);
            AddControlListII(groupNameC1TextBox, null, "", false, enumCate.Key);

            // 右パネル
            AddControlListII(ordTotalNumC1NumericEdit, null, null, false, enumCate.Key);
            AddControlListII(delivTotalNumC1NumericEdit, null, null, false, enumCate.Key);
            AddControlListII(ordRemainNumC1NumericEdit, null, null, false, enumCate.Key);

            AddControlListII(dataCateC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(entryDateC1TextBox, null, null, false, enumCate.Key);
            AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", false, enumCate.Key);
            AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(transCateC1ComboBox, transCateC1TextBox, "", false, enumCate.Key);
            AddControlListII(transCateC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(delivStatusFlgC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(supOsrcCateC1TextBox, null, "", false, enumCate.Key);

            AddControlListII(nextProcessCodeC1TextBox, nextProcessNameC1TextBox, "", false, enumCate.Key);
            AddControlListII(nextProcessNameC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(nextProcessCateC1ComboBox, nextProcessCateC1TextBox, "", false, enumCate.Key);
            AddControlListII(nextProcessCateC1TextBox, null, "", false, enumCate.Key);
            AddControlListII(insideTransDateC1DateEdit, null, null, false, enumCate.Key);
            AddControlListII(transUnitPriceC1NumericEdit, null, null, false, enumCate.Key);
            AddControlListII(firstProdChkC1TextBox, null, "", false, enumCate.Key);

            AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.Key);
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
            SetNextProcessCateC1ComboBox();
            SetUnitPriceCateC1ComboBox();

            // DefaultButtomMessageをセット
            defButtomMessage = "必須項目入力後に実行（F10）を押してください。";

            // 課別コード、処理年月取得
            GetGroupCode();

            // クリア処理
            DisplayClear();

            poCode1C1TextBox.BorderColor = Color.Red;
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

        /// <summary>
        /// 次工程区分　コンボボックスセット
        /// </summary>
        private void SetNextProcessCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("8", "社内移行");
            dt.Rows.Add("6", "有償（外注）");
            dt.Rows.Add("1", "有償（一般）");

            ControlAF.SetC1ComboBox(nextProcessCateC1ComboBox, dt, nextProcessCateC1ComboBox.Width,
                nextProcessCateC1TextBox.Width, "ID", "NAME", false);
        }

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
        #endregion  ＜コンボボックス設定処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        /// <param name="cate">入力無し or ""：注文番号手打ち、入力有：注文番号バーコード入力 </param>
        private void DisplayClear(string cate = "")
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
            specialNextProcessCode = "";
            nextProcessError = "";
            manageCode = "";
            firstProdCheckFlg = "";
            finalResult = "";
            actionC1CheckBox.Checked = false;
            nextProcessC1CheckBox.Checked = false;

            stockCateC1ComboBox.SelectedIndex = -1;
            transCateC1ComboBox.SelectedIndex = -1;
            nextProcessCateC1ComboBox.SelectedIndex = -1;
            unitPriceCateC1ComboBox.SelectedIndex = -1;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            if (cate == "")
            {
                // 手入力
                barcodeC1CheckBox.Checked = false;
                poCode1C1TextBox.Enabled = true;
                poCode1C1TextBox.BackColor = SColDef;

                poCode2C1TextBox.Enabled = false;
                poCode2C1TextBox.BackColor = SColReadOnly;

                ActiveControl = panel1;
                ActiveControl = poCode1C1TextBox;
            }
            else
            {
                // バーコード入力
                barcodeC1CheckBox.Checked = true;
                poCode1C1TextBox.Enabled = false;
                poCode1C1TextBox.BackColor = SColReadOnly;

                poCode2C1TextBox.Enabled = true;
                poCode2C1TextBox.BackColor = SColDef;

                ActiveControl = panel1;
                ActiveControl = poCode2C1TextBox;
            }
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
        /// 注文番号 検証時
        /// </summary>
        private void poCode1C1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 注文番号チェック
                var t = (C1TextBox)sender;
                var isOk = PoCodeErrorCheck(t);
                if (isOk == false)
                {
                    ActiveControl = t;
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
        /// 注文番号 検証後
        /// </summary>
        private void poCode1C1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var t = poCode1C1TextBox;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                // 注文番号後処理
                PoCodeValidated(t);

                // 発注マスタ 排他チェック用データ確保
                var paramOrd = new OrdMst();
                paramOrd.SelectStr = "*";
                paramOrd.WhereColuList.Add((paramOrd.PoCode, t.Text));
                paramOrd.SetDBName("製造調達");
                var resultOrd = CommonAF.ExecutSelectSQL(paramOrd);
                var dtOrd = resultOrd.Table;
                if (resultOrd.IsOk == false)
                {
                    ChangeTopMessage("E0008", "発注マスタ検索時に");
                    ActiveControl = t;
                    return;
                }
                if (dtOrd.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "注文番号", "発注マスタ");
                    return;
                }
                ordMstDTCheck.SaveDT(dtOrd);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 注文番号(バーコード用) 検証時
        /// </summary>
        private void poCode2C1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var t = (C1TextBox)sender;

                var isOk1 = PoCode2ErrorCheck(t);
                if (isOk1 == false)
                {
                    ActiveControl = t;
                    e.Cancel = true;
                    return;
                }

                // 注文番号チェック
                var isOk2 = PoCodeErrorCheck(t);
                if (isOk2 == false)
                {
                    ActiveControl = t;
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
        /// 注文番号(バーコード用) 検証後
        /// </summary>
        private void poCode2C1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var t = poCode2C1TextBox;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                var max = t.Text.Length;
                doCodeC1TextBox.Text = t.Text.Substring(max - 4);

                poCode1C1TextBox.Text = poCode2C1TextBox.Text;

                // 注文番号後処理
                PoCodeValidated(t);

                // 発注マスタ 排他チェック用データ確保
                var paramOrd = new OrdMst();
                paramOrd.SelectStr = "*";
                paramOrd.WhereColuList.Add((paramOrd.PoCode, t.Text));
                paramOrd.SetDBName("製造調達");
                var resultOrd = CommonAF.ExecutSelectSQL(paramOrd);
                var dtOrd = resultOrd.Table;
                if (resultOrd.IsOk == false)
                {
                    ChangeTopMessage("E0008", "発注マスタ検索時に");
                    ActiveControl = t;
                    return;
                }
                if (dtOrd.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "注文番号", "発注マスタ");
                    return;
                }
                ordMstDTCheck.SaveDT(dtOrd);

                inNumC1NumericEdit.Value = ordTotalNumC1NumericEdit.Value;

                // 金額計算
                numPriceC1NumericEdit_Validated(inNumC1NumericEdit, new EventArgs());

                // 初品検査
                GetFirstProdChk();

                // 即実行チェックボックス確認
                if (actionC1CheckBox.Checked == true)
                {
                    // 実行前チェック
                    if (RequirF10() == false)
                    {
                        return;
                    }

                    if (ConsisF10() == false)
                    {
                        return;
                    }

                    if (ErrCKF10() == false)
                    {
                        return;
                    }

                    // 実行
                    ActionProc();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// バーコード入力チェック 変更後
        /// </summary>
        private void barcodeC1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (barcodeC1CheckBox.Checked == true)
                {
                    // バーコード入力の時
                    poCode1C1TextBox.Enabled = false;
                    poCode2C1TextBox.Enabled = true;
                    poCodeLabel.Enabled = true;
                    inNumC1NumericEdit.Enabled = false;
                    unitPriceC1NumericEdit.Enabled = false;
                    processUnitPriceC1NumericEdit.Enabled = false;
                    doCodeC1TextBox.Enabled = false;

                    poCode1C1TextBox.BackColor = SColReadOnly;
                    poCode1C1TextBox.BorderColor = Color.Gray;
                    poCode2C1TextBox.BackColor = SColDef;
                    poCode2C1TextBox.BorderColor = Color.Red;
                    inNumC1NumericEdit.BackColor = SColReadOnly;
                    unitPriceC1NumericEdit.BackColor = SColReadOnly;
                    processUnitPriceC1NumericEdit.BackColor = SColReadOnly;
                    doCodeC1TextBox.BackColor = SColReadOnly;

                    ActiveControl = poCode2C1TextBox;
                }
                else
                {
                    // 手入力の時
                    poCode1C1TextBox.Enabled = true;
                    poCode2C1TextBox.Enabled = false;
                    poCodeLabel.Enabled = true;
                    inNumC1NumericEdit.Enabled = true;
                    unitPriceC1NumericEdit.Enabled = true;
                    processUnitPriceC1NumericEdit.Enabled = true;
                    doCodeC1TextBox.Enabled = true;

                    poCode1C1TextBox.BackColor = SColDef;
                    poCode1C1TextBox.BorderColor = Color.Red;
                    poCode2C1TextBox.BackColor = SColReadOnly;
                    poCode2C1TextBox.BorderColor = Color.Gray;
                    inNumC1NumericEdit.BackColor = SColDef;
                    unitPriceC1NumericEdit.BackColor = SColDef;
                    processUnitPriceC1NumericEdit.BackColor = SColDef;
                    doCodeC1TextBox.BackColor = SColDef;

                    ActiveControl = poCode1C1TextBox;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 特採申請番号 検証時
        /// </summary>
        private void applicationCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ApplicationCodeErrorCheck();
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
        /// 納入単価 検証時
        /// </summary>
        /// <param name="sender"></param>
        private void unitPriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 納入単価チェック
                var n = (C1NumericEdit)sender;
                var isOk = UnitPriceErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = n;
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

        /// <summary>
        /// 社内移行日 検証時
        /// </summary>
        private void insideTransDateC1DateEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = InsideTransDateErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = (C1.Win.Calendar.C1DateEdit)sender;
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
        /// 次工程コード 検証時
        /// </summary>
        private void nextProcessCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = nextProcessCodeErrorCheck();
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
        /// 実行（F10）必須チェック
        /// </summary>
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        private bool RequirF10()
        {
            // 注文番号の必須チェック
            var t = poCode1C1TextBox;
            if (barcodeC1CheckBox.Checked == true)
            {
                t = poCode2C1TextBox;
            }
            if (string.IsNullOrEmpty(t.Text))
            {
                ActiveControl = t;
                ChangeTopMessage("W0007", t.Label.Text);
                return false;
            }

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
            var t1 = poCode1C1TextBox;
            if (barcodeC1CheckBox.Checked == true)
            {
                // 注文番号（バーコード用）
                t1 = poCode2C1TextBox;
                var isOk0 = PoCode2ErrorCheck(t1);

                if (isOk0 == false)
                {
                    ActiveControl = t1;
                    return false;
                }
            }

            var isOk1 = PoCodeErrorCheck(t1);
            if (isOk1 == false)
            {
                ActiveControl = t1;
                return false;
            }

            var isOk2 = ApplicationCodeErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = applicationCodeC1TextBox;
                return false;
            }

            var isOk3 = InNumErrorCheck();
            if (isOk3 == false)
            {
                ActiveControl = inNumC1NumericEdit;
                return false;
            }

            var isOk4 = UnitPriceErrorCheck();
            if (isOk4 == false)
            {
                ActiveControl = unitPriceC1NumericEdit;
                return false;
            }

            var isOk5 = InDateErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = inDateC1DateEdit;
                return false;
            }

            var isOk6 = DoCodeErrorCheck();
            if (isOk6 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            var isOk7 = InsideTransDateErrorCheck();
            if (isOk7 == false)
            {
                ActiveControl = insideTransDateC1DateEdit;
                return false;
            }

            var isOk8 = nextProcessCodeErrorCheck();
            if (isOk8 == false)
            {
                ActiveControl = nextProcessCodeC1TextBox;
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

            if (round != inPrice)
            {
                ChangeTopMessage(1, "WARN", "納入金額が納入数量 × 納入単価と異なります");
                return false;
            }

            // 発注マスタ 抽出
            var paramOrd = new OrdMst();
            paramOrd.SelectStr = "*";
            paramOrd.WhereColuList.Add((paramOrd.PoCode, t1.Text));
            paramOrd.SetDBName("製造調達");
            var resultOrd = CommonAF.ExecutSelectSQL(paramOrd);
            var dtOrd = resultOrd.Table;
            if (resultOrd.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注マスタ検索時に");
                ActiveControl = t1;
                return false;
            }
            if (dtOrd.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "注文番号", "発注マスタ");
                return false;
            }

            // 排他制御  発注マスタ
            if (ordMstDTCheck.CheckDT(dtOrd) == false)
            {
                ChangeTopMessage("E0004");
                return false;
            }

            // 次工程処理の確認
            if (groupCodeC1TextBox.Text == "3623")
            {
                nextProcessC1CheckBox.Checked = false;
            }
            if (nextProcessC1CheckBox.Checked == true)
            {
                if (MessageBox.Show("次工程の処理をしますか？", "処理の確認", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    // 「いいえ」の場合
                    MessageBox.Show($"次工程のチェックを外してください");
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

                // 発注マスタ、発注明細、入出庫ファイル、在庫マスタ、素材在庫マスタ更新
                var result1 = UpdateIOFileAPI();
                if (result1.IsOk == false)
                {
                    if (result1.ReLogin == true)
                    {
                        ShowMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", result1.Msg);
                        return;
                    }
                }

                // 発注明細 工事番号毎の納入完了判別
                var result2 = UpdateOrdDetail();
                if (result2.IsOk == false)
                {
                    if (result2.ReLogin == true)
                    {
                        ShowMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", result2.Msg);
                        return;
                    }
                }

                // 印刷処理
                var isOk = PrintProc();
                if (isOk == false)
                {
                    return;
                }

                // バーコードチェックボックスの内容によって画面クリアの引数変える
                if (barcodeC1CheckBox.Checked == true)
                {
                    // 注文番号（バーコード入力）
                    DisplayClear("1");
                }
                else
                {
                    // 注文番号（手入力）
                    DisplayClear();
                }

                ChangeTopMessage("I0009", "処理が");

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
        private bool PrintProc()
        {
            // マウスカーソル待機状態
            Cursor = Cursors.WaitCursor;

            if (nextProcessCodeC1TextBox.Text != "" && nextProcessC1CheckBox.Checked == true)
            {
                var printFlg = "";
                var reportName = "";

                switch (nextProcessCateC1ComboBox.Text)
                {
                    case "6":
                        printFlg = "1";
                        reportName = "R001_IONSupStockTrans";
                        break;

                    case "1":
                        printFlg = "1";
                        reportName = "R006_NSupStockTransDocu";
                        break;

                    case "8":
                        printFlg = "1";

                        if (nextProcessCodeC1TextBox.Text == "3630")
                        {
                            reportName = "R004_InsideTransPaint";
                        }
                        else
                        {
                            reportName = "R003_InsideTrans";
                        }
                        break;
                }

                if (printFlg == "1")
                {
                    // 入出庫ファイル更新、帳票データ抽出処理
                    var resultIO = UpdateIOFile();
                    if (resultIO.IsOk == false)
                    {
                        if (resultIO.ReLogin == true)
                        {
                            ShowMessageBox();
                            return false;
                        }
                        else
                        {
                            ChangeTopMessage(1, "WARN", resultIO.Msg);
                            return false;
                        }
                    }

                    if (resultIO.Table == null || resultIO.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0017", "有償支給/社内移行レポート");
                    }
                    else
                    {
                        // 機種名取得
                        var jyuyoyosokuCode = resultIO.Table.Rows[0]["jyuyoyosokuCode"].ToString() ?? "";
                        var productNameIO = GetProductName(jyuyoyosokuCode);

                        using (var report = new C1.Win.FlexReport.C1FlexReport())
                        {
                            report.Load(EXE_DIRECTORY + $@"\Reports\{reportName}.flxr", reportName);

                            // データソース設定
                            var ds = new C1.Win.FlexReport.DataSource
                            {
                                Name = " ",
                                ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                                Recordset = resultIO.Table
                            };
                            report.DataSources.Add(ds);
                            report.DataSourceName = ds.Name;

                            switch (nextProcessCateC1ComboBox.Text)
                            {
                                case "6":
                                    var visible = (supCodeC1TextBox.Text == "3730" ? true : false);
                                    var barCode = resultIO.Table.Rows[0]["バーコード"].ToString();
                                    ((C1.Win.FlexReport.Field)report.Fields["バーコード１"]).Text = barCode;
                                    ((C1.Win.FlexReport.Field)report.Fields["バーコード２"]).Text = barCode;
                                    ((C1.Win.FlexReport.Field)report.Fields["バーコード１"]).Visible = visible;
                                    ((C1.Win.FlexReport.Field)report.Fields["バーコード２"]).Visible = visible;
                                    ((C1.Win.FlexReport.Field)report.Fields["バーコード３"]).Visible = false;
                                    ((C1.Win.FlexReport.Field)report.Fields["機種名１"]).Text = productNameIO;
                                    ((C1.Win.FlexReport.Field)report.Fields["機種名２"]).Text = productNameIO;
                                    ((C1.Win.FlexReport.Field)report.Fields["機種名３"]).Text = productNameIO;
                                    break;

                                case "1":
                                    ((C1.Win.FlexReport.Field)report.Fields["productName1"]).Text = productNameIO;
                                    ((C1.Win.FlexReport.Field)report.Fields["productName2"]).Text = productNameIO;
                                    ((C1.Win.FlexReport.Field)report.Fields["productName3"]).Text = productNameIO;
                                    break;

                                case "8":
                                    if (nextProcessCodeC1TextBox.Text == "3630")
                                    {
                                        ((C1.Win.FlexReport.Field)report.Fields["productName1"]).Text = productNameIO;
                                        ((C1.Win.FlexReport.Field)report.Fields["productName2"]).Text = productNameIO;
                                        ((C1.Win.FlexReport.Field)report.Fields["productName3"]).Text = productNameIO;

                                        var denNo = resultIO.Table.Rows[0]["denNo"].ToString();
                                        ((C1.Win.FlexReport.Field)report.Fields["バーコード１"]).Text = denNo;
                                        ((C1.Win.FlexReport.Field)report.Fields["バーコード１"]).Visible = true;
                                    }
                                    else
                                    {
                                        ((C1.Win.FlexReport.Field)report.Fields["productName1"]).Text = productNameIO;
                                        ((C1.Win.FlexReport.Field)report.Fields["productName2"]).Text = productNameIO;
                                        ((C1.Win.FlexReport.Field)report.Fields["productName3"]).Text = productNameIO;
                                    }
                                    break;
                            }

                            // 即印刷
                            var p = new System.Drawing.Printing.PrinterSettings();
                            p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                            report.Render();
                            var print = PrintReport(report, false, p);
                            if (print.IsOk == false)
                            {
                                ChangeTopMessage("E0008", "有償支給/社内移行レポート 印刷処理で");
                                return false;
                            }
                        }
                    }
                }

                if (nextProcessError == "E")
                {
                    // Ｒ入出納入受付注番次工程ERR
                    // 画面から入力値設定

                    using (var report = new C1.Win.FlexReport.C1FlexReport())
                    {

                        report.Load(EXE_DIRECTORY + @"\Reports\R009_DelivAcceptanceRoCodeErr.flxr", "R009_DelivAcceptanceRoCodeErr");

                        DataTable dt = new DataTable();
                        dt.Columns.Add("発注部門", typeof(string));
                        dt.Columns.Add("注文番号", typeof(string));
                        dt.Columns.Add("仕入先コード", typeof(string));
                        dt.Columns.Add("仕入先名", typeof(string));
                        dt.Columns.Add("部品コード", typeof(string));
                        dt.Columns.Add("部品名", typeof(string));
                        dt.Columns.Add("納入指示数", typeof(string));
                        dt.Columns.Add("次工程コード", typeof(string));
                        dt.Columns.Add("次工程名", typeof(string));
                        dt.Rows.Add(groupCodeC1TextBox.Text, poCode1C1TextBox.Text, supCodeC1TextBox.Text, supNameC1TextBox.Text,
                            partsCodeC1TextBox.Text, partsNameC1TextBox.Text, inNumC1NumericEdit.Value.ToString(),
                            nextProcessCodeC1TextBox.Text, nextProcessNameC1TextBox.Text);


                        // データソース設定
                        var ds = new C1.Win.FlexReport.DataSource
                        {
                            Name = " ",
                            ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                            Recordset = dt
                        };
                        report.DataSources.Add(ds);
                        report.DataSourceName = ds.Name;

                        // 即印刷
                        var p = new System.Drawing.Printing.PrinterSettings();
                        p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                        report.Render();
                        var print = PrintReport(report, false, p);
                        if (print.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "入出納入受付注番次工程ERRレポート 印刷処理で");
                            return false;
                        }
                    }
                }
            }

            if (firstProdCheckFlg == "1")
            {
                var title = "";

                if (string.IsNullOrEmpty(finalResult) == true || finalResult.Substring(0, 1) != "G")
                {
                    //Ｒ初品検査未合格
                    title = "初品未合格";
                }
                else
                {
                    //Ｒ初品検査合格
                    title = "初品合格";
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("title", typeof(string));
                dt.Columns.Add("仕入先コード", typeof(string));
                dt.Columns.Add("仕入先名", typeof(string));
                dt.Columns.Add("部品コード", typeof(string));
                dt.Columns.Add("図面番号", typeof(string));
                dt.Columns.Add("部品名", typeof(string));
                dt.Columns.Add("管理番号", typeof(string));
                dt.Rows.Add(title, supCodeC1TextBox.Text, supNameC1TextBox.Text, partsCodeC1TextBox.Text, drawingCodeC1TextBox.Text,
                    partsNameC1TextBox.Text, manageCode);

                // レポート印刷
                using (var report = new C1.Win.FlexReport.C1FlexReport())
                {
                    report.Load(EXE_DIRECTORY + @"\Reports\R010_FirstProdCheckResult.flxr", "R010_FirstProdCheckResult");

                    // データソース設定
                    var ds = new C1.Win.FlexReport.DataSource
                    {
                        Name = " ",
                        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                        Recordset = dt
                    };
                    report.DataSources.Add(ds);
                    report.DataSourceName = ds.Name;

                    // 即印刷
                    var p = new System.Drawing.Printing.PrinterSettings();
                    p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                    report.Render();
                    var print = PrintReport(report, false, p);
                    if (print.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "初品検査レポート 印刷処理で");
                        return false;
                    }
                }
            }

            // Ｒ次工程確認シート
            if (string.IsNullOrEmpty(specialNextProcessCode))
            {
                return true;
            }

            // 工程マスタ取得処理
            var result1 = GetProcessMst();
            if (result1.IsOk == false)
            {
                if (result1.ReLogin == true)
                {
                    ShowMessageBox();
                    return false;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", result1.Msg);
                    return false;
                }
            }

            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                return true;
            }

            // 機種名取得処理
            var result2 = GetBOMMst();
            if (result2.IsOk == false)
            {
                if (result2.ReLogin == true)
                {
                    ShowMessageBox();
                    return false;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", result2.Msg);
                    return false;
                }
            }

            var productName = "";
            if (result2.Table != null && result2.Table.Rows.Count >= 1)
            {
                productName = result2.Table.Rows[0]["productName"].ToString();
            }

            // レポート印刷
            using (var report = new C1.Win.FlexReport.C1FlexReport())
            {
                report.Load(EXE_DIRECTORY + @"\Reports\R011_NextProcessReport.flxr", "R011_NextProcessReport");

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                    Recordset = result1.Table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // 個別設定            
                ((C1.Win.FlexReport.Field)report.Fields["数量"]).Text = inNumC1NumericEdit.Value.ToString();
                ((C1.Win.FlexReport.Field)report.Fields["課別コード"]).Text = groupCodeC1TextBox.Text;
                ((C1.Win.FlexReport.Field)report.Fields["機種名"]).Text = productName;

                // 即印刷
                var p = new System.Drawing.Printing.PrinterSettings();
                p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["OutSideTrans"];
                report.Render();
                var print = PrintReport(report, false, p);
                if (print.IsOk == false)
                {
                    ChangeTopMessage("E0008", "初品検査レポート 印刷処理で");
                    return false;
                }
            }
            return true;
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

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
                    ShowMessageBox();
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

            groupCodeList = result1.Table.AsEnumerable().Select(v => v["groupCode"].ToString()).ToList();
            var groupCode = groupCodeList[0] ?? "";
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
            else
            {
                var isOk = DateTime.TryParse(result.Table.Rows[0].Field<string>("処理年月"), out DateTime date);
                if(isOk == false)
                {
                    ChangeTopMessage("W0002", "処理年月", "部門マスタ");
                    return;
                }

                startDate = date.AddMonths(-1);
                endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);
                executeDateValueLabel.Text = startDate.ToString("yyyy/MM");
                executeDateValueLabel.Visible = true;
                executeDateLabel.Visible = true;
            }
        }


        /// <summary>
        /// 注文番号チェック
        /// </summary>
        /// <param name="t">注文番号</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PoCodeErrorCheck(C1TextBox t)
        {
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

            // 発注マスタ 抽出
            var paramOrd = new OrdMst();
            paramOrd.SelectStr = "*";
            paramOrd.WhereColuList.Add((paramOrd.PoCode, t.Text));
            paramOrd.SetDBName("製造調達");
            var resultOrd = CommonAF.ExecutSelectSQL(paramOrd);
            var dtOrd = resultOrd.Table;
            if (resultOrd.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注マスタ検索時に");
                ActiveControl = t;
                return false;
            }
            if (dtOrd.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "注文番号", "発注マスタ");
                return false;
            }

            partsCodeC1TextBox.Text = dtOrd.Rows[0].Field<string>("部品コード") ?? "";
            supCodeC1TextBox.Text = dtOrd.Rows[0].Field<string>("仕入先コード") ?? "";

            // 発注マスタから取得した部品コードの確認
            var paramParts = new PartsMst();
            paramParts.SelectStr = "*";
            paramParts.WhereColuList.Add((paramParts.PartsCode, partsCodeC1TextBox.Text));
            paramParts.SetDBName("製造調達");
            var resultParts = CommonAF.ExecutSelectSQL(paramParts);
            var dtParts = resultParts.Table;
            if (resultParts.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                ActiveControl = t;
                return false;
            }
            if (dtParts.Rows.Count <= 0)
            {
                partsNameC1TextBox.Text = "部品マスタ未登録";
            }

            partsNameC1TextBox.Text = dtParts.Rows[0].Field<string>("部品名") ?? "";

            // 発注マスタから取得した仕入先コード確認
            var paramSup = new SupMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.SupCode, supCodeC1TextBox.Text));
            paramSup.SetDBName("製造調達");
            var resultSup = CommonAF.ExecutSelectSQL(paramSup);
            var dtSup = resultSup.Table;
            if (resultSup.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                ActiveControl = t;
                return false;
            }
            if (dtSup.Rows.Count <= 0)
            {
                supNameC1TextBox.Text = "仕入先マスタ未登録";
                supOsrcCateC1TextBox.Text = "";
            }

            supNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? "";
            supOsrcCateC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先区分") ?? "";

            // 住所録マスタ取得
            var resultAddress = SelectDBAF.GetSansoMainAddressMst(supCodeC1TextBox.Text);
            if (resultAddress.IsOk == false)
            {
                ChangeTopMessage("E0008", "住所録マスタ検索時に");
                return false;
            }

            outDataCateC1TextBox.Text = "";
            entryDateC1TextBox.Text = DateTime.Today.ToShortDateString();
            delivStatusFlgC1TextBox.Text = dtOrd.Rows[0].Field<string>("納入完了フラッグ") ?? "";

            var groupCode = dtOrd.Rows[0].Field<string>("発注部門") ?? "";
            if (groupCodeList.Contains(groupCode) == false)
            {
                ChangeTopMessage("W0021", "ユーザの所属と発注データの部門");
                return false;
            }

            // 住所録マスタ
            if (resultAddress.Table.Rows.Count <= 0)
            {
                var temp = dtSup.AsEnumerable().Where(v => v["仕入先区分"].ToString() == "K");
                if (temp.Count() <= 0)
                {
                    ChangeTopMessage("W0002", "課別コードが住所録マスタ、部門マスタ");
                    return false;
                }
                supOsrcCateC1TextBox.Text = "K";
            }
            else
            {
                var stopCate = resultAddress.Table.Rows[0].Field<string>("取引停止区分").Substring(1, 1) ?? "";
                if (stopCate == "D" || stopCate == "K")
                {
                    ChangeTopMessage("W0002", "課別コードが住所録マスタ、部門マスタ");
                    return false;
                }
            }

            // 初品検査メッセージ 表示
            firstProdChkC1TextBox.Text = "";
            var result = GetFirstProdCheckOK();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowMessageBox();
                    return false;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", result.Msg);
                    return false;
                }
            }

            if (result.Table != null && result.Table.Rows.Count >= 1)
            {
                var firstJudgement = result.Table.Rows[0]["firstJudgement"].ToString() ?? "";
                var totalJudgement = result.Table.Rows[0]["totalJudgement"].ToString() ?? "";

                if (totalJudgement == "ＯＫ")
                {
                    firstProdChkC1TextBox.Text = "部品検査　総合判定ＯＫ";
                    return true;
                }

                if (firstJudgement == "ＯＫ")
                {
                    firstProdChkC1TextBox.Text = "部品検査　一次判定ＯＫ";
                    return true;
                }

                if (totalJudgement == "中止")
                {
                    firstProdChkC1TextBox.Text = "部品検査　総合判定中止";
                    return true;
                }

                var temp = dtOrd.Rows[0].Field<string>("受入検査フラッグ") ?? "";
                if (temp == "1")
                {
                    firstProdChkC1TextBox.Text = "部品検査が終了していません";

                    if (applicationCodeC1TextBox.Text == "")
                    {
                        ChangeTopMessage("W0016", "特採申請番号がない場合");
                        return false;
                    }

                    firstProdChkC1TextBox.Text = "部品検査　特採申請";
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// 注文番号（バーコード用） エラーチェック
        /// </summary>
        /// <param name="t">注文番号</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PoCode2ErrorCheck(C1TextBox t)
        {
            if (string.IsNullOrEmpty(t.Text) == true)
            {
                return true;
            }

            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (t.Text.Contains("-"))
            {
                ChangeTopMessage(1, "WARN", "この伝票はWeb-EDIの伝票です。「納入受付(伝票番号)」画面で処理してください。");
                return false;
            }

            t.Text = t.Text.Substring(0, 10);

            return true;
        }

        /// <summary>
        /// 注文番号検証後 共通処理
        /// </summary>
        /// <param name="t"></param>
        private void PoCodeValidated(C1TextBox t)
        {

            // 発注マスタ 抽出
            var paramOrd = new OrdMst();
            paramOrd.SelectStr = "*";
            paramOrd.WhereColuList.Add((paramOrd.PoCode, t.Text));
            paramOrd.SetDBName("製造調達");
            var resultOrd = CommonAF.ExecutSelectSQL(paramOrd);
            var dtOrd = resultOrd.Table;
            if (resultOrd.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注マスタ検索時に");
                ActiveControl = t;
                return;
            }
            if (dtOrd.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "注文番号", "発注マスタ");
                return;
            }

            partsCodeC1TextBox.Text = dtOrd.Rows[0].Field<string>("部品コード") ?? "";
            supCodeC1TextBox.Text = dtOrd.Rows[0].Field<string>("仕入先コード") ?? "";

            // 発注マスタから取得した部品コードの確認
            var paramParts = new PartsMst();
            paramParts.SelectStr = "*";
            paramParts.WhereColuList.Add((paramParts.PartsCode, partsCodeC1TextBox.Text));
            paramParts.SetDBName("製造調達");
            var resultParts = CommonAF.ExecutSelectSQL(paramParts);
            var dtParts = resultParts.Table;
            if (resultParts.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                ActiveControl = t;
                return;
            }
            if (dtParts.Rows.Count <= 0)
            {
                partsNameC1TextBox.Text = "部品マスタ未登録";
            }

            partsNameC1TextBox.Text = dtParts.Rows[0].Field<string>("部品名") ?? "";
            drawingCodeC1TextBox.Text = dtParts.Rows[0].Field<string>("図面番号") ?? "";

            // 発注マスタから取得した仕入先コードで仕入先マスタ取得
            var paramSup = new SupMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.SupCode, supCodeC1TextBox.Text));
            paramSup.SetDBName("製造調達");
            var resultSup = CommonAF.ExecutSelectSQL(paramSup);
            var dtSup = resultSup.Table;
            if (resultSup.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                ActiveControl = t;
                return;
            }
            if (dtSup.Rows.Count <= 0)
            {
                supNameC1TextBox.Text = "仕入先マスタ未登録";
                supOsrcCateC1TextBox.Text = "";
            }

            supNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? "";
            supOsrcCateC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先区分") ?? "";

            // 発注情報セット
            var num1 = dtOrd.Rows[0]["納入指示数"].ToString();
            var num2 = dtOrd.Rows[0]["納入数"].ToString();
            double d1 = num1 == "" ? 0 : System.Convert.ToDouble(num1);
            double d2 = num2 == "" ? 0 : System.Convert.ToDouble(num2);
            double ordRemainNum = d1 - d2;
            ordTotalNumC1NumericEdit.Value = num1;
            delivTotalNumC1NumericEdit.Value = num2;
            ordRemainNumC1NumericEdit.Value = ordRemainNum.ToString();

            groupCodeC1TextBox.Text = dtOrd.Rows[0].Field<string>("発注部門") ?? "";
            unitPriceCateC1ComboBox.Text = dtOrd.Rows[0].Field<string>("単価区分") ?? "";
            ComboBoxValidated(unitPriceCateC1ComboBox, new EventArgs());
            unitPriceC1NumericEdit.Value = dtOrd.Rows[0]["仕入単価"].ToString() ?? "0";
            processUnitPriceC1NumericEdit.Value = dtOrd.Rows[0]["加工単価"].ToString() ?? "0";

            // 住所録マスタ取得
            var resultAddress = SelectDBAF.GetSansoMainAddressMst(supCodeC1TextBox.Text);
            if (resultAddress.IsOk == false)
            {
                ChangeTopMessage("E0008", "住所録マスタ検索時に");
                return;
            }

            // 仕入外注区分 判定
            switch (supOsrcCateC1TextBox.Text)
            {
                case "G": // 外注仕入先
                    dataCateC1TextBox.Text = "3";
                    accountCodeC1TextBox.Text = "083";
                    break;

                case "S": // 一般仕入先
                    dataCateC1TextBox.Text = "1";

                    // 住所録マスタ
                    if (resultAddress.Table.Rows.Count >= 1)
                    {
                        var temp = resultAddress.Table.Rows[0].Field<string>("仕外区分詳細") ?? "";
                        accountCodeC1TextBox.Text = temp == "N" ? "062" : "060";
                    }
                    break;

                case "T":
                    dataCateC1TextBox.Text = "1";
                    accountCodeC1TextBox.Text = "060";
                    break;

                case "K": // 社内部門
                    dataCateC1TextBox.Text = "2";
                    accountCodeC1TextBox.Text = "";
                    break;

                default: break;
            }

            inDateC1DateEdit.Value = endDate.ToString();
            outDataCateC1TextBox.Text = "";
            entryDateC1TextBox.Text = DateTime.Now.ToShortDateString();
            delivStatusFlgC1TextBox.Text = dtOrd.Rows[0].Field<string>("納入完了フラッグ") ?? "";

            var groupCode = groupCodeList[0] ?? "";
            if (groupCode == "3623")
            {
                // 在庫P = Z にする
                stockCateC1ComboBox.SelectedIndex = 1;
            }
            else
            {
                stockCateC1ComboBox.Text = dtOrd.Rows[0].Field<string>("在庫P") ?? "";
            }

            // 初品検査
            GetFirstProdChk();

            // 工程マスタ
            var paramProcess = new ProcessMst();
            paramProcess.SelectStr = "*";
            paramProcess.WhereColuList.Add((paramProcess.PartsCode, partsCodeC1TextBox.Text));
            paramProcess.WhereColuList.Add((paramProcess.SupCode, supCodeC1TextBox.Text));
            paramProcess.SetDBName("製造調達");
            var resultProcess = CommonAF.ExecutSelectSQL(paramProcess);
            var dtProcess = resultProcess.Table;
            if (resultProcess.IsOk == false)
            {
                ChangeTopMessage("E0008", "工程マスタ検索時に");
                ActiveControl = t;
                return;
            }
            if (dtProcess.Rows.Count <= 0)
            {
                transCateC1ComboBox.Text = "";
            }

            specialNextProcessCode = dtProcess.Rows[0].Field<string>("特別後工程コード") ?? "";
            transCateC1ComboBox.Text = dtProcess.Rows[0].Field<string>("有償P") ?? "";
            ComboBoxValidated(transCateC1ComboBox, new EventArgs());

            // 部門マスタ
            var paramGroup = new GroupMst();
            paramGroup.SelectStr = "*";
            paramGroup.WhereColuList.Add((paramGroup.GroupCode, groupCodeC1TextBox.Text));
            paramGroup.SetDBName("製造調達");
            var resultGroup = CommonAF.ExecutSelectSQL(paramGroup);
            var dtGroup = resultGroup.Table;
            if (resultGroup.IsOk == false)
            {
                ChangeTopMessage("E0008", "部門マスタ検索時に");
                ActiveControl = t;
                return;
            }
            if (dtGroup.Rows.Count <= 0)
            {
                groupNameC1TextBox.Text = "部門マスタ未登録";
                ChangeTopMessage("W0002", "課別コード", "部門マスタ");
            }
            groupNameC1TextBox.Text = dtGroup.Rows[0]["部門名"].ToString() ?? "";

            // 在庫情報設定
            SetStockInfo();

            // 次工程情報
            GetNextProcessInfo();

            // 部品検査チェック
            PartsCheck();

        }

        /// <summary>
        /// 特採申請番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ApplicationCodeErrorCheck()
        {
            var t = applicationCodeC1TextBox;
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

            // 10桁のみ
            var result = Check.IsStringRange(t.Text, 10, 10);
            if (result.Result == false)
            {
                ChangeTopMessage("W0003", t.Label.Text,"10", "10");
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

            decimal.TryParse(ordRemainNumC1NumericEdit.Value.ToString(), out decimal num2);
            if (num > num2)
            {
                ChangeTopMessage("W0014", "納入数量", "注文残数");
                return false;
            }

            if (n.Text == "0")
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

            decimal.TryParse(unitPriceC1NumericEdit.Text, out decimal unitPrice);
            if (unitPrice <= 0)
            {
                ChangeTopMessage("W0016", "単価に0以下");
                return false;
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
        /// 社内移行日チェック
        /// </summary>
        /// <returns>true：チェックＯＫ、false：チェックＮＧ</returns>
        private bool InsideTransDateErrorCheck()
        {
            // 次工程コードが未入力 or 次工程チェックが入っていない場合
            if (nextProcessCodeC1TextBox.Text == "" || nextProcessC1CheckBox.Checked == false)
            {
                return true;
            }

            var d = insideTransDateC1DateEdit;

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

            DateTime.TryParse(inDateC1DateEdit.Text, out var inDate);
            if (date < inDate)
            {
                ChangeTopMessage("W0016", "納入日付より前の日付");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 次工程コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool nextProcessCodeErrorCheck()
        {
            var t = nextProcessCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                nextProcessNameC1TextBox.Text = "";
                return true;
            }

            var isOk1 = Check.HasBanChar(t.Text).Result;
            if (isOk1 == false)
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
                nextProcessNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? "";
            }
            else
            {
                nextProcessNameC1TextBox.Text = "仕入先マスタ未登録";
            }

            return true;
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
            url += $"Solution/S036/F206E/GetGroupComboListByUser?sid={solutionIdShort}&fid={formIdShort}";

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
        /// 品質管理.dbo.Ｖ初品検査ＯＫ判別　取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetFirstProdCheckOK()
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetFirstProdCheckOK?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("partsCode", new JValue(partsCodeC1TextBox.Text ?? ""));
            param.Add("supCode", new JValue(supCodeC1TextBox.Text ?? ""));

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
        /// 在庫情報設定
        /// </summary>
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

            // 当月にセット
            prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
            thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
            thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
            thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");

            // 月末にセット
            prevMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
            thisMonthInNumEOMC1TextBox.Text = resultE.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
            thisMonthOutNumEOMC1TextBox.Text = resultE.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
            thisMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
        }

        /// <summary>
        /// ログイン有効期限切れメッセージ表示
        /// </summary>
        private void ShowMessageBox()
        {
            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                     $"再度、処理を実行してください",
                     "ログイン有効期限切れエラー",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
        }


        /// <summary>
        /// 初品検査取得
        /// </summary>
        private void GetFirstProdChk()
        {
            firstProdCheckFlg = "";
            finalResult = "";
            manageCode = "";

            // 初回ロット以外確認
            var result1 = FirstProdCheckApi(false);
            if (result1.IsOk == false)
            {
                if (result1.ReLogin == true)
                {
                    ShowMessageBox();
                }
                else
                {
                    ChangeTopMessage("E0008", "初品検査マスタ取得時に");
                    return;
                }
            }

            if (result1.Count >= 1)
            {
                firstProdCheckFlg = result1.Table.Rows[0]["firstProdCheckF"].ToString() ?? "";

                if (firstProdCheckFlg == "1")
                {
                    firstProdChkC1TextBox.Text = "初品検査対象部品";
                    finalResult = result1.Table.Rows[0]["finalResult"].ToString() ?? "";
                    manageCode = result1.Table.Rows[0]["manageCode"].ToString() ?? "";
                }
            }

            // 初回ロット確認
            var result2 = FirstProdCheckApi(true);
            if (result2.IsOk == false)
            {
                if (result2.ReLogin == true)
                {
                    ShowMessageBox();
                }
                else
                {
                    ChangeTopMessage("E0008", "初品検査マスタ取得時に");
                    return;
                }
            }

            if (result2.Count >= 1)
            {
                firstProdCheckFlg = result2.Table.Rows[0]["firstProdCheckFlg"].ToString() ?? "";

                if (firstProdCheckFlg == "1")
                {
                    firstProdChkC1TextBox.Text = "初回ロット検査対象部品";
                    finalResult = result2.Table.Rows[0]["finalResult"].ToString() ?? "";
                    manageCode = result2.Table.Rows[0]["manageCode"].ToString() ?? "";
                }
            }
        }

        /// <summary>
        /// 初品検査取得WebAPI呼び出し
        /// </summary>
        /// <param name="lot">true:初回ロットを検索 false:初回ロット以外を検索</param>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Count：取得件数
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, int Count, DataTable Table) FirstProdCheckApi(bool lot)
        {
            // 必要なパラメータ設定            
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");
            var supCode = controlListII.SGetText("supCodeC1TextBox");
            var date = controlListII.SGetText("inDateC1DateEdit");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetFirstProdCheckMst?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("date", new JValue(date));
            param.Add("lot", new JValue(lot));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, 0, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, 0, null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, 0, null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, (int)result["count"], table);
        }

        /// <summary>
        /// 部品検査確認
        /// </summary>
        private void PartsCheck()
        {
            var result = PartsCheckApi();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowMessageBox();
                }
                else
                {
                    ChangeTopMessage("E0008", "部品検査確認時に");
                    return;
                }
            }

            if (result.Count >= 1)
            {
                ChangeTopMessage(1, "WARN", "この注文番号は部品検査が必要です。※処理は進みます。");
            }
        }

        /// <summary>
        /// 部品検査確認WebAPI呼び出し
        /// </summary>
        private (bool IsOk, bool ReLogin, int Count) PartsCheckApi()
        {
            // 必要なパラメータ設定
            var poCode = controlListII.SGetText("poCode1C1TextBox");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetPartsCheck?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("poCode", new JValue(poCode));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, 0);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, 0);
            }

            return (true, false, (int)result["count"]);
        }

        /// <summary>
        /// 次工程情報取得
        /// </summary>
        private void GetNextProcessInfo()
        {
            var result = GetNextProcessInfoApi();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowMessageBox();
                }
                else
                {
                    ChangeTopMessage("E0008", "次工程情報取得時に");
                    return;
                }
            }

            var nextProcessCode = "";
            if (result.Table != null && result.Table.Rows.Count >= 1)
            {
                nextProcessCode = result.Table.Rows[0]["nextProcessCode"].ToString() ?? "";
            }

            if (nextProcessCode == "" || nextProcessCode == supCodeC1TextBox.Text)
            {
                nextProcessCodeC1TextBox.Text = "";
                nextProcessNameC1TextBox.Text = "";
                nextProcessCateC1ComboBox.Text = "";
                nextProcessC1CheckBox.Checked = false;
                transUnitPriceC1NumericEdit.Text = "0";
                return;
            }
            nextProcessCodeC1TextBox.Text = nextProcessCode;

            // 仕入先マスタ取得
            var paramSup = new SupMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.SupCode, nextProcessCode));
            paramSup.SetDBName("製造調達");
            var resultSup = CommonAF.ExecutSelectSQL(paramSup);
            var dtSup = resultSup.Table;
            if (resultSup.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return;
            }
            if (dtSup.Rows.Count >= 1)
            {
                nextProcessNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? "";

                var supCate = dtSup.Rows[0].Field<string>("仕入先区分") ?? "";
                switch (supCate)
                {
                    case "K":
                        nextProcessCateC1ComboBox.SelectedIndex = 0; // 8 社内移行
                        break;

                    case "G":
                        nextProcessCateC1ComboBox.SelectedIndex = 1; // 6 有償（外注）
                        break;

                    case "S":
                        nextProcessCateC1ComboBox.SelectedIndex = 2; // 1 有償（一般）
                        break;

                    default:
                        nextProcessC1CheckBox.Checked = false;
                        break;
                }
            }

            insideTransDateC1DateEdit.Value = endDate.ToString();

            // 単価マスタ取得
            var paramPrice = new UnitPriceMst();
            paramPrice.SelectStr = "*";
            paramPrice.WhereColuList.Add((paramPrice.PartsCode, partsCodeC1TextBox.Text));
            paramPrice.WhereColuList.Add((paramPrice.SupCode, nextProcessCode));
            paramPrice.SetDBName("製造調達");
            var resultPrice = CommonAF.ExecutSelectSQL(paramPrice);
            var dtPrice = resultPrice.Table;
            if (resultPrice.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return;
            }
            if (dtPrice.Rows.Count <= 0)
            {
                nextProcessC1CheckBox.Checked = false;
                transUnitPriceC1NumericEdit.Text = "0";
                nextProcessError = "E";
            }
            else
            {
                nextProcessC1CheckBox.Checked = true;
                transUnitPriceC1NumericEdit.Value = dtPrice.Rows[0]["支給単価"].ToString();

                var unitPrice = decimal.Parse(transUnitPriceC1NumericEdit.Value.ToString());
                if (unitPrice <= 0)
                {
                    nextProcessError = "E";
                }
            }
        }

        /// <summary>
        /// 次工程情報取得WebAPI呼び出し
        /// </summary>
        private (bool IsOk, bool ReLogin, DataTable Table) GetNextProcessInfoApi()
        {
            // 必要なパラメータ設定
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");
            var supCode = controlListII.SGetText("supCodeC1TextBox");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetNextProcessInfo?sid={solutionIdShort}&fid={formIdShort}";

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
                        return (false, true, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, table);
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
            var poCode = barcodeC1CheckBox.Checked == true ?
                            controlListII.SGetText("poCode2C1TextBox") : controlListII.SGetText("poCode1C1TextBox");
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
            var batchProcessFlg = barcodeC1CheckBox.Checked == true ? "A" : "";
            var createDate = DateTime.Now.ToString();
            var applicationCode = controlListII.SGetText("applicationCodeC1TextBox") ?? "";
            var remarks = applicationCode == "" ? null : $"特採申請番号:{applicationCode}";
            var den_NO = "";
            var createStaffCode = LoginInfo.Instance.UserNo;
            var createID = LoginInfo.Instance.UserId;
            var nextProcessCode = controlListII.SGetText("nextProcessCodeC1TextBox");
            var nextProcessCheck = nextProcessC1CheckBox.Checked;
            var nextProcessCate = controlListII.SGetText("nextProcessCateC1ComboBox");
            var transUnitPrice = controlListII.SGetText("transUnitPriceC1NumericEdit").Replace(",", "");
            var insideTransDate = controlListII.SGetText("insideTransDateC1DateEdit");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += "IOFile/CreateDelivAcceptance";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("dataCate", new JValue(dataCate));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("poCode", new JValue(poCode));
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
            param.Add("batchProcessFlg", new JValue(batchProcessFlg));
            param.Add("createDate", new JValue(createDate));
            param.Add("remarks", new JValue(remarks));
            param.Add("den_NO", new JValue(den_NO));
            param.Add("createStaffCode", new JValue(createStaffCode));
            param.Add("createID", new JValue(createID));
            param.Add("isEOM", new JValue(true));
            param.Add("nextProcessCode", new JValue(nextProcessCode));
            param.Add("nextProcessCheck", new JValue(nextProcessCheck));
            param.Add("nextProcessCate", new JValue(nextProcessCate));
            param.Add("transUnitPrice", new JValue(transUnitPrice));
            param.Add("insideTransDate", new JValue(insideTransDate));
            param.Add("num", new JValue(inNum));

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

        /// <summary>
        /// 発注明細 工事番号毎の納入完了判別
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg) UpdateOrdDetail()
        {

            // 必要なパラメータ設定済
            var poCode = barcodeC1CheckBox.Checked == true ?
                            controlListII.SGetText("poCode2C1TextBox") : controlListII.SGetText("poCode1C1TextBox");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/UpdateManufactInstructionMst?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("poCode", new JValue(poCode));

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

        /// <summary>
        /// 入出庫ファイル 印刷時更新処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) UpdateIOFile()
        {

            // 必要なパラメータ設定済
            var password = LoginInfo.Instance.UserNo;
            var machineName = LoginInfo.Instance.MachineCode;
            var nextProcessCate = controlListII.SGetText("nextProcessCateC1ComboBox");
            var groupCode = controlListII.SGetText("groupCodeC1TextBox");
            var nextProcessCode = controlListII.SGetText("nextProcessCodeC1TextBox");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/UpdateIOFile?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("password", new JValue(password));
            param.Add("machineName", new JValue(machineName));
            param.Add("nextProcessCate", new JValue(nextProcessCate));
            param.Add("groupCode", new JValue(groupCode));
            param.Add("nextProcessCode", new JValue(nextProcessCode));

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
        /// 工程マスタ 印刷データ取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetProcessMst()
        {

            // 必要なパラメータ設定済
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");
            var supCode = controlListII.SGetText("supCodeC1TextBox");
            var inNum = controlListII.SGetText("inNumC1NumericEdit").Replace(",", "");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetProcessMst?sid={solutionIdShort}&fid={formIdShort}";
            
            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("inNum", new JValue(inNum));

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
        /// 部品構成表 機種名取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetBOMMst()
        {

            // 必要なパラメータ設定済
            var partsCode = controlListII.SGetText("partsCodeC1TextBox");

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetBOMMst?sid={solutionIdShort}&fid={formIdShort}";
            
            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
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
        /// 製造指令ファイル 機種名取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetManufactFile(string jyuyoyosokuCode)
        {
            // 必要なパラメータ設定済
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F206E/GetManufactFile?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCode));

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
        /// レポート機種名取得
        /// </summary>
        /// <returns>IsOk:エラー無し False:エラー有り、機種名</returns>
        private string GetProductName(string jyuyoyosokuCode)
        {
            var productName = "";
            if (jyuyoyosokuCode == "")
            {
                // 需要予測番号が入力無い場合、部品構成表から取得
                var result2 = GetBOMMst();

                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部品構成表検索時に");
                    return "";
                }

                if (result2.Table.Rows.Count >= 1)
                {
                    productName = result2.Table.Rows[0]["productName"].ToString();
                }

                return productName;
            }
            else
            {
                // 需要予測番号が入力ある場合、製造指令ファイルから取得
                var result2 = GetManufactFile(jyuyoyosokuCode);

                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "製造指令ファイル検索時に");
                    return "";
                }

                if (result2.Table.Rows.Count <= 0)
                {
                    productName = "需要予測番号エラー";
                }

                var temp = result2.Table.Rows[0]["productName"].ToString();
                productName = temp == "" ? "機種マスタ無し" : temp;

                return productName;
            }
        }

        #endregion  ＜その他処理 END＞
        
    }
}
