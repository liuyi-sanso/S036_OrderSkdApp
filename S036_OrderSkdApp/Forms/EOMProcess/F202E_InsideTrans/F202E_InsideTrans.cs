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
    /// 月末社内移行発行
    /// </summary>
    public partial class F202E_InsideTrans : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"]
                                + "Solution/S036/F202E/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 変更前 出庫先
        /// </summary>
        private string stSupCode = "";

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F202E_InsideTrans(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "月末社内移行発行";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F202E_InsideTrans_Load(object sender, EventArgs e)
        {
            try
            {
                // 月末
                IsEOMTitleBackColor = true;

                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outNumC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(unitPriceC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(outPriceC1TextBox, null, "", false, enumCate.無し);
                var today = DateTime.Today;
                AddControlListII(outDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1ComboBox, groupNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(ordRemainNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthInNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthOutNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthStockNumC1TextBox, null, "", false, enumCate.無し);
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
                SetStockCateC1ComboBox();
                SetGroupCodeC1ComboBox();

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
        /// 課別コード　コンボボックスセット
        /// </summary>
        private void SetGroupCodeC1ComboBox()
        {
            var userId = LoginInfo.Instance.UserId.ToUpper();
            var result = GetGroupComboListByUser(userId);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "課別コード検索時に");
                return;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(groupCodeC1ComboBox, result.Table);

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

            c1TrueDBGrid.SetDataBinding(null, "", true);
            groupCodeC1ComboBox.SelectedIndex = 0;
            SetGroupName();
            stSupCode = "";

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

                // 在庫情報設定
                SetStockInfo();

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

                // 在庫情報設定
                SetStockInfo();

                // 入出工程単価情報設定
                SetGridViewInfo();

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

                var isOk = JyuyoyosokuCodeErrorCheck();
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
        /// 需要予測番号　検証後
        /// </summary>
        private void jyuyoyosokuCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
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
        /// 出庫数量　検証時
        /// </summary>
        private void outNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = OutNumErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                //SetSumOutNum();
                SetOutPrice();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        

        /// <summary>
        /// 単価　検証時
        /// </summary>
        private void unitPriceC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = UnitPriceErrorCheck();
                if (isOk == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                SetOutPrice();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 伝票番号入力判断
        /// </summary>
        private void doCodeC1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (doCodeC1CheckBox.Checked == true)
            {
                doCodeC1TextBox.Text = "";
                doCodeC1TextBox.Enabled = true;
                doCodeC1TextBox.BackColor = Color.White;
                doCodeC1TextBox.BorderColor = Color.Red;
            }
            else
            {
                doCodeC1TextBox.Text = "";
                doCodeC1TextBox.Enabled = false;
                doCodeC1TextBox.Label.Enabled = true;
                doCodeC1TextBox.BackColor = Color.PeachPuff;
                doCodeC1TextBox.BorderColor = Color.DimGray;
            }
        }

        /// <summary>
        /// 処理年月設定
        /// </summary>
        private void groupCodeC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

                // 処理年月設定
                var af = new F202E_InsideTransAF();
                var result = af.GetExecuteDate(c.Text);
                if (!result.IsOk)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }

                if (result.Table.Rows.Count >= 1)
                {
                    executeDateValueLabel.Text = result.Table.Rows[0].Field<string>("月末処理年月") ?? "";
                    executeDateValueLabel.Visible = true;
                    executeDateLabel.Visible = true;
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
        /// 出庫先  検索ボタン押下時
        /// </summary>
        private void supSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_ProductMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        supNameC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
                        stSupCode = supCodeC1TextBox.Text;
                        ClearTopMessage();

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
        /// 出庫先　検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk1 = SupCodeErrorCheck();
                if (isOk1 == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                var isOk3 = PoCodeErrorCheck();
                if (isOk3 == false)
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
        /// 出庫先　検証後
        /// </summary>
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false
                    || stSupCode == t.Text)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                // 在庫P設定
                if (t.Text == "3623")
                {
                    stockCateC1ComboBox.SelectedIndex = 1;
                }

                // 在庫情報設定
                SetStockInfo();

                stSupCode = t.Text;

                var isOk = GetUnitPrice();
                if (isOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
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

                var isOk = PoCodeErrorCheck();
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
        /// 注文番号　検証後
        /// </summary>
        private void poCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
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
        /// 伝票番号　検証時
        /// </summary>
        private void doCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = DoCodeErrorCheck();
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
        /// 課別コード　検証時
        /// </summary>
        private void groupCodeC1ComboBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var isOk1 = SupCodeErrorCheck();
                if (isOk1 == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                // コンボボックスの共通Validating処理
                ComboBoxValidating(sender, e);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        /// <summary>
        /// 課別コード　検証後
        /// </summary>
        private void groupCodeC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                // 在庫情報設定
                SetStockInfo();

                // コンボボックスの共通Validating処理
                ComboBoxValidated(sender, e);

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

            var d1 = outDateC1DateEdit;
            var isOk1 = DateTime.TryParse(d1.Text, out var dt1);
            if (!isOk1)
            {
                ActiveControl = d1;
                ChangeTopMessage("W0007", "出庫日付");
                return false;
            }

            var isOk2 = OutDateErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = outDateC1DateEdit;
                return false;
            }

            var isOk3 = JyuyoyosokuCodeErrorCheck();
            if (isOk3 == false)
            {
                ActiveControl = jyuyoyosokuCodeC1TextBox;
                return false;
            }

            var isOk4 = PartsCodeErrorCheck();
            if (isOk4 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk5 = OutNumErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = outNumC1NumericEdit;
                return false;
            }

            var isOk7 = UnitPriceErrorCheck();
            if (isOk7 == false)
            {
                ActiveControl = unitPriceC1NumericEdit;
                return false;
            }

            var isOk8 = SupCodeErrorCheck();
            if (isOk8 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            var isOk9 = PoCodeErrorCheck();
            if (isOk9 == false)
            {
                ActiveControl = poCodeC1TextBox;
                return false;
            }

            var isOk10 = DoCodeErrorCheck();
            if (isOk10 == false)
            {
                ActiveControl = doCodeC1TextBox;
                return false;
            }

            CancelEventArgs e1 = new CancelEventArgs();
            var isOk11 = IsOkComboBoxValidating(stockCateC1ComboBox, e1);
            if (!isOk11)
            {
                ActiveControl = stockCateC1ComboBox;
                return false;
            }

            CancelEventArgs e2 = new CancelEventArgs();
            var isOk12 = IsOkComboBoxValidating(groupCodeC1ComboBox, e2);
            if (!isOk12)
            {
                ActiveControl = groupCodeC1ComboBox;
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

                //var af = new F202E_InsideTransAF();
                // 仕入先マスタ更新情報作成
                var columnName = "";
                double doCodeValue = 0;
                if (!doCodeC1CheckBox.Checked)
                {

                    var result1 = GetNumManage(groupCodeC1ComboBox.Text);
                    if (!result1.IsOk)
                    {
                        ChangeTopMessage("E0008", "部門マスタ検索時に");
                        return;
                    }

                    if (result1.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0001", "部門マスタに");
                        return;
                    }

                    var result2 = GetDoCode("9998");
                    if (!result2.IsOk)
                    {
                        ChangeTopMessage("E0008", "仕入先マスタマスタ検索時に");
                        return;
                    }

                    var numManage = result1.Table.Rows[0]["番号管理"].ToString();
                    var doCode = "";
                    double doCodeSave = 0;

                    if (numManage == "0")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号"].ToString();
                        columnName = "伝票番号";
                    }
                    else if (numManage == "1")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号1"].ToString();
                        columnName = "伝票番号1";
                    }
                    else if (numManage == "2")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号2"].ToString();
                        columnName = "伝票番号2";
                    }
                    else if (numManage == "3")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号3"].ToString();
                        columnName = "伝票番号3";
                    }
                    else if (numManage == "4")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号4"].ToString();
                        columnName = "伝票番号4";
                    }
                    else if (numManage == "5")
                    {
                        doCode = result2.Table.Rows[0]["伝票番号5"].ToString();
                        columnName = "伝票番号5";
                    }
                    else
                    {
                        doCode = result2.Table.Rows[0]["伝票番号"].ToString();
                        columnName = "伝票番号";
                    }

                    doCodeValue = doCode == "" ? 0 : System.Convert.ToDouble(doCode);
                    if (doCodeValue > 9000)
                    {
                        doCodeSave = doCodeValue + 1;
                        doCodeValue = 0;
                    }
                    else
                    {
                        doCodeSave = doCodeValue + 1;
                        doCodeValue = doCodeValue + 1;
                    }

                    doCodeC1TextBox.Text = doCodeSave.ToString("0000");

                }
                
                // 仕入先マスタ、入出庫ファイル、在庫マスタ、素材在庫マスタ、営業支援マスタ更新
                //var result = af.UpdateIOFile(controlListII, columnName, doCodeValue, doCodeC1CheckBox.Checked);
                //if (!result)
                //{
                //    ChangeTopMessage("E0001", "");
                //    return;
                //}

                // 入出庫ファイル、在庫マスタ、素材在庫マスタ更新
                apiParam.RemoveAll();
                apiParam.Add("dbName", new JValue("製造調達"));
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
                apiParam.Add("poCode", new JValue(poCodeC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                apiParam.Add("outNum", new JValue(outNumC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("unitPrice", new JValue(unitPriceC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("price", new JValue(outPriceC1TextBox.Text.Replace(",", "")));
                apiParam.Add("acceptDate", new JValue(outDateC1DateEdit.Text));
                apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                apiParam.Add("dataCate", new JValue(dataCateC1TextBox.Text));
                apiParam.Add("outDataCate", new JValue(outDataCateC1TextBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.Text));
                apiParam.Add("password", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString()));
                apiParam.Add("createStaffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("createID", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("doCodeCheck", new JValue(doCodeC1CheckBox.Checked));
                apiParam.Add("isEOM", new JValue(true));

                var result = ApiCommonUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result.Msg);
                    return;
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

                var result1 = GetStockInfo(controlListII);
                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return false;
                }

                if (result1.Table == null || result1.Table.Rows.Count <= 0)
                {
                    return false;
                }

                prevMonthStockNumC1TextBox.Text = result1.Table.Rows[0]["前残数量"].ToString();
                thisMonthInNumC1TextBox.Text = result1.Table.Rows[0]["入庫数量"].ToString();
                thisMonthOutNumC1TextBox.Text = result1.Table.Rows[0]["出庫数量"].ToString();
                thisMonthStockNumC1TextBox.Text = result1.Table.Rows[0]["当残数量"].ToString();


                var result2 = GetEOMStockInfo(controlListII);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return false;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    return false;
                }

                prevMonthStockNumEOMC1TextBox.Text = result2.Table.Rows[0]["前残数量"].ToString();
                thisMonthInNumEOMC1TextBox.Text = result2.Table.Rows[0]["入庫数量"].ToString();
                thisMonthOutNumEOMC1TextBox.Text = result2.Table.Rows[0]["出庫数量"].ToString();
                thisMonthStockNumEOMC1TextBox.Text = result2.Table.Rows[0]["当残数量"].ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }

        /// <summary>
        /// 入出工程単価情報設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetGridViewInfo()
        {
            try
            {

                c1TrueDBGrid.SetDataBinding(null, "", true);

                var af = new F202E_InsideTransAF();
                var result = af.GetGridViewData(partsCodeC1TextBox.Text);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return false;
                }

                if (result.Table.Rows.Count <= 0)
                {
                    return false;
                }

                c1TrueDBGrid.SetDataBinding(result.Table, "", true);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }
        

        /// <summary>
        /// 出庫金額設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetOutPrice()
        {
            try
            {
                var outNum = outNumC1NumericEdit.Text;
                var unitPrice = unitPriceC1NumericEdit.Text;
                double d1 = outNum == "" ? 0 : System.Convert.ToDouble(outNum);
                double d2 = unitPrice == "" ? 0 : System.Convert.ToDouble(unitPrice);
                double d = d1 * d2;
                outPriceC1TextBox.Value = Math.Round(d).ToString("###,###,##0");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }

        /// <summary>
        /// 部門名設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetGroupName()
        {
            try
            {

                var param = new SansoBase.GroupMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.GroupCode, groupCodeC1ComboBox.Text));
                param.SetDBName("三相メイン");
                var result = CommonAF.ExecutSelectSQL(param);

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部門マスタ検索時に");
                    return false;
                }
                if (result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "部門マスタに");
                    return false;
                }
                groupNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部門名") ?? "";

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
        private bool OutNumErrorCheck()
        {
            var t = outNumC1NumericEdit;
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

            if (t.Text == "0.00")
            {
                ChangeTopMessage("W0016", "出庫数に0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 単価チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool UnitPriceErrorCheck()
        {
            var t = unitPriceC1NumericEdit;
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

            decimal.TryParse(unitPriceC1NumericEdit.Text, out decimal unitPrice);
            if (unitPrice == 0)
            {
                ChangeTopMessage("W0016", "単価に0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 出庫日付チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool OutDateErrorCheck()
        {
            var d1 = outDateC1DateEdit;
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
        /// 需要予測番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool JyuyoyosokuCodeErrorCheck()
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

            apiParam.RemoveAll();
            apiParam.Add("jyuyoyosokuCode", new JValue(t.Text.Replace("-", "")));

            var result = ApiCommonGet(apiUrl + "GetManufactFile", apiParam);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "需要予測マスタ検索時に");
                return false;
            }

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", t.Label.Text, "需要予測マスタ");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 出庫先チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SupCodeErrorCheck()
        {
            var t = supCodeC1TextBox;
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

            if (t.Text == groupCodeC1ComboBox.Text)
            {
                ChangeTopMessage("W0016", "課別コードと同じコード");
                return false;
            }

            var paramSup = new SupMst();
            paramSup.SelectStr = "*";
            paramSup.WhereColuList.Add((paramSup.SupCode, t.Text));
            var afSup = CommonAF.ExecutSelectSQL(paramSup);
            var dtSup = afSup.Table;
            if (dtSup.Rows.Count >= 1)
            {
                var supCate = dtSup.Rows[0].Field<string>("仕入先区分") ?? string.Empty;
                if (supCate.Trim() != "K")
                {
                    ChangeTopMessage("W0016", "社内コード以外");
                    return false;
                }

                supNameC1TextBox.Text = dtSup.Rows[0].Field<string>("仕入先名１") ?? string.Empty;
                supCateC1TextBox.Text = supCate;
            }
            else
            {
                supNameC1TextBox.Text = "仕入先マスタ未登録";
                supCateC1TextBox.Text = "";
            }

            if (supCateC1TextBox.Text == "K")
            {
                dataCateC1TextBox.Text = "8";
            }

            return true;
        }

        /// <summary>
        /// 単価マスタから支給単価取得
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool GetUnitPrice()
        {
            var result = GetUnitPrice(partsCodeC1TextBox.Text, supCodeC1TextBox.Text);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return false;
            }

            if (result.Table.Rows.Count <= 0)
            {
                // 処理なし
            }
            else
            {
                unitPriceC1NumericEdit.Text = result.Table.Rows[0]["支給単価"].ToString();
            }
            return true;
        }

        /// <summary>
        /// 注文番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PoCodeErrorCheck()
        {
            var t = poCodeC1TextBox;
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

            if (supCodeC1TextBox.Text == "2930")
            {

                var result = GetPartsArrangeMst(t.Text, partsCodeC1TextBox.Text);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return false;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", "注文番号", "部品手配マスタ");
                    return false;
                }

                var num1 = result.Table.Rows[0]["delivInstructionNum"].ToString();
                var num2 = result.Table.Rows[0]["supDelivNum"].ToString();
                double d1 = num1 == "" ? 0 : System.Convert.ToDouble(num1);
                double d2 = num2 == "" ? 0 : System.Convert.ToDouble(num2);
                double dOrdRemainNum = d1 - d2;
                ordRemainNumC1TextBox.Text = dOrdRemainNum.ToString();

                if (dOrdRemainNum < 0)
                {
                    ChangeTopMessage(1, "WARN", "すでに納品が完了しています。");
                }

                var outNum = outNumC1NumericEdit.Text;
                double dOutNum = outNum == "" ? 0 : System.Convert.ToDouble(outNum);
                if (dOrdRemainNum < dOutNum)
                {
                    ChangeTopMessage(1, "WARN", "受注残より多い数が入力されました。");

                }

            }

            return true;
        }

        /// <summary>
        /// 伝票番号チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool DoCodeErrorCheck()
        {
            var t = doCodeC1TextBox;
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

            var isOk2 = Check.IsStringRange(t.Text, 4, 4);
            if (isOk2.Result == false)
            {
                ChangeTopMessage("W0009", t.Label.Text, "4", "4");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ユーザの課別コードを取得
        /// </summary>
        private (bool IsOk, DataTable Table) GetGroupComboListByUser(string id)
        {
            apiParam.RemoveAll();
            apiParam.Add("id", new JValue(id));
            var result = ApiCommonGet(apiUrl + "GetGroupComboListByUser", apiParam);

            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 在庫情報取得
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetStockInfo(List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");

            if (stockCate == "Z")
            {
                var param = new SansoBase.StockMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCode));
                param.WhereColuList.Add((param.GroupCode, groupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);

                return (result.IsOk, result.Table);

            }
            else
            {
                var param = new SansoBase.MaterialStockMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCode));
                param.WhereColuList.Add((param.GroupCode, groupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);

                return (result.IsOk, result.Table);
            }

        }

        /// <summary>
        /// 月末在庫情報取得
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetEOMStockInfo(List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var groupCode = controlList.SGetText("groupCodeC1ComboBox");
            var stockCate = controlList.SGetText("stockCateC1ComboBox");

            if (stockCate == "Z")
            {
                var param = new SansoBase.StockMstE();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCode));
                param.WhereColuList.Add((param.GroupCode, groupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);

                return (result.IsOk, result.Table);

            }
            else
            {
                var param = new SansoBase.MaterialStockMstE();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, partsCode));
                param.WhereColuList.Add((param.GroupCode, groupCode));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);

                return (result.IsOk, result.Table);
            }

        }

        /// <summary>
        /// 単価情報取得
        /// </summary>
        /// <param name="s1">部品コード</param>
        /// <param name="s2">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetUnitPrice(string s1, string s2)
        {
            var param = new SansoBase.UnitPriceMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, s1));
            param.WhereColuList.Add((param.SupCode, s2));

            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);

            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// 部品手配マスタ情報取得
        /// </summary>
        /// <param name="s1">注文番号</param>
        /// <param name="s2">部品コード</param>
        public (bool IsOk, DataTable Table) GetPartsArrangeMst(string s1, string s2)
        {
            apiParam.RemoveAll();
            apiParam.Add("poCode", new JValue(s1));
            apiParam.Add("partsCode", new JValue(s2));
            var result = ApiCommonGet(apiUrl + "GetPartsArrange", apiParam);

            return (result.IsOk, result.Table);

        }


        /// <summary>
        /// 番号管理情報取得
        /// </summary>
        /// <param name="s1">課別コード</param>
        public (bool IsOk, DataTable Table) GetNumManage(string s1)
        {
            var param = new SansoBase.GroupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.GroupCode, s1));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);

            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 伝票番号情報取得
        /// </summary>
        /// <param name="s1">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetDoCode(string s1)
        {
            var param = new SansoBase.SupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.SupCode, s1));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);

            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0], エラーメッセージ)</returns>
        private (bool IsOk, int Count, string Msg, string doCode) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
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
                        return (false, 0, (string)(result["msg"]), "");
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, (string)(result["msg"]), "");
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
                return (false, 0, (string)(result["msg"]), "");
            }

            return (
                true,
                (int)(result["count"]),
                "",
                (string)(result["doCode"])
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
                        return (false, 0, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, null);
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

        #endregion  ＜その他処理 END＞



    }
}
