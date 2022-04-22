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
    /// 関係会社納入処理
    /// </summary>
    public partial class F211_CompanyDelivProcess : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F211/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 組立部門の変更前保管エリア
        /// </summary>
        private string stGroupCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F211_CompanyDelivProcess(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "関係会社納入処理";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F211_CompanyDelivProcess_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(groupCodeC1ComboBox, groupNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dateC1DateEdit, null, DateTime.Today.ToShortDateString(), true, enumCate.無し);
                AddControlListII(batchCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(sendGroupCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(batchCodeReprintC1TextBox, null, "", false, enumCate.無し);
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

                // コンボボックスセット、コンボボックスの内容をセットするメソッドは１コントロールずつに分ける
                SetGroupCodeC1ComboBox();

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後、「取込開始」→「納入受付更新」を押してください。　 " +
                    "「選択」ボタン押下後、該当行のデータがが画面に表示されます。　 \n" +
                    "「チェックリスト再発行」押下後、「買掛データチェックリスト（関係会社）」が印刷されます。";

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
            groupCodeC1ComboBox.SelectedIndex = -1;
            c1TrueDBGrid.SetDataBinding(null, "", true);
            stGroupCode = "";
            groupCodeC1ComboBox.Enabled = true;
            dateC1DateEdit.Enabled = true;
            batchCodeC1TextBox.Enabled = true;
            startButton.Enabled = true;
            notProcesseListButton.Enabled = false;
            errorListButton.Enabled = false;
            inPossibleListButton.Enabled = false;
            updateButton.Enabled = false;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = groupCodeC1ComboBox;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 組立部門  コンボボックスセット
        /// </summary>
        private void SetGroupCodeC1ComboBox()
        {
            // 部門マスタ
            var result = ApiCommonGet(apiUrl + "GetGroupComboList", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                groupCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(groupCodeC1ComboBox, result.Table, groupCodeC1ComboBox.Width,
                groupNameC1TextBox.Width, "groupCode", "groupName");
        }

        #endregion  ＜コンボボックス設定処理 END＞

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
        /// 組立部門　検証された後
        /// </summary>
        private void groupCodeC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stGroupCode == s.Text)
                {
                    return;
                }
                stGroupCode = s.Text;

                // クリア
                groupNameC1TextBox.Text = "";
                batchCodeC1TextBox.Text = "";
                sendGroupCodeC1TextBox.Text = "";
                c1TrueDBGrid.SetDataBinding(null, "", true);

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

                DrawC1TrueDBGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 組立部門  選択された後
        /// </summary>
        private void groupCodeC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stGroupCode == s.Text)
                {
                    return;
                }
                stGroupCode = s.Text;

                // クリア
                groupNameC1TextBox.Text = "";
                batchCodeC1TextBox.Text = "";
                sendGroupCodeC1TextBox.Text = "";
                c1TrueDBGrid.SetDataBinding(null, "", true);

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

                DrawC1TrueDBGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// バッチ番号　検証している
        /// </summary>
        private void batchCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック バッチ番号
                var isOk = ErrorCheckBatchCode();
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
        /// バッチ番号　検証された後
        /// </summary>
        private void batchCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // クリア
                sendGroupCodeC1TextBox.Text = "";

                // 未入力時処理
                var s = (C1TextBox)sender;
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                if (string.IsNullOrEmpty(groupCodeC1ComboBox.Text))
                {
                    return;
                }

                // パラメータ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(s.Text));

                // 送受信履歴ファイル
                var result = ApiCommonGet(apiUrl + "GetSendReceiveFile", apiParam);
                var table = result.Table.AsEnumerable().Where(v => (v.Field<double?>("receiveNum") ?? 0d) == 0d);

                sendGroupCodeC1TextBox.Text = table.First()["sendGroupCode"].ToString().Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 日付　検証している
        /// </summary>
        private void dateC1DateEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 日付
                var isOk = ErrorCheckDate();
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
        /// c1TrueDBGrid　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName != "選択")
                {
                    return;
                }

                batchCodeC1TextBox.Text= grid[row, "batchCode"].ToString().TrimEnd();
                sendGroupCodeC1TextBox.Text = grid[row, "sendGroupCode"].ToString().TrimEnd();
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
                case "選択":
                    e.Value = "選択";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 取込開始ボタン押下後、 Ｗ関係会社ファイルにデータインポート、送受信履歴ファイルを更新
        /// </summary>
        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

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

                // データ年月チェック
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeC1TextBox.Text));
                var result = ApiCommonGet(apiUrl + "GetYearMonthList", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage(1, "WARN", "このバッチ番号のデータの中に違う処理年月のデータが混ざっています。確認してください");
                    return;
                }

                if (sendGroupCodeC1TextBox.Text != "3730" 
                    && sendGroupCodeC1TextBox.Text != "3731" 
                    && sendGroupCodeC1TextBox.Text != "3732" 
                    && sendGroupCodeC1TextBox.Text != "4019") 
                {
                    ChangeTopMessage("W0015", sendGroupCodeC1TextBox.Label.Text);
                    return;
                }

                var dialog = MessageBox.Show("関係会社データを取込します、開始してもよいですか？",
                                             "データ登録の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    statusC1TextBox.Text = "取込は、キャンセルされました";
                    statusC1TextBox.Refresh();
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                (bool isOk, int count) updateResult;

                statusC1TextBox.Text = "データ登録中";
                statusC1TextBox.Refresh();

                // データインポート
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeC1TextBox.Text));
                apiParam.Add("sendGroupCode", new JValue(sendGroupCodeC1TextBox.Text));
                updateResult = ApiCommonUpdate(apiUrl + "SendReceiveDataImport", apiParam);
                if (updateResult.isOk == false)
                {
                    ChangeTopMessage("E0008", "データインポート時に");
                    return;
                }

                statusC1TextBox.Text += "\nデータ取込み完了　　取込件数：" + updateResult.count;
                statusC1TextBox.Refresh();

                groupCodeC1ComboBox.Enabled = false;
                dateC1DateEdit.Enabled = false;
                batchCodeC1TextBox.Enabled = false;
                startButton.Enabled = false;
                notProcesseListButton.Enabled = true;
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
        /// 未処理一覧（検収）ボタン押下後、Ｗ関係会社ファイルを修正できる
        /// </summary>
        private void notProcesseListButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // エラーチェック Ｗ関係会社ファイル
                var isOk = ErrorCheckWCompanyFile();
                if (isOk == false)
                {
                    return;
                }

                // 関係会社未処理一覧画面へ遷移
                var form = new F211_CompanyDelivNotProcessList("F211_CompanyDelivNotProcessList", groupCodeC1ComboBox.Text);
                form.Show();
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

                // エラーチェック Ｗ関係会社ファイル
                var isOk = ErrorCheckWCompanyFile();
                if (isOk == false)
                {
                    return;
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                var result = ApiCommonGet(apiUrl + "GetErrorList", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                var p = PrintReport("R023_WCompanyErrorList", result.Table);
                if (p == false)
                {
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

                if (string.IsNullOrEmpty(batchCodeC1TextBox.Text))
                {
                    ChangeTopMessage("W0007", batchCodeC1TextBox.Label.Text);
                    return;
                }

                // エラーチェック Ｗ関係会社ファイル
                var isOk = ErrorCheckWCompanyFile();
                if (isOk == false)
                {
                    return;
                }

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeC1TextBox.Text));
                var result = ApiCommonGet(apiUrl + "GetInPossibleList", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                var list = new Dictionary<string, string>();
                list.Add("テキスト_件数", result.Table.Rows.Count.ToString("#,##0"));

                var p = PrintReport("R024_WCompanyInPossibleList", result.Table, list);
                if (p == false)
                {
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

        /// <summary>
        /// 納入受付更新ボタン押下後、入出庫を更新
        /// </summary>
        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                if (RequirF10() == false)
                {
                    return;
                }

                var dialog = MessageBox.Show("関係会社納入受付を実行します、開始してもよいですか？",
                                                 "更新の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    statusC1TextBox.Text = "更新は、キャンセルされました";
                    statusC1TextBox.Refresh();
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // エラーチェック Ｗ関係会社ファイル
                var isOk = ErrorCheckWCompanyFile();
                if (isOk == false)
                {
                    return;
                }

                // ロック処理開始
                var v1 = new SansoBase.LockFile();
                v1.SelectStr = "*";
                v1.WhereColuList.Add((v1.LockID, "R002"));
                v1.WhereColuList.Add((v1.LockGroup, "9999"));
                var result1 = CommonAF.ExecutSelectSQL(v1);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "ロックファイル検索時に");
                    return;
                }
                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "ロックID", "ロックファイル");
                    return;
                }

                var lockCate = result1.Table.Rows[0]["ロック区分"].ToString().Trim();
                if (lockCate == "R")
                {
                    ChangeTopMessage("E0004");
                    return;
                }
                else
                {
                    var v2 = new SansoBase.LockFile();
                    v2.WhereColuList.Add((v2.LockID, "R002"));
                    v2.WhereColuList.Add((v2.LockGroup, "9999"));
                    v2.UpdateSetColuList.Add((v2.LockCate, "R"));
                    v2.UpdateSetColuList.Add((v2.MachineName, LoginInfo.Instance.MachineCode));
                    v2.UpdateSetColuList.Add((v2.AssemblyGroup, groupCodeC1ComboBox.Text));
                    var result2 = CommonAF.ExecutUpdateSQL(v2);
                    if (result2.IsOk == false)
                    {
                        ChangeTopMessage("E0001", "ロックファイル");
                        return;
                    }
                }

                statusC1TextBox.Text = "納入受付　処理開始";
                statusC1TextBox.Refresh();       

                // データ登録
                apiParam.RemoveAll();
                apiParam.Add("acceptDate", new JValue(dateC1DateEdit.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeC1TextBox.Text));              
                var result = ApiCommonUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "更新時に");
                    return;
                }

                statusC1TextBox.Text += "\n納入受付　処理完了";
                statusC1TextBox.Refresh();

                DisplayClear();
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

                try
                {
                    // ロック処理解除
                    UnlockProcessing();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// チェックリスト再発行ボタン押下後、チェックリスト再発行を印刷
        /// </summary>
        private void checkListReprintButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(groupCodeC1ComboBox.Text))
                {
                    ChangeTopMessage("W0007", groupCodeC1ComboBox.Label.Text);
                    return;
                }

                if (string.IsNullOrEmpty(batchCodeReprintC1TextBox.Text))
                {
                    ChangeTopMessage("W0007", batchCodeReprintC1TextBox.Label.Text);
                    return;
                }

                // 送受信履歴ファイル
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeReprintC1TextBox.Text));
                var result = ApiCommonGet(apiUrl + "GetSendReceiveFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", batchCodeReprintC1TextBox.Label.Text, "送受信履歴ファイル");
                    return;
                }

                var sendGroupCode = result.Table.Rows[0]["sendGroupCode"].ToString().Trim();

                // レポートデータ
                apiParam.RemoveAll();
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("batchCode", new JValue(batchCodeReprintC1TextBox.Text));
                apiParam.Add("sendGroupCode", new JValue(sendGroupCode));
                var result2 = ApiCommonGet(apiUrl + "GetReprintCheckList", apiParam);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                var list = new Dictionary<string, string>();
                list.Add("テキスト_件数", result2.Table.Rows.Count.ToString("#,##0"));

                var p = PrintReport("R024_WCompanyInPossibleList", result2.Table, list);
                if (p == false)
                {
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
            // エラーチェック バッチ番号
            var isOk1 = ErrorCheckBatchCode();
            if (isOk1 == false)
            {
                ActiveControl = batchCodeC1TextBox;
                return false;
            }

            // エラーチェック 日付
            var isOk2 = ErrorCheckDate();
            if (isOk2 == false)
            {
                ActiveControl = dateC1DateEdit;
                return false;
            }

            return true;
        }

        #endregion  ＜実行前チェック END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// c1TrueDBGrid　描画
        /// </summary>
        private void DrawC1TrueDBGrid()
        {
            // クリア
            c1TrueDBGrid.SetDataBinding(null, "", true);

            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));

            // 送受信履歴ファイル
            var result = ApiCommonGet(apiUrl + "GetSendReceiveNotProcessList", apiParam);
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

            c1TrueDBGrid.SetDataBinding(result.Table, "", true);
            ChangeTopMessage("I0011", result.Table.Rows.Count.ToString("#,###"));
        }

        /// <summary>
        /// エラーチェック  バッチ番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckBatchCode()
        {
            // 未入力時処理
            var s = batchCodeC1TextBox;
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

            if (string.IsNullOrEmpty(groupCodeC1ComboBox.Text))
            {
                return true;
            }

            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
            apiParam.Add("batchCode", new JValue(s.Text));

            // 送受信履歴ファイル
            var result = ApiCommonGet(apiUrl + "GetSendReceiveFile", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return false;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "送受信履歴ファイル");
                return false;
            }

            var table = result.Table.AsEnumerable().Where(v => (v.Field<double?>("receiveNum") ?? 0d) == 0d);

            if (table.Count() <= 0)
            {
                ChangeTopMessage(1, "WARN", "入力されたバッチ番号はすでに取込されています。");
                return false;
            }

            if (sendGroupCodeC1TextBox.Text == "") 
            {
                sendGroupCodeC1TextBox.Text = table.First()["sendGroupCode"].ToString().Trim();
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  日付
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDate()
        {
            // 未入力時処理
            var s = dateC1DateEdit;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            DateTime date = DateTime.Parse(s.Text);
            DateTime startDate = DateTime.Parse(executeDateValueLabel.Text.Trim() + "/01");
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 日付範囲チェック
            var isOk = Check.IsDateRange(date, startDate, endDate).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0016", "範囲外の日付");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  Ｗ関係会社ファイル
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckWCompanyFile()
        {
            // 未入力時処理
            var s = groupCodeC1ComboBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                ChangeTopMessage("W0007", s.Label.Text);
                return false;
            }

            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(s.Text));

            // Ｗ関係会社ファイルチェック
            var result = ApiCommonUpdate(apiUrl + "ErrorCheckWCompanyFile", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "Ｗ関係会社ファイルチェック時に");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ロック処理解除
        /// </summary>
        private void UnlockProcessing()
        {
            var v1 = new SansoBase.LockFile();
            v1.SelectStr = "*";
            v1.WhereColuList.Add((v1.LockID, "R002"));
            v1.WhereColuList.Add((v1.LockGroup, "9999"));
            var result1 = CommonAF.ExecutSelectSQL(v1);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "ロックファイル検索時に");
                return;
            }
            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "ロックID", "ロックファイル");
                return;
            }

            // ロック解除に変更
            var v2 = new SansoBase.LockFile();
            v2.WhereColuList.Add((v2.LockID, "R002"));
            v2.WhereColuList.Add((v2.LockGroup, "9999"));
            v2.UpdateSetColuList.Add((v2.LockCate, "U"));
            v2.UpdateSetColuList.Add((v2.MachineName, ""));
            v2.UpdateSetColuList.Add((v2.AssemblyGroup, ""));
            var result2 = CommonAF.ExecutUpdateSQL(v2);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0001", "ロックファイル");
                return;
            }
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0])</returns>
        private (bool IsOk, int Count) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
        {
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
        /// <returns>true:印刷成功　false：印刷失敗</returns>
        private bool PrintReport(string reportName, DataTable table, Dictionary<string, string> fieldList = null)
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

        #endregion  ＜その他処理 END＞
    }
}