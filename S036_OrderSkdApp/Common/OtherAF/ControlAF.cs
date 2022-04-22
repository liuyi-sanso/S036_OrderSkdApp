using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S036_OrderSkdApp
{
    public static class ControlAF
    {

        /// <summary>
        /// C1ComboBoxにDisplayMemberとValueMemberをセットする
        /// </summary>
        ///// <param name="ComboBox">C1ComboBoxのオブジェクト</param>
        /// <param name="dt">リスト化するデータテーブル</param>
        /// <param name="ItemsDisplayMember">DisplayMemberに使用する列名。省略可。デフォルトは０列目タイトル</param>
        /// <param name="ItemsValueMember">ValueMemberに使用する列名。省略可。デフォルトは０列目タイトル</param>
        /// <param name="ItemsDisplayMemberWidth">DisplayMemberの幅。省略可。デフォルトは50</param>
        /// <param name="ItemsValueMemberWidth">ValueMemberの幅。省略可。デフォルトは200</param>
        public static void SetC1ComboBox(C1.Win.C1Input.C1ComboBox ComboBox, DataTable dt, int ItemsDisplayMemberWidth = 50, int ItemsValueMemberWidth = 200, string ItemsDisplayMember = "", string ItemsValueMember = "",bool SingleComboBox = false)
        {
            dt.CaseSensitive = true;
            ComboBox.TextDetached = true;
            ComboBox.ItemsDataSource = dt.DefaultView;
            if (ItemsDisplayMember == "")
            {
                ItemsDisplayMember = dt.Columns[0].ColumnName;
            }
            if (ItemsValueMember == "")
            {
                ItemsValueMember = dt.Columns[1].ColumnName;
            }
            ComboBox.ItemsDisplayMember = ItemsDisplayMember;
            ComboBox.ItemsValueMember = ItemsDisplayMember;
            ComboBox.ItemMode = C1.Win.C1Input.ComboItemMode.HtmlPattern;

            if (SingleComboBox)
            {
                ComboBox.HtmlPattern = "<table><tr><td width=" + ItemsValueMemberWidth + ">{" + ItemsValueMember + "}</td></tr></table>";
            }
            else
            {
                ComboBox.HtmlPattern = "<table><tr><td width=" + ItemsDisplayMemberWidth + ">{" + ItemsDisplayMember + "}</td><td width=" + ItemsValueMemberWidth + ">{" + ItemsValueMember + "}</td></tr></table>";
            }
        }



        /// <summary>
        /// テキストボックス転記
        /// </summary>
        /// <param name="dt">転記元データテーブル</param>
        /// <param name="TextBox">転記先テキストボックスオブジェクト</param>
        /// <returns></returns>
        public static bool SetC1TextBoxData(DataTable dt,List<C1.Win.C1Input.C1TextBox> TextBox)
        {
            if (dt.Rows.Count < 1)
            {
                return false;
            }

            foreach (var v in TextBox)
            {
                v.Text = dt.Rows[0].Field<string>(v.Label.Text) ?? string.Empty;
            }

            return true;
        }



        /// <summary>
        /// コンボボックスリストの存在チェック
        /// </summary>
        /// <param name="ComboBox">コンボボックスオブジェクト</param>
        /// <param name="Name">リスト名オブジェクト</param>
        /// <param name="Code">存在チェックするコード</param>
        /// <returns></returns>
        public static bool CheckComboBoxList(C1.Win.C1Input.C1ComboBox ComboBox, C1.Win.C1Input.C1TextBox Name)
        {
            if ((ComboBox.Items.Count < 1) && (ComboBox.Text != ""))
            {
                if (Name != null)
                {
                    Name.Text = "";
                }         
                return false;
            }

            if (string.IsNullOrEmpty(ComboBox.Text))
            {
                var dv2 = (System.Data.DataView)ComboBox.ItemsDataSource;
                if (dv2 == null)
                {
                    if (Name != null)
                    {
                        Name.Text = "";
                    }
                    return true;
                }

                dv2.RowFilter = dv2.Table.Columns[0].ColumnName + " = '' ";
                if (dv2.Count < 1)
                {
                    if (Name != null)
                    {
                        Name.Text = "";
                    }
                    return true;
                }
            }

            var dv = (System.Data.DataView)ComboBox.ItemsDataSource;
            dv.RowFilter = dv.Table.Columns[0].ColumnName + " = '" + ComboBox.Text + "' ";
            if (dv.Count > 0)
            {
                if (Name != null)
                {
                    Name.Text = dv.ToTable().Rows[0][1].ToString();
                }
                return true;
            }
            else
            {
                if (Name != null)
                {
                    Name.Text = "";
                }
                return false;
            }

        }

        /// <summary>
        /// GETメソッドでWebAPIを実行
        /// </summary>
        /// <param name="apiUrl">WebAPIのURL</param>
        /// <param name="jsonParameter">GETのJSONパラメータ</param>
        /// <param name="token">jwt認証用トークン</param>
        /// <returns>HTTPのレスポンス、ない場合はnull
        /// 【再ログインが実行された場合の戻り値。
        /// result["Status"]：false、result["reLogIn"]：null以外、かつtrue：再ログインＯＫ
        /// result["Status"]：false、result["reLogIn"]：null以外、かつfalse：再ログインＮＧ
        /// result["Status"]：false、result["reLogIn"]：null：その他のエラー</returns>
        public static JObject GetRequest(string apiUrl, JObject jsonParameter, string token)
        {
            JObject response = new JObject();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";
                request.ContentType = "application/json;";
                request.Headers.Add("Authorization", token);

                using (var res = (HttpWebResponse)request.GetResponse())
                using (var s = res.GetResponseStream())
                using (var sr = new StreamReader(s, Encoding.UTF8))
                {
                    response = JObject.Parse(sr.ReadToEnd());

                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        response["Status"] = (int)res.StatusCode;
                    }
                }
            }
            catch (WebException wex)
            {
                if (wex.Response == null)
                {
                    return response;
                }

                // tokenの有効期限切れの場合
                HttpWebResponse errres = (System.Net.HttpWebResponse)wex.Response;
                if (errres.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show($"ログイン有効期限が切れました。ログイン後、再度実行してください。{Environment.NewLine}",
                                    "ログイン有効期限切れエラー",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    // ログイン画面起動 + ユーザ情報設定
                    var loginForm = new SansoBase.LoginMenu.LoginMenu();
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.ShowDialog();
                    var loginOk = loginForm.IsLoginOk;

                    if (loginOk)
                    {
                        return new JObject
                                {
                                    { "Status", false },
                                    { "reLogIn", true }
                                };
                    }
                    else
                    {
                        return new JObject
                                {
                                    { "Status", false },
                                    { "reLogIn", false }
                                };
                    }
                }


                try
                {
                    using (var res = (HttpWebResponse)wex.Response)
                    using (var s = res.GetResponseStream())
                    using (var sr = new StreamReader(s, Encoding.UTF8))
                    {
                        response = JObject.Parse(sr.ReadToEnd());

                        if (res.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            response["Status"] = (int)res.StatusCode;
                        }
                    }
                }
                catch
                {
                    return response;
                }
            }

            return response;
        }

        /// <summary>
        /// 機種マスタの項目セット
        /// </summary>
        //public static bool SetProductMstColumn(string SearchCode,List<object> Column)
        //{







        //}

    }
}
