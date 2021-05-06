using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MESWebApi.Services;
using MESWebApi.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace MESWebApi.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
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
        public void login()
        {
            UserService us = new UserService();
            string token = us.CheckUserLogin("001", "123456");
            System.Console.WriteLine(token);
        }

        [TestMethod]
        public void add_user_role()
        {
            UserService us = new UserService();
            List<int> roleids = new List<int>();
            roleids.Add(1);
            roleids.Add(2);
            roleids.Add(3);
            roleids.Add(4);
            us.SaveUserRoles(1, roleids);
        }

        [TestMethod]
        public void add_user_menus()
        {
            RoleService rs = new RoleService();
            List<int> menuids = new List<int>();
            menuids.Add(5);
            menuids.Add(6);
            menuids.Add(7);
            menuids.Add(8);
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

        }
}
