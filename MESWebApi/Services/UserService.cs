using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.DB;
using Dapper;
using System.Threading.Tasks;
using MESWebApi.Util;
using MESWebApi.Models;

namespace MESWebApi.Services
{
    public class UserService
    {
        public UserService()
        {

        }

        public string CheckUserLogin(string username, string userpwd)
        {
            string token = string.Empty;
            token = new JWTHelper().CreateToken();
            return token;
        }

        public sys_user UserInfo(string token)
        {
            string imgurl = "http://"+HttpContext.Current.Request.Url.Authority + "/Images/headimg/default.jpg";
            return new sys_user() {
            username="admin",
            password="ddd",
            headimg = imgurl
            };
        }

    }
}