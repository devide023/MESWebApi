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
    public class LoginController : ApiController
    {
        // GET: Login
        [HttpPost]
        public IHttpActionResult checklogin(sys_user obj)
        {
            UserService us = new UserService();
            var token = us.CheckUserLogin(obj.username, obj.password);
            return Json(new { code = 1, msg = "ok", token = token });
        }
        [HttpGet]
        public IHttpActionResult get_user()
        {
            List<sys_user> list = new List<sys_user>();
            list.Add(new sys_user() { username = "admin", password = "123456" });
            list.Add(new sys_user() { username = "test", password = "456789" });
            return Json(list);
        }
    }
}