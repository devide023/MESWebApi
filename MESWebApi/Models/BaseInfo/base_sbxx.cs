using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class base_sbxx
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string sbbh { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string sbmc { get; set; }
        /// <summary>
        /// 工厂代码
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 岗位号
        /// </summary>
        public string gwh { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string sblx { get; set; }
        /// <summary>
        /// 通信方式
        /// </summary>
        public string txfs { get; set; }
        public string ip { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public string sfky { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public string sflj { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        public string lrr { get; set; }
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string com { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public string port { get; set; }
    }
}