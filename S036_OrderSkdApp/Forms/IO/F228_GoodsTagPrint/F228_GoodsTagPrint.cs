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
    /// 現品票未発行分発行処理
    /// </summary>
    public partial class F228_GoodsTagPrint : BaseForm
    {
        #region ＜フィールド＞

        /// <summary>
        /// C1Report接続文字列
        /// </summary>
        private string reportConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"];

        /// <summary>
        /// WEBAPI接続先
        /// </summary>
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Solution/S036/F228/";

        /// <summary>
        /// WEBAPIに渡すパラメータ
        /// </summary>
        private JObject apiParam = new JObject();

        /// <summary>
        /// M_TagCode更新用DT
        /// </summary>
        private DataTable M_TagCodeDT = null;

        /// <summary>
        /// 現品票作成用のワークTagCode
        /// </summary>
        string wTagCode = "";

        /// <summary>
        /// 最大箱連番保持
        /// </summary>
        private int maxBoxSerial = 0;

        /// <summary>
        /// 発注元コンボボックスのインデックス番号保持
        /// </summary>
        private int orderGroupIndex = 0;

        #endregion  ＜フィールド END＞

        #region ＜起動処理＞ 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public F228_GoodsTagPrint(string fId) : base(fId)
        {
            InitializeComponent();
            titleLabel.Text = "現品票発行処理(未発行分)";
        }

        /// <summary>
        /// ロード処理
        /// </summary>
        private void F228_GoodsTagPrint_Load(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

                // コントロールリストをセット
                AddControlListII(doCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(partsCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(partsNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(cusCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(cusNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(delivNumC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(poCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(groupCodeC1TextBox, null, "", true, enumCate.無し);
                AddControlListII(groupNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(sakubanC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(jyuyoyosokuCodeC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(numC1NumericEdit, null, "", true, enumCate.無し);
                AddControlListII(possibleNumC1NumericEdit, null, "", false, enumCate.無し);
                AddControlListII(nxtCusCodeC1ComboBox, nxtCusNameC1TextBox, "", false, enumCate.無し);
                AddControlListII(nxtCusNameC1TextBox, null, "", false, enumCate.無し);
                AddControlListII(orderGroupC1ComboBox, null, "", true, enumCate.無し);

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

                //// c1TrueDBGrid1の設定(参考)
                //// ビューのすべての列の編集を禁止する
                //foreach (C1.Win.C1TrueDBGrid.C1DisplayColumn v in this.c1TrueDBGrid1.Splits[0].DisplayColumns)
                //{
                //    v.Locked = true;
                //}

                //// 「選択」列のみ編集を可能にする
                //c1TrueDBGrid1.Splits[0].DisplayColumns["選択"].Locked = false;

                //// c1TrueDBGrid1のコンボボックス設定
                //C1.Win.C1TrueDBGrid.C1DataColumn col1;
                //col1 = c1TrueDBGrid1.Columns["選択"];
                //col1.ValueItems.Values.Clear();
                //col1.ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.ComboBox;
                //col1.ValueItems.Translate = true;

                // コンボボックスセット、コンボボックスの内容をセットするメソッドは１コントロールずつに分ける
                SetorderGroupC1ComboBox();

                // 文字サイズを変更
                buttomMessageLabel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                // DefaultButtomMessageをセット
                defButtomMessage = "上部一覧から現品票発行したい項目を選択してください。" + 
                    "発行数を入力して「追加」を押下すると画面下部に追加されます。 " + Environment.NewLine +
                    "発行するデータ作成後「実行(F10)」を押下すると、現品票が発行されます。";

                // M_TagCode更新用DTの列作成
                M_TagCodeDT = new DataTable();
                M_TagCodeDT.Columns.Add("bizCode", typeof(string));
                M_TagCodeDT.Columns.Add("tagCode", typeof(string));
                M_TagCodeDT.Columns.Add("skdCode", typeof(string));
                M_TagCodeDT.Columns.Add("doCode", typeof(string));
                M_TagCodeDT.Columns.Add("partsCode", typeof(string));
                M_TagCodeDT.Columns.Add("delivNum", typeof(string));
                M_TagCodeDT.Columns.Add("poCode", typeof(string));
                M_TagCodeDT.Columns.Add("cusCode", typeof(string));
                M_TagCodeDT.Columns.Add("groupCode", typeof(string));
                M_TagCodeDT.Columns.Add("nxtCusCode", typeof(string));
                M_TagCodeDT.Columns.Add("partsName", typeof(string));
                M_TagCodeDT.Columns.Add("groupName", typeof(string));
                M_TagCodeDT.Columns.Add("cusName", typeof(string));
                M_TagCodeDT.Columns.Add("nxtCusName", typeof(string));
                M_TagCodeDT.Columns.Add("nxtCusCodeVisible", typeof(string));
                M_TagCodeDT.Columns.Add("sakuban", typeof(string));
                M_TagCodeDT.Columns.Add("PackingBoxSerial", typeof(int));
                M_TagCodeDT.Columns.Add("PackingBoxNum", typeof(int));

                // 現品票番号を非表示
                c1TrueDBGrid1.Splits[0].DisplayColumns["現品票番号"].Visible = false;

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
            if (M_TagCodeDT.Rows.Count > 0)
            {
                var dialog = MessageBox.Show("まだ登録完了していない現品票データがあります。削除してもよろしいですか？",
                             "現品票削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                             MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    return;
                }
            }

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
                    if(v.Initial == "")
                    {
                        ((C1NumericEdit)c).Value = null;
                    }
                    else
                    {
                        ((C1NumericEdit)c).Value = v.Initial;
                    }
                }
                else
                {
                    ((C1TextBox)c).Text = v.Initial;
                }
            }

            // 初期設定
            c1TrueDBGrid.SetDataBinding(null, "", true);
            c1TrueDBGrid1.SetDataBinding(null, "", true);
            wTagCode = "";
            maxBoxSerial = 0;
            inspectionMsgLabel.Text = "";
            orderGroupC1ComboBox.SelectedIndex = (orderGroupIndex < 0 ? 0: orderGroupIndex) ;

            M_TagCodeDT.Clear();

            // C1TrueDBGrid描画
            var result = C1TrueDBGrid_Drawing();
            if(result.isOk == false)
            {
                ChangeTopMessage("E0008", "現品票未発行データ検索時に");
                return;
            }

            c1TrueDBGrid.SetDataBinding(result.dt, "", true);

            // トップメッセージクリア　
            ClearTopMessage();

            // ボトムメッセージに初期値設定　
            buttomMessageLabel.Text = defButtomMessage;

            // フォームオープン時のアクティブコントロールを設定
            ActiveControl = numC1NumericEdit;
        }

        #endregion  ＜クリア処理 END＞

        #region ＜コンボボックス設定処理＞ 
        /// <summary>
        /// 発注元コンボボックスセット
        /// </summary>
        private void SetorderGroupC1ComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Rows.Add("調達", "製造調達");
            dt.Rows.Add("岡山", "製造熊山");
            dt.Rows.Add("切削係", "機械二課");
            dt.Rows.Add("ロータ係", "ロータ管理");
            dt.CaseSensitive = true;
            ControlAF.SetC1ComboBox(orderGroupC1ComboBox, dt, orderGroupC1ComboBox.Width,
                orderGroupC1ComboBox.Width, "NAME", "NAME", true);
        }

        /// <summary>
        /// 次工程  コンボボックスセット
        /// </summary>
        private void SetNxtCusCodeC1ComboBox()
        {
            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
            apiParam.Add("supCode", new JValue(cusCodeC1TextBox.Text));
            apiParam.Add("jyuyoyosokuCode", new JValue(jyuyoyosokuCodeC1TextBox.Text));
            apiParam.Add("sakuban", "");

            // 次工程一覧抽出
            var result = ApiCommonGet(apiUrl + "GetNexCusCode?sid=S036&fid=F228", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "次工程検索時に");
                return;
            }
            DataTable dt = new DataTable();

            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                dt.Columns.Add("nxtCusCode", typeof(string));
                dt.Columns.Add("nxtCusCodeName", typeof(string));
            }
            else
            {
                dt = result.Table;
            }
            dt.Rows.Add("", "次工程なし");

            dt.CaseSensitive = true;

            ControlAF.SetC1ComboBox(nxtCusCodeC1ComboBox, dt, nxtCusCodeC1ComboBox.Width,
                nxtCusNameC1TextBox.Width, "nxtCusCode", "nxtCusCodeName");
        }

        #endregion  ＜コンボボックス設定処理 END＞

        #region ＜共通イベント処理＞ 

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

        #endregion  ＜共通イベント処理 END＞

        #region ＜イベント処理＞ 

        /// <summary>
        /// 発行数　検証前
        /// </summary>
        private void numC1NumericEdit_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                // 未入力チェック
                if (string.IsNullOrEmpty(numC1NumericEdit.Text) || string.IsNullOrEmpty(doCodeC1TextBox.Text) ||
                    numC1NumericEdit.Text == "0")
                {
                    ClearTopMessage();
                    return;
                }

                // エラーチェック  数量
                if (this.ErrorCheckNum() == false)
                {
                    this.numC1NumericEdit.Focus();
                    return;
                }

                ClearTopMessage();
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

                if (colName != "選択")
                {
                    return;
                }

                if (M_TagCodeDT.Rows.Count > 0) 
                {
                    var dialog = MessageBox.Show("まだ登録完了していない現品票データがあります。削除してもよろしいですか？",
                                 "現品票削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                 MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        return;
                    }
                }

                M_TagCodeDT.Clear();
                c1TrueDBGrid1.SetDataBinding(null, "", true);
                inspectionMsgLabel.Text = "";

                doCodeC1TextBox.Text = grid[row, "doCode"].ToString().TrimEnd();
                partsCodeC1TextBox.Text = grid[row, "partsCode"].ToString().TrimEnd();
                partsNameC1TextBox.Text = grid[row, "partsName"].ToString().TrimEnd();
                delivNumC1NumericEdit.Text = grid[row, "delivNum"].ToString().TrimEnd();
                poCodeC1TextBox.Text = grid[row, "poCode"].ToString().TrimEnd();
                cusCodeC1TextBox.Text = grid[row, "cusCode"].ToString().TrimEnd();
                cusNameC1TextBox.Text = grid[row, "cusName"].ToString().TrimEnd();
                groupCodeC1TextBox.Text = grid[row, "groupCode"].ToString().TrimEnd();
                groupNameC1TextBox.Text = grid[row, "groupName"].ToString().TrimEnd();
                sakubanC1TextBox.Text = grid[row, "sakuban"].ToString().TrimEnd();
                jyuyoyosokuCodeC1TextBox.Text = grid[row, "jyuyoyosokuCode"].ToString().TrimEnd();
                
                wTagCode = "2900" + DateTime.Now.ToString("yyyyMMddHHmmss");

                //既存のM_TagCodeの合計数取得
                var result2 = GetTagCodeTotal(doCodeC1TextBox.Text);
                if (result2.isOk == false)
                {
                    ChangeTopMessage("E0008", "合計値検索時に");
                    return;
                }
                if (result2.count <= 0)
                {
                    possibleNumC1NumericEdit.Value = delivNumC1NumericEdit.Value;
                }
                else
                {
                    possibleNumC1NumericEdit.Value = result2.possibleNum;
                }

                // 初品検査等の情報取得
                apiParam.RemoveAll();
                apiParam.Add("supCode", new JValue(grid[row, "cusCode"].ToString()));
                apiParam.Add("partsCode", new JValue(grid[row, "partsCode"].ToString().TrimEnd()));
                apiParam.Add("jyuyoyosokuCode", new JValue(grid[row, "jyuyoyosokuCode"].ToString()));
                apiParam.Add("acceptDate", new JValue(grid[row, "insDate"].ToString()));

                var apiUrl2 = System.Configuration.ConfigurationManager.AppSettings["connectionWebAPIURL"] + "Task/";

                var webApi = new WebAPI();
                var result = webApi.PostRequest(apiUrl2 + "GetInspectionJudgment?sid=S036&fid=F228", apiParam, LoginInfo.Instance.Token);
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
                            return;
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }

                    ChangeTopMessage("E0008", "初品検査等の情報取得時に");
                    return;
                }
                if ((bool)(result["isok"]) == false)
                {
                    ChangeTopMessage("E0008", "初品検査等の情報取得時に");
                    return;
                }

                if (result["data"] != null)
                {
                    inspectionMsgLabel.Text = result["data"]["inspectionMsg"]?.ToString() ?? "";
                }

                // コンボボックスセット、コンボボックスの内容をセットするメソッドは１コントロールずつに分ける
                SetNxtCusCodeC1ComboBox();

                ActiveControl = numC1NumericEdit;
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

                default:
                    break;
            }
        }

        /// <summary>
        /// c1TrueDBGrid1　ボタンクリック処理
        /// </summary>
        private void c1TrueDBGrid1_ButtonClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            try
            {
                isRunValidating = false;
                var grid = (C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender;
                int row = grid.Row;
                string colName = e.Column.Name;

                if (colName != "削除")
                {
                    return;
                }

                var dialog = MessageBox.Show("現品票を削除してもよいですか？",
                                                 "現品票削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                 MessageBoxDefaultButton.Button2);
                if (dialog != DialogResult.Yes)
                {
                    return;
                }

                // 指定行を削除
                M_TagCodeDT.Rows.RemoveAt(row);

                //string bizCode = grid[row, "bizCode"].ToString().TrimEnd();
                //string tagCode = grid[row, "現品票番号"].ToString().TrimEnd();

                //// パラメータ
                //apiParam.RemoveAll();
                //apiParam.Add("bizCode", new JValue(bizCode));
                //apiParam.Add("tagCode", new JValue(tagCode));

                //// 既存のM_TagCode削除
                //var result = ApiCommonUpdate(apiUrl + "DeleteTagCode?sid=S036&fid=F228", apiParam);
                //if (result.IsOk == false)
                //{
                //    ChangeTopMessage("E0008", "現品票削除時に");
                //    return;
                //}

                // 既存のM_TagCodeの合計数取得
                var result2 = GetTagCodeTotal(doCodeC1TextBox.Text);
                if (result2.isOk == false)
                {
                    ChangeTopMessage("E0008", "合計値検索時に");
                    return;
                }
                if (result2.count <= 0)
                {
                    possibleNumC1NumericEdit.Value = delivNumC1NumericEdit.Value;
                }
                else
                {
                    possibleNumC1NumericEdit.Value = result2.possibleNum;
                }

                // 既存のM_TagCode描画
                c1TrueDBGrid1.SetDataBinding(null, "", true);
                c1TrueDBGrid1.SetDataBinding(M_TagCodeDT, "", true);
                //printDt = M_TagCodeDT;
                //C1TrueDBGrid1_Drawing();

                ActiveControl = numC1NumericEdit;
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
        private void c1TrueDBGrid1_UnboundColumnFetch(object sender, C1.Win.C1TrueDBGrid.UnboundColumnFetchEventArgs e)
        {
            switch (e.Column.Caption)
            {
                case "削除":
                    e.Value = "削除";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 追加ボタン押下時イベント
        /// </summary>
        private void insertButton_Click(object sender, EventArgs e)
        {
            try
            {
                isRunValidating = false;

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

                // エラーチェック  数量
                if (this.ErrorCheckNum() == false || numC1NumericEdit.Text == "")
                {
                    ChangeTopMessage("W0007", numC1NumericEdit.Label.Text);
                    this.numC1NumericEdit.Focus();
                    return;
                }
                var num = numC1NumericEdit.Value;
                if (int.Parse(num.ToString()) <= 0)
                {
                    ChangeTopMessage("W0016", numC1NumericEdit.Label.Text + "に0またはマイナス");
                    this.numC1NumericEdit.Focus();
                    return;
                }

                // 箱連番取得
                var result = GetTagCodeMaxBoxSerial(doCodeC1TextBox.Text);
                if(result.isOk == false)
                {
                    ChangeTopMessage("E0008", "箱番号検索時に");
                    return;
                }
                maxBoxSerial = result.maxBoxSerial;

                int maxBoxSerial_Update = maxBoxSerial + 1;

                // M_TagCode更新用DT作成
                DataRow row;
                row = M_TagCodeDT.NewRow();
                row["bizCode"] = "2900";
                row["tagCode"] = wTagCode;
                row["skdCode"] = doCodeC1TextBox.Text;
                row["partsCode"] = partsCodeC1TextBox.Text;
                row["delivNum"] = numC1NumericEdit.Text;
                row["packingBoxSerial"] = maxBoxSerial + 1;
                row["packingBoxNum"] = "0";
                row["poCode"] = poCodeC1TextBox.Text;
                row["sakuban"] = sakubanC1TextBox.Text;
                row["cusCode"] = cusCodeC1TextBox.Text;
                row["nxtCusCode"] = nxtCusCodeC1ComboBox.Text;
                row["groupCode"] = groupCodeC1TextBox.Text;
                row["doCode"] = doCodeC1TextBox.Text;
                row["partsName"] = partsNameC1TextBox.Text;
                row["groupName"] = groupNameC1TextBox.Text;
                row["cusName"] = cusNameC1TextBox.Text;
                row["nxtCusName"] = nxtCusNameC1TextBox.Text;
                M_TagCodeDT.Rows.Add(row);

                // パラメータ
                //apiParam.RemoveAll();
                //apiParam.Add("bizCode", new JValue("2900"));
                //apiParam.Add("tagCode", new JValue(doCodeC1TextBox.Text.Replace("-", "") + maxBoxSerial_Update.ToString() + "0" +
                //                                   maxBoxSerial_Update.ToString("00")));
                //apiParam.Add("skdCode", new JValue(doCodeC1TextBox.Text));
                //apiParam.Add("partsCode", new JValue(partsCodeC1TextBox.Text));
                //apiParam.Add("skdNum", new JValue(numC1NumericEdit.Text));
                //apiParam.Add("packingBoxSerial", new JValue(maxBoxSerial + 1));
                //apiParam.Add("packingBoxNum", new JValue(0));
                //apiParam.Add("poCode", new JValue(poCodeC1TextBox.Text));
                //apiParam.Add("cusCode", new JValue(cusCodeC1TextBox.Text));
                //apiParam.Add("nxtCusCode", new JValue(nxtCusCodeC1ComboBox.Text));
                //apiParam.Add("groupCode", new JValue(groupCodeC1TextBox.Text));
                //apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));
                //apiParam.Add("sakuban", new JValue(sakubanC1TextBox.Text));

                //// M_TagCode追加処理
                //var result2 = ApiCommonUpdate(apiUrl + "CreateTagCode?sid=S036&fid=F228", apiParam);
                //if (result2.IsOk == false)
                //{
                //    ChangeTopMessage("E0008", "現品票追加時に");
                //    return;
                //}

                // 既存のM_TagCodeの合計数取得
                var result3 = GetTagCodeTotal(doCodeC1TextBox.Text);
                if (result3.isOk == false)
                {
                    ChangeTopMessage("E0008", "合計値検索時に");
                    return;
                }
                if (result3.count <= 0)
                {
                    possibleNumC1NumericEdit.Value = delivNumC1NumericEdit.Value;
                }
                else
                {
                    possibleNumC1NumericEdit.Value = result3.possibleNum;
                }

                // 既存のM_TagCode描画
                c1TrueDBGrid1.SetDataBinding(null, "", true);
                c1TrueDBGrid1.SetDataBinding(M_TagCodeDT, "", true);
                //printDt = M_TagCodeDT;
                //C1TrueDBGrid1_Drawing();

                numC1NumericEdit.Value = 0;
                ActiveControl = numC1NumericEdit;
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

        //(未使用・参考)
        ///// <summary>
        ///// 次工程を更新した後処理
        ///// </summary>
        //private void c1TrueDBGrid1_AfterColUpdate(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        //{
        //    try
        //    {
        //        int rowIndex = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender).Row;

        //        // 更新行の情報を取得
        //        var bizCode = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender)[rowIndex, "bizCode"].ToString().Trim();
        //        var tagCode = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender)[rowIndex, "現品票番号"].ToString().Trim();
        //        var nxtCusCode = ((C1.Win.C1TrueDBGrid.C1TrueDBGrid)sender)[rowIndex, "選択"].ToString().Trim();

        //        // パラメータ
        //        apiParam.RemoveAll();
        //        apiParam.Add("bizCode", new JValue(bizCode));
        //        apiParam.Add("tagCode", new JValue(tagCode));
        //        apiParam.Add("nxtCusCode", new JValue(nxtCusCode));

        //        // データベース更新
        //        var result = ApiCommonUpdate(apiUrl + "UpdateTagCode?sid=S036&fid=F228", apiParam);
        //        if (result.IsOk == false)
        //        {
        //            ChangeTopMessage("E0008", "現品票更新時に");
        //            return;
        //        }

        //        // 既存のM_TagCode描画
        //        C1TrueDBGrid1_Drawing();

        //        c1TrueDBGrid1[rowIndex, "次工程"] = nxtCusCode;
        //        c1TrueDBGrid1[rowIndex, "選択"] = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}");
        //    }
        //}

        #endregion  ＜イベント処理 END＞

        #region ＜実行前チェック＞ 

        /// <summary>
        /// 実行（F10）必須チェック
        /// </summary>
        /// <returns>True：必須項目の入力ＯＫ False：必須項目の入力漏れあり</returns>
        private bool RequirF10()
        {
            if (doCodeC1TextBox.Text == "")
            {
                ChangeTopMessage("W0007", "伝票番号");
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
            if (M_TagCodeDT == null || M_TagCodeDT.Rows.Count <= 0) 
            {
                ActiveControl = numC1NumericEdit;
                ChangeTopMessage("W0017", "現品票発行する");
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

                double totalNum = 0;
                double delivNum = 0;

                // 発行数チェック
                var result0 = GetTagCodeTotal(doCodeC1TextBox.Text);
                if (result0.isOk == false)
                {
                    ChangeTopMessage("E0008", "合計発行数検索時に");
                    return;
                }
                if (result0.count <= 0)
                {
                    ChangeTopMessage("E0008", "合計発行数検索時に");
                    return;
                }
                else
                {
                    totalNum = result0.totalNum;
                }
                // M_DelivSlipの数量取得
                // 現品票未発行データ抽出
                var result1 = C1TrueDBGrid_Drawing(doCodeC1TextBox.Text);
                if (result1.isOk == false)
                {
                    ChangeTopMessage("E0008", "検索時に");
                    return;
                }
                if (result1.dt == null || result1.dt.Rows.Count <= 0)
                {
                    ChangeTopMessage("I0005");
                    return;
                }
                delivNum = double.Parse(result1.dt.Rows[0]["delivNum"].ToString());

                if (totalNum > delivNum)
                {
                    ChangeTopMessage("W0014", "現品票発行数", "発行可能数");
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // プレビューを表示するかどうかを判定
                bool isPreview = true;
                string printerName = System.Configuration.ConfigurationManager.AppSettings["GoodsTagA6"];
                foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (p == printerName) { isPreview = false; }
                }

                // M_TagCode TagCode作成
                // 現品票ファイル抽出
                var af = new F228_GoodsTagPrintAF();
                var result2 = af.CreateMTagCode(M_TagCodeDT, inspectionMsgLabel.Text);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result2.Msg);
                    return;
                }

                // マウスカーソル待機状態を解除
                Cursor = Cursors.Default;

                // 現品票印刷
                //using (var report = new C1.Win.FlexReport.C1FlexReport())
                //{
                //    report.Load(@"\\sserv04\WORK\CS\AllMenuShortcut\CommonGoodsTag.flxr", "CommonGoodsTag");
                //    //report.Load(EXE_DIRECTORY + @"\Reports\R031_GoodsTag.flxr", "R031_GoodsTag");

                //    // データソース設定
                //    var ds = new C1.Win.FlexReport.DataSource
                //    {
                //        Name = " ",
                //        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                //        Recordset = result2.Table
                //    };
                //    report.DataSources.Add(ds);
                //    report.DataSourceName = ds.Name;

                //    // サブレポート  設定
                //    var dsSub1 = new C1.Win.FlexReport.DataSource
                //    {
                //        Name = " ",
                //        ConnectionString = System.Configuration.ConfigurationManager.AppSettings["C1ReportConnectionString"],
                //        RecordSource = SansoBase.Common.SelectDBAF.CreateSubReportSQL(7)
                //    };
                //    ((C1.Win.FlexReport.SubreportField)report.Fields["sub1"]).Subreport.DataSources.Add(dsSub1);
                //    ((C1.Win.FlexReport.SubreportField)report.Fields["sub1"]).Subreport.DataSourceName = dsSub1.Name;


                //    if (isPreview)
                //    {
                //        // プレビュー印刷
                //        report.Render();
                //        var print = PrintReport(report);
                //        if (print.IsOk == false)
                //        {
                //            ChangeTopMessage("E0008", "現品票印刷処理時に");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        // 即印刷
                //        report.Render();
                //        var p = new System.Drawing.Printing.PrinterSettings();
                //        p.PrinterName = System.Configuration.ConfigurationManager.AppSettings["GoodsTag"];
                //        var print = PrintReport(report, false, p);
                //        if (print.IsOk == false)
                //        {
                //            ChangeTopMessage("E0008", "現品票印刷処理時に");
                //            return;
                //        }
                //    }
                //}

                // 現品票（A6版）印刷処理
                var result4 = PrintGoodsTagA6(result2.Table, isPreview);
                if (result4.isOk == false)
                {
                    ChangeTopMessage(1, "ERR", result4.msg);
                    return;
                }

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // M_DelivSlip 全数発行済みデータ削除
                // M_TagCode ステータス更新
                var result3 = af.UpdateMTagCode(result2.Table);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result3.Msg);
                    return;
                }

                // 品管の受入検査用データ作成処理を実施
                apiParam.RemoveAll();
                string json1 = JsonConvert.SerializeObject(M_TagCodeDT, Formatting.Indented);
                apiParam.Add("mTagCodeDT", new JValue(json1));
                var result5 = CallSansoWebAPI("POST", apiUrl + "UpdateQuality", apiParam);
                if (result5.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result5.Msg);
                    return;
                }

                M_TagCodeDT.Clear();

                DisplayClear();
                ChangeTopMessage("I0009", "現品票発行が");
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
        /// 印刷処理
        /// </summary>
        private void PrintProc()
        {
            try
            {
                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // レポートデータ
                if (excelDt == null)
                {
                    ChangeTopMessage("I0007");
                    return;
                }

                var p = PrintReport("R031_GoodsTag", excelDt);
                if (p == false)
                {
                    return;
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
            }
        }

        #endregion  ＜メイン処理 END＞

        #region ＜その他処理＞ 

        /// <summary>
        /// c1TrueDBGridを描画する
        /// </summary>
        private (bool isOk, DataTable dt) C1TrueDBGrid_Drawing(string doCode = "")
        {
            // パラメータ
            apiParam.RemoveAll();

            apiParam.Add("dbName", new JValue(orderGroupC1ComboBox.SGetText(1)));
            //apiParam.Add("dbName", new JValue("製造調達"));
            apiParam.Add("doCode", new JValue(doCode));

            // 現品票未発行データ抽出
            var result = ApiCommonGet(apiUrl + "GetTagCodeCreateData?sid=S036&fid=F228", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                //return (false, null);
            }
            var dt = result.Table;
            //if (dt == null || dt.Rows.Count <= 0)
            //{
            //    ChangeTopMessage("I0005");
            //    return (false, null);
            //}
            return (true, dt);
        }

        /// <summary>
        /// 既存のM_TagCodeの合計数と件数を取得
        /// </summary>
        /// <param name="doCode">伝票番号</param>
        /// <returns></returns>
        private (bool isOk, double possibleNum, int count, double totalNum) GetTagCodeTotal(string doCode)
        {
            // パラメータ
            apiParam.RemoveAll();
            apiParam.Add("doCode", new JValue(doCode));

            double tableTotalNum = 0;
            int tableCount = 0;

            // 既存のM_TagCodeの合計検索
            var result = ApiCommonGet(apiUrl + "GetTagCodeTotalData?sid=S036&fid=F228", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "検索時に");
                return (false, 0, 0, 0);
            }

            if (result.Table != null && result.Table.Rows.Count > 0)
            {
                tableTotalNum = double.Parse(result.Table.Rows[0]["totalNum"].ToString());
                tableCount = int.Parse(result.Table.Rows[0]["count"].ToString());
            }

            // M_TagCodeDTの合計検索
            double dtTotalNum = M_TagCodeDT.AsEnumerable()
                                        .Where(v => v.Field<string>("doCode") == doCode)
                                        .Select(v => double.Parse(v.Field<string>("delivNum")))
                                        .Sum();
            int dtTotalCount = M_TagCodeDT.AsEnumerable()
                                        .Where(v => v.Field<string>("doCode") == doCode)
                                        .Count();

            // 合計計算
            double possibleNum = double.Parse(delivNumC1NumericEdit.Text) -
                                 tableTotalNum - dtTotalNum;

            int count = tableCount + dtTotalCount;

            double totalNum = tableTotalNum + dtTotalNum;

            return (true, possibleNum, count, totalNum);

        }

        /// <summary>
        /// c1TrueDBGrid1を描画する
        /// </summary>
        //private (bool isOk, DataTable dt) C1TrueDBGrid1_Drawing()
        //{
        //    // パラメータ
        //    apiParam.RemoveAll();
        //    apiParam.Add("dbName", new JValue("製造調達"));
        //    apiParam.Add("skdCode", new JValue(doCodeC1TextBox.Text));

        //    // 既存のM_TagCode検索
        //    var result2 = ApiCommonGet(apiUrl + "GetTagCode?sid=S036&fid=F228", apiParam);
        //    if (result2.IsOk == false)
        //    {
        //        ChangeTopMessage("E0008", "検索時に");
        //        return (false, null);
        //    }
        //    var dt = result2.Table;

        //    if (dt == null || dt.Rows.Count <= 0)
        //    {
        //        ChangeTopMessage("I0005");
        //        c1TrueDBGrid1.SetDataBinding(null, "", true);
        //        return (true, null);
        //    }
        //    c1TrueDBGrid1.SetDataBinding(dt, "", true);


        //    c1TrueDBGrid1.SetDataBinding(M_TagCodeDT, "", true);

        //    printDt = M_TagCodeDT;

        //    return (true, M_TagCodeDT);
        //}

        /// <summary>
        /// 既存のM_TagCodeの合計数と件数を取得
        /// </summary>
        /// <param name="doCode">伝票番号</param>
        /// <returns></returns>
        private (bool isOk, int maxBoxSerial) GetTagCodeMaxBoxSerial(string doCode)
        {
            apiParam.RemoveAll();
            apiParam.Add("doCode", new JValue(doCodeC1TextBox.Text));

            var result = ApiCommonGet(apiUrl + "GetTagCodeMaxData?sid=S036&fid=F228", apiParam);
            if (result.IsOk == false)
            {
                ChangeTopMessage("E0008", "箱番号検索時に");
                return (false, 0);
            }
            if (result.Table == null || result.Table.Rows.Count <= 0)
            {
                maxBoxSerial = 1;
            }
            else
            {
                maxBoxSerial = int.Parse(result.Table.Rows[0]["maxBoxSerial"].ToString());
            }

            return (true, maxBoxSerial);
        }

        /// <summary>
        /// エラーチェック  発行数
        /// </summary>
        /// <returns>True：エラーが無し False：エラーがある</returns>
        private bool ErrorCheckNum()
        {
            // 未入力時処理
            var s = numC1NumericEdit;
            if (string.IsNullOrEmpty(s.Text))
            {
                return true;
            }

            // 使用禁止文字
            var isOk = Check.IsNumeric(s.Text).Result;
            if (isOk == false)
            {
                ChangeTopMessage("W0018");
                return false;
            }

            var num = numC1NumericEdit.Value;
            if (int.Parse(num.ToString()) <= 0)
            {
                ChangeTopMessage("W0016", numC1NumericEdit.Label.Text + "に0またはマイナス");
                this.numC1NumericEdit.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(numC1NumericEdit.Text))
            {
                return true;
            }

            double totalNum = 0;
            var result = GetTagCodeTotal(doCodeC1TextBox.Text);
            if (result.isOk == false)
            {
                ChangeTopMessage("E0008", "合計値検索時に");
                return false;
            }
            if (result.count <= 0)
            {
                totalNum = double.Parse(delivNumC1NumericEdit.Text);
            }
            else
            {
                totalNum = result.possibleNum;
            }

            if(totalNum < double.Parse(numC1NumericEdit.Text))
            {
                ChangeTopMessage("W0014", "発行数", "発行可能数");
                return false;
            }

            return true;
        }

        /// <summary>
        /// WEBAPI側共通更新処理
        /// </summary>
        /// <param name="apiParam">パラメータ</param>
        /// <param name="apiUrl">URL</param>
        /// <returns>(実行成否[falseの場合は例外発生], 影響したデータの行数[例外発生時は0])</returns>
        private (bool IsOk, int Count) ApiCommonUpdate(string apiUrl, JObject apiParam = null)
        {
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
                        return (false, 0);
                    }
                    else
                    {
                        Application.Exit();
                    }
                }

                return (false, 0);
            }

            if (result["isok"] != null)
            {
                if ((bool)result["isok"] == false)
                {
                    return (false, 0);
                }
            }
            else
            {
                if ((bool)result["isOk"] == false)
                {
                    return (false, 0);
                }
            }

            return (
                true,
                (int)(result["count"])
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

            if (result["isok"] != null)
            {
                if((bool)result["isok"] == false)
                {
                    return (false, 0, null);
                }
            }
            else
            {
                if ((bool)result["isOk"] == false)
                {
                    return (false, 0, null);
                }
            }

            var dt = result["data"].ToString();

            if (dt == "" || (int)(result["count"]) <= 0)
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

        /// <summary>
        /// レポート印刷処理
        /// </summary>
        /// <param name="reportName">レポート名</param>
        /// <param name="table">レポート印刷用データテーブル</param>
        /// <returns>true:印刷成功　false：印刷失敗</returns>
        private bool PrintReport(string reportName, DataTable printDt)
        {
            if (printDt == null || printDt.Rows.Count <= 0)
            {
                return true;
            }

            using (var report = new C1.Win.FlexReport.C1FlexReport())
            {
                report.Load(EXE_DIRECTORY + @"\Reports\" + reportName + ".flxr", reportName);

                // データソース設定
                var ds = new C1.Win.FlexReport.DataSource
                {
                    Name = " ",
                    ConnectionString = reportConnectionString,
                    Recordset = printDt
                };
                report.DataSources.Add(ds);
                report.DataSourceName = ds.Name;

                // プレビュー印刷
                report.Render();
                var print = PrintReport(report);
                if (print.IsOk == false)
                {
                    ChangeTopMessage("E0008", "印刷処理で");
                    return false;
                }
            }
            return true;
        }

        #endregion  ＜その他処理 END＞

        private void orderGroupC1ComboBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                // コンボボックスの共通Validating処理（戻り値あり）
                if (IsOkComboBoxValidating(sender, e) == false)
                {
                    return;
                }

                // 編集中データチェック
                if ((M_TagCodeDT != null) && (M_TagCodeDT.Rows.Count > 0))
                {
                    var dialog = MessageBox.Show("まだ登録完了していない現品票データがあります。発注元を変更してもよろしいですか？",
                                                  "現品票発行予定削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                  MessageBoxDefaultButton.Button2);
                    if (dialog != DialogResult.Yes)
                    {
                        ActiveControl = orderGroupC1ComboBox;
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

        private void orderGroupC1ComboBox_Validated(object sender, EventArgs e)
        {
            try 
            {
                if (isRunValidating == false)
                {
                    return;
                }

                orderGroupIndex = orderGroupC1ComboBox.SelectedIndex;
                DisplayClear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}"); 
            }
        }

        private void orderGroupC1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveControl = c1TrueDBGrid;
        }

        private void directButton_Click(object sender, EventArgs e)
        {
            try 
            {
                // 編集中データチェック
                if ((M_TagCodeDT != null) && (M_TagCodeDT.Rows.Count > 0))
                {
                    var dialog1 = MessageBox.Show("まだ登録完了していない現品票データがあります。現品票発行予定を削除し、" +
                                                  "直送に変更してよろしいですか？",
                                                  "現品票発行予定削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                                  MessageBoxDefaultButton.Button2);
                    if (dialog1 != DialogResult.Yes)
                    {
                        return;
                    }
                }

                var dialog2 = MessageBox.Show("伝票番号「" + doCodeC1TextBox.Text + "」を直送処理します。よろしいですか？",
                                              "直送処理の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                              MessageBoxDefaultButton.Button2);
                if (dialog2 != DialogResult.Yes)
                {
                    return;
                }

                isRunValidating = false;
                ClearTopMessage();

                // マウスカーソル待機状態
                Cursor = Cursors.WaitCursor;

                // M_TagCode更新用DT作成
                M_TagCodeDT.Clear();
                DataRow row;
                row = M_TagCodeDT.NewRow();
                row["bizCode"] = "2900";
                row["tagCode"] = "2900" + DateTime.Now.ToString("yyyyMMddHHmmss");
                row["skdCode"] = doCodeC1TextBox.Text;
                row["partsCode"] = partsCodeC1TextBox.Text;
                row["delivNum"] = possibleNumC1NumericEdit.Text;
                row["packingBoxSerial"] = 1;
                row["packingBoxNum"] = 1;
                row["poCode"] = poCodeC1TextBox.Text;
                row["sakuban"] = sakubanC1TextBox.Text;
                row["cusCode"] = cusCodeC1TextBox.Text;
                row["nxtCusCode"] = "";
                row["groupCode"] = groupCodeC1TextBox.Text;
                row["doCode"] = doCodeC1TextBox.Text;
                row["partsName"] = partsNameC1TextBox.Text;
                row["groupName"] = groupNameC1TextBox.Text;
                row["cusName"] = cusNameC1TextBox.Text;
                row["nxtCusName"] = nxtCusNameC1TextBox.Text;
                M_TagCodeDT.Rows.Add(row);

                // M_TagCode TagCode作成
                var af = new F228_GoodsTagPrintAF();
                var result2 = af.CreateMTagCode(M_TagCodeDT, inspectionMsgLabel.Text);
                if (result2.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result2.Msg);
                    return;
                }

                // M_DelivSlip 全数発行済みデータ削除
                // M_TagCode ステータス更新
                var result3 = af.UpdateMTagCode(result2.Table, true);
                if (result3.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result3.Msg);
                    return;
                }

                // 品管の受入検査用データ作成処理を実施
                apiParam.RemoveAll();
                string json1 = JsonConvert.SerializeObject(M_TagCodeDT, Formatting.Indented);
                apiParam.Add("mTagCodeDT", new JValue(json1));
                var result4 = CallSansoWebAPI("POST", apiUrl + "UpdateQuality", apiParam);
                if (result4.IsOk == false)
                {
                    ChangeTopMessage(1, "ERR", result4.Msg);
                    return;
                }

                M_TagCodeDT.Clear();

                DisplayClear();
                ChangeTopMessage("I0009", "直送処理が");
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
    }
}
