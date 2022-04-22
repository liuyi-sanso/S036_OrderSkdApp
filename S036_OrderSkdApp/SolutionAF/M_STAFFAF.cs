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

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 三相メイン.dbo.M_STAFFに関する処理をまとめる
    /// </summary>
    public class M_STAFFAF
    {
        /// <summary>
        /// 品証　取得
        /// </summary>
        /// <returns>標準時間測定などの品証コンボボックス用のデータを取得する</returns>
        public (bool IsOk, DataTable Table) GetSTAFFMstCertifier()
        {
            string sql = "SELECT " +
　　　　　　　　　　　　 "StaffCode, " +
　　　　　　　　　　　　 "StaffName " +
　　　　　　　　　　　　 "FROM " +
                         "三相メイン.dbo.M_STAFF " +
　　　　　　　　　　　　 "WHERE " +
　　　　　　　　　　　　 "(CompanyCode = '0000') AND " +
　　　　　　　　　　　　 "(GroupCode IN ('3010', '3020')) " +
　　　　　　　　　　　　 "ORDER BY StaffCode";

            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }


    }
}
