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
    /// 納入予定一覧
    /// </summary>
    public partial class F002_DelivScheduleList : BaseFormSimple
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F002/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 一覧表示用日付配列
        /// </summary>
        private DateTime[] dateArray;

        /// <summary>
        /// 一覧表示用テーブル
        /// </summary>
        private DataTable table;

        /// <summary>
        /// 一覧表示更新用スイッチ
        /// </summary>
        private bool doUpdate = false;

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F002_DelivScheduleList(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "納入予定一覧";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F002_DelivScheduleList_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // DefaultButtomMessageをセット
                defButtomMessage = "";

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // 日付計算
                CalDate();

                // 一覧の表示列名を設定
                c1TrueDBGrid.Columns["num1"].Caption = dateArray[0].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate1"].Caption = dateArray[0].ToString("(ddd)");
                c1TrueDBGrid.Columns["num2"].Caption = dateArray[1].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate2"].Caption = dateArray[1].ToString("(ddd)");
                c1TrueDBGrid.Columns["num3"].Caption = dateArray[2].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate3"].Caption = dateArray[2].ToString("(ddd)");
                c1TrueDBGrid.Columns["num4"].Caption = dateArray[3].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate4"].Caption = dateArray[3].ToString("(ddd)");
                c1TrueDBGrid.Columns["num5"].Caption = dateArray[4].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate5"].Caption = dateArray[4].ToString("(ddd)");
                c1TrueDBGrid.Columns["num6"].Caption = dateArray[5].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate6"].Caption = dateArray[5].ToString("(ddd)");
                c1TrueDBGrid.Columns["num7"].Caption = dateArray[6].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate7"].Caption = dateArray[6].ToString("(ddd)");

                // 画面情報保存用データテーブル　設定
                table = new DataTable();
                table.CaseSensitive = true;
                table.Columns.Add("groupName", typeof(string));
                table.Columns.Add("item", typeof(string));
                table.Columns.Add("remainTotal", typeof(double));
                table.Columns.Add("num1", typeof(string));
                table.Columns.Add("rate1", typeof(double));
                table.Columns.Add("num2", typeof(string));
                table.Columns.Add("rate2", typeof(double));
                table.Columns.Add("num3", typeof(string));
                table.Columns.Add("rate3", typeof(double));
                table.Columns.Add("num4", typeof(string));
                table.Columns.Add("rate4", typeof(double));
                table.Columns.Add("num5", typeof(string));
                table.Columns.Add("rate5", typeof(double));
                table.Columns.Add("num6", typeof(string));
                table.Columns.Add("rate6", typeof(double));
                table.Columns.Add("num7", typeof(string));
                table.Columns.Add("rate7", typeof(double));

                // クリア処理
                DisplayClear();

                this.WindowState = FormWindowState.Maximized;

                // タイマー作成
                AddTimer("timer1", 40);

                // タイマー起動
                StartTimer(true, 1);
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

        #endregion  ＜起動処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            // ファンクションキーの使用可否設定
            SimpleTopMenuEnable("F6", true);
            TopMenuEnable("F6", true);

            SimpleTopMenuEnable("F12", true);
            TopMenuEnable("F12", true);

            // 初期設定
            DrawView();

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜共通イベント処理＞

        /// <summary>
        /// タイマー起動処理
        /// </summary>
        protected override void ActuationTimer(string timerName)
        {
            try
            {
                switch (timerName)
                {
                    case "timer1":
                        UpdateForm();
                        break;

                    default:
                        break;
                }
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
            finally
            {
                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;

                isRunValidating = true;
            }
        }

        /// <summary>
        /// Excelボタンクリック シンプル画面
        /// </summary>
        protected void F12BtSimple_Click(object sender, EventArgs e)
        {
            // Baseのイベントを起動
            F12Bt_Click(sender, e);
        }

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 画面の最大化の時、一覧のロケーション、サイズを変更する
        /// </summary>
        private void F002_DelivScheduleList_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState != FormWindowState.Maximized || table.Rows.Count == 0)
                {
                    c1TrueDBGrid.RowHeight = 20;
                    c1TrueDBGrid.Splits[0].ColumnCaptionHeight = 20;
                    c1TrueDBGrid.HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[2].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[4].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[6].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[8].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[10].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[12].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[14].HeadingStyle.Font = new Font("MS UI Gothic", 12F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[16].HeadingStyle.Font = new Font("MS UI Gothic", 12F);

                    var font1 = new Font("MS UI Gothic", 12F);
                    foreach (C1.Win.C1TrueDBGrid.C1DisplayColumn v in c1TrueDBGrid.Splits[0].DisplayColumns)
                    {
                        v.Style.Font = font1;
                    }

                    c1TrueDBGrid.Splits[0].DisplayColumns[0].Width = 95;
                    c1TrueDBGrid.Splits[0].DisplayColumns[1].Width = 38;

                    int w = c1TrueDBGrid.Width
                          - c1TrueDBGrid.Splits[0].DisplayColumns[0].Width
                          - c1TrueDBGrid.Splits[0].DisplayColumns[1].Width;

                    c1TrueDBGrid.Splits[0].DisplayColumns[2].Width = int.Parse(Math.Floor(w * 0.090d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[3].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[5].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[7].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[9].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[11].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[13].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[15].Width = int.Parse(Math.Floor(w * 0.082d).ToString());

                    c1TrueDBGrid.Splits[0].DisplayColumns[4].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[6].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[8].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[10].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[12].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[14].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[16].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                }
                else
                {
                    c1TrueDBGrid.RowHeight = (c1TrueDBGrid.Height - 50) / table.Rows.Count - 1;
                    c1TrueDBGrid.Splits[0].ColumnCaptionHeight = 50;
                    c1TrueDBGrid.HeadingStyle.Font = new Font("MS UI Gothic", 30F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[2].HeadingStyle.Font = new Font("MS UI Gothic", 20F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[4].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[6].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[8].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[10].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[12].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[14].HeadingStyle.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[16].HeadingStyle.Font = new Font("MS UI Gothic", 25F);

                    var font = new Font("MS UI Gothic", 18F);
                    foreach (C1.Win.C1TrueDBGrid.C1DisplayColumn v in c1TrueDBGrid.Splits[0].DisplayColumns)
                    {
                        v.Style.Font = font;
                    }

                    c1TrueDBGrid.Splits[0].DisplayColumns[0].Style.Font = new Font("MS UI Gothic", 25F);
                    c1TrueDBGrid.Splits[0].DisplayColumns[1].Style.Font = new Font("MS UI Gothic", 20F);

                    c1TrueDBGrid.Splits[0].DisplayColumns[0].Width = 200;
                    c1TrueDBGrid.Splits[0].DisplayColumns[1].Width = 60;

                    int w = c1TrueDBGrid.Width
                      - c1TrueDBGrid.Splits[0].DisplayColumns[0].Width
                      - c1TrueDBGrid.Splits[0].DisplayColumns[1].Width;

                    c1TrueDBGrid.Splits[0].DisplayColumns[2].Width = int.Parse(Math.Floor(w * 0.090d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[3].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[5].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[7].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[9].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[11].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[13].Width = int.Parse(Math.Floor(w * 0.082d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[15].Width = int.Parse(Math.Floor(w * 0.082d).ToString());

                    c1TrueDBGrid.Splits[0].DisplayColumns[4].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[6].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[8].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[10].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[12].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[14].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                    c1TrueDBGrid.Splits[0].DisplayColumns[16].Width = int.Parse(Math.Floor(w * 0.048d).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// フォームを閉じたときの処理
        /// </summary>
        private void F002_DelivScheduleList_FormClosed(object sender, FormClosedEventArgs e)
        {
            // タイマーをストップ
            StartTimer(false);
        }

        /// <summary>
        /// セルの値により、セルのスタイルを設定。
        /// </summary>
        /// <remark>セルの背景色</remark>
        /// <remark>レコード間でのみ境界線を強調表示</remark>
        private void c1TrueDBGrid_FetchCellStyle(object sender, C1.Win.C1TrueDBGrid.FetchCellStyleEventArgs e)
        {
            try
            {
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;

                if (grid.RowCount <= 0)
                {
                    return;
                }

                var colName = grid.Columns[e.Col].DataField;

                c1TrueDBGrid.Columns["num1"].Caption = dateArray[0].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate1"].Caption = dateArray[0].ToString("(ddd)");
                c1TrueDBGrid.Columns["num2"].Caption = dateArray[1].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate2"].Caption = dateArray[1].ToString("(ddd)");
                c1TrueDBGrid.Columns["num3"].Caption = dateArray[2].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate3"].Caption = dateArray[2].ToString("(ddd)");
                c1TrueDBGrid.Columns["num4"].Caption = dateArray[3].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate4"].Caption = dateArray[3].ToString("(ddd)");
                c1TrueDBGrid.Columns["num5"].Caption = dateArray[4].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate5"].Caption = dateArray[4].ToString("(ddd)");
                c1TrueDBGrid.Columns["num6"].Caption = dateArray[5].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate6"].Caption = dateArray[5].ToString("(ddd)");
                c1TrueDBGrid.Columns["num7"].Caption = dateArray[6].ToString("MM/dd");
                c1TrueDBGrid.Columns["rate7"].Caption = dateArray[6].ToString("(ddd)");

                decimal rate;

                switch (colName)
                {
                    case "rate1":

                        rate = decimal.Parse(grid[e.Row, "rate1"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate2":
                        rate = decimal.Parse(grid[e.Row, "rate2"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate3":
                        rate = decimal.Parse(grid[e.Row, "rate3"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate4":
                        rate = decimal.Parse(grid[e.Row, "rate4"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate5":
                        rate = decimal.Parse(grid[e.Row, "rate5"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate6":
                        rate = decimal.Parse(grid[e.Row, "rate6"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    case "rate7":
                        rate = decimal.Parse(grid[e.Row, "rate7"].ToString());
                        if (rate >= 1m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.Cyan;
                        }
                        else if (rate >= 0.75m)
                        {
                            e.CellStyle.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                        {
                            //処理なし
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion  ＜イベント処理 END＞

        #region ＜メイン処理＞ 

        /// <summary>
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {
            isRunValidating = false;
            ClearTopMessage();

            // マウスカーソル待機状態
            Cursor = Cursors.WaitCursor;

            if ((excelDt == null) || (excelDt.Rows.Count <= 0))
            {
                ChangeTopMessage("W0017");
                return;
            }

            // 列名設定
            var table = excelDt.Copy();
            table.Columns["groupName"].ColumnName = "部門";
            table.Columns["item"].ColumnName = " ";
            table.Columns["remainTotal"].ColumnName = "残合計";
            table.Columns["num1"].ColumnName = dateArray[0].ToString("MM/dd");
            table.Columns["rate1"].ColumnName = dateArray[0].ToString("(ddd)");
            table.Columns["num2"].ColumnName = dateArray[1].ToString("MM/dd");
            table.Columns["rate2"].ColumnName = dateArray[1].ToString("(ddd) ");
            table.Columns["num3"].ColumnName = dateArray[2].ToString("MM/dd");
            table.Columns["rate3"].ColumnName = dateArray[2].ToString("(ddd)  ");
            table.Columns["num4"].ColumnName = dateArray[3].ToString("MM/dd");
            table.Columns["rate4"].ColumnName = dateArray[3].ToString("(ddd)   ");
            table.Columns["num5"].ColumnName = dateArray[4].ToString("MM/dd");
            table.Columns["rate5"].ColumnName = dateArray[4].ToString("(ddd)    ");
            table.Columns["num6"].ColumnName = dateArray[5].ToString("MM/dd");
            table.Columns["rate6"].ColumnName = dateArray[5].ToString("(ddd)     ");
            table.Columns["num7"].ColumnName = dateArray[6].ToString("MM/dd");
            table.Columns["rate7"].ColumnName = dateArray[6].ToString("(ddd)      ");

            var param = new List<(int ColumnsNum, string Format, int? Width)>();
            param.Add((2, "#,##0", 1200));
            param.Add((4, "##0.00%", 1200));
            param.Add((6, "##0.00%", 1200));
            param.Add((8, "##0.00%", 1200));
            param.Add((10, "##0.00%", 1200));
            param.Add((12, "##0.00%", 1200));
            param.Add((14, "##0.00%", 1200));
            param.Add((16, "##0.00%", 1200));

            var cef = new CreateExcelFile(MainMenu.FileOutputPath,
                titleLabel.Text, table);
            var resultExcel = cef.CreateSaveExcelFile(param);
            if (resultExcel.IsOk == false)
            {
                ChangeTopMessage("E0008", "エクセルデータ検索時に");
                return;
            }
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// 一覧の日付を計算
        /// </summary>
        private void CalDate()
        {
            dateArray = new DateTime[7];

            // カレンダファイル
            var result = ApiCommonGet(apiUrl + "GetCalendarFile", null);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0017", "カレンダファイルには");
                return;
            }

            bool isDateExsit;
            DateTime dateTemp;
            dateArray[0] = DateTime.Today;
            dateArray[1] = DateTime.Today;
            dateArray[2] = DateTime.Today;
            dateArray[3] = DateTime.Today;
            dateArray[4] = DateTime.Today;
            dateArray[5] = DateTime.Today;
            dateArray[6] = DateTime.Today;

            // 当日より-1日
            isDateExsit = false;
            dateTemp = DateTime.Today;
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(-1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[1] = dateTemp;
            }

            // 当日より-2日
            isDateExsit = false;
            dateTemp = dateArray[1];
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(-1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[0] = dateTemp;
            }

            // 当日より+1日
            isDateExsit = false;
            dateTemp = DateTime.Today;
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[3] = dateTemp;
            }

            // 当日より+2日
            isDateExsit = false;
            dateTemp = dateArray[3];
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[4] = dateTemp;
            }

            // 当日より+3日
            isDateExsit = false;
            dateTemp = dateArray[4];
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[5] = dateTemp;
            }

            // 当日より+4日
            isDateExsit = false;
            dateTemp = dateArray[5];
            while (isDateExsit == false)
            {
                dateTemp = dateTemp.AddDays(1);
                isDateExsit = result.Table.AsEnumerable().Where(v => v.Field<DateTime>("date") == dateTemp).Any();
                dateArray[6] = dateTemp;
            }
        }

        /// <summary>
        /// 一覧を描画
        /// </summary>
        private void DrawView()
        {
            try
            {
                isRunValidating = false;

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // クリア
                c1TrueDBGrid.SetDataBinding(null, "", true);
                excelDt = null;
                doUpdate = false;
                table.Rows.Clear();

                // 部門コードリスト
                var result1 = ApiCommonGet(apiUrl + "GetGroupList", null);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                var tableList = new List<DataTable>();

                foreach (var v1 in dateArray)
                {
                    var tempTable = new DataTable();
                    tempTable.CaseSensitive = true;
                    tempTable.Columns.Add("groupCode", typeof(string));
                    tempTable.Columns.Add("item", typeof(string));
                    tempTable.Columns.Add("num", typeof(double));
                    tempTable.Columns.Add("numPlan", typeof(double));

                    foreach (DataRow v2 in result1.Table.Rows)
                    {
                        apiParam.RemoveAll();
                        apiParam.Add("date", new JValue(v1));
                        apiParam.Add("groupCode", new JValue(v2["groupCode"].ToString()));
                        var result2 = ApiCommonGet(apiUrl + "GetDelivScheduleByDateGroup", apiParam);
                        if (result2.IsOk == false)
                        {
                            ChangeTopMessage("E0008", "検索時に");
                            return;
                        }

                        var dr1 = tempTable.NewRow();
                        dr1["groupCode"] = v2["groupCode"].ToString();
                        dr1["item"] = "件数";

                        var dr2 = tempTable.NewRow();
                        dr2["groupCode"] = v2["groupCode"].ToString();
                        dr2["item"] = "数量";

                        if (result2.Table == null || result2.Table.Rows.Count <= 0)
                        {
                            dr1["num"] = 0d;
                            dr1["numPlan"] = 0d;

                            dr2["num"] = 0d;
                            dr2["numPlan"] = 0d;

                            tempTable.Rows.Add(dr1);
                            tempTable.Rows.Add(dr2);
                            continue;
                        }

                        dr1["num"] = double.Parse(result2.Table.AsEnumerable()
                            .Where(v => v.Field<double>("status") == 2).Count().ToString());
                        dr1["numPlan"] = double.Parse(result2.Table.AsEnumerable().Count().ToString());

                        dr2["num"] = result2.Table.AsEnumerable()
                            .Where(v => v.Field<double>("status") == 2).Sum(v => v.Field<double>("delivNum"));
                        dr2["numPlan"] = result2.Table.AsEnumerable().Sum(v => v.Field<double>("delivNum"));

                        tempTable.Rows.Add(dr1);
                        tempTable.Rows.Add(dr2);
                    }

                    tableList.Add(tempTable);
                }

                var dc1 = new DataColumn("item", typeof(string));
                dc1.DefaultValue = "件数";

                var dc2 = new DataColumn("remainTotal", typeof(double));
                dc2.DefaultValue = 0d;

                result1.Table.Columns.Add(dc1);
                result1.Table.Columns.Add(dc2);

                var tempGroupTable = result1.Table.Clone();
                
                foreach (DataRow v in result1.Table.Rows)
                {
                    // 残合計を計算（当日より-3日まで）
                    apiParam.RemoveAll();
                    apiParam.Add("date", new JValue(dateArray[0].AddDays(-1)));
                    apiParam.Add("groupCode", new JValue(v["groupCode"].ToString()));
                    var result3 = ApiCommonGet(apiUrl + "GetDelivRemainNumByDateGroup", apiParam);
                    if (result3.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "検索時に");
                        return;
                    }

                    var dr = tempGroupTable.NewRow();
                    dr["groupCode"] = v["groupCode"].ToString();
                    dr["groupName"] = v["groupName"].ToString();
                    dr["order"] = int.Parse(v["order"].ToString());
                    dr["item"] = "数量";

                    if (result3.Table == null || result3.Table.Rows.Count <= 0)
                    {
                        v["remainTotal"] = 0d;
                        dr["remainTotal"] = 0d;
                        tempGroupTable.Rows.Add(dr);
                        continue;
                    }

                    v["remainTotal"] = result3.Table.Rows[0].Field<double>("count");
                    dr["remainTotal"] = result3.Table.Rows[0].Field<double>("delivNum");
                    tempGroupTable.Rows.Add(dr);
                }

                DataTable groupTable = result1.Table.AsEnumerable().Union(tempGroupTable.AsEnumerable()).CopyToDataTable<DataRow>();

                var result4 = from row0 in groupTable.AsEnumerable()

                             join row1 in tableList[0].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row1.Field<string>("groupCode"), key2 = row1.Field<string>("item") }

                             join row2 in tableList[1].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row2.Field<string>("groupCode"), key2 = row2.Field<string>("item") }

                             join row3 in tableList[2].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row3.Field<string>("groupCode"), key2 = row3.Field<string>("item") }

                             join row4 in tableList[3].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row4.Field<string>("groupCode"), key2 = row4.Field<string>("item") }

                             join row5 in tableList[4].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row5.Field<string>("groupCode"), key2 = row5.Field<string>("item") }

                             join row6 in tableList[5].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row6.Field<string>("groupCode"), key2 = row6.Field<string>("item") }

                             join row7 in tableList[6].AsEnumerable()
                             on new { key1 = row0.Field<string>("groupCode"), key2 = row0.Field<string>("item") }
                             equals new { key1 = row7.Field<string>("groupCode"), key2 = row7.Field<string>("item") }

                             orderby int.Parse(row0["order"].ToString()), row0.Field<string>("groupCode"), row0.Field<string>("item")

                             select new
                             {
                                 groupCode = row0.Field<string>("groupCode"),
                                 groupName = (row0.Field<string>("item") == "数量") ? "" : (row0.Field<string>("groupName")),
                                 item = row0.Field<string>("item"),
                                 order = int.Parse(row0["order"].ToString()),
                                 remainTotal = row0.Field<double>("remainTotal"),
                                 num1 = row1.Field<double>("num") + "/" + row1.Field<double>("numPlan"),
                                 rate1 = (row1.Field<double>("num") == 0d) || (row1.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row1.Field<double>("num") / row1.Field<double>("numPlan")),
                                 num2 = row2.Field<double>("num") + "/" + row2.Field<double>("numPlan"),
                                 rate2 = (row2.Field<double>("num") == 0d) || (row2.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row2.Field<double>("num") / row2.Field<double>("numPlan")),
                                 num3 = row3.Field<double>("num") + "/" + row3.Field<double>("numPlan"),
                                 rate3 = (row3.Field<double>("num") == 0d) || (row3.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row3.Field<double>("num") / row3.Field<double>("numPlan")),
                                 num4 = row4.Field<double>("num") + "/" + row4.Field<double>("numPlan"),
                                 rate4 = (row4.Field<double>("num") == 0d) || (row4.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row4.Field<double>("num") / row4.Field<double>("numPlan")),
                                 num5 = row5.Field<double>("num") + "/" + row5.Field<double>("numPlan"),
                                 rate5 = (row5.Field<double>("num") == 0d) || (row5.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row5.Field<double>("num") / row5.Field<double>("numPlan")),
                                 num6 = row6.Field<double>("num") + "/" + row6.Field<double>("numPlan"),
                                 rate6 = (row6.Field<double>("num") == 0d) || (row6.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row6.Field<double>("num") / row6.Field<double>("numPlan")),
                                 num7 = row7.Field<double>("num") + "/" + row7.Field<double>("numPlan"),
                                 rate7 = (row7.Field<double>("num") == 0d) || (row7.Field<double>("numPlan") == 0d) ? 0d 
                                    : (row7.Field<double>("num") / row7.Field<double>("numPlan")),
                             };

                foreach (var v in result4)
                {
                    DataRow dr = table.NewRow();
                    dr["groupName"] = v.groupName;
                    dr["item"] = v.item;
                    dr["remainTotal"] = v.remainTotal;
                    dr["num1"] = v.num1;
                    dr["rate1"] = v.rate1;
                    dr["num2"] = v.num2;
                    dr["rate2"] = v.rate2;
                    dr["num3"] = v.num3;
                    dr["rate3"] = v.rate3;
                    dr["num4"] = v.num4;
                    dr["rate4"] = v.rate4;
                    dr["num5"] = v.num5;
                    dr["rate5"] = v.rate5;
                    dr["num6"] = v.num6;
                    dr["rate6"] = v.rate6;
                    dr["num7"] = v.num7;
                    dr["rate7"] = v.rate7;
                    table.Rows.Add(dr);
                }

                c1TrueDBGrid.SetDataBinding(table, "", true);
                excelDt = table.Copy();
                doUpdate = true;
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
        /// 画面更新処理
        /// </summary>
        private void UpdateForm()
        {
            if (doUpdate == false)
            {
                return;
            }

            // 一覧を描画
            DrawView();
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

        #endregion  ＜その他処理 END＞
    }
}