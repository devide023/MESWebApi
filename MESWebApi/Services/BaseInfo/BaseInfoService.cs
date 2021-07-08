using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Dapper;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using MESWebApi.Models.BaseInfo;
using System.Text;
using DapperExtensions;

namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 公共基础信息服务
    /// </summary>
    public class BaseInfoService
    {
        private ILog log;
        private string constr = "";
        public BaseInfoService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }
        /// <summary>
        /// 工厂
        /// </summary>
        /// <returns></returns>
        public IEnumerable<base_gcxx> FactoryList()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select gcdm, gcmc,gsxx,gsmc from base_gcxx");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Query<base_gcxx>(sql.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 人员信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<sec_users> PersonList(string key)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT comp_no,");
                sql.Append(" user_code,");
                sql.Append(" user_name,");
                sql.Append(" user_type,");
                sql.Append(" depart_no,");
                sql.Append(" gwxx,");
                sql.Append(" pass_word,");
                sql.Append(" bz,");
                sql.Append(" class_no,");
                sql.Append(" tsqx,");
                sql.Append(" scx,");
                sql.Append(" lx");
                sql.Append(" FROM sec_users where 1=1 ");
                if (!string.IsNullOrEmpty(key))
                {
                    sql.Append(" and (user_code like :key or user_name like :key or scx like :key or class_no like :key )");
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Query<sec_users>(sql.ToString(), new { key = "%" + key + "%" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 通机生产线
        /// </summary>
        /// <returns></returns>
        public IEnumerable<tj_base_scxxx> ScxList()
        {
            try
            {
                using (var db = new OracleBaseFixture(constr).DB)
                {
                    return db.GetList<tj_base_scxxx>().OrderBy(t=>t.scx);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 岗位站点
        /// </summary>
        /// <returns></returns>
        public IEnumerable<zxjc_gxzd> GWList()
        {
            try
            {
                using (var db = new OracleBaseFixture(constr).DB)
                {
                  return  db.GetList<zxjc_gxzd>();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 机型列表()
        /// </summary>
        /// <param name="key">查找字段（jx）</param>
        /// <returns>返回字段jx</returns>
        public IEnumerable<ztbm_new> GetJxList(string key)
        {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select distinct jx from ZTBM_NEW where lower(jx) like :key and rownum  <20");
                    return conn.Query<ztbm_new>(sql.ToString(), new { key = "%" + key.ToLower() + "%" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 机型状态
        /// </summary>
        /// <param name="jx"></param>
        /// <returns></returns>
        public IEnumerable<ztbm_new> GetZtList(string jx)
        {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select distinct cpbm as ztbm from ZTBM_NEW where lower(jx) like :key and rownum < 10 order by cpbm asc");
                    return conn.Query<ztbm_new>(sql.ToString(), new { key = "%" + jx.ToLower() + "%" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 物料信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<base_wlxx> GetWlList(string key) {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select wlbm,wlmc,wljc,wlz,wlsx from base_wlxx where gc='9100' ");
                    sql.Append(" and ( lower(wlbm) like :key or lower(wlmc) like :key ) ");
                    sql.Append(" and rownum < 10 order by wlbm asc");
                    return conn.Query<base_wlxx>(sql.ToString(), new { key = "%" + key.ToLower() + "%" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}