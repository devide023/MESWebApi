using MESWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Util;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 技术通知分配接口(zxjc_t_jstcfp)
    /// </summary>
    [RoutePrefix("api/baseinfo/distribute")]
    public class DistributeController : ApiController
    {
        /// <summary>
        /// 未分配技术通知列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost,Route("list")]
        public IHttpActionResult List(DisJTParm parm)
        {
            try
            {
                int resultcount = 0;
                DistributeService diss = new DistributeService();
                var list = diss.GetUnDistributeJT(parm, out resultcount);
                return Json(new { code = 1, msg = "ok",list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 分配记录,根据技通id
        /// </summary>
        /// <param name="obj">
        /// 例：{jtid:''}
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("dislist")]
        public IHttpActionResult DisList(dynamic obj)
        {
            try
            {
                string jtid = string.Empty;
                jtid = (obj.jtid ?? "").ToString();
                DistributeService diss = new DistributeService();
                var list = diss.GetDistributeListById(jtid);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        public IHttpActionResult Add(List<zxjc_t_jstcfp> entitys) 
        {
            try
            {
                DistributeService diss = new DistributeService();
                sys_user user = CacheManager.Instance().Current_User;
                entitys.ForEach(t => { t.lrr1 = user.name; t.lrsj1 = DateTime.Now; });
                var ret = diss.Add(entitys);
                if (ret>0)
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
        /// 编辑分配
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(zxjc_t_jstcfp entity)
        {
            try
            {
                DistributeService diss = new DistributeService();
                int ret = diss.Modify(entity);
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
        /// 删除分配信息
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("del")]
        public IHttpActionResult DelDistribute(List<zxjc_t_jstcfp> entitys)
        {
            try
            {
                DistributeService diss = new DistributeService();
                entitys.ForEach(t => t.gcdm = "9100");
                int ret = diss.DelDistribute(entitys);
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