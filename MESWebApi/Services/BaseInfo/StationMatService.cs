using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.InterFaces;
using log4net;
using MESWebApi.Util;
using System.Text;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 岗位物料
    /// </summary>
    public class StationMatService:DBOperImp<base_gwbj>,IComposeQuery<base_gwbj,sys_page>
    {
        ILog log;
        public StationMatService(string constr = "tjmes"):base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public IEnumerable<base_gwbj> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();

                if (parm.explist.Count > 0)
                {
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                var q = this.Conn.Query<base_gwbj>(sql.ToString(), p)
                    .OrderBy(t=>t.wlbm)
                    .ToPagedList(parm.pageindex,parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}