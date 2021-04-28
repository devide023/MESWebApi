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
        public async Task<IHttpActionResult> checklogin(sys_user obj)
        {
            UserService us = new UserService();
            var token = await us.CheckUserLogin(obj.name, obj.pwd);
            return Json(new { token=token});
        }
        [HttpGet]
        public IHttpActionResult get_user()
        {
            List<sys_user> list = new List<sys_user>();
            list.Add(new sys_user() { name = "admin", pwd = "123456" });
            list.Add(new sys_user() { name = "test", pwd = "456789" });
            return Json(list);
        }
    }
}