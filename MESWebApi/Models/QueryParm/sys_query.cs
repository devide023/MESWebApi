using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.QueryParm
{
    public class sys_query
    {
        public string left { get; set; }
        public string colname { get; set; }
        public string oper { get; set; }
        public string value { get; set; }
        public string logic { get; set; }
        public string right { get; set; }
    }
}