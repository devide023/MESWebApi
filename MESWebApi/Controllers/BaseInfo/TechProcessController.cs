using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models.BaseInfo;
using System.Web;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 电子工艺控制器
    /// </summary>
    /// 
    [RoutePrefix("api/baseinfo/dzgy")]
    public class TechProcessController : ApiController
    {
        [HttpPost,Route("list")]
        public IHttpActionResult List(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                DZGYService dzgys = new DZGYService();
                var list = dzgys.Search(parm,out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_t_dzgy> entitys)
        {
            try
            {
                DZGYService dzgys = new DZGYService();
                int cnt = dzgys.Add(entitys);
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

        [HttpPost, Route("readxls")]
        public IHttpActionResult ReadXls(dynamic obj)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/UpLoad/");
                string filename = (obj.filename ?? "").ToString();
                if (!string.IsNullOrEmpty(filename))
                {
                    string fullpath = path + filename;
                    DZGYService dzgys = new DZGYService();
                    var list = dzgys.FromExcel(fullpath);
                    return Json(new { code = 1, msg = "ok", list = list });
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
    }
}