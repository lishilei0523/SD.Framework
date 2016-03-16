using System;
using System.Configuration;
using ShSoft.Framework2015.CacheManager.Implements;
using ShSoft.Framework2015.CacheManager.Interface;

namespace ShSoft.Framework2015.CacheManager.Factory
{
    /// <summary>
    /// 缓存容器工厂
    /// </summary>
    internal static class CacheAdapterFactory
    {
        #region # 静态字段及静态构造器

        /// <summary>
        /// 缓存类型AppSetting键
        /// </summary>
        private const string CacheTypeAppSettingKey = "CacheType";

        /// <summary>
        /// 缓存类型
        /// </summary>
        private static readonly CacheType? _CacheType;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheAdapterFactory()
        {
            //从配置文件读取缓存类型
            string cacheTypeString = ConfigurationManager.AppSettings[CacheTypeAppSettingKey];
            if (!string.IsNullOrWhiteSpace(cacheTypeString))
            {
                _CacheType = (CacheType)Enum.Parse(typeof(CacheType), cacheTypeString);
            }
        }
        #endregion

        #region # 创建缓存适配器对象 —— static ICacheAdapter GetCacheAdapter(CacheType? cacheType)
        /// <summary>
        /// 创建缓存适配器对象
        /// </summary>
        /// <param name="cacheType">缓存容器类型</param>
        /// <returns>缓存适配器</returns>
        public static ICacheAdapter GetCacheAdapter(CacheType? cacheType)
        {
            switch (cacheType)
            {
                //HttpRuntime缓存
                case CacheType.HttpRuntime:
                    return HttpRuntimeCacheAdapter.Current;

                //Memcached缓存
                case CacheType.Memcached:
                    return MemcachedAdapter.Current;

                //Redis缓存
                case CacheType.Redis:
                    return RedisCacheAdapter.Current;

                //无缓存
                case CacheType.Null:
                    return NullCacheAdapter.Current;

                //默认无缓存
                default:
                    return NullCacheAdapter.Current;
            }
        }
        #endregion

        #region # 创建默认缓存适配器对象（HttpRuntime） —— static ICacheAdapter GetDefaultCacheAdapter()
        /// <summary>
        /// 创建默认缓存适配器对象（HttpRuntime）
        /// </summary>
        /// <returns>缓存适配器</returns>
        public static ICacheAdapter GetDefaultCacheAdapter()
        {
            //HttpRuntime缓存
            return HttpRuntimeCacheAdapter.Current;
        }
        #endregion

        #region # 读取配置创建缓存适配器对象 —— static ICacheAdapter GetCacheAdapter()
        /// <summary>
        /// 读取配置创建缓存适配器对象
        /// </summary>
        /// <returns>缓存适配器</returns>
        internal static ICacheAdapter GetCacheAdapter()
        {
            return GetCacheAdapter(_CacheType);
        }
        #endregion
    }
}
