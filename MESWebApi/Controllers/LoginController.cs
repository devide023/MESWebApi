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

namespace MESWebApi.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Route("checklogin")]
        [HttpPost]
        public IHttpActionResult checklogin(sys_user obj)
        {
            try
            {
                UserService us = new UserService();
                var token = us.CheckUserLogin(obj.code, obj.pwd);
                return Json(new { code = 1, msg = "ok", token = token });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message, token = "" });
            }
        }
        [Route("logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            var authorization = HttpContext.Current.Request.Headers["Authorization"];

            return Json(new { code = 1, msg = "ok",auth = authorization });
        }
    }
}