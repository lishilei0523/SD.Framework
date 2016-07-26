using System;
using System.Configuration;
using System.Linq;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - Redis提供者
    /// </summary>
    public class RedisStoreProvider : IEventStore
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis服务器地址AppSetting键
        /// </summary>
        private const string RedisServerAppSettingKey = "RedisServer";

        /// <summary>
        /// Redis客户端
        /// </summary>
        private static readonly IRedisClient _RedisClient;

        /// <summary>
        /// 静态构造器
        /// </summary>

        static RedisStoreProvider()
        {
            //读取配置文件中的Redis服务端IP地址、端口号
            string ip = ConfigurationManager.AppSettings[RedisServerAppSettingKey];   //127.0.0.1,6379

            //判断是否为空
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new SystemException("Redis服务端IP地址未配置！");
            }

            string[] redisServer = ip.Split(',');

            //实例化RedisClient
            _RedisClient = new RedisClient(redisServer[0], int.Parse(redisServer[1]));
        }

        /// <summary>
        /// Redis类型客户端
        /// </summary>
        private readonly IRedisTypedClient<Event> _redisTypedClient;

        /// <summary>
        /// Redis表
        /// </summary>
        private readonly IRedisList<Event> _table;

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly string _sessionId;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisStoreProvider()
        {
            this._sessionId = WebConfigSetting.CurrentSessionId.ToString();
            this._redisTypedClient = _RedisClient.As<Event>();
            this._table = this._redisTypedClient.Lists[this._sessionId];
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
            this._redisTypedClient.AddItemToList(this._table, eventSource as Event);
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
                this._redisTypedClient.RemoveItemFromList(this._table, eventSource);
            }

            //递归
            if (this._table.Any(x => !x.Handled))
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
            foreach (Event eventSource in this._table.GetAll().ToArray())
            {
                this._redisTypedClient.RemoveItemFromList(this._table, eventSource);
            }
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }
}
