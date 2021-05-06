using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services;
using MESWebApi.Models;
using MESWebApi.Util;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/user")]
    [CheckLogin]
    public class UserController : ApiController
    {
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
       
    }
}