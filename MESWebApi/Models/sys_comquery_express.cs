using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    /// <summary>
    /// 组合查询实体
    /// </summary>
    public class sys_comquery_express
    {
        public string colname { get; set; }
        public string left { get; set; }
        public string logic { get; set; }
        public string oper { get; set; }
        public string right { get; set; }
        public string rowno { get; set; }
        public string value { get; set; }
    }
}