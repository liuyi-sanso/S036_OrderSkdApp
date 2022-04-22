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
    /// 出庫処理（部品振替）
    /// </summary>
    public partial class F205_OutDataPartsExchange : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"]
                                + "Solution/S036/F205/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F205_OutDataPartsExchange(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "出庫処理（部品振替）";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F205_OutDataPartsExchange_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1ComboBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(delivNumC1NumericEdit, null, null, true, enumCate.無し);
                var today = DateTime.Today;
                AddControlListII(delivDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(codeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1ComboBox, outDataCateNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(outDataCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(entryDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, stockCateNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockInfoC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthInNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthOutNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthStockNumC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksAllC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsCodeAfterC1TextBox, partsNameAfterC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameAfterC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(prevMonthStockNumAfterC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthInNumAfterC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthOutNumAfterC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(thisMonthStockNumAfterC1TextBox, null, "", false, enumCate.無し);

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

                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, (byte)128);

                // DefaultButtomMessageをセット
                defButtomMessage = "振替前部品：マイナス出庫処理されて在庫が増えます。" +  Environment.NewLine  +
                                   "振替後部品：出庫処理されて在庫が減ります";

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
            dt.Rows.Add("6", "変更・改造");
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
            dt.Rows.Add("", "素材在庫");
            dt.Rows.Add("Z", "完成品在庫");

            ControlAF.SetC1ComboBox(stockCateC1ComboBox, dt, stockCateC1ComboBox.Width,
                stockCateNameC1TextBox.Width, "ID", "NAME", true);
        }

        /// <summary>
        /// 出庫先　コンボボックスセット
        /// </summary>
        private void SetSupCodeC1ComboBox()
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
            ControlAF.SetC1ComboBox(supCodeC1ComboBox, result.Table);

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

            supCodeC1ComboBox.SelectedIndex = (supCodeC1ComboBox.ItemsDataSource == null) ? -1 : 0;
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

                    case "partsCodeAfterC1TextBox":
                        partsSearchAfterBt_Click(sender, e);
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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 部品検索ボタン　振替前　押下時
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
        /// 部品検索ボタン　振替後　押下時
        /// </summary>
        private void partsSearchAfterBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        partsCodeAfterC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString().TrimEnd();
                        partsNameAfterC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                    }
                }
                ActiveControl = partsCodeAfterC1TextBox;
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
                SetStockInfoAfter();

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
                SetStockInfoAfter();

                // 処理年月設定
                var result = GetExecuteDate(groupCodeC1TextBox.Text);

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "部門マスタ検索時に");
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    executeDateValueLabel.Visible = false;
                    executeDateLabel.Visible = false;
                    ChangeTopMessage("W0001", "部門マスタ");
                    return;
                }
                else
                {
                    executeDateValueLabel.Text = result.Table.Rows[0].Field<string>("executeDate") ?? "";
                    executeDateValueLabel.Visible = true;
                    executeDateLabel.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 部品コード　振替前　検証時
        /// </summary>
        private void partsCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = PartsCodeErrorCheck(sender);
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
        /// 部品コード　振替前　検証後
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
                SetStockInfoAfter();
                dataCateC1TextBox.Text = "";

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


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コード　振替後　検証時
        /// </summary>
        private void partsCodeAfterC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = PartsCodeErrorCheck(sender);
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
        /// 部品コード　振替後　検証後
        /// </summary>
        private void partsCodeAfterC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                partsNameAfterC1TextBox.Text = "";
                // 在庫情報設定
                SetStockInfo();
                SetStockInfoAfter();

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
                partsNameAfterC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";

                //stockCateC1ComboBox.SelectedIndex = 1;
                //dataCateC1TextBox.Text = "4";


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


            if (outDataCateC1ComboBox.Text == "6")
            {
                if (codeC1TextBox.Text == "")
                {
                    ActiveControl = codeC1TextBox;
                    ChangeTopMessage("W0007", codeC1TextBox.Label.Text);
                    return false;
                }

                if (jyuyoyosokuCodeC1TextBox.Text == "")
                {
                    ActiveControl = jyuyoyosokuCodeC1TextBox;
                    ChangeTopMessage("W0007", jyuyoyosokuCodeC1TextBox.Label.Text);
                    return false;
                }
            }
            else if (outDataCateC1ComboBox.Text == "A")
            {
                if (codeC1TextBox.Text == "")
                {
                    ActiveControl = codeC1TextBox;
                    ChangeTopMessage("W0007", codeC1TextBox.Label.Text);
                    return false;
                }
            }
            else
            {
                // 処理なし
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
            var isOk6 = PartsCodeErrorCheck(partsCodeC1TextBox);
            if (isOk6 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk7 = PartsCodeErrorCheck(partsCodeAfterC1TextBox);
            if (isOk7 == false)
            {
                ActiveControl = partsCodeAfterC1TextBox;
                return false;
            }

            var d1 = delivDateC1DateEdit;
            var isOk1 = DateTime.TryParse(d1.Text, out var dt1);
            if (!isOk1)
            {
                ActiveControl = d1;
                ChangeTopMessage("W0007", "出庫日付");
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

                var isOk4 = RemarksCodeErrorCheck(7);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
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

                var isOk4 = RemarksCodeErrorCheck(5);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
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

                var isOk4 = RemarksCodeErrorCheck(5);
                if (isOk4 == false)
                {
                    ActiveControl = codeC1TextBox;
                    return false;
                }

            }

            var isOk5 = RemarksJyuyoyosokuCodeErrorCheck();
            if (isOk5 == false)
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
                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // 入出庫ファイル、在庫マスタ、素材在庫マスタ更新
                apiParam.RemoveAll();
                apiParam.Add("dbName", new JValue("製造調達"));
                apiParam.Add("dataCate", new JValue(dataCateC1TextBox.Text));
                apiParam.Add("partsCode", new JValue(""));
                apiParam.Add("beforePartsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("afterPartsCode", new JValue(partsCodeAfterC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeC1ComboBox.Text));
                apiParam.Add("outNum", new JValue(delivNumC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("acceptDate", new JValue(delivDateC1DateEdit.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1TextBox.Text));
                apiParam.Add("outDataCate", new JValue(outDataCateC1ComboBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.Text));
                apiParam.Add("password", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("remarks", new JValue(remarksC1TextBox.Text));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString()));
                apiParam.Add("createStaffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("createID", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("isEOM", new JValue(false));

                var result = ApiCommonUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result.Msg);
                    return;
                }

                DisplayClear();
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0001", "出庫処理");

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
        /// 在庫情報　振替前　設定
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

                if (stockCateC1ComboBox.Text == "Z")
                {
                    stockInfoC1TextBox.Text = "完成品在庫";
                }
                else if (stockCateC1ComboBox.Text == "")
                {
                    stockInfoC1TextBox.Text = "素材在庫";
                }
                else
                {
                    stockInfoC1TextBox.Text = "";
                }

                (bool IsOk, DataTable Table, string Sql) result;

                if (stockCateC1ComboBox.Text == "Z")
                {
                    // 在庫マスタ
                    var param = new StockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);
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
                }

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                    return false;
                }

                if (result.Table.Rows.Count <= 0)
                {
                    return false;
                }

                prevMonthStockNumC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString();
                thisMonthInNumC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString();
                thisMonthOutNumC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString();
                thisMonthStockNumC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;

        }


        /// <summary>
        /// 在庫情報　振替後　設定
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool SetStockInfoAfter()
        {
            try
            {

                prevMonthStockNumAfterC1TextBox.Text = "";
                thisMonthInNumAfterC1TextBox.Text = "";
                thisMonthOutNumAfterC1TextBox.Text = "";
                thisMonthStockNumAfterC1TextBox.Text = "";

                if (stockCateC1ComboBox.Text == "Z")
                {
                    stockInfoC1TextBox.Text = "完成品在庫";
                }
                else if (stockCateC1ComboBox.Text == "")
                {
                    stockInfoC1TextBox.Text = "素材在庫";
                }
                else
                {
                    stockInfoC1TextBox.Text = "";
                }

                (bool IsOk, DataTable Table, string Sql) result;

                if (stockCateC1ComboBox.Text == "Z")
                {
                    // 在庫マスタ
                    var param = new StockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeAfterC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);
                }
                else
                {
                    // 素材在庫マスタ
                    var param = new MaterialStockMst();
                    param.SelectStr = "*";
                    param.WhereColuList.Add((param.PartsCode, partsCodeAfterC1TextBox.Text));
                    param.WhereColuList.Add((param.GroupCode, groupCodeC1TextBox.Text));
                    param.SetDBName("製造調達");
                    result = CommonAF.ExecutSelectSQL(param);
                }

                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "在庫マスタ/素材在庫マスタ検索時に");
                    return false;
                }

                if (result.Table.Rows.Count <= 0)
                {
                    return false;
                }

                prevMonthStockNumAfterC1TextBox.Text = result.Table.Rows[0]["前残数量"].ToString();
                thisMonthInNumAfterC1TextBox.Text = result.Table.Rows[0]["入庫数量"].ToString();
                thisMonthOutNumAfterC1TextBox.Text = result.Table.Rows[0]["出庫数量"].ToString();
                thisMonthStockNumAfterC1TextBox.Text = result.Table.Rows[0]["当残数量"].ToString();

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
        private bool PartsCodeErrorCheck(object sender)
        {
            var t = (C1TextBox)sender;
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

            if (t.Name == "partsCodeC1TextBox")
            {
                partsNameC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";
            }
            else if (t.Name == "partsCodeAfterC1TextBox")
            {
                partsNameAfterC1TextBox.Text = result.Table.Rows[0].Field<string>("部品名") ?? "";
            }
            else
            {
                // 処理なし
            }

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
            DateTime date = dt1;
            DateTime startDate = DateTime.Parse(executeDate.Trim() + "/01");
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            if (Check.IsDateRange(date, startDate, endDate).Result == false)
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
        /// 処理年月 取得処理
        /// </summary>
        public (bool IsOk, DataTable Table) GetExecuteDate(string groupCode)
        {

            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(groupCode));

            var result = ApiCommonGet(apiUrl + "GetExecuteDate", apiParam);

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
                System.Convert.ToInt32(result["count"]),
                "",
                (result["doCode"] == null) ? "null" : (string)result["doCode"]
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
