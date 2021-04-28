using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using MESWebApi.Models;
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
            return Json(new { name= obj.name,pwd= obj.pwd});
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