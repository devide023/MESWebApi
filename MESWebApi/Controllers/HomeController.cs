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
        public JsonResult Token()
        {
            try
            {
                var newtoken = new JWTHelper().CreateToken();
                return Json(new { code = 1, msg = "ok", token = newtoken }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
