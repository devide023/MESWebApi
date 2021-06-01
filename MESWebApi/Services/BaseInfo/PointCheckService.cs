using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using System.Text;
using Dapper;
using Dapper.Oracle;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 点检
    /// </summary>
    public class PointCheckService : IDBOper<zxjc_djgw>, IComposeQuery<zxjc_djgw, CheckPointQueryParm>
    {
        ILog log;
        string constr = String.Empty;
        public PointCheckService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_djgw Add(zxjc_djgw entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_djgw ");
                sql.Append("(gcdm, scx, gwh, jx_no, status_no, djno, djxx, scbz, lrr, lrsj, djlx)");
                sql.Append(" values ");
                sql.Append(" (:gcdm,:scx,:gwh,:jx_no,:status_no,:djno,:djxx,:scbz,:lrr, sysdate,:djlx)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int ret = conn.Execute(sql.ToString(), entity);
                    if (ret > 0)
                    {
                        return new zxjc_djgw();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public int Add(List<zxjc_djgw> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_djgw ");
                sql.Append("(gcdm, scx, gwh, jx_no, status_no, djno, djxx, scbz, lrr, lrsj, djlx)");
                sql.Append(" values ");
                sql.Append(" (:gcdm,:scx,:gwh,:jx_no,:status_no,:djno,:djxx,:scbz,:lrr, sysdate,:djlx)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int ret = conn.Execute(sql.ToString(), entitys.ToArray());
                    return ret;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public zxjc_djgw Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(zxjc_djgw entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update zxjc_djgw ");
                sql.Append("  set gcdm = :gcdm, ");
                sql.Append("      scx = :scx, ");
                sql.Append("      gwh = :gwh, ");
                sql.Append("      jx_no = :jx_no, ");
                sql.Append("      status_no = :status_no, ");
                sql.Append("      djno = :djno, ");
                sql.Append("      djxx = :djxx, ");
                sql.Append("      scbz = :scbz, ");
                sql.Append("      djlx = :djlx ");
                sql.Append(" where djno = :djno ");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entity);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<zxjc_djgw> Search(CheckPointQueryParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select gcdm, scx, gwh, jx_no, status_no, djno, djxx, scbz, lrr, lrsj, djlx ");
                sql.Append(" from zxjc_djgw where 1 = 1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (djxx like :key or status_no like :key )");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Util.Tool.ComQueryExp(parm.explist));
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_djgw>(sql.ToString(), p)
                         .OrderBy(t => t.djno)
                         .ToPagedList(parm.pageindex, parm.pagesize);
                    resultcount = q.TotalItemCount;
                    return q;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 点检编号
        /// </summary>
        public string GetDJNo()
        {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   var no = conn.ExecuteScalar<int>("select seq_pointcheck_no.nextval from dual");
                    return "DJ" + no.ToString().PadLeft(4, '0');
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}