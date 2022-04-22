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
    /// 都度発注処理
    /// </summary>
    public partial class F404_EachTimeOrd : BaseForm
    {
        #region ＜フィールド＞ 

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F404/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// 依頼部門の変更前保管エリア
        /// </summary>
        private string stRequestGroupCode = "";

        /// <summary>
        /// 更新用テーブル
        /// </summary>
        private DataTable updateTable = new DataTable();

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fId"></param>
        public F404_EachTimeOrd(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "都度発注処理";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F404_EachTimeOrd_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(processCateC1ComboBox, null, "", true, enumCate.無し);
                AddControlListII(skdCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(bizCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(groupCodeC1ComboBox, groupNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(supCodeC1TextBox, supNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(supNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, partsNameC1TextBox, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(stockCateC1ComboBox, null, "", true, enumCate.無し);
                AddControlListII(delivInstructionDateC1DateEdit, null, DateTime.Today.ToShortDateString(), true, enumCate.無し);
                AddControlListII(delivInstructionNumC1NumericEdit, null, null, true, enumCate.無し);        
                AddControlListII(requestGroupCodeC1ComboBox, requestGroupNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(requestGroupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(requestStaffCodeC1ComboBox, requestStaffNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(requestStaffNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(sakubanC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(remarksC1TextBox, null, "", false, enumCate.無し);

                //API001テスト用
                AddControlListII(ordGroupCodeTestC1ComboBox, ordGroupNameTestC1TextBox, "", false, enumCate.無し);
                AddControlListII(ordGroupNameTestC1TextBox, null, "", false, enumCate.無し);
                SetOrdGroupCodeC1ComboBox();
                partsCodeTestLabel.Enabled = true;


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
                SetProcessCateC1ComboBox();
                SetGroupCodeC1ComboBox();
                SetStockCateC1ComboBox();
                SetRequestGroupCodeC1ComboBox();

                // 更新用テーブル　設定
                updateTable.CaseSensitive = true;
                updateTable.Columns.Add("instrCode", typeof(string));
                updateTable.Columns.Add("bizCode", typeof(string));
                updateTable.Columns.Add("groupCode", typeof(string));
                updateTable.Columns.Add("groupName", typeof(string));
                updateTable.Columns.Add("supCode", typeof(string));
                updateTable.Columns.Add("supName", typeof(string));
                updateTable.Columns.Add("partsCode", typeof(string));
                updateTable.Columns.Add("partsName", typeof(string));
                updateTable.Columns.Add("stockCate", typeof(string));
                updateTable.Columns.Add("delivDate", typeof(DateTime));
                updateTable.Columns.Add("delivNum", typeof(double));
                updateTable.Columns.Add("reqGroupCode", typeof(string));
                updateTable.Columns.Add("reqGroupName", typeof(string));
                updateTable.Columns.Add("reqStaffID", typeof(string));
                updateTable.Columns.Add("reqStaffName", typeof(string));
                updateTable.Columns.Add("reqStaffCode", typeof(string));
                updateTable.Columns.Add("remarks", typeof(string));
                updateTable.Columns.Add("sakuban", typeof(string));
                updateTable.Columns.Add("poCode", typeof(string));
                updateTable.Columns.Add("createDate", typeof(DateTime));
                updateTable.Columns.Add("createStaffName", typeof(string));
                updateTable.Columns.Add("updateDate", typeof(DateTime));
                updateTable.Columns.Add("updateStaffName", typeof(string));

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "必須項目入力し、実行（F10）押下後、発注内容を追加・修正できます。　　" +
                    "「削除」ボタン押下後、該当行の発注内容が削除されます。\n" +
                    "「選択」ボタン押下後、該当行の発注内容が画面に表示されます。   " +
                    "「発注確定」ボタン押下後、都度発注処理を実行します。　　";

                // クリア処理
                DisplayClear(true);
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
        /// <param name="sw">スイッチ</param>
        private void DisplayClear(bool sw)
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

            // 初期設定
            stRequestGroupCode = "";
            processCateC1ComboBox.SelectedIndex = 0;
            groupCodeC1ComboBox.SelectedIndex = -1;
            stockCateC1ComboBox.SelectedIndex = 0;
            requestGroupCodeC1ComboBox.SelectedIndex = -1;
            requestStaffCodeC1ComboBox.SelectedIndex = -1;
            DrawC1TrueDBGrid(sw);

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = processCateC1ComboBox;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜コンボボックス設定処理＞ 

        /// <summary>
        /// 処理区分  コンボボックスセット
        /// </summary>
        private void SetProcessCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("新規｜修正", "0");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(processCateC1ComboBox, dt, processCateC1ComboBox.Width,
                processCateC1ComboBox.Width, "NAME", "NAME", true);
        }

        /// <summary>
        /// 課別コード  コンボボックスセット
        /// </summary>
        private void SetGroupCodeC1ComboBox()
        {
            // 部門マスタ
            apiParam.RemoveAll();
            apiParam.Add("userId", new JValue(LoginInfo.Instance.UserId));
            var result = ApiCommonGet(apiUrl + "GetGroupEnable", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "課別コード検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                groupCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(groupCodeC1ComboBox, result.Table, groupCodeC1ComboBox.Width,
                groupNameC1TextBox.Width, "groupCode", "groupName");
        }

        /// <summary>
        /// 在庫  コンボボックスセット
        /// </summary>
        private void SetStockCateC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("素材", "");
            dt.Rows.Add("仕掛品", "Z");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(stockCateC1ComboBox, dt, stockCateC1ComboBox.Width,
                stockCateC1ComboBox.Width, "NAME", "NAME", true);
        }

        /// <summary>
        /// 依頼部門  コンボボックスセット
        /// </summary>
        private void SetRequestGroupCodeC1ComboBox()
        {
            // 部門マスタ
            apiParam.RemoveAll();
            apiParam.Add("userId", new JValue(""));
            var result = ApiCommonGet(apiUrl + "GetGroupEnable", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "依頼部門検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                requestGroupCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(requestGroupCodeC1ComboBox, result.Table, requestGroupCodeC1ComboBox.Width,
                requestGroupNameC1TextBox.Width, "groupCode", "groupName");
        }

        /// <summary>
        /// 依頼者  コンボボックスセット
        /// </summary>
        private void SetRequestStaffCodeC1ComboBox()
        {
            // M_STAFF
            apiParam.RemoveAll();
            apiParam.Add("groupCode", new JValue(requestGroupCodeC1ComboBox.Text));
            var result = ApiCommonGet(apiUrl + "GetStaffDataByGroupCode", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "依頼者検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                requestStaffCodeC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(requestStaffCodeC1ComboBox, result.Table, requestStaffCodeC1ComboBox.Width,
                requestStaffNameC1TextBox.Width, "staffCode", "staffName");
        }

        #endregion  ＜コンボボックス設定処理 END＞

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
                        supSearchButton_Click(sender, e);
                        break;

                    case "jyuyoyosokuCodeC1TextBox":
                        sakubanSearchButton_Click(sender, e);
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
        /// 仕入先コード検索ボタン押下時
        /// </summary>
        private void supSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_SupMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        supNameC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
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
        ///　需要予測番号  検索ボタン押下時
        /// </summary>
        private void sakubanSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F904_SakubanCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        sakubanC1TextBox.Text = form.row.Cells["需要予測番号"].Value.ToString();
                    }
                }
                ActiveControl = sakubanC1TextBox;
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
                DisplayClear(true);
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
        /// 依頼部門　検証された後
        /// </summary>
        private void requestGroupCodeC1ComboBox_Validated(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stRequestGroupCode == s.Text)
                {
                    return;
                }
                stRequestGroupCode = s.Text;

                // クリア
                SetRequestStaffCodeC1ComboBox();
                requestGroupNameC1TextBox.Text = "";

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                SetRequestStaffCodeC1ComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 依頼部門  選択された後
        /// </summary>
        private void requestGroupCodeC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                // 変更無ければ何もしない
                var s = (C1ComboBox)sender;
                if (stRequestGroupCode == s.Text)
                {
                    return;
                }
                stRequestGroupCode = s.Text;

                // クリア
                SetRequestStaffCodeC1ComboBox();
                requestGroupNameC1TextBox.Text = "";

                // 未入力時処理
                if (string.IsNullOrEmpty(s.Text))
                {
                    return;
                }

                // コンボボックスの共通Validated処理（戻り値あり）
                if (IsOkComboBoxValidated(sender, e) == false)
                {
                    return;
                }

                SetRequestStaffCodeC1ComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 日程番号　検証時
        /// </summary>
        private void skdCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckSkdCode();
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
        /// 業務コード　検証時
        /// </summary>
        private void bizCodeC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckBizCode();
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

                var isOk1 = ErrorCheckSupCode();
                if (isOk1 == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
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

                var isOk1 = ErrorCheckPartsCode();
                if (isOk1 == false)
                {
                    ActiveControl = (C1TextBox)sender;
                    e.Cancel = true;
                    return;
                }

                var isOk2 = ErrorCheckPartsCodeAndSupCode();
                if (isOk2 == false)
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
        /// 納入指示数　検証時
        /// </summary>
        private void delivInstructionNumC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckDelivInstructionNum();
                if (isOk == false)
                {
                    ActiveControl = (C1NumericEdit)sender;
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
        /// 需番/作番　検証時
        /// </summary>
        private void sakubanC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (isRunValidating == false)
                {
                    return;
                }

                var isOk = ErrorCheckSakuban();
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

                var isOk = ErrorCheckRemarks();
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
        /// c1TrueDBGrid　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName == "選択")
                {
                    skdCodeC1TextBox.Text = grid[row, "instrCode"].ToString().TrimEnd();
                    bizCodeC1TextBox.Text = grid[row, "bizCode"].ToString().TrimEnd();
                    groupCodeC1ComboBox.Text = grid[row, "groupCode"].ToString().TrimEnd();
                    groupNameC1TextBox.Text = grid[row, "groupName"].ToString().TrimEnd();

                    supCodeC1TextBox.Text = grid[row, "supCode"].ToString().TrimEnd();
                    supNameC1TextBox.Text = grid[row, "supName"].ToString().TrimEnd();
                    partsCodeC1TextBox.Text = grid[row, "partsCode"].ToString().TrimEnd();
                    partsNameC1TextBox.Text = grid[row, "partsName"].ToString().TrimEnd();
                    stockCateC1ComboBox.Text = (grid[row, "stockCate"].ToString().TrimEnd() == "" ? "素材" : "仕掛品");

                    if (grid[row, "delivDate"].ToString() == "")
                    {
                        delivInstructionDateC1DateEdit.Value = null;
                    }
                    else
                    {
                        delivInstructionDateC1DateEdit.Value = DateTime.Parse(grid[row, "delivDate"].ToString());
                    }

                    delivInstructionNumC1NumericEdit.Value = decimal.Parse(grid[row, "delivNum"].ToString());
                    requestGroupCodeC1ComboBox.Text = grid[row, "reqGroupCode"].ToString().TrimEnd();
                    requestGroupNameC1TextBox.Text = grid[row, "reqGroupName"].ToString().TrimEnd();
                    SetRequestStaffCodeC1ComboBox();
                    requestStaffCodeC1ComboBox.Text = grid[row, "reqStaffCode"].ToString().TrimEnd();
                    requestStaffNameC1TextBox.Text = grid[row, "reqStaffName"].ToString().TrimEnd();
                    sakubanC1TextBox.Text = grid[row, "sakuban"].ToString().TrimEnd();
                    poCodeC1TextBox.Text = grid[row, "poCode"].ToString().TrimEnd();
                    remarksC1TextBox.Text = grid[row, "remarks"].ToString().TrimEnd();
                }
                else if(colName == "削除")
                {
                    var instrCode = grid[row, "instrCode"].ToString().TrimEnd();

                    var dialog = MessageBox.Show("削除してよろしいですか？"
                                                 + Environment.NewLine
                                                 + Environment.NewLine + "　日程番号：" + instrCode,
                                                 "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        return;
                    }

                    // データベース更新
                    apiParam.RemoveAll();
                    apiParam.Add("instrCode", new JValue(instrCode));
                    var result = ApiCommonUpdate(apiUrl + "DeleteOrdSkdMst", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "W発注日程マスタ削除時に");
                        return;
                    }

                    foreach (DataRow v in updateTable.Rows)
                    {
                        if(v["instrCode"].ToString() == instrCode) 
                        {
                            updateTable.Rows.Remove(v);
                            break;
                        }        
                    }

                    // 画面クリア
                    DisplayClear(false);
                    ChangeTopMessage("I0003", "W発注日程マスタ");
                }
                else
                {
                    //処理なし
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

        /// <summary>
        /// 非連携列のテキストを設定
        /// </summary>
        private void c1TrueDBGrid_UnboundColumnFetch(object sender, C1.Win.C1TrueDBGrid.UnboundColumnFetchEventArgs e)
        {
            switch (e.Column.Caption)
            {
                case "選択":
                    e.Value = "選択";
                    break;

                case "削除":
                    e.Value = "削除";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 発注確定ボタン押下後、都度発注処理を実行
        /// </summary>
        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;
                ClearTopMessage();

                if (c1TrueDBGrid.RowCount <= 0)
                {
                    ChangeTopMessage("W0017", "更新");
                    return;
                }

                if (c1TrueDBGrid.RowCount != updateTable.Rows.Count)
                {
                    ChangeTopMessage("W0015", "更新データ");
                    return;
                }

                foreach (DataRow v in updateTable.Rows)
                {
                    if (v["bizCode"].ToString().TrimEnd() == "")
                    {
                        ChangeTopMessage("W0007", "業務コード");
                        return;
                    }
                }

                Cursor = Cursors.WaitCursor;

                // データベース更新
                apiParam.RemoveAll();
                string json = JsonConvert.SerializeObject(updateTable, Formatting.Indented);
                apiParam.Add("tableJson", new JValue(json));
                var result = ApiCommonUpdate(apiUrl + "InsertBizSkd", apiParam);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "都度日程登録時に");
                    return;
                }

                // 画面クリア
                DisplayClear(true);
                ChangeTopMessage("I0001", "都度日程");
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
            // エラーチェック 日程番号
            var isOk1 = ErrorCheckSkdCode();
            if (isOk1 == false)
            {
                ActiveControl = skdCodeC1TextBox;
                return false;
            }

            // エラーチェック 業務コード
            var isOk2 = ErrorCheckBizCode();
            if (isOk2 == false)
            {
                ActiveControl = bizCodeC1TextBox;
                return false;
            }

            // エラーチェック 仕入先コード
            var isOk3 = ErrorCheckSupCode();
            if (isOk3 == false)
            {
                ActiveControl = supCodeC1TextBox;
                return false;
            }

            // エラーチェック 部品コード
            var isOk4 = ErrorCheckPartsCode();
            if (isOk4 == false)
            {
                ActiveControl = partsCodeC1TextBox;
                return false;
            }

            // エラーチェック 部品コード＆仕入先コード
            var isOk5 = ErrorCheckPartsCodeAndSupCode();
            if (isOk5 == false)
            {
                return false;
            }

            // エラーチェック 納入指示数
            var isOk6 = ErrorCheckDelivInstructionNum();
            if (isOk6 == false)
            {
                ActiveControl = delivInstructionNumC1NumericEdit;
                return false;
            }

            // エラーチェック 需番/作番
            var isOk7 = ErrorCheckSakuban();
            if (isOk7 == false)
            {
                ActiveControl = sakubanC1TextBox;
                return false;
            }

            // エラーチェック 注文番号
            var isOk8 = ErrorCheckPoCode();
            if (isOk8 == false)
            {
                ActiveControl = poCodeC1TextBox;
                return false;
            }

            // エラーチェック 備考
            var isOk9 = ErrorCheckRemarks();
            if (isOk9 == false)
            {
                ActiveControl = remarksC1TextBox;
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

                if (processCateC1ComboBox.SGetText(1) == "0")
                {
                    // データベース更新
                    apiParam.RemoveAll();
                    apiParam.Add("instrCode", new JValue(skdCodeC1TextBox.Text));
                    apiParam.Add("bizCode", new JValue(bizCodeC1TextBox.Text));
                    apiParam.Add("groupCode", new JValue(groupCodeC1ComboBox.Text));
                    apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
                    apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                    apiParam.Add("partsName", new JValue(partsNameC1TextBox.Text));
                    apiParam.Add("stockCate", new JValue(stockCateC1ComboBox.SGetText(1)));
                    apiParam.Add("delivDate", new JValue(delivInstructionDateC1DateEdit.Text));
                    apiParam.Add("delivNum", new JValue(delivInstructionNumC1NumericEdit.Text));
                    apiParam.Add("reqGroupCode", new JValue(requestGroupCodeC1ComboBox.Text));
                    apiParam.Add("reqStaffCode", new JValue(requestStaffCodeC1ComboBox.Text));
                    apiParam.Add("remarks", new JValue(remarksC1TextBox.Text));
                    var result = ApiCommonUpdate(apiUrl + "UpdateOrdSkdMst", apiParam);
                    if (result.IsOk == false)
                    {
                        ChangeTopMessage("E0008", "W発注日程マスタ登録修正時に");
                        return;
                    }

                    DataRow dr = updateTable.NewRow();
                    dr["instrCode"] = skdCodeC1TextBox.Text;
                    dr["bizCode"] = bizCodeC1TextBox.Text;
                    dr["groupCode"] = groupCodeC1ComboBox.Text;
                    dr["groupName"] = groupNameC1TextBox.Text;
                    dr["supCode"] = supCodeC1TextBox.Text;
                    dr["supName"] = supNameC1TextBox.Text;
                    dr["partsCode"] = partsCodeC1TextBox.Text;
                    dr["partsName"] = partsNameC1TextBox.Text;
                    dr["stockCate"] = stockCateC1ComboBox.SGetText(1);
                    dr["delivDate"] = delivInstructionDateC1DateEdit.Text;
                    dr["delivNum"] = delivInstructionNumC1NumericEdit.Text;
                    dr["reqGroupCode"] = requestGroupCodeC1ComboBox.Text;
                    dr["reqGroupName"] = requestGroupNameC1TextBox.Text;
                    dr["reqStaffCode"] = requestStaffCodeC1ComboBox.Text;
                    dr["reqStaffName"] = requestStaffNameC1TextBox.Text;
                    dr["remarks"] = remarksC1TextBox.Text;
                    dr["sakuban"] = sakubanC1TextBox.Text;
                    dr["poCode"] = poCodeC1TextBox.Text;
                    updateTable.Rows.InsertAt(dr, 0);

                    // 画面クリア
                    DisplayClear(false);
                    ChangeTopMessage("I0002", "W発注日程マスタ");
                }

                ActiveControl = processCateC1ComboBox;
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
        /// c1TrueDBGrid　描画
        /// </summary>
        /// <param name="sw">スイッチ</param>
        private void DrawC1TrueDBGrid(bool sw)
        {
            // クリア
            c1TrueDBGrid.SetDataBinding(null, "", true);

            // MW_OrdSkdMst
            var result = ApiCommonGet(apiUrl + "GetOrdSkdMst", null);
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
            ChangeTopMessage("I0011", result.Table.Rows.Count.ToString("#,###"));

            if (sw) 
            {
                // 更新用テーブル　設定
                updateTable = result.Table.Copy();
            }
        }

        /// <summary>
        /// エラーチェック  日程番号
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSkdCode()
        {
            // 未入力時処理
            var s = skdCodeC1TextBox;
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
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // 仕入先マスタ
            var param = new SansoBase.SupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.SupCode, s.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return false;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "仕入先マスタ");
                return false;
            }

            supNameC1TextBox.Text = result.Table.Rows[0]["仕入先名１"].ToString();

            return true;
        }

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
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // SANSODB.dbo.M_OrderParts
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(s.Text));
            var result = ApiCommonGet(apiUrl + "GetOrderPartsMst", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "部品マスタ検索時に");
                return false;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", s.Label.Text, "部品マスタ");
                return false;
            }

            partsNameC1TextBox.Text = result.Table.Rows[0]["partsName"].ToString();

            return true;
        }

        /// <summary>
        /// エラーチェック  部品コード＆仕入先コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckPartsCodeAndSupCode()
        {
            // 未入力時処理
            if (partsCodeC1TextBox.Text == "" || supCodeC1TextBox.Text == "")
            {
                return true;
            }

            // 単価マスタ
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
            apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
            var result2 = ApiCommonGet(apiUrl + "GetOrderUnitPriceMst", apiParam);
            if (result2.IsOk == false)
            {
                ChangeTopMessage("E0008", "単価マスタ検索時に");
                return false;
            }
            if (result2.Table == null || result2.Table.Rows.Count <= 0)
            {
                ChangeTopMessage("W0002", "部品コードと仕入先コード", "単価マスタ");
                return false;
            }

            double supUnitPrice = result2.Table.Rows[0].Field<double?>("supUnitPrice") ?? 0d;
            string unitPriceCate = result2.Table.Rows[0]["unitPriceCate"].ToString().TrimEnd();
            DateTime? unitPriceEnableDate = result2.Table.Rows[0].Field<DateTime?>("unitPriceEnableDate") ?? null;

            if (supUnitPrice <= 0d)
            {
                ChangeTopMessage("W0016", "仕入単価には０以下");
                return false;
            }

            // COSTDB.dbo.CT_PRICE
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
            apiParam.Add("supCode", new JValue(supCodeC1TextBox.Text));
            var result3 = ApiCommonGet(apiUrl + "GetCostUnitPriceMst", apiParam);
            if (result3.IsOk == false)
            {
                ChangeTopMessage("E0008", "CT_PRICE検索時に");
                return false;
            }
            if (result3.Table == null || result3.Table.Rows.Count <= 0)
            {
                if (unitPriceCate == "1" && unitPriceEnableDate != null && unitPriceEnableDate < DateTime.Today)
                {
                    ChangeTopMessage(1, "WARN", "見積原価が未決定です。確認してください。");
                    return false;
                }
                return true;
            }

            double status = result3.Table.Rows[0].Field<double?>("STATUS") ?? 0d;
            DateTime? ordDate = result3.Table.Rows[0].Field<DateTime?>("HACCYU_YMD_MIN") ?? null;
            double sum = result3.Table.Rows[0].Field<double?>("PRICE") ?? 0d
                + result3.Table.Rows[0].Field<double?>("SUPPLY_COST") ?? 0d;

            if (status == 3d || status == 4d)
            {
                // 仮単価初回発注日 < 1ヶ月前　仮単価で発注できるのは 1ヶ月前間のみ
                if (ordDate != null && ordDate < DateTime.Today.AddMonths(-1))
                {
                    ChangeTopMessage(1, "WARN", "仮単価初回発注日より１ヶ月越えています。");
                    return false;
                }
            }

            if (unitPriceCate == "1"
                && status == 0d
                && sum != supUnitPrice
                && unitPriceEnableDate != null
                && unitPriceEnableDate < DateTime.Today)
            {
                ChangeTopMessage(1, "WARN", "仕入単価と見積原価が違います。確認してください。");
                return false;
            }

            if (unitPriceCate == "1"
                && status != 0d
                && unitPriceEnableDate != null
                && unitPriceEnableDate < DateTime.Today)
            {
                ChangeTopMessage(1, "WARN", "見積原価が未決定です。確認してください。");
                return false;
            }

            if (unitPriceCate != "1"
                && unitPriceEnableDate != null
                && unitPriceEnableDate < DateTime.Today)
            {
                ChangeTopMessage(1, "WARN", "仕入単価が未決定です。確認してください。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  納入指示数
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckDelivInstructionNum()
        {
            // 未入力チェック
            var s = delivInstructionNumC1NumericEdit;
            if (s.Text == "")
            {
                return true;
            }

            // 数値か
            var chk1 = Check.IsNumeric(s.Text);
            if (chk1.Result == false)
            {
                ChangeTopMessage("W0019", s.Label.Text + "には");
                return false;
            }

            decimal value = decimal.Parse(s.Text);

            // 正数か
            if (value < 0m)
            {
                ChangeTopMessage("W0006", s.Label.Text);
                return false;
            }

            // 桁数チェック
            var chk2 = Check.IsPointNumberRange(value, 7, 0);
            if (chk2.Result == false)
            {
                ChangeTopMessage(1, "WARN", s.Label.Text + "の" + chk2.Msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  業務コード
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckBizCode()
        {
            // 未入力時処理
            var s = bizCodeC1TextBox;
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

            return true;
        }

        /// <summary>
        /// エラーチェック  需番/作番
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckSakuban()
        {
            // 未入力時処理
            var s = sakubanC1TextBox;
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
            var isOk = Check.HasSQLBanChar(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            return true;
        }

        /// <summary>
        /// エラーチェック  備考
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckRemarks()
        {
            // 未入力時処理
            var s = remarksC1TextBox;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk1 = Check.HasSQLBanChar(s.Text).Result;
            if (isOk1 == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            // バイト数の範囲チェック
            var isOk2 = Check.IsByteRange(s.Text, 0, 255).Result;
            if (isOk2 == false)
            {
                ChangeTopMessage("W0009", "備考", "0", "255");
                return false;
            }

            return true;
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0], エラーメッセージ)</returns>
        private (bool IsOk, int Count, string Msg) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
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
                        return (false, 0, (string)(result["msg"]));
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0, (string)(result["msg"]));
            }

            if ((bool)result["isOk"] == false)
            {
                return (false, 0, (string)(result["msg"]));
            }

            return (
                true,
                (int)(result["count"]),
                ""
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

            if ((bool)result["isOk"] == false)
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

        #region ＜API001テスト＞ 

        /// <summary>
        /// API001テスト　実行処理
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult dResult =
                MessageBox.Show(this, "テストAPI001を実行します。発注明細、発注マスタにデータが作成されます。実行してもよろしいですか？",
                "確認", buttons, MessageBoxIcon.Question);
            if (dResult == DialogResult.Yes)
            {
                apiParam.RemoveAll();
                apiParam.Add("DBName", new JValue("製造調達"));
                /*
                apiParam.Add("partsCode", new JValue("570000130"));
                apiParam.Add("supCode", new JValue("1005"));
                apiParam.Add("num", new JValue("1"));
                apiParam.Add("jyuyoyosokuCode", new JValue(""));
                apiParam.Add("delivDate", new JValue("2021/05/14"));
                apiParam.Add("ordGroup", new JValue(""));
                apiParam.Add("ordCate", new JValue("Y"));
                */
                apiParam.Add("partsCode", new JValue(partsCodeTestC1TextBox.Text));
                apiParam.Add("supCode", new JValue(supCodeTestC1TextBox.Text));
                apiParam.Add("num", new JValue(delivNumTestC1NumericEdit.Text));
                apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeTestC1TextBox.Text));
                apiParam.Add("delivDate", new JValue(delivDateTestC1DateEdit.Text));
                apiParam.Add("ordGroup", new JValue(ordGroupCodeTestC1ComboBox.Text));
                apiParam.Add("ordCate", new JValue(ordCateTestC1TextBox.Text));

                apiParam.Add("createId", new JValue(LoginInfo.Instance.UserId));
                apiParam.Add("createDate", new JValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                apiParam.Add("userNo", new JValue(LoginInfo.Instance.UserNo));
                apiParam.Add("machineCode", new JValue(LoginInfo.Instance.MachineCode));

                var result1 = CallSansoWebAPI("POST", apiUrl + "CreateOrdMst", apiParam);

                if (result1.IsOk == false)
                {
                    ChangeTopMessage("E0008", "発注明細、発注マスタ更新時に");
                    return;
                }

                ChangeTopMessage("I0001", "製造調達の発注明細、発注マスタ");
            }

        }

        /// <summary>
        /// 部品コード検索　テスト用
        /// </summary>
        private void partsSearchBt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F902_PartsMCommonSearch("F902_PartsMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        partsCodeTestC1TextBox.Text = form.row.Cells["部品コード"].Value.ToString();
                        partsNameTestC1TextBox.Text = form.row.Cells["部品名"].Value.ToString();
                    }
                }
                this.ActiveControl = partsCodeTestC1TextBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion  ＜API001テスト END＞

        /// <summary>
        /// 仕入先コード検索　テスト用
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new F903_SupMCommonSearch("F903_SupMCommonSearch"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        supCodeTestC1TextBox.Text = form.row.Cells["仕入先コード"].Value.ToString();
                        supNameTestC1TextBox.Text = form.row.Cells["仕入先名１"].Value.ToString();
                    }
                }
                ActiveControl = supCodeTestC1TextBox;
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
        /// 部品コード　検証時
        /// </summary>
        private void partsCodeTestC1TextBox_Validating(object sender, CancelEventArgs e)
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
        /// 部品マスタ　検証後
        /// </summary>
        private void partsCodeTestC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    partsNameTestC1TextBox.Text = "";
                    return;
                }

                var param = new SansoBase.PartsMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.PartsCode, t.Text));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ActiveControl = t;
                    ChangeTopMessage("E0008", "部品マスタ検索時に");
                    return;
                }
                if (result.Table.Rows.Count <= 0)
                {
                    ActiveControl = t;
                    ChangeTopMessage("W0002", t.Label.Text, "部品マスタ");
                    return;
                }
                partsNameTestC1TextBox.Text = result.Table.Rows[0]["部品名"].ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 部品コードチェック
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool PartsCodeErrorCheck()
        {
            try
            {
                var t = partsCodeTestC1TextBox;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return true;
                }

                if (Check.HasSQLBanChar(t.Text).Result == false)
                {
                    ChangeTopMessage("W0018");
                    return false;
                }

                /*
                if (Check.IsByteRange(obj.Text, 0, 4).Result == false)
                {
                    ChangeTopMessage("W0009", obj.Label.Text, "0", "4");
                    return false;
                }
                */

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;
        }


        /// <summary>
        /// 仕入先コード　検証時
        /// </summary>
        private void supCodeTestC1TextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    supNameTestC1TextBox.Text = "";
                    return;
                }

                // エラーチェック　仕入先コード                
                var isOk1 = SupCodeErrorCheck();
                if (isOk1 == false)
                {
                    ActiveControl = t;
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
        /// 仕入先コード　検証後
        /// </summary>
        private void supCodeTestC1TextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                var t = (C1TextBox)sender;

                if (isRunValidating == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(t.Text))
                {
                    supNameTestC1TextBox.Text = "";
                    return;
                }

                // 未入力時処理        
                if (string.IsNullOrEmpty(t.Text))
                {
                    return;
                }

                // 仕入先マスタ
                var param = new SansoBase.SupMst();
                param.SelectStr = "*";
                param.WhereColuList.Add((param.SupCode, t.Text));
                param.SetDBName("製造調達");
                var result = CommonAF.ExecutSelectSQL(param);
                if (result.IsOk == false)
                {
                    ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                    ActiveControl = t;
                    return;
                }
                supNameTestC1TextBox.Text = result.Table.Rows[0]["仕入先名１"].ToString();

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
        private bool SupCodeErrorCheck()
        {
            // 未入力時処理
            var obj = supCodeTestC1TextBox;
            if (string.IsNullOrEmpty(obj.Text))
            {
                return true;
            }

            // 使用禁止文字
            var chk = Check.HasSQLBanChar(obj.Text).Result;
            if (chk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            if (Check.IsByteRange(obj.Text, 0, 4).Result == false)
            {
                ChangeTopMessage("W0009", obj.Label.Text, "0", "4");
                return false;
            }

            // 仕入先マスタ
            var param = new SansoBase.SupMst();
            param.SelectStr = "*";
            param.WhereColuList.Add((param.SupCode, obj.Text));
            param.SetDBName("製造調達");
            var result = CommonAF.ExecutSelectSQL(param);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "仕入先マスタ検索時に");
                return false;
            }
            if (result.Table.Rows.Count == 0)
            {
                ChangeTopMessage("W0002", obj.Label.Text, "仕入先マスタ");
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
            try
            {
                var t = jyuyoyosokuCodeTestC1TextBox;
                if (string.IsNullOrEmpty(t.Text))
                {
                    return true;
                }

                if (Check.HasSQLBanChar(t.Text).Result == false)
                {
                    ChangeTopMessage("W0018");
                    return false;
                }

                /*
                if (Check.IsByteRange(obj.Text, 0, 4).Result == false)
                {
                    ChangeTopMessage("W0009", obj.Label.Text, "0", "4");
                    return false;
                }
                */

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return true;
        }

        /// <summary>
        /// 発注部門  コンボボックスセット
        /// </summary>
        private void SetOrdGroupCodeC1ComboBox()
        {
            // 部門マスタ
            apiParam.RemoveAll();
            apiParam.Add("userId", new JValue(""));
            var result = ApiCommonGet(apiUrl + "GetGroupEnable", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "発注部門検索時に");
                return;
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                ordGroupCodeTestC1ComboBox.ItemsDataSource = null;
                return;
            }

            result.Table.CaseSensitive = true;
            ControlAF.SetC1ComboBox(ordGroupCodeTestC1ComboBox, result.Table, ordGroupCodeTestC1ComboBox.Width,
                ordGroupCodeTestC1ComboBox.Width, "groupCode", "groupName");
        }

        /// <summary>
        /// クリアボタン　テスト用
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            partsCodeTestC1TextBox.Text = "";
            partsNameTestC1TextBox.Text = "";
            supCodeTestC1TextBox.Text = "";
            supNameTestC1TextBox.Text = "";
            delivNumTestC1NumericEdit.Value = "";
            jyuyoyosokuCodeTestC1TextBox.Text = "";
            delivDateTestC1DateEdit.Clear();
            ordGroupCodeTestC1ComboBox.Text = "";
            ordGroupNameTestC1TextBox.Text = "";

        }
    }
}