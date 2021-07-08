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
            return Content("欢迎使用");
        }

        public ContentResult Pwd(string pwd= "")
        {
            return Content("");
        }
        public ContentResult Pwd1(string pwd = "")
        {
            return Content("");
        }
    }
}
