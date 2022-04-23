using SD.Infrastructure.Constants;
using SD.Infrastructure.CrontabBase;
using SD.Infrastructure.CrontabStore.Redis.Toolkits;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.CrontabStoreProvider
{
    /// <summary>
    /// 定时任务存储 - Redis提供者
    /// </summary>
    public class RedisStoreProvider : ICrontabStore
    {
        #region # 字段及构造器

        /// <summary>
        /// 缓存键
        /// </summary>
        private static readonly string _CacheKey = GlobalSetting.ApplicationId;

        /// <summary>
        /// Redis客户端
        /// </summary>
        private readonly IDatabase _redisClient;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisStoreProvider()
        {
            this._redisClient = RedisManager.GetDatabase();
        }

        #endregion

        #region # 保存定时任务 —— void Store(ICrontab crontab) 
        /// <summary>
        /// 保存定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public void Store(ICrontab crontab)
        {
            this._redisClient.HashSet(_CacheKey, crontab.Id, crontab.CrontabToJson());
        }
        #endregion

        #region # 获取定时任务 —— T Get<T>(string crontabId)
        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontabId">定时任务Id</param>
        /// <returns>定时任务</returns>
        public T Get<T>(string crontabId) where T : ICrontab
        {
            string crontabStr = this._redisClient.HashGet(_CacheKey, crontabId);

            #region # 验证

            if (string.IsNullOrWhiteSpace(crontabStr))
            {
                return default(T);
            }

            #endregion

            ICrontab crontab = crontabStr.JsonToCrontab();

            return (T)crontab;
        }
        #endregion

        #region # 删除定时任务 —— void Remove(ICrontab crontab)
        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public void Remove(ICrontab crontab)
        {
            this._redisClient.HashDelete(_CacheKey, crontab.Id);
        }
        #endregion

        #region # 删除定时任务 —— void Remove(string crontabId)
        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public void Remove(string crontabId)
        {
            this._redisClient.HashDelete(_CacheKey, crontabId);
        }
        #endregion

        #region # 清空定时任务 —— void Clear()
        /// <summary>
        /// 清空定时任务
        /// </summary>
        public void Clear()
        {
            this._redisClient.KeyDelete(_CacheKey);
        }
        #endregion

        #region # 获取全部定时任务列表 —— IList<ICrontab> FindAll()
        /// <summary>
        /// 获取全部定时任务列表
        /// </summary>
        /// <returns>定时任务列表</returns>
        public IList<ICrontab> FindAll()
        {
            IList<ICrontab> crontabs = new List<ICrontab>();
            RedisValue[] crontabsStr = this._redisClient.HashValues(_CacheKey);
            foreach (string crontabStr in crontabsStr)
            {
                #region # 验证

                if (string.IsNullOrWhiteSpace(crontabStr))
                {
                    continue;
                }

                #endregion

                ICrontab crontab = crontabStr.JsonToCrontab();
                crontabs.Add(crontab);
            }

            return crontabs;
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
        #endregion
    }
}
