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
            token = Tool.DESEncrypt($"{username}##{ userpwd}");
            return token;
        }

        public sys_user UserInfo(string token)
        {
            return new sys_user() {
            username="admin",
            password="ddd",
            token = "abcdefghijk"
            };
        }

    }
}