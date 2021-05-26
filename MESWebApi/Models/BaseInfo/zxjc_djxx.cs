using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 点检记录
    /// </summary>
    public class zxjc_djxx
    {
        public string id { get; set; }
        public string gcdm { get; set; }
        public string scx { get; set; }
        public string gwh { get; set; }
        public string jx_no { get; set; }
        public string status_no { get; set; }
        public string djno { get; set; }
        public string djxx { get; set; }
        public string djjg { get; set; }
        public string bz { get; set; }
        public string lrr { get; set; }
        public string lrsj { get; set; }
    }
}