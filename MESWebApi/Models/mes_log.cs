using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Util;
using DapperExtensions.Mapper;
namespace MESWebApi.Models
{
    public class mes_log
    {
        public int id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string user_code { get; set; } = CacheManager.Instance().Current_User?.code;
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string user_name { get; set; }= CacheManager.Instance().Current_User?.name;
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
        public string czlx { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string ip { get; set; } = Tool.GetIPAddress;
        /// <summary>
        /// 日志内容
        /// </summary>
        public string rznr { get; set; }
    }

    public class mes_log_mapper : ClassMapper<mes_log>
    {
        public mes_log_mapper()
        {
            Map(t => t.id).Key(KeyType.TriggerIdentity);
            AutoMap();
        }
    }
}