using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - MongoDB提供者
    /// </summary>
    public class MongoStoreProvider : IEventStore
    {
        #region # 字段及构造器

        /// <summary>
        /// Mongo服务器与数据库名分隔符号
        /// </summary>
        private const string Separator = "::";

        /// <summary>
        /// MongoDB连接字符串键
        /// </summary>
        private const string MongoConnectionStringKey = "MongoConnection";

        /// <summary>
        /// MongoDB连接字符串
        /// </summary>
        private static readonly string _ConnectionString;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MongoStoreProvider()
        {
            string connStr = ConfigurationManager.ConnectionStrings[MongoConnectionStringKey].ConnectionString;

            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new ApplicationException(string.Format("MongoDB连接字符串未设置，默认连接字符串键为\"{0}\"！", MongoConnectionStringKey));
            }
            _ConnectionString = connStr;

            //注册实体类型
            RegisterTypes();
        }

        /// <summary>
        /// MongoDB实体对象集合
        /// </summary>
        private readonly IMongoCollection<Event> _collection;

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly Guid _sessionId;

        /// <summary>
        /// 构造器
        /// </summary>
        public MongoStoreProvider()
        {
            string[] connStr = _ConnectionString.Split(new[] { Separator }, StringSplitOptions.None);

            if (connStr.Length != 2)
            {
                throw new ApplicationException(string.Format("连接字符串格式不正确，请使用\"{0}\"来分隔服务器地址与数据库名称！", Separator));
            }

            MongoClient client = new MongoClient(connStr[0]);
            IMongoDatabase database = client.GetDatabase(connStr[1]);
            this._collection = database.GetCollection<Event>(typeof(Event).FullName);

            this._sessionId = WebConfigSetting.CurrentSessionId;
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
            this._collection.InsertOneAsync(eventSource as Event).Wait();
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            Expression<Func<Event, bool>> condition =
                x =>
                    !x.Handled &&
                    x.SessionId == this._sessionId;

            IFindFluent<Event, Event> eventSources = this._collection.Find(condition).SortByDescending(x => x.AddedTime);

            //如果有未处理的
            foreach (Event eventSource in eventSources.ToEnumerable())
            {
                EventMediator.Handle((IEvent)eventSource);
                eventSource.Handled = true;

                this._collection.ReplaceOne(x => x.Id == eventSource.Id, eventSource);
            }

            //递归
            if (this._collection.Find(condition).Any())
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
            this._collection.DeleteMany(x => x.SessionId == this._sessionId && !x.Handled);
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


        //Private

        #region # 注册实体类型 —— static void RegisterTypes()
        /// <summary>
        /// 注册实体类型
        /// </summary>
        private static void RegisterTypes()
        {
            //注册领域事件基类
            BsonClassMap.RegisterClassMap<Event>(map =>
            {
                map.AutoMap();
                map.SetIsRootClass(true);
            });

            //加载事件所在程序集
            Assembly eventAssembly = Assembly.Load(WebConfigSetting.EventSourceAssembly);
            IEnumerable<Type> eventTypes = eventAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Event)));

            //注册具体类
            foreach (Type eventType in eventTypes)
            {
                BsonClassMap map = new BsonClassMap(eventType);
                map.AutoMap();
                map.SetIsRootClass(false);
                BsonClassMap.RegisterClassMap(map);
            }
        }
        #endregion
    }
}
