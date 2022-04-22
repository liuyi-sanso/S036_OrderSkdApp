using SansoBase;
using System;
using System.Data;
using System.Data.SqlClient;

namespace S036_OrderSkdApp
{
    class IOFileEditor
    {
        /// <summary>
        ///  データ区分
        /// <summary>
        public string DataCate { get; set; }

        /// <summary>
        ///  部品コード
        /// <summary>
        public string PartsCode { get; set; }

        /// <summary>
        ///  仕入先コード
        /// <summary>
        public string SupCode { get; set; }

        /// <summary>
        ///  工程コード
        /// <summary>
        public string ProcessCode { get; set; }

        /// <summary>
        ///  号機
        /// <summary>
        public string Machine { get; set; }

        /// <summary>
        ///  号機コード
        /// <summary>
        public string MachineCode { get; set; }

        /// <summary>
        ///  注文番号
        /// <summary>
        public string PoCode { get; set; }

        /// <summary>
        ///  工事番号
        /// <summary>
        public string JyuyoyosokuCode { get; set; }

        /// <summary>
        ///  入庫数
        /// <summary>
        public decimal? InNum { get; set; }

        /// <summary>
        ///  出庫数
        /// <summary>
        public decimal? OutNum { get; set; }

        /// <summary>
        ///  単価区分
        /// <summary>
        public string UnitPriceCate { get; set; }

        /// <summary>
        ///  単価
        /// <summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        ///  加工単価
        /// <summary>
        public decimal? ProcessUnitPrice { get; set; }

        /// <summary>
        ///  金額
        /// <summary>
        public decimal Price { get; set; }

        /// <summary>
        ///  受払年月日
        /// <summary>
        public DateTime AcceptDate { get; set; }

        /// <summary>
        ///  課別コード
        /// <summary>
        public string GroupCode { get; set; }

        /// <summary>
        ///  科目コード
        /// <summary>
        public string AccountCode { get; set; }

        /// <summary>
        ///  出庫区分
        /// <summary>
        public string OutDataCate { get; set; }

        /// <summary>
        ///  伝票番号
        /// <summary>
        public string DoCode { get; set; }

        /// <summary>
        ///  在庫Ｐ
        /// <summary>
        public string StockCate { get; set; }

        /// <summary>
        ///  有償Ｐ
        /// <summary>
        public string NSupCate { get; set; }

        /// <summary>
        ///  パスワード
        /// <summary>
        public string Password { get; set; }

        /// <summary>
        ///  送信済みＦ
        /// <summary>
        public string SentFlg { get; set; }

        /// <summary>
        ///  有償データＦ
        /// <summary>
        public string NSupDataFlg { get; set; }

        /// <summary>
        ///  実績データＦ
        /// <summary>
        public string PerformanceDataFlg { get; set; }

        /// <summary>
        ///  社内移行データＦ
        /// <summary>
        public string InsideTransDataFlg { get; set; }

        /// <summary>
        ///  有償支給データＦ
        /// <summary>
        public string NSupStockTransDataFlg { get; set; }

        /// <summary>
        ///  納品書データＦ
        /// <summary>
        public string DelivSlipDataFlg { get; set; }

        /// <summary>
        ///  初品検査Ｆ
        /// <summary>
        public string FirstProductChkFlg { get; set; }

        /// <summary>
        ///  進捗F
        /// <summary>
        public string ScheduleFlg { get; set; }

        /// <summary>
        ///  進捗数
        /// <summary>
        public decimal ScheduleNum { get; set; }

        /// <summary>
        ///  実績番号
        /// <summary>
        public string PerformanceCode { get; set; }

        /// <summary>
        ///  有償送信済みＦ
        /// <summary>
        public string NSupSentFlg { get; set; }

        /// <summary>
        ///  送受信管理番号
        /// <summary>
        public string SendReceiveManageCode { get; set; }

        /// <summary>
        ///  作成日付
        /// <summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  変更日付
        /// <summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        ///  返品大分類
        /// <summary>
        public string ProductReturnLargeCate { get; set; }

        /// <summary>
        ///  返品中分類
        /// <summary>
        public string ProductReturnMiddleCate { get; set; }

        /// <summary>
        ///  返品備考
        /// <summary>
        public string ProductReturnRemarks { get; set; }

        /// <summary>
        ///  進捗数分類
        /// <summary>
        public string ScheduleNumCate { get; set; }

        /// <summary>
        ///  バッチ処理Ｆ(バッチ処理の場合に使う)
        /// <summary>
        public string BatchProcessFlg { get; set; }

        /// <summary>
        ///  端末名
        /// <summary>
        public string MachineName { get; set; }

        /// <summary>
        ///  仕損設備
        /// <summary>
        public string ScrapEquipment { get; set; }

        /// <summary>
        ///  仕損内容
        /// <summary>
        public string ScrapContent { get; set; }

        /// <summary>
        ///  作業時間
        /// <summary>
        public int WorkTime { get; set; }

        /// <summary>
        ///  段取時間
        /// <summary>
        public int SetupTime { get; set; }

        /// <summary>
        ///  作業者名
        /// <summary>
        public string WorkerName { get; set; }

        /// <summary>
        ///  不良原因
        /// <summary>
        public string DefectReason { get; set; }

        /// <summary>
        ///  責任区分
        /// <summary>
        public string ResponsibilityCate { get; set; }

        /// <summary>
        ///  修正Ｆ
        /// <summary>
        public string UpdateFlg { get; set; }

        /// <summary>
        ///  備考
        /// <summary>
        public string Remarks { get; set; }

        /// <summary>
        ///  オートＮＯ２
        /// <summary>
        public Int64 AutoCode2 { get; set; }

        /// <summary>
        ///  作業者
        /// <summary>
        public string Worker { get; set; }

        /// <summary>
        ///  金型償却費
        /// <summary>
        public decimal MoldDepreciationPrice { get; set; }

        /// <summary>
        ///  作番
        /// <summary>
        public string Sakuban { get; set; }

        /// <summary>
        ///  工程作業者
        /// <summary>
        public string ProcessWorker { get; set; }

        /// <summary>
        ///  残業Ｆ
        /// <summary>
        public string OvertimeFlg { get; set; }

        /// <summary>
        ///  DEN_NO
        /// <summary>
        public string DEN_NO { get; set; }

        /// <summary>
        ///  作成者ID
        /// <summary>
        public string CreateID { get; set; }

        /// <summary>
        ///  変更者ID
        /// <summary>
        public string UpdateID { get; set; }

        /// <summary>
        ///  返品区分
        /// <summary>
        public string ReturnCate { get; set; }

        /// <summary>
        ///  作成者
        /// <summary>
        public string CreateStaffCode { get; set; }

        /// <summary>
        ///  変更者
        /// <summary>
        public string UpdateStaffCode { get; set; }

        /// <summary>
        ///  データ区分詳細
        /// <summary>
        public string DataCateDetail { get; set; }

        /// <summary>
        ///  不良区分
        /// <summary>
        public string DefectCate { get; set; }

        /// <summary>
        ///  不良内容
        /// <summary>
        public string DefectContent { get; set; }

        /// <summary>
        ///  勤務
        /// <summary>
        public string Work { get; set; }

        /// <summary>
        ///  段取者
        /// <summary>
        public string SetupID { get; set; }

        /// <summary>
        ///  納入先コード
        /// <summary>
        public string DelivCode { get; set; }

        /// <summary>
        ///  部品名
        /// <summary>
        public string PartsName { get; set; }

        /// <summary>
        ///  部品サイズ
        /// <summary>
        public string PartsSize { get; set; }

        /// <summary>
        ///  伝票番号２
        /// <summary>
        public string DoCode2 { get; set; }

        /// <summary>
        ///  連記式納品書発行F
        /// <summary>
        public Byte? SeqDelivSlipIssueF { get; set; }

        /// <summary>
        ///  現品票番号
        /// <summary>
        public string GoodsTagCode { get; set; }

        /// <summary>
        ///  作業指示番号
        /// <summary>
        public string WorkInstructionCode { get; set; }

        /// <summary>
        ///  代表機種名
        /// <summary>
        public string RepProductName { get; set; }

        /// <summary>
        /// 金額　  計算＆エラーチェック
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="unitPrice">単価</param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        /// <returns>Price(金額)</returns> 
        private (bool IsOk, string Msg, decimal Price) CalPrice(decimal num, decimal unitPrice)
        {
            var price = num * unitPrice;

            // 四捨五入
            price = (price > 0m) ? (((Int64)(System.Math.Abs(price) + 0.5m)) * 1m) : (((Int64)(System.Math.Abs(price) + 0.5m)) * (-1m));

            // 数値項目の桁数範囲チェック
            if (Check.IsPointNumberRange(price, 11, 0).Result == false)
            {
                return (false, Check.IsPointNumberRange(price, 11, 0).Msg, 0m);
            }
            return (true, "", price);
        }

        /// <summary>
        /// 共通エラーチェック
        /// </summary>
        /// <param name="num">ioFile</param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        private (bool IsOk, string Msg) ErrorCheck(IOFileEditor ioFile)
        {
            if (string.IsNullOrEmpty(ioFile.DataCate))
            {
                return (false, "データ区分は必須項目です");
            }
            if (string.IsNullOrEmpty(ioFile.PartsCode))
            {
                return (false, "部品コードは必須項目です");
            }
            if (string.IsNullOrEmpty(ioFile.SupCode))
            {
                return (false, "仕入先コードは必須項目です");
            }
            if (ioFile.AcceptDate.Year == 1)
            {
                return (false, "受払年月日は必須項目です");
            }
            if (string.IsNullOrEmpty(ioFile.GroupCode))
            {
                return (false, "課別コードは必須項目です");
            }
            if (ioFile.StockCate == null)
            {
                return (false, "在庫Ｐは必須項目です");
            }
            if (ioFile.CreateDate.Year == 1)
            {
                return (false, "作成日付は必須項目です");
            }
            if (ioFile.UnitPrice == null)
            {
                return (false, "単価は必須項目です");
            }

            switch (ioFile.DataCate)
            {               
                case "1": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.UnitPriceCate))
                    {
                        return (false, "単価区分は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "2": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.UnitPriceCate))
                    {
                        return (false, "単価区分は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "3": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.UnitPriceCate))
                    {
                        return (false, "単価区分は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "5": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    break;

                case "F": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "S": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    if (ioFile.ProcessUnitPrice == null)
                    {
                        return (false, "加工単価は必須項目です");
                    }
                    break;

                case "7": // 入庫
                    if (ioFile.InNum == null)
                    {
                        return (false, "入庫数は必須項目です");
                    }
                    break;

                case "4": // 出庫
                    if (ioFile.OutNum == null)
                    {
                        return (false, "出庫数は必須項目です");
                    }
                    break;

                case "6": // 出庫
                    if (ioFile.OutNum == null)
                    {
                        return (false, "出庫数は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "8": // 出庫
                    if (ioFile.OutNum == null)
                    {
                        return (false, "出庫数は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                case "U": // 出庫
                    if (ioFile.OutNum == null)
                    {
                        return (false, "出庫数は必須項目です");
                    }
                    if (string.IsNullOrEmpty(ioFile.DoCode))
                    {
                        return (false, "伝票番号は必須項目です");
                    }
                    break;

                default:
                    return (false, "データ区分は必須項目です");
            }

            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：1】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileCommonBuy(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if(ioFile.DataCate != "1")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "1"));        
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, "060"));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));         
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.NSupStockTransDataFlg, string.IsNullOrEmpty(ioFile.NSupStockTransDataFlg) ? "" : ioFile.NSupStockTransDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnLargeCate, string.IsNullOrEmpty(ioFile.ProductReturnLargeCate) ? "" : ioFile.ProductReturnLargeCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnMiddleCate, string.IsNullOrEmpty(ioFile.ProductReturnMiddleCate) ? "" : ioFile.ProductReturnMiddleCate.Trim()));                
                v.InsertIntoColuList.Add((v.BatchProcessFlg, string.IsNullOrEmpty(ioFile.BatchProcessFlg) ? "" : ioFile.BatchProcessFlg.Trim()));
                v.InsertIntoColuList.Add((v.DelivSlipDataFlg, string.IsNullOrEmpty(ioFile.DelivSlipDataFlg) ? "" : ioFile.DelivSlipDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnRemarks, string.IsNullOrEmpty(ioFile.ProductReturnRemarks) ? "" : ioFile.ProductReturnRemarks.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                //v.InsertIntoColuList.Add((v.DelivCode , string.IsNullOrEmpty(ioFile.DelivCode) ? "" : ioFile.DelivCode.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：2】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileInsideBuy(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "2")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "2"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, ""));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.InsideTransDataFlg, string.IsNullOrEmpty(ioFile.InsideTransDataFlg) ? "" : ioFile.InsideTransDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.BatchProcessFlg, string.IsNullOrEmpty(ioFile.BatchProcessFlg) ? "" : ioFile.BatchProcessFlg.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnLargeCate, string.IsNullOrEmpty(ioFile.ProductReturnLargeCate) ? "" : ioFile.ProductReturnLargeCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnMiddleCate, string.IsNullOrEmpty(ioFile.ProductReturnMiddleCate) ? "" : ioFile.ProductReturnMiddleCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnRemarks, string.IsNullOrEmpty(ioFile.ProductReturnRemarks) ? "" : ioFile.ProductReturnRemarks.Trim()));              
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：3】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileOutsideBuy(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "3")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "3"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, "083"));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.OutDataCate, string.IsNullOrEmpty(ioFile.OutDataCate) ? "" : ioFile.OutDataCate.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));          
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.ProductReturnLargeCate, string.IsNullOrEmpty(ioFile.ProductReturnLargeCate) ? "" : ioFile.ProductReturnLargeCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnMiddleCate, string.IsNullOrEmpty(ioFile.ProductReturnMiddleCate) ? "" : ioFile.ProductReturnMiddleCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupStockTransDataFlg, string.IsNullOrEmpty(ioFile.NSupStockTransDataFlg) ? "" : ioFile.NSupStockTransDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.BatchProcessFlg, string.IsNullOrEmpty(ioFile.BatchProcessFlg) ? "" : ioFile.BatchProcessFlg.Trim()));        
                v.InsertIntoColuList.Add((v.DelivSlipDataFlg, string.IsNullOrEmpty(ioFile.DelivSlipDataFlg) ? "" : ioFile.DelivSlipDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnRemarks, string.IsNullOrEmpty(ioFile.ProductReturnRemarks) ? "" : ioFile.ProductReturnRemarks.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：5】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileCusSupTrans(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "5")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "5"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, "060"));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));             
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：F】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileExchange(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "F")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "F"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, "")); // 振替などは単価区分不用、空値
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.OutDataCate, "F"));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));              
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：S】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="processMstStockCate">「工程管理マスタ」の「在庫区分」("Z"：在庫マスタ/素材在庫マスタを更新する；　"Z"以外：在庫マスタ/素材在庫マスタを更新しない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileProduction(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null, string processMstStockCate = "Z")
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "S")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "S"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));            
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.BatchProcessFlg, "S"));
                v.InsertIntoColuList.Add((v.OvertimeFlg, string.IsNullOrEmpty(ioFile.OvertimeFlg) ? "" : ioFile.OvertimeFlg.Trim()));
                v.InsertIntoColuList.Add((v.WorkTime, ioFile.WorkTime.ToString()));
                v.InsertIntoColuList.Add((v.SetupTime, ioFile.SetupTime.ToString()));
                v.InsertIntoColuList.Add((v.Worker, string.IsNullOrEmpty(ioFile.Worker) ? "" : ioFile.Worker.Trim()));
                v.InsertIntoColuList.Add((v.ProcessCode, string.IsNullOrEmpty(ioFile.ProcessCode) ? "" : ioFile.ProcessCode.Trim()));
                v.InsertIntoColuList.Add((v.MachineCode, string.IsNullOrEmpty(ioFile.MachineCode) ? "" : ioFile.MachineCode.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                v.InsertIntoColuList.Add((v.DataCateDetail, string.IsNullOrEmpty(ioFile.DataCateDetail) ? "" : ioFile.DataCateDetail.Trim()));
                v.InsertIntoColuList.Add((v.DefectCate, string.IsNullOrEmpty(ioFile.DefectCate) ? "" : ioFile.DefectCate.Trim()));
                v.InsertIntoColuList.Add((v.DefectContent, string.IsNullOrEmpty(ioFile.DefectContent) ? "" : ioFile.DefectContent.Trim()));
                v.InsertIntoColuList.Add((v.Work, string.IsNullOrEmpty(ioFile.Work) ? "" : ioFile.Work.Trim()));
                v.InsertIntoColuList.Add((v.SetupID, string.IsNullOrEmpty(ioFile.SetupID) ? "" : ioFile.SetupID.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                // 「工程管理マスタ」の「在庫区分」が「Z」の場合のみ、在庫マスタ/素材在庫マスタを更新する
                if (processMstStockCate != "Z")
                {
                    return (true, "");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：7】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileSupStockTrans(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "7")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "7"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.BatchProcessFlg, "S"));
                v.InsertIntoColuList.Add((v.OvertimeFlg, string.IsNullOrEmpty(ioFile.OvertimeFlg) ? "" : ioFile.OvertimeFlg.Trim()));
                v.InsertIntoColuList.Add((v.WorkTime, ioFile.WorkTime.ToString()));
                v.InsertIntoColuList.Add((v.SetupTime, ioFile.SetupTime.ToString()));
                v.InsertIntoColuList.Add((v.Worker, string.IsNullOrEmpty(ioFile.Worker) ? "" : ioFile.Worker.Trim()));
                v.InsertIntoColuList.Add((v.ProcessCode, string.IsNullOrEmpty(ioFile.ProcessCode) ? "" : ioFile.ProcessCode.Trim()));
                v.InsertIntoColuList.Add((v.MachineCode, string.IsNullOrEmpty(ioFile.MachineCode) ? "" : ioFile.MachineCode.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstIn(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.InNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }


        /// <summary>
        /// 入出庫ファイル（入庫）　【データ区分：K】
        /// 購入品。在庫マスタは更新しない
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFilePurchaseItem(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                //var errorCheck = ErrorCheck(ioFile);
                //if (errorCheck.IsOk == false)
                //{
                //    return (false, errorCheck.Msg);
                //}

                // データ区分
                if (ioFile.DataCate != "K")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.InNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "K"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                //v.InsertIntoColuList.Add((v.DelivCode, string.IsNullOrEmpty(ioFile.DelivCode) ? "" : ioFile.DelivCode.Trim()));
                v.InsertIntoColuList.Add((v.PartsName, string.IsNullOrEmpty(ioFile.PartsName) ? "" : ioFile.PartsName));
                v.InsertIntoColuList.Add((v.PartsSize, string.IsNullOrEmpty(ioFile.PartsSize) ? "" : ioFile.PartsSize));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode2, string.IsNullOrEmpty(ioFile.DoCode2) ? "" : ioFile.DoCode2.Trim()));
                v.InsertIntoColuList.Add((v.AccountCode, string.IsNullOrEmpty(ioFile.AccountCode) ? "" : ioFile.AccountCode.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));

                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（出庫）　【データ区分：4】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileOut(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "4")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.OutNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "4"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, ""));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.OutNum, ioFile.OutNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.OutDataCate, string.IsNullOrEmpty(ioFile.OutDataCate) ? "" : ioFile.OutDataCate.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.ScrapEquipment, string.IsNullOrEmpty(ioFile.ScrapEquipment) ? "" : ioFile.ScrapEquipment.Trim()));
                v.InsertIntoColuList.Add((v.ScrapContent, string.IsNullOrEmpty(ioFile.ScrapContent) ? "" : ioFile.ScrapContent.Trim()));
                v.InsertIntoColuList.Add((v.Worker, string.IsNullOrEmpty(ioFile.Worker) ? "" : ioFile.Worker.Trim()));
                v.InsertIntoColuList.Add((v.WorkerName, string.IsNullOrEmpty(ioFile.WorkerName) ? "" : ioFile.WorkerName.Trim()));
                v.InsertIntoColuList.Add((v.DefectReason, string.IsNullOrEmpty(ioFile.DefectReason) ? "" : ioFile.DefectReason.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                v.InsertIntoColuList.Add((v.DataCateDetail, string.IsNullOrEmpty(ioFile.DataCateDetail) ? "" : ioFile.DataCateDetail.Trim()));
                v.InsertIntoColuList.Add((v.DefectCate, string.IsNullOrEmpty(ioFile.DefectCate) ? "" : ioFile.DefectCate.Trim()));
                v.InsertIntoColuList.Add((v.DefectContent, string.IsNullOrEmpty(ioFile.DefectContent) ? "" : ioFile.DefectContent.Trim()));
                v.InsertIntoColuList.Add((v.MachineCode, string.IsNullOrEmpty(ioFile.MachineCode) ? "" : ioFile.MachineCode.Trim()));
                v.InsertIntoColuList.Add((v.Machine, string.IsNullOrEmpty(ioFile.Machine) ? "" : ioFile.Machine.Trim()));
                v.InsertIntoColuList.Add((v.Work, string.IsNullOrEmpty(ioFile.Work) ? "" : ioFile.Work.Trim()));
                v.InsertIntoColuList.Add((v.SetupID, string.IsNullOrEmpty(ioFile.SetupID) ? "" : ioFile.SetupID.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                        if (result.IsOk == false)
                        {
                            return (false, result.Msg);
                        }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（出庫）　【データ区分：6】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileNSupStockTrans(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "6")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                decimal num = 0;
                if (ioFile.OutNum.ToString() != "0")
                {
                    num = decimal.Parse(ioFile.OutNum.ToString());
                }
                else 
                {
                    num = decimal.Parse(ioFile.InNum.ToString());
                }
                var calPrice = CalPrice(num, decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "6"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, "083"));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.InNum, ioFile.InNum.ToString()));
                v.InsertIntoColuList.Add((v.OutNum, ioFile.OutNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.InsideTransDataFlg, string.IsNullOrEmpty(ioFile.InsideTransDataFlg) ? "" : ioFile.InsideTransDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.NSupStockTransDataFlg, string.IsNullOrEmpty(ioFile.NSupStockTransDataFlg) ? "" : ioFile.NSupStockTransDataFlg.Trim()));           
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), num, calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), num, calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), num, calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), num, calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（出庫）　【データ区分：8】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileInsideSale(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "8")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.OutNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "8"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, ""));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.OutNum, ioFile.OutNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.OutDataCate, string.IsNullOrEmpty(ioFile.OutDataCate) ? "" : ioFile.OutDataCate.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                //v.InsertIntoColuList.Add((v.PerformanceCode, string.IsNullOrEmpty(ioFile.PerformanceCode) ? "" : ioFile.PerformanceCode.Trim()));
                v.InsertIntoColuList.Add((v.InsideTransDataFlg, string.IsNullOrEmpty(ioFile.InsideTransDataFlg) ? "" : ioFile.InsideTransDataFlg.Trim()));
                //v.InsertIntoColuList.Add((v.ResponsibilityCate, string.IsNullOrEmpty(ioFile.ResponsibilityCate) ? "" : ioFile.ResponsibilityCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnRemarks, string.IsNullOrEmpty(ioFile.ProductReturnRemarks) ? "" : ioFile.ProductReturnRemarks.Trim()));         
                //v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                if (ioFile.SeqDelivSlipIssueF != null) { v.InsertIntoColuList.Add((v.SeqDelivSlipIssueF, ioFile.SeqDelivSlipIssueF.ToString())); }
                v.InsertIntoColuList.Add((v.GoodsTagCode, string.IsNullOrEmpty(ioFile.GoodsTagCode) ? "" : ioFile.GoodsTagCode.Trim()));
                //v.InsertIntoColuList.Add((v.WorkInstructionCode, string.IsNullOrEmpty(ioFile.WorkInstructionCode) ? "" : ioFile.WorkInstructionCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 入出庫ファイル（出庫）　【データ区分：U】
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ioFile">入出庫ファイルデータ</param>
        /// <param name="isEOM">(true：月末　false：月末ではない)</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public (bool IsOk, string Msg) InsertIOFileOutsideSale(SqlConnection con, IOFileEditor ioFile, bool isEOM, SqlTransaction tran = null)
        {
            try
            {
                // エラーチェック
                var errorCheck = ErrorCheck(ioFile);
                if (errorCheck.IsOk == false)
                {
                    return (false, errorCheck.Msg);
                }

                // データ区分
                if (ioFile.DataCate != "U")
                {
                    return (false, "データ区分が間違っています。入出庫ファイルは更新できません");
                }

                // 金額
                var calPrice = CalPrice(decimal.Parse(ioFile.OutNum.ToString()), decimal.Parse(ioFile.UnitPrice.ToString()));
                if (calPrice.IsOk == false)
                {
                    return (false, calPrice.Msg);
                }

                var v = new SansoBase.IOFile();
                // 固定値
                v.InsertIntoColuList.Add((v.DataCate, "U"));
                v.InsertIntoColuList.Add((v.Password, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateStaffCode, LoginInfo.Instance.UserNo));
                v.InsertIntoColuList.Add((v.CreateID, LoginInfo.Instance.UserId));
                v.InsertIntoColuList.Add((v.MachineName, LoginInfo.Instance.MachineCode));
                v.InsertIntoColuList.Add((v.Price, calPrice.Price.ToString()));
                v.InsertIntoColuList.Add((v.AccountCode, "083"));

                // 各画面で、下記の値を設定する
                v.InsertIntoColuList.Add((v.PartsCode, string.IsNullOrEmpty(ioFile.PartsCode) ? "" : ioFile.PartsCode.TrimEnd()));
                v.InsertIntoColuList.Add((v.SupCode, string.IsNullOrEmpty(ioFile.SupCode) ? "" : ioFile.SupCode.Trim()));
                v.InsertIntoColuList.Add((v.JyuyoyosokuCode, string.IsNullOrEmpty(ioFile.JyuyoyosokuCode) ? "" : ioFile.JyuyoyosokuCode.Trim()));
                v.InsertIntoColuList.Add((v.OutNum, ioFile.OutNum.ToString()));
                v.InsertIntoColuList.Add((v.UnitPriceCate, string.IsNullOrEmpty(ioFile.UnitPriceCate) ? "" : ioFile.UnitPriceCate.Trim()));
                v.InsertIntoColuList.Add((v.UnitPrice, ioFile.UnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.ProcessUnitPrice, (ioFile.ProcessUnitPrice == null) ? "0" : ioFile.ProcessUnitPrice.ToString()));
                v.InsertIntoColuList.Add((v.AcceptDate, ioFile.AcceptDate.ToString()));
                v.InsertIntoColuList.Add((v.GroupCode, string.IsNullOrEmpty(ioFile.GroupCode) ? "" : ioFile.GroupCode.Trim()));
                v.InsertIntoColuList.Add((v.DoCode, string.IsNullOrEmpty(ioFile.DoCode) ? "" : ioFile.DoCode.Trim()));
                v.InsertIntoColuList.Add((v.OutDataCate, string.IsNullOrEmpty(ioFile.OutDataCate) ? "" : ioFile.OutDataCate.Trim()));
                v.InsertIntoColuList.Add((v.StockCate, string.IsNullOrEmpty(ioFile.StockCate) ? "" : ioFile.StockCate.Trim()));
                v.InsertIntoColuList.Add((v.NSupCate, string.IsNullOrEmpty(ioFile.NSupCate) ? "" : ioFile.NSupCate.Trim()));
                v.InsertIntoColuList.Add((v.Remarks, string.IsNullOrEmpty(ioFile.Remarks) ? "" : ioFile.Remarks.Trim()));
                v.InsertIntoColuList.Add((v.Sakuban, string.IsNullOrEmpty(ioFile.Sakuban) ? "" : ioFile.Sakuban.Trim()));
                v.InsertIntoColuList.Add((v.CreateDate, ioFile.CreateDate.ToString()));
                v.InsertIntoColuList.Add((v.PerformanceCode, string.IsNullOrEmpty(ioFile.PerformanceCode) ? "" : ioFile.PerformanceCode.Trim()));
                v.InsertIntoColuList.Add((v.InsideTransDataFlg, string.IsNullOrEmpty(ioFile.InsideTransDataFlg) ? "" : ioFile.InsideTransDataFlg.Trim()));
                v.InsertIntoColuList.Add((v.ResponsibilityCate, string.IsNullOrEmpty(ioFile.ResponsibilityCate) ? "" : ioFile.ResponsibilityCate.Trim()));
                v.InsertIntoColuList.Add((v.ProductReturnRemarks, string.IsNullOrEmpty(ioFile.ProductReturnRemarks) ? "" : ioFile.ProductReturnRemarks.Trim()));
                v.InsertIntoColuList.Add((v.ReturnCate, string.IsNullOrEmpty(ioFile.ReturnCate) ? "0" : ioFile.ReturnCate.Trim()));
                if (ioFile.SeqDelivSlipIssueF != null) { v.InsertIntoColuList.Add((v.SeqDelivSlipIssueF, ioFile.SeqDelivSlipIssueF.ToString())); }                
                v.InsertIntoColuList.Add((v.GoodsTagCode, string.IsNullOrEmpty(ioFile.GoodsTagCode) ? "" : ioFile.GoodsTagCode.Trim()));
                v.InsertIntoColuList.Add((v.WorkInstructionCode, string.IsNullOrEmpty(ioFile.WorkInstructionCode) ? "" : ioFile.WorkInstructionCode.Trim()));
                v.InsertIntoColuList.Add((v.PoCode, string.IsNullOrEmpty(ioFile.PoCode) ? "" : ioFile.PoCode.Trim()));
                v.InsertIntoColuList.Add((v.PartsName, string.IsNullOrEmpty(ioFile.PartsName) ? "" : ioFile.PartsName.Trim()));
                v.InsertIntoColuList.Add((v.RepProductName, string.IsNullOrEmpty(ioFile.RepProductName) ? "" : ioFile.RepProductName.Trim()));
                var af = CommonAF.ExecutInsertSQL(v, tran, con);
                if (af.IsOk == false)
                {
                    return (false, "入出庫ファイル更新時にエラーが発生しました");
                }

                // 「部品コード」が「999999999」の場合、在庫マスタ/素材在庫マスタを更新しない
                if (ioFile.PartsCode == "999999999")
                {
                    return (true, "");
                }

                var stockMstEditor = new StockMstEditor();
                if (isEOM && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut_E(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "Z")
                {
                    var result = stockMstEditor.UpdateStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else if (isEOM == false && ioFile.StockCate == "")
                {
                    var result = stockMstEditor.UpdateMaterialStockMstOut(con, ioFile.GroupCode.Trim(), ioFile.PartsCode.TrimEnd(), decimal.Parse(ioFile.OutNum.ToString()), calPrice.Price, ioFile.AcceptDate, tran);
                    if (result.IsOk == false)
                    {
                        return (false, result.Msg);
                    }
                }
                else
                {
                    // 何もしない
                }
            }
            catch
            {
                return (false, "入出庫ファイル更新時にエラーが発生しました");
            }
            return (true, "");
        }
    }
}