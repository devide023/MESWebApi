﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MESWebApi.Controllers
{
    [RoutePrefix("api/baseinfo")]
    public class BaseInfoController : ApiController
    {
        [Route("gcxx")]
        [HttpPost]
        public IHttpActionResult FactoryInfo() {
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