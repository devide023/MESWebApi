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
using MESWebApi.Util;

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
        [HttpPost, Route("edit")]
        public IHttpActionResult edit(zxjc_t_dzgy entity)
        {
            try
            {
                DZGYService dzgys = new DZGYService();
                int cnt = dzgys.Modify(entity);
                if (cnt > 0)
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

        [HttpGet, Route("dzgyno")]
        public IHttpActionResult DZGYNumber()
        {
            try
            {
                DZGYService dzgys = new DZGYService();
                var number = dzgys.GetDZGYNumber();
                return Json(new { code = 1, msg = "ok", dzgyid=Guid.NewGuid().ToString(),dzgyno = number });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost,Route("updatefiles")]
        public IHttpActionResult ChangeFileName(dynamic obj)
        {
            try
            {
                string gyid = (obj.gyid ?? "").ToString();
                List<sys_file> files = obj.files!=null? obj.files.ToObject<List<sys_file>>():new List<sys_file>();
                string filenames = string.Empty;
                files.ForEach(t => filenames = filenames + t.filename + ",");
                if (filenames.Length > 0) {
                    filenames = filenames.Remove(filenames.Length - 1, 1);
                }
                sys_user user = CacheManager.Instance().Current_User;
                DZGYService dzgys = new DZGYService();
                zxjc_t_dzgy entity = new zxjc_t_dzgy() {
                    gyid = gyid,
                    wjlj = filenames,
                    jwdx = files.Sum(t => t.filesize).ToString(),
                    scry = user.name,
                    scsj = DateTime.Now
                };
                int cnt = dzgys.ModifyFileName(entity);
                if (cnt > 0) { 
                return Json(new { code = 1, msg = "文件上传成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "文件上传失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}