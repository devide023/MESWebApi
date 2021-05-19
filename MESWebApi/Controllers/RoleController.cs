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
                return Json(new { code = 1, msg = "ok", list = list, resultcount = cnt });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
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
        public IHttpActionResult Add(dynamic objs)
        {
            try
            {
                List<int> menuids = new List<int>();
                List<sys_menu_permission> permis = new List<sys_menu_permission>();
                List<sys_menu> list = objs.menu_nodes.ToObject<List<sys_menu>>();
                var pages = list.Where(t => new string[] { "01", "02" }.Contains(t.menutype));
                menuids = pages.Select(t => t.id).ToList();
                foreach (var item in pages)
                {
                    sys_menu_permission mp = new sys_menu_permission();
                    sys_permission p = new sys_permission();
                    //功能列表
                    var funfloder = list.Where(t => t.pid == item.id && t.title == "页面功能").FirstOrDefault();
                    if (funfloder != null)
                    {
                        p.funs = list.Where(t => t.pid == funfloder.id).Select(t => t.code).ToList();
                    }
                    //编辑字段
                    var editfloder = list.Where(t => t.pid == item.id && t.title == "编辑字段").FirstOrDefault();
                    if (editfloder != null)
                    {
                        p.editfields = list.Where(t => t.pid == editfloder.id).Select(t => t.code).ToList();
                    }
                    //隐藏字段
                    var hidefloder = list.Where(t => t.pid == item.id && t.title == "隐藏字段").FirstOrDefault();
                    if (hidefloder != null)
                    {
                        p.hidefields = list.Where(t => t.pid == hidefloder.id).Select(t => t.code).ToList();
                    }
                    mp.menuid = item.id;
                    mp.permission = p;
                    permis.Add(mp);
                }
                RoleService rs = new RoleService();
                int adduserid = 0, status;
                int.TryParse(objs.adduser != null ? objs.adduser.ToString() : "0", out adduserid);
                int.TryParse(objs.status != null ? objs.status.ToString() : "1", out status);
                sys_role entity = new sys_role()
                {
                    title = objs.title.ToString(),
                    code = objs.code.ToString(),
                    addtime = DateTime.Now,
                    adduser = adduserid,
                    status = status
                };
                entity = rs.Add(entity);
                if (entity.id > 0)
                {
                    permis.ForEach(t => t.roleid = entity.id);
                    int ret = rs.Save_RoleMenus(permis);
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
                int roleid = 0, status = 1;
                List<int> menuids = new List<int>();
                List<sys_menu_permission> permis = new List<sys_menu_permission>();
                List<sys_menu> list = obj.menu_nodes.ToObject<List<sys_menu>>();
                var pages = list.Where(t => new string[] { "01", "02" }.Contains(t.menutype));
                menuids = pages.Select(t => t.id).ToList();
                foreach (var item in pages)
                {
                    sys_menu_permission mp = new sys_menu_permission();
                    sys_permission p = new sys_permission();
                    //功能列表
                    var funfloder = list.Where(t => t.pid == item.id && t.title == "页面功能").FirstOrDefault();
                    if (funfloder != null)
                    {
                        p.funs = list.Where(t => t.pid == funfloder.id).Select(t => t.code).ToList();
                    }
                    //编辑字段
                    var editfloder = list.Where(t => t.pid == item.id && t.title == "编辑字段").FirstOrDefault();
                    if (editfloder != null)
                    {
                        p.editfields = list.Where(t => t.pid == editfloder.id).Select(t => t.code).ToList();
                    }
                    //隐藏字段
                    var hidefloder = list.Where(t => t.pid == item.id && t.title == "隐藏字段").FirstOrDefault();
                    if (hidefloder != null)
                    {
                        p.hidefields = list.Where(t => t.pid == hidefloder.id).Select(t => t.code).ToList();
                    }
                    mp.menuid = item.id;
                    mp.permission = p;
                    permis.Add(mp);
                }
                int.TryParse(obj.id != null ? obj.id.ToString() : "0", out roleid);
                int.TryParse(obj.status != null ? obj.status.ToString() : "1", out status);
                RoleService rs = new RoleService();
                sys_role entity = new sys_role()
                {
                    id = roleid,
                    title = obj.title.ToString(),
                    status = status
                };
                int cnt = rs.Modify(entity);
                if (cnt > 0)
                {
                    permis.ForEach(t => t.roleid = roleid);
                    int ret = rs.Save_RoleMenus(permis);
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

        [Route("find")]
        [HttpPost]
        public IHttpActionResult Find(int id)
        {
            try
            {
                RoleService rs = new RoleService();
                sys_role entity = rs.Find(id);
                return Json(new { code = 1, msg = "ok", role = entity });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("menulist")]
        [HttpGet]
        public IHttpActionResult RoleMenus(int id)
        {
            try
            {
                RoleService rs = new RoleService();
                var menus = rs.Get_Role_Menus(id);
                return Json(new { code = 1, msg = "ok", menus = menus });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("disable")]
        [HttpPost]
        public IHttpActionResult Disable(dynamic obj)
        {
            try
            {
                int upuser, status;
                int.TryParse(obj.upuser != null ? obj.upuser.ToString() : "0", out upuser);
                int.TryParse(obj.status != null ? obj.status.ToString() : "1", out status);
                List<int> ids = obj.ids != null ? obj.ids.ToObject<List<int>>() : new List<int>();
                RoleService rs = new RoleService();
                int ret = rs.RoleStatus(ids, status, upuser);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据操作成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据操作失败" });
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("userlist")]
        [HttpGet]
        public IHttpActionResult RoleUsers(int id)
        {
            try
            {
                RoleService rs = new RoleService();
                var list = rs.GetRoleUsers(id);
                return Json(new { code = 1, msg = "ok",list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}