using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using MESWebApi.Util;
using System.Text;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using log4net;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services.BaseInfo
{
    public class DistributeService : DBOperImp<zxjc_t_jstcfp>, IComposeQuery<zxjc_t_jstcfp, sys_page>
    {
        ILog log;
        public DistributeService(string connstr = "tjmes") : base(connstr)
        {
            log = LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// 查询未分配的技通
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="resultcount"></param>
        /// <returns></returns>
        public IEnumerable<zxjc_t_jstc> GetUnDistributeJT(DisJTParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT jtid,jcbh,jcmc,jcms,wjlj,jwdx,scry,scpc,scsj,yxqx1,yxqx2,gcdm,fp_flg,fp_sj,fpr,wjfl,scx");
                sql.Append(" FROM zxjc_t_jstc where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword)) {
                    sql.Append(" and (jcbh like :key or jcmc like :key or jcms like :key )");
                    p.Add("key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.sffp))
                {
                    sql.Append(" and fp_flg = :sffp ");
                    p.Add(":sffp", parm.sffp, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                var q = Conn.Query<zxjc_t_jstc>(sql.ToString(), p)
                    .OrderBy(t => t.jcbh)
                    .ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public IEnumerable<zxjc_t_jstcfp> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append(" select jtid, gcdm, scx, gwh, jx_no, status_no, bz, lrr1, lrsj1, lrr2, lrsj2 ");
                sql.Append(" from ZXJC_T_JSTCFP where 1 = 1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (jx_no like :key or status_no like :key) ");
                    p.Add(":key", "%"+parm.keyword+"%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                var q = Conn.Query<zxjc_t_jstcfp>(sql.ToString(), p)
                .OrderBy(t => t.jtid)
                .ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}