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
        /// <summary>
        /// 技通分配列表
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="resultcount"></param>
        /// <returns></returns>
        public IEnumerable<zxjc_t_jstcfp> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append(" select ta.jtid, ta.gcdm, ta.scx, ta.gwh,(select work_name from ZXJC_GXZD where work_no=ta.gwh) as gwmc,ta.jx_no, ta.status_no, ta.bz, ta.lrr1, ta.lrsj1, ta.lrr2, ta.lrsj2 ");
                sql.Append(" from ZXJC_T_JSTCFP ta where 1 = 1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (ta.jtid like :key or ta.jx_no like :key or ta.status_no like :key) ");
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
        /// <summary>
        /// 根据技通查询分配详情
        /// </summary>
        /// <param name="jtid"></param>
        /// <returns></returns>
        public IEnumerable<zxjc_t_jstcfp> GetDistributeListById(string jtid)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select ta.jtid, ta.gcdm, ta.scx, ta.gwh,(select work_name from ZXJC_GXZD where work_no=ta.gwh) as gwmc,ta.jx_no, ta.status_no, ta.bz, ta.lrr1, ta.lrsj1, ta.lrr2, ta.lrsj2 ");
                sql.Append(" from ZXJC_T_JSTCFP ta where ta.jtid=:jtid ");
                return Conn.Query<zxjc_t_jstcfp>(sql.ToString(), new { jtid = jtid });
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public override int Add(List<zxjc_t_jstcfp> entitys)
        {
            try
            {
                string jtid = entitys.FirstOrDefault().jtid;
                int ret = base.Add(entitys);
                if (ret > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sys_user user = CacheManager.Instance().Current_User;
                    sql.Append("update zxjc_t_jstc set fp_flg='Y',fp_sj=sysdate,fpr=:name where jtid =:jtid");
                    Conn.Execute(sql.ToString(), new { name = user.name, jtid = jtid });
                }
                return ret;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 更新分配人信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Modify(zxjc_t_jstcfp entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update zxjc_t_jstcfp");
                sql.Append(" set bz = :bz, lrr1 = :lrr1, lrsj1 = :lrsj1, lrr2 = :lrr2, lrsj2 = :lrsj2");
                sql.Append(" where  jtid = :jtid");
                sql.Append(" and    gcdm = :gcdm");
                sql.Append(" and    scx = :scx");
                sql.Append(" and    jx_no = :jx_no");
                sql.Append(" and    status_no = :status_no");
                return Conn.Execute(sql.ToString(), entity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public int DelDistribute(List<zxjc_t_jstcfp> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from ZXJC_T_JSTCFP ");
                sql.Append(" where jtid =:jtid and gcdm = :gcdm and scx =:scx and gwh=:gwh and jx_no =:jx_no and status_no = :status_no ");
                return Conn.Execute(sql.ToString(), entitys.ToArray());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}