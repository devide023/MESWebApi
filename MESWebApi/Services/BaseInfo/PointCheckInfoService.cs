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
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 点检信息
    /// </summary>
    public class PointCheckInfoService:IDBOper<zxjc_djxx>
    {
        private ILog log;
        private string constr = string.Empty;
        public PointCheckInfoService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_djxx Add(zxjc_djxx entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int cnt = conn.Execute(sql.ToString(), entity);
                    if (cnt > 0)
                    {
                        return new zxjc_djxx();
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
        public int Add(List<zxjc_djxx> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entitys);
                    
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

        public zxjc_djxx Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(zxjc_djxx entity)
        {
            throw new NotImplementedException();
        }
    }
}