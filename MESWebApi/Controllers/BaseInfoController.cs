using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/baseinfo")]
    public class BaseInfoController : ApiController
    {
        [Route("gcxx")]
        [HttpGet]
        public IHttpActionResult FactoryInfo() {
            try
            {
                BaseInfoService bis = new BaseInfoService();
                var list = bis.FactoryList().Where(t=>t.gcdm=="9100");
                return Json(new { code = 1, msg = "ok",list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost,Route("scx")]
        public IHttpActionResult GetScx(dynamic obj)
        {
            try
            {
                BaseInfoService bis = new BaseInfoService();
                var list = bis.ScxList();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost,Route("person")]
        public IHttpActionResult GetPerson(dynamic obj)
        {
            try
            {
                BaseInfoService bis = new BaseInfoService();
                string key = (obj.key ?? "").ToString();
                var list = bis.PersonList(key);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}