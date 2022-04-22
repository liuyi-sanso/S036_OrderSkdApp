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
using C1.Win.C1Input;
using System.Runtime.InteropServices;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 発注確定処理
    /// </summary>
    public partial class F401_OrdSourceProcess : BaseForm
    {
        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F401_OrdSourceProcess(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "発注確定処理";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F401_OrdSourceProcess_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(searchC1ComboBox, null, "", true, enumCate.無し);
                AddControlListII(searchCodeC1TextBox, searchNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(searchNameC1TextBox, null, "", false, enumCate.無し);

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
                SetSearchC1ComboBox();

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "「Ｗ発注日程マスタExcel」押下後、絞り込みで絞込んだデータをExcelに出力します。　　　" +
                    "「確定」ボタン押下後、加工日程マスタに絞込んだデータを登録します。　\n" +
                    "「Ｗ発注日程マスタメンテナンス」押下後、「Ｗ発注日程マスタメンテナンス」画面へ遷移します。　";

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
            searchC1ComboBox.SelectedIndex = 0;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = searchC1ComboBox;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 絞り込み  コンボボックスセット
        /// </summary>
        private void SetSearchC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("発注課別", "0");
            dt.Rows.Add("仕入先担当者", "1");
            dt.Rows.Add("仕入先コード", "2");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(searchC1ComboBox, dt, searchC1ComboBox.Width,
                searchC1ComboBox.Width, "NAME", "NAME", true);
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
            // エラーチェック コード
            var isOk = ErrorCheckSearchCode();
            if (isOk == false)
            {
                return false;
            }

            return true;
        }

        #endregion  ＜実行前チェック END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// コード　検証している
        /// </summary>
        private void searchCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック コード
                var isOk = ErrorCheckSearchCode();
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
        /// W日程Excelボタン押したら、絞り込みで絞込んだデータをExcelに出力
        /// </summary>
        private void excelButton_Click(object sender, EventArgs e)
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

                Cursor = Cursors.WaitCursor;

                // データ取得
                var af = new MW_OrdSkdMstAF();
                var result = af.GetWOrdSkdMst(searchC1ComboBox.SGetText(1), searchCodeC1TextBox.Text);
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

                var param = new List<(int ColumnsNum, string Format, int? Width)>();
                param.Add((6, "yyyy/MM/dd", null));
                param.Add((7, "#,###", null));
                param.Add((11, "#,###.##", null));
                param.Add((12, "#,###.##", null));
                param.Add((13, "#,###", null));
                param.Add((22, "yyyy/MM/dd hh:mm:ss", null));
                param.Add((24, "yyyy/MM/dd hh:mm:ss", null));
                param.Add((26, "yyyy/MM/dd hh:mm:ss", null));

                var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, result.Table);
                var result2 = cef.CreateSaveExcelFile(param);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "エクセルデータ検索時に");
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
        /// W発注日程マスタメンテナンスボタン押したら、F402画面へ遷移する
        /// </summary>
        private void maintButton_Click(object sender, EventArgs e)
        {
            try
            {
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

                // メンテナンス中のスタッフIDを取得
                var af = new MW_OrdSkdMstAF();
                var result = af.GetMaintStaffID(searchC1ComboBox.SGetText(1), searchCodeC1TextBox.Text);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "メンテナンス");
                    return;
                }

                foreach (DataRow v in result.Table.Rows)
                {
                    var id = v["MaintStaffID"].ToString().TrimEnd();

                    if (id != "")
                    {
                        var name = v["メンテ者名"].ToString().TrimEnd();
                        var date = v.Field<DateTime>("MaintDate").ToString("MM月dd日 H時mm分");
                        ChangeTopMessage(1, "WARN", name + "さんが" + date + " からメンテナンス中です。");
                        return;
                    }
                }

                // メンテナンス中のスタッフIDを更新
                var now = DateTime.Now;
                var isOk = af.UpdateMaintStaffID(searchC1ComboBox.SGetText(1), searchCodeC1TextBox.Text, now);
                if (isOk == false)
                {
                    ChangeTopMessage("E0008", "メンテナンス中のスタッフID更新時に");
                    return;
                }

                var form = new F402_OrdMstMaint("F402_OrdMstMaint", LoginInfo.Instance.UserId, now);
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 確定ボタン押したら、加工日程マスタへデータを追加
        /// </summary>
        private void confirmButton_Click(object sender, EventArgs e)
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

                Cursor = Cursors.WaitCursor;

                // 加工日程マスタへデータを追加
                var af = new M_BizSkdAF();
                var result = af.InsertMBizSkd(searchC1ComboBox.SGetText(1), searchCodeC1TextBox.Text);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "加工日程マスタ更新時に");
                    return;
                }

                if (result.Table.Rows[0][0].ToString() == "0")
                {
                    ChangeTopMessage("W0017", "追加");
                    return;
                }

                ChangeTopMessage("I0001", "加工日程マスタ");
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

        #region ＜その他処理＞ 

        /// <summary>
        /// エラーチェック  コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSearchCode()
        {
            searchNameC1TextBox.Text = "";

            // 未入力時処理
            var s = searchCodeC1TextBox;
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

            if (searchC1ComboBox.Text == "")
            {
                return true;
            }

            // 使用禁止文字
            var isOk2 = Check.HasSQLBanChar(searchC1ComboBox.Text).Result;
            if (isOk2 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            switch (searchC1ComboBox.SGetText(1))
            {
                // 発注課別
                case "0":
                    var result1 = SelectDBAF.GetSansoMainGroupMst(s.Text);
                    if (result1.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "部門マスタ検索時に");
                        return false;
                    }
                    if (result1.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0002", s.Label.Text, "部門マスタ");
                        return false;
                    }
                    searchNameC1TextBox.Text = result1.Table.Rows[0]["部門名"].ToString().TrimEnd();
                    break;

                // 仕入先担当者
                case "1":
                    break;

                // 仕入先コード
                case "2":
                    var param = new SansoBase.SupMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.SupCode, s.Text));
                    param.SetDBName("製造調達");
                    var result3 = CommonAF.ExecutSelectSQL(param);
                    if (result3.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                        return false;
                    }
                    if (result3.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0002", s.Label.Text, "仕入先マスタ");
                        return false;
                    }
                    searchNameC1TextBox.Text = result3.Table.Rows[0]["仕入先名１"].ToString().TrimEnd();
                    break;

                default:
                    ChangeTopMessage("W0013", searchC1ComboBox.Label.Text);
                    return false;
            }

            return true;
        }

        #endregion  ＜その他処理 END＞
    }
}