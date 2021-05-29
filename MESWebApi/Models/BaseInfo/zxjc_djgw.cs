using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 点检岗位
    /// </summary>
    public class zxjc_djgw
    {
        /// <summary>
        /// 工厂代号
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>
        public string gwh { get; set; }
        /// <summary>
        /// 机型编号
        /// </summary>
        public string jx_no { get; set; }
        /// <summary>
        /// 状态编号
        /// </summary>
        public string status_no { get; set; }
        /// <summary>
        /// 点检编号
        /// </summary>
        public string djno { get; set; }
        /// <summary>
        /// 点检内容
        /// </summary>
        public string djxx { get; set; }
        public string scbz { get; set; }
        public string lrr { get; set; }
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 点检分类
        /// </summary>
        public string djlx { get; set; }
    }
}