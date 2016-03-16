using System;
using System.Configuration;
using Memcached.Client;
using ShSoft.Framework2015.CacheManager.Interface;

namespace ShSoft.Framework2015.CacheManager.Implements
{
    /// <summary>
    /// Memcached缓存容器
    /// </summary>
    internal class MemcachedAdapter : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// Memcached服务器地址集AppSetting键
        /// </summary>
        private const string MemcachedServersAppSettingKey = "MemcachedServers";

        /// <summary>
        /// 定义Memcached客户端私有字段
        /// </summary>
        private static readonly MemcachedClient _MemcachedClient;

        /// <summary>
        /// 当前实例
        /// </summary>
        private static readonly HttpRuntimeCacheAdapter _Current;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MemcachedAdapter()
        {
            //00.读取配置文件
            string memcachedServers = ConfigurationManager.AppSettings[MemcachedServersAppSettingKey];

            //01.判断是否为空
            if (string.IsNullOrEmpty(memcachedServers))      //为空
            {
                //抛出异常
                throw new SystemException("Memcached服务端IP地址组未配置！");
            }

            //02.分割IP字符串获得IP数组
            string[] servers = memcachedServers.Split(',');

            //03.初始化缓存池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(servers);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();
            _MemcachedClient = new MemcachedClient { EnableCompression = false };

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
            _MemcachedClient.Set(key, value);
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
            _MemcachedClient.Set(key, value, exp);
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
            return (T)_MemcachedClient.Get(key);
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            _MemcachedClient.Delete(key);
        }
        #endregion

        #region # 清空缓存 —— void RemoveAll()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void RemoveAll()
        {
            _MemcachedClient.FlushAll();
        }
        #endregion
    }
}
