using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Services.BaseInfo;
using System.Web.Http;

namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/pointcheck")]
    public class PointCheckController : ApiController
    {
       [HttpPost,Route("list")]
        public IHttpActionResult List()
        {
            try
            {
                return Json(new { code = 1, msg = "ok", list = new List<string>() });
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
                return Json(new { code = 1, msg = "ok", list = new List<string>() });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("edit")]
        public IHttpActionResult Edit()
        {
            try
            {
                return Json(new { code = 1, msg = "ok", list = new List<string>() });
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}