using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 人员技能
    /// </summary>
    public class zxjc_ryxx_jn
    {
        /// <summary>
        /// 工厂代码
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string user_code { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 技能编号
        /// </summary>
        public string jnbh { get; set; }
        /// <summary>
        /// 技能描述
        /// </summary>
        public string jnxx { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>
        public string gwh { get; set; }
        /// <summary>
        /// 是否合格
        /// </summary>
        public string sfhg { get; set; } = "N";
        public string lrr { get; set; }
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 技能分类
        /// </summary>
        public string jnfl { get; set; }
        /// <summary>
        /// 技能时间
        /// </summary>
        public DateTime? jnsj { get; set; } = DateTime.Now;
    }

    public class zxjc_ryxx_jn_map : ClassMapper<zxjc_ryxx_jn>
    {
        public zxjc_ryxx_jn_map()
        {
            Map(t => t.user_name).Ignore();
            AutoMap();
        }
    }
}