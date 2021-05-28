using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using MESWebApi.Services.BaseInfo;
using System.Web.Http;

namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/pointcheck")]
    public class PointCheckController : ApiController
    {
        [HttpPost, Route("list")]
        public IHttpActionResult List(CheckPointQueryParm parm)
        {
            try
            {
                int resultcount = 0;
                PointCheckService pcs = new PointCheckService();
                var list = pcs.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_djgw> entitys)
        {
            try
            {
                PointCheckService pcs = new PointCheckService();
                var ret = pcs.Add(entitys);
                if (ret > 0)
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

        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(zxjc_djgw entity)
        {
            try
            {
                PointCheckService pcs = new PointCheckService();
                int ret = pcs.Modify(entity);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}