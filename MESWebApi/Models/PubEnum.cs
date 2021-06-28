using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models
{
    public static class PubEnum
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public enum CZLX { 
            登录=1,
            新增=2,
            修改=3,
            删除=4
        }
    }
}