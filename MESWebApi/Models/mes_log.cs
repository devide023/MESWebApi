using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Util;
namespace MESWebApi.Models
{
    public class mes_log
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string user_code { get; set; } = CacheManager.Instance().Current_User.code;
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string user_name { get; set; }= CacheManager.Instance().Current_User.name;
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime czrq { get; set; } = DateTime.Now;
        /// <summary>
        /// 操作菜单
        /// </summary>
        public string czcd { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public PubEnum.CZLX czlx { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string ip { get; set; } = Tool.GetIPAddress;
        /// <summary>
        /// 日志内容
        /// </summary>
        public string rznr { get; set; }
    }
}