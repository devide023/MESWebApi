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
    public class StationService:DBOperImp<zxjc_gxzd>
    {
        private ILog log;
        public StationService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

    }
}