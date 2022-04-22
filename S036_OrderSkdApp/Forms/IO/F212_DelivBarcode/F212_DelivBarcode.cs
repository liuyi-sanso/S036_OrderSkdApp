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
    /// 納入受付　バーコード
    /// </summary>
    public partial class F212_DelivBarcode : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 部門コード
        /// </summary>
        private string groupCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F212_DelivBarcode(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "納入受付　バーコード";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F212_DelivBarcode_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(status1C1TextBox, null, "", false, enumCate.無し);
                AddControlListII(status2C1TextBox, null, "", false, enumCate.無し);
                AddControlListII(status3C1TextBox, null, "", false, enumCate.無し);
                AddControlListII(statusC1TextBox, null, "", false, enumCate.無し);

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

                // 処理年月チェック
                var result = ApiCommonGet(apiUrl + "Solution/S036/F212/GetProcessDateCount");
                if (result.IsOk == false)
                {
                    MessageBox.Show("処理年月チェック時にエラー発生しました。" +
                                    "\r\n処理を中止します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    MessageBox.Show("処理年月チェック時にエラー発生しました。" +
                                    "\r\n処理を中止します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                var count = result.Table.Rows[0].Field<double>("count");
                if (count != 1d)
                {
                    MessageBox.Show("処理年月が違う課別が存在します。" +
                                    "\r\n（月末処理をしている課別、していない課別が存在している。）" +
                                    "\r\n処理年月を統一してから処理してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "「取込開始」→「納入受付　更新」を押してください。　 ";

                // クリア処理
                DisplayClear();

                // 部門マスタ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue("3623"));
                var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetGroupMst", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部門マスタ検索時に");
                    return;
                }
                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    groupCode = "";
                }
                else
                {
                    groupCode = "3623";
                }
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
            startButton.Enabled = true;
            notProcesseListButton.Enabled = false;
            notProcesseListPrintButton.Enabled = false;
            errorListButton.Enabled = false;
            inPossibleListButton.Enabled = false;
            updateButton.Enabled = false;

            // 進捗表示
            DisplayStatus();

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = startButton;
        }

        /// <summary>
        /// ボディクリア処理
        /// </summary>
        private void DisplayClearBody()
        {
            // 初期設定
            startButton.Enabled = true;
            notProcesseListButton.Enabled = false;
            notProcesseListPrintButton.Enabled = false;
            errorListButton.Enabled = false;
            inPossibleListButton.Enabled = false;
            updateButton.Enabled = false;

            // 進捗表示
            DisplayStatus();

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = startButton;
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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// フォームを閉じようとした時の処理
        /// </summary>
        private void F212_DelivBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isRunValidating = false;
                if (status1C1TextBox.Text != "")
                {
                    var dialog = MessageBox.Show("納入受付一括バーコードが実行中です。閉じてよろしいですか？",
                                                     "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                     MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                this.isRunValidating = true;
            }
        }

        /// <summary>
        /// 取込開始ボタン押下後、 データインポート
        /// </summary>
        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                var dialog1 = MessageBox.Show("納入受付バーコードデータを受信してから行ってください、準備はよろしいですか？" +
                                             "\n既に取込んであるデータ(未処理)は削除されます。",
                                             "データ取込みの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);
                if (dialog1 != DialogResult.Yes)
                {
                    statusC1TextBox.Text = "取込は、キャンセルされました";
                    statusC1TextBox.Refresh();
                    return;
                }

                var dialog2 = MessageBox.Show("納入受付バーコードデータを取込みます、開始してもよいですか？",
                                             "データ取込みの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);
                if (dialog2 != DialogResult.Yes)
                {
                    statusC1TextBox.Text = "取込は、キャンセルされました";
                    statusC1TextBox.Refresh();
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                statusC1TextBox.Text = "納入受付バーコードデータ登録中";
                statusC1TextBox.Refresh();

                // データインポート
                string path = Environment.GetEnvironmentVariable("systemdrive");
                path = path + @"\" + "00" + @"\" + "SIR.TXT";

                // ファイル存在か
                if (System.IO.File.Exists(path) == false)
                {
                    ChangeTopMessage(1, "WARN", "バーコードファイル（SIR.TXT）が存在しません。");
                    return;
                }

                // 中身は空白か
                using (var reader = new System.IO.StreamReader(path))
                {
                    var text = reader.ReadToEnd();
                    reader.Close();

                    if (string.IsNullOrEmpty(text.Trim()))
                    {
                        ChangeTopMessage(1, "WARN", "バーコードファイルの中身は空白です。");
                        return;
                    }
                }

                // 合計件数（txtファイルの行数合計）
                int totalCount = 0;
                using (var reader = new System.IO.StreamReader(path))
                {
                    while (reader.ReadLine() != null)
                    {
                        totalCount++;
                    }
                }

                // データインポート
                apiParam.RemoveAll();
                apiParam.Add("path", new JValue(path));
                apiParam.Add("groupCode", new JValue(groupCode));
                var result = ApiCommonUpdate(apiUrl + "Solution/S036/F212/WDelivBarcodeDataImport", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "データインポート時に");
                    statusC1TextBox.Text += "\nエラー発生！データを確認してもう一度最初から処理して下さい。";
                    statusC1TextBox.Refresh();
                    return;
                }

                statusC1TextBox.Text += "\n" + totalCount + " 件  取り込みました。" +
                                        "\n" + (totalCount - result.Count) + "件  異常" +
                                        "\n" + result.Count + "件  完了";

                statusC1TextBox.Text += "\n納入受付バーコードデータ取込み　完了";
                statusC1TextBox.Refresh();

                // エラーチェック
                if (ErrCKF10() == false) 
                {
                    return;
                }

                statusC1TextBox.Text += "\n納入受付バーコードデータチェック　完了";
                statusC1TextBox.Refresh();

                // 次工程ありデータ一覧発行
                var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNextProcessList");
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result2.Table != null && result2.Table.Rows.Count >= 1)
                {
                    var list = new Dictionary<string, string>();
                    list.Add("テキスト_件数", result2.Table.Rows.Count.ToString("#,##0"));

                    var p = PrintReport("R025_DelivBarcodeNextProcessList", result2.Table, list);
                    if (p == false) 
                    {
                        return;
                    }
                }

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue("番"));
                apiParam.Add("content2", new JValue("1"));
                apiParam.Add("content3", new JValue(DateTime.Now));
                var result3 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 進捗表示
                DisplayStatus();

                startButton.Enabled = false;
                notProcesseListButton.Enabled = true;
                notProcesseListPrintButton.Enabled = true;
                errorListButton.Enabled = true;
                inPossibleListButton.Enabled = true;
                updateButton.Enabled = true;
                ActiveControl = notProcesseListButton;
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
        /// 未処理一覧（検収）ボタン押下後、Ｗ納入受付バーコードを修正できる
        /// </summary>
        private void notProcesseListButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // エラーチェック
                if (ErrCKF10() == false)
                {
                    return;
                }

                // 納入データ検収画面へ遷移
                using (var form = new F212_DelivBarcodeNotProcessList("F212_DelivBarcodeNotProcessList"))
                {
                    form.ShowDialog();
                }

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue("番"));
                apiParam.Add("content2", new JValue("2"));
                apiParam.Add("content3", new JValue(DateTime.Now));
                var result2 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 進捗表示
                DisplayStatus();
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
        /// 未処理リストボタン押下後、未処理リストを印刷
        /// </summary>
        private void notProcesseListPrintButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // エラーチェック
                if (ErrCKF10() == false)
                {
                    return;
                }

                // レポートデータ
                var result = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNotProcessList", null);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                }
                else 
                {
                    var list = new Dictionary<string, string>();
                    list.Add("テキスト_件数", result.Table.Rows.Count.ToString("#,##0"));

                    var p = PrintReport("R026_DelivBarcodeNotProcessList", result.Table, list);
                    if (p == false)
                    {
                        return;
                    }
                }

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue("番"));
                apiParam.Add("content2", new JValue("3"));
                apiParam.Add("content3", new JValue(DateTime.Now));
                var result2 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 進捗表示
                DisplayStatus();
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
        /// エラーリストボタン押下後、エラーリストを印刷
        /// </summary>
        private void errorListButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // エラーチェック
                if (ErrCKF10() == false)
                {
                    return;
                }

                // レポート
                var result1 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNoOrdDetailErrorList", null);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result1.Table != null && result1.Table.Rows.Count >= 1)
                {
                    var list1 = new Dictionary<string, string>();
                    list1.Add("テキスト_件数", result1.Table.Rows.Count.ToString("#,##0"));
                    list1.Add("title", "納入受付バーコード　エラー（発注明細無し）");

                    var p1 = PrintReport("R027_DelivBarcodeErrorList", result1.Table, list1);
                    if (p1 == false)
                    {
                        return;
                    }
                }

                // レポート
                var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetErrorList", null);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                }
                else 
                {
                    var list2 = new Dictionary<string, string>();
                    list2.Add("テキスト_件数", result2.Table.Rows.Count.ToString("#,##0"));
                    list2.Add("title", "納入受付バーコード　エラー一覧");

                    var p2 = PrintReport("R027_DelivBarcodeErrorList", result2.Table, list2);
                    if (p2 == false)
                    {
                        return;
                    }
                }

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue("番"));
                apiParam.Add("content2", new JValue("4"));
                apiParam.Add("content3", new JValue(DateTime.Now));
                var result3 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 進捗表示
                DisplayStatus();
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
        /// 入庫可能リストボタン押下後、入庫可能リストを印刷
        /// </summary>
        private void inPossibleListButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // エラーチェック
                if (ErrCKF10() == false)
                {
                    return;
                }

                // レポート
                var result1 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNextProcessList", null);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result1.Table != null && result1.Table.Rows.Count >= 1)
                {
                    var list1 = new Dictionary<string, string>();
                    list1.Add("テキスト_件数", result1.Table.Rows.Count.ToString("#,##0"));

                    var p1 = PrintReport("R025_DelivBarcodeNextProcessList", result1.Table, list1);
                    if (p1 == false)
                    {
                        return;
                    }
                }

                // レポート
                var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetInPossibleList", null);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                }
                else 
                {
                    string subSql = "SELECT 課別コード, COUNT(*) AS 件数 " +
                    "FROM 製造調達.dbo.Ｗ納入受付バーコード " +
                    "WHERE(ISNULL(納入エラーFLG, '') <> 'E') " +
                    "AND (ISNULL(作成者, '') = '50240') " +
                    "AND (ISNULL(作成端末, '') = 'SCL1028') " +
                    "AND (ISNULL(課別コード, '') = 課別コードpara) " +
                    "GROUP BY 課別コード ";

                    var p2 = PrintReport("R028_DelivBarcodeInPossibleList", result2.Table, null, subSql);
                    if (p2 == false)
                    {
                        return;
                    }
                }

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue("番"));
                apiParam.Add("content2", new JValue("5"));
                apiParam.Add("content3", new JValue(DateTime.Now));
                var result3 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 進捗表示
                DisplayStatus();
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
        /// 更新ボタン押下後、入出庫を更新
        /// </summary>
        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // 処理年月チェック
                var result1 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetProcessDateCount");
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "処理年月チェック時に");
                    return;
                }

                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("E0008", "処理年月チェック時に");
                    return;
                }

                var count = result1.Table.Rows[0].Field<double>("count");
                if (count != 1d)
                {
                    ChangeTopMessage(1, "WARN", "処理年月が違う課別が存在します。" +
                        "（月末処理をしている課別、していない課別が存在している）");
                    return;
                }

                // エラーチェック
                if (ErrCKF10() == false)
                {
                    return;
                }

                // 更新用データリストを取得
                var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetUpdateDataList", null);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "更新用データ検索時に");
                    return;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "更新用");
                    return;
                }

                result2.Table.Columns.Add("delivNum", typeof(decimal));
                foreach (DataRow v in result2.Table.Rows)
                {
                    string doCode = v["doCode"].ToString();

                    // エラーチェック 納品書番号
                    (bool isOk, decimal delivNum) = ErrorCheckCode(doCode);
                    if (isOk == false)
                    {
                        return;
                    }

                    v["delivNum"] = delivNum;
                }

                var dialog = MessageBox.Show("バーコード入力　納入受付を実行します、開始してもよいですか？",
                                                 "更新の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    statusC1TextBox.Text = "更新は、キャンセルされました";
                    statusC1TextBox.Refresh();
                    return;
                }

                statusC1TextBox.Text = "納入受付　処理開始";
                statusC1TextBox.Refresh();

                // データ登録
                apiParam.RemoveAll();
                string json = JsonConvert.SerializeObject(result2.Table, Formatting.Indented);
                apiParam.Add("tableJson", new JValue(json));
                var result3 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateIOFile", apiParam);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage(1,"ERR", result3.Msg);
                    return;
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                var result4 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetInsideTransData", apiParam);
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
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                var result5 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetPaintInsideTransData", apiParam);
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
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                var result6 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNSupStockTransDocuBarcodeData", apiParam);
                if (result6.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result6.Table != null && result6.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result6.Table);
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
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                var result7 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetNSupStockTransDocuData", apiParam);
                if (result7.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result7.Table != null && result7.Table.Rows.Count >= 1)
                {
                    // 機種名設定
                    var v = SetProductName(result7.Table);
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
                apiParam.Add("staffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("computerName", new JValue(LoginInfo.Instance.MachineCode));
                var result8 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/ClearWDelivBarcode", apiParam);
                if (result8.IsOk == false)
                {
                    ChangeTopMessage("E0008", "更新時に");
                    return;
                }

                statusC1TextBox.Text += "\n納入受付　処理完了";
                statusC1TextBox.Refresh();

                // 進捗更新
                apiParam.RemoveAll();
                apiParam.Add("cate", new JValue("納入受付"));
                apiParam.Add("content1", new JValue(""));
                apiParam.Add("content2", new JValue("0"));
                apiParam.Add("content3", new JValue(""));
                var result9 = ApiCommonUpdate(apiUrl + "Solution/S036/F212/UpdateVariableMst", apiParam);
                if (result9.IsOk == false)
                {
                    ChangeTopMessage("E0008", "進捗更新時に");
                    return;
                }

                // 品管検査対象
                var result10 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetCheckList", null);
                if (result10.IsOk == false)
                {
                    ChangeTopMessage("E0008", "品管検査対象検索時に");
                    return;
                }

                if (result10.Table != null && result10.Table.Rows.Count >= 1)
                {
                    var n1 = result10.Table.AsEnumerable().Where(v => v["alert"].ToString() == "初品対象").Count();
                    var n2 = result10.Table.AsEnumerable().Where(v => v["alert"].ToString() == "初回対象").Count();

                    statusC1TextBox.Text += (n1 >= 1 ? "\n初品検査対象品があります" : "");
                    statusC1TextBox.Text += (n2 >= 1 ? "\n初回ロット検査対象品があります" : "");
                    statusC1TextBox.Refresh();
                }

                DisplayClearBody();
                ChangeTopMessage("I0009", "納入受付処理が");
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

        #endregion  ＜イベント処理 END＞

        #region ＜実行前チェック＞ 

        /// <summary>
        /// 実行（F10）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF10()
        {
            var startDate = DateTime.Parse(executeDateValueLabel.Text + "/01");
            var endDate = startDate.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd" + " 23:59:59");

            // データインポート
            apiParam.RemoveAll();
            apiParam.Add("startDate", new JValue(startDate));
            apiParam.Add("endDate", new JValue(endDate));
            apiParam.Add("groupCode", new JValue(groupCode));
            var result = ApiCommonUpdate(apiUrl + "Solution/S036/F212/WDelivBarcodeErrorCheck", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "エラーチェック時に");
                return false;
            }

            return true;
        }

        #endregion  ＜実行前チェック END＞

        #region ＜メイン処理＞ 



        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// エラーチェック  納品書番号
        /// </summary>
        /// <param name="code">納品書番号</param>
        /// <returns>IsOk(True：エラーが無し False：エラーがある)、delivNum（納入数）</returns>
        private (bool IsOk, decimal delivNum) ErrorCheckCode(string code)
        {
            // 未入力時処理
            if (string.IsNullOrEmpty(code))
            {
                return (true, 0);
            }

            // 使用禁止文字
            var isOk = Check.HasSQLBanChar(code).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return (false, 0);
            }

            if (code.Length < 13)
            {
                return (true, 0);
            }

            // T_SHIPMENT_DATA
            var result1 = ApiCommonGet(apiUrl + "DeliveryNote/State/" + code, null, true);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "T_SHIPMENT_DATA検索時");
                return (false, 0);
            }

            if (result1.Obj == null || result1.Count <= 0)
            {
                ChangeTopMessage("W0002", "納品書番号(" + code + ")", "T_SHIPMENT_DATA");
                return (false, 0);
            }

            var state = result1.Obj["state"].ToString().TrimEnd();
            if (state == "2")
            {
                ChangeTopMessage(1, "WARN", "納品書番号(" + code + ")は既に取込済みです");
                return (false, 0);
            }

            var poCode = result1.Obj["orderCode"].ToString().TrimEnd();
            var doCode = result1.Obj["code"].ToString().TrimEnd();
            var skdCode = result1.Obj["code"].ToString().TrimEnd();
            var num = decimal.Parse(result1.Obj["remain"].ToString().TrimEnd());

            if ((poCode == "") || (skdCode == "") || (doCode == ""))
            {
                ChangeTopMessage("W0007", "納品書番号(" + code + ")の注文番号と伝票番号と日程番号のいずれか");
                return (false, 0);
            }

            if (num == 0m)
            {
                ChangeTopMessage("W0016", "納品書番号(" + code + ")の納入数には数値0");
                return (false, 0);
            }

            // 発注明細
            apiParam.RemoveAll();
            apiParam.Add("poCode", new JValue(poCode));
            var result2 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetOrdDetailMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注明細マスタ検索時に");
                return (false, 0);
            }
            if (result2.Table == null || result2.Count <= 0)
            {
                ChangeTopMessage("W0002", "納品書番号(" + code + ")", "発注明細マスタ");
                return (false, 0);
            }

            var count1 = decimal.Parse(result2.Table.Rows[0]["count"].ToString());
            if (count1 <= 0m)
            {
                ChangeTopMessage("W0002", "納品書番号(" + code + ")", "発注明細マスタ");
                return (false, 0);
            }

            // M_DelivSlip
            apiParam.RemoveAll();
            apiParam.Add("doCode", new JValue(doCode));
            var result3 = ApiCommonGet(apiUrl + "Solution/S036/F212/GetDelivSlipFile", apiParam);
            if (result3.IsOk == false)
            {
                ChangeTopMessage("E0008", "納品書ファイル検索時に");
                return (false, 0);
            }
            if (result3.Table == null || result3.Count <= 0)
            {
                ChangeTopMessage("W0002", "納品書番号(" + code + ")", "納品書ファイル");
                return (false, 0);
            }

            var count2 = decimal.Parse(result3.Table.Rows[0]["count"].ToString());
            if (count2 >= 1m)
            {
                ChangeTopMessage(1, "WARN", "納品書番号(" + code + ")は既に取込済みです");
                return (false, 0);
            }

            return (true, num);
        }

        /// <summary>
        /// 進捗表示
        /// </summary>
        private void DisplayStatus()
        {
            // 変数表
            apiParam.RemoveAll();
            apiParam.Add("cate", new JValue("納入受付"));
            var result = ApiCommonGet(apiUrl + "Solution/S036/F212/GetVariableMst", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "変数表検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "変数表");
                return;
            }

            var v1 = result.Table.Rows[0]["content1"].ToString();
            var v2 = result.Table.Rows[0].Field<double?>("content2") ?? 0d;
            var v3 = result.Table.Rows[0]["content3"].ToString();

            if (v2 == 0d) 
            {
                status1C1TextBox.Text = "";
                status2C1TextBox.Text = "";
                status3C1TextBox.Text = "";
            }
            else
            {
                status1C1TextBox.Text = "作成中";
                status2C1TextBox.Text = v2 + v1 + "の実行終了時間";
                status3C1TextBox.Text = v3;
            }
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0], エラーメッセージ)</returns>
        private (bool IsOk, int Count, string Msg) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
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
                        return (false, 0, (string)(result["msg"]));
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, (string)(result["msg"]));
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
                return (false, 0, (string)(result["msg"]));
            }

            return (
                true,
                (int)(result["count"]),
                ""
            );
        }

        /// <summary>
        /// WEBAPI側共通取得処理
        /// </summary>
        /// <param name="apiUrl">URL</param>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="isHttpGet">true：WEBAPIのメソッドが[HttpGet]、false：WEBAPIのメソッドが[HttpPost]</param>
        /// <returns>(isOk：実行成否、count：取得数、data：検索結果)</returns>
        private (bool IsOk, int Count, JObject Obj, DataTable Table) ApiCommonGet(string apiUrl,
            JObject apiParam = null, bool isHttpGet = false)
        {
            apiUrl += $"?sid={solutionIdShort}&fid={formIdShort}";
            JObject result;

            if (isHttpGet)
            {
                result = ControlAF.GetRequest(apiUrl, apiParam ?? (new JObject() { }), LoginInfo.Instance.Token);
            }
            else
            {
                var webApi = new WebAPI();
                result = webApi.PostRequest(apiUrl, apiParam ?? (new JObject() { }), LoginInfo.Instance.Token);
            }

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
                        return (false, 0, null, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, null, null);
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
                return (false, 0, null, null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, 0, null, null);
            }

            if (result["data"].Type.ToString() == "Array")
            {
                var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());

                return (
                    true,
                    (int)(result["count"]),
                    null,
                    table);
            }
            else
            {
                return (
                    true,
                    (int)(result["count"]),
                    (JObject)(result["data"]),
                    null);
            }
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

                if(fieldList != null && fieldList.Count() >= 1) 
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
                if(print.IsOk == false) 
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
                    var result = ApiCommonGet(apiUrl + "Solution/S036/F212/GetBOMMst", apiParam);
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
                    var result = ApiCommonGet(apiUrl + "Solution/S036/F212/GetManufactFile", apiParam);
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