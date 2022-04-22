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
    /// バーコード次工程処理
    /// </summary>
    public partial class F212_DelivBarcodeNextProcess : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F212/";

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
        public F212_DelivBarcodeNextProcess(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "バーコード次工程処理";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F212_DelivBarcodeNextProcess_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット

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

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "一覧の「部品コード」と「仕入先」と「次工程」と「伝票種類」と「納入年月日」と「移行年月日」" +
                    "(薄い黄色列)は直接変更できます。\n" +
                    "「削除」ボタン押下後、該当行のデータが削除されます。　　　" +
                    "※F10(実行)押下後、一覧のデータで次工程処理を更新します。";

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
            TopMenuEnable("F6", false);
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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // 初期設定
            DrawC1TrueDBGrid();
        }

        #endregion  ＜クリア処理 END＞

        #region ＜共通イベント処理＞ 

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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// c1TrueDBGrid　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                ClearTopMessage();
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName != "削除")
                {
                    return;
                }

                var autoNo = grid[row, "autoNo"].ToString();

                // 削除
                apiParam.RemoveAll();
                apiParam.Add("autoNo", new JValue(autoNo));
                var result = ApiCommonUpdate(apiUrl + "DeleteWDelivBarcodeFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0007", "");
                    return;
                }

                DrawC1TrueDBGrid();
                ChangeTopMessage("I0003", "Ｗ納入受付バーコード");
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

        /// <summary>
        /// 非連携列のテキストを設定
        /// </summary>
        private void c1TrueDBGrid_UnboundColumnFetch(object sender, C1.Win.C1TrueDBGrid.UnboundColumnFetchEventArgs e)
        {
            switch (e.Column.Caption)
            {
                case "削除":
                    e.Value = "削除";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 一覧画面　検証している処理
        /// </summary>
        private void c1TrueDBGrid_BeforeColUpdate(object sender, C1.Win.C1TrueDBGrid.BeforeColUpdateEventArgs e)
        {
            try
            {
                ClearTopMessage();
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int rowIndex = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;
                var caption = e.Column.DataColumn.Caption;
                var field = e.Column.DataColumn.DataField;
                var text = e.Column.DataColumn.Text.TrimEnd();

                if (caption != "部品コード" 
                    && caption != "仕入先" 
                    && caption != "次工程" 
                    && caption != "伝票種類"
                    && caption != "納入年月日"
                    && caption != "移行年月日")
                {
                    ChangeTopMessage(1, "ERR", "（「部品コード」と「仕入先」と「次工程」と「伝票種類」と「納入年月日」と「移行年月日」" +
                        "（薄黄色列）以外は更新できません");
                    e.Cancel = true;
                    return;
                }

                // 未入力チェック
                if (caption != "次工程" && text == "")
                {
                    ChangeTopMessage("W0007", caption);
                    e.Cancel = true;
                    return;
                }

                var autoNo = grid[rowIndex, "autoNo"].ToString().TrimEnd();
                var partsCode = grid[rowIndex, "partsCode"].ToString();
                var supCode = grid[rowIndex, "supCode"].ToString();
                var transDate = grid[rowIndex, "transDate"].ToString();
                var delivDate = grid[rowIndex, "delivDate"].ToString();
                var tableIndex = excelDt.Rows.IndexOf(excelDt.AsEnumerable().Where(v => v["autoNo"].ToString() == autoNo)
                    .FirstOrDefault());

                if (caption == "部品コード")
                {
                    var result = ErrorCheckPartsCode(text, supCode, caption);
                    if (result.IsOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    excelDt.Rows[tableIndex]["partsName"] = result.PartsName;

                    grid[rowIndex, field] = text;
                    grid[rowIndex, "partsName"] = result.PartsName;
                }
                else if (caption == "仕入先")
                {
                    var result = ErrorCheckSupCode(text, partsCode, caption);
                    if (result.IsOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    excelDt.Rows[tableIndex]["supName"] = result.SupName;

                    grid[rowIndex, field] = text;
                    grid[rowIndex, "supName"] = result.SupName;
                }
                else if (caption == "次工程")
                {                
                    var result = ErrorCheckNextProcessCode(text, partsCode, caption);
                    if (result.IsOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    excelDt.Rows[tableIndex]["nextProcessName"] = result.SupName;
                    excelDt.Rows[tableIndex]["supCate"] = result.SupCate;
                    excelDt.Rows[tableIndex]["printCate"] = result.PrintCate;

                    grid[rowIndex, field] = text;
                    grid[rowIndex, "nextProcessName"] = result.SupName;
                    grid[rowIndex, "supCate"] = result.SupCate;
                    grid[rowIndex, "printCate"] = result.PrintCate;
                }
                else if (caption == "伝票種類")
                {
                    var isOk = ErrorCheckPrintFlg(text, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    grid[rowIndex, field] = text;
                }
                else if (caption == "納入年月日")
                {
                    var isOk = ErrorCheckDelivDate(text, transDate, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    grid[rowIndex, field] = text;
                }
                else if (caption == "移行年月日")
                {
                    var isOk = ErrorCheckTransDate(text, delivDate, caption);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    excelDt.Rows[tableIndex][field] = text;
                    grid[rowIndex, field] = text;
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
        /// 一覧画面　列を更新した後処理
        /// </summary>
        private void c1TrueDBGrid_AfterColUpdate(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int rowIndex = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;
                var caption = e.Column.DataColumn.Caption;
                var field = e.Column.DataColumn.DataField;

                if (caption != "部品コード"
                    && caption != "仕入先"
                    && caption != "次工程"
                    && caption != "伝票種類"
                     && caption != "納入年月日"
                    && caption != "移行年月日")
                {
                    return;
                }

                // 更新前の情報
                var autoNo = grid[rowIndex, "autoNo"].ToString().TrimEnd();
                var tableIndex = excelDt.Rows.IndexOf(excelDt.AsEnumerable().Where(v => v["autoNo"].ToString() == autoNo)
                    .FirstOrDefault());

                // 更新後の情報
                var text = excelDt.Rows[tableIndex][field].ToString().TrimEnd();
                var partsCode = excelDt.Rows[tableIndex]["partsCode"].ToString().TrimEnd();
                var partsName = excelDt.Rows[tableIndex]["partsName"].ToString().TrimEnd();
                var supCode = excelDt.Rows[tableIndex]["supCode"].ToString().TrimEnd();
                var supName = excelDt.Rows[tableIndex]["supName"].ToString().TrimEnd();
                var nextProcessCode = excelDt.Rows[tableIndex]["nextProcessCode"].ToString().TrimEnd();
                var nextProcessName = excelDt.Rows[tableIndex]["nextProcessName"].ToString().TrimEnd();
                var supCate = excelDt.Rows[tableIndex]["supCate"].ToString().TrimEnd();
                var printCate = excelDt.Rows[tableIndex]["printCate"].ToString().TrimEnd();
                var delivDate = excelDt.Rows[tableIndex]["delivDate"].ToString().TrimEnd();
                var transDate = excelDt.Rows[tableIndex]["transDate"].ToString().TrimEnd();

                // 未入力チェック
                if (caption != "次工程" && text == "")
                {
                    ChangeTopMessage("W0007", caption);
                    return;
                }

                // パラメータ
                apiParam.RemoveAll();
                apiParam.Add("autoNo", new JValue(autoNo));
                apiParam.Add("captionName", new JValue(caption));

                if (caption == "部品コード")
                {
                    apiParam.Add("partsCode", new JValue(partsCode));
                    apiParam.Add("partsName", new JValue(partsName));
                }
                else if (caption == "仕入先")
                {
                    apiParam.Add("supCode", new JValue(supCode));
                    apiParam.Add("supName", new JValue(supName));
                }
                else if (caption == "次工程")
                {
                    apiParam.Add("nextProcessCode", new JValue(nextProcessCode));
                    apiParam.Add("nextProcessName", new JValue(nextProcessName));
                    apiParam.Add("supCate", new JValue(supCate));
                    apiParam.Add("printCate", new JValue(printCate));
                }
                else if (caption == "伝票種類")
                {
                    apiParam.Add("printCate", new JValue(printCate));
                }
                else if (caption == "納入年月日")
                {
                    apiParam.Add("delivDate", new JValue(delivDate));
                }
                else if (caption == "移行年月日")
                {
                    apiParam.Add("transDate", new JValue(transDate));
                }
                else
                {
                    // 処理なし
                }

                // 修正
                var result = ApiCommonUpdate(apiUrl + "UpdateWDelivBarcodeFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0001", "Ｗ納入受付バーコード");
                    return;
                }

                DrawC1TrueDBGrid();
                ChangeTopMessage("I0002", "Ｗ納入受付バーコード");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜イベント処理 END＞

        #region ＜実行前チェック＞ 

        /// <summary>
        /// 実行（F10）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF10()
        {
            if (c1TrueDBGrid.RowCount <= 1)
            {
                ChangeTopMessage("I0007");
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

                var dialog = MessageBox.Show("バーコード次工程処理を実行します、開始してもよいですか？",
                                                 "更新の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // データ登録
                var result1 = ApiCommonUpdate(apiUrl + "UpdateNextProcess", null);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "更新時に");
                    return;
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue("HT"));
                apiParam.Add("computerName", new JValue("HT"));
                var result2 = ApiCommonGet(apiUrl + "GetInsideTransData", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
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

                    // 可視設定
                    var vList = new Dictionary<string, bool>();
                    vList.Add("テキスト_denNo", true);

                    var p = PrintReport("R003_InsideTrans", v.Table, null, null, vList);
                    if (p == false)
                    {
                        return;
                    }
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue("HT"));
                apiParam.Add("computerName", new JValue("HT"));
                var result3 = ApiCommonGet(apiUrl + "GetPaintInsideTransData", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result3.Table != null && result3.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result3.Table);
                    if (v.IsOk == false)
                    {
                        return;
                    }

                    // 可視設定
                    var vList = new Dictionary<string, bool>();
                    vList.Add("テキスト_denNo", true);

                    var p = PrintReport("R004_InsideTransPaint", v.Table, null, null, vList);
                    if (p == false)
                    {
                        return;
                    }
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue("HT"));
                apiParam.Add("computerName", new JValue("HT"));
                var result4 = ApiCommonGet(apiUrl + "GetNSupStockTransDocuBarcodeData", apiParam);
                if (result4.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result4.Table != null && result4.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result4.Table);
                    if (v.IsOk == false)
                    {
                        return;
                    }

                    var p = PrintReport("R007_NSupStockTransDocuBarcode", v.Table, null, null, null);
                    if (p == false)
                    {
                        return;
                    }
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue("HT"));
                apiParam.Add("computerName", new JValue("HT"));
                var result5 = ApiCommonGet(apiUrl + "GetNSupStockTransDocuData", apiParam);
                if (result5.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result5.Table != null && result5.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result5.Table);
                    if (v.IsOk == false)
                    {
                        return;
                    }

                    var p = PrintReport("R006_NSupStockTransDocu", v.Table, null, null, null);
                    if (p == false)
                    {
                        return;
                    }
                }

                // クリア
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue("HT"));
                apiParam.Add("computerName", new JValue("HT"));
                var result6 = ApiCommonUpdate(apiUrl + "ClearWDelivBarcode", apiParam);
                if (result6.IsOk == false)
                {
                    ChangeTopMessage("E0008", "更新時に");
                    return;
                }

                DisplayClear();
                ChangeTopMessage("I0009", "バーコード次工程処理が");
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
            c1TrueDBGrid.SetDataBinding(null, "", true);

            // Ｗ納入受付バーコード
            apiParam.RemoveAll();
            apiParam.Add("staffCode", new JValue("HT"));
            apiParam.Add("computerName", new JValue("HT"));
            var result = ApiCommonGet(apiUrl + "GetWDelivBarcodeFile", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("I0005");
                return;
            }

            excelDt = result.Table.Copy();
            c1TrueDBGrid.SetDataBinding(result.Table.Copy(), "", true);

            // c1TrueDBGridのコンボボックス設定
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("0", "0");
            dt.Rows.Add("1", "1");
            dt.Rows.Add("2", "2 ");
            dt.Rows.Add("3", "3");

            C1.Win.C1TrueDBGrid.C1DataColumn col1;
            col1 = c1TrueDBGrid.Columns["printCate"];
            col1.ValueItems.Values.Clear();
            foreach (DataRow v in dt.Rows)
            {
                col1.ValueItems.Values.Add(new C1.Win.C1TrueDBGrid.ValueItem(v.Field<string>("ID").ToString(),
                                                                             v.Field<string>("ID")));
            }
            col1.ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.ComboBox;
            col1.ValueItems.Translate = true;

            ChangeTopMessage("I0011", result.Table.Rows.Count.ToString("#,###"));
        }

        /// <summary>
        /// エラーチェック  部品コード
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        /// <param name="name">名前</param>
        /// <returns>IsOk(True：エラーが無し False：エラーがある)  、部品名</returns>
        private (bool IsOk, string PartsName) ErrorCheckPartsCode(string partsCode, string supCode, string name)
        {
            // 未入力チェック
            if (partsCode == "")
            {
                ChangeTopMessage("W0007", name);
                return (false, "");
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(partsCode).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return (false, "");
            }

            // 部品マスタ
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("partsCode", new JValue(partsCode));
            var result1 = ApiCommonGet(apiUrl + "GetPartsMst", apiParam);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return (false, "");
            }
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "部品マスタ");
                return (false, "");
            }

            var partsName = result1.Table.Rows[0]["partsName"].ToString();

            // 工程マスタ
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCode));
            apiParam.Add("supCode", new JValue(supCode));
            var result2 = ApiCommonGet(apiUrl + "GetProcessMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "工程マスタ検索時に");
                return (false, "");
            }
            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "工程マスタ");
                return (false, "");
            }

            return (true, partsName);
        }

        /// <summary>
        /// エラーチェック  仕入先
        /// </summary>
        /// <param name="supCode">仕入先コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="name">名前</param>
        /// <returns>IsOk(True：エラーが無し False：エラーがある)  、仕入先名</returns>
        private (bool IsOk, string SupName) ErrorCheckSupCode(string supCode, string partsCode, string name)
        {
            // 未入力チェック
            if (supCode == "")
            {
                ChangeTopMessage("W0007", name);
                return (false, "");
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(supCode).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return (false, "");
            }

            // 仕入先マスタ
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("supCode", new JValue(supCode));
            var result1 = ApiCommonGet(apiUrl + "GetSupMst", apiParam);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return (false, "");
            }
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "仕入先マスタ");
                return (false, "");
            }

            var supName = result1.Table.Rows[0]["supName"].ToString();

            // 工程マスタ
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCode));
            apiParam.Add("supCode", new JValue(supCode));
            var result2 = ApiCommonGet(apiUrl + "GetProcessMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "工程マスタ検索時に");
                return (false, "");
            }
            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "工程マスタ");
                return (false, "");
            }

            return (true, supName);
        }

        /// <summary>
        /// エラーチェック  次工程
        /// </summary>
        /// <param name="supCode">次工程コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="name">名前</param>
        /// <returns>IsOk(True：エラーが無し False：エラーがある)  、仕入先名、仕入先区分、発行区分</returns>
        private (bool IsOk, string SupName, string SupCate, string PrintCate) ErrorCheckNextProcessCode(string supCode, string partsCode, 
            　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　string name)
        {
            // 未入力チェック
            if (supCode == "")
            {
                return (true, "", "0", "0");
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(supCode).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return (false, "", "", "");
            }

            // 仕入先マスタ
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("supCode", new JValue(supCode));
            var result1 = ApiCommonGet(apiUrl + "GetSupMst", apiParam);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return (false, "", "", "");
            }
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "仕入先マスタ");
                return (false, "", "", "");
            }

            var supCate = result1.Table.Rows[0]["supCate"].ToString();
            var supName = result1.Table.Rows[0]["supName"].ToString();
            var printCate = "0";

            switch (supCate)
            {
                case "K":
                    printCate = "1";
                    break;

                case "G":
                    printCate = "2";
                    break;

                case "S":
                    printCate = "3";
                    break;

                default:
                    printCate = "0";
                    break;
            }

            // 工程マスタ
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCode));
            apiParam.Add("supCode", new JValue(supCode));
            var result2 = ApiCommonGet(apiUrl + "GetProcessMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "工程マスタ検索時に");
                return (false, "", "", "");
            }
            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", name, "工程マスタ");
                return (false, "", "", "");
            }

            return (true, supName, supCate, printCate);
        }

        /// <summary>
        /// エラーチェック  伝票種類
        /// </summary>
        /// <param name="txt">テキスト</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPrintFlg(string txt, string name)
        {
            // 未入力チェック
            if (txt == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            if (txt != "0" && txt != "1" && txt != "2" && txt != "3")
            {
                ChangeTopMessage("W0013", name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  納入年月日
        /// </summary>
        /// <param name="delivDate">納入年月日</param>
        /// <param name="transDate">移行年月日</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivDate(string delivDate, string transDate, string name)
        {
            // 未入力チェック
            if (delivDate == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            var chk = Check.IsDate(delivDate);
            if (chk.Result == false)
            {
                ChangeTopMessage("W0019", chk.Msg);
                return false;
            }

            if (transDate == "") 
            {
                return true;
            }

            if (DateTime.Parse(transDate) < DateTime.Parse(delivDate))
            {
                ChangeTopMessage("W0014", name, "移行年月日");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  移行年月日
        /// </summary>
        /// <param name="transDate">移行年月日</param>
        /// <param name="delivDate">納入年月日</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckTransDate(string transDate, string delivDate, string name)
        {
            // 未入力チェック
            if (transDate == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            var chk = Check.IsDate(transDate);
            if (chk.Result == false)
            {
                ChangeTopMessage("W0019", chk.Msg);
                return false;
            }

            if (delivDate == "")
            {
                return true;
            }

            if (DateTime.Parse(transDate) < DateTime.Parse(delivDate))
            {
                ChangeTopMessage("W0014", "納入年月日", name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0])</returns>
        private (bool IsOk, int Count) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
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
                        return (false, 0);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0);
            }

            if ((bool)result["isOk"] == false)
            {
                return (false, 0);
            }

            return (
                true,
                (int)(result["count"])
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

        /// <summary>
        /// レポート印刷処理
        /// </summary>
        /// <param name="reportName">レポート名</param>
        /// <param name="table">レポート印刷用データテーブル</param>
        /// <param name="fieldList">レポートのフィールドの名前と値</param>
        /// <param name="subSql">サブレポートのSQL文字列</param>
        /// <param name="visibleList">レポートのフィールドの可視リスト（フィールドの名前と可視値「bool/false」）</param>
        /// <returns>true:印刷成功　false：印刷失敗</returns>
        private bool PrintReport(string reportName, DataTable table, Dictionary<string, string> fieldList = null,
            string subSql = "", Dictionary<string, bool> visibleList = null)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                return true;
            }

            using (var report = new C1.Win.FlexReport.C1FlexReport())
            {
                report.Load(EXE_DIRECTORY + @"\Reports\" + reportName + ".flxr", reportName);

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = reportConnectionString,
                    Recordset = table
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                if (fieldList != null && fieldList.Count() >= 1)
                {
                    foreach (KeyValuePair<string, string> v in fieldList)
                    {
                        ((C1.Win.FlexReport.Field)report.Fields[v.Key]).Text = v.Value;
                    }
                }

                if (subSql != null && subSql != "")
                {
                    // サブレポート1  設定
                    var dsSub1 = new C1.Win.FlexReport.DataSource
                    {
                        Name = " ",
                        ConnectionString = reportConnectionString,
                        RecordSource = subSql
                    };
                    ((C1.Win.FlexReport.SubreportField)report.Fields["sub1"]).Subreport.DataSources.Add(dsSub1);
                    ((C1.Win.FlexReport.SubreportField)report.Fields["sub1"]).Subreport.DataSourceName = dsSub1.Name;
                }

                if (visibleList != null && visibleList.Count() >= 1)
                {
                    foreach (KeyValuePair<string, bool> v in visibleList)
                    {
                        ((C1.Win.FlexReport.Field)report.Fields[v.Key]).Visible = v.Value;
                    }
                }

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (print.IsOk == false)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return false;
                }
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

        #endregion  ＜その他処理 END＞
    }
}