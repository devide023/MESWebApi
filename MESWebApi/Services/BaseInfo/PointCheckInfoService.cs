using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Text;
using Dapper;
using Dapper.Oracle;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using MESWebApi.Util;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 点检信息
    /// </summary>
    public class PointCheckInfoService : DBOperImp<zxjc_djxx>, IComposeQuery<zxjc_djxx, sys_page>
    {
        private ILog log;
        string constr = string.Empty;
        public PointCheckInfoService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        //public override int Modify(zxjc_djxx entity)
        //{
        //    try
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        sql.Append("update zxjc_djxx set gcdm=:gcdm,");
        //        sql.Append(" scx=:scx,");
        //        sql.Append(" gwh =:gwh,");
        //        sql.Append(" jx_no =:jx_no,");
        //        sql.Append(" status_no =:status_no,");
        //        sql.Append(" djno =:djno,");
        //        sql.Append(" djxx =:djxx,");
        //        sql.Append(" djjg =:djjg,");
        //        sql.Append(" bz =:bz where id=:id ");
        //        using (var conn = new OraDBHelper(constr).Conn)
        //        {
        //            return conn.Execute(sql.ToString(), entity);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e.Message);
        //        throw;
        //    }
        //}

        public IEnumerable<zxjc_djxx> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select id,gcdm,scx,gwh,jx_no,status_no,djno,djxx,djjg,bz,lrr,lrsj from zxjc_djxx where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (djxx like :key or jx_no like :key or djno like :key) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_djxx>(sql.ToString(), p)
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
    }
}