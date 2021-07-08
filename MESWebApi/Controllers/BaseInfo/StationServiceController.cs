using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 岗位站点接口（zxjc_gxzd）
    /// </summary>
    /// 
    [RoutePrefix("api/baseinfo/station")]
    public class StationServiceController : ApiController
    {
        /// <summary>
        /// 岗位站点列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost,Route("list")]
        public IHttpActionResult List(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                return Json(new { code = 1, msg = "ok", resultcount = resultcount });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_gxzd> entitys)
        {
            try
            {
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(zxjc_gxzd entity)
        {
            try
            {
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 批量编辑
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("batedit")]
        public IHttpActionResult BatEdit(List<zxjc_gxzd> entitys)
        {
            try
            {
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("del")]
        public IHttpActionResult Del(List<zxjc_gxzd> entitys)
        {
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