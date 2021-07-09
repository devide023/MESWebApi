using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public class sys_response
    {
        /// <summary>
        /// 状态码,0:失败，1:成功
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string msg { get; set; }
    }

    public class sys_response_list<T>:sys_response where T : class, new()
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public IEnumerable<T> list { get; set; }
        /// <summary>
        /// 记录数
        /// </summary>
        public int resultcount { get; set; }
    }
}