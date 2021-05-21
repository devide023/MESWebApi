using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using MESWebApi.DBAttr;
namespace MESWebApi.Models
{
    public class sys_user
    {
        [DbField("id")]
        public int id { get; set; }
        [DbField("status")]
        public int status { get; set; }
        [DbField("code")]
        public string code { get; set; }
        [DbField("name")]
        public string name { get; set; }
        [DbField("pwd")]
        public string pwd{ get; set; }
        public string headimg { get; set; }
        [DbField("token")]
        public string token { get; set; }
        public int adduser { get; set; }
        public DateTime? addtime { get; set; }
        
        public List<sys_role> roles { get; set; }
    }
}