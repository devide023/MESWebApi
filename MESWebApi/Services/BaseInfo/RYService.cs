using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.InterFaces;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using System.Text;
using Webdiyer.WebControls.Mvc;
using DapperExtensions;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using log4net;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class RYService : IComposeQuery<sec_users,sec_userparm>
    {
        private ILog log;
        private string constr = "";
        public RYService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }
        public IEnumerable<sec_users> FindUserByName(string key)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select comp_no,user_code,user_name,user_type,depart_no,gwxx,bz,class_no,tsqx,scx,lx");
                sql.Append(" from sec_users where (user_code like :key or user_name like :key)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   return conn.Query<sec_users>(sql.ToString(), new { code = "%"+key+"%" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<sec_users> Search(sec_userparm parm, out int resultcount)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select comp_no,");
                sql.Append(" user_code,");
                sql.Append(" user_name,");
                sql.Append(" user_type,");
                sql.Append(" depart_no,");
                sql.Append(" gwxx,");
                sql.Append(" pass_word,");
                sql.Append(" bz,");
                sql.Append(" version,");
                sql.Append(" version_b,");
                sql.Append(" mac,");
                sql.Append(" ip,");
                sql.Append(" class_no,");
                sql.Append(" tsqx,");
                sql.Append(" scx,");
                sql.Append(" work_no,");
                sql.Append(" lx from sec_users where (user_code like :code or user_name like :code ) ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                if(!string.IsNullOrEmpty(parm.keyword))
                {
                    p.Add(":code", "%"+parm.keyword+"%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   var q = conn.Query<sec_users>(sql.ToString(), p)
                        .OrderBy(t => t.user_code)
                        .ToPagedList(parm.pageindex, parm.pagesize);
                    resultcount = q.TotalItemCount;
                    return q;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}