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
    /// 発注状況問合せ
    /// </summary>
    public partial class F405_OrderStatusInfo : BaseForm
    {

        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F405/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F405_OrderStatusInfo(string FID) : base(FID)
        {
            InitializeComponent();
            this.titleLabel.Text = "発注状況問合せ";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F405_OrderStatusInfo_Load(object sender, EventArgs e)
        {
            try
            {
                //C1TextBoxをリスト化
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(poCodeC1TextBox, null, string.Empty, false, enumCate.Key);
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, string.Empty, false, enumCate.Key);
                AddControlListII(partsNameC1TextBox, null, string.Empty, false, enumCate.Key);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, string.Empty, false, enumCate.Key);
                AddControlListII(supNameC1TextBox, null, string.Empty, false, enumCate.Key);

                //C1CombBoxをリスト化
                AddControlListII(groupC1ComboBox, groupNameC1TextBox, string.Empty, true, enumCate.無し);
                AddControlListII(completFlagC1ComboBox, null, string.Empty, true, enumCate.無し);
                AddControlListII(orderCateC1ComboBox, null, string.Empty, true, enumCate.無し);
                AddControlListII(periodicFigC1ComboBox, null, string.Empty, true, enumCate.無し);

                //C1NumericEditをリスト化
                //AddControlListII(ltC1NumericEdit, null, "0", false, enumCate.無し);
                //AddControlListII(ltC1NumericEdit, null, null, false, enumCate.無し);

                //C1DateEditをリスト化
                AddControlListII(startDateC1DateEdit, null, DatetimeFC.GetBeginOfMonth(DateTime.Today.AddMonths(-1)).ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(endDateC1DateEdit, null, DatetimeFC.GetEndOfMonth(DateTime.Today.AddMonths(2)).ToString("yyyy/MM/dd"), true, enumCate.無し);

                
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

                // コンボボックスセット、コンボボックスの内容をセットするメソッドは１コントロールずつに分ける
                SetGroupC1ComboBox();
                SetCompletFlagC1ComboBox();
                SetOrderCateC1ComboBox();
                SetPeriodicFigC1ComboBox();

                // DefaultButtomMessageをセット
                DefButtomMessage = "" +
                                   "";
                /*
                if (partsCode != "")
                {
                    // クリア処理２
                    DisplayClear2();
                    ComboBoxValidated(groupC1ComboBox, null);
                }
                else
                {
                    */
                    // クリア処理
                    DisplayClear();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜起動処理 END＞

        #region ＜コンボボックス設定処理＞ 
        /// <summary>
        /// 課別コードコンボボックスセット
        /// </summary>
        private void SetGroupC1ComboBox()
        {
            try
            {
                var dt = SelectDBAF.GetProcurementGroupCode().Table;
                dt.CaseSensitive = true;
                ControlAF.SetC1ComboBox(groupC1ComboBox, dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 完了フラッグコンボボックスセット
        /// </summary>
        private void SetCompletFlagC1ComboBox()
        {
            try
            {
                ControlAF.SetC1ComboBox(completFlagC1ComboBox, SetCompletFlag().Table, 0, 200, "NAME", "NAME", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 発注種別コンボボックスセット
        /// </summary>
        private void SetOrderCateC1ComboBox()
        {
            try
            {
                ControlAF.SetC1ComboBox(orderCateC1ComboBox, SetOrderCate().Table, 0, 200, "NAME", "NAME", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 都度フラグコンボボックスセット
        /// </summary>
        private void SetPeriodicFigC1ComboBox()
        {
            try
            {
                ControlAF.SetC1ComboBox(periodicFigC1ComboBox, SetPeriodicFig().Table, 0, 200, "NAME", "NAME", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 完了フラグコンボボックスの内容
        /// </summary>
        /// <returns></returns>

        public static (bool IsOk, DataTable Table) SetCompletFlag()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("すべて");
            dt.Rows.Add("残のみ");

            return (true, dt);
        }

        /// <summary>
        /// 発注種別コンボボックスの内容
        /// </summary>
        /// <returns></returns>

        public static (bool IsOk, DataTable Table) SetOrderCate()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("すべて");
            dt.Rows.Add("内示");
            dt.Rows.Add("確定");

            return (true, dt);
        }

        /// <summary>
        /// 都度フラグコンボボックスの内容
        /// </summary>
        /// <returns></returns>
        public static (bool IsOk, DataTable Table) SetPeriodicFig()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("すべて");
            dt.Rows.Add("都度発注のみ");
            dt.Rows.Add("都度以外");

            return (true, dt);
        }


        #endregion  ＜コンボボックス設定処理 END＞


        #region ＜クリア処理＞ 
        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            try
            {
                tabControl3.SelectedIndex = 0;

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

                //startDateC1DateEdit.Value = DateTime.Now;
                //startDateC1DateEdit.Value = DateTime.Today.AddYears(-1).AddDays(-DateTime.Today.Day + 1);
                //endDateC1DateEdit.Value = DateTime.Today.AddMonths(2).AddDays(-DateTime.Today.Day);

                // コンボボックス　初期値
                groupC1ComboBox.Text = "_";
                groupNameC1TextBox.Text = "すべて";
                completFlagC1ComboBox.Text = "すべて";
                orderCateC1ComboBox.Text = "すべて";
                periodicFigC1ComboBox.Text = "すべて";

                DB1C1CheckBox.Checked = true;
                DB2C1CheckBox.Checked = true;
                DB3C1CheckBox.Checked = false;
                DB4C1CheckBox.Checked = true;
                DB5C1CheckBox.Checked = false;
                DB6C1CheckBox.Checked = true;
                DB7C1CheckBox.Checked = true;
                DB8C1CheckBox.Checked = false;
                DB9C1CheckBox.Checked = false;
                DB10C1CheckBox.Checked = false;
                DB11C1CheckBox.Checked = true;
                DB12C1CheckBox.Checked = true;

                limitLabel1.Visible = false;
                limitLabel2.Visible = false;

                c1TrueDBGrid1.SetDataBinding(null, "", true);
                c1TrueDBGrid2.SetDataBinding(null, "", true);

                c1TrueDBGrid1.Columns["納入指示数"].FooterText = string.Empty;
                c1TrueDBGrid1.Columns["納入数"].FooterText = string.Empty;
                c1TrueDBGrid2.Columns["納入指示数"].FooterText = string.Empty;
                c1TrueDBGrid2.Columns["納入数"].FooterText = string.Empty;

                // トップメッセージクリア　
                ClearTopMessage();

                // ボトムメッセージに初期値設定　
                this.buttomMessageLabel.Text = DefButtomMessage;

                // エクセルファイル用DataTable
                EXCELdt = null;

                // フォームオープン時のアクティブコントロールを設定
                this.ActiveControl = jyuyoyosokuCodeC1TextBox;
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
                    case "jyuyoyosokuCodeC1TextBox":
                        jyuyoyosokuSearchBt_Click(sender, e);
                        break;

                    case "partsCodeC1TextBox":
                        partsSearchBt_Click(sender, e);
                        break;

                    case "supCodeC1TextBox":
                        cusSearchBtt_Click(sender, e);
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
        /// 需要予測番号検索
        /// </summary>
        private void jyuyoyosokuSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F904_SakubanCommonSearch("F904_SakubanCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        jyuyoyosokuCodeC1TextBox.Text = form.row.Cells["需要予測番号"].Value.ToString();
                    }
                }
                this.ActiveControl = jyuyoyosokuCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コード検索
        /// </summary>
        private void partsSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.partsCodeC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString();
                        this.partsNameC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                    }
                }
                this.ActiveControl = partsCodeC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 仕入先コード検索
        /// </summary>
        private void cusSearchBtt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        this.supNameC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
                    }
                }
                this.ActiveControl = supCodeC1TextBox;
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
                if (c1TrueDBGrid1.EditActive)
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

            // コントロールのTagの値を下部メッセージに表示する
            TextBox tgt = (TextBox)sender;
            if ((tgt.Tag ?? "").ToString() == "")
            {
                // DefaultButtomMessageをセットする
                this.buttomMessageLabel.Text = DefButtomMessage;
            }
            else
            {
                buttomMessageLabel.Text = (tgt.Tag ?? "").ToString();
            }
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
        /// <returns></returns>
        private bool CheckComboBoxListAll()
        {
            /*
            try
            {
                var SelectControlListII = ControlListII.Where(v => v.Control.GetType() == typeof(C1ComboBox) && v.SubControl != null);
                foreach (var v in SelectControlListII)
                {
                    if (ControlAF.CheckComboBoxList((C1ComboBox)v.Control, (C1TextBox)v.SubControl) == false)
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
            */
            tabControl3.SelectedIndex = 0;

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
                        return returnFlg;
                    }

                    if (ControlAF.CheckComboBoxList(ctl,
                        (v.SubControl != null ? ((C1TextBox)v.SubControl) : null)) == false)
                    {
                        ChangeTopMessage("W0013", ctl.Label.Text);
                        returnFlg = false;
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
        /// 一覧　行の描画処理
        /// </summary>
        private void c1TrueDBGrid_FetchRowStyle(object sender, C1.Win.C1TrueDBGrid.FetchRowStyleEventArgs e)
        {
            // 確定の場合
            if (c1TrueDBGrid1[e.Row, "内示/確定"].ToString() == "確定")
            {
                e.CellStyle.BackColor = Color.LightYellow;
                return;
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

            var isOk2 = ErrorCheckSupCode();
            if (isOk2 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            var isOk3 = ErrorCheckPoCode();
            if (isOk3 == false)
            {
                ActiveControl = poCodeC1TextBox;
                return false;
            }

            var isOk4 = ErrorCheckJyuyoyosokuCode();
            if (isOk4 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
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
                //int count = 0;

                ////需要予測番号,注文番号,部品コードが入力されている数をカウント
                //if (Check.IsNullOrEmpty(jyuyoyosokuCodeC1TextBox.Text).Result == false)
                //{
                //    count = count += 1;
                //}
                //if (Check.IsNullOrEmpty(poCodeC1TextBox.Text).Result == false)
                //{
                //    count = count += 1;
                //}
                //if (Check.IsNullOrEmpty(partsCodeC1TextBox.Text).Result == false)
                //{
                //    count = count += 1;
                //}

                ////需要予測番号,注文番号,部品コードは１つのみ
                //if (count >= 2)
                //{
                //    ChangeTopMessage(1, "WARN", "需要予測番号、注文番号、" +
                //        "部品コードの入力は1つのみとしてください");
                //    c1TrueDBGrid.SetDataBinding(null, "", true);
                //    return;
                //}
                ////需要予測番号,注文番号,部品コードは必ず一つは入力させる
                //else if (count == 0)
                //{
                //    ChangeTopMessage(1, "WARN", "需要予測番号、注文番号、" +
                //        "部品コードの内、必ず1つ入力してください");
                //    c1TrueDBGrid.SetDataBinding(null, "", true);
                //    return;
                //}

                
                //需要予測番号,注文番号,部品コード,仕入先コードは必ず一つは入力させる
                if (Check.IsNullOrEmpty(jyuyoyosokuCodeC1TextBox.Text).Result
                      && Check.IsNullOrEmpty(poCodeC1TextBox.Text).Result && Check.IsNullOrEmpty(partsCodeC1TextBox.Text).Result
                      && Check.IsNullOrEmpty(supCodeC1TextBox.Text).Result)
                {
                    ChangeTopMessage(1, "WARN", "需要予測番号、注文番号、" +
                        "部品コード、仕入先コードの内、必ず1つ入力してください");
                    c1TrueDBGrid1.SetDataBinding(null, "", true);
                    return;
                }
                
                
                this.isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                this.Cursor = Cursors.WaitCursor;


                List<string> DBList = new List<string>();
                if (DB1C1CheckBox.Checked) { DBList.Add("製造調達"); }
                if (DB2C1CheckBox.Checked) { DBList.Add("製造熊山"); }
                if (DB3C1CheckBox.Checked) { DBList.Add("製造関連"); }
                if (DB4C1CheckBox.Checked) { DBList.Add("機械二課"); }
                if (DB5C1CheckBox.Checked) { DBList.Add("機械管理"); }
                if (DB6C1CheckBox.Checked) { DBList.Add("成型管理"); }
                if (DB7C1CheckBox.Checked) { DBList.Add("ロータ管理"); }
                if (DB8C1CheckBox.Checked) { DBList.Add("播磨三相"); }
                if (DB9C1CheckBox.Checked) { DBList.Add("新宮プレス"); }
                if (DB10C1CheckBox.Checked) { DBList.Add("新宮ダイカスト"); }
                if (DB11C1CheckBox.Checked) { DBList.Add("資材システム"); }
                if (DB12C1CheckBox.Checked) { DBList.Add("資材システム岡山"); }

                c1TrueDBGrid1.SetDataBinding(null, "", true);
                c1TrueDBGrid2.SetDataBinding(null, "", true);
                c1TrueDBGrid1.Columns["納入指示数"].FooterText = string.Empty;
                c1TrueDBGrid1.Columns["納入数"].FooterText = string.Empty;
                c1TrueDBGrid2.Columns["納入指示数"].FooterText = string.Empty;
                c1TrueDBGrid2.Columns["納入数"].FooterText = string.Empty;
                EXCELdt = null;

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                int grid1Count = 0;
                int grid2Count = 0;
                foreach (var v in DBList)
                {
                    //c1TrueDBGrid1
                    apiParam.RemoveAll();
                    apiParam.Add("DBName", new JValue(v));
                    apiParam.Add("startDate", new JValue(DateTime.Parse(startDateC1DateEdit.Text)));
                    apiParam.Add("endDate", new JValue(DateTime.Parse(endDateC1DateEdit.Text)));
                    apiParam.Add("orderGroup", new JValue(groupC1ComboBox.Text));
                    apiParam.Add("completFlag", new JValue(completFlagC1ComboBox.Text));
                    apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                    apiParam.Add("poCode", new JValue(poCodeC1TextBox.Text));
                    apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                    apiParam.Add("orderCate", new JValue(orderCateC1ComboBox.Text));
                    apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                    apiParam.Add("periodicFig", new JValue(periodicFigC1ComboBox.Text));

                    var result = CallSansoWebAPI("POST", apiUrl + "GetOrderDetail", apiParam);

                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "発注明細検索時に");
                        return;
                    }

                    if (result.Table == null || result.Table.Rows.Count <= 0)
                    {
                        /*
                        ChangeTopMessage("I0007");
                        //c1TrueDBGrid.SetDataBinding(null, "", true);
                        c1TrueDBGrid.Columns["納入指示数"].FooterText = string.Empty;
                        c1TrueDBGrid.Columns["納入数"].FooterText = string.Empty;
                        EXCELdt = null;
                        return;
                        */
                    }
                    else
                    {
                        //DataTable dtMerge = result.Table.Copy();
                        //dt2.PrimaryKey = new DataColumn[] { dt2.Columns["poCode"] };
                        dt.Merge(result.Table);
                        if (result.Obj["msg"].ToString() != "")
                        {
                            if (int.Parse(result.Obj["msg"].ToString()) > 1000)
                            {
                                limitLabel1.Visible = true;
                            }
                            grid1Count += int.Parse(result.Obj["msg"].ToString());
                        }
                    }


                    //c1TrueDBGrid2
                    apiParam.RemoveAll();
                    apiParam.Add("DBName", new JValue(v));
                    apiParam.Add("startDate", new JValue(DateTime.Parse(startDateC1DateEdit.Text)));
                    apiParam.Add("endDate", new JValue(DateTime.Parse(endDateC1DateEdit.Text)));
                    apiParam.Add("orderGroup", new JValue(groupC1ComboBox.Text));
                    //apiParam.Add("completFlag", new JValue(completFlagC1ComboBox.Text));
                    //apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                    apiParam.Add("poCode", new JValue(poCodeC1TextBox.Text));
                    apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                    //apiParam.Add("orderCate", new JValue(orderCateC1ComboBox.Text));
                    apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                    apiParam.Add("periodicFig", new JValue(periodicFigC1ComboBox.Text));

                    var result2 = CallSansoWebAPI("POST", apiUrl + "GetOrderMst", apiParam);

                    if (result2.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "発注マスタ検索時に");
                        return;
                    }

                    if (result2.Table == null || result2.Table.Rows.Count <= 0)
                    {
                        /*
                        ChangeTopMessage("I0007");
                        //c1TrueDBGrid.SetDataBinding(null, "", true);
                        c1TrueDBGrid.Columns["納入指示数"].FooterText = string.Empty;
                        c1TrueDBGrid.Columns["納入数"].FooterText = string.Empty;
                        EXCELdt = null;
                        return;
                        */
                    }
                    else
                    {
                        //DataTable dtMerge = result.Table.Copy();
                        //dt2.PrimaryKey = new DataColumn[] { dt2.Columns["poCode"] };
                        dt2.Merge(result2.Table);
                        if (result2.Obj["msg"].ToString() != "")
                        {
                            if (int.Parse(result2.Obj["msg"].ToString()) > 1000)
                            {
                                limitLabel2.Visible = true;
                            }
                            grid2Count += int.Parse(result2.Obj["msg"].ToString());
                        }

                    }

                }

                //c1TrueDBGrid.SetDataBinding(result.Table, "", true);
                //EXCELdt = result.Table;
                c1TrueDBGrid1.SetDataBinding(dt, "", true);
                c1TrueDBGrid2.SetDataBinding(dt2, "", true);
                EXCELdt = dt;


                

                /*
                // 合計数計算
                decimal sum1 = 0;
                decimal sum2 = 0;
                for (int i = 0; i < EXCELdt.Rows.Count; i++)
                {
                    sum1 += EXCELdt.Rows[i].Field<decimal?>("delivInstNum") ?? 0m;
                    sum2 += EXCELdt.Rows[i].Field<decimal?>("delivNum") ?? 0m;
                }
                c1TrueDBGrid.Columns["納入指示数"].FooterText = sum1.ToString("#,##0");
                c1TrueDBGrid.Columns["納入数"].FooterText = sum2.ToString("#,##0");
                */

                c1TrueDBGrid1.Columns["納入指示日"].FooterText = "合計";
                c1TrueDBGrid1.Columns["納入指示数"].FooterText = dt.AsEnumerable()
                                .Select(v => decimal.Parse(v["delivInstNum"].ToString())).Sum().ToString("#,##0");
                c1TrueDBGrid1.Columns["納入数"].FooterText = dt.AsEnumerable()
                                .Select(v => decimal.Parse(v["delivNum"].ToString())).Sum().ToString("#,##0");

                c1TrueDBGrid2.Columns["納入指示日"].FooterText = "合計";
                c1TrueDBGrid2.Columns["納入指示数"].FooterText = dt2.AsEnumerable()
                                .Select(v => decimal.Parse(v["delivInstNum"].ToString())).Sum().ToString("#,##0");
                c1TrueDBGrid2.Columns["納入数"].FooterText = dt2.AsEnumerable()
                                .Select(v => decimal.Parse(v["delivNum"].ToString())).Sum().ToString("#,##0");

                ////需要予測番号が入力されていたら
                //if (Check.IsNullOrEmpty(jyuyoyosokuCodeC1TextBox.Text).Result == false)
                //{

                //    var result = af.GetJuyoInfo(jyuyoyosokuCodeC1TextBox.Text, DateTime.Parse(startDateC1DateEdit.Text), DateTime.Parse(endDateC1DateEdit.Text));
                //    if (result.isOk == false)
                //    {
                //        throw new Exception("処理時にエラーが発生しました");
                //    }
                //    if (result.table.Rows.Count < 1)
                //    {
                //        ChangeTopMessage("I0007");
                //        c1TrueDBGrid.SetDataBinding(null, "", true);
                //        c1TrueDBGrid.Columns["納入指示数"].FooterText = string.Empty;
                //        c1TrueDBGrid.Columns["納入数"].FooterText = string.Empty;
                //        EXCELdt = null;
                //        return;
                //    }

                //    var result2 = af.GetSeikoPartsProgressInfo(jyuyoyosokuCodeC1TextBox.Text);
                //    if (result2.isOk == false)
                //    {
                //        throw new Exception("処理時にエラーが発生しました");
                //    }
                //    if (result2.table.Rows.Count < 1)
                //    {
                //        ChangeTopMessage("I0007");
                //        c1TrueDBGrid2.SetDataBinding(null, "", true);
                //        //return;
                //    }

                //    c1TrueDBGrid.SetDataBinding(result.table, "", true);
                //    c1TrueDBGrid2.SetDataBinding(result2.table, "", true);
                //    EXCELdt = result.table;

                //    // 合計数計算
                //    decimal sum1 = 0;
                //    decimal sum2 = 0;
                //    for (int i = 0; i < EXCELdt.Rows.Count; i++)
                //    {
                //        sum1 += EXCELdt.Rows[i].Field<decimal?>("納入指示数") ?? 0m;
                //        sum2 += EXCELdt.Rows[i].Field<decimal?>("納入数") ?? 0m;
                //    }
                //    c1TrueDBGrid.Columns["納入指示数"].FooterText = sum1.ToString("#,##0");
                //    c1TrueDBGrid.Columns["納入数"].FooterText = sum2.ToString("#,##0");
                //}


                ////注文番号が入力されていたら
                //else if (Check.IsNullOrEmpty(poCodeC1TextBox.Text).Result == false)
                //{

                //    var result = af.GetPoInfo(poCodeC1TextBox.Text, DateTime.Parse(startDateC1DateEdit.Text), DateTime.Parse(endDateC1DateEdit.Text));
                //    if (result.isOk == false)
                //    {
                //        throw new Exception("処理時にエラーが発生しました");
                //    }
                //    if (result.table.Rows.Count < 1)
                //    {
                //        ChangeTopMessage("I0007");
                //        c1TrueDBGrid.SetDataBinding(null, "", true);
                //        EXCELdt = null;
                //        return;
                //    }

                //    c1TrueDBGrid.SetDataBinding(result.table, "", true);
                //    EXCELdt = result.table;

                //    // 合計数計算
                //    decimal sum1 = 0;
                //    decimal sum2 = 0;
                //    for (int i = 0; i < EXCELdt.Rows.Count; i++)
                //    {
                //        sum1 += EXCELdt.Rows[i].Field<decimal?>("納入指示数") ?? 0m;
                //        sum2 += EXCELdt.Rows[i].Field<decimal?>("納入数") ?? 0m;
                //    }
                //    c1TrueDBGrid.Columns["納入指示数"].FooterText = sum1.ToString("#,##0");
                //    c1TrueDBGrid.Columns["納入数"].FooterText = sum2.ToString("#,##0");
                //}

                ////部品コードが入力されていたら
                //else if (Check.IsNullOrEmpty(partsCodeC1TextBox.Text).Result == false)
                //{

                //    var result = af.GetPartsInfo(DateTime.Parse(startDateC1DateEdit.Text), DateTime.Parse(endDateC1DateEdit.Text),
                //                        groupC1ComboBox.Text, completFlagC1ComboBox.Text,
                //                        partsCodeC1TextBox.Text, orderCateC1ComboBox.Text,
                //                        supCodeC1TextBox.Text);

                //    if (result.isOk == false)
                //    {
                //        throw new Exception("処理時にエラーが発生しました");
                //    }
                //    if (result.table.Rows.Count < 1)
                //    {
                //        ChangeTopMessage("I0007");
                //        c1TrueDBGrid.SetDataBinding(null, "", true);
                //        EXCELdt = null;
                //        return;
                //    }

                //    c1TrueDBGrid.SetDataBinding(result.table, "", true);
                //    EXCELdt = result.table;

                //    // 合計数計算
                //    decimal sum1 = 0;
                //    decimal sum2 = 0;
                //    for (int i = 0; i < EXCELdt.Rows.Count; i++)
                //    {
                //        sum1 += EXCELdt.Rows[i].Field<decimal?>("納入指示数") ?? 0m;
                //        sum2 += EXCELdt.Rows[i].Field<decimal?>("納入数") ?? 0m;
                //    }
                //    c1TrueDBGrid.Columns["納入指示数"].FooterText = sum1.ToString("#,##0");
                //    c1TrueDBGrid.Columns["納入数"].FooterText = sum2.ToString("#,##0");
                //}

                //ChangeTopMessage("I0006");
                ChangeTopMessage("I0011", "発注明細：" + grid1Count.ToString() + "件、発注マスタ：" + grid2Count.ToString());

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
        private void EXCELProc()
        {
            /*
            try
            {
                if ((EXCELdt == null) || (EXCELdt.Rows.Count < 1))
                {
                    ChangeTopMessage("I0007");
                    EXCELdt = null;
                    return;
                }
                else
                {
                    // 不用項目の削除
                    EXCELdt.Columns.Remove("残数");
                    EXCELdt.Columns.Remove("残数Ｆ");
                    EXCELdt.Columns.Remove("作成日付1");
                    EXCELdt.Columns.Remove("残数１");
                    EXCELdt.Columns.Remove("発行部門");
                    EXCELdt.Columns.Remove("都度Ｆ");
                    EXCELdt.Columns.Remove("残数2");

                    var param = new List<(int ColumnsNum, string Format, int? Width)>();
                    param.Add((5, "yyyy/m/d;@", 1200));
                    param.Add((6, "#,##0;@", 1200));
                    param.Add((7, "#,##0;@", 1200));
                    param.Add((13, "yyyy/m/d;@", 1200));
                    param.Add((14, "yyyy/m/d hh:mm:ss;@", 2000));

                    var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, EXCELdt);
                    var result = cef.CreateSaveExcelFile(param);
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
            /*
            try
            {
                
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                this.Cursor = Cursors.WaitCursor;
                
                var af = new F007_OrderStatusInfoAF();
                var result = af.GetOrderDetailInfo(DateTime.Parse(startDateC1DateEdit.Text), DateTime.Parse(endDateC1DateEdit.Text),
                                   groupC1ComboBox.Text, completFlagC1ComboBox.Text, jyuyoyosokuCodeC1TextBox.Text, poCodeC1TextBox.Text,
                                   partsCodeC1TextBox.Text, orderCateC1ComboBox.Text, supCodeC1TextBox.Text, periodicFigC1ComboBox.Text);

                if (result.isOk == false)
                {
                    throw new Exception("処理時にエラーが発生しました");
                }
                if (result.table.Rows.Count < 1)
                {
                    ChangeTopMessage("W0017", "印刷");
                    return;
                }
                
                //var dt = result.table;
                var dt = excelDt;

                // 印刷処理 /////////////////////////////////
                // 発注状況問合せ
                using (var report = new C1.Win.FlexReport.C1FlexReport())
                {
                    report.Load(EXE_DIRECTORY + @"\Reports\OrderStatusInfoList.flxr", "OrderStatusInfoList");

                    // データソース設定
                    var ds = new C1.Win.FlexReport.DataSource
                    {
                        Name = " ",
                        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                        Recordset = dt
                    };
                    report.DataSources.Add(ds);
                    report.DataSourceName = ds.Name;

                    // フィールド値設定
                    // 検索条件を画面から転記
                    ((C1.Win.FlexReport.Field)report.Fields["partsCode"]).Text = partsCodeC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["partsName"]).Text = partsNameC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["drawingCode"]).Text = "";
                    ((C1.Win.FlexReport.Field)report.Fields["groupCode"]).Text = groupC1ComboBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["groupName"]).Text = groupNameC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["startDate"]).Text = startDateC1DateEdit.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["endDate"]).Text = endDateC1DateEdit.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["jyuyoyosokuCode"]).Text = jyuyoyosokuCodeC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["poCode"]).Text = poCodeC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["supCode"]).Text = supCodeC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["supName"]).Text = supNameC1TextBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["completFlag"]).Text = completFlagC1ComboBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["orderCate"]).Text = orderCateC1ComboBox.Text;
                    ((C1.Win.FlexReport.Field)report.Fields["periodicFig"]).Text = periodicFigC1ComboBox.Text;

                    // プレビュー印刷
                    report.Render();
                    using (var dialog = new C1.Win.FlexViewer.C1FlexViewerDialog())
                    {
                        dialog.DocumentSource = report;
                        dialog.ShowDialog();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                this.Cursor = Cursors.Default;

                isRunValidating = true;
            }
            */
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

            /*
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
            */

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

            /*
            apiParam.RemoveAll();
            apiParam.Add("supCode", new JValue(s.Text));

            var result = CallSansoWebAPI("POST", apiUrl + "GetSupInfo", apiParam);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return false;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "部門マスタまたは住所録マスタ");
                return false;
            }

            supNameC1TextBox.Text = result.Table.Rows[0]["SupName"].ToString();
            */

            return true;
        }


        /// <summary>
        /// エラーチェック  注文番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPoCode()
        {

            // 未入力時処理
            var s = poCodeC1TextBox;
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

            return true;
        }

        /// <summary>
        /// エラーチェック  需要予測番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckJyuyoyosokuCode()
        {

            // 未入力時処理
            var s = jyuyoyosokuCodeC1TextBox;
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

            return true;
        }

        #endregion  ＜その他処理 END＞

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
        /// 仕入先コード　検証時
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
        /// 注文番号　検証時
        /// </summary>
        private void poCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 注文番号
                var isOk = ErrorCheckPoCode();
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
        /// 需要予測番号　検証時
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // エラーチェック 需要予測番号
                var isOk = ErrorCheckJyuyoyosokuCode();
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
    }
}
