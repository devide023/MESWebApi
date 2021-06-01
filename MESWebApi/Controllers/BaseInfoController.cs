using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models.BaseInfo;
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
        [HttpGet,Route("scx")]
        public IHttpActionResult GetScx()
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
        [HttpGet, Route("gwlist")]
        public IHttpActionResult GetGwList()
        {
            try
            {
                BaseInfoService bis = new BaseInfoService();
                var list = bis.GWList();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("jxlist")]
        public IHttpActionResult GetJXList(dynamic obj)
        {
            try
            {
                string key = (obj.key ?? "").ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    BaseInfoService bis = new BaseInfoService();
                    var list = bis.GetJxList(key);
                    return Json(new { code = 1, msg = "ok", list = list });
                }
                else {
                    return Json(new { code = 1, msg = "ok", list = new List<ztbm_new>() });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost, Route("ztlist")]
        public IHttpActionResult GetZTByJX(dynamic obj)
        {
            try
            {
                string key = (obj.key ?? "").ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    BaseInfoService bis = new BaseInfoService();
                    var list = bis.GetZtList(key);
                    return Json(new { code = 1, msg = "ok", list = list });
                }
                else
                {
                    return Json(new { code = 1, msg = "ok", list = new List<ztbm_new>() });
                }
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