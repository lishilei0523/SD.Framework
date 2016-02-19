using System;
using System.Collections.Generic;
using System.Linq;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;
using ShSoft.Framework2015.Infrastructure.IOC.Mediator;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.Factories
{
    /// <summary>
    /// 领域事件处理者工厂
    /// </summary>
    internal static class DomainEventHandlerFactory
    {
        #region # 获取同步领域事件处理者实例集合 —— IEnumerable<IDomainEventHandler<T>> GetEventHandlersFor<T>...
        /// <summary>
        /// 获取同步领域事件处理者实例集合
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="domainEvent">领域事件源</param>
        /// <returns>同步领域事件处理者集合</returns>
        public static IEnumerable<IDomainEventHandler<T>> GetEventHandlersFor<T>(T domainEvent) where T : IDomainEvent.DomainEvent
        {
            return ResolverMediator.ResolveAll<IDomainEventHandler<T>>();
        }
        #endregion

        #region # 获取同步领域事件处理者实例集合 —— IEnumerable<IDomainEventHandler> GetEventHandlersFor(Type eventType)
        /// <summary>
        /// 获取同步领域事件处理者实例集合
        /// </summary>
        /// <param name="eventType">领域事件类型</param>
        /// <returns>同步领域事件处理者集合</returns>
        public static IEnumerable<IDomainEventHandler> GetEventHandlersFor(Type eventType)
        {
            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            IEnumerable<object> handlers = ResolverMediator.ResolveAll(handlerType);

            return handlers.Select(handler => (IDomainEventHandler)handler);
        }
        #endregion

        #region # 获取异步领域事件处理者实例集合 —— IEnumerable<IDomainEventHandlerAsync<T>> GetEventHandlersAsyncFor<T>...
        /// <summary>
        /// 获取异步领域事件处理者实例集合
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="domainEvent">领域事件源</param>
        /// <returns>异步领域事件处理者集合</returns>
        public static IEnumerable<IDomainEventHandlerAsync<T>> GetEventHandlersAsyncFor<T>(T domainEvent) where T : IDomainEvent.DomainEvent
        {
            return ResolverMediator.ResolveAll<IDomainEventHandlerAsync<T>>();
        }
        #endregion

        #region # 获取异步领域事件处理者实例集合 —— IEnumerable<object> GetEventHandlersAsyncFor(Type eventType)
        /// <summary>
        /// 获取异步领域事件处理者实例集合
        /// </summary>
        /// <param name="eventType">领域事件类型</param>
        /// <returns>异步领域事件处理者集合</returns>
        public static IEnumerable<object> GetEventHandlersAsyncFor(Type eventType)
        {
            Type handlerType = typeof(IDomainEventHandlerAsync<>).MakeGenericType(eventType);

            return ResolverMediator.ResolveAll(handlerType);
        }
        #endregion
    }
}
