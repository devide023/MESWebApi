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
    /// <summary>
    /// 系统登录退出接口
    /// </summary>
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// 登录，接收用户编码，用户密码两个参数
        /// </summary>
        /// <param name="obj">sys_user实体对象</param>
        /// <returns>成功返回Token值</returns>
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
        /// <summary>
        /// 退出，传Token值
        /// </summary>
        /// <param name="obj">Token</param>
        /// <returns>退出成功后会刷新Token对应用户的Token</returns>
        [Route("logout")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Logout(dynamic obj)
        {
            try
            {
                string token = (obj.token ?? "").ToString();
                UserService us = new UserService();
                var cnt = us.LogOut(token);
                if (cnt > 0)
                {
                    return Json(new { code = 1, msg = "成功退出" });
                }
                else
                {
                    return Json(new { code = 0, msg = "退出失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}