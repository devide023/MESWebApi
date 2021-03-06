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
    /// <summary>
    /// 设备信息接口（base_sbxx）
    /// </summary>
    [RoutePrefix("api/baseinfo/device")]
    public class DeviceController : ApiController
    {
        /// <summary>
        /// 设备列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost, Route("list")]
        public IHttpActionResult List(DeviceQueryParm parm)
        {
            try
            {
                int resultcount = 0;
                DeviceService ds = new DeviceService();
                var list = ds.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok",list=list,resultcount = resultcount });
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
        public IHttpActionResult AddDevice(List<base_sbxx> entitys)
        {
            try
            {
                DeviceService ds = new DeviceService();
                var ret = ds.Add(entitys);
                if (ret != null)
                {
                    return Json(new { code = 1, msg = "数据保成功" });
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
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult EditDevice(base_sbxx entity) {
            try
            {
                DeviceService ds = new DeviceService();
                int isok = ds.Modify(entity);
                if (isok>0)
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

    }
}