using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_page
    {
        public string keyword { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int pageindex { get; set; } = 1;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int pagesize { get; set; } = 20;
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int resultcount { get; set; }
    }
}