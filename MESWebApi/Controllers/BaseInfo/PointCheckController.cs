using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using MESWebApi.Services.BaseInfo;
using System.Web.Http;
using MESWebApi.Util;
using MESWebApi.Models;
using System.Web;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 点检接口(zxjc_djgw)
    /// </summary>
    [RoutePrefix("api/baseinfo/pointcheck")]
    public class PointCheckController : ApiController
    {
        /// <summary>
        /// 点检列表
        /// </summary>
        /// <param name="parm">CheckPointQueryParm</param>
        /// <returns>{code:1,msg:'ok',list:[],resultcount:20}</returns>
        [HttpPost, Route("list")]
        public IHttpActionResult List(CheckPointQueryParm parm)
        {
            try
            {
                int resultcount = 0;
                PointCheckService pcs = new PointCheckService();
                var list = pcs.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 新增点检信息
        /// </summary>
        /// <param name="entitys">
        /// zxjc_djgw
        /// 例：[{zxjc_djgw实体}]
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_djgw> entitys)
        {
            try
            {
                sys_user user = CacheManager.Instance().Current_User;
                entitys.ForEach(t => { t.lrr = user.name; t.lrsj = DateTime.Now; });
                PointCheckService pcs = new PointCheckService();
                var ret = pcs.Add(entitys);
                if (ret > 0)
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
        /// 删除点检信息
        /// </summary>
        /// <param name="entitys">
        /// zxjc_djgw
        /// 例：[{zxjc_djgw实体}]
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("del")]
        public IHttpActionResult Delete(List<zxjc_djgw> entitys) {
            try
            {
                PointCheckService pcs = new PointCheckService();
                int cnt = pcs.Delete(entitys);
                if (cnt > 0)
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
        /// <summary>
        /// 编辑点检信息
        /// </summary>
        /// <param name="entity">
        /// 例：{zxjc_djgw实体}
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(zxjc_djgw entity)
        {
            try
            {
                PointCheckService pcs = new PointCheckService();
                int ret = pcs.Modify(entity);
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
        /// 批量编辑
        /// </summary>
        /// <param name="entitys">
        /// 例：[{zxjc_djgw实体}]
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("batedit")]
        public IHttpActionResult BatEdit(List<zxjc_djgw> entitys)
        {
            try
            {
                PointCheckService pcs = new PointCheckService();
                int ret = pcs.Modify(entitys);
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
        /// 生产点检编号
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("djno")]
        public IHttpActionResult DjNo()
        {
            try
            {
                PointCheckService pcs = new PointCheckService();
                var no = pcs.GetDJNo();
                return Json(new { code = 1, msg = "ok",djno = no });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 读取点检模板数据
        /// </summary>
        /// <param name="obj">
        /// {filename:''}
        /// </param>
        /// <returns></returns>
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
                    PointCheckService pcs = new PointCheckService();
                    var list = pcs.FromExcel(fullpath);
                    return Json(new { code = 1, msg = "ok", list = list });
                }
                else
                {
                    return Json(new { code = 0, msg = "文件名为空或不存在" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}