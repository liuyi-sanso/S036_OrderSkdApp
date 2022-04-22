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
    /// Ｗ発注日程マスタメンテナンス
    /// </summary>
    public partial class F402_OrdMstMaint : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// 実行処理をするかどうかを判断する
        /// </summary>
        private bool isActionDone = false;

        /// <summary>
        /// メンテナンス中のスタッフID
        /// </summary>
        private string maintStaffID = "";

        /// <summary>
        /// メンテナンスを初めた日付
        /// </summary>
        private DateTime maintDate;

        /// <summary>
        /// 更新前テーブル
        /// </summary>
        private DataTable tableBefore = new DataTable();

        /// <summary>
        /// 更新後テーブル
        /// </summary>
        private DataTable tableAfter = new DataTable();

        /// <summary>
        /// DTCheckクラス(排他制御)　Ｗ発注日程マスタ
        /// </summary>
        DTCheck wOrdSkdMstDTCheck = new DTCheck();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F402_OrdMstMaint(string fId, string maintStaffID, DateTime maintDate) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "Ｗ発注日程マスタメンテナンス";
            this.maintStaffID = maintStaffID;
            this.maintDate = maintDate;
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F402_OrdMstMaint_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);

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
                defButtomMessage = "「検索」ボタン押下後、絞込んだＷ発注日程マスタのデータが表示されます。　　　　" +
                   "一覧の「指示日」と「必要数」(薄い黄色列)は直接変更できます。　\n" +
                   "「明細」ボタン押下後、「Ｗ発注日程明細問合せ」画面へ遷移します。　　　　" +
                   "実行（F10）ボタンを押すと一覧の情報が一括に更新されます。";

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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = partsCodeC1TextBox;

            // 初期設定
            DrawC1TrueDBGrid();
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
        /// 部品コード  検索ボタン押下時
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
        /// 仕入先コード検索ボタン押下時
        /// </summary>
        private void supSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_SupMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        supNameC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 部品コード　検証している
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
        /// 仕入先コード　検証している
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 仕入先コード
                var isOk = ErrorCheckSupCode();
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
        /// 検索ボタン押下後、絞り込み条件で画面の一覧を表示
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 変更前テーブルと変更後テーブルの差分を抽出
                DataTable dtDiff = new DataTable();
                var dr = tableAfter.AsEnumerable().Except(tableBefore.AsEnumerable(), DataRowComparer.Default);
                if (dr.Any())
                {
                    dtDiff = dr.CopyToDataTable();
                }
                if (dtDiff.Rows.Count >= 1)
                {
                    var dialog = MessageBox.Show("一覧の変更点がクリアされます。検索してよろしいですか？",
                                              "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                              MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // エラーチェック 部品コード
                var isOk1 = ErrorCheckPartsCode();
                if (isOk1 == false)
                {
                    ActiveControl = partsCodeC1TextBox;
                    return;
                }

                // エラーチェック 仕入先コード
                var isOk2 = ErrorCheckSupCode();
                if (isOk2 == false)
                {
                    ActiveControl = supCodeC1TextBox;
                    return;
                }

                // 描画
                DrawC1TrueDBGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                if (colName != "明細")
                {
                    return;
                }

                var instrCode = grid[row, "指示番号"].ToString();

                var af = new MW_OrdSkdDMstAF();
                var result = af.GetWOrdSkdDMstList(instrCode);
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

                // W発注日程明細問合せ画面へ
                var form = new F403_OrdMstDInfo("F403_OrdMstDInfo", instrCode);
                form.Show();
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
                case "明細":
                    e.Value = "明細";
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
                var field = e.Column.DataColumn.DataField;
                var text = e.Column.DataColumn.Text.TrimEnd();

                if (field != "指示日" && field != "必要数")
                {
                    ChangeTopMessage(1, "ERR", "（「指示日」と「必要数」（薄黄色列）以外は更新できません");
                    e.Cancel = true;
                    return;
                }

                // 未入力チェック
                if (text == "")
                {
                    ChangeTopMessage("W0007", field);
                    e.Cancel = true;
                    return;
                }

                if (field == "指示日")
                {
                    var isOk = ErrorCheckDelivDate(text, field);
                    if (isOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }

                    tableAfter.Rows[rowIndex][field] = DateTime.Parse(text);
                }
                else if (field == "必要数")
                {
                    var result = ErrorCheckDelivNum(text, grid[rowIndex, "単価"].ToString(), field);
                    if (result.IsOk == false)
                    {
                        e.Cancel = true;
                        return;
                    }
  
                    tableAfter.Rows[rowIndex][field] = decimal.Parse(text);
                    tableAfter.Rows[rowIndex]["金額"] = result.Price;
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
                ClearTopMessage();
                c1TrueDBGrid.SetDataBinding(tableAfter.Copy(), "", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// フォームを閉じようとした時の処理
        /// </summary>
        private void F402_OrdMstMaint_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isRunValidating = false;
                if (isActionDone == false)
                {
                    var dialog = MessageBox.Show("このまま閉じるとデータ消えるけど、閉じてよろしいですか？",
                                                     "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                     MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                // メンテナンス中のスタッフIDを更新
                var af = new MW_OrdSkdMstAF();
                var isOk = af.ClearMaintStaffID(maintStaffID, maintDate);
                if (isOk == false)
                {
                    ChangeTopMessage("E0008", "メンテナンス中のスタッフIDクリア時に");
                    e.Cancel = true;
                    return;
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

        #endregion  ＜イベント処理 END＞

        #region ＜メイン処理＞ 

        /// <summary>
        /// 実行処理
        /// </summary>
        private void ActionProc()
         {
            try
            {
                if (c1TrueDBGrid.EditActive)
                {
                    return;
                }

                isRunValidating = false;
                ClearTopMessage();

                // 排他制御  テーブル取得
                var af = new MW_OrdSkdMstAF();
                var result = af.GetWOrdSkdMstList(maintStaffID, maintDate, controlListII);
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

                // 排他制御
                var p = result.Table.Copy();
                p.Columns.Remove("指示日");
                p.Columns.Remove("必要数");
                var isOk1 = wOrdSkdMstDTCheck.CheckDT(p);
                if (isOk1 == false)
                {
                    ChangeTopMessage("E0004");
                    return;
                }

                // 変更前テーブルと変更後テーブルの差分を抽出
                DataTable dtDiff = new DataTable();
                var drDiff = tableAfter.AsEnumerable().Except(tableBefore.AsEnumerable(), DataRowComparer.Default);
                if (drDiff.Any())
                {
                    dtDiff = drDiff.CopyToDataTable();
                }
                if (dtDiff.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "更新");
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                var dialog = MessageBox.Show("一括更新してよろしいですか？",
                                             "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    return;
                }

                // データベース更新
                //var af = new MW_OrdSkdMstAF();
                var isOk2 = af.UpdateWOrdSkdMst(tableBefore, dtDiff);
                if (isOk2 == false)
                {
                    ChangeTopMessage("E0001", "Ｗ発注日程マスタ");
                    return;
                }

                // 画面クリア
                DisplayClear();
                
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0002", "Ｗ発注日程マスタ");
                isActionDone = true;
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
            tableBefore.Rows.Clear();
            tableAfter.Rows.Clear();

            var af = new MW_OrdSkdMstAF();
            var result = af.GetWOrdSkdMstList(maintStaffID, maintDate, controlListII);
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

            // 変更前テーブル　変更後テーブル　設定
            tableBefore = result.Table.Copy();
            tableAfter = result.Table.Copy();

            // 排他制御
            var p = result.Table.Copy();
            p.Columns.Remove("指示日");
            p.Columns.Remove("必要数");
            wOrdSkdMstDTCheck.SaveDT(p);

            ChangeTopMessage("I0011", tableAfter.Rows.Count.ToString("#,###"));
        }

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

            // 部品マスタ
            var af = new MW_OrdSkdMstAF();
            var result = af.GetPartsName(s.Text);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "Ｗ発注日程マスタ検索時に");
                return false;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "Ｗ発注日程マスタ");
                return false;
            }

            partsNameC1TextBox.Text = result.Table.Rows[0]["PartsName"].ToString();

            return true;
        }

        /// <summary>
        /// エラーチェック  仕入先コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSupCode()
        {
            supNameC1TextBox.Text = "";

            // 未入力時処理
            var s = supCodeC1TextBox;
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

            supNameC1TextBox.Text = result.Table.Rows[0]["仕入先名１"].ToString();

            return true;
        }

        /// <summary>
        /// エラーチェック  指示日
        /// </summary>
        /// <param name="dateTxt">指示日</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivDate(string dateTxt, string name)
        {
            // 未入力チェック
            if (dateTxt == "")
            {
                ChangeTopMessage("W0007", name);
                return false;
            }

            var chk = Check.IsDate(dateTxt);
            if (chk.Result == false)
            {
                ChangeTopMessage("W0019", chk.Msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  必要数
        /// </summary>
        /// <param name="numTxt">必要数</param>
        /// <param name="unitPriceTxt">単価</param>
        /// <param name="name">名前</param>
        /// <returns>True：エラーが無し False：エラーがある   Price：金額</returns>
        private (bool IsOk, decimal? Price) ErrorCheckDelivNum(string numTxt, string unitPriceTxt, string name)
        {
            // 未入力チェック
            if (numTxt == "")
            {
                ChangeTopMessage("W0007", name);
                return (false, null);
            }

            // 数値か
            var chk1 = Check.IsNumeric(numTxt);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", name + "には");
                return (false, null);
            }

            decimal value = decimal.Parse(numTxt);

            // 正数か
            if (value < 0m)
            {
                ChangeTopMessage("W0006", name);
                return (false, null);
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 9, 0);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", name + "の" + chk2.Msg);
                return (false, null);
            }

            // 金額チェック
            var unitPrice = decimal.Parse(string.IsNullOrEmpty(unitPriceTxt) ? "0" : unitPriceTxt);
            var price = decimal.Parse(numTxt) * unitPrice;

            //四捨五入
            price = (price > 0m) ? (((Int64)(System.Math.Abs(price) + 0.5m)) * 1m) :
                (((Int64)(System.Math.Abs(price) + 0.5m)) * (-1m));

            var chk3 = Check.IsPointNumberRange(price, 9, 0);
            if (chk3.Result == false)
            {
                ChangeTopMessage(1, "WARN", "金額（" + price + "）の" + chk3.Msg);
                return (false, null);
            }

            return (true, price);
        }

        #endregion  ＜その他処理 END＞
    }
}