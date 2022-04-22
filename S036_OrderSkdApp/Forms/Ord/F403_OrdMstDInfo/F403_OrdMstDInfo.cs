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
    /// Ｗ発注日程明細マスタ問合せ
    /// </summary>
    public partial class F403_OrdMstDInfo : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// 指示番号
        /// </summary>
        private string instrCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        ///  <param name="instrCode">指示番号</param>
        public F403_OrdMstDInfo(string fId, string instrCode) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "Ｗ発注日程明細マスタ問合せ";
            this.instrCode = instrCode;
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F403_OrdMstDInfo_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(instrCodeC1TextBox, null, instrCode, true, enumCate.無し);

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
                defButtomMessage = "「F12」押下後、Excelを出力します。　　　　";

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
            TopMenuEnable("F12", true);
            TopMenuEnable("F6", false);

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

        #endregion  ＜共通イベント処理 END＞    

        #region ＜メイン処理＞ 

        /// <summary>
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                if ((excelDt == null) || (excelDt.Rows.Count == 0))
                {
                    ChangeTopMessage("I0007");
                    excelDt = null;
                    return;
                }

                var param = new List<(int ColumnsNum, string Format, int? Width)>();
                param.Add((10, "yyyy/MM/dd", 1200));
                param.Add((11, "#,##0", 1200));
                param.Add((14, "#,##0", 1200));
                param.Add((15, "#,###.000", 1200));
                param.Add((16, "#,###.000", 1200));
                param.Add((17, "#,##0", 1200));
                param.Add((18, "#,##0", 1200));
                param.Add((19, "#,##0", 1200));
                param.Add((24, "yyyy/MM/dd hh:mm:ss", null));
                param.Add((26, "yyyy/MM/dd hh:mm:ss", null));

                var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, excelDt);
                var result = cef.CreateSaveExcelFile(param);
                if (result.IsOk == false)
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
            excelDt = null;
            c1TrueDBGrid.SetDataBinding(null, "", true);
            c1TrueDBGrid.Columns["納入数量"].FooterText = "";
            c1TrueDBGrid.Columns["所要量"].FooterText = "";
            c1TrueDBGrid.Columns["実引当数"].FooterText = "";
            c1TrueDBGrid.Columns["予定引当数"].FooterText = "";

            var af = new MW_OrdSkdDMstAF();
            var result = af.GetWOrdSkdDMstList(instrCode);
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
            excelDt = result.Table;

            // 合計数計算
            c1TrueDBGrid.Columns["納入数量"].FooterText = result.Table.AsEnumerable()
                .Select(v => decimal.Parse(v["納入数量"].ToString())).Sum().ToString("#,##0");

            c1TrueDBGrid.Columns["所要量"].FooterText = result.Table.AsEnumerable()
                .Select(v => decimal.Parse(v["所要量"].ToString())).Sum().ToString("#,##0");

            c1TrueDBGrid.Columns["実引当数"].FooterText = result.Table.AsEnumerable()
                .Select(v => decimal.Parse(v["実引当数"].ToString())).Sum().ToString("#,##0");

            c1TrueDBGrid.Columns["予定引当数"].FooterText = result.Table.AsEnumerable()
                .Select(v => decimal.Parse(v["予定引当数"].ToString())).Sum().ToString("#,##0");

            ChangeTopMessage("I0011", result.Table.Rows.Count.ToString("#,###"));
        }

        #endregion  ＜その他処理 END＞
    }
}