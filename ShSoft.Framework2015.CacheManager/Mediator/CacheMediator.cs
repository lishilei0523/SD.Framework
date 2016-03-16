using System;
using ShSoft.Framework2015.CacheManager.Factory;
using ShSoft.Framework2015.CacheManager.Interface;

namespace ShSoft.Framework2015.CacheManager.Mediator
{
    /// <summary>
    /// 缓存中介者
    /// </summary>
    public static class CacheMediator
    {
        #region # 写入缓存（无过期时间） —— static void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(string key, T value)
        {
            ICacheAdapter cacheAdapter = CacheAdapterFactory.GetCacheAdapter();
            cacheAdapter.Set(key, value);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— static void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public static void Set<T>(string key, T value, DateTime exp)
        {
            ICacheAdapter cacheAdapter = CacheAdapterFactory.GetCacheAdapter();
            cacheAdapter.Set(key, value, exp);
        }
        #endregion

        #region # 读取缓存 —— static T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get<T>(string key)
        {
            ICacheAdapter cacheAdapter = CacheAdapterFactory.GetCacheAdapter();
            return cacheAdapter.Get<T>(key);
        }
        #endregion

        #region # 移除缓存 —— static void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            ICacheAdapter cacheAdapter = CacheAdapterFactory.GetCacheAdapter();
            cacheAdapter.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— static void RemoveAll()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void RemoveAll()
        {
            ICacheAdapter cacheAdapter = CacheAdapterFactory.GetCacheAdapter();
            cacheAdapter.RemoveAll();
        }
        #endregion
    }
}
