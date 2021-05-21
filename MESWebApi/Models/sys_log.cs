using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_log
    {
        public string tablename { get; set; }
        public Dictionary<string, object> fields { get; set; }
        public string querysql { get; set; }
        public string insertsql { get; set; }
    }
}