using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Util;
namespace MESWebApi.Controllers
{
    /// <summary>
    /// 基础数据接口
    /// </summary>
    [RoutePrefix("api/baseinfo")]
    public class BaseInfoController : ApiController
    {
        /// <summary>
        /// 工厂信息(base_gcxx)
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 生产线(tj_base_scxxx)
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 岗位列表(zxjc_gxzd)
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 机型(ZTBM_NEW)
        /// </summary>
        /// <param name="obj">
        /// 机型字符
        /// 查找字段（jx）
        /// 例：{keyword:''}
        /// </param>
        /// <returns>返回字段（jx）</returns>
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
        /// <summary>
        /// 机型状态（ZTBM_NEW）
        /// </summary>
        /// <param name="obj">机型编码，
        /// 查找字段（jx）
        /// 例：{keyword:''}
        /// </param>
        /// <returns>返回字段cpbm</returns>
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
        /// <summary>
        /// 人员信息(sec_users)
        /// </summary>
        /// <param name="obj">
        /// 关键字筛选,
        /// 字段（用户编码、用户名称、生产线、课线）
        /// 例：{keyword:''}
        /// </param>
        /// <returns></returns>
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
        /// <summary>
        /// 物料信息(base_wlxx)
        /// </summary>
        /// <param name="obj">关键字筛选,
        /// 查找字段：（物料编码、物料名称）
        /// 例：{keyword:''}
        /// </param>
        /// <returns>{ code = 1, msg = "ok", list = list }</returns>
        [HttpPost, Route("wllist")]
        public IHttpActionResult GetWLList(dynamic obj)
        {
            try
            {
                BaseInfoService bis = new BaseInfoService();
                string key = (obj.key ?? "").ToString();
                var list = bis.GetWlList(key);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}