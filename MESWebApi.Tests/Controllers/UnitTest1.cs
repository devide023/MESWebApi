using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MESWebApi.Services;
using MESWebApi.Models;
using System.Collections.Generic;

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
    }
}
