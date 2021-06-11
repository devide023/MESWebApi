using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;
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
        /// 岗位名称
        /// </summary>
        public string gwmc { get; set; }
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
        /// <summary>
        /// 删除标志
        /// </summary>
        public string scbz { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string lrr { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 点检分类
        /// </summary>
        public string djlx { get; set; }
    }

    public class zxjc_djgw_mapper : ClassMapper<zxjc_djgw>
    {
        public zxjc_djgw_mapper()
        {
            Map(t => t.gwmc).Ignore();
            AutoMap();
        }
    }
}