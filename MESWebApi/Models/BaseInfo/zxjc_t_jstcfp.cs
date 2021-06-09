using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;
namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 技术通知关联生产线,分发技术通知
    /// </summary>
    public class zxjc_t_jstcfp
    {
        /// <summary>
        /// 技通文件ID
        /// </summary>
        public string jtid { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 生产线（质量班长维护）
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 岗位号（质量组长维护）
        /// </summary>
        public string gwh { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string gwmc { get; set; }
        /// <summary>
        /// 机型 (质量班长维护）
        /// </summary>
        public string jx_no { get; set; }
        /// <summary>
        /// 状态码（质量班长维护）
        /// </summary>
        public string status_no { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        /// <summary>
        /// 班长录入人
        /// </summary>
        public string lrr1 { get; set; }
        /// <summary>
        /// 班长录入时间
        /// </summary>
        public DateTime? lrsj1 { get; set; } 
        /// <summary>
        /// 组长录入人
        /// </summary>
        public string lrr2 { get; set; }
        /// <summary>
        /// 组长录入时间
        /// </summary>
        public DateTime? lrsj2 { get; set; }
    }

    public class zxjc_t_jstcfp_map : ClassMapper<zxjc_t_jstcfp>
    {
        public zxjc_t_jstcfp_map()
        {
            Map(t => t.gwmc).Ignore();
            AutoMap();
        }
    }
}