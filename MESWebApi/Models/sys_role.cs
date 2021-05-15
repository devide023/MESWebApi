using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_role
    {
        public int id { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public int status { get; set; }
        public int adduser { get; set; }
        public string addusername { get; set; }
        public DateTime? updatetime { get; set; }
        public int updateuser { get; set; }
        public string updateusername { get; set; }
        public DateTime? addtime { get; set; }
        public List<sys_menu> role_menus { get; set; }
        public sys_permission permission { get; set; }
    }
}