using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;
namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 特殊技术通知
    /// </summary>
    public class zxjc_t_tstc
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public string tcid { get; set; }
        /// <summary>
        /// 通知编号
        /// </summary>
        public string tcbh { get; set; }
        /// <summary>
        ///  通知内容
        /// </summary>
        public string tcms { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 岗位编码
        /// </summary>
        public string gwh { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string gwmc { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string jx_no { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string status_no { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>
        public string yxbz { get; set; } = "Y";
        /// <summary>
        /// 录入人
        /// </summary>
        public string lrr { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? lrsj { get; set; } = DateTime.Now;
    }

    public class zxjc_t_tstc_mapper : ClassMapper<zxjc_t_tstc>
    {
        public zxjc_t_tstc_mapper()
        {
            Map(t => t.gwmc).Ignore();
            AutoMap();
        }
    }
}