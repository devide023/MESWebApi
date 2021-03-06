using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;
namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 技术通知
    /// </summary>
    public class zxjc_t_jstc
    {
        /// <summary>
        /// 技通ID(GUID)
        /// </summary>
        public string jtid { get; set; }
        /// <summary>
        /// 技术通知编号（调用服务获取）
        /// </summary>
        public string jcbh { get; set; }
        /// <summary>
        /// 技术通知名称（调用服务获取）
        /// </summary>
        public string jcmc { get; set; }
        /// <summary>
        /// 文件描述（手输）
        /// </summary>
        public string jcms { get; set; }
        /// <summary>
        /// 文件路径（返回值）
        /// </summary>
        public string wjlj { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long jwdx { get; set; }
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
        /// 有效期限-开始
        /// </summary>
        public DateTime? yxqx1 { get; set; }
        /// <summary>
        /// 有效期限-结束
        /// </summary>
        public DateTime? yxqx2 { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string gcdm { get; set; }
        /// <summary>
        /// 分配标志
        /// </summary>
        public string fp_flg { get; set; }
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? fp_sj { get; set; }
        /// <summary>
        /// 分配人
        /// </summary>
        public string fpr { get; set; }
        /// <summary>
        /// 文件分类
        /// </summary>
        public string wjfl { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        public string scx { get; set; }
        /// <summary>
        /// 技通关联分配详情
        /// </summary>
        public List<zxjc_t_jstcfp> details { get; set; } = new List<zxjc_t_jstcfp>();
    }

    public class zxjc_t_jstc_mapper : ClassMapper<zxjc_t_jstc>
    {
        public zxjc_t_jstc_mapper()
        {
            Map(t => t.details).Ignore();
            AutoMap();
        }
    }
}