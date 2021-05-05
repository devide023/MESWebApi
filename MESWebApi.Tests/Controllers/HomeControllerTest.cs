using MESWebApi;
using MESWebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MESWebApi.Models;
using MESWebApi.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace MESWebApi.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            MenuService service = new MenuService();
            //List<sys_menu> list = new List<sys_menu>();
            //list.Add(new sys_menu()
            //{
            //    pid=0,
            //    title = "系统管理",
            //    icon = "dashboard",
            //    action = "",
            //    controller = "",
            //    code = "systemmgr",
            //    menutype = "01",
            //    path = "/systemmgr",
            //    viewpath = "",
            //    seq = 2,
            //    adduser = 1
            //});
            //list.Add(new sys_menu()
            //{
            //    pid = 1,
            //    title = "用户管理",
            //    icon = "user",
            //    action = "",
            //    controller = "",
            //    code = "usermgr",
            //    menutype = "02",
            //    path = "usermgr",
            //    viewpath = "user/index",
            //    seq = 3,
            //    adduser = 1
            //});
            //list.Add(new sys_menu()
            //{
            //    pid = 1,
            //    title = "角色管理",
            //    icon = "table",
            //    action = "",
            //    controller = "",
            //    code = "rolemgr",
            //    menutype = "01",
            //    path = "rolemgr",
            //    viewpath = "role/index",
            //    seq = 4,
            //    adduser = 1
            //});
            //list.Add(new sys_menu()
            //{
            //    pid = 1,
            //    title = "菜单管理",
            //    icon = "tree",
            //    action = "",
            //    controller = "",
            //    code = "menumgr",
            //    menutype = "01",
            //    path = "menumgr",
            //    viewpath = "menu/index",
            //    seq = 5,
            //    adduser = 1
            //});
           
                sys_menu cnt = service.Add_Menu(new sys_menu() {
                    pid = 0,
                    title = "报表管理",
                    icon = "table",
                    action = "",
                    controller = "",
                    code = "reportmgr",
                    menutype = "01",
                    path = "/reportmgr",
                    viewpath = "report/index",
                    seq = 6,
                    adduser = 1
                });
                System.Console.WriteLine(cnt.id);
            
        }

        [TestMethod]

        public void Query()
        {
            MenuService s = new MenuService();
            var list = s.User_Menus(0);
            string json = JsonConvert.SerializeObject(list);
            
                System.Console.WriteLine(json);
            
        }
    }
}
