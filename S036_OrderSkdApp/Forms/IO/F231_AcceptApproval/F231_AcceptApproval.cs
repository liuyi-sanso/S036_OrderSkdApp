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

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 受入承認画面
    /// </summary>
    public partial class F231_AcceptApproval : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F231/";
        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// メニューから引き継いだ部門コード
        /// </summary>
        private string argumentGroupCode = "";

        /// <summary>
        ///  c1TrueDBGrid用のDT 
        /// </summary>
        private DataTable c1TrueDBGridDT = new DataTable();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F231_AcceptApproval(string fId, string argumentGroupCode) : base(fId)
        {
            InitializeComponent();
            this.argumentGroupCode = argumentGroupCode;
            titleLabel.Text = "受入承認";

        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F231_AcceptApproval_Load(object sender, EventArgs e)
        {
            try
            {
                //C1TextBoxをリスト化
                AddControlListII(staffNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(drawingCodeC1TextBox, null, "", false, enumCate.無し);

                //C1CombBoxをリスト化
                AddControlListII(groupCodeC1ComboBox, groupCodeNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(staffIDC1ComboBox, staffNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(approvalNameC1ComboBox, null, "", true, enumCate.無し);

                //C1NumericEditをリスト化
                //AddControlListII(ltC1NumericEdit, null, "0", false, enumCate.無し);
                //AddControlListII(ltC1NumericEdit, null, null, false, enumCate.無し);

                //C1DateEditをリスト化
                AddControlListII(startDateC1DateEdit, null, DatetimeFC.GetBeginOfMonth(DateTime.Today).ToShortDateString(), true, enumCate.無し);
                AddControlListII(endDateC1DateEdit, null, DatetimeFC.GetEndOfMonth(DateTime.Today).ToShortDateString(), true, enumCate.無し);
                AddControlListII(startDate2C1DateEdit, null, DateTime.Today.AddYears(-1).ToString(), true, enumCate.無し);
                AddControlListII(endDate2C1DateEdit, null, DateTime.Today.ToString(), true, enumCate.無し);

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
                SetstaffIDC1ComboBox();
                SetapprovalNameC1ComboBox();
                SetgroupCodeC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "承認する対象データを選択し、実行（F10）を押してください。";

                // クリア処理
                DisplayClear();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        #endregion  ＜起動処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 承認者  コンボボックスセット
        /// </summary>
        private void SetstaffIDC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("StaffID", typeof(string));
            dt.Rows.Add("_", "すべて", "_");
            dt.Rows.Add("1321", "中木村 有一朗", "NAKAKIMURA-Y");
            dt.Rows.Add("1762", "長澤 英樹", "NAGASAWA-H");
            dt.CaseSensitive = true;
            SansoBaseControlAF.SetC1ComboBox(staffIDC1ComboBox, dt, staffIDC1ComboBox.Width,
                staffNameC1TextBox.Width, "ID", "NAME");

        }

        /// <summary>
        /// 承認者  コンボボックスセット
        /// </summary>
        private void SetapprovalNameC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("すべて", "_");
            dt.Rows.Add("未承認", "0");
            dt.Rows.Add("承認済", "1");
            dt.CaseSensitive = true;

            SansoBaseControlAF.SetC1ComboBox(approvalNameC1ComboBox, dt, approvalNameC1ComboBox.Width,
                approvalNameC1ComboBox.Width, "NAME", "NAME", true);
        }

        /// <summary>
        /// 課別コード  コンボボックスセット
        /// </summary>
        private void SetgroupCodeC1ComboBox()
        {
            apiParam.RemoveAll();
            var result = CallSansoWebAPI("POST", apiUrl + "GetGroupCode", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", " 課別コード抽出時に");
                return;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0017", "課別コード");
                return;
            }

            DataRow dr;
            dr = result.Table.NewRow();
            dr[0] = "_";
            dr[1] = "すべて";
            result.Table.Rows.InsertAt(dr, 0);

            result.Table.CaseSensitive = true;
            SansoBaseControlAF.SetC1ComboBox(groupCodeC1ComboBox, result.Table);
        }

        #endregion  ＜コンボボックス設定処理 END＞

        #region ＜クリア処理＞ 

        /// <summary>
        /// クリア処理
        /// </summary>
        private void DisplayClear()
        {
            if (c1TrueDBGridDT != null && c1TrueDBGridDT.Rows.Count > 0)
            {
                var ck = c1TrueDBGridDT.AsEnumerable().Where(v => v.Field<string>("choice") == "True").Count();
                if (ck > 0) 
                {
                    var dialog = MessageBox.Show("登録されていないデータが残っています。" +
                                        "再検索すると登録されていないデータが削除されます。よろしいですか？"
                                         + Environment.NewLine,
                                         "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            // ファンクションキーの使用可否設定
            TopMenuEnable("F8", false);
            TopMenuEnable("F10", true);
            TopMenuEnable("F12", true);

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

            staffIDC1ComboBox.SelectedIndex = 0;
            approvalNameC1ComboBox.SelectedIndex = 1;
            groupCodeC1ComboBox.SelectedIndex = 0;

            c1TrueDBGrid.SetDataBinding(null, "", true);
            c1TrueDBGridDT = null;

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // 初期表示
            SearchProc();

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = startDateC1DateEdit;
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
                    case "productCodeC1TextBox":
                        productSearchBt_Click(sender, e);
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
        /// 機種コード検索ボタン押下時
        /// </summary>
        private void productSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F901_ProductMCommonSearch("F901_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        //STProductCode = form.row.Cells["機種コード"].Value.ToString();
                        //STProductName = form.row.Cells["機種名"].Value.ToString();

                        //productCodeC1TextBox.Text = form.row.Cells["機種コード"].Value.ToString();
                        //productNameC1TextBox.Text = form.row.Cells["機種名"].Value.ToString();
                    }
                }
                //ActiveControl = productCodeC1TextBox;
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
            finally
            {
                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;
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

        /// <summary>
        /// タイマー起動処理
        /// </summary>
        protected override void ActuationTimer(string timerName)
        {
            try
            {
                switch (timerName)
                {
                    case "sampleTimer1":
                        // イベント1
                        break;
                    case "sampleTimer2":
                        // イベント2
                        break;
                    case "sampleTimer3":
                        // イベント3
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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 全てにチェックを入れるボタンを押下
        /// </summary>
        private void allChoiceButton_Click(object sender, EventArgs e)
        {
            if (c1TrueDBGridDT == null)
            {
                return;
            }

            for (int i = 0; i < c1TrueDBGridDT.Rows.Count; i++)
            {
                this.c1TrueDBGrid[i, "選択"] = "True";
            }
        }

        /// <summary>
        /// 全てのチェックを外すボタンを押下
        /// </summary>
        private void clearChoiceButton_Click(object sender, EventArgs e)
        {
            if (c1TrueDBGridDT == null)
            {
                return;
            }

            for (int i = 0; i < c1TrueDBGridDT.Rows.Count; i++)
            {
                this.c1TrueDBGrid[i, "選択"] = "False";
            }
        }


        /// <summary>
        /// c1TrueDBGrid  行設定
        /// </summary>
        private void c1TrueDBGrid_FetchRowStyle(object sender, C1.Win.C1TrueDBGrid.FetchRowStyleEventArgs e)
        {
            try
            {
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;

                if (grid.RowCount == 0)
                {
                    return;
                }

                // チェック対象
                if (c1TrueDBGridDT.Rows[e.Row].Field<string>("choice") == "True")
                {
                    e.CellStyle.BackColor = Color.Aqua;
                    return;
                }
                else if (c1TrueDBGridDT.Rows[e.Row].Field<string>("approvalCate") == "1")
                {
                    e.CellStyle.BackColor = Color.LightGray;
                    return;
                }
                else
                {
                    // 処理なし
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 一覧表選択時チェック
        /// </summary>
        private void c1TrueDBGrid_BeforeColEdit(object sender, C1.Win.C1TrueDBGrid.BeforeColEditEventArgs e)
        {
            try
            {
                ClearTopMessage();

                int row = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;

                // 承認済チェック
                if ((c1TrueDBGridDT.Rows[row].Field<string>("approvalCate") == "1"))
                {
                    ChangeTopMessage(1, "ERR", "既に承認されているため、変更できません");
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
        /// テキストボックス共通検証時
        /// </summary>
        private void CommonC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckTextBox((C1TextBox)sender);
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
        /// エラーチェック テキストボックス
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckTextBox(C1TextBox t)
        {
            // 未入力時処理
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.HasBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 仕入先コード　検証中
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

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
        /// エラーチェック  仕入先コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSupCode()
        {
            // 未入力時処理
            var s = supCodeC1TextBox;
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

            // 仕入先マスタ
            apiParam.RemoveAll();
            apiParam.Add("supCode", new JValue(s.Text));
            var result1 = CallSansoWebAPI("POST", apiUrl + "GetMSup", apiParam);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ取得");
                return false;
            }

            if (result1.Table == null || result1.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "仕入先マスタ");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 仕入先コード検証後
        /// </summary>
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                supNameC1TextBox.Text = "";

                // 未入力時処理
                var s = supCodeC1TextBox;
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // 仕入先マスタ
                apiParam.RemoveAll();
                apiParam.Add("supCode", new JValue(s.Text));
                var result1 = CallSansoWebAPI("POST", apiUrl + "GetMSup", apiParam);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "仕入先マスタ取得");
                    return;
                }

                supNameC1TextBox.Text = result1.Table.Rows[0]["supName"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            try
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
                    return;
                }

                // 未更新データチェック
                if (c1TrueDBGridDT != null && c1TrueDBGridDT.Rows.Count > 0)
                {
                    var ck = c1TrueDBGridDT.AsEnumerable().Where(v => v.Field<string>("choice") == "True").Count();
                    if (ck > 0)
                    {
                        var dialog = MessageBox.Show("登録されていないデータが残っています。" +
                                            "再検索すると登録されていないデータが削除されます。よろしいですか？"
                                             + Environment.NewLine,
                                             "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);
                        if (dialog != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }

                SearchProc();
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
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        /// </summary>
        private bool RequirF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）整合性チェック
        /// </summary>
        /// <returns>True：整合性ＯＫ False：整合性を満たしていない項目がある</returns>
        private bool ConsisF08()
        {
            return true;
        }

        /// <summary>
        /// 印刷（F08）エラーチェック
        /// </summary>
        /// <returns>True：項目にエラー無し、ＯＫ　False：項目エラーがある、ＮＧ</returns>
        private bool ErrCKF08()
        {
            return true;
        }


        /// <summary>
        /// 実行（F10）必須チェック
        /// </summary>
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        private bool RequirF10()
        {
            //// 必須チェック
            //var control = controlListII?
            //              .Where(v => v.Required)
            //              .FirstOrDefault(v => string.IsNullOrEmpty(v.Control.Text));

            //if (control != null)
            //{
            //    var ctl = ((C1TextBox)control.Control);
            //    this.ActiveControl = ctl;
            //    ChangeTopMessage("W0007", ctl.Label.Text);
            //    return false;
            //}

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
            // 権限チェック
            var no = LoginInfo.Instance.UserNo; //職番
            var groupCode = LoginInfo.Instance.GroupCode; //所属部署

            var ck = false;

            if (groupCode == "3520")
            {
                if (   no == "1321"     // B許可
                    || no == "1762")    // K許可
                {
                    ck = true; // 承認
                }
                else
                {
                    ck = false;// 承認しません
                }
            }
            else if (groupCode == "3350") // 情シスは許可
            {
                    ck = true; // 承認
            }
            else
            {
                ck = false;// 承認しません
            }

            if (ck == false)
            {
                MessageBox.Show("承認する権限がありません。処理を中止します。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return false;
                // 承認しません
            }

            if (c1TrueDBGridDT == null || c1TrueDBGridDT.Rows.Count <= 0)
            {
                ChangeTopMessage("W0017", "承認する");
                return false;
            }

            var selectCount = c1TrueDBGridDT.AsEnumerable().Where(v => v.Field<string>("choice") == "True").Count();

            if (selectCount <= 0)
            {
                ChangeTopMessage("W0017", "承認する");
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

                DialogResult d = MessageBox.Show("受入承認を登録します。よろしいですか？", "登録確認", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (d != DialogResult.Yes)
                {
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                var selectDT = c1TrueDBGridDT.AsEnumerable().Where(v => v.Field<string>("choice") == "True").CopyToDataTable();

                var table = JsonConvert.SerializeObject(selectDT, Formatting.Indented);
                apiParam.RemoveAll();
                apiParam.Add("table", new JValue(table));
                var result = CallSansoWebAPI("POST", apiUrl + "UpdateApproval", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "受入承認登録時に");
                    return;
                }

                c1TrueDBGrid.SetDataBinding(null, "", true);
                c1TrueDBGridDT = null;

                SearchProc();
                ChangeTopMessage("I0001", "受入承認登録");
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
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {

            apiParam.RemoveAll();
            apiParam.Add("startDate", new JValue(DateTime.Parse(startDateC1DateEdit.Text)));
            apiParam.Add("endDate", new JValue(DateTime.Parse(endDateC1DateEdit.Text + " 23:59:59")));

            var result = CallSansoWebAPI("POST", apiUrl + "GetAcceptList", apiParam);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "受入一覧抽出時に");
                return;
            }

            if ((result.Table == null) || (result.Table.Rows.Count <= 0))
            {
                ChangeTopMessage("I0005");
                return;
            }


            var param = new List<(int ColumnsNum, string Format, int? Width)>();
            //param.Add((1, "", 2400));
            //param.Add((4, "yyyy/m/d;@", 1200));
            //param.Add((6, "yyyy/m/d;@", 1200));

            var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, result.Table);
            var result2 = cef.CreateSaveExcelFile(param);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "エクセルデータ検索時に");
                return;
            }

            if ((c1TrueDBGridDT != null) && (c1TrueDBGridDT.Rows.Count >= 1))
            {
                // 列名設定
                var table = c1TrueDBGridDT.Copy();

                table.Columns.Remove("choice");
                table.Columns.Remove("autoNo");
                table.Columns.Remove("stockCate");
                table.Columns.Remove("createStaffID");
                table.Columns.Remove("approvalStaffID");
                table.Columns.Remove("approvalCate");
                table.Columns.Remove("createDate");

                table.Columns["dataCate"].ColumnName = "データ区分";
                table.Columns["dataCateName"].ColumnName = "データ区分名";
                table.Columns["acceptDate"].ColumnName = "受払年月日";
                table.Columns["partsCode"].ColumnName = "部品コード";
                table.Columns["partsName"].ColumnName = "部品名";
                table.Columns["drawingCode"].ColumnName = "図面番号";
                table.Columns["supCode"].ColumnName = "仕入先コード";
                table.Columns["supName"].ColumnName = "仕入先名";
                table.Columns["poCode"].ColumnName = "注文番号";
                table.Columns["inNum"].ColumnName = "入庫数";
                table.Columns["outNum"].ColumnName = "出庫数";
                table.Columns["groupCode"].ColumnName = "依頼部門";
                table.Columns["doCode"].ColumnName = "伝票番号";
                table.Columns["createStaffName"].ColumnName = "検収者";
                table.Columns["approvalStaffName"].ColumnName = "承認者";
                table.Columns["approvalDate"].ColumnName = "承認日時";
                table.Columns["approvalCateName"].ColumnName = "承認区分";

                var cef2 = new CreateExcelFile(MainMenu.FileOutputPath,
                    titleLabel.Text + "明細一覧", table);
                var resultExcel = cef2.CreateSaveExcelFile(null);
                if (resultExcel.IsOk == false)
                {
                    ChangeTopMessage("E0008", "エクセルデータ検索時に");
                    return;
                }
            }
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        private void PrintProc()
        {
            // マウスカーソル待機状態
            Cursor = Cursors.WaitCursor;




            // マウスカーソル待機状態を解除
            Cursor = Cursors.Default;
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// 未発行データ検索
        /// </summary>
        private void SearchProc()
        {
            try
            {
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // クリア
                c1TrueDBGrid.SetDataBinding(null, "", true);
                c1TrueDBGridDT = null;

                apiParam.RemoveAll();
                apiParam.Add("startDate", new JValue(DateTime.Parse(startDateC1DateEdit.Text)));
                apiParam.Add("endDate", new JValue(DateTime.Parse(endDateC1DateEdit.Text + " 23:59:59")));
                apiParam.Add("startDate2", new JValue(DateTime.Parse(startDate2C1DateEdit.Text)));
                apiParam.Add("endDate2", new JValue(DateTime.Parse(endDate2C1DateEdit.Text + " 23:59:59")));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("drawingCode", new JValue(drawingCodeC1TextBox.Text));
                apiParam.Add("staffID", new JValue(staffIDC1ComboBox.SGetText(2)));
                apiParam.Add("approval", new JValue(approvalNameC1ComboBox.SGetText(1)));
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));

                var result = CallSansoWebAPI("POST", apiUrl + "GetAcceptApprovalList", apiParam);

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "承認リスト抽出時に");
                    return;
                }

                if ((result.Table == null) || (result.Table.Rows.Count <= 0))
                {
                    ChangeTopMessage("I0005");
                    return;
                }

                c1TrueDBGridDT = result.Table;
                c1TrueDBGrid.SetDataBinding(c1TrueDBGridDT, "", true);
                ChangeTopMessage("I0011", c1TrueDBGridDT.Rows.Count.ToString("#,##0"));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;
            }
        }
        #endregion  ＜その他処理 END＞



    }
}
