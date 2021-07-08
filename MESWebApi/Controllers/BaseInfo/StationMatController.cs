using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models;
using MESWebApi.Util;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 岗位站点物料接口(base_gwbj1)
    /// </summary>
    /// 
    [RoutePrefix("api/baseinfo/stationmat")]
    public class StationMatController : ApiController
    {
        /// <summary>
        /// 岗位站点物料列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost, Route("list")]
        public IHttpActionResult List(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                StationMatService sms = new StationMatService();
                var list = sms.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 读取岗位站点模板数据
        /// </summary>
        /// <param name="obj">
        /// {filename:''}
        /// </param>
        /// <returns></returns>
        [HttpPost,Route("readxls")]
        public IHttpActionResult ImpExcel(dynamic obj)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/UpLoad/");
                string filename = (obj.filename ?? "").ToString();
                if (!string.IsNullOrEmpty(filename))
                {
                    string fullpath = path + filename;
                    StationMatService smat = new StationMatService();
                    var list = smat.FromExcel(fullpath);
                    return Json(new { code = 1, msg = "ok",list = list });
                }
                else
                {
                    return Json(new { code = 0, msg = "error" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 增加岗位站点物料
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<base_gwbj1> entitys)
        {
            try
            {
                sys_user user = CacheManager.Instance().Current_User;
                entitys.ForEach(t => { t.lrr = user.name; t.lrsj = DateTime.Now; });
                StationMatService sms = new StationMatService();
                int cnt = sms.Add(entitys);
                if (cnt > 0)
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
        /// <summary>
        /// 批量编辑
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("batedit")]
        public IHttpActionResult BatEdit(List<base_gwbj1> entitys)
        {
            try
            {
                StationMatService sms = new StationMatService();
                int ret = sms.Modify(entitys);
                if (ret > 0)
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
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(base_gwbj1 entity)
        {
            try
            {
                StationMatService sms = new StationMatService();
                int ret = sms.Modify(entity);
                if (ret > 0)
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("del")]
        public IHttpActionResult Del(List<base_gwbj1> entitys)
        {
            try
            {
                StationMatService sms = new StationMatService();
                int ret = sms.Delete(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据删除成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据删除失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}