using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
namespace MESWebApi.Models.QueryParm
{
    public class MenuQueryParm:sys_page
    {
        public int pid { get; set; }
        public string code { get; set; }
    }
}