using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class sec_users
    {
        public string comp_no { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string user_type { get; set; }
        public string depart_no { get; set; }
        public string gwxx { get; set; }
        public string pass_word { get; set; }
        public string bz { get; set; }
        public string version { get; set; }
        public string version_b { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
        public string class_no { get; set; }
        public string tsqx { get; set; }
        public string scx { get; set; }
        public string work_no { get; set; }
        public string lx { get; set; }
    }
}