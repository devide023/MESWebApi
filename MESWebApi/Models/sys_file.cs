using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_file
    {
        public string fileid { get; set; }
        public string filename { get; set; }
        public long filesize { get; set; }
    }
}