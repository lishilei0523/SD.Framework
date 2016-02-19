using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ShSoft.Framework2015.Infrastructure.DomainEvent.Factories;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator
{
    /// <summary>
    /// 领域事件中介者
    /// </summary>
    public static class EventMediator
    {
        #region # 挂起领域事件 —— void Suspend<T>(T domainSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="domainSource">领域事件源</param>
        public static void Suspend<T>(T domainSource) where T : IDomainEvent.DomainEvent
        {
            using (IDomainEventStorer eventStorer = DomainEventStorerFactory.GetEventEventStorer())
            {
                eventStorer.Suspend(domainSource);
            }
        }
        #endregion

        #region # 处理领域事件 —— void Handle<T>(T eventSource)
        /// <summary>
        /// 处理领域事件
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public static void Handle<T>(T eventSource) where T : IDomainEvent.DomainEvent
        {
            //01.获取相应同步领域事件处理者实例集合
            IEnumerable<IDomainEventHandler<T>> eventHandlers =
                DomainEventHandlerFactory.GetEventHandlersFor(eventSource).OrderByDescending(x => x.Sort);

            //02.获取相应异步领域事件处理者实例集合
            IEnumerable<IDomainEventHandlerAsync<T>> eventHandlersAsync =
                DomainEventHandlerFactory.GetEventHandlersAsyncFor(eventSource);

            //03.顺序处理同步领域事件
            foreach (IDomainEventHandler<T> handler in eventHandlers)
            {
                handler.Handle(eventSource);
            }

            //04.并行处理异步领域事件
            Task.Factory.StartNew(() => Parallel.ForEach(eventHandlersAsync, handler => handler.HandleAsync(eventSource)));
        }
        #endregion

        #region # 处理领域事件 —— void Handle(IDomainEvent eventSource)
        /// <summary>
        /// 处理领域事件
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        /// <remarks>此方法涉及反射操作，慎做修改</remarks>
        internal static void Handle(IDomainEvent.IDomainEvent eventSource)
        {
            //01.获取相应同步领域事件处理者实例集合
            IEnumerable<IDomainEventHandler> eventHandlers =
                DomainEventHandlerFactory.GetEventHandlersFor(eventSource.GetType()).OrderByDescending(x => x.Sort);

            //02.获取相应异步领域事件处理者实例集合
            IEnumerable<object> eventHandlersAsync =
                DomainEventHandlerFactory.GetEventHandlersAsyncFor(eventSource.GetType());

            //03.顺序处理同步领域事件
            foreach (IDomainEventHandler handler in eventHandlers)
            {
                try
                {
                    handler.GetType().GetMethod("Handle").Invoke(handler, new object[] { eventSource });
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
            }

            //04.并行处理异步领域事件
            Task.Factory.StartNew(() => Parallel.ForEach(eventHandlersAsync, handler =>
            {
                try
                {
                    handler.GetType().GetMethod("HandleAsync").Invoke(handler, new object[] { eventSource });
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
            }));
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        internal static void HandleUncompletedEvents()
        {
            using (IDomainEventStorer eventStorer = DomainEventStorerFactory.GetEventEventStorer())
            {
                eventStorer.HandleUncompletedEvents();
            }
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        internal static void ClearUncompletedEvents()
        {
            using (IDomainEventStorer eventStorer = DomainEventStorerFactory.GetEventEventStorer())
            {
                eventStorer.ClearUncompletedEvents();
            }
        }
        #endregion
    }
}
