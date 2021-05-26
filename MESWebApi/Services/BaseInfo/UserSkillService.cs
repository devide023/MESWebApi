using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Text;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.DB;
using MESWebApi.Models.BaseInfo;
using MESWebApi.InterFaces;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 人员技能基础信息
    /// </summary>
    public class UserSkillService:IDBOper<zxjc_ryxx_jn>
    {
        private ILog log;
        private string constr = "";
        public UserSkillService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_ryxx_jn Add(zxjc_ryxx_jn entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_ryxx_jn(gcdm, user_code, jnbh, jnxx, scx, gwh, sfhg, lrr, lrsj, jnfl, jnsj) ");
                sql.Append(" values(:gcdm,:user_code,:jnbh,:jnxx,:scx,:gwh,:sfhg,:lrr,sysdate,:jnfl,:jnsj) ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":gcdm", entity.gcdm, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":user_code", entity.user_code, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnbh", entity.jnbh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnxx", entity.jnxx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":scx", entity.scx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":gwh", entity.gwh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sfhg", entity.sfhg, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":lrr", entity.lrr, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnfl", entity.jnfl, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnsj", entity.jnsj, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    conn.Execute(sql.ToString(), p);
                    return entity;
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
                sql.Append(" update zxjc_ryxx_jn");
                sql.Append(" set gcdm = :gcdm,");
                sql.Append("        user_code = :user_code,");
                sql.Append("        jnbh = :jnbh,");
                sql.Append("        jnxx = :jnxx,");
                sql.Append("        scx = :scx,");
                sql.Append("        gwh = :gwh,");
                sql.Append("        sfhg = :sfhg,");
                sql.Append("        jnfl = :jnfl,");
                sql.Append("        jnsj = :jnsj");
                sql.Append(" where  gcdm = :gcdm");
                sql.Append(" and    user_code = :user_code");
                sql.Append(" and    jnbh = :jnbh");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":gcdm", entity.gcdm, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":user_code", entity.user_code, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnbh", entity.jnbh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnxx", entity.jnxx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":scx", entity.scx, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":gwh", entity.gwh, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":sfhg", entity.sfhg, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnfl", entity.jnfl, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                p.Add(":jnsj", entity.jnsj, OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                
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
    }
}