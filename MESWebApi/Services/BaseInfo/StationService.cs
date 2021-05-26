using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Dapper;
using MESWebApi.Models.BaseInfo;
using Dapper.Oracle;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 岗位站点
    /// </summary>
    public class StationService
    {
        private ILog log;
        private string constr = string.Empty;
        public StationService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }
    }
}