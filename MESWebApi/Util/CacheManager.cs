using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Util
{

    public class CacheManager
    {
        private static Dictionary<String, Object> cache = new Dictionary<string, object>();
        private static CacheManager cacheManager = null;
        /// <summary>
        /// 程序运行时，创建一个静态只读的进程辅助对象
        /// </summary>
        private static readonly object _object = new object();
        /// <summary>
        /// Make sure the class is singleton so only one instance is shared by all.
        /// </summary>
        private CacheManager()
        {
            cache = new Dictionary<string, object>();
        }
        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        /// <returns></returns>
        public static CacheManager Instance()
        {
            //先判断实例是否存在，不存在再加锁处理
            if (cacheManager == null)
            {
                //在同一时刻加了锁的那部分程序只有一个线程可以进入，
                lock (_object)
                {
                    //如实例不存在，则New一个新实例，否则返回已有实例
                    if (cacheManager == null)
                    {
                        cacheManager = new CacheManager();
                    }
                }
            }
            return cacheManager;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void add(String key, Object value)
        {
            if (!cache.ContainsKey(key))
            { 
                cache.Add(key, value);
            }
            else
            {
                remove(key);
                cache.Add(key, value);
            }
        }

        /// <summary>
        /// 删除用户状态信息
        /// </summary>
        /// <param name="key"></param>
        public void remove(String key)
        {
            cache.Remove(key);
        }

        /// <summary>
        /// 获取用户状态信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object get(String key)
        {
            Object obj;
            if (cache.ContainsKey(key))
            { 
                cache.TryGetValue(key, out obj);
            }
            else
            {
                obj = null;
            }
            return obj;
        }

        public Models.sys_user Current_User
        {
            get
            {
                string token = HttpContext.Current.Request.Headers.GetValues("Authorization").FirstOrDefault();
                token = token.Replace("Bearer ", "");
                return get(token) as Models.sys_user;
            }
        }
    }
}
