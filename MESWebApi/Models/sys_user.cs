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
        [DbField("id","id")]
        public int id { get; set; }
        [DbField("status","状态")]
        public int status { get; set; }
        [DbField("code","编码")]
        public string code { get; set; }
        [DbField("name","姓名")]
        public string name { get; set; }
        [DbField("pwd","密码")]
        public string pwd{ get; set; }
        public string headimg { get; set; }
        [DbField("token","Token")]
        public string token { get; set; }
        public int adduser { get; set; }
        [DbField("addtime","操作日期")]
        public DateTime? addtime { get; set; }
        
        public List<sys_role> roles { get; set; }
    }
}