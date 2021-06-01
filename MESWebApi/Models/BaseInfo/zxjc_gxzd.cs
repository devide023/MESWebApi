using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 工序站点、岗位站点
    /// </summary>
    public class zxjc_gxzd
    {
        /// <summary>
        /// 岗位编号
        /// </summary>
        public string work_no { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string work_name { get; set; }
        /// <summary>
        /// 站点类型
        /// </summary>
        public string lx { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string lrr { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 审核标志
        /// </summary>
        public string shbz { get; set; } = "Y";
    }
}