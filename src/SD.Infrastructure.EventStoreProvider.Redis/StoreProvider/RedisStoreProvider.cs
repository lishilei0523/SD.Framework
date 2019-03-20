using SD.Infrastructure.Constants;
using SD.Infrastructure.EventBase;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventStoreProvider.Redis.Toolkits;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System.Linq;

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
        /// Redis客户端
        /// </summary>
        private readonly IDatabase _redisClient;

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly string _sessionId;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisStoreProvider()
        {
            this._redisClient = RedisManager.GetDatabase();
            this._sessionId = GlobalSetting.CurrentSessionId.ToString();
        }

        #endregion

        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource"></param>
        public void Suspend<T>(T eventSource) where T : class, IEvent
        {
            this._redisClient.HashSet(this._sessionId, eventSource.Id.ToString(), eventSource.EventToJson());
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            RedisValue[] eventSourcesStr = this._redisClient.HashValues(this._sessionId);

            foreach (string eventSourceStr in eventSourcesStr)
            {
                IEvent eventSource = eventSourceStr.JsonToEvent();
                EventMediator.Handle(eventSource);

                this._redisClient.HashDelete(this._sessionId, eventSource.Id.ToString());
            }

            RedisValue[] newEventSourcesStr = this._redisClient.HashValues(this._sessionId);

            if (newEventSourcesStr.Any())
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
            this._redisClient.KeyDelete(this._sessionId);
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
