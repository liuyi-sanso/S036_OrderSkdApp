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
using System.Net;
using Newtonsoft.Json;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 仕入先マスタメンテナンス
    /// </summary>
    public partial class F602_SupMstMaint : BaseForm
    {
        #region ＜フィールド＞ 
        /// <summary>
        /// 処理区分の変更前保管エリア
        /// </summary>
        private string stProcessCate = "0";

        /// <summary>
        /// 仕入先コードの変更前保管エリア
        /// </summary>
        private string stSupCode = "";

        /// <summary>
        /// 郵便番号の変更前保管エリア
        /// </summary>
        private string stPostalCode = "";

        /// <summary>
        /// DTCheckクラス(排他制御)　仕入先マスタ
        /// </summary>
        DTCheck supMstDTCheck = new DTCheck();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId">フォームID</param>
        public F602_SupMstMaint(string fId) : base(fId)
        {
            InitializeComponent();
            this.titleLabel.Text = "仕入先マスタメンテナンス";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F602_SupMstMaint_Load(object sender, EventArgs e)
        {
            try
            {
                AddControlListII(supCodeC1TextBox, null, "", true, enumCate.Key);
                AddControlListII(supName1C1TextBox, null, "", true, enumCate.Key);
                AddControlListII(supName2C1TextBox, null, "", false, enumCate.Key);
                AddControlListII(postalCodeC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(address1C1TextBox, null, "", false, enumCate.Key);
                AddControlListII(address2C1TextBox, null, "", false, enumCate.Key);
                AddControlListII(phoneNumC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(faxC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(mailC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(supStaffNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(sansoStaffNameC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(createStaffIDC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(createDateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(updateStaffIDC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(updateDateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(supCateC1TextBox, null, "", false, enumCate.Key);
                AddControlListII(processCateC1ComboBox, null, "", true, enumCate.無し);
                AddControlListII(supCateC1ComboBox, supCateC1TextBox, "", true, enumCate.無し);
                AddControlListII(delivSlipIssueCateC1ComboBox, null, "", true, enumCate.無し);

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

                SetProcessC1ComboBox();
                SetSupCateC1ComboBox();
                SetDelivSlipIssueCateC1ComboBox();

                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, (byte)128);

                defButtomMessage = "処理区分、仕入先コードを入力すると登録済のマスタ情報が表示されます。\r\n" +
                                   "最後に実行（F10）を押すとデータベースに更新されます。";

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
        /// 処理区分  コンボボックスセット
        /// </summary>
        private void SetProcessC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("新規｜修正", "0");
            dt.Rows.Add("削除", "1");
            ControlAF.SetC1ComboBox(processCateC1ComboBox, dt, 0, 150, "NAME", "NAME", true);
        }

        /// <summary>
        /// 仕入先区分  コンボボックスセット
        /// </summary>
        private void SetSupCateC1ComboBox()
        {
            var af = new SupMstAF();
            var result = af.GetSupCate();
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先区分の");
                return;
            }
            var dt = result.Table;
            if (dt.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "仕入先区分", "マスタ");
                return;
            }
            
            ControlAF.SetC1ComboBox(supCateC1ComboBox, dt);
        }

        /// <summary>
        /// 納品書発行区分  コンボボックスセット
        /// </summary>
        private void SetDelivSlipIssueCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("発行する", "");
            dt.Rows.Add("発行しない", "1");
            ControlAF.SetC1ComboBox(delivSlipIssueCateC1ComboBox, dt, 0, 150, "NAME", "NAME", true);
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

            // キー以外の入力項目編集を不可にする
            EditEnable(false);

            stProcessCate = "0";
            stSupCode = "";
            stPostalCode = "";
            processCateC1ComboBox.SelectedIndex = 0;
            supCateC1ComboBox.SelectedIndex = 0;
            delivSlipIssueCateC1ComboBox.SelectedIndex = 0;

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // エクセルファイル用DataTable
            excelDt = null;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = processCateC1ComboBox;
        }

        /// <summary>
        /// 仕入先情報 クリア処理
        /// </summary>
        private void DetailClear()
        {
            // パネルの一括クリア処理
            foreach (Control c in panel2.Controls)
            {
                var type = c.GetType();
                if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).Text = "";
                }
            }
            supCateC1ComboBox.SelectedIndex = 0;
            delivSlipIssueCateC1ComboBox.SelectedIndex = 0;

            // キー以外の入力項目編集を不可にする
            EditEnable(false);

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
                    case "supCodeC1TextBox":
                        supCodeSearchBt_Click(sender, e);
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

        /*
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
        */

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
        /// 仕入先コード検索ボタン押下時
        /// </summary>
        private void supCodeSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_SupMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        SetSupColumn();
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
        /// 処理区分 検証後
        /// </summary>
        private void processCateC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var c = (C1ComboBox)sender;

                if (string.IsNullOrEmpty(c.Text))
                {
                    return;
                }

                var s = c.SGetText(1);
                if (isRunValidating == false
                    || stProcessCate == s)
                {
                    return;
                }

                DetailClear();

                SetSupColumn();

                stProcessCate = s;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// 仕入先コード検証時
        /// </summary>
        private void supCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false
                    || string.IsNullOrEmpty(t.Text)
                    || stSupCode == t.Text)
                {
                    return;
                }

                var result = SupCodeCK(t);
                if (result.IsOk == false)
                {
                    ActiveControl = t;
                    e.Cancel = true;
                    return;
                }

                // 処理区分が削除 かつ 仕入先マスタ未登録の場合エラー
                var s = processCateC1ComboBox.SGetText(1);
                if (s == "1" && result.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0002", supCodeC1TextBox.Label.Text, "仕入先マスタ");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 仕入先コード 検証後
        /// </summary>
        private void supCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            var t = (C1TextBox)sender;

            if (isRunValidating == false
                || stSupCode == t.Text)
            {
                return;
            }

            if (string.IsNullOrEmpty(t.Text))
            {
                DetailClear();
                stSupCode = t.Text;
                return;
            }

            SetSupColumn();

            stSupCode = t.Text;
        }

        /// <summary>
        /// 仕入先名 検証時
        /// </summary>
        private void supNameC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // 全角項目共通チェック
                var result = FullSizeCommonCheck(t, 30);
                if (result == false)
                {
                    this.ActiveControl = t;
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
        /// 郵便番号 検証時
        /// </summary>
        private void postalCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false
                    || string.IsNullOrEmpty(t.Text)
                    || stPostalCode == t.Text)
                {
                    return;
                }

                // 半角項目共通チェック
                var result = HalfSizeCommonCheck(t, 8);
                if (result == false)
                {
                    this.ActiveControl = t;
                    e.Cancel = true;
                    return;
                }

                // 郵便番号からハイフン、ブランクを除去
                string postalCode = t.Text.Replace("-", "").Replace(" ", "");
                if (string.IsNullOrEmpty(postalCode))
                {
                    return;
                }

                // ハイフンを除いて数値7桁か
                var numChk = Check.IsNumeric(postalCode).Result;
                if (numChk == false || postalCode.Length != 7)
                {
                    ChangeTopMessage("W0021", "郵便番号");
                    this.ActiveControl = (C1TextBox)sender;
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
        /// 郵便番号 検証後
        /// </summary>
        private void postalCodeC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // 郵便番号からハイフン、ブランクを除去
                string postalCode = t.Text.Replace("-", "").Replace(" ", "");

                if (isRunValidating == false
                    || string.IsNullOrEmpty(t.Text)
                    || stPostalCode == t.Text
                    || string.IsNullOrEmpty(postalCode))
                {
                    return;
                }

                // API起動
                var url = $"http://zipcloud.ibsnet.co.jp/api/search?zipcode={ postalCode }";
                F602_ApiAddress AddressJson = null;
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var statusCode = ((HttpWebResponse)response).StatusCode;

                    Console.WriteLine($"StatusCode:{statusCode}, json={json}");

                    AddressJson = JsonConvert.DeserializeObject<F602_ApiAddress>(json);
                }

                if (AddressJson.status != 200)
                {
                    MessageBox.Show("エラーコード=" + AddressJson.status + ",メッセージ=" + AddressJson.message);
                    return;
                }

                try
                {
                    foreach (var v in AddressJson.results)
                    {
                        address1C1TextBox.Text = v.address1 + v.address2 + v.address3;
                    }
                }
                catch (Exception ex)
                {
                    ChangeTopMessage("W0001", "入力された郵便番号は、郵便局");
                    this.ActiveControl = t;
                    return;
                }
                finally
                {
                    response.Close();
                    stPostalCode = t.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Eメールアドレス 検証時
        /// </summary>
        private void mailC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // メールアドレスチェック
                var result = MailCheck(t);
                if (result == false)
                {
                    this.ActiveControl = t;
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
        /// 仕入先担当者名 検証時
        /// </summary>
        /// <remarks>既存データに禁止文字が入っている場合があるので、SQL禁止文字をエラーとする</remarks>
        private void supStaffNameC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // 全角項目SQL禁止文字チェック
                var result = FullSizeSQLCheck(t, 50);
                if (result == false)
                {
                    this.ActiveControl = t;
                    e.Cancel = true;
                    return;
                }
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

            var c = processCateC1ComboBox;

            if (string.IsNullOrEmpty(c.Text))
            {
                this.ActiveControl = c;
                ChangeTopMessage("W0007", c.Label.Text);
                return false;
            }

            Func<bool> isError = () => {
                ActiveControl = c;
                ChangeTopMessage("W0013", c.Label.Text);
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
            }

            var s = processCateC1ComboBox.SGetText(1);

            // 新規・修正時
            if (s == "0")
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
            }

            // 削除時 かつ 仕入先コードが無い場合
            if (s != "0" && supCodeC1TextBox.Text == "")
            {
                var ctl = supCodeC1TextBox;
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
            var t1 = supCodeC1TextBox;
            if (string.IsNullOrEmpty(t1.Text) == false)
            {
                var result = SupCodeCK(t1);
                if (result.IsOk == false)
                {
                    ActiveControl = t1;
                    return false;
                }
            }

            var t2 = supName1C1TextBox;
            if (string.IsNullOrEmpty(t2.Text) == false)
            {
                var result = FullSizeCommonCheck(t2, 30);
                if (result == false)
                {
                    ActiveControl = t2;
                    return false;
                }
            }

            var t3 = supName2C1TextBox;
            if (string.IsNullOrEmpty(t3.Text) == false)
            {
                var result = FullSizeCommonCheck(t3, 30);
                if (result == false)
                {
                    ActiveControl = t3;
                    return false;
                }
            }

            var t4 = postalCodeC1TextBox;
            if (string.IsNullOrEmpty(t4.Text) == false)
            {
                var result = HalfSizeCommonCheck(t4, 8);
                if (result == false)
                {
                    ActiveControl = t4;
                    return false;
                }

                // 郵便番号からハイフン、ブランクを除去
                string postalCode = t4.Text.Replace("-", "").Replace(" ", "");
                if (string.IsNullOrEmpty(postalCode))
                {
                    return false;
                }

                // ハイフンを除いて数値7桁か
                var numChk = Check.IsNumeric(postalCode).Result;
                if (numChk == false || postalCode.Length != 7)
                {
                    ChangeTopMessage("W0021", "郵便番号");
                    return false;
                }
            }

            var t5 = address1C1TextBox;
            if (string.IsNullOrEmpty(t5.Text) == false)
            {
                var result = FullSizeCommonCheck(t5, 50);
                if (result == false)
                {
                    ActiveControl = t5;
                    return false;
                }
            }

            var t6 = address2C1TextBox;
            if (string.IsNullOrEmpty(t6.Text) == false)
            {
                var result = FullSizeCommonCheck(t6, 50);
                if (result == false)
                {
                    ActiveControl = t6;
                    return false;
                }
            }

            var t7 = phoneNumC1TextBox;
            if (string.IsNullOrEmpty(t7.Text) == false)
            {
                var result = HalfSizeCommonCheck(t7, 20);
                if (result == false)
                {
                    ActiveControl = t7;
                    return false;
                }
            }

            var t8 = faxC1TextBox;
            if (string.IsNullOrEmpty(t8.Text) == false)
            {
                var result = HalfSizeCommonCheck(t8, 20);
                if (result == false)
                {
                    ActiveControl = t8;
                    return false;
                }
            }

            var t9 = mailC1TextBox;
            if (string.IsNullOrEmpty(t9.Text) == false)
            {
                var result = MailCheck(t9);
                if (result == false)
                {
                    ActiveControl = t9;
                    return false;
                }
            }

            var t10 = supStaffNameC1TextBox;
            if (string.IsNullOrEmpty(t10.Text) == false)
            {
                var result = FullSizeSQLCheck(t10, 50);
                if (result == false)
                {
                    ActiveControl = t10;
                    return false;
                }
            }

            var t11 = sansoStaffNameC1TextBox;
            if (string.IsNullOrEmpty(t11.Text) == false)
            {
                var result = FullSizeCommonCheck(t11, 50);
                if (result == false)
                {
                    ActiveControl = t11;
                    return false;
                }
            }

            var af = new SupMstAF();
            var result1 = af.GetSupMstMaint(supCodeC1TextBox.Text);
            if (result1.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタの");
                return false;
            }

            var s = processCateC1ComboBox.SGetText(1);
            // 削除の場合
            if (s == "1")
            {
                if (result1.Table.Rows.Count <= 0)
                {
                    ChangeTopMessage("W0001", "仕入先マスタ");
                    return false;
                }
            }
            else
            {
                // 排他制御  仕入先マスタ
                if (supMstDTCheck.CheckDT(result1.Table) == false)
                {
                    ChangeTopMessage("E0004");
                    return false;
                }
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


                var af = new F602_SupMstMaintAF();

                // 新規と修正
                if (processCateC1ComboBox.SGetText(1) == "0")
                {
                    var result = af.UpdateSupMST(controlListII);
                    if (result == false)
                    {
                        ChangeTopMessage("E0008", "仕入先マスタ登録修正時に");
                        return;
                    }

                    DisplayClear();
                    ChangeTopMessage("I0002", "仕入先マスタ");

                }
                // 削除
                else
                {
                    DialogResult d = MessageBox.Show(
                        "仕入先コード「" + supCodeC1TextBox.Text + "」の仕入先マスタを削除してよろしいですか？",
                        "削除確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2);
                    if (d == DialogResult.No)
                    {
                        return;
                    }

                    var result = af.DeleteSupMST(controlListII);
                    if (result == false)
                    {
                        ChangeTopMessage("E0008", "仕入先マスタ削除時に");
                        return;
                    }

                    DisplayClear();
                    ChangeTopMessage("I0003", "仕入先マスタ");

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
        /*
        /// <summary>
        /// エクセル出力処理
        /// </summary>
        private void EXCELProc()
        {
            if ((excelDt == null) || (excelDt.Rows.Count <= 0))
            {
                ChangeTopMessage("I0007");
                excelDt = null;
                return;
            }

            var param = new List<(int ColumnsNum, string Format, int? Width)>();
            //param.Add((1, "", 2400));
            //param.Add((4, "yyyy/m/d;@", 1200));
            //param.Add((6, "yyyy/m/d;@", 1200));

            var cef = new CreateExcelFile(MainMenu.FileOutputPath, titleLabel.Text, excelDt);
            var result = cef.CreateSaveExcelFile(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "エクセルデータ検索時に");
                return;
            }
        }
        */

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// キー以外の入力項目の編集可否設定
        /// </summary>
        /// <param name="enable">True:入力可能にする False:入力不可にする</param>
        private void EditEnable(bool edit)
        {
            // コントロールEnable用フラグ
            bool readOnly = false;
            bool enable = true;
            Color backColor = SColDef;

            if (edit == false)
            {
                readOnly = true;
                enable = false;
                backColor = SColReadOnly;
            }

            // キー以外の入力項目の設定
            foreach (var v in controlListII.Where(v => v.Control.Name == "supName1C1TextBox" ||
                                                        v.Control.Name == "supName2C1TextBox" ||
                                                        v.Control.Name == "postalCodeC1TextBox" ||
                                                        v.Control.Name == "address1C1TextBox" ||
                                                        v.Control.Name == "address2C1TextBox" ||
                                                        v.Control.Name == "phoneNumC1TextBox" ||
                                                        v.Control.Name == "faxC1TextBox" ||
                                                        v.Control.Name == "mailC1TextBox" ||
                                                        v.Control.Name == "supStaffNameC1TextBox" ||
                                                        v.Control.Name == "sansoStaffNameC1TextBox" ||
                                                        v.Control.Name == "supCateC1ComboBox" ||
                                                        v.Control.Name == "delivSlipIssueCateC1ComboBox"
                                                        ))
            {
                var c = v.Control;
                var type = c.GetType();
                if (type == typeof(C1TextBox))
                {
                    ((C1TextBox)c).ReadOnly = readOnly;
                    c.Enabled = enable;
                    if (((C1TextBox)c).Label != null)
                    {
                        ((C1TextBox)c).Label.Enabled = true;
                    }
                    c.BackColor = backColor;
                }
                else if (type == typeof(C1ComboBox))
                {
                    ((C1ComboBox)c).ReadOnly = readOnly;
                    c.Enabled = enable;
                    if (((C1ComboBox)c).Label != null)
                    {
                        ((C1ComboBox)c).Label.Enabled = true;
                    }
                    c.BackColor = backColor;
                }
                else
                {
                    // 処理なし
                }
            }
        }

        /// <summary>
        /// 仕入先情報抽出
        /// </summary>
        /// <param name="supCode">仕入先コード</param>
        private void SetSupColumn()
        {
            ClearTopMessage();

            var s = processCateC1ComboBox.SGetText(1);
            var supCode = supCodeC1TextBox.Text;

            // 処理区分に変更がない、かつ仕入先コードが未入力 or 変更なし
            if (stProcessCate == s && (supCode == "" || stSupCode == supCode))
            {
                return;
            }

            var af = new SupMstAF();

            (bool isOk1, DataTable dt) = af.GetSupMstMaint(supCode);
            if (isOk1 == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタの");
                return;
            }

            // DTCheckクラス(排他制御)仕入先マスタ セット
            supMstDTCheck.SaveDT(dt);

            // 削除かつ仕入先コード入力無の場合
            if (s == "1" && supCode == "")
            {
                DetailClear();

                // キー以外の入力項目編集を不可にする
                EditEnable(false);

                return;
            }

            // 削除の場合は仕入先マスタの存在チェックを行う
            //if ((dt.Rows.Count <= 0) && (s == "1") && supCode != "")
            if ((dt.Rows.Count <= 0) && (s == "1"))
            {
                ChangeTopMessage("W0001", "仕入先マスタ");
                return;
            }

            // 新規の場合は画面クリア
            if (s == "0" && dt.Rows.Count <= 0)
            {
                DetailClear();

                // キー以外の入力項目編集を可能にする
                EditEnable(true);
                ActiveControl = supName1C1TextBox;

                return;
            }

            // 修正か削除の場合は仕入先マスタの値を表示する
            // テーブル情報をコントロールにセットしない物(カラム名とコントロールラベルが違うなど)を除外し
            // DataTableの内容をコントロールにセット
            var cl = ControlListII.Where(v => ((C1TextBox)v.Control).Label != null).ToList();
            if (cl.Count <= 0)
            {
                return;
            }

            foreach (DataColumn vv in dt.Columns)
            {
                var ml = cl.Where(v => ((C1TextBox)v.Control).Label.Text == vv.ColumnName);
                if (ml.Count() <= 0)
                {
                    continue;
                }

                var c = ml.First().Control;
                var type = c.GetType();
                if (type == typeof(C1.Win.Calendar.C1DateEdit))
                {
                    var dv = dt.Rows[0].Field<DateTime?>(((C1.Win.Calendar.C1DateEdit)c).Label.Text) ?? null;
                    ((C1.Win.Calendar.C1DateEdit)c).Value = dv;
                }
                else if (type == typeof(C1NumericEdit))
                {
                    var dv = dt.Rows[0].Field<decimal?>(((C1NumericEdit)c).Label.Text) ?? null;
                    ((C1NumericEdit)c).Value = dv;
                }
                else if ((type == typeof(C1TextBox)) && (c.Name.Contains("DateEdit")))
                {
                    var dv = (dt.Rows[0].Field<DateTime?>(((C1TextBox)c).Label.Text) ?? null).ToString();
                    ((C1TextBox)c).Text = dv;
                }
                else if ((type == typeof(C1TextBox)) && (c.Name.Contains("NumericEdit")))
                {
                    var dv = (dt.Rows[0].Field<decimal?>(((C1TextBox)c).Label.Text) ?? null).ToString();
                    ((C1TextBox)c).Text = dv;
                }
                else
                {
                    var dv = dt.Rows[0].Field<string>(((C1TextBox)c).Label.Text) ?? string.Empty;
                    c.Text = dv;
                }
            }

            // 上記に該当しないもの
            // 納品書発行区分
            var delivSlipIssueCate = dt.Rows[0].Field<string>("納品書発行区分");
            delivSlipIssueCateC1ComboBox.SelectedIndex = (delivSlipIssueCate == "1") ? 1 : 0;

            // 仕入先区分名
            var temp = supCateC1ComboBox.Text;
            var ds = (System.Data.DataView)supCateC1ComboBox.ItemsDataSource;
            ds.RowFilter = "ID = '" + temp + "' ";
            if (ds.Count >= 1)
            {
                supCateC1TextBox.Text = ds.ToTable().Rows[0]["NAME"].ToString();
            }

            // 変更前郵便番号
            stPostalCode = dt.Rows[0].Field<string>("郵便番号");

            // 修正の場合
            if (s == "0")
            {
                // キー以外の入力項目編集を可能にする
                EditEnable(true);
                ActiveControl = supName1C1TextBox;
            }

            return;
        }

        /// <summary>
        /// 仕入先コードチェック
        /// </summary>
        /// <param name="t">チェック対象コントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private (bool IsOk, DataTable Table) SupCodeCK(C1TextBox t)
        {
            if (Check.HasBanChar(t.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return (false, null);
            }

            if (Check.IsHalfSizeString(t.Text).Result == false)
            {
                ChangeTopMessage("W0016", "全角文字");
                return (false, null);
            }

            var af = new SupMstAF();
            var result = af.GetSupMstMaint(t.Text);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return (false, null);
            }

            return result;
        }

        /// <summary>
        /// メールアドレス チェック
        /// </summary>
        /// <param name="t">チェック対象のコントロール</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool MailCheck(C1TextBox t)
        {
            if (isRunValidating == false
                || string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            // SQL使用禁止文字のチェック
            if (Check.HasSQLBanChar(t.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 全角文字のチェック
            if (Check.IsHalfSizeString(t.Text).Result == false)
            {
                ChangeTopMessage("W0016", $"{t.Label.Text}に全角文字");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOk, string msg) = Check.IsByteRange(t.Text, 0, 100);
            if (isOk == false)
            {
                ChangeTopMessage(1, "WARN", msg);
                return false;
            }

            return true;
        }


        /// <summary>
        /// 半角項目共通チェック
        /// </summary>
        /// <param name="t">チェック対象のコントロール</param>
        /// <param name="checkByteEnd">最大バイト数</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool HalfSizeCommonCheck(C1TextBox t, int checkByteEnd)
        {
            if (isRunValidating == false
                || string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            if (Check.HasBanChar(t.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 全角文字のチェック
            if (Check.IsHalfSizeString(t.Text).Result == false)
            {
                ChangeTopMessage("W0016", $"{t.Label.Text}に全角文字");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, string msg) = Check.IsByteRange(t.Text, 0, checkByteEnd);
            if (isOK == false)
            {
                ChangeTopMessage(1, "WARN", msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 全角項目共通チェック
        /// </summary>
        /// <param name="t">チェック対象のコントロール</param>
        /// <param name="checkByteEnd">最大バイト数</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool FullSizeCommonCheck(C1TextBox t, int checkByteEnd)
        {
            if (isRunValidating == false
                || string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            if (Check.HasBanChar(t.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, string msg) = Check.IsByteRange(t.Text, 0, checkByteEnd);
            if (isOK == false)
            {
                ChangeTopMessage(1, "WARN", msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 全角項目SQL禁止文字チェック
        /// </summary>
        /// <param name="t">チェック対象のコントロール</param>
        /// <param name="checkByteEnd">最大バイト数</param>
        /// <returns>True：エラー無し False：エラー有り</returns>
        private bool FullSizeSQLCheck(C1TextBox t, int checkByteEnd)
        {
            if (isRunValidating == false
                || string.IsNullOrEmpty(t.Text))
            {
                return true;
            }

            // SQL使用禁止文字のチェック
            if (Check.HasSQLBanChar(t.Text).Result == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            (bool isOK, string msg) = Check.IsByteRange(t.Text, 0, checkByteEnd);
            if (isOK == false)
            {
                ChangeTopMessage(1, "WARN", msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 全角50バイト項目 検証時
        /// </summary>
        private void FullSize50C1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // 全角項目共通チェック
                var result = FullSizeCommonCheck(t, 50);
                if (result == false)
                {
                    this.ActiveControl = t;
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
        /// 半角20バイト項目 検証時
        /// </summary>
        private void HalfSize20C1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                // 半角項目共通チェック
                var result = HalfSizeCommonCheck(t, 20);
                if (result == false)
                {
                    this.ActiveControl = t;
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜その他処理 END＞

    }
}
