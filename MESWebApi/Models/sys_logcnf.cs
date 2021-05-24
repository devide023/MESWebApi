using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_logcnf
    {
        public List<string> include { get; set; }
        public List<string> except { get; set; }
    }
}