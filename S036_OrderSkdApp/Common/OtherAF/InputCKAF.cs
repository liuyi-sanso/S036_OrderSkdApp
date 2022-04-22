using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SansoBase;

namespace S036_OrderSkdApp.Common.OtherAF
{
    public static class InputCKAF
    {

        /// <summary>
        /// 機種コード用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr,string msgCate) CKProductCode(C1.Win.C1Input.C1TextBox prodCode)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(prodCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message , msg.cate);
            }

            // 機種マスタチェック
            var result = SansoBase.Common.SelectDBAF.GetSansoMainProdMst(prodCode.Text);
            if (result.isOk == false)
            {
                return (false, "処理時にエラーが発生しました","Err");
            }
            else if (result.table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0002", prodCode.Label.Text, "機種マスタ");
                return (false, msg.message, msg.cate);
            }

            return (true, "","");
        }

        /// <summary>
        /// 機種名用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKProductName(C1.Win.C1Input.C1TextBox prodName)
        {
            // 機種マスタチェック
            var result = SansoBase.Common.SelectDBAF.GetSansoMainProdMst(prodName.Text);
            if (result.isOk == false)
            {
                return (false, "処理時にエラーが発生しました", "Err");
            }
            else if (result.table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0002", prodName.Label.Text, "機種マスタ");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 課別コード用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKGroupCode(C1.Win.C1Input.C1ComboBox GroupCode, C1.Win.C1Input.C1TextBox GroupName)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(GroupCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            // ComboBoxリスト存在チェック
            if (ControlAF.CheckComboBoxList(GroupCode, GroupName) == false)
            {

                var msg = Message.GetMessage("W0013", GroupCode.Label.Text);
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// ライン用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKLineCode(C1.Win.C1Input.C1ComboBox LineCode, C1.Win.C1Input.C1TextBox LineName)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(LineCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            // ComboBoxリスト存在チェック
            if (ControlAF.CheckComboBoxList(LineCode, LineName) == false)
            {

                var msg = Message.GetMessage("W0013", LineCode.Label.Text);
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 並び替え用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKSort(C1.Win.C1Input.C1ComboBox SortCode, C1.Win.C1Input.C1TextBox SortName)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(SortCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            // ComboBoxリスト存在チェック
            if (ControlAF.CheckComboBoxList(SortCode, SortName) == false)
            {

                var msg = Message.GetMessage("W0013", SortCode.Label.Text);
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 機種コードと機種名　整合性チェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CheckProduct(string productCode, string productName)
        {
            if (SelectDBAF.CheckProduct(productCode, productName).Table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0021", "機種コードと機種名");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// ラインコードとライン名　整合性チェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CheckGroupLine(string  groupCode, string line)
        {
            if (SelectDBAF.CheckGroupLine(groupCode, line).Table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0021", "部門とライン");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// エラーチェック　部門コード（三相メイン.dbo.部門マスタ）
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKGroupCode(C1.Win.C1Input.C1TextBox groupCode)
        {
            // 部門マスタ  チェック
            var result = SansoBase.Common.SelectDBAF.GetSansoMainGroupMst(groupCode.Text.Trim());
            if (result.isOk == false)
            {
                return (false, "処理時にエラーが発生しました", "Err");
            }
            else if (result.table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0002", groupCode.Label.Text, "部門マスタ");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 依頼番号用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKRequestCode(C1.Win.C1Input.C1TextBox requestCode)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(requestCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            // 機種組替ファイルチェック
            var result = SelectDBAF.GetProductChangeFile(requestCode.Text);
            if (result.IsOk == false)
            {
                return (false, "処理時にエラーが発生しました", "Err");
            }
            else if (result.Table.Rows.Count < 1)
            {
                var msg = Message.GetMessage("W0002", requestCode.Label.Text, "機種組替ファイル");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 工程番号用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKProcessCode(C1.Win.C1Input.C1TextBox processCode)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(processCode.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }

        /// <summary>
        /// 作番用のエラーチェック
        /// </summary>
        public static (bool isOK, string msgStr, string msgCate) CKSakuban(C1.Win.C1Input.C1TextBox sakuban)
        {
            // 使用禁止文字のチェック
            if (Check.HasBanChar(sakuban.Text).Result == false)
            {
                var msg = Message.GetMessage("W0018");
                return (false, msg.message, msg.cate);
            }

            return (true, "", "");
        }
    }
}
