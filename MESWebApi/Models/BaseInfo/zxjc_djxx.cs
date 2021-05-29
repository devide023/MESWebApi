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
        /// <summary>
        /// 工厂代码
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
        /// 状态码
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
        /// <summary>
        /// 点检结果
        /// </summary>
        public string djjg { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        /// <summary>
        /// 点检人
        /// </summary>
        public string lrr { get; set; }
        public DateTime? lrsj { get; set; } = DateTime.Now;
    }
}