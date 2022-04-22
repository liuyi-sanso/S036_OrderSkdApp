using SansoBase;
using System;
using System.Data.SqlClient;

namespace S036_OrderSkdApp
{
    class SendMailMstEditor
    {
        /// <summary>
        /// 端末名
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SubjectNo { get; set; }

        /// <summary>
        /// 件名
        /// </summary>
        public string SubjectMessage { get; set; }

        /// <summary>
        /// 本文
        /// </summary>
        public string BodyMessage { get; set; }

        /// <summary>
        /// 重要度 0：指定無し、1：低、3：高
        /// </summary>
        public int PriorityNum { get; set; }

        /// <summary>
        /// 添付ファイルのパス（19/12/24 まだ未実装）
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 送信者
        /// </summary>
        public string SendFromAddress { get; set; }

        /// <summary>
        /// 宛先TO（複数の場合は「;」区切り）
        /// </summary>
        public string SendToAddress { get; set; }

        /// <summary>
        /// 宛先CC（複数の場合は「;」区切り）
        /// </summary>
        public string SendCcAddress { get; set; }

        /// <summary>
        /// 宛先BCC（19/12/24 まだ未実装）
        /// </summary>
        public string SendBccAddress { get; set; }

        /// <summary>
        /// 備考（メールには関係なし）
        /// </summary>
        public string remarks { get; set; }
        
        /// <summary>
        /// 送信日付
        /// </summary>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// ステータス 0：未送信、1：送信済、8：保留・テスト、9：削除
        /// </summary>
        public string StatusCate { get; set; }
        
        /// <summary>
        /// 作成日付
        /// </summary>
        public DateTime CreateDate { get; set; }
        
        /// <summary>
        /// 変更日付
        /// </summary>
        public DateTime UpdateDate { get; set; }
        
        /// <summary>
        /// 作成者
        /// </summary>
        public string CreateStaffCode { get; set; }
        
        /// <summary>
        /// 変更者
        /// </summary>
        public string UpdateStaffCode { get; set; }
        
        /// <summary>
        /// 作成者ID
        /// </summary>
        public string CreateID { get; set; }
        
        /// <summary>
        /// 変更者ID
        /// </summary>
        public string UpdateID { get; set; }


        public (bool IsOk, string Msg) CreateSendMailMst(SqlConnection con, SqlTransaction tran = null)
        {
            try
            {
                // SendMailMst 更新
                string sql = "INSERT INTO 三相メイン.dbo.SendMailMst " +
                              "(DeviceName, " +
                              "SubjectMessage, " +
                              "BodyMessage, " +
                              "PriorityNum, " +
                              "SendFromAddress, " +
                              "SendToAddress, " +
                              "SendCcAddress, " +
                              "SendBccAddress, " +
                              "remarks, " +
                              "StatusCate, " +
                              "CreateDate, " +
                              "CreateID, " +
                              "CreateStaffCode)" +
                              "VALUES(" +
                              "'" + DeviceName + "', " +
                              "'" + SubjectMessage + "', " +
                              "'" + BodyMessage + "', " +
                              "'" + PriorityNum + "', " +
                              "'" + SendFromAddress + "', " +
                              "" + SendToAddress + ", " +
                              "'" + SendCcAddress + "', " +
                              "'" + SendBccAddress + "', " +
                              "'" + remarks + "', " +
                              "'" + StatusCate + "', " +
                              "'" + CreateDate + "', " +
                              "'" + CreateID + "', " +
                              "'" + CreateStaffCode + "') ";

                if (CommonAF.ExecutInsertSQL(sql, tran, con).IsOk == false)
                {
                    return (false, "メール送信内容更新時にエラーが発生しました");
                }
            }
            catch
            {
                return (false, "メール送信内容更新時にエラーが発生しました");
            }
            return (true, "");
        }

    }
}
