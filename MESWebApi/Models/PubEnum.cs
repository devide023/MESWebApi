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
            登录,
            新增,
            修改,
            删除,
            批量插入,
            批量修改,
            批量删除
        }
    }
}