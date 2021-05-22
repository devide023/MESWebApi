using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Models;
using MESWebApi.Models.QueryParm;
using MESWebApi.Services;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult List(MenuQueryParm parm)
        {
            try
            {
                int rscnt = 0;
                MenuService ms = new MenuService();
                var list = ms.Search(parm, out rscnt);
                return Json(new { code = 1, msg = "ok",list=list,resultcount=rscnt });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("tree")]
        [HttpPost]
        public IHttpActionResult MenuTree(MenuQueryParm parm)
        {
            try
            {
                int rescnt = 0;
                MenuService ms = new MenuService();
                var list = ms.MenuTree(parm,out rescnt);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = rescnt});
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("submenu")]
        [HttpPost]
        public IHttpActionResult SubMenu(MenuQueryParm parm)
        {
            try
            {
                int rescnt = 0;
                MenuService ms = new MenuService();
                var list = ms.SubMenuByPid(parm, out rescnt);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = rescnt });
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
                int pid = 0,status=1,adduserid=0,seq;
                int.TryParse(obj.pid.ToString(), out pid);
                int.TryParse(obj.status!=null? obj.status.ToString():"1", out status);
                int.TryParse(obj.adduser!=null?obj.adduser.ToString():"0", out adduserid);
                int.TryParse(obj.seq!=null?obj.seq1.ToString():"0", out seq);
                MenuService ms = new MenuService();
                sys_menu entity = new sys_menu() {
                    pid = pid,
                    code = obj.code.ToString(),
                    icon = obj.icon.ToString(),
                    path = obj.path.ToString(),
                    title = obj.title.ToString(),
                    viewpath = obj.viewpath.ToString(),
                    menutype=obj.menutype.ToString(),
                    status=status,
                    adduser=adduserid,
                    seq = seq,
                    addtime = DateTime.Now
                };
                entity = ms.Add(entity);
                if (entity.id > 0)
                {
                    return Json(new { code = 1, msg = "菜单添加成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "菜单添加失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        /// <summary>
        /// 添加功能，字段
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [Route("add_funs_field")]
        [HttpPost]
        public IHttpActionResult AddFuns_Filed(dynamic menu) {
            try
            {
                MenuService ms = new MenuService();
                List<dynamic> funs = menu.funs.ToObject<List<dynamic>>();
                List<dynamic> fiels = menu.fields.ToObject<List<dynamic>>();
                funs = funs.Where(t => !string.IsNullOrEmpty((t.name??"").ToString()) && !string.IsNullOrEmpty((t.code??"").ToString())).ToList();
                fiels = fiels.Where(t => !string.IsNullOrEmpty((t.name??"").ToString()) && !string.IsNullOrEmpty((t.code??"").ToString())).ToList();
                int pid = 0,adduserid=0;
                List<int> oklist = new List<int>();
                int.TryParse(menu.pid!=null?menu.pid.ToString():"0", out pid);
                int.TryParse(menu.adduser!=null?menu.adduser.ToString():"0", out adduserid);
                var codelist = ms.FindCodesByPid(pid);
                List<sys_menu> insertlist = new List<sys_menu>();
                foreach (var item in funs)
                {
                    var isexsit = codelist.Where(t => t == item.code.ToString()).Count();
                    if (isexsit > 0) continue;
                    var entity = new sys_menu()
                    {
                        code = item.code,
                        menutype = "03",
                        title = item.name,
                        pid = pid,
                        addtime = DateTime.Now,
                        seq = 10,
                        status = 1,
                        icon="",
                        adduser = adduserid
                    };
                    insertlist.Add(entity);
                }
                foreach (var item in fiels)
                {
                    var isexsit = codelist.Where(t => t == item.code.ToString()).Count();
                    if (isexsit > 0 ) continue;
                    var entity = new sys_menu()
                    {
                        code = item.code,
                        menutype = "04",
                        title = item.name,
                        pid = pid,
                        addtime = DateTime.Now,
                        seq = 10,
                        status = 1,
                        icon=" ",
                        adduser = adduserid
                    };
                    insertlist.Add(entity);
                }
                if(ms.Add(insertlist))
                {
                    return Json(new { code = 1, msg = "数据保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据保存失败" });
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
                int id = 0,  status = 1, upuserid = 0,seq=10;
                int.TryParse(obj.id!=null?obj.id.ToString():"0", out id);
                int.TryParse(obj.status!=null?obj.status.ToString():"1", out status);
                int.TryParse(obj.updateuser!=null?obj.updateuser.ToString():"0", out upuserid);
                int.TryParse(obj.seq!=null?obj.seq.ToString():"10", out seq);
                MenuService ms = new MenuService();
                sys_menu entity = new sys_menu()
                {
                    id = id,
                    code = obj.code!=null?obj.code.ToString():"",
                    icon = obj.icon!=null?obj.icon.ToString():"",
                    path = obj.path!=null?obj.path.ToString():"",
                    title = obj.title!=null?obj.title.ToString():"",
                    viewpath = obj.viewpath!=null?obj.viewpath.ToString():"",
                    menutype = obj.menutype!=null?obj.menutype.ToString():"",
                    status = status,
                    seq = seq,
                    updateuser = upuserid
                };
                int cnt = ms.Modify(entity);
                if (cnt > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
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
                MenuService ms = new MenuService();
                bool isok = ms.Delete(id);
                if (isok)
                {
                    return Json(new { code = 1, msg = "菜单删除成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "菜单删除失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("menucode")]
        [HttpGet]
        public IHttpActionResult MenuCode(int id)
        {
            try
            {
                MenuService ms = new MenuService();
                string code = ms.MenuMaxCode(id);
                return Json(new { code = 1, msg = "ok", menucode = code });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("permission_tree")]
        [HttpGet]
        public IHttpActionResult PermissionTree()
        {
            try
            {
                MenuService ms = new MenuService();
                var list = ms.PermissionTree();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}