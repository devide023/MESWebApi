using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_role_menu
    {
        public int roleid { get; set; }
        public int menuid { get; set; }
        public string permis { get; set; }
    }
}