using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MESWebApi.Util;
namespace MESWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ContentResult Index()
        {
            string token = new JWTHelper().CreateToken();

            return Content(token);
        }

        public ContentResult Pwd(string pwd= "")
        {
           var ret = new JWTHelper().CheckToken(pwd);
            return Content(ret.ToString());
        }
        public ContentResult Pwd1(string pwd = "")
        {
            return Content("");
        }
    }
}
