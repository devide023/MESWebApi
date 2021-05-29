using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MESWebApi.Services;
using MESWebApi.Models;
using MESWebApi.Models.QueryParm;
using MESWebApi.Util;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using MESWebApi.Services.BaseInfo;
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
            string json = "{\"funs\":[\"add\",\"edit\",\"del\",\"query\"],\"editfields\":[\"name\"],\"readfields\":[\"code\"],\"hidefields\":[\"pwd\"],\"showfield\":[\"code\",\"name\"]}";
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
                name = "admin",
                code = "001",
                pwd = "123456",
                status = 1,
                adduser = 1,
                addtime = DateTime.Now,

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
            var t = ms.MenuTree(new MenuQueryParm()
            {
                keyword = "角色"
            }, out cnt);
            Console.WriteLine(JsonConvert.SerializeObject(t));
        }

        [TestMethod]
        public void RoleInfo()
        {
            RoleService rs = new RoleService();
            var list = rs.Get_Role_Menus(1);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void PermisTree()
        {
            MenuService ms = new MenuService();
            var list = ms.PermissionTree();
            var json = JsonConvert.SerializeObject(list);
            System.Console.WriteLine(json);
        }

        [TestMethod]
        public void Modify()
        {
           string path = HttpContext.Current.Request.Path;
            Console.WriteLine(path);
        }

        [TestMethod]
        public void Ry()
        {
            UserSkillService uss = new UserSkillService();
            uss.Add(new Models.BaseInfo.zxjc_ryxx_jn()
            {
                user_code="001",
                gcdm="9200",
                jnbh="001"
            });
        }

        [TestMethod]
        public void device() {
            PointCheckInfoService pcis = new PointCheckInfoService();
            List<Models.BaseInfo.zxjc_djxx> list = new List<Models.BaseInfo.zxjc_djxx>();
            list.Add(new Models.BaseInfo.zxjc_djxx()
            {
                id = "2",
                djno = "001",
                jx_no = "ddd",
                bz = "bz",
                djjg = "djjg",
                djxx = "djxx",
                gcdm = "9100",
                gwh = "A01",
                lrr = "admin",
                lrsj = DateTime.Now,
                scx = "1",
                status_no = "1"
            });
            list.Add(new Models.BaseInfo.zxjc_djxx()
            {
                id = "3",
                djno = "001",
                jx_no = "ddd",
                bz = "bz",
                djjg = "djjg",
                djxx = "djxx",
                gcdm = "9100",
                gwh = "A01",
                lrr = "admin",
                lrsj = DateTime.Now,
                scx = "1",
                status_no = "1"
            });
            list.Add(new Models.BaseInfo.zxjc_djxx()
            {
                id = "4",
                djno = "001",
                jx_no = "ddd",
                bz = "bz",
                djjg = "djjg",
                djxx = "djxx",
                gcdm = "9100",
                gwh = "A01",
                lrr = "admin",
                lrsj = DateTime.Now,
                scx = "1",
                status_no = "1"
            });
            pcis.Add(list);
        }
        [TestMethod]
        public void update()
        {
            PointCheckInfoService pcis = new PointCheckInfoService();
            pcis.Modify(new Models.BaseInfo.zxjc_djxx()
            {
                id = "4",
                djno = "ddd",
                jx_no = "ddd",
                bz = "ddd",
                djjg = "aa",
                djxx = "aaa",
                gcdm = "ddd",
                gwh = "ddd",
                lrr = "ddd",
                scx = "ddd",
                status_no = "ddd"
            });
        }
        [TestMethod]
        public void JTInsert()
        {
            JTService jts = new JTService();
            jts.OracleInsert(new Models.BaseInfo.zxjc_t_jstc()
            {
                jtid = Guid.NewGuid().ToString(),
                jcbh = "001",
                jcmc = "关于×××的通知",
                jcms = "描述",
                jwdx = "100",
                fpr = "a",
                fp_flg = "Y",
                fp_sj = DateTime.Now,
                gcdm = "9100",
                scpc = "pc",
                scry = "admin",
                scsj = DateTime.Now,
                scx = "1",
                wjfl = "fdp",
                wjlj = "upload/",
                yxqx1 = DateTime.Now,
                yxqx2 = DateTime.Now.AddDays(5)
            }) ;
        }
        }
}
