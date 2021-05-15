using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_menu_permission
    {
        public int roleid { get; set; }
        public int menuid { get; set; }
        public sys_permission permission { get; set; }
    }
}