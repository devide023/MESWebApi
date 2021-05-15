using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_permission
    {
        public List<string> funs { get; set; } = new List<string>();
        public List<string> editfields { get; set; } = new List<string>();
        public List<string> hidefields { get; set; } = new List<string>();
    }
}