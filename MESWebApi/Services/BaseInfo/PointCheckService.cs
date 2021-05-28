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
namespace MESWebApi.Services.BaseInfo
{
    public class PointCheckService:IDBOper<zxjc_djgw>
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
            throw new NotImplementedException();
        }
    }
}