using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 技术通知，控制器
    /// </summary>
    /// 
    [RoutePrefix("api/baseinfo/notice")]
    public class TecNoticeController : ApiController
    {
        [HttpPost, Route("list")]
        public IHttpActionResult JTList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                JTService jts = new JTService();
                var list = jts.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost, Route("add")]
        public IHttpActionResult JTAdd(List<zxjc_t_jstc> entitys)
        {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Add(entitys);
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

        [HttpPost, Route("edit")]
        public IHttpActionResult JTEdit(zxjc_t_jstc entity)
        {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Modify(entity);
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

        [HttpPost, Route("tsjt/list")]
        public IHttpActionResult TsjtList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                TsJTService tsjts = new TsJTService();
                var list = tsjts.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("tsjt/add")]
        public IHttpActionResult TsjtAdd(List<zxjc_t_tstc> entitys)
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Add(entitys);
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

        [HttpPost, Route("tsjt/edit")]
        public IHttpActionResult TsjtEdit(zxjc_t_tstc entity)
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Modify(entity);
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

        [HttpPost, Route("jtyd/list")]
        public IHttpActionResult JtydList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                JTYDService jtyds = new JTYDService();
                var list = jtyds.Search(parm,out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("jtyd/add")]
        public IHttpActionResult JtydAdd(List<zxjc_t_ydjl> entitys)
        {
            try
            {
                JTYDService jtyds = new JTYDService();
                int cnt = jtyds.Add(entitys);
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
    }
}