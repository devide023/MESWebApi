using MESWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using MESWebApi.Services.BaseInfo;
namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/distribute")]
    public class DistributeController : ApiController
    {
        [HttpPost,Route("list")]
        public IHttpActionResult List(DisJTParm parm)
        {
            try
            {
                int resultcount = 0;
                DistributeService diss = new DistributeService();
                var list = diss.GetUnDistributeJT(parm, out resultcount);
                return Json(new { code = 1, msg = "ok",list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_t_jstcfp> entitys) 
        {
            try
            {
                DistributeService diss = new DistributeService();
                var ret = diss.Add(entitys);
                if (ret>0)
                {
                    return Json(new { code = 1, msg = "数据保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据保存失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}