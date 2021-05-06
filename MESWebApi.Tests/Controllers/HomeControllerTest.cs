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
            List<sys_menu> list = new List<sys_menu>();
            list.Add(new sys_menu()
            {
                pid = 0,
                title = "系统管理",
                icon = "dashboard",
                code = "systemmgr",
                menutype = "01",
                path = "/systemmgr",
                viewpath = "",
                seq = 2,
                adduser = 1
            });
            list.Add(new sys_menu()
            {
                pid = 1,
                title = "用户管理",
                icon = "user",
                code = "usermgr",
                menutype = "02",
                path = "usermgr",
                viewpath = "user/index",
                seq = 3,
                adduser = 1
            });
            list.Add(new sys_menu()
            {
                pid = 1,
                title = "角色管理",
                icon = "table",
                code = "rolemgr",
                menutype = "01",
                path = "rolemgr",
                viewpath = "role/index",
                seq = 4,
                adduser = 1
            });
            list.Add(new sys_menu()
            {
                pid = 1,
                title = "菜单管理",
                icon = "tree",
                code = "menumgr",
                menutype = "01",
                path = "menumgr",
                viewpath = "menu/index",
                seq = 5,
                adduser = 1
            });

            foreach (var item in list)
            {
                var ret = service.Add(item);
                System.Console.WriteLine(ret.id);
            }
            
        }

        [TestMethod]

        public void addrole()
        {
            RoleService rs = new RoleService();
            rs.Add(new sys_role
            {
                title = "资料查看",
                status = 1,
                adduser = 1,
                code = "04",
                addtime = System.DateTime.Now
            }) ;
            
        }
        [TestMethod]
        public void queryrole()
        {
            int cnt = 0;
            var p = new sys_page()
            {
                keyword="管理",
                pageindex=1,
                pagesize = 20
            };
            RoleService rs = new RoleService();
            var list = rs.Find(p, out cnt);

        }
        [TestMethod]
        public void delrole()
        {
            RoleService rs = new RoleService();
            List<int> ids = new List<int>();
            ids.Add(2);
            ids.Add(3);
            rs.Delete(ids);
        }
        [TestMethod]
        public void modifyrole()
        {
            RoleService rs = new RoleService();
            sys_role role = new sys_role()
            {
                id = 1,
                title = "sysadmin",
                status = 1
            };
            rs.Modify(role);
        }
    }
}
