using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.DB;
using Dapper;
using System.Threading.Tasks;
using MESWebApi.Util;
namespace MESWebApi.Services
{
    public class UserService
    {
        public UserService()
        {

        }

        public dynamic CheckUserLogin(string username,string userpwd)
        {
            using (var db = new OraDBHelper())
            {
                string token = string.Empty;
                var ret = db.Conn.Query("select 'admin' as username,'123456' as userpwd from dual ");
                if (ret.Count() > 0)
                { 
                    
                }
                var obj = ret.FirstOrDefault();
                token = Tool.DESEncrypt($"{obj.username}##{ obj.userpwd}");
                return new {token=token,isok = true};
            }
            
        }

    }
}