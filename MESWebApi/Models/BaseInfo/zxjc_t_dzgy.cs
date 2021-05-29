using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 电子工艺
    /// </summary>
    public class zxjc_t_dzgy
    {
        /// <summary>
        /// 工艺文件ID（GUID）
        /// </summary>
        public string gyid { get; set; }
        /// <summary>
        /// 工艺文件编号（调服务获取）
        /// </summary>
        public string gybh { get; set; }
        /// <summary>
        /// 工艺文件名称（调服务获取）
        /// </summary>
        public string gymc { get; set; }
        /// <summary>
        /// 工艺文件描述
        /// </summary>
        public string gyms { get; set; }
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
        /// 机型
        /// </summary>
        public string jx_no { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string status_no { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string wjlj { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string jwdx { get; set; }
        /// <summary>
        /// 上传用户名
        /// </summary>
        public string scry { get; set; }
        /// <summary>
        /// 上传电脑名
        /// </summary>
        public string scpc { get; set; }
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime? scsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 版本号
        /// </summary>
        public string bbbh { get; set; }
    }
}