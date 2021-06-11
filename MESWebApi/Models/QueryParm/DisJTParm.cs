using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.QueryParm
{
    /// <summary>
    /// 技通分配查询参数
    /// </summary>
    public class DisJTParm:sys_page
    {
        public string jcbh { get; set; }
        public string jcmc { get; set; }
        public string jcms { get; set; }
        public string wjlj { get; set; }
        public string sffp { get; set; }
    }
}