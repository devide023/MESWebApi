using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
using MESWebApi.Models.BaseInfo;
using System.Text;
using MESWebApi.Models.QueryParm;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 设备基础信息
    /// </summary>
    public class DeviceService : IDBOper<base_sbxx>,IComposeQuery<base_sbxx,DeviceQueryParm>
    {
        private ILog log;
        private string constr = string.Empty;
        public DeviceService()
        {
            log = LogManager.GetLogger(this.GetType());
            this.constr = "tjmes";
        }

        public base_sbxx Add(List<base_sbxx> entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" insert into base_sbxx(sbbh, sbmc, gcdm, scx, gwh, sblx, txfs, ip, sfky, sflj, bz, lrr, lrsj, com, port)");
                sql.Append(" values");
                sql.Append(" (:sbbh, :sbmc, :gcdm, :scx, :gwh, :sblx, :txfs, :ip, :sfky, :sflj, :bz, :lrr, sysdate, :com, :port)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int ret = conn.Execute(sql.ToString(), entity.ToArray());
                    if (ret > 0)
                    {
                        return new base_sbxx();
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

        public base_sbxx Add(base_sbxx entity)
        {
            try
            {
                using (var db = new OracleBaseFixture(constr).DB)
                {
                   var ret = db.Insert(entity);
                    if (ret != null)
                    {
                        return entity;
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

        public base_sbxx Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(base_sbxx entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update base_sbxx ");
                sql.Append(" set sbbh=:sbbh,");
                sql.Append(" sbmc=:sbmc,");
                sql.Append(" gcdm=:gcdm,");
                sql.Append(" scx=:scx,");
                sql.Append(" gwh=:gwh,");
                sql.Append(" sblx=:sblx,");
                sql.Append(" txfs=:txfs,");
                sql.Append(" ip=:ip,");
                sql.Append(" sfky=:sfky,");
                sql.Append(" sflj=:sflj,");
                sql.Append(" bz=:bz,");
                sql.Append(" com=:com,");
                sql.Append(" port =:port where sbbh = :sbbh ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":sbbh", entity.sbbh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sbmc", entity.sbmc, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":gcdm", entity.gcdm, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":scx", entity.scx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":gwh", entity.gwh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sblx", entity.sblx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":txfs", entity.txfs, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":ip", entity.ip, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sfky", entity.sfky, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sflj", entity.sflj, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":bz", entity.bz, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":com", entity.com, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":port", entity.port, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);

                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), p);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<base_sbxx> Search(DeviceQueryParm parm, out int resultcount)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select sbbh, sbmc, gcdm, scx, gwh, sblx, txfs, ip, sfky, sflj, bz, lrr, lrsj, com, port from base_sbxx where 1=1 ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (sbbh like :key or sbmc like :key) ");
                    p.Add(":key", "%"+parm.keyword+"%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Util.Tool.ComQueryExp(parm.explist));
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   var q = conn.Query<base_sbxx>(sql.ToString(), p)
                        .OrderBy(t => t.sbbh)
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