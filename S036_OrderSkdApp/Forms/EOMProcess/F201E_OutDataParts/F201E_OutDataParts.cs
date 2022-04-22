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
    /// 月末出庫処理（部品）
    /// </summary>
    public partial class F201E_OutDataParts : BaseForm
    {

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F201E_OutDataParts(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "月末出庫処理（部品）";
        }


        /// <summary>
        /// ロード処理
        /// </summary>
        private void F201E_OutDataParts_Load(object sender, EventArgs e)
        {
            try
            {
                // 月末
                IsEOMTitleBackColor = true;

                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1ComboBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(delivNumC1NumericEdit, null, "", true, enumCate.無し);
                var today = DateTime.Today;
                AddControlListII(delivDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(codeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1ComboBox, outDataCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(outDataCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthInNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthOutNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksAllC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumEOMC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthInNumEOMC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthOutNumEOMC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthStockNumEOMC1TextBox, null, "", false, enumCate.無し);


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
                SetOutDataCateC1ComboBox();
                SetStockCateC1ComboBox();
                SetSupCodeC1ComboBox();

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力後に実行（F10）を押してください。";

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
        /// 出庫区分　コンボボックスセット
        /// </summary>
        private void SetOutDataCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add("2", "部品出庫");
            dt.Rows.Add("3", "不良処分");
            dt.Rows.Add("4", "仕損品");
            dt.Rows.Add("9", "調整出庫");
            dt.Rows.Add("5", "部品ｵｰﾀﾞ出庫");
            dt.Rows.Add("6", "変更・改造");
            dt.Rows.Add("X", "実施棚卸調整");
            dt.Rows.Add("A", "設計変更");
            dt.Rows.Add("B", "部品切替");

            ControlAF.SetC1ComboBox(outDataCateC1ComboBox, dt, outDataCateC1ComboBox.Width,
                outDataCateNameC1TextBox.Width, "ID", "NAME", true);
        }

        /// <summary>
        /// 在庫P　コンボボックスセット
        /// </summary>
        private void SetStockCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Rows.Add(" ", "素材在庫");
            dt.Rows.Add("Z", "完成品在庫");

            ControlAF.SetC1ComboBox(stockCateC1ComboBox, dt, stockCateC1ComboBox.Width,
                stockCateNameC1TextBox.Width, "ID", "NAME", true);
        }

        /// <summary>
        /// 出庫先　コンボボックスセット
        /// </summary>
        private void SetSupCodeC1ComboBox()
        {
            var result = GetGroupManufactDirect();
            if (result.IsOk == false)
            {
                if (result.ReLogin == true)
                {
                    ShowLoginMessageBox();
                    return;
                }
                else
                {
                    ChangeTopMessage(1, "WARN", "出庫先検索 " + result.Msg);
                    return;
                }
            }

            var dt = result.Table;
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(supCodeC1ComboBox, dt);

        }


        #endregion  ＜コンボボックス設定処理 END＞

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

            supCodeC1ComboBox.SelectedIndex = 0;
            codeC1TextBox.Enabled = false;
            jyuyoyosokuCodeC1TextBox.Enabled = false;
            codeC1TextBox.MaxLength = 7;


            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = partsCodeC1TextBox;
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
                    dv.RowFilter = $"{dv.Table.Columns[1].ColumnName} = '{c.Text}' ";
                    if (dv.Count <= 0)
                    {
                        return isError();
                    }
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
        /// 部品検索ボタン押下時
        /// </summary>
        private void partsSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        partsCodeC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString().TrimEnd();
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
        /// 在庫情報設定
        /// </summary>
        private void stockCateC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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
                        //return;
                    }
                    else
                    {
                        listCtr.Text = "";
                        //return;
                    }
                }

                // ComboBoxの内容を名称テキストに反映
                dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ((C1ComboBox)SelectList[0].Control).Text + "' ";
                listCtr.Text = dv.ToTable().Rows[0][1].ToString();

                // 在庫情報設定
                SetStockInfo();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 在庫情報、処理年月設定
        /// </summary>
        private void supCodeC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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


                // 在庫P設定
                if (c.Text == "3623")
                {
                    stockCateC1ComboBox.SelectedIndex = 1;
                }

                // 課別コード設定
                groupCodeC1TextBox.Text = c.Text;
                groupNameC1TextBox.Text = listCtr.Text;

                // 在庫情報設定
                SetStockInfo();

                // 月末処理の処理年月設定
                var result = GetEOMExecuteDate();
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "部門マスタ検索 " + result.Msg);
                        return;
                    }
                }

                if (result.Table != null && result.Table.Rows.Count >= 1)
                {
                    executeDateValueLabel.Text = result.Table.Rows[0].Field<string>("executeDate") ?? "";
                    executeDateValueLabel.Visible = true;
                    executeDateLabel.Visible = true;

                    // 納入日付設定
                    var date = DateTime.Parse(executeDateValueLabel.Text);
                    DateTime endDate = SansoBase.DatetimeFC.GetEndOfMonth(date);
                    delivDateC1DateEdit.Text = endDate.ToString("yyyy/MM/dd");
                }
                else
                {
                    executeDateValueLabel.Visible = false;
                    executeDateLabel.Visible = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


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

                var isOk = PartsCodeErrorCheck();
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
        /// 部品コード　検証後
        /// </summary>
        private void partsCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                partsNameC1TextBox.Text = "";

                var t = (C1TextBox)sender;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                var param = new SansoBase.PartsMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, t.Text));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部品マスタ検索時に");
                    ActiveControl = t;
                    return;
                }
                partsNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";

                stockCateC1ComboBox.SelectedIndex = 1;
                dataCateC1TextBox.Text = "4";
                // 在庫情報設定
                SetStockInfo();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 出庫数量　検証時
        /// </summary>
        private void delivNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = DelivNumErrorCheck();
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
        /// 備考設定
        /// </summary>
        private void outDataCateC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

                // 備考番号、需要予測番号入力設定
                codeC1TextBox.Text = "";
                jyuyoyosokuCodeC1TextBox.Text = "";
                if (c.Text == "6")
                {
                    codeC1TextBox.Enabled = true;
                    codeC1TextBox.MaxLength = 7;
                    jyuyoyosokuCodeC1TextBox.Enabled = true;

                }
                else if (c.Text == "A")
                {
                    codeC1TextBox.Enabled = true;
                    codeC1TextBox.MaxLength = 5;
                    jyuyoyosokuCodeC1TextBox.Enabled = false;
                }
                else
                {
                    codeC1TextBox.Enabled = false;
                    jyuyoyosokuCodeC1TextBox.Enabled = false;

                }

                SetRemarks();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 備考　検証時
        /// </summary>
        private void remarksC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                if (outDataCateC1ComboBox.Text == "6")
                {
                    var isOk = RemarksErrorCheck(31);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }
                else if (outDataCateC1ComboBox.Text == "A")
                {
                    var isOk = RemarksErrorCheck(44);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    var isOk = RemarksErrorCheck(50);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 備考番号　検証時
        /// </summary>
        private void codeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                if (outDataCateC1ComboBox.Text == "6")
                {
                    var isOk = RemarksCodeErrorCheck(7);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }
                else if (outDataCateC1ComboBox.Text == "A")
                {
                    var isOk = RemarksCodeErrorCheck(5);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    var isOk = RemarksCodeErrorCheck(5);
                    if (isOk == false)
                    {
                        ActiveControl = (C1TextBox)sender;
                        e.Cancel = true;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 備考需要予測番号　検証時
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = RemarksJyuyoyosokuCodeErrorCheck();
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
        /// 備考　検証後
        /// </summary>
        private void remarksC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                SetRemarks();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 備考番号　検証後
        /// </summary>
        private void codeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                SetRemarks();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 備考需要予測番号　検証後
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                SetRemarks();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
            var isOk = DelivNumErrorCheck();
            if (isOk == false)
            {
                ActiveControl = delivNumC1NumericEdit;
                return false;
            }

            var d1 = entryDateC1DateEdit;
            var isOk1 = DateTime.TryParse(d1.Text, out var dt1);
            if (!isOk1)
            {
                ActiveControl = d1;
                ChangeTopMessage("W0007", "期間");
                return false;
            }

            var isOk2 = DelivDateErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = delivDateC1DateEdit;
                return false;
            }


            if (outDataCateC1ComboBox.Text == "6")
            {
                var isOk3 = RemarksErrorCheck(31);
                if (isOk3 == false)
                {
                    ActiveControl = remarksC1TextBox;
                    return false;
                }
            }
            else if (outDataCateC1ComboBox.Text == "A")
            {
                var isOk3 = RemarksErrorCheck(44);
                if (isOk3 == false)
                {
                    ActiveControl = remarksC1TextBox;
                    return false;
                }
            }
            else
            {
                var isOk3 = RemarksErrorCheck(50);
                if (isOk3 == false)
                {
                    ActiveControl = remarksC1TextBox;
                    return false;
                }
            }

            if (outDataCateC1ComboBox.Text == "6")
            {
                var isOk4 = RemarksCodeErrorCheck(7);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
                    return false;
                }
            }
            else if (outDataCateC1ComboBox.Text == "A")
            {
                var isOk4 = RemarksCodeErrorCheck(5);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
                    return false;
                }
            }
            else
            {
                var isOk4 = RemarksCodeErrorCheck(5);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
                    return false;
                }
            }

            SetRemarks();

            var isOk5 = RemarksJyuyoyosokuCodeErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
                return false;
            }

            var isOk6 = PartsCodeErrorCheck();
            if (isOk6 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            CancelEventArgs e1 = new CancelEventArgs();
            var isOk7 = IsOkComboBoxValidating(outDataCateC1ComboBox, e1);
            if (!isOk7)
            {
                ActiveControl = outDataCateC1ComboBox;
                return false;
            }

            CancelEventArgs e2 = new CancelEventArgs();
            var isOk8 = IsOkComboBoxValidating(stockCateC1ComboBox, e2);
            if (!isOk8)
            {
                ActiveControl = stockCateC1ComboBox;
                return false;
            }

            CancelEventArgs e3 = new CancelEventArgs();
            var isOk9 = IsOkComboBoxValidating(supCodeC1ComboBox, e3);
            if (!isOk9)
            {
                ActiveControl = supCodeC1ComboBox;
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

                Cursor = Cursors.WaitCursor;

                var result = UpdateIOFile(controlListII);
                if (result.IsOk == false)
                {
                    if (result.ReLogin == true)
                    {
                        ShowLoginMessageBox();
                        return;
                    }
                    else
                    {
                        ChangeTopMessage(1, "WARN", "入出庫ファイル更新 " + result.Msg);
                        return;
                    }
                }

                ActiveControl = partsCodeC1TextBox;
                DisplayClear();
                ChangeTopMessage("I0001", "出庫処理");

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


        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// 在庫情報設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetStockInfo()
        {
            try
            {

                stockInfoC1TextBox.Text = "";
                prevMonthStockNumC1TextBox.Text = "";
                thisMonthInNumC1TextBox.Text = "";
                thisMonthOutNumC1TextBox.Text = "";
                thisMonthStockNumC1TextBox.Text = "";
                prevMonthStockNumEOMC1TextBox.Text = "";
                thisMonthInNumEOMC1TextBox.Text = "";
                thisMonthOutNumEOMC1TextBox.Text = "";
                thisMonthStockNumEOMC1TextBox.Text = "";

                if (stockCateC1ComboBox.Text == "Z")
                {
                    stockInfoC1TextBox.Text = "完成品在庫";
                }
                else if (stockCateC1ComboBox.Text == " ")
                {
                    stockInfoC1TextBox.Text = "素材在庫";
                }
                else
                {
                    stockInfoC1TextBox.Text = "";
                }
                
                (bool IsOk, DataTable Table, string Sql) result;
                (bool IsOk, DataTable Table, string Sql) resultE;

                if (stockCateC1ComboBox.Text == "Z")
                {
                    // 在庫マスタ
                    var param = new StockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);

                    // 月末在庫マスタ
                    var paramE = new StockMstE();
                    paramE.SelectStr = "*";
                    paramE.WhereColuList.Add((paramE.PartsCode, partsCodeC1TextBox.Text));
                    paramE.WhereColuList.Add((paramE.GroupCode, groupCodeC1TextBox.Text));
                    paramE.SetDBName("製造調達");
                    resultE = CommonAF.ExecutSelectSQL(paramE);
                }
                else
                {
                    // 素材在庫マスタ
                    var param = new MaterialStockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);

                    // 月末素材在庫マスタ
                    var paramE = new MaterialStockMstE();
                    paramE.SelectStr = "*";
                    paramE.WhereColuList.Add((paramE.PartsCode, partsCodeC1TextBox.Text));
                    paramE.WhereColuList.Add((paramE.GroupCode, groupCodeC1TextBox.Text));
                    paramE.SetDBName("製造調達");
                    resultE = CommonAF.ExecutSelectSQL(paramE);
                }

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                    return false;
                }
                if (resultE.IsOk == false)
                {
                    ChangeTopMessage("E0008", "月末在庫マスタ/月末素材在庫マスタ検索時に");
                    return false;
                }
                
                if (result.Table.Rows.Count >= 1)
                {
                    // 当月にセット
                    prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
                    thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
                    thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
                    thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
                }

                if (resultE.Table.Rows.Count >= 1)
                {
                    // 月末にセット
                    prevMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["前残数量"].ToString().Replace(".000", "");
                    thisMonthInNumEOMC1TextBox.Text = resultE.Table.Rows[0]["入庫数量"].ToString().Replace(".000", "");
                    thisMonthOutNumEOMC1TextBox.Text = resultE.Table.Rows[0]["出庫数量"].ToString().Replace(".000", "");
                    thisMonthStockNumEOMC1TextBox.Text = resultE.Table.Rows[0]["当残数量"].ToString().Replace(".000", "");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }

        /// <summary>
        /// 備考設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetRemarks()
        {
            try
            {
                string s = "";
                if (outDataCateC1ComboBox.Text == "6")
                {
                    s = (codeC1TextBox.Text == "" ? "" : codeC1TextBox.Text + ":")
                        + (jyuyoyosokuCodeC1TextBox.Text == "" ? "" : jyuyoyosokuCodeC1TextBox.Text.Substring(0, 4) + "-" +
                         jyuyoyosokuCodeC1TextBox.Text.Substring(4, 6) + ":");

                }
                else if (outDataCateC1ComboBox.Text == "A")
                {
                    s = (codeC1TextBox.Text == "" ? "" : codeC1TextBox.Text + ":");

                }
                s += remarksC1TextBox.Text;

                remarksAllC1TextBox.Text = s;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;
        }

        /// <summary>
        /// 部品コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PartsCodeErrorCheck()
        {
            var t = partsCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var param = new SansoBase.PartsMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, t.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "部品マスタ");
                return false;
            }
            partsNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";

            return true;
        }


        /// <summary>
        /// 出庫数量チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool DelivNumErrorCheck()
        {
            var t = delivNumC1NumericEdit;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (t.Text == "0")
            {
                ChangeTopMessage("W0016", "出庫数に0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 出庫日付チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool DelivDateErrorCheck()
        {
            var d1 = delivDateC1DateEdit;
            var isOk1 = DateTime.TryParse(d1.Text, out var dt1);
            if (!isOk1)
            {
                ActiveControl = d1;
                ChangeTopMessage("W0007", "出庫日付");
                return false;
            }

            var executeDate = executeDateValueLabel.Text;
            var isOk2 = DateTime.TryParse(executeDate, out var dt2);
            if (!isOk2)
            {
                ChangeTopMessage("W0007", "処理日付");
                return false;
            }

            DateTime startDate = dt2;
            DateTime endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);

            if (Check.IsDateRange(dt1, startDate, endDate).Result == false)
            {
                ChangeTopMessage("W0016", "範囲外の日付");
                return false;
            }

            return true;

        }


        /// <summary>
        /// 備考チェック
        /// </summary>
        /// <param name="byteNum">最大バイト数</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool RemarksErrorCheck(int byteNum)
        {
            var t = remarksC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, _) = Check.IsByteRange(t.Text, 0, byteNum);
            if (isOK == false)
            {
                decimal byteNumZen = byteNum / 2;
                ChangeTopMessage("W0022", t.Label.Text, byteNum.ToString(), Math.Floor(byteNumZen).ToString());
                return false;
            }

            return true;

        }

        /// <summary>
        /// 備考番号チェック
        /// </summary>
        /// <param name="byteNum">最大バイト数</param>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool RemarksCodeErrorCheck(int byteNum)
        {
            var t = codeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, _) = Check.IsByteRange(t.Text, 0, byteNum);
            if (isOK == false)
            {
                decimal byteNumZen = byteNum / 2;
                ChangeTopMessage("W0022", t.Label.Text, byteNum.ToString(), Math.Floor(byteNumZen).ToString());
                return false;
            }

            return true;

        }

        /// <summary>
        /// 備考需要予測番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool RemarksJyuyoyosokuCodeErrorCheck()
        {
            var t = jyuyoyosokuCodeC1TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            int byteNum = 10;
            (bool isOK, _) = Check.IsByteRange(t.Text, 0, byteNum);
            if (isOK == false)
            {
                decimal byteNumZen = byteNum / 2;
                ChangeTopMessage("W0022", t.Label.Text, byteNum.ToString(), Math.Floor(byteNumZen).ToString());
                return false;
            }

            var result = SelectDBAF.GetJyuyoyosoku(t.Text.Replace("-", ""));
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "需要予測マスタ検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "需要予測マスタ");
                return false;
            }

            return true;

        }

        /// <summary>
        /// ログイン有効期限切れメッセージ表示
        /// </summary>
        private void ShowLoginMessageBox()
        {
            MessageBox.Show($"ログイン有効期限が切れていたため、処理が実行されていません。{Environment.NewLine}" +
                     $"再度、処理を実行してください",
                     "ログイン有効期限切れエラー",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
        }


        /// <summary>
        /// 入出庫ファイル、在庫マスタ、素材在庫マスタ更新（月末）
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        /// <returns>IsOk：エラーが無し：True　エラーがある：False</returns>
        /// <returns>再ログインしたかどうか：再ログインした場合:true</returns>
        public (bool IsOk, bool ReLogin, string Msg) UpdateIOFile(List<ControlParam> controlList)
        {
            var dataCate = controlList.SGetText("dataCateC1TextBox");
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var supCode = controlList.SGetText("supCodeC1ComboBox");
            var delivNum = controlList.SGetText("delivNumC1NumericEdit");
            var delivDate = controlList.SGetText("delivDateC1DateEdit");
            var groupCode = controlList.SGetText("groupCodeC1TextBox");
            var outDataCate = controlList.SGetText("outDataCateC1ComboBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");
            var remarks = controlList.SGetText("remarksC1TextBox");
            var code = controlList.SGetText("codeC1TextBox");
            var jyuyoyosokuCode = controlList.SGetText("jyuyoyosokuCodeC1TextBox");
            var remarksAll = controlList.SGetText("remarksAllC1TextBox");
            var password = LoginInfo.Instance.UserNo;
            var machineName = LoginInfo.Instance.MachineCode;
            var createDate = DateTime.Now.ToString();
            var createStaffCode = LoginInfo.Instance.UserNo;
            var createID = LoginInfo.Instance.UserId;

            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += "IOFile/CreateIOFileE";
            var token = LoginInfo.Instance.Token;

            JObject param = new JObject();
            param.Add("dbName", new JValue("製造調達"));
            param.Add("dataCate", new JValue(dataCate));
            param.Add("partsCode", new JValue(partsCode));
            param.Add("supCode", new JValue(supCode));
            param.Add("outNum", new JValue(delivNum));
            param.Add("unitPrice", new JValue("0"));
            param.Add("acceptDate", new JValue(delivDate));
            param.Add("groupCode", new JValue(groupCode));
            param.Add("outDataCate", new JValue(outDataCate));
            param.Add("stockCate", new JValue(stockCate));
            param.Add("remarks", new JValue(remarksAll));
            param.Add("password", new JValue(password));
            param.Add("machineName", new JValue(machineName));
            param.Add("createDate", new JValue(createDate));
            param.Add("createStaffCode", new JValue(createStaffCode));
            param.Add("createID", new JValue(createID));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "");
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, false, (string)result["msg"]);
            }

            return (true, false, "");
        }

        /// <summary>
        /// S1製造直接部門 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetGroupManufactDirect()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F201E/GetGroupManufactDirect?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            param.Add("userId", new JValue(LoginInfo.Instance.UserId.ToUpper() ?? ""));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }

        /// <summary>
        /// 処理年月 取得処理
        /// </summary>
        /// <returns>IsOk
        /// ReLogin：再ログイン要否
        /// Msg：エラーメッセージ
        /// Table：取得データ
        /// </returns>
        private (bool IsOk, bool ReLogin, string Msg, DataTable Table) GetEOMExecuteDate()
        {
            // 必要なパラメータ設定
            string url = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"];
            url += $"Solution/S036/F201E/GetEOMExecuteDate?sid={solutionIdShort}&fid={formIdShort}";

            JObject param = new JObject();
            var groupCode = controlListII.SGetText("groupCodeC1TextBox");
            param.Add("groupCode", new JValue(groupCode));

            var af = new WebAPI();
            var result = af.PostRequest(url, param, LoginInfo.Instance.Token);
            if ((result == null) || ((int)result["Status"] != (int)HttpStatusCode.OK) || ((bool)result["isOk"] == false))
            {
                if (result["reLogIn"] != null)
                {
                    if ((bool)result["reLogIn"])
                    {
                        return (false, true, "", null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                return (false, false, (string)result["msg"], null);
            }

            if (result["data"] == null || (int)(result["count"]) <= 0)
            {
                return (true, false, "", null);
            }

            var table = JsonConvert.DeserializeObject<DataTable>(((JArray)result["data"]).ToString());
            return (true, false, "", table);
        }

        #endregion  ＜その他処理 END＞

    }
}
