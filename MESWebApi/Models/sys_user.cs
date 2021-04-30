using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_user
    {
        public string username { get; set; }
        public string password{ get; set; }
        public string token { get; set; }
    }
}