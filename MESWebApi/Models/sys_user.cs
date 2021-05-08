using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_user
    {
        public int id { get; set; }
        public int status { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string pwd{ get; set; }
        public string headimg { get; set; }
        public string token { get; set; }
        public int adduser { get; set; }
        public DateTime? addtime { get; set; }
        public List<sys_role> roles { get; set; }
    }
}