using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;
namespace MESWebApi.Models.BaseInfo
{
    /// <summary>
    /// 岗位物料
    /// </summary>
    public class base_gwbj
    {
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
        /// 岗位名称
        /// </summary>
        public string gwmc { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string wlbm { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string wlmc { get; set; }
        /// <summary>
        /// 料箱判断（N:不验证,Y:首台,A:每台）
        /// </summary>
        public string lxpd { get; set; } = "N";
        /// <summary>
        /// 料箱类型
        /// </summary>
        public string lxlx { get; set; }
        /// <summary>
        /// 单箱数量
        /// </summary>
        public int dxsl { get; set; }
        /// <summary>
        /// 岗位配比
        /// </summary>
        public decimal gwpb { get; set; }
        /// <summary>
        /// 前五位编码
        /// </summary>
        public string qwwbm { get; set; }
        /// <summary>
        /// 物料属性（A：大件,B:小件）
        /// </summary>
        public string wlsx { get; set; }
        /// <summary>
        /// 暂存区料道号
        /// </summary>
        public string zcqld { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public string psfs { get; set; }
        /// <summary>
        /// 辊道编号
        /// </summary>
        public string gdbh { get; set; }
        /// <summary>
        /// 辊道层
        /// </summary>
        public string gdc { get; set; }
        /// <summary>
        /// 最高线边库
        /// </summary>
        public int zgkc { get; set; }
        /// <summary>
        /// 是否打印
        /// </summary>
        public string sfdy { get; set; } = "Y";
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        /// <summary>
        /// 录入员
        /// </summary>
        public string lrr { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>
        public DateTime? lrsj { get; set; } = DateTime.Now;
        /// <summary>
        /// 机型
        /// </summary>
        public string jx_no { get; set; }
        /// <summary>
        /// 工作中心
        /// </summary>
        public string gzzx { get; set; } = "";
    }

    public class base_gwbj_mapper : ClassMapper<base_gwbj>
    {
        public base_gwbj_mapper()
        {
            Map(t => t.gwmc).Ignore();
            Map(t => t.wlmc).Ignore();
            AutoMap();
        }
    }
}