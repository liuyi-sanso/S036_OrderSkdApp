using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using C1.Win.C1Input;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 仕入先マスタメンテナンス
    /// </summary>
    public class F602_SupMstMaintAF
    {

        /// <summary>
        /// 仕入先マスタ　登録更新
        /// </summary>
        /// <param name="controlListII">controlListII</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool UpdateSupMST(List<ControlParam> controlListII)
        {
            var createDate = DateTime.Now;

            var supCode = controlListII.SGetText("supCodeC1TextBox");
            var supName1 = controlListII.SGetText("supName1C1TextBox");
            var supName2 = controlListII.SGetText("supName2C1TextBox");
            var postalCode = controlListII.SGetText("postalCodeC1TextBox");
            var address1 = controlListII.SGetText("address1C1TextBox");
            var address2 = controlListII.SGetText("address2C1TextBox");
            var phoneNum = controlListII.SGetText("phoneNumC1TextBox");
            var fax = controlListII.SGetText("faxC1TextBox");
            var mail = controlListII.SGetText("mailC1TextBox");
            var supStaffName = controlListII.SGetText("supStaffNameC1TextBox");
            var sansoStaffName = controlListII.SGetText("sansoStaffNameC1TextBox");
            var supCate = controlListII.SGetText("supCateC1ComboBox");
            var delivSlipIssueCate = ((C1ComboBox)(controlListII
                                .Where(v => v.Control.Name == "delivSlipIssueCateC1ComboBox").ToList()[0].Control)).SGetText(1);

            // MARGE処理
            string sql =
                "MERGE INTO 製造調達.dbo.仕入先マスタ AS A " +
                "USING(SELECT '" + supCode + "' AS 仕入先コード) AS B " +
                "on (A.仕入先コード = B.仕入先コード) " +
                "WHEN MATCHED " +
                "THEN " +
                "UPDATE SET " +
                "仕入先名１ = " + (supName1 == "" ? "NULL, " : $"'{supName1}', ") +
                "仕入先名２ = " + (supName2 == "" ? "NULL, " : $"'{supName2}', ") +
                "郵便番号 = " + (postalCode == "" ? "NULL, " : $"'{postalCode}', ") +
                "住所１ = " + (address1 == "" ? "NULL, " : $"'{address1}', ") +
                "住所２ = " + (address2 == "" ? "NULL, " : $"'{address2}', ") +
                "電話番号 = " + (phoneNum == "" ? "NULL, " : $"'{phoneNum}', ") +
                "ＦＡＸ番号 = " + (fax == "" ? "NULL, " : $"'{fax}', ") +
                "Ｅメールアドレス = " + (mail == "" ? "NULL, " : $"'{mail}', ") +
                "仕入先担当者名 = " + (supStaffName == "" ? "NULL, " : $"'{supStaffName}', ") +
                "三相担当者名 = " + (sansoStaffName == "" ? "NULL, " : $"'{sansoStaffName}', ") +
                "仕入先区分 = '" + supCate + "'," +
                "納品書発行区分 = " + (delivSlipIssueCate == "1" ? $"'{delivSlipIssueCate}', " : "NULL, ") +
                "変更日付 = '" + createDate + "', " +
                "変更者 = '" + LoginInfo.Instance.UserNo + "', " +
                "変更者ID = '" + LoginInfo.Instance.UserId + "' " +
                "WHEN NOT MATCHED " +
                "THEN " +
                "INSERT(仕入先コード,仕入先名１,仕入先名２,郵便番号,住所１,住所２,電話番号,ＦＡＸ番号," +
                "Ｅメールアドレス,仕入先担当者名,三相担当者名,仕入先区分,納品書発行区分" +
                ", 伝票番号, 伝票番号1, 伝票番号2, 伝票番号3, 伝票番号4, 伝票番号5" +
                ", 注文番号連番, 注文番号連番1, 注文番号連番2, 注文番号連番3, 注文番号連番4, 注文番号連番5" +
                ",作成日付,作成者,作成者ID) " +
                "VALUES " +
                "(" + (supCode == "" ? "NULL, " : $"'{supCode}', ") +
                (supName1 == "" ? "NULL, " : $"'{supName1}', ") +
                (supName2 == "" ? "NULL, " : $"'{supName2}', ") +
                (postalCode == "" ? "NULL, " : $"'{postalCode}', ") +
                (address1 == "" ? "NULL, " : $"'{address1}', ") +
                (address2 == "" ? "NULL, " : $"'{address2}', ") +
                (phoneNum == "" ? "NULL, " : $"'{phoneNum}', ") +
                (fax == "" ? "NULL, " : $"'{fax}', ") +
                (mail == "" ? "NULL, " : $"'{mail}', ") +
                (supStaffName == "" ? "NULL, " : $"'{supStaffName}', ") +
                (sansoStaffName == "" ? "NULL, " : $"'{sansoStaffName}', ") +
                "'" + supCate + "', " +
                (delivSlipIssueCate == "1" ? $"'{delivSlipIssueCate}', " : "NULL, ") +
                "0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, " +
                "'" + createDate + "', " +
                "'" + LoginInfo.Instance.UserNo + "'," +
                "'" + LoginInfo.Instance.UserId + "'" +
                "); ";

            var result = CommonAF.ExecutUpdateSQL(sql);
            if (result.IsOk == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 仕入先マスタ　削除
        /// </summary>
        /// <param name="controlListII">controlListII</param>
        /// <returns>True エラー無し、False エラー有り</returns>
        public bool DeleteSupMST(List<ControlParam> controlListII)
        {
            var supCode = controlListII.SGetText("supCodeC1TextBox");

            string sql = "DELETE FROM 製造調達.dbo.仕入先マスタ " +
            "WHERE " +
            "(仕入先コード = '" + supCode + "')  ";
            var result = CommonAF.ExecutDeleteSQL(sql);
            if (result.IsOk == false)
            {
                return false;
            }
            return true;
        }

    }
}
