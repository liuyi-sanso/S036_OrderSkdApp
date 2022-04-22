using SansoBase;
using System;
using System.Data;
using System.Data.SqlClient;

namespace S036_OrderSkdApp
{
    class StockMstEditor
    {
        /// <summary>
        /// 在庫マスタ（入庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateStockMstIn​(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mst = new SansoBase.StockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        
                        var v = new SansoBase.StockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.InNum, ((dt.Rows[0].Field<decimal?>("入庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.InPrice, ((dt.Rows[0].Field<decimal?>("入庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dt.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 在庫マスタ（出庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateStockMstOut(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mst = new SansoBase.StockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.StockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.OutNum, ((dt.Rows[0].Field<decimal?>("出庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dt.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 月末在庫マスタ（入庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateStockMstIn​_E(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mstE = new SansoBase.StockMstE();
                mstE.SelectStr = "*";
                mstE.WhereColuList.Add((mstE.GroupCode, groupCode.Trim()));
                mstE.WhereColuList.Add((mstE.PartsCode, partsCode.TrimEnd()));
                var dtE = CommonAF.ExecutSelectSQL(mstE, tran, con).Table;
                if (dtE.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.StockMstE();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.InNum, ((dtE.Rows[0].Field<decimal?>("入庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.InPrice, ((dtE.Rows[0].Field<decimal?>("入庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dtE.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthPrice, ((dtE.Rows[0].Field<decimal?>("当残金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dtE.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dtE.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dtE.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "月末在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "月末在庫マスタが存在しません");
                }

                var mst = new SansoBase.StockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.StockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.PrevMonthStockNum, ((dt.Rows[0].Field<decimal?>("前残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.PrevMonthPrice, ((dt.Rows[0].Field<decimal?>("前残金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dt.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "月末在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 月末在庫マスタ（出庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateStockMstOut_E(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mstE = new SansoBase.StockMstE();
                mstE.SelectStr = "*";
                mstE.WhereColuList.Add((mstE.GroupCode, groupCode.Trim()));
                mstE.WhereColuList.Add((mstE.PartsCode, partsCode.TrimEnd()));
                var dtE = CommonAF.ExecutSelectSQL(mstE, tran, con).Table;
                if (dtE.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.StockMstE();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.OutNum, ((dtE.Rows[0].Field<decimal?>("出庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.OutPrice, ((dtE.Rows[0].Field<decimal?>("出庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dtE.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthPrice, ((dtE.Rows[0].Field<decimal?>("当残金額") ?? 0m) - (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dtE.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dtE.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dtE.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "月末在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "月末在庫マスタが存在しません");
                }

                var mst = new SansoBase.StockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.StockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.PrevMonthStockNum, ((dt.Rows[0].Field<decimal?>("前残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.PrevMonthPrice, ((dt.Rows[0].Field<decimal?>("前残金額") ?? 0m) - (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dt.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "月末在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 素材在庫マスタ（入庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateMaterialStockMstIn​(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mst = new SansoBase.MaterialStockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.InNum, ((dt.Rows[0].Field<decimal?>("入庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.InPrice, ((dt.Rows[0].Field<decimal?>("入庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dt.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "素材在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "素材在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 素材在庫マスタ（出庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateMaterialStockMstOut(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mst = new SansoBase.MaterialStockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.OutNum, ((dt.Rows[0].Field<decimal?>("出庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dt.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "素材在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "素材在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 月末素材在庫マスタ（入庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateMaterialStockMstIn​_E(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mstE = new SansoBase.MaterialStockMstE();
                mstE.SelectStr = "*";
                mstE.WhereColuList.Add((mstE.GroupCode, groupCode.Trim()));
                mstE.WhereColuList.Add((mstE.PartsCode, partsCode.TrimEnd()));
                var dtE = CommonAF.ExecutSelectSQL(mstE, tran, con).Table;
                if (dtE.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMstE();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.InNum, ((dtE.Rows[0].Field<decimal?>("入庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.InPrice, ((dtE.Rows[0].Field<decimal?>("入庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dtE.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthPrice, ((dtE.Rows[0].Field<decimal?>("当残金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dtE.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dtE.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dtE.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "月末素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "月末素材在庫マスタが存在しません");
                }

                var mst = new SansoBase.MaterialStockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.PrevMonthStockNum, ((dt.Rows[0].Field<decimal?>("前残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.PrevMonthPrice, ((dt.Rows[0].Field<decimal?>("前残金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastInDate, string.IsNullOrEmpty(dt.Rows[0]["最終入庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終入庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終入庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "素材在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "月末素材在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }

        /// <summary>
        /// 月末素材在庫マスタ（出庫）
        /// </summary>
        /// <param name="con"></param>
        /// <param name="groupCode">課別コード</param>
        /// <param name="partsCode">部品コード</param>
        /// <param name="num">数量</param>
        /// <param name="price">金額</param>
        /// <param name="acceptDate">受払年月日</param>
        /// <param name="tran"></param>
        /// <returns>IsOk(true：更新成功　false：更新失敗)</returns>
        /// <returns>Msg(エラーメッセージ)</returns>
        public (bool IsOk, string Msg) UpdateMaterialStockMstOut_E(SqlConnection con, string groupCode, string partsCode, decimal num, decimal price, DateTime acceptDate, SqlTransaction tran = null)
        {
            try
            {
                var mstE = new SansoBase.MaterialStockMstE();
                mstE.SelectStr = "*";
                mstE.WhereColuList.Add((mstE.GroupCode, groupCode.Trim()));
                mstE.WhereColuList.Add((mstE.PartsCode, partsCode.TrimEnd()));
                var dtE = CommonAF.ExecutSelectSQL(mstE, tran, con).Table;
                if (dtE.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMstE();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.OutNum, ((dtE.Rows[0].Field<decimal?>("出庫数量") ?? 0m) + (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.OutPrice, ((dtE.Rows[0].Field<decimal?>("出庫金額") ?? 0m) + (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dtE.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthPrice, ((dtE.Rows[0].Field<decimal?>("当残金額") ?? 0m) - (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dtE.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dtE.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dtE.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "月末素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "月末素材在庫マスタが存在しません");
                }

                var mst = new SansoBase.MaterialStockMst();
                mst.SelectStr = "*";
                mst.WhereColuList.Add((mst.GroupCode, groupCode.Trim()));
                mst.WhereColuList.Add((mst.PartsCode, partsCode.TrimEnd()));
                var dt = CommonAF.ExecutSelectSQL(mst, tran, con).Table;
                if (dt.Rows.Count > 0)
                {
                    using (var command = con.CreateCommand())
                    {
                        var v = new SansoBase.MaterialStockMst();
                        v.WhereColuList.Add((v.GroupCode, groupCode.Trim()));
                        v.WhereColuList.Add((v.PartsCode, partsCode.TrimEnd()));
                        v.UpdateSetColuList.Add((v.PrevMonthStockNum, ((dt.Rows[0].Field<decimal?>("前残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.PrevMonthPrice, ((dt.Rows[0].Field<decimal?>("前残金額") ?? 0m) - (string.IsNullOrEmpty(price.ToString()) ? 0m : price)).ToString()));
                        v.UpdateSetColuList.Add((v.ThisMonthStockNum, ((dt.Rows[0].Field<decimal?>("当残数量") ?? 0m) - (string.IsNullOrEmpty(num.ToString()) ? 0m : num)).ToString()));
                        v.UpdateSetColuList.Add((v.LastOutDate, string.IsNullOrEmpty(dt.Rows[0]["最終出庫日"].ToString()) ? acceptDate.ToString() : ((dt.Rows[0].Field<DateTime>("最終出庫日") <= acceptDate) ? acceptDate.ToString() : (dt.Rows[0]["最終出庫日"].ToString()))));
                        var af = CommonAF.ExecutUpdateSQL(v, tran, con);
                        if (af.IsOk == false)
                        {
                            return (false, "素材在庫マスタ更新時にエラーが発生しました");
                        }
                    }
                }
                else
                {
                    return (false, "素材在庫マスタが存在しません");
                }
            }
            catch
            {
                return (false, "月末素材在庫マスタ更新時にエラーが発生しました");
            }
            return (true, "");
        }
    }
}