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
                    sql.Append(" and (user_code like :key or user_name like :key or scx like :key or class_no like :key or class_no like :key )");
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

    }
}