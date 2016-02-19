using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using ShSoft.Framework2015.CacheStorage.Interface;

namespace ShSoft.Framework2015.CacheStorage.Implements
{
    /// <summary>
    /// HttpRuntimeCache缓存容器
    /// </summary>
    internal class HttpRuntimeCacheAdapter : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// 当前实例
        /// </summary>
        private static readonly HttpRuntimeCacheAdapter _Current;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static HttpRuntimeCacheAdapter()
        {
            _Current = new HttpRuntimeCacheAdapter();
        }

        #endregion

        #region # 访问器 —— static ICacheAdapter Current
        /// <summary>
        /// 当前实例
        /// </summary>
        public static ICacheAdapter Current
        {
            get { return _Current; }
        }
        #endregion

        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set<T>(string key, T value)
        {
            //如果缓存已存在则清空
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public void Set<T>(string key, T value, DateTime exp)
        {
            //如果缓存已存在则清空
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value, null, exp, TimeSpan.Zero);
        }
        #endregion

        #region # 读取缓存 —— T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— void RemoveAll()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void RemoveAll()
        {
            //01.定义key集合用于存储所有cache的键
            List<string> keys = new List<string>();

            //02.获取HttpRuntime字典枚举器
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            //03.将所有缓存键添加到keys集合
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            //04.循环删除所有缓存
            foreach (string key in keys)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }
        #endregion
    }
}
