using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models;
using MESWebApi.Services;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult List(sys_page parm)
        {
            try
            {
                MenuService ms = new MenuService();
                return Json(new { code = 1, msg = "ok" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add(dynamic obj)
        {
            try
            {
                return Json(new { code = 1, msg = "菜单添加成功" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("edit")]
        [HttpPost]
        public IHttpActionResult Edit(dynamic obj)
        {
            try
            {
                return Json(new { code = 1, msg = "数据修改成功" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("del")]
        [HttpGet]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                return Json(new { code = 1, msg = "菜单删除成功" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}