using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Dapper;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using MESWebApi.Models.BaseInfo;
using System.Text;

namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 公共基础信息服务
    /// </summary>
    public class BaseInfoService
    {
        private ILog log;
        private string constr = "";
        public BaseInfoService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public IEnumerable<base_gcxx> FactoryList()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select gcdm, gcmc,gsxx,gsmc from base_gcxx");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Query<base_gcxx>(sql.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}