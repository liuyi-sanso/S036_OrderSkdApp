using SansoBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using C1.Win.C1Input;
using C1.Win.Calendar;
using System.Threading.Tasks;

namespace S036_OrderSkdApp
{
    public class M_CateNameAF
    {
        /// <summary>
        /// 名称マスタ抽出（コンボボックス用）
        /// </summary>
        /// <returns></returns>
        public (bool IsOk, DataTable Table) GetComboCateName(string groupCode ,string cateCode)
        {
            string sql = " select " +
                "CateCode AS ID,CateName AS NAME " +
                "from SANSODB.dbo.M_CateName " +
                "where " +
                "GroupCode = '" + groupCode + "' and DivisionCode = '" + cateCode+ "' " +
                "order by DisplayOrder";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 名称マスタ抽出（コンボボックス用）
        /// </summary>
        /// <returns></returns>
        /// <remarks>SANSODB.dbo.M_CateNameから各区分とその名称を名称(NAME)、区分(ID)の順番で取得する</remarks>
        public (bool IsOk, DataTable Table) GetComboCateNameReverse(string groupCode, string cateCode)
        {
            string sql = " select " +
                "CateName AS NAME,CateCode AS ID " +
                "from SANSODB.dbo.M_CateName " +
                "where " +
                "GroupCode = '" + groupCode + "' and DivisionCode = '" + cateCode + "' " +
                "order by DisplayOrder";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table);
        }

        /// <summary>
        /// 名称マスタ抽出（NameからCodeを抽出）
        /// </summary>
        /// <returns></returns>
        public (bool IsOk, string CateCode) GetCateCode(string groupCode, string divisionCode, string cateName)
        {
            string sql = "select CateCode " +
                "from SANSODB.dbo.M_CateName where " +
                "GroupCode = '" + groupCode + "' and " +
                "DivisionCode = '" + divisionCode + "' and " +
                "CateName = '" + cateName+ "' ";
            var af = CommonAF.ExecutSelectSQL(sql);
            if (af.IsOk == false)
            {
                return (false, null);
            }
            return (true, af.Table.Rows[0].Field<string>("CateCode"));
        }
    }
}
