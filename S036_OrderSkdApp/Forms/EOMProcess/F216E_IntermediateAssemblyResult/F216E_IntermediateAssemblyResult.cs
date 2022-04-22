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
    /// 月末中間組品 実績
    /// </summary>
    public partial class F216E_IntermediateAssemblyResult : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"]
                                + "Solution/S036/F216E/";

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
        public F216E_IntermediateAssemblyResult(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "月末中間組品 実績";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F216E_IntermediateAssemblyResult_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(resultGroupC1ComboBox, resultGroupC1TextBox, "", true, enumCate.無し);
                AddControlListII(resultGroupC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(resultNumC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(unitPriceC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(priceCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(mfgPriceC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(priceC1TextBox, null, "", false, enumCate.無し);
                var today = DateTime.Today;
                AddControlListII(resultDateC1DateEdit, null, today.ToString("yyyy/MM/dd"), true, enumCate.無し);
                AddControlListII(doCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, groupNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(dataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(accountCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(outDataCateC1TextBox, outDataCateNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(entryDateC1TextBox, null, today.ToString("yyyy/MM/dd"), false, enumCate.無し);
                AddControlListII(stockCateC1TextBox, stockCateNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(stockCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(transCateC1TextBox, transCateNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(transCateNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCateC1TextBox, null, "", false, enumCate.無し);


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

                SetResultGroupC1ComboBox();

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
        /// 実績部門　コンボボックスセット
        /// </summary>
        private void SetResultGroupC1ComboBox()
        {
            var userId = LoginInfo.Instance.UserId.ToUpper();

            var result = GetGroupManufactDirect(userId);

            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "実績部門検索時に");
                return;
            }
            var dt = result.Table;
            
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(resultGroupC1ComboBox, dt);

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
                priceCateC1TextBox.Text = "";
                unitPriceC1TextBox.Text = "";
                mfgPriceC1TextBox.Text = "";

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

                dataCateC1TextBox.Text = "S";


                // 工程マスタ確認
                var result2 = GetStockCate();
                if (!result2)
                {
                    ActiveControl = partsCodeC1TextBox;
                    return;
                }

                // 単価マスタ確認
                if (partsCodeC1TextBox.Text != "" && resultGroupC1ComboBox.Text != "")
                {
                    var result3 = GetUnitPrice(partsCodeC1TextBox.Text, resultGroupC1ComboBox.Text);

                    if (!result3.IsOk)
                    {
                        ChangeTopMessage("E0008", "単価マスタ検索時に");
                        ActiveControl = partsCodeC1TextBox;
                        return;
                    }

                    if (result3.Table == null || result3.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0001", "単価マスタ");
                        ActiveControl = partsCodeC1TextBox;
                        return;
                    }
                    else
                    {
                        priceCateC1TextBox.Text = result3.Table.Rows[0].Field<string>("単価区分") ?? "";
                        unitPriceC1TextBox.Text = (result3.Table.Rows[0].Field<decimal?>("仕入単価") ?? 0).ToString();
                        mfgPriceC1TextBox.Text = (result3.Table.Rows[0].Field<decimal?>("加工費") ?? 0).ToString();
                    }
                }


                // 部品構成表２確認
                var result4 = GetBOM2(partsCodeC1TextBox.Text);
                
                if (!result4.IsOk)
                {
                    ChangeTopMessage("E0008", "部品構成表２検索時に");
                    ActiveControl = partsCodeC1TextBox;
                    return;
                }

                if (result4.Table == null || result4.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "部品構成表２");
                    ActiveControl = partsCodeC1TextBox;
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 処理年月設定、単価マスタ確認、部品構成表２確認
        /// </summary>
        private void resultGroupC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

                // 課別コード設定
                groupCodeC1TextBox.Text = c.Text;
                groupNameC1TextBox.Text = listCtr.Text;

                // 処理年月設定
                var result = GetEOMExecuteDate(c.Text);

                if (!result.IsOk)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    ActiveControl = resultGroupC1ComboBox;
                    return;
                }

                if (result.Table == null || result.Table.Rows.Count <= 0)
                {
                    executeDateValueLabel.Visible = false;
                    executeDateLabel.Visible = false;
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
        /// 実績数量　検証時
        /// </summary>
        private void resultNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ResultNumErrorCheck();
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
        /// 実績数量　検証後
        /// </summary>
        private void resultNumC1NumericEdit_Validated(object sender, EventArgs e)
        {
            try
            {
                //金額の算出
                var resultNum = resultNumC1NumericEdit.Text;
                var unitPrice = unitPriceC1TextBox.Text;
                decimal d1 = resultNum == "" ? 0 : System.Convert.ToDecimal(resultNum);
                decimal d2 = unitPrice == "" ? 0 : System.Convert.ToDecimal(unitPrice);
                decimal d = d1 * d2;

                var round = Math.Round(d);
                var check = SansoBase.Check.IsPointNumberRange(round, 11, 0);
                if (check.Result == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
                    ChangeTopMessage(1, "WARN", "実績数量 × 単価の" + check.Msg);
                    return;
                }

                priceC1TextBox.Text = round.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 実績部門　検証後
        /// </summary>
        private void resultGroupC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                priceCateC1TextBox.Text = "";
                unitPriceC1TextBox.Text = "";
                mfgPriceC1TextBox.Text = "";

                //仕入先マスタ確認
                var paramSup = new SupMst();
                paramSup.SelectStr = "*";
                paramSup.WhereColuList.Add((paramSup.SupCode, resultGroupC1ComboBox.Text));
                var afSup = CommonAF.ExecutSelectSQL(paramSup);
                var dtSup = afSup.Table;
                if (dtSup.Rows.Count >= 1)
                {
                    var supCate = dtSup.Rows[0].Field<string>("仕入先区分") ?? string.Empty;
                    if (supCate.Trim() != "K")
                    {
                        ChangeTopMessage("W0016", "社内コード以外");
                        ActiveControl = resultGroupC1ComboBox;
                        return;
                    }

                    supCateC1TextBox.Text = supCate;
                }

                // 工程マスタ確認
                var result = GetStockCate();
                if (!result)
                {
                    ActiveControl = resultGroupC1ComboBox;
                    return;
                }

                // 単価マスタ確認
                if (partsCodeC1TextBox.Text != "" && resultGroupC1ComboBox.Text != "")
                {
                    var result2 = GetUnitPrice(partsCodeC1TextBox.Text, resultGroupC1ComboBox.Text);

                    if (!result2.IsOk)
                    {
                        ChangeTopMessage("E0008", "単価マスタ検索時に");
                        ActiveControl = resultGroupC1ComboBox;
                        return;
                    }

                    if (result2.Table == null || result2.Table.Rows.Count <= 0)
                    {
                        ChangeTopMessage("W0001", "単価マスタ");
                        ActiveControl = resultGroupC1ComboBox;
                        return;
                    }
                    else
                    {
                        priceCateC1TextBox.Text = result2.Table.Rows[0].Field<string>("単価区分") ?? "";
                        unitPriceC1TextBox.Text = (result2.Table.Rows[0].Field<decimal?>("仕入単価") ?? 0).ToString();
                        mfgPriceC1TextBox.Text = (result2.Table.Rows[0].Field<decimal?>("加工費") ?? 0).ToString();
                    }

                }

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
            var isOk1 = PartsCodeErrorCheck();
            if (isOk1 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk2 = ResultDateErrorCheck();
            if (isOk2 == false)
            {
                ActiveControl = resultDateC1DateEdit;
                return false;
            }

            var isOk3 = priceCateErrorCheck();
            if (isOk3 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            var isOk4 = stockCateErrorCheck();
            if (isOk4 == false)
            {
                ActiveControl = stockCateC1TextBox;
                return false;
            }

            var isOk5 = transCateErrorCheck();
            if (isOk5 == false)
            {
                ActiveControl = transCateC1TextBox;
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
                apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                apiParam.Add("supCode", new JValue(resultGroupC1ComboBox.Text));
                apiParam.Add("inNum", new JValue(resultNumC1NumericEdit.Text.Replace(",", "")));
                apiParam.Add("unitPriceCate", new JValue(priceCateC1TextBox.Text));
                apiParam.Add("unitPrice", new JValue(unitPriceC1TextBox.Text.Replace(",", "")));
                apiParam.Add("processUnitPrice", new JValue(mfgPriceC1TextBox.Text.Replace(",", "")));
                apiParam.Add("price", new JValue(priceC1TextBox.Text.Replace(",", "")));
                apiParam.Add("acceptDate", new JValue(resultDateC1DateEdit.Text));
                apiParam.Add("groupCode", new JValue(groupCodeC1TextBox.Text));
                apiParam.Add("accountCode", new JValue(accountCodeC1TextBox.Text));
                apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));
                apiParam.Add("outDataCate", new JValue(outDataCateC1TextBox.Text));
                apiParam.Add("stockCate", new JValue(stockCateC1TextBox.Text));
                apiParam.Add("nSupCate", new JValue(transCateC1TextBox.Text));
                apiParam.Add("password", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineName", new JValue(LoginInfo.Instance.MachineCode));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString()));
                apiParam.Add("createStaffCode", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("createID", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("isEOM", new JValue(true));

                var result = ApiCommonUpdate(apiUrl + "UpdateIOFile", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result.Msg);
                    return;
                }

                DisplayClear();
                ActiveControl = partsCodeC1TextBox;
                ChangeTopMessage("I0001", "月末中間組品実績処理");


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
        /// 実績日付チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ResultDateErrorCheck()
        {
            var d1 = resultDateC1DateEdit;
            var isOk1 = DateTime.TryParse(d1.Text, out var dt1);
            if (!isOk1)
            {
                ActiveControl = d1;
                ChangeTopMessage("W0007", "実績日付");
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
            DateTime endDate = SansoBase.DatetimeFC.GetEndOfMonth(startDate);

            if (Check.IsDateRange(date, startDate, endDate).Result == false)
            {
                ChangeTopMessage("W0016", "範囲外の日付");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 工程マスタ　取得
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool GetStockCate()
        {
            if (partsCodeC1TextBox.Text != "" && resultGroupC1ComboBox.Text != "")
            {
                stockCateC1TextBox.Text = "";
                transCateC1TextBox.Text = "";

                
                var result2 = GetStockCate(partsCodeC1TextBox.Text, resultGroupC1ComboBox.Text);
                
                if (!result2.IsOk)
                {
                    ChangeTopMessage("E0008", "工程マスタ検索時に");
                    return false;
                }

                if (result2.Table == null || result2.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "工程マスタ");
                    return false;
                }
                else
                {
                    stockCateC1TextBox.Text = result2.Table.Rows[0].Field<string>("stockCate") ?? "";
                    transCateC1TextBox.Text = result2.Table.Rows[0].Field<string>("suppliesCate") ?? "";
                }

            }

            return true;
        }

        /// <summary>
        /// 実績数量チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ResultNumErrorCheck()
        {
            var t = resultNumC1NumericEdit;
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
        /// 単価区分チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool priceCateErrorCheck()
        {
            var t = priceCateC1TextBox;

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (t.Text != "1" && t.Text != "K")
            {
                ChangeTopMessage("W0016", "単価区分に 1:決定 または K:仮 以外");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 在庫Pチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool stockCateErrorCheck()
        {
            var t = stockCateC1TextBox;

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (t.Text != "" && t.Text != "Z")
            {
                ChangeTopMessage("W0016", "在庫Pに Z:在庫 または 空白:素材在庫 以外");
                return false;
            }


            var result2 = GetStockInfo(controlListII);
            if (!result2.IsOk)
            {
                ChangeTopMessage("E0008", "在庫マスタ検索時に");
                return false;
            }

            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0001", "在庫マスタ");
                return false;
            }


            return true;
        }

        /// <summary>
        /// 支給区分チェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool transCateErrorCheck()
        {
            var t = transCateC1TextBox;

            var isOk = Check.HasSQLBanChar(t.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (t.Text != "" && t.Text != "Y" && t.Text != "M")
            {
                ChangeTopMessage("W0016", "支給区分に Y:有償 または M:無償 または空白 以外");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 部品構成表２　取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        public (bool IsOk, DataTable Table) GetBOM2(string partsCode)
        {
            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("partsCode", new JValue(partsCode));

            var result = ApiCommonGet(apiUrl + "GetBOM2Mst", apiParam);

            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 工程マスタ　在庫Ｐ　取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetStockCate(string partsCode, string supCode)
        {

            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("partsCode", new JValue(partsCode));
            apiParam.Add("supCode", new JValue(supCode));

            var result = ApiCommonGet(apiUrl + "GetProcessMst", apiParam);

            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 在庫情報取得
        /// </summary>
        /// <param name="controlList">コントロールリスト</param>
        public (bool IsOk, DataTable Table) GetStockInfo(List<ControlParam> controlList)
        {
            var partsCode = controlList.SGetText("partsCodeC1TextBox");
            var groupCode = controlList.SGetText("groupCodeC1TextBox");
            var stockCate = controlList.SGetText("stockCateC1TextBox");

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
        /// S1製造直接部門　取得
        /// </summary>
        /// <param name="s">作成者ID</param>
        public (bool IsOk, DataTable Table) GetGroupManufactDirect(string s)
        {
            string sql =
            "SELECT " +
            "B.部門コード " +
            ", B.部門名 " +
            "FROM " +
            "三相メイン.dbo.M_SystemUsingGroup AS A " +
            ", 三相メイン.dbo.部門マスタ AS B " +
            "WHERE " +
            "A.STAFFID = '" + s + "' " +
            "AND B.有効部門区分 = '1' AND B.製販区分 = 'S1' " +
            "AND A.GROUPCODE = B.部門コード " +
            "ORDER BY GROUPCODE ";

            var result = CommonAF.ExecutSelectSQL(sql);
            return (result.IsOk, result.Table);
        }

        /// <summary>
        /// 単価情報取得
        /// </summary>
        /// <param name="partsCode">部品コード</param>
        /// <param name="supCode">仕入先コード</param>
        public (bool IsOk, DataTable Table) GetUnitPrice(string partsCode, string supCode)
        {
            var param = new SansoBase.UnitPriceMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.PartsCode, partsCodeC1TextBox.Text));
            param.WhereColuList.Add((param.SupCode, resultGroupC1ComboBox.Text));

            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);

            return (result.IsOk, result.Table);

        }

        /// <summary>
        /// 月末処理の処理年月　取得
        /// </summary>
        /// <param name="s">部門コード</param>
        public (bool IsOk, DataTable Table) GetEOMExecuteDate(string s)
        {

            apiParam.RemoveAll();
            apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("groupCode", new JValue(s));

            var result = ApiCommonGet(apiUrl + "GetEOMExecuteDate", apiParam);

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

            
            //return (
            //    true,
            //    (int)(result["count"]),
            //    "",
            //    (string)(result["doCode"])
            //);
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
