using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password{ get; set; }
        public string headimg { get; set; }
        public int companyid { get; set; }
        public int orgid { get; set; }
    }
}