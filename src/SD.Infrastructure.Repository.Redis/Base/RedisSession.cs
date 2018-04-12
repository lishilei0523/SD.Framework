using SD.Toolkits.Redis;
using ServiceStack.Redis;
using System;
using System.Runtime.Remoting.Messaging;

namespace SD.Infrastructure.Repository.Redis.Base
{
    /// <summary>
    /// Redis上下文对象
    /// </summary>
    internal class RedisSession : IDisposable
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// Redis上下文对象缓存键
        /// </summary>
        internal const string CurrentInstanceKey = "CurrentInstance";

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// Redis客户端管理器
        /// </summary>
        private readonly IRedisClientsManager _clientsManager;

        /// <summary>
        /// 构造器
        /// </summary>
        private RedisSession()
        {
            this._clientsManager = RedisManager.CreateClientsManager();

            //实例化RedisClient
            this.RedisWriteClient = this._clientsManager.GetClient();
            this.RedisReadClient = this._clientsManager.GetReadOnlyClient();
        }

        #endregion

        #region # 访问器

        #region Redis上下文对象 —— static DbContext CommandInstance
        /// <summary>
        /// Redis上下文对象
        /// </summary>
        public static RedisSession Current
        {
            get
            {
                lock (_Sync)
                {
                    RedisSession redisSession = CallContext.GetData(CurrentInstanceKey) as RedisSession;
                    if (redisSession == null)
                    {
                        redisSession = new RedisSession();
                        CallContext.SetData(CurrentInstanceKey, redisSession);
                    }
                    return redisSession;
                }
            }
        }
        #endregion

        #endregion

        #region # 属性

        #region Redis（写）客户端 —— IRedisClient RedisWriteClient
        /// <summary>
        /// Redis（写）客户端
        /// </summary>
        public IRedisClient RedisWriteClient { get; private set; }
        #endregion

        #region Redis（读）客户端 —— IRedisClient RedisReadClient
        /// <summary>
        /// Redis（读）客户端
        /// </summary>
        public IRedisClient RedisReadClient { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.RedisWriteClient != null)
            {
                this.RedisWriteClient.Dispose();
            }
            if (this.RedisReadClient != null)
            {
                this.RedisReadClient.Dispose();
            }
            if (this._clientsManager != null)
            {
                this._clientsManager.Dispose();
            }
        }
        #endregion

        #endregion
    }
}
