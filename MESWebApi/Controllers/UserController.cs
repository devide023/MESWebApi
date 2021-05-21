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
                int adduserid = 0,status=1;
                int.TryParse(obj.adduser!=null?obj.adduser.ToString():"0", out adduserid);
                int.TryParse(obj.status!=null?obj.status.ToString():"1", out status);
                string userpwd = Tool.Str2MD5(obj.pwd.ToString());
                List<int> roleids = obj.roleids!=null?obj.roleids.ToObject<List<int>>():new List<int>();
                UserService us = new UserService();
                sys_user entity = new sys_user()
                {
                    status = status,
                    addtime = DateTime.Now,
                    adduser = adduserid,
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
        [Route("edit")]
        [HttpPost]
        [LogActionFilter]
        public IHttpActionResult Edit_User(dynamic obj) {
            try
            {
                int userid = 0,adduserid=0;
                int.TryParse(obj.id!=null?obj.id.ToString():"0", out userid);
                int.TryParse(obj.adduser!=null?obj.adduser.ToString():"0", out adduserid);
                List<int> roleids = obj.roleids!=null?obj.roleids.ToObject<List<int>>():new List<int>();
                UserService us = new UserService();
                if (userid > 0)
                {
                    sys_user entity = new sys_user()
                    {
                        id = userid,
                        adduser = adduserid,
                        name = obj.name.ToString(),
                        code = obj.code.ToString()
                    };
                    int cnt = us.Modify(entity);
                    if (cnt > 0)
                    {
                        cnt = us.SaveUserRoles(entity.id, roleids);
                        return Json(new { code = 1, msg = "数据保存成功" });
                    }
                    else
                    {
                        return Json(new { code = 0, msg = "修改用户信息失败" });
                    }
                }
                else
                {
                    return Json(new { code = 0, msg = "修改用户信息失败" });
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
        [Route("find")]
        [HttpGet]
        public IHttpActionResult UserEntity(int id)
        {
            try
            {
                UserService us = new UserService();
                sys_user entity = us.Find(id);
                return Json(new { code = 1, msg = "ok",user = entity });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("resetpwd")]
        [HttpPost]
        public IHttpActionResult ResetPwd(dynamic obj)
        {
            try
            {
                UserService us = new UserService();
                int userid = 0;
                int.TryParse(obj.id != null ? obj.id.ToString() : "0", out userid);
                int ret = us.ChangePwd(userid, obj.pwd.ToString());
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "重置密码成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "重置密码失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("enable")]
        [HttpPost]
        public IHttpActionResult UserEnable(dynamic obj)
        {
            try
            {
                List<int> ids = obj.ids != null ? obj.ids.ToObject<List<int>>() : new List<int>();
                UserService us = new UserService();
                int ret = us.EnableUser(ids);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据操作成功" });
                }
                else {
                    return Json(new { code = 0, msg = "数据操作失败" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("disable")]
        [HttpPost]
        public IHttpActionResult UserDisable(dynamic obj)
        {
            try
            {
                List<int> ids = obj.ids != null ? obj.ids.ToObject<List<int>>() : new List<int>();
                UserService us = new UserService();
                int ret = us.DisableUser(ids);
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

        [Route("keyfind")]
        [HttpPost]
        public IHttpActionResult FindUserByName(dynamic obj)
        {
            try
            {
                UserService us = new UserService();
                var list = us.FindUserByName(obj.key!=null?obj.key.ToString():"");
                return Json(new { code = 1, msg = "ok",list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}