using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using MESWebApi.Models;
using MESWebApi.Services;
using System.Web.Http.Results;
using System.Net.Http;
using MESWebApi.Util;
namespace MESWebApi.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Route("checklogin")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult checklogin(sys_user obj)
        {
            try
            {
                UserService us = new UserService();
                var token = us.CheckUserLogin(obj.code, obj.pwd);
                if (!string.IsNullOrEmpty(token))
                {
                    return Json(new { code = 1, msg = "ok", token = token });

                }
                else
                {
                    return Json(new { code = 0, msg = "用户名或密码错误", token = token });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message, token = "" });
            }
        }
        [Route("logout")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            sys_user cur_user = CacheManager.Instance().Current_User;
            return Json(new { code = 1, msg = "ok",user = cur_user });
        }
    }
}