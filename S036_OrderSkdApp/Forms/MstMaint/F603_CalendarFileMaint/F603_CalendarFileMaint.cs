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
    /// カレンダファイル作成
    /// </summary>
    public partial class F603_CalendarFileMaint : BaseForm
    {
        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F603_CalendarFileMaint(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "カレンダファイル作成";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F603_CalendarFileMaint_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(dayOffC1DateEdit, null, DateTime.Today.ToString("yyyy/MM/01"), true, enumCate.無し);
                AddControlListII(startDateC1C1TextBox, null, null, false, enumCate.無し);
                AddControlListII(endDateC1C1TextBox, null, null, false, enumCate.無し);

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

                // DefaultButtomMessageをセット
                defButtomMessage = "休日設定日を入力後に「追加」または「削除」ボタンを押してください。";

                // クリア処理
                DisplayClear();

                // 初期値設定
                startDateC1C1TextBox.Text = DateTime.Parse(dayOffC1DateEdit.Text).ToString("yyyy/MM") + "/01";
                endDateC1C1TextBox.Text = DateTime.Parse(startDateC1C1TextBox.Text).AddMonths(1)
                                                                                   .AddDays(-1).ToString("yyyy/MM/dd");

                // 一覧にデータを設定する処理
                SetDataBind();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = dayOffC1DateEdit;

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
        /// 選択した行の日付を休日設定日に代入する処理
        /// </summary>
        private void c1TrueDBGrid_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;

                // 空欄行の場合代入しない
                if (grid[row, "休日設定日"].ToString() == "")
                {
                    return;
                }

                dayOffC1DateEdit.Value = DateTime.Parse(grid[row, "休日設定日"].ToString()).ToString("yyyy/MM/dd");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// カレンダファイルを登録する処理
        /// </summary>
        private void insertBt_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                Cursor = Cursors.WaitCursor;

                // 日付空欄時処理
                var d = dayOffC1DateEdit;
                var isOk = DateTime.TryParse(d.Text, out var dt1);
                if (!isOk)
                {
                    ActiveControl = d;
                    ChangeTopMessage("W0013", "日付");
                    return;
                }

                // DB日付範囲外の時未処理
                DateTime valueDay = DateTime.Parse(d.Text);
                DateTime startDay = new DateTime(1753, 1, 1);
                DateTime endDay = new DateTime(9999, 12, 31);
                if (valueDay < startDay || valueDay > endDay)
                {
                    ActiveControl = d;
                    ChangeTopMessage("W0012", "日付", startDay.ToString(), endDay.ToString());
                    return;
                }

                // レコード重複の場合にメッセージを表示
                string strDayOff = DateTime.Parse(d.Text).ToString("yyyy/MM/dd");
                var af = new CalendarAF();
                var resultExist = af.ExistCheck(strDayOff);
                if (resultExist.IsOk == false)
                {
                    ChangeTopMessage("E0008", "読み込み時に");
                    return;
                }

                if (resultExist.Table.Rows.Count >= 1)
                {
                    ChangeTopMessage("W0008", "日付");
                    return;
                }

                var result = af.InsertCalendarFile(strDayOff);
                if (result == false)
                {
                    ChangeTopMessage("E0008", "カレンダファイル登録時に");
                    return;
                }

                // 一覧にデータを設定する処理
                SetDataBind();

                ActiveControl = d;

                ChangeTopMessage("I0001", "カレンダファイル");

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
        /// カレンダファイルを削除する処理
        /// </summary>
        private void deleteBt_Click(object sender, EventArgs e)
        {
            try
            {

                isRunValidating = false;
                ClearTopMessage();

                Cursor = Cursors.WaitCursor;

                // 日付空欄時処理
                var d = dayOffC1DateEdit;
                var isOk = DateTime.TryParse(d.Text, out var dt1);
                if (!isOk)
                {
                    ActiveControl = d;
                    ChangeTopMessage("W0013", "日付");
                    return;
                }

                // DB日付範囲外の時未処理
                DateTime valueDay = DateTime.Parse(d.Text);
                DateTime startDay = new DateTime(1753, 1, 1);
                DateTime endDay = new DateTime(9999, 12, 31);
                if (valueDay < startDay || valueDay > endDay)
                {
                    ActiveControl = d;
                    ChangeTopMessage("W0012", "日付", startDay.ToString(), endDay.ToString());
                    return;
                }

                // レコード重複の場合にメッセージを表示
                string strDayOff = DateTime.Parse(d.Text).ToString("yyyy/MM/dd");
                var af = new CalendarAF();
                var resultExist = af.ExistCheck(strDayOff);
                if (resultExist.IsOk == false)
                {
                    ChangeTopMessage("E0008", "読み込み時に");
                    return;
                }

                if (resultExist.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0017", "日付");
                    return;
                }

                var result = af.DeleteCalendarFile(strDayOff);
                if (result == false)
                {
                    ChangeTopMessage("E0007", "カレンダファイル");
                    return;
                }

                // 一覧にデータを設定する処理
                SetDataBind();

                ActiveControl = d;

                ChangeTopMessage("I0003", "カレンダファイル");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;

                isRunValidating = true;
            }
        }

        /// <summary>
        /// 期間、一覧を表示する処理
        /// </summary>
        private void dayOffC1DateEdit_ValueChanged(object sender, EventArgs e)
        {

            try
            {
                // 日付空欄時処理
                var d = (C1.Win.Calendar.C1DateEdit)sender;
                var isOk = DateTime.TryParse(d.Text, out var dt1);
                if (!isOk)
                {
                    return;
                }

                startDateC1C1TextBox.Text = DateTime.Parse(d.Text).ToString("yyyy/MM") + "/01";
                endDateC1C1TextBox.Text = DateTime.Parse(startDateC1C1TextBox.Text).AddMonths(1)
                                                                                   .AddDays(-1).ToString("yyyy/MM/dd");
                ClearTopMessage();

                // 一覧にデータを設定する処理
                SetDataBind();

                // ６ヶ月以上先の日付を編集してる場合にメッセージを表示
                var dt2 = DateTime.Now.AddMonths(6);

                if (dt1 >= dt2)
                {
                    ChangeTopMessage(1, "WARN", "６ヶ月以上先の日付を編集しています。注意してください。");
                }

                // 過去の日付を編集してる場合にメッセージを表示
                var dt3 = DateTime.Now.AddMonths(-1);
                if (dt1 < dt3)
                {
                    ChangeTopMessage(1, "WARN", "過去の日付を編集しています。注意してください。");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 期間を表示する処理
        /// </summary>
        private void dayOffC1DateEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                // 日付空欄時処理
                var d = (C1.Win.Calendar.C1DateEdit)sender;
                var isOk = DateTime.TryParse(d.Text, out var dt1);
                if (!isOk)
                {
                    return;
                }

                startDateC1C1TextBox.Text = DateTime.Parse(d.Text).ToString("yyyy/MM") + "/01";
                endDateC1C1TextBox.Text = DateTime.Parse(startDateC1C1TextBox.Text).AddMonths(1)
                                                                                   .AddDays(-1).ToString("yyyy/MM/dd");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        #endregion  ＜イベント処理 END＞


        #region ＜その他処理＞ 

        /// <summary>
        /// 一覧にデータを設定する処理
        /// </summary>
        private void SetDataBind()
        {

            try
            {
                isRunValidating = false;
                ClearTopMessage();

                c1TrueDBGrid.SetDataBinding(null, "", true);

                Cursor = Cursors.WaitCursor;

                string strDayOff = dayOffC1DateEdit.Text;

                // DB日付範囲外の時未処理
                DateTime valueDay = DateTime.Parse(strDayOff);
                DateTime startDay = new DateTime(1753, 1, 1);
                DateTime endDay = new DateTime(9999, 12, 31);
                if (valueDay < startDay || valueDay > endDay)
                {
                    return;
                }

                //休日設定日の月初日
                string st1 = DateTime.Parse(strDayOff).ToString("yyyy/MM") + "/01";
                //休日設定日の月末日
                string st2 = DateTime.Parse(st1).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                var af = new CalendarAF();
                var result = af.GetGridViewData(st1, st2);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "読み込み時に");
                    return;
                }

                if (result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0005");
                    return;
                }

                c1TrueDBGrid.SetDataBinding(result.Table, "", true);

                ChangeTopMessage("I0011", result.Table.Rows.Count.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;

                isRunValidating = true;
            }

        }

        #endregion  ＜その他処理 END＞

    }
}
