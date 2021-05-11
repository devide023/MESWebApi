using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models;
using MESWebApi.Services;
using Newtonsoft.Json;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult Get()
        {
            try
            {
                int cnt;
                RoleService rs = new RoleService();
                sys_page page = new sys_page();
                page.pageindex = 1;
                page.pagesize = int.MaxValue;
                var list = rs.Find(page, out cnt);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message});
            }
        }

        [Route("alllist")]
        [HttpGet]
        public IHttpActionResult All_List()
        {
            try
            {
                int cnt;
                RoleService rs = new RoleService();
                sys_page page = new sys_page();
                page.pageindex = 1;
                page.pagesize = int.MaxValue;
                var list = rs.Find(page, out cnt);
                return Json(new { code = 1, msg = "ok", list = list });
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
                RoleService rs = new RoleService();
                int adduserid = 0, status;
                List<int> menuids = obj.menuids.ToObject<List<int>>();
                int.TryParse(obj.adduser, out adduserid);
                int.TryParse(obj.status, out status);
                sys_role entity = new sys_role()
                {
                    title = obj.title.ToString(),
                    code = obj.code.ToString(),
                    addtime = DateTime.Now,
                    adduser = adduserid,
                    status = status
                };
                sys_role role = new sys_role();
                role = rs.Add(role);
                if (role.id > 0)
                {
                    int ret = rs.Save_RoleMenus(role.id, menuids);
                    return Json(new { code = 1, msg = "角色添加成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "角色添加失败" });
                }
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
                int roleid = 0, adduserid = 0, status;
                int.TryParse(obj.id, out roleid);
                int.TryParse(obj.adduser, out adduserid);
                int.TryParse(obj.status, out status);
                RoleService rs = new RoleService();
                sys_role entity = new sys_role()
                {
                    id = roleid,
                    title = obj.title.ToString(),
                    code = obj.code.ToString(),
                    adduser = adduserid,
                    status = status
                };
                int cnt = rs.Modify(entity);
                if (cnt > 0)
                {
                    return Json(new { code = 1, msg = "角色修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "角色修改失败" });
                }
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
                RoleService rs = new RoleService();
                bool ok = rs.Delete(id);
                if (ok)
                {
                    return Json(new { code = 1, msg = "角色删除成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "角色删除失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}