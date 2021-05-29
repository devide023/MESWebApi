using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 技术通知查看记录
    /// </summary>
    public class zxjc_t_ydjl
    {
        /// <summary>
        /// 查看ID（GUID）
        /// </summary>
        public string ydid { get; set; }
        /// <summary>
        /// 技通文件ID(其它ID)
        /// </summary>
        public string jtid { get; set; }
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
        /// 生产订单号
        /// </summary>
        public string order_no { get; set; }
        /// <summary>
        /// 机号（完整）
        /// </summary>
        public string engine_no { get; set; }
        /// <summary>
        /// 查看人员姓名
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 查看时间
        /// </summary>
        public DateTime? ydsj { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string lrr { get; set; }
        /// <summary>
        ///  录入时间
        /// </summary>
        public DateTime? lrsj { get; set; } = DateTime.Now;
    }
}