using System;
using ShSoft.Framework2015.CacheStorage.Interface;

namespace ShSoft.Framework2015.CacheStorage.Implements
{
    /// <summary>
    /// 空缓存容器
    /// </summary>
    internal class NullCacheAdapter : ICacheAdapter
    {
        public static readonly ICacheAdapter Current = new NullCacheAdapter();
        public void Set<T>(string key, T value) { }
        public void Set<T>(string key, T value, DateTime exp) { }
        public T Get<T>(string key) { return default(T); }
        public void Remove(string key) { }
        public void RemoveAll() { }
    }
}
