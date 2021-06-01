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
    public class RYService :IDBOper<zxjc_ryxx_jn>,IComposeQuery<zxjc_ryxx_jn,SkillQueryParm>
    {
        private ILog log;
        private string constr = "";
        public RYService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_ryxx_jn Add(zxjc_ryxx_jn entity)
        {
            throw new NotImplementedException();
        }
        public zxjc_ryxx_jn Add(List<zxjc_ryxx_jn> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_ryxx_jn(gcdm, user_code, jnbh, jnxx, scx, gwh, sfhg, lrr, lrsj, jnfl, jnsj) ");
                sql.Append(" values ");
                sql.Append("(:gcdm,:user_code,:jnbh,:jnxx,:scx,:gwh,:sfhg,:lrr, sysdate,:jnfl,:jnsj)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   int cnt = conn.Execute(sql.ToString(), entitys.ToArray());
                    if(cnt>0)
                    {
                        return new zxjc_ryxx_jn();
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

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public zxjc_ryxx_jn Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(zxjc_ryxx_jn entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update zxjc_ryxx_jn");
                sql.Append(" set gcdm = :gcdm,");
                sql.Append("        user_code = :user_code,");
                sql.Append("        jnbh = :jnbh,");
                sql.Append("        jnxx = :jnxx,");
                sql.Append("        scx = :scx,");
                sql.Append("        gwh = :gwh,");
                sql.Append("        sfhg = :sfhg,");
                sql.Append("        jnfl = :jnfl,");
                sql.Append("        jnsj = :jnsj");
                sql.Append(" where  jnbh = :jnbh ");
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

        public IEnumerable<zxjc_ryxx_jn> Search(SkillQueryParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT ta.gcdm, ta.user_code,(select user_name from sec_users where user_code = ta.user_code) as user_name, ta.jnbh, ta.jnxx, ta.scx, ta.gwh,(select work_name from ZXJC_GXZD where work_no = ta.gwh) as gwmc, ta.sfhg, ta.lrr, ta.lrsj, ta.jnfl, ta.jnsj");
                sql.Append(" FROM zxjc_ryxx_jn ta where 1 = 1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (user_code like :key or jnbh like :key) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    foreach (var item in parm.explist)
                    {
                        sql.Append($"{item.left}");
                        if (item.oper == "like")
                        {
                            sql.Append($" {item.colname} {item.oper} '%{item.value}%' {item.logic} ");
                        }
                        else
                        {
                            sql.Append($" {item.colname} {item.oper} '{item.value}' {item.logic} ");
                        }
                        sql.Append($"{item.right}");
                    }
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_ryxx_jn>(sql.ToString(), p)
                          .OrderBy(t => t.jnbh)
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

        public string GetSkillNo()
        {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int skillid = conn.ExecuteScalar<int>("SELECT seq_skill_id.nextval FROM dual");
                    return "JN" + skillid.ToString().PadLeft(4, '0');
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