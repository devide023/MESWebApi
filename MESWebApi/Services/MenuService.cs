using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace MESWebApi.Services
{
    public class MenuService
    {
        public MenuService()
        {

        }

        public IEnumerable<sys_menu> Get_User_Menus(int userid)
        {
            string path = HttpContext.Current.Server.MapPath("~/menus.json");
            string json = File.ReadAllText(path, Encoding.UTF8);
            return JsonConvert.DeserializeObject<IEnumerable<sys_menu>>(json);
        }
    }
}