using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventStoreProvider.RavenDB.Configuration;

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
        /// RavenDB连接字符串名称
        /// </summary>
        private const string ConnectionStringName = "RavenConnection";

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
            IDocumentStore documentStore = new DocumentStore { ConnectionStringName = ConnectionStringName };
            documentStore.Initialize();

            Assembly assembly = Assembly.Load("ShSoft.Infrastructure.EventBaseTests");

            IndexCreation.CreateIndexes(assembly, documentStore);

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
            Event @event = eventSource as Event;

            this._dbSession.Store(@event);
            this._dbSession.SaveChanges();
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

            IRavenQueryable<Event> eventSources = this._dbSession.Query<Event>(typeof(EventIndex).Name).Where(condition).OrderByDescending(x => x.AddedTime);

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
            if (this._dbSession.Query<Event>(typeof(EventIndex).Name).Any(condition))
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
    }
}
