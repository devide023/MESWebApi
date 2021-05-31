using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/fault")]
    public class FaultController : ApiController
    {
        [HttpPost, Route("list")]
        public IHttpActionResult List() {
            try
            {
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}