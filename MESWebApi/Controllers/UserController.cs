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
    /// <summary>
    /// 用户接口（sys_user）
    /// </summary>
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="q">UserQueryParm</param>
        /// <returns></returns>
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
        /// <summary>
        /// 根据token查询用户信息
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
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
        /// <param name="userid">用户id</param>
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
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="obj">
        /// 例：{sys_user实体}
        /// </param>
        /// <returns></returns>
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
                if (us.IsExsitCode(entity.code))
                {
                    return Json(new { code = 0, msg = "用户编码已存在" });
                }
                entity = us.Add(entity);
                if (entity.id > 0)
                {
                    int cnt = us.SaveUserRoles(entity.id, roleids);
                    return Json(new { code = 1, msg = "数据保存成功" });
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
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("edit")]
        [HttpPost]
        public IHttpActionResult Edit_User(dynamic obj) {
            try
            {
                int userid = 0;
                int.TryParse((obj.id??"0").ToString(), out userid);
                List<int> roleids = obj.roleids!=null?obj.roleids.ToObject<List<int>>():new List<int>();
                UserService us = new UserService();
                if (userid > 0)
                {
                    sys_user entity = new sys_user()
                    {
                        id = userid,
                        name = obj.name.ToString()
                    };
                    sys_user orginal = us.Find(entity.id);
                    int cnt = us.Modify(entity);
                    if (cnt > 0)
                    {
                        cnt = us.SaveUserRoles(entity.id, roleids);
                        entity = us.Find(entity.id);
                        us.LogS.UpdateLogJson<sys_user>(entity, orginal);
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
        /// <summary>
        /// 修改当前登录人的密码
        /// </summary>
        /// <param name="obj">
        /// {pwd:'',}
        /// </param>
        /// <returns></returns>
        [Route("chpwd")]
        [HttpPost]
        public IHttpActionResult ChangePwd(dynamic obj)
        {
            try
            {
                string pwd = (obj.pwd ?? "").ToString();
                UserService us = new UserService();
                sys_user user = CacheManager.Instance().Current_User;
                int cnt = us.ChangePwd(user.id, pwd);
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
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
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
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="obj">
        /// {id:1,pwd:''}
        /// </param>
        /// <returns></returns>
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
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 关键字查找用户
        /// </summary>
        /// <param name="obj">
        /// 查找列：用户名，用户编码，列：{key:''}
        /// </param>
        /// <returns></returns>
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