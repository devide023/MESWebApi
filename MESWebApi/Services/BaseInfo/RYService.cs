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
using System.Text;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class RYService :IDBOper<zxjc_ryxx_jn>
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
            throw new NotImplementedException();
        }
    }
}