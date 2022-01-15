using SD.Infrastructure.EventBase.Factories;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace SD.Infrastructure.EventBase.Mediators
{
    /// <summary>
    /// 领域事件中介者
    /// </summary>
    public static class EventMediator
    {
        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public static void Suspend<T>(T eventSource) where T : class, IEvent
        {
            using (IEventStore eventStorer = ResolveMediator.Resolve<IEventStore>())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                {
                    eventStorer.Suspend(eventSource);
                    scope.Complete();
                }
            }
        }
        #endregion

        #region # 挂起领域事件（异步） —— async Task SuspendAsync<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件（异步）
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public static async Task SuspendAsync<T>(T eventSource) where T : class, IEvent
        {
            await Task.Run(() =>
            {
                using (IEventStore eventStorer = ResolveMediator.Resolve<IEventStore>())
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                    {
                        eventStorer.Suspend(eventSource);
                        scope.Complete();
                    }
                }
            });
        }
        #endregion

        #region # 处理领域事件 —— void Handle<T>(T eventSource)
        /// <summary>
        /// 处理领域事件
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public static void Handle<T>(T eventSource) where T : class, IEvent
        {
            //获取相应事件处理者实例集合
            IEnumerable<IEventHandler<T>> eventHandlers = EventHandlerFactory.GetEventHandlersFor(eventSource);
            foreach (IEventHandler<T> handler in eventHandlers)
            {
                handler.Handle(eventSource);
            }
        }
        #endregion

        #region # 处理领域事件 —— void Handle(IEvent eventSource)
        /// <summary>
        /// 处理领域事件
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        /// <remarks>此方法涉及反射操作，慎做修改</remarks>
        public static void Handle(IEvent eventSource)
        {
            //获取相应事件处理者实例集合
            Type eventSourseType = eventSource.GetType();
            IEnumerable<IEventHandler> eventHandlers = EventHandlerFactory.GetEventHandlersFor(eventSourseType);
            foreach (IEventHandler handler in eventHandlers)
            {
                try
                {
                    Type handlerType = handler.GetType();
                    MethodInfo methodInfo = handlerType.GetMethod("Handle", new[] { eventSource.GetType() });
                    methodInfo?.Invoke(handler, new object[] { eventSource });
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
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        internal static void HandleUncompletedEvents()
        {
            using (IEventStore eventStorer = ResolveMediator.Resolve<IEventStore>())
            {
                eventStorer.HandleUncompletedEvents();
            }
        }
        #endregion

        #region # 处理未处理的领域事件（异步） —— Task HandleUncompletedEventsAsync()
        /// <summary>
        /// 处理未处理的领域事件（异步）
        /// </summary>
        internal static async Task HandleUncompletedEventsAsync()
        {
            await Task.Run(() =>
            {
                using (IEventStore eventStorer = ResolveMediator.Resolve<IEventStore>())
                {
                    eventStorer.HandleUncompletedEvents();
                }
            });
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        internal static void ClearUncompletedEvents()
        {
            using (IEventStore eventStorer = ResolveMediator.Resolve<IEventStore>())
            {
                eventStorer.ClearUncompletedEvents();
            }
        }
        #endregion
    }
}
