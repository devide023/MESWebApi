using Newtonsoft.Json;
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
        public int status { get; set; }
        public string path { get; set; }
        public string viewpath { get; set; }
        public int seq { get; set; }
        public int adduser { get; set; }
        public DateTime? addtime { get; set; }
        public bool hasChildren { get; set; }
        public List<sys_menu> children { get; set; } = new List<sys_menu>();
        [JsonIgnore]
        public string permission { get; set; }
        public sys_permission menu_permission { get; set; }
        public List<sys_role> roles { get; set; } = new List<sys_role>();

    }
}