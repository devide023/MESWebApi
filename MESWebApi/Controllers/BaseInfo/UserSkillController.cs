using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models.BaseInfo;
namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/skill")]
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
        [HttpPost, Route("add")]
        public IHttpActionResult AddSkill(List<zxjc_ryxx_jn> entitys)
        {
            try
            {
                RYService rys = new RYService();
                var ret = rys.Add(entitys);
                if (ret != null)
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