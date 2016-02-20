namespace ShSoft.Framework2015.CacheStorage.Factory
{
    /// <summary>
    /// 缓存容器类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 无缓存
        /// </summary>
        Null = 0,

        /// <summary>
        /// HttpRuntime缓存
        /// </summary>
        HttpRuntime = 1,

        /// <summary>
        /// Memcached缓存
        /// </summary>
        Memcached = 2,

        /// <summary>
        /// Redis缓存
        /// </summary>
        Redis = 3
    }
}
