using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
namespace MESWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo/skill")]
    public class UserSkillController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult UserSkillList(SkillQueryParm parm)
        {
            try
            {
                int resultcount = 0;
                RYService rys = new RYService();
                var list = rys.Search(parm,out resultcount);
                return Json(new { code = 1, msg = "ok",list = list, resultcount = resultcount });
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

        [HttpPost, Route("edit")]
        public IHttpActionResult EditSkill(zxjc_ryxx_jn entity)
        {
            try
            {
                RYService rys = new RYService();
                var ret = rys.Modify(entity);
                if (ret >0)
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
        [HttpGet, Route("getskillno")]
        public IHttpActionResult SkillNo()
        {
            try
            {
                RYService rys = new RYService();
                var ret = rys.GetSkillNo();
                if (ret.Length > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功", no = ret }); ;
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败",no = "" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}