using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services;
using MESWebApi.Models;
using MESWebApi.Util;
using MESWebApi.Models.QueryParm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MESWebApi.Controllers
{
    [RoutePrefix("api/user")]
    [CheckLogin]
    public class UserController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult List(UserQueryParm q)
        {
            try
            {
                int resultcount = 0;
                UserService us = new UserService();
                var list = us.Search(q, out resultcount);
                return Json(new { code = 1, msg = "ok",list = list.ToList(),resultcount = resultcount });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message, resultcount=0 });
            }
        }
        [Route("info")]
        [HttpGet]
        public IHttpActionResult Info(string token)
        {
                UserService us = new UserService();
                MenuService ms = new MenuService();
                sys_user user = us.UserInfo(token);
                return Json(new { code = 1,
                    menulist = ms.User_Menus(user.id),
                    msg = "ok",
                    user = user });
        }
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("menus")]
        [HttpGet]
        public IHttpActionResult UserMenus(int userid)
        {
            return Json(new {
            code=1,
            msg="ok",
            result=new sys_menu()
            });
        }
        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add_User(dynamic obj)
        {
            try
            {
                string userpwd = Tool.Str2MD5(obj.pwd.ToString());
                List<int> roleids = obj.roleids.ToObject<List<int>>();
                UserService us = new UserService();
                sys_user entity = new sys_user()
                {
                    status = 1,
                    addtime = DateTime.Now,
                    adduser = 1,
                    name = obj.name.ToString(),
                    code = obj.code.ToString(),
                    pwd = userpwd,
                    token = new JWTHelper().CreateToken()
                };
                entity = us.Add(entity);
                if (entity.id > 0)
                {
                    int cnt = us.SaveUserRoles(entity.id, roleids);
                    return Json(new { code = 1, msg = "ok" });
                }
                else
                {
                    return Json(new { code = 0, msg = "保存数据失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        public IHttpActionResult ChangePwd(dynamic obj)
        {
            try
            {
                int userid = 0;
                int.TryParse(obj.userid, out userid);
                string pwd = obj.userpwd.ToString();
                UserService us = new UserService();
                int cnt = us.ChangePwd(userid, pwd);
                if (cnt > 0)
                {
                    return Json(new { code = 1, msg = "修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "修改失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
            
        }

    }
}