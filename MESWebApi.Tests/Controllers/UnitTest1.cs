using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MESWebApi.Services;
using MESWebApi.Models;
using MESWebApi.Models.QueryParm;
using MESWebApi.Util;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace MESWebApi.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void getmd5()
        {
           var pwd = Tool.Str2MD5("123456");
           var token = new JWTHelper().CreateToken();
            System.Console.WriteLine(token);
        }
        [TestMethod]
        public void SearchMenu()
        {
            int cnt = 0;
            MenuQueryParm p = new MenuQueryParm();
            MenuService ms = new MenuService();
            var list = ms.Search(p, out cnt);
            var count = list.Count();
            System.Console.WriteLine(count);
        }
        [TestMethod]
        public void MenuTree()
        {
            string json= "{\"funs\":[\"add\",\"edit\",\"del\",\"query\"],\"editfields\":[\"name\"],\"readfields\":[\"code\"],\"hidefields\":[\"pwd\"],\"showfield\":[\"code\",\"name\"]}";
            sys_permission obj = JsonConvert.DeserializeObject<sys_permission>(json);
        }
        [TestMethod]
        public void checklogin()
        {
            UserService us = new UserService();
            var token = us.CheckUserLogin("001", "123456");
        }
        [TestMethod]
        public void TestMethod1()
        {
            UserService us = new UserService();
            us.Add(new sys_user()
            {
                name="admin",
                code="001",
                pwd="123456",
                status=1,
                adduser=1,
                addtime=DateTime.Now,
                
            });
        }
        [TestMethod]
        public void freshtoken()
        {
            UserService us = new UserService();
            us.FreshToken(1);
        }
        [TestMethod]
        public void login()
        {
            UserService us = new UserService();
            string token = us.CheckUserLogin("001", "123456");
            System.Console.WriteLine(token);
        }
        [TestMethod]
        public void search_user()
        {
            int cnt = 0;
            UserService us = new UserService();
            var list = us.Search(new Models.QueryParm.UserQueryParm() { }, out cnt);
            
        }
        [TestMethod]
        public void add_user_role()
        {
            UserService us = new UserService();
            List<int> roleids = new List<int>();
            roleids.Add(1);
            roleids.Add(5);
            roleids.Add(6);
            us.SaveUserRoles(1, roleids);
        }

        [TestMethod]
        public void add_user_menus()
        {
            RoleService rs = new RoleService();
            List<int> menuids = new List<int>();
            menuids.Add(1);
            menuids.Add(2);
            menuids.Add(3);
            menuids.Add(4);
            rs.Save_RoleMenus(1, menuids);
        }

        [TestMethod]
        public void user_menus()
        {
            MenuService ms = new MenuService();
            var list = ms.User_Menus(1);
            string json = JsonConvert.SerializeObject(list);
            System.Console.WriteLine(json);
        }

        [TestMethod]
        public void Icons()
        {
            int cnt = 0;
            MenuService ms = new MenuService();
            var t = ms.MenuTree(new MenuQueryParm() { 
                keyword="角色"
            },out cnt);
            Console.WriteLine(JsonConvert.SerializeObject(t));
        }

        }
}
