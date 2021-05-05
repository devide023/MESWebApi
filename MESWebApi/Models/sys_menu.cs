using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_menu
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string menutype { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string icon { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string path { get; set; }
        public string viewpath { get; set; }
        public int seq { get; set; }
        public int adduser { get; set; }
        public List<sys_menu> children { get; set; }
        public List<string> funs { get; set; }

    }
}