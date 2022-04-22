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
using Newtonsoft.Json.Linq;
using System.Net;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 工程単価問合せ
    /// </summary>
    public partial class F406_ProcessUnitPriceInfo : BaseForm
    {

        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F406/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F406_ProcessUnitPriceInfo(string FID) : base(FID)
        {
            InitializeComponent();
            this.titleLabel.Text = "工程単価問合せ";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F406_ProcessUnitPriceInfo_Load(object sender, EventArgs e)
        {
            try
            {
                //C1TextBoxをリスト化
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, string.Empty, true, enumCate.Key);
                AddControlListII(drawingCodeC1TextBox, null, string.Empty, false, enumCate.Key);
                AddControlListII(partsNameC1TextBox, null, string.Empty, false, enumCate.Key);
                
                // 共通の設定変更
                foreach (var v in ControlListII)
                {
                    // 全コントロールのShowErrorMessageをfalseに変更
                    ((C1TextBox)v.Control).ErrorInfo.ShowErrorMessage = false;

                    // 色替えの場合には「BorderStyle」は「FixedSingle」でないと動かない
                    ((C1TextBox)v.Control).BorderStyle = BorderStyle.FixedSingle;

                    // 必須項目を赤枠に変更（不要ならデザイナ側でやってもよい）
                    if (v.Required == true)
                    {
                        ((C1TextBox)v.Control).BorderColor = Color.Red;
                    }
                    else
                    {
                        ((C1TextBox)v.Control).BorderColor = SystemColors.WindowFrame;
                    }

                    // ラベルがグレーになってしまうため、EnabledをTrueに戻す
                    if (((C1.Win.C1Input.C1TextBox)v.Control).Label != null)
                    {
                        ((C1.Win.C1Input.C1TextBox)v.Control).Label.Enabled = true;
                    }
                }

                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                    System.Drawing.GraphicsUnit.Point, (byte)128);

                // DefaultButtomMessageをセット
                DefButtomMessage = "必須項目入力後に実行（F10）を押してください。\r\n" +
                                    "親部品、子部品一覧の部品コードをクリックすると検索欄に部品コードが設定されます。";

                DisplayClear();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜起動処理 END＞


        #region ＜コンボボックス設定処理＞ 

        #endregion  ＜コンボボックス設定処理 END＞


        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            try
            {
                // ファンクションキーの使用可否設定
                //TopMenuEnable("F8", true);
                TopMenuEnable("F10", true);
                //TopMenuEnable("F12", true);

                // コントロールの一括クリア処理
                if (ControlListII != null)
                {
                    foreach (var v in ControlListII)
                    {
                        if (v.Control.GetType() == typeof(C1.Win.Calendar.C1DateEdit))
                        {
                            ((C1.Win.Calendar.C1DateEdit)v.Control).Value = v.Initial;
                        }
                        else if (v.Control.GetType() == typeof(C1NumericEdit))
                        {
                            ((C1NumericEdit)v.Control).Value = v.Initial;
                        }
                        else
                        {
                            ((C1TextBox)v.Control).Text = v.Initial;
                        }
                    }
                }

                c1TrueDBGrid1.DataSource = null;
                c1TrueDBGrid2.DataSource = null;
                c1TrueDBGrid3.DataSource = null;

                // トップメッセージクリア　
                ClearTopMessage();

                // ボトムメッセージに初期値設定　
                this.buttomMessageLabel.Text = DefButtomMessage;

                // エクセルファイル用DataTable
                //excelDT = null;
                //excelDT2 = null;

                // フォームオープン時のアクティブコントロールを設定
                this.ActiveControl = partsCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
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
                switch (this.ActiveControl.Name)
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
                        //drawingCodeC1TextBox.Text = form.row.Cells["図面番号"].Value.ToString();

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
                if (c1TrueDBGrid1.EditActive || c1TrueDBGrid3.EditActive)
                {
                    ActiveControl = F10Bt;
                }

                this.ActionProc();
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

                // 未入力時処理
                if (string.IsNullOrEmpty(((C1ComboBox)sender).Text))
                {
                    return false;
                }

                // ControlListIIから対象のコントロールの情報を取得
                var SelectList = ControlListII.Where(v => v.Control.Name == ((C1ComboBox)sender).Name).ToList();

                // ComboBoxリスト存在チェック
                var dv = (System.Data.DataView)((C1ComboBox)sender).ItemsDataSource;
                if (dv == null)
                {
                    ChangeTopMessage("W0013", ((C1ComboBox)sender).Label.Text);
                    this.ActiveControl = (C1ComboBox)sender;
                    e.Cancel = true;
                    return false;
                }

                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ((C1ComboBox)sender).Text + "' ";
                if (dv.Count < 1)
                {
                    ChangeTopMessage("W0013", ((C1ComboBox)sender).Label.Text);
                    this.ActiveControl = (C1ComboBox)sender;
                    e.Cancel = true;
                    return false;
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

                // ControlListIIから対象のコントロールの情報を取得
                var SelectList = ControlListII.Where(v => v.Control.Name == ((C1ComboBox)sender).Name).ToList();

                // コンボボックスに対応したテキストボックスがない場合は何もしない
                if (SelectList[0].SubControl == null) { return false; }

                // コンボボックスDataSourceをDataViewに変換
                var dv = (System.Data.DataView)((C1ComboBox)sender).ItemsDataSource;
                if (dv == null)
                {
                    SelectList[0].SubControl.Text = "";
                    return true;
                }

                // 未入力時処理
                if (string.IsNullOrEmpty(((C1ComboBox)sender).Text))
                {
                    SelectList[0].SubControl.Text = "";
                    dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '' ";
                    if (dv.Count > 0)
                    {
                        SelectList[0].SubControl.Text = dv.ToTable().Rows[0][1].ToString();
                    }
                    return true;
                }

                // ComboBoxの内容を名称テキストに反映
                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ((C1ComboBox)sender).Text + "' ";
                SelectList[0].SubControl.Text = dv.ToTable().Rows[0][1].ToString();

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
                if (((C1ComboBox)sender).SelectedIndex < 0)
                {
                    return;
                }

                // ControlListIIから対象のコントロールの情報を取得
                var SelectList = ControlListII.Where(v => v.Control.Name == ((C1ComboBox)sender).Name).ToList();

                // コンボボックスDataSourceをDataViewに変換
                var dv = (System.Data.DataView)((C1ComboBox)SelectList[0].Control).ItemsDataSource;
                if (dv == null)
                {
                    SelectList[0].SubControl.Text = "";
                    return;
                }

                // 未入力時処理
                if (string.IsNullOrEmpty(((C1ComboBox)sender).Text))
                {
                    dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '' ";
                    if (dv.Count > 0)
                    {
                        SelectList[0].SubControl.Text = dv.ToTable().Rows[0][1].ToString();
                        return;
                    }
                    else
                    {
                        SelectList[0].SubControl.Text = "";
                        return;
                    }
                }

                // ComboBoxの内容を名称テキストに反映
                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ((C1ComboBox)SelectList[0].Control).Text + "' ";
                SelectList[0].SubControl.Text = dv.ToTable().Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// ComboBoxリスト存在チェックと名称テキストに反映（全て）
        /// </summary>
        /// <returns></returns>
        private bool CheckComboBoxListAll()
        {
            try
            {
                var SelectControlListII = ControlListII.Where(v => v.Control.GetType() == typeof(C1ComboBox));
                foreach (var v in SelectControlListII)
                {

                    if (ControlAF.CheckComboBoxList((C1ComboBox)v.Control, (v.SubControl != null ? ((C1TextBox)v.SubControl) : null)) == false)
                    {
                        ChangeTopMessage("W0013", ((C1ComboBox)v.Control).Label.Text);
                        return false;
                    }
                }
                return true;
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
        /// 部品コード　検証時
        /// </summary>
        private void partsCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 部品コード
                var isOk = ErrorCheckPartsCode();
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
        /// 図面番号　検証時
        /// </summary>
        private void drawingCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 図面番号
                var isOk = ErrorCheckDrawingCode();
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
        /// 部品コード　検証後
        /// </summary>
        private void partsCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // クリア
                partsNameC1TextBox.Text = "";
                //drawingCodeC1TextBox.Text = "";

                var s = partsCodeC1TextBox;

                // 未入力時処理
                if (string.IsNullOrEmpty(((C1.Win.C1Input.C1TextBox)sender).Text))
                {
                    return;
                }

                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(s.Text));

                var result = CallSansoWebAPI("POST", apiUrl + "GetM_OrderParts", apiParam);

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "M_OrderParts検索時に");
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", s.Label.Text, "M_OrderParts");
                    return;
                }

                partsNameC1TextBox.Text = result.Table.Rows[0]["PartsName"].ToString();
                //drawingCodeC1TextBox.Text = result.Table.Rows[0]["DrawingCode"].ToString();


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
        /// </summary>
        private bool RequirF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）整合性チェック
        /// </summary>
        private bool ConsisF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）エラーチェック
        /// </summary>
        private bool ErrCKF08()
        {
            return true;
        }

        /// <summary>
        /// 実行（F10）必須チェック
        /// </summary>
        private bool RequirF10()
        {
            // 必須チェック
            var SelectControlListII = ControlListII.Where(v => v.Required == true);
            if (SelectControlListII != null)
            {
                foreach (var v in SelectControlListII)
                {
                    if (string.IsNullOrEmpty(v.Control.Text))
                    {
                        ChangeTopMessage("W0007", ((C1TextBox)v.Control).Label.Text);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 実行（F10）整合性チェック
        /// </summary>
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
        private bool ErrCKF10()
        {

            var isOk1 = ErrorCheckPartsCode();
            if (isOk1 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk3 = ErrorCheckDrawingCode();
            if (isOk3 == false)
            {
                ActiveControl = drawingCodeC1TextBox;
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

                this.isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                this.Cursor = Cursors.WaitCursor;

                c1TrueDBGrid1.SetDataBinding(null, "", true);
                c1TrueDBGrid2.SetDataBinding(null, "", true);
                c1TrueDBGrid3.SetDataBinding(null, "", true);
                EXCELdt = null;

                //対象部品
                //c1TrueDBGrid2
                //M_Proc
                apiParam.RemoveAll();
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("drawingCode", new JValue(drawingCodeC1TextBox.Text));

                var result2 = CallSansoWebAPI("POST", apiUrl + "GetM_Proc", apiParam);

                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "M_Proc検索時に");
                    return;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                c1TrueDBGrid2.SetDataBinding(result2.Table, "", true);
                EXCELdt = result2.Table;

                c1TrueDBGrid1.Enabled = true;
                c1TrueDBGrid3.Enabled = true;
                parts1Label.Enabled = true;
                parts3Label.Enabled = true;

                //M_BoM　子部品表示
                apiParam.RemoveAll();
                apiParam.Add("listType", new JValue("3"));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));

                var result3 = CallSansoWebAPI("POST", apiUrl + "GetM_BoM", apiParam);

                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "M_BoM検索時に");
                    return;
                }

                if (result3.Table == null || result3.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage(1, "INFO", "子部品データはありません");
                    //return;
                }

                c1TrueDBGrid3.SetDataBinding(result3.Table, "", true);


                //M_BoM　親部品表示
                apiParam.RemoveAll();
                apiParam.Add("listType", new JValue("1"));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));

                var result1 = CallSansoWebAPI("POST", apiUrl + "GetM_BoM", apiParam);

                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "M_BoM検索時に");
                    return;
                }

                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage(1, "INFO", "親部品データはありません");
                    //return;
                }

                c1TrueDBGrid1.SetDataBinding(result1.Table, "", true);

                ChangeTopMessage("I0006");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                this.Cursor = Cursors.Default;

                this.isRunValidating = true;
            }
        }

        /// <summary>
        /// エクセル出力処理
        /// </summary>
        //private void EXCELProc()
        //{
        //    try
        //    {
        //        if ((excelDT == null) || (excelDT.Rows.Count < 1))
        //        {
        //            ChangeTopMessage("I0007");
        //            excelDT = null;
        //            //return;
        //        }
        //        else
        //        {
        //            var param = new List<(int ColumnsNum, string Format, int? Width)>();
        //            excelDT.Columns.Remove("単価区分");
        //            excelDT.Columns.Remove("発注ロット区分");
        //            excelDT.Columns.Remove("発注確定区分");

        //            param.Add((5, "#,##", 2400));
        //            //param.Add((4, "yyyy/m/d;@", 1200));
        //            //param.Add((6, "yyyy/m/d;@", 1200));

        //            var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, excelDT);
        //            var result = cef.CreateSaveExcelFile(param);
        //            if (result.IsOk == false)
        //            {
        //                throw new Exception(result.Msg);
        //            }
        //        }

        //        if ((excelDT2 == null) || (excelDT2.Rows.Count < 1))
        //        {
        //            ChangeTopMessage("I0007");
        //            excelDT2 = null;
        //            return;
        //        }
        //        else
        //        {
        //            var param = new List<(int ColumnsNum, string Format, int? Width)>();
        //            excelDT2.Columns.Remove("単価区分");
        //            excelDT2.Columns.Remove("発注ロット区分");
        //            excelDT2.Columns.Remove("発注確定区分");

        //            var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, excelDT2);
        //            var result = cef.CreateSaveExcelFile(param);
        //            if (result.IsOk == false)
        //            {
        //                throw new Exception(result.Msg);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}");
        //    }
        //}


        /// <summary>
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {
            /*
            try
            {
                if (excelDT == null || excelDT.Rows.Count < 1)
                {
                    ChangeTopMessage("I0007");
                    excelDT = null;
                    //return;
                }

                if (excelDT2 == null || excelDT2.Rows.Count < 1)
                {
                    ChangeTopMessage("I0007");
                    excelDT2 = null;
                    //return;
                }

                if (excelDT == null && excelDT2 == null)
                {
                    ChangeTopMessage("I0007");
                    excelDT = null;
                    excelDT2 = null;
                    return;
                }
                else
                {
                    var sheet = new List<(DataTable Dt, string SheetName)>();
                    var param = new List<(int ColumnsNum, string Format, int? Width, string SheetName)>();

                    if (excelDT == null)
                    {
                        sheet.Add((excelDT2, "工程"));
                        param.Add((6, "yyyy/MM/dd", 1200, "工程"));
                        param.Add((7, "yyyy/MM/dd", 1200, "工程"));
                        param.Add((14, "#,##0.##0", 1200, "工程"));
                        param.Add((15, "#,##0.##0", 1200, "工程"));
                        param.Add((16, "#,##0.##0", 1200, "工程"));
                        param.Add((17, "#,##0.##0", 1200, "工程"));
                    }
                    else if (excelDT2 == null)
                    {
                        sheet.Add((excelDT, "前工程"));
                        param.Add((6, "yyyy/MM/dd", 1200, "前工程"));
                        param.Add((7, "yyyy/MM/dd", 1200, "前工程"));
                        param.Add((14, "#,##0.##0", 1200, "前工程"));
                        param.Add((15, "#,##0.##0", 1200, "前工程"));
                        param.Add((16, "#,##0.##0", 1200, "前工程"));
                        param.Add((17, "#,##0.##0", 1200, "前工程"));

                    }
                    else
                    {
                        sheet.Add((excelDT, "前工程"));
                        param.Add((6, "yyyy/MM/dd", 1200, "前工程"));
                        param.Add((7, "yyyy/MM/dd", 1200, "前工程"));
                        param.Add((14, "#,##0.##0", 1200, "前工程"));
                        param.Add((15, "#,##0.##0", 1200, "前工程"));
                        param.Add((16, "#,##0.##0", 1200, "前工程"));
                        param.Add((17, "#,##0.##0", 1200, "前工程"));

                        sheet.Add((excelDT2, "工程"));
                        param.Add((6, "yyyy/MM/dd", 1200, "工程"));
                        param.Add((7, "yyyy/MM/dd", 1200, "工程"));
                        param.Add((14, "#,##0.##0", 1200, "工程"));
                        param.Add((15, "#,##0.##0", 1200, "工程"));
                        param.Add((16, "#,##0.##0", 1200, "工程"));
                        param.Add((17, "#,##0.##0", 1200, "工程"));

                    }

                    var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, sheet);
                    var result = cef.CreateSaveMultiExcelFile(param);

                    if (result.IsOk == false)
                    {
                        throw new Exception(result.Msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            */
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        private void PrintProc()
        {
            try
            {
                // マウスカーソル待機状態
                this.Cursor = Cursors.WaitCursor;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                this.Cursor = Cursors.Default;
            }
        }



        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 


        /// <summary>
        /// エラーチェック  部品コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPartsCode()
        {
            partsNameC1TextBox.Text = "";

            // 未入力時処理
            var s = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var chk = Check.HasSQLBanChar(s.Text).Result;
            if (chk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(s.Text));

            var result = CallSansoWebAPI("POST", apiUrl + "GetM_OrderParts", apiParam);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "M_OrderParts検索時に");
                return false;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "M_OrderParts");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// エラーチェック  図面番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDrawingCode()
        {
            //partsNameC1TextBox.Text = "";

            // 未入力時処理
            var s = drawingCodeC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var chk = Check.HasSQLBanChar(s.Text).Result;
            if (chk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            apiParam.RemoveAll();
            apiParam.Add("drawingCode", new JValue(s.Text));

            var result = CallSansoWebAPI("POST", apiUrl + "GetM_OrderParts", apiParam);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "M_OrderParts検索時に");
                return false;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "M_OrderParts");
                return false;
            }

            return true;
        }

        #endregion  ＜その他処理 END＞


        /// <summary>
        /// 子部品一覧から部品コードを設定
        /// </summary>
        private void c1TrueDBGrid3_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                bool b = false;
                int R, C = 0;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;

                // CellContainingメソッドにより、グリッド内がクリックされたことを確認
                b = grid.CellContaining(e.X, e.Y, out R, out C);
                if (b == false)
                {
                    return;
                }

                // セルの所在行のデータ
                string partsCode = grid[R, "部品コード"].ToString().TrimEnd();
                if (partsCode == "")
                {
                    return;
                }

                if (grid.Columns[C].DataField == "ChPartsCode")
                {
                    partsCodeC1TextBox.Text = partsCode;
                    partsNameC1TextBox.Text = "";

                    apiParam.RemoveAll();
                    apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));

                    var result = CallSansoWebAPI("POST", apiUrl + "GetM_OrderParts", apiParam);

                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "M_OrderParts検索時に");
                        return;
                    }

                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0002", partsCodeC1TextBox.Label.Text, "M_OrderParts");
                        return;
                    }

                    partsNameC1TextBox.Text = result.Table.Rows[0]["PartsName"].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 子部品一覧から部品コードを設定
        /// </summary>
        private void c1TrueDBGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                bool b = false;
                int R, C = 0;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;

                // CellContainingメソッドにより、グリッド内がクリックされたことを確認
                b = grid.CellContaining(e.X, e.Y, out R, out C);
                if (b == false)
                {
                    return;
                }

                // セルの所在行のデータ
                string partsCode = grid[R, "部品コード"].ToString().TrimEnd();
                if (partsCode == "")
                {
                    return;
                }

                if (grid.Columns[C].DataField == "ChPartsCode")
                {
                    partsCodeC1TextBox.Text = partsCode;
                    partsNameC1TextBox.Text = "";

                    apiParam.RemoveAll();
                    apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));

                    var result = CallSansoWebAPI("POST", apiUrl + "GetM_OrderParts", apiParam);

                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "M_OrderParts検索時に");
                        return;
                    }

                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0002", partsCodeC1TextBox.Label.Text, "M_OrderParts");
                        return;
                    }

                    partsNameC1TextBox.Text = result.Table.Rows[0]["PartsName"].ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
