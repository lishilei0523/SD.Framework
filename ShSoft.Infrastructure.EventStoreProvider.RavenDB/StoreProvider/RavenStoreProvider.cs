using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - RavenDB提供者
    /// </summary>
    public class RavenStoreProvider : IEventStore
    {
        #region # 常量

        /// <summary>
        /// RavenDB连接字符串AppSetting键
        /// </summary>
        private const string AppSettingKey = "RavenEventConnection";

        #endregion

        #region # 静态构造器

        /// <summary>
        /// RavenDB文档存储延迟加载字段
        /// </summary>
        private static readonly IDocumentStore _Store;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RavenStoreProvider()
        {
            string connectionStringName = ConfigurationManager.AppSettings[AppSettingKey];

            #region # 验证

            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                throw new ApplicationException("Raven领域事件存储AppSetting未设置，请联系管理员！");
            }

            #endregion

            IDocumentStore documentStore = new DocumentStore { ConnectionStringName = connectionStringName };
            documentStore.Initialize();

            _Store = documentStore;
        }

        #endregion

        #region # 实例构造器

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly Guid _sessionId;

        /// <summary>
        /// RavenDB会话对象
        /// </summary>
        private readonly IDocumentSession _dbSession;

        /// <summary>
        /// 构造器
        /// </summary>
        public RavenStoreProvider()
        {
            this._dbSession = _Store.OpenSession();
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
            this._dbSession.Store(eventSource);
            this._dbSession.SaveChanges();
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            IOrderedEnumerable<Event> eventSources = this.GetEventSources().OrderByDescending(x => x.AddedTime);

            //如果有未处理的
            if (eventSources.Any())
            {
                foreach (Event eventSource in eventSources)
                {
                    EventMediator.Handle((IEvent)eventSource);
                    eventSource.Handled = true;
                }
                this._dbSession.SaveChanges();
            }

            //递归
            if (this.GetEventSources().Any())
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
            Event[] eventSources = this._dbSession.Query<Event>().Where(x => !x.Handled).ToArray();

            foreach (Event eventSource in eventSources)
            {
                this._dbSession.Delete(eventSource.Id);
            }

            this._dbSession.SaveChanges();
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._dbSession != null)
            {
                this._dbSession.Dispose();
            }
        }
        #endregion


        //Private

        #region # 获取领域事件列表 —— IEnumerable<Event> GetEventSources()
        /// <summary>
        /// 获取领域事件列表
        /// </summary>
        /// <returns>领域事件列表</returns>
        private IEnumerable<Event> GetEventSources()
        {
            //反射获取所有领域事件类型
            Assembly eventAssembly = Assembly.Load(WebConfigSetting.EventSourceAssembly);
            IEnumerable<Type> eventTypes = eventAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Event)));

            //获取IDocumentSession.Query<T>()方法
            Func<MethodInfo, bool> condition =
                x =>
                    x.Name == "Query" &&
                    x.IsGenericMethod &&
                    x.GetGenericArguments().Length == 1 &&
                    x.GetParameters().Length == 0;

            MethodInfo methodQuery = this._dbSession.GetType().GetMethods().Single(condition);

            //声明领域事件集合
            IList<Event> events = new List<Event>();

            //遍历领域事件类型
            foreach (Type eventType in eventTypes)
            {
                //填充泛型
                MethodInfo genericMethodQuery = methodQuery.MakeGenericMethod(eventType);

                //查询领域事件列表
                object methodResult = genericMethodQuery.Invoke(this._dbSession, null);
                IQueryable<Event> queryable = (IQueryable<Event>)methodResult;

                //过滤
                IEnumerable<Event> specEvents = queryable.ToArray().Where(x => !x.Handled && x.SessionId == this._sessionId);

                //填充集合
                events.AddRange(specEvents);
            }

            return events;
        }
        #endregion
    }
}
