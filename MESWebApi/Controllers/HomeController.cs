using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MESWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ContentResult Index()
        {
            string token = MESWebApi.Util.Tool.DESEncrypt("abcd##123456");

            string token1 = "";

            return Content(token+"<br/>"+token1);
        }

        public ContentResult Pwd(string pwd= "")
        {
            string mm = MESWebApi.Util.Tool.DESDecrypt(pwd);
            return Content(mm);
        }
        public ContentResult Pwd1(string pwd = "")
        {
            return Content("");
        }
    }
}
