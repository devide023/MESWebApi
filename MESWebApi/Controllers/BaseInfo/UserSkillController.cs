using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/userskill")]
    public class UserSkillController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult UserSkillList()
        {
            try
            {
                RYService rys = new RYService();
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception)
            {
                throw;
            }    
        }
    }
}