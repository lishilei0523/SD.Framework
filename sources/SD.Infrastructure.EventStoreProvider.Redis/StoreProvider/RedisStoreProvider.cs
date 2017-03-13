using System.Linq;
using SD.Toolkits.Redis;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SD.Infrastructure.Constants;
using SD.Infrastructure.EventBase;
using SD.Infrastructure.EventBase.Mediator;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - Redis提供者
    /// </summary>
    public class RedisStoreProvider : IEventStore
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis客户端管理器
        /// </summary>
        private readonly IRedisClientsManager _clientsManager;

        /// <summary>
        /// Redis客户端
        /// </summary>
        private readonly IRedisClient _redisClient;

        /// <summary>
        /// Redis类型客户端
        /// </summary>
        private readonly IRedisTypedClient<Event> _redisTypedClient;

        /// <summary>
        /// Redis表
        /// </summary>
        private readonly IRedisList<Event> _table;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisStoreProvider()
        {
            this._clientsManager = RedisManager.CreateClientsManager();

            //获取会话Id
            string sessionId = WebConfigSetting.CurrentSessionId.ToString();

            //实例化RedisClient
            this._redisClient = this._clientsManager.GetClient();
            this._redisTypedClient = this._redisClient.As<Event>();
            this._table = this._redisTypedClient.Lists[sessionId];
        }

        #endregion


        //Implements

        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public void Suspend<T>(T eventSource) where T : class, IEvent
        {
            this._table.Append(eventSource as Event);
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            IOrderedEnumerable<Event> eventSources = this._table.OrderByDescending(x => x.AddedTime);

            //如果有未处理的
            foreach (Event eventSource in eventSources.ToArray())
            {
                EventMediator.Handle((IEvent)eventSource);
                this._table.RemoveStart();
            }

            //递归
            if (this._table.Any())
            {
                this.HandleUncompletedEvents();
            }
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            while (this._table.GetAll().Any())
            {
                this._table.RemoveStart();
            }
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._redisClient != null)
            {
                this._redisClient.Dispose();
            }
            if (this._clientsManager != null)
            {
                this._clientsManager.Dispose();
            }
        }
        #endregion
    }
}
