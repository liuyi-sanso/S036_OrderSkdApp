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
using C1.Win.C1Tile;
using System.IO;
using System.Configuration;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// メニュー
    /// </summary>
    public partial class MainMenu : Form
    {

        #region ＜フィールド＞ 

        /// <summary>
        /// 接続データベースがS12、S33を判断する
        /// S12：True　　S33：false
        /// </summary>
        private bool masterDBFlg = true;

        /// <summary>
        /// エクセルファイルなどのデフォルト保存先
        /// </summary>
        public static string FileOutputPath = 
            System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\部品調達システム";

        /// <summary>
        /// お知らせ画面 重複起動防止
        /// </summary>
        public Information information = null;

        /// <summary>
        /// ログイン者の利用可能メニューを保管
        /// </summary>
        private DataTable availableList;

        /// <summary>
        /// メニューグループリスト
        /// </summary>
        List<(string GroupID, string GroupName)> menuGroupList = new List<(string GroupID, string GroupName)>();

        /// <summary>
        /// メニューリスト
        /// </summary>
        List<MenuParam> menuList = new List<MenuParam>();

        /// <summary>
        /// 画面（フォーム）リスト
        /// </summary>
        List<(string FormName, EventHandler EventMethod)> formList = new List<(string FormName, EventHandler EventMethod)>();

        /// <summary>
        /// 在場時間測定画面を表示しているかどうかを保管
        /// 表示中：True 非表示：False
        /// </summary>
        public static bool showStayTimeMeasurement = false;

        #endregion  ＜フィールド END＞

        #region<初期化>

        public bool RelogFlg { get; set; } = false;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            // 部品調達システムはログイン画面無し
            //bool loginOk = false;
            bool loginOk = true;

            // SSERRV33接続判断
            string DBchekc = System.Configuration.ConfigurationManager.ConnectionStrings["SystemMainDBConnectionStrings"].ConnectionString;
            if ((DBchekc.Contains("ERV33")) || (DBchekc.Contains("erv33")))
            {
                masterDBFlg = false;
                // SSEVER33接続時は背景色を変更
                this.BackColor = ColorTranslator.FromHtml(System.Configuration.ConfigurationManager.AppSettings["DevelopBackColor"]);
                // メッセージ表示
                developLabel.Visible = true;
            }

            try
            {
                // ログイン画面起動 + ユーザ情報設定
                var loginForm = new SansoBase.LoginMenu.LoginMenu();
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.ShowDialog();
                loginOk = loginForm.IsLoginOk;

                // 部品調達システムはログイン画面無し
                //LoginInfo.Instance.SetInfo("",
                //    Environment.UserName,
                //    "",
                //    "",
                //    "",
                //    Environment.MachineName,
                //    "",
                //    DateTime.Today.ToString());

                // メッセージxml取得
                var message = new SansoBase.Message("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 利用可能メニュー抽出
            var af = new M_SystemUsingMenuAF();
            var result = af.GetMSystemUsingMenu(LoginInfo.Instance.UserId, "SansoProductionManagementApp");
            if (result.IsOk == false)
            {
                MessageBox.Show("利用可能メニュー抽出時にエラーが発生しました。画面を終了します。");
                this.Close();
            }
            if (result.Table.Rows.Count <= 0)
            {
                // まだ未実装
                //MessageBox.Show("利用可能なメニューが登録されていません。画面を終了します。");
                //this.Close();
            }
            availableList = result.Table;

            // ログインＯＫならメニューを表示
            if (loginOk == true)
            {
                RelogFlg = true;
                this.ShowMainMenu();
                L_Message.Text = string.Empty;
            }
            else
            {
                RelogFlg = false;
                // アプリケーション終了
                this.Close();
            }

            try
            {
                // 画面を開いたログを残す
                var data = new HistoryData(
                    Path.GetFileName(Environment.GetCommandLineArgs()[0]),
                    this.GetType().Name,
                    LoginInfo.Instance.MachineCode,
                    LoginInfo.Instance.UserNo,
                    "OPEN",
                    "",         // SQLはないので空
                    "",         // SQLパラメータはないので空
                    LoginInfo.Instance.IPAdress
                );
                History.WriteFormAccessHistory(data);


                // お知らせ画面を開く
                // 現在より３日内に掲載されたお知らせがあれば表示する
                var v = SansoBase.InformationAF.GetInformationMST(Path.GetFileName(Environment.GetCommandLineArgs()[0]), "");
                if ((v.Table.Rows.Count > 0) && (DateTime.Parse(v.Table.Rows[0]["ReleaseDate"].ToString()) > DateTime.Now.AddDays(-3)))
                {
                    //var form = new SansoBase.Information(Path.GetFileName(Environment.GetCommandLineArgs()[0]), "");
                    //form.StartPosition = FormStartPosition.CenterScreen;
                    //form.Show();
                    ShowInformation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion<初期化>

        #region<メニュー>

        /// <summary>
        /// トップメニュー
        /// </summary>
        private void ShowMainMenu()
        {
            tileControlMainMenu.BeginUpdate();

            // タイルグループ全てリセット
            tileControlMainMenu.Groups.Clear(true);

            try
            {
                // グループ作成
                label_main_title.Text = "部品調達システム";

                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 130;

                // ショートカットグループ作成
                var TopGroup = new Group();
                tileControlMainMenu.Groups.Add(TopGroup);

                // グループ追加（グループID、グループ名）
                menuGroupList.Clear();
                AddMenuGroupList("001", "");
                AddMenuGroupList("002", "");
                AddMenuGroupList("009", "その他");
                AddMenuGroupList("099", "開発支援");

                // メニュー追加（グループID、メニュー名、メニューID、メニューボタン背景色、イベントのメソッド）
                menuList.Clear();
                if(masterDBFlg == false){ AddMenuList("001", $"問合せ{ Environment.NewLine }処理", "M001", Color.DodgerBlue, informationTile_Click); };
                AddMenuList("001", $"入出庫処理{ Environment.NewLine }　", "M201", Color.DodgerBlue, IOProcessTile_Click);
                if(masterDBFlg == false){ AddMenuList("002", $"マスタメンテナンス", "M601", Color.Tomato, MstMaintTile_Click); };
                if (masterDBFlg == false) { AddMenuList("002", $"月末業務{ Environment.NewLine }　", "M401", Color.Tomato, EOMProcessTile_Click); };
                if (masterDBFlg == false) { AddMenuList("002", $"発注処理{ Environment.NewLine }　", "M301", Color.Orange, OrderTile_Click); };
                AddMenuList("099", $"C#開発支援{ Environment.NewLine }　", "M901", Color.Purple, DevelopSupportTile_Click);

                AddMenuList("009", $"お知らせ{ Environment.NewLine }　", "M902", Color.Tomato, Information_Click);
                if (masterDBFlg == false)
                {
                    AddMenuList("009", $"現品票{ Environment.NewLine }メンテ", "M902", Color.Tomato, F910_TagCodeMaintTile_Click);
                }

                // グループ作成
                foreach (var v in menuGroupList.OrderBy(x => x.GroupID))
                {
                    var gp = new Group();
                    gp.Text = v.GroupName;
                    tileControlMainMenu.Groups.Add(gp);

                    // メニュー作成
                    foreach (var vv in menuList.Where(xx => xx.GroupID == v.GroupID))
                    {
                        // M_SystemUsingMenuに登録されていないメニューは表示しない（まだ未実装）
                        //var ck = availableList.AsEnumerable().Where(xxx => xxx.Field<string>("MenuID") == vv.MenuID);
                        //if (ck.Count() <= 0)
                        //{
                        //    continue;
                        //}
                        // 開発支援は本番環境では表示しない
                        if (masterDBFlg && vv.MenuID == "M901")
                        {
                            continue;
                        }
                        var mu = new Tile();
                        mu.Text = vv.MenuName;
                        mu.Click += vv.EventMethod;
                        mu.BackColor = vv.BackColor;
                        gp.Tiles.Add(mu);
                    }
                }

                // 戻るボタンを非表示
                B_Back.Visible = false;
                //B_Back.Click -= BackMainTile_Click;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        /// <summary>
        /// 現品票メンテナンス画面へリンク
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F910_TagCodeMaintTile_Click(object sender, EventArgs e)
        {
            var form = new F910_TagCodeMaint("F910_TagCodeMaint");
            form.Show();
        }


        /// <summary>
        /// 技術部品構成表システムへリンク
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TechnologyBOMTile_Click(object sender, EventArgs e)
        {
            var path = System.Configuration.ConfigurationManager.AppSettings["S018_SansoBillsOfMaterialsAppPath"];

            System.Diagnostics.Process p =
                System.Diagnostics.Process.Start(path);

        }

        /// <summary>
        /// お知らせ画面を開くイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_Click(object sender, EventArgs e)
        {
            ShowInformation();
        }

        /// <summary>
        /// 共通ショートカットタイルを作成
        /// </summary>
        private void CreateShortcutTile(object sender)
        {
            try
            {
                var ShortcutGroup = new Group();
                ShortcutGroup.Text = "ショートカット";
                tileControlMainMenu.Groups.Add(ShortcutGroup);

                var backTile2 = new Tile();
                backTile2.Text = $"戻る{ Environment.NewLine }　";
                backTile2.BackColor = Color.ForestGreen;
                backTile2.Click += BackMainTile_Click;
                var ProdManageInfoTile = new Tile();
                //ProdManageInfoTile.Text = $"問合せ{ Environment.NewLine }処理";
                //ProdManageInfoTile.Click += ProdManageInfoTile_Click;
                //ProdManageInfoTile.BackColor = Color.DodgerBlue;
                //ProdManageInfoTile.ToolTipText = "問合せ処理を行います";

                ShortcutGroup.Tiles.Add(backTile2);
                if (((C1.Win.C1Tile.Tile)sender).Text != ProdManageInfoTile.Text) { ShortcutGroup.Tiles.Add(ProdManageInfoTile); };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        /// <summary>
        /// 問合せ
        /// </summary>
        private void informationTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/問合せ処理";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var Group1 = new Group();
                Group1.Text = "";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(Group1);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;


                // 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                AddFormList($"教育履歴{ Environment.NewLine }問合せ", F001_TrainingHisInfo_Click);
                AddFormList($"納入予定{ Environment.NewLine }一覧", F002_DelivScheduleList_Click);
                AddFormList($"未検収{ Environment.NewLine }問合せ", F003_NoDelivInfo_Click);
                AddFormList($"未受入{ Environment.NewLine }問合せ", F004_NoAcceptInfo_Click);
                AddFormList($"不適合品{ Environment.NewLine }問合せ", F005_FailedInfo_Click);
                AddFormList($"不適合品{ Environment.NewLine }入出庫問合せ", F006_FailedIOInfo_Click);
                AddFormList($"不適合品{ Environment.NewLine }レポート印刷", F007_FailedReportPrint_Click);
                AddFormList($"発送一覧{ Environment.NewLine }問合せ", F008_shippingList_Click);
                CreateFormTile(formList, Group1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜問合せイベント＞ 
        /// <summary>
        /// 未受入問合せ
        /// </summary>
        private void F004_NoAcceptInfo_Click(object sender, EventArgs e)
        {
            var form = new F004_NoAcceptInfo("F004_NoAcceptInfo");
            form.Show();
        }

        /// <summary>
        /// 未検収問合せ
        /// </summary>
        private void F003_NoDelivInfo_Click(object sender, EventArgs e)
        {
            var form = new F003_NoDelivInfo("F003_NoDelivInfo");
            form.Show();
        }

        /// <summary>
        /// 納入予定一覧
        /// </summary>
        private void F002_DelivScheduleList_Click(object sender, EventArgs e)
        {
            var form = new F002_DelivScheduleList("F002_DelivScheduleList");
            form.Show();
        }

        /// <summary>
        /// 不適合品問合せ
        /// </summary>
        private void F005_FailedInfo_Click(object sender, EventArgs e)
        {
            var form = new F005_FailedInfo("F005_FailedInfo");
            form.Show();
        }

        /// <summary>
        /// 不適合品入出庫問合せ
        /// </summary>
        private void F006_FailedIOInfo_Click(object sender, EventArgs e)
        {
            var form = new F006_FailedIOInfo("F006_FailedIOInfo");
            form.Show();
        }

        /// <summary>
        /// 不適合品レポート印刷
        /// </summary>
        private void F007_FailedReportPrint_Click(object sender, EventArgs e)
        {
            var form = new F007_FailedReportPrint("F007_FailedReportPrint");
            form.Show();
        }

        /// <summary>
        /// 不適合品レポート印刷
        /// </summary>
        private void F008_shippingList_Click(object sender, EventArgs e)
        {
            var form = new F008_shippingList("F008_shippingList");
            form.Show();
        }

        /// <summary>
        /// 教育履歴問合せ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F001_TrainingHisInfo_Click(object sender, EventArgs e)
        {
            //var form = new F001_TrainingHisInfo("F001_TrainingHisInfo");
            //form.Show();
        }


        #endregion  ＜問合せイベント END＞


        /// <summary>
        /// 入出庫処理
        /// </summary>
        private void IOProcessTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/入出庫処理";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var InGroup = new Group();
                InGroup.Text = "入庫処理";

                var OutGroup = new Group();
                OutGroup.Text = "出庫処理";

                var TagGroup = new Group();
                TagGroup.Text = "現品票処理";

                var CancelGroup = new Group();
                CancelGroup.Text = "返品処理";


                var OtherGroup = new Group();
                OtherGroup.Text = "その他処理";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(InGroup);
                tileControlMainMenu.Groups.Add(OutGroup);
                tileControlMainMenu.Groups.Add(TagGroup);
                tileControlMainMenu.Groups.Add(CancelGroup);
                tileControlMainMenu.Groups.Add(OtherGroup);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;


                //// 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                if (masterDBFlg == false) { AddFormList($"納入受付{ Environment.NewLine }（注番）", F206_DelivAcceptanceRoCode_Click); };
                if (masterDBFlg == false) { AddFormList($"内部受付{ Environment.NewLine }（部品）", F207_InsideAcceptanceParts_Click); };
                if (masterDBFlg == false) { AddFormList($"納入受付{ Environment.NewLine }（伝票番号）", F208_DelivAcceptanceDoCode_Click); };
                CreateFormTile(formList, InGroup);

                formList.Clear();
                if (masterDBFlg == false) { AddFormList($"出庫処理{ Environment.NewLine }（部品）", F201_OutDataParts_Click); };
                if (masterDBFlg == false) { AddFormList($"社内移行発行{ Environment.NewLine } ", F202_InsideTrans_Click); };
                if (masterDBFlg == false) { AddFormList($"有償支給発行{ Environment.NewLine }（外注）", F203_OutProcessOsrc_Click); };
                if (masterDBFlg == false) { AddFormList($"有償支給発行{ Environment.NewLine }（一般）", F204_NSupStockTrans_Click); };
                if (masterDBFlg == false) { AddFormList($"伝票再発行{ Environment.NewLine } ", F219_DocuReprint_Click); };
                AddFormList($"入出庫{ Environment.NewLine }チェックリスト ", F224_IOCheckList_Click);
                if (masterDBFlg == false) { AddFormList($"社内引渡し{ Environment.NewLine }入庫処理 ", F210_InsideInProcess_Click); };
                if (masterDBFlg == false) { AddFormList($"関係会社{ Environment.NewLine }納入処理 ", F211_CompanyDelivProcess_Click); };
                if (masterDBFlg == false) { AddFormList($"納入受付{ Environment.NewLine }バーコード ", F212_DelivBarcode_Click); };
                if (masterDBFlg == false) { AddFormList($"出庫処理{ Environment.NewLine }（部品振替）", F205_OutDataPartsExchange_Click); };
                CreateFormTile(formList, OutGroup);


                formList.Clear();
                if (masterDBFlg == false) { AddFormList($"中間組品実績{ Environment.NewLine } ", F216_IntermediateAssemblyResult_Click);};
                if (masterDBFlg == false) { AddFormList($"返品有償受付{ Environment.NewLine } ", F223_CancelNSupStockTrans_Click);};
                if (masterDBFlg == false) { AddFormList($"社内移行受付{ Environment.NewLine }（現品票番号） ", F225_InsideTransAccept_Click);};
                AddFormList($"納品書受付処理{ Environment.NewLine }  ", F226_DelivSlipReception_Click);
                AddFormList($"伝票発行処理{ Environment.NewLine }  ", F227_DocumentPrint_Click);
                AddFormList($"現品票発行処理{ Environment.NewLine }  ", F228_GoodsTagPrint_Click);
                AddFormList($"現品票{ Environment.NewLine }メンテナンス", F910_TagCodeMaint_Click);
                AddFormList($"受入承認{ Environment.NewLine }　", F231_AcceptApproval_Click); 
                CreateFormTile(formList, TagGroup);

                formList.Clear();
                AddFormList($"返品内部受付{ Environment.NewLine }（部品）", F220_InsideAcceptanceCancel_Click);
                AddFormList($"不適合品返品{ Environment.NewLine }　", F229_FailedCancel_Click);
                AddFormList($"不適合品返品{ Environment.NewLine }再発行/取消", F230_FailedCancelMaint_Click);
                CreateFormTile(formList, CancelGroup);


                // 戻るグループ
                menuGroupBack.Tiles.Add(backTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜入出庫処理 イベント＞ 

        /// <summary>
        /// 現品票未発行分現品票発行処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F228_GoodsTagPrint_Click(object sender, EventArgs e)
        {
            var form = new F228_GoodsTagPrint("F228_GoodsTagPrint");
            form.Show();
        }

        /// <summary>
        ///  伝票発行処理処理
        /// </summary>
        private void F227_DocumentPrint_Click(object sender, EventArgs e)
        {
            var form = new F227_DocumentPrint("F227_DocumentPrint");
            form.Show();
        }

        /// <summary>
        ///  納品書受付処理
        /// </summary>
        private void F226_DelivSlipReception_Click(object sender, EventArgs e)
        {
            var form = new F226_DelivSlipReception("F226_DelivSlipReception");
            form.Show();
        }

        /// <summary>
        ///  内部受付返品（部品）
        /// </summary>
        private void F220_InsideAcceptanceCancel_Click(object sender, EventArgs e)
        {
            var form = new F220_InsideAcceptanceCancel("F220_InsideAcceptanceCancel");
            form.Show();
        }

        /// <summary>
        ///  不適合品 返品
        /// </summary>
        private void F229_FailedCancel_Click(object sender, EventArgs e)
        {
            var form = new F229_FailedCancel("F229_FailedCancel");
            form.Show();
        }

        /// <summary>
        ///  不適合品 返品メンテナンス
        /// </summary>
        private void F230_FailedCancelMaint_Click(object sender, EventArgs e)
        {
            var form = new F230_FailedCancelMaint("F230_FailedCancelMaint");
            form.Show();
        }

        /// <summary>
        ///  受入承認
        /// </summary>
        private void F231_AcceptApproval_Click(object sender, EventArgs e)
        {
            var form = new F231_AcceptApproval("F231_AcceptApproval","");
            form.Show();
        }

        /// <summary>
        /// 納入受付（伝票番号）
        /// </summary>
        private void F201_DelivAcceptanceDoCode_Click(object sender, EventArgs e)
        {
            //var form = new F201_DelivAcceptanceDoCode("F201_DelivAcceptanceDoCode");
            //form.Show();
        }

        /// <summary>
        /// 納入受付（注番）
        /// </summary>
        private void F206_DelivAcceptanceRoCode_Click(object sender, EventArgs e)
        {
            var form = new F206_DelivAcceptanceRoCode("F206_DelivAcceptanceRoCode");
            form.Show();
        }

        /// <summary>
        /// 内部受付（部品）
        /// </summary>
        private void F207_InsideAcceptanceParts_Click(object sender, EventArgs e)
        {
            var form = new F207_InsideAcceptanceParts("F207_InsideAcceptanceParts");
            form.Show();
        }

        /// <summary>
        /// 納入受付（伝票番号）
        /// </summary>
        private void F208_DelivAcceptanceDoCode_Click(object sender, EventArgs e)
        {
            var form = new F208_DelivAcceptanceDoCode("F208_DelivAcceptanceDoCode");
            form.Show();
        }

        /// <summary>
        /// 出庫処理（部品）
        /// </summary>
        private void F201_OutDataParts_Click(object sender, EventArgs e)
        {
            var form = new F201_OutDataParts("F201_OutDataParts");
            form.Show();
        }

        /// <summary>
        /// 社内移行発行
        /// </summary>
        private void F202_InsideTrans_Click(object sender, EventArgs e)
        {
            var form = new F202_InsideTrans("F202_InsideTrans");
            form.Show();
        }

        /// <summary>
        /// 出庫処理外注
        /// </summary>
        private void F203_OutProcessOsrc_Click(object sender, EventArgs e)
        {
            var form = new F203_OutProcessOsrc("F203_OutProcessOsrc");
            form.Show();
        }

        /// <summary>
        /// 有償支給発行（一般）
        /// </summary>
        private void F204_NSupStockTrans_Click(object sender, EventArgs e)
        {
            var form = new F204_NSupStockTrans("F204_NSupStockTrans");
            form.Show();
        }

        /// <summary>
        /// 伝票再発行
        /// </summary>
        private void F219_DocuReprint_Click(object sender, EventArgs e)
        {
            var form = new F219_DocuReprint("F219_DocuReprint");
            form.Show();
        }

        /// <summary>
        /// 社内引渡し入庫処理
        /// </summary>
        private void F210_InsideInProcess_Click(object sender, EventArgs e)
        {
            var form = new F210_InsideInProcess("F210_InsideInProcess");
            form.Show();
        }

        /// <summary>
        /// 関係会社納入処理
        /// </summary>
        private void F211_CompanyDelivProcess_Click(object sender, EventArgs e)
        {
            var form = new F211_CompanyDelivProcess("F211_CompanyDelivProcess");
            form.Show();
        }

        /// <summary>
        /// 納入受付　バーコード
        /// </summary>
        private void F212_DelivBarcode_Click(object sender, EventArgs e)
        {
            var form = new F212_DelivBarcode("F212_DelivBarcode");
            form.Show();
        }

        /// <summary>
        /// 中間組品 実績
        /// </summary>
        private void F216_IntermediateAssemblyResult_Click(object sender, EventArgs e)
        {
            var form = new F216_IntermediateAssemblyResult("F216_IntermediateAssemblyResult");
            form.Show();
        }

        /// <summary>
        /// 入出庫チェックリスト
        /// </summary>
        private void F224_IOCheckList_Click(object sender, EventArgs e)
        {
            var form = new F224_IOCheckList("F224_IOCheckList");
            form.Show();
        }

        /// <summary>
        /// 返品有償受付
        /// </summary>
        private void F223_CancelNSupStockTrans_Click(object sender, EventArgs e)
        {
            var form = new F223_CancelNSupStockTrans("F223_CancelNSupStockTrans");
            form.Show();
        }

        /// <summary>
        /// 出庫処理（部品振替）
        /// </summary>
        private void F205_OutDataPartsExchange_Click(object sender, EventArgs e)
        {
            var form = new F205_OutDataPartsExchange("F205_OutDataPartsExchange");
            form.Show();
        }

        /// <summary>
        /// 社内移行受付（現品票番号）
        /// </summary>
        private void F225_InsideTransAccept_Click(object sender, EventArgs e)
        {
            var form = new F225_InsideTransAccept("F225_InsideTransAccept");
            form.Show();
        }

        /// <summary>
        /// 現品票メンテナンス
        /// </summary>
        private void F910_TagCodeMaint_Click(object sender, EventArgs e)
        {
            var form = new F910_TagCodeMaint("F910_TagCodeMaint");
            form.Show();
        }

        #endregion  ＜入出庫処理 イベント END＞

        /// <summary>
        /// 発注処理
        /// </summary>
        private void OrderTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/発注処理";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var Group1 = new Group();
                Group1.Text = "";

                var InfoGroup = new Group();
                InfoGroup.Text = "問合せ処理";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(Group1);
                tileControlMainMenu.Groups.Add(InfoGroup);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;

                // 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                AddFormList($"発注確定処理{ Environment.NewLine } ", F401_OrdSourceProcess_Click);
                AddFormList($"都度発注処理{ Environment.NewLine } ", F404_EachTimeOrd_Click);
                CreateFormTile(formList, Group1);

                formList.Clear();
                AddFormList($"発注状況問合せ{ Environment.NewLine } ", F405_OrderStatusInfo_Click);
                AddFormList($"工程単価問合せ{ Environment.NewLine } ", F406_ProcessUnitPriceInfo_Click);
                CreateFormTile(formList, InfoGroup);


                // 戻るグループ
                menuGroupBack.Tiles.Add(backTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜発注処理 イベント＞ 

        /// <summary>
        /// 発注確定処理
        /// </summary>
        private void F401_OrdSourceProcess_Click(object sender, EventArgs e)
        {
            var form = new F401_OrdSourceProcess("F401_OrdSourceProcess");
            form.Show();
        }

        /// <summary>
        /// 都度発注処理
        /// </summary>
        private void F404_EachTimeOrd_Click(object sender, EventArgs e)
        {
            var form = new F404_EachTimeOrd("F404_EachTimeOrd");
            form.Show();
        }

        /// <summary>
        /// 発注状況問合せ
        /// </summary>
        private void F405_OrderStatusInfo_Click(object sender, EventArgs e)
        {
            var form = new F405_OrderStatusInfo("F405_OrderStatusInfo");
            form.Show();
        }

        /// <summary>
        /// 工程単価問合せ
        /// </summary>
        private void F406_ProcessUnitPriceInfo_Click(object sender, EventArgs e)
        {
            var form = new F406_ProcessUnitPriceInfo("F406_ProcessUnitPriceInfo");
            form.Show();
        }

        #endregion  ＜発注処理 イベント END＞

        /// <summary>
        /// 月末業務
        /// </summary>
        private void EOMProcessTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/月末業務";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var Group1 = new Group();
                Group1.Text = "";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(Group1);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;

                // 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                AddFormList($"返品内部受付{ Environment.NewLine }（部品）", F220E_InsideAcceptanceCancel_Click);
                AddFormList($"月末出庫処理{ Environment.NewLine }（部品）", F201E_OutDataParts_Click);
                AddFormList($"月末有償支給{ Environment.NewLine }発行（一般）", F204E_NSupStockTrans_Click);
                AddFormList($"月末{ Environment.NewLine }中間組品実績 ", F216E_IntermediateAssemblyResult_Click);
                AddFormList($"月末{ Environment.NewLine }返品有償受付 ", F223E_CancelNSupStockTrans_Click);
                AddFormList($"月末{ Environment.NewLine }社内移行発行", F202E_InsideTrans_Click);
                AddFormList($"月末有償支給{ Environment.NewLine }発行（外注）", F203E_OutProcessOsrc_Click);
                AddFormList($"月末出庫処理{ Environment.NewLine }（部品振替）", F205E_OutDataPartsExchange_Click);
                AddFormList($"月末納入受付{ Environment.NewLine }（注番）", F206E_DelivAcceptanceRoCode_Click);
                AddFormList($"月末内部受付{ Environment.NewLine }（部品）", F207E_InsideAcceptanceParts_Click);
                AddFormList($"月末納入受付{ Environment.NewLine }（伝票番号）", F208E_DelivAcceptanceDoCode_Click);

                CreateFormTile(formList, Group1);

                // 戻るグループ
                menuGroupBack.Tiles.Add(backTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜月末業務 イベント＞ 

        /// <summary>
        ///  月末 内部受付返品（部品）
        /// </summary>
        private void F220E_InsideAcceptanceCancel_Click(object sender, EventArgs e)
        {
            var form = new F220E_InsideAcceptanceCancel("F220E_InsideAcceptanceCancel");
            form.Show();
        }

        /// <summary>
        /// 月末出庫処理（部品）
        /// </summary>
        private void F201E_OutDataParts_Click(object sender, EventArgs e)
        {
            var form = new F201E_OutDataParts("F201E_OutDataParts");
            form.Show();
        }

        /// <summary>
        /// 月末有償支給発行（一般）
        /// </summary>
        private void F204E_NSupStockTrans_Click(object sender, EventArgs e)
        {
            var form = new F204E_NSupStockTrans("F204E_NSupStockTrans");
            form.Show();
        }

        /// <summary>
        /// 月末中間組品 実績
        /// </summary>
        private void F216E_IntermediateAssemblyResult_Click(object sender, EventArgs e)
        {
            var form = new F216E_IntermediateAssemblyResult("F216E_IntermediateAssemblyResult");
            form.Show();
        }

        /// <summary>
        /// 月末返品有償受付
        /// </summary>
        private void F223E_CancelNSupStockTrans_Click(object sender, EventArgs e)
        {
            var form = new F223E_CancelNSupStockTrans("F223E_CancelNSupStockTrans");
            form.Show();
        }

        /// <summary>
        /// 月末社内移行発行
        /// </summary>
        private void F202E_InsideTrans_Click(object sender, EventArgs e)
        {
            var form = new F202E_InsideTrans("F202E_InsideTrans");
            form.Show();
        }

        /// <summary>
        /// 出庫処理外注
        /// </summary>
        private void F203E_OutProcessOsrc_Click(object sender, EventArgs e)
        {
            var form = new F203E_OutProcessOsrc("F203E_OutProcessOsrc");
            form.Show();
        }

        /// <summary>
        /// 月末出庫処理（部品振替）
        /// </summary>
        private void F205E_OutDataPartsExchange_Click(object sender, EventArgs e)
        {
            var form = new F205E_OutDataPartsExchange("F205E_OutDataPartsExchange");
            form.Show();
        }

        /// <summary>
        /// 月末納入受付（注番）
        /// </summary>
        private void F206E_DelivAcceptanceRoCode_Click(object sender, EventArgs e)
        {
            var form = new F206E_DelivAcceptanceRoCode("F206E_DelivAcceptanceRoCode");
            form.Show();
        }

        /// <summary>
        /// 月末内部受付（部品）
        /// </summary>
        private void F207E_InsideAcceptanceParts_Click(object sender, EventArgs e)
        {
            var form = new F207E_InsideAcceptanceParts("F207E_InsideAcceptanceParts");
            form.Show();
        }

        /// <summary>
        /// 月末納入受付（伝票番号）
        /// </summary>
        private void F208E_DelivAcceptanceDoCode_Click(object sender, EventArgs e)
        {
            var form = new F208E_DelivAcceptanceDoCode("F208E_DelivAcceptanceDoCode");
            form.Show();
        }

        #endregion  ＜月末業務 イベント END＞

        /// <summary>
        /// マスタメンテナンス
        /// </summary>
        private void MstMaintTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/マスタメンテナンス";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var Group1 = new Group();
                Group1.Text = "";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(Group1);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;

                // 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                AddFormList($"作業者マスタ{ Environment.NewLine }メンテナンス", F601_WorkerMstMaint_Click);
                AddFormList($"単価マスタ{ Environment.NewLine }メンテナンス", F601_UintPriceMasterMaint_Click);
                AddFormList($"仕入先マスタ{ Environment.NewLine }メンテナンス", F602_SupMstMaint_Click);
                AddFormList($"カレンダ{ Environment.NewLine }ファイル作成", F603_CalendarFileMaint_Click);
                CreateFormTile(formList, Group1);

                // 戻るグループ
                menuGroupBack.Tiles.Add(backTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜マスタメンテナンス イベント＞ 

        /// <summary>
        /// 作業者マスタメンテナンス
        /// </summary>
        private void F601_WorkerMstMaint_Click(object sender, EventArgs e)
        {
            //var form = new F601_WorkerMstMaint("F601_WorkerMstMaint");
            //form.Show();
        }

        /// <summary>
        /// 単価マスタメンテナンス
        /// </summary>
        private void F601_UintPriceMasterMaint_Click(object sender, EventArgs e)
        {
            var form = new F601_UintPriceMasterMaint("F601_UintPriceMasterMaint");
            form.Show();
        }

        /// <summary>
        /// 仕入先マスタメンテナンス
        /// </summary>
        private void F602_SupMstMaint_Click(object sender, EventArgs e)
        {
            var form = new F602_SupMstMaint("F602_SupMstMaint");
            form.Show();
        }

        /// <summary>
        /// カレンダファイル作成
        /// </summary>
        private void F603_CalendarFileMaint_Click(object sender, EventArgs e)
        {
            var form = new F603_CalendarFileMaint("F603_CalendarFileMaint");
            form.Show();
        }

        #endregion  ＜マスタメンテナンス イベント END＞


        /// <summary>
        /// 開発支援
        /// </summary>
        private void DevelopSupportTile_Click(object sender, EventArgs e)
        {
            tileControlMainMenu.BeginUpdate();

            tileControlMainMenu.Groups.Clear(true);

            try
            {
                //タイトル変更
                label_main_title.Text += "/開発支援";

                // 共通ショートカットタイル作成
                CreateShortcutTile(sender);

                // グループ作成
                var Group1 = new Group();
                Group1.Text = "";

                var menuGroupBack = new Group();
                menuGroupBack.Text = "";

                tileControlMainMenu.Groups.Add(Group1);
                tileControlMainMenu.Groups.Add(menuGroupBack);
                tileControlMainMenu.GroupTextSize = 15;
                tileControlMainMenu.CellHeight = 65;
                tileControlMainMenu.CellWidth = 145;

                var backTile = new Tile();
                backTile.Text = "戻る";
                backTile.Click += BackMainTile_Click;

                B_Back.Visible = true;
                B_Back.Click += BackMainTile_Click;

                // 画面タイル作成（画面タイトル、クリックイベント）
                formList.Clear();
                AddFormList($"画面表示{ Environment.NewLine }サンプル", PictureDisplaySample_Click);
                AddFormList($"シンプル画面{ Environment.NewLine }サンプル", BaseFormSimpleSample_Click);
                CreateFormTile(formList, Group1);

                // 戻るグループ
                menuGroupBack.Tiles.Add(backTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            tileControlMainMenu.EndUpdate();
        }

        #region ＜開発支援　イベント＞ 
        
        /// <summary>
        /// サンプル
        /// </summary>
        private void PictureDisplaySample_Click(object sender, EventArgs e)
        {
            var form = new Sample01("Sample01");
            form.Show();
        }

        /// <summary>
        /// BaseFormSimpleを使ったサンプル画面
        /// </summary>
        private void BaseFormSimpleSample_Click(object sender, EventArgs e)
        {
            //var form = new BaseFormSimpleSample("BaseFormSimpleSample");
            //form.Show();
        }

        #endregion  ＜開発支援　イベント END＞



        /// <summary>
        /// 戻る処理
        /// </summary>
        private void BackMainTile_Click(object sender, EventArgs e)
        {
            this.ShowMainMenu();
        }

        #endregion


        #region<その他>

        /// <summary>
        /// お知らせ画面　起動処理
        /// </summary>
        private void ShowInformation()
        {
            if (this.information == null || this.information.IsDisposed)
            {
                information = new SansoBase.Information(Path.GetFileName(Environment.GetCommandLineArgs()[0]), "");
                information.StartPosition = FormStartPosition.CenterScreen;
                information.Show();
            }
            else if (!this.information.Visible)
            {
                this.information.Show();
            }
            else
            {
                this.information.Activate();
            }

        }


        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 画面を開いたログを残す
                var data = new HistoryData(
                    Path.GetFileName(Environment.GetCommandLineArgs()[0]),
                    this.GetType().Name,
                    LoginInfo.Instance.MachineCode,
                    LoginInfo.Instance.UserNo,
                    "CLOSE",
                    "",         // SQLはないので空
                    "",         // SQLパラメータはないので空
                    LoginInfo.Instance.IPAdress
                );
                History.WriteFormAccessHistory(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void B_Close_Click(object sender, EventArgs e)
        {
            RelogFlg = false;
            // アプリケーション終了
            this.Close();
        }


        /// <summary>
        /// メニューグループリストへ追加
        /// </summary>
        /// <param name="groupID">グループID。stringl型</param>
        /// <param name="groupName">グループ名。string型</param>
        public void AddMenuGroupList(string groupID, string groupName)
        {
            menuGroupList.Add((groupID, groupName));
        }

        /// <summary>
        /// メニューリストへ追加
        /// </summary>
        /// <param name="groupID">グループID。stringl型</param>
        /// <param name="menuName">メニュー名。stringl型</param>
        /// <param name="menuID">メニューID。stringl型</param>
        /// <param name="backColor">メニューボタン背景色。Color型</param>
        /// <param name="eventMethod">メニューボタンクリックイベント。EventHandler型</param>
        public void AddMenuList(string groupID, string menuName, string menuID, Color backColor, EventHandler eventMethod)
        {
            MenuParam param = new MenuParam(groupID, menuName, menuID, backColor, eventMethod);
            menuList.Add(param);
        }

        /// <summary>
        /// フォームリストへ追加
        /// </summary>
        /// <param name="groupID">グループID。stringl型</param>
        /// <param name="groupName">グループ名。string型</param>
        public void AddFormList(string formName, EventHandler eventMethod)
        {
            formList.Add((formName, eventMethod));
        }

        /// <summary>
        /// 画面タイル作成
        /// </summary>
        private void CreateFormTile(List<(string FormName, EventHandler EventMethod)> list, Group group)
        {
            foreach (var v in list)
            {
                // 利用不可画面のチェック（未実装）
                if (true)
                {
                    var tile = new Tile();
                    tile.Text = v.FormName;
                    tile.Click += v.EventMethod;
                    group.Tiles.Add(tile);
                }
            }
        }

        #endregion


    }
}