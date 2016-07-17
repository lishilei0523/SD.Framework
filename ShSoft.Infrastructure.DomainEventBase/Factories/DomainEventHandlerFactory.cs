using System;
using System.Collections.Generic;
using System.Linq;
using SD.IOC.Core.Mediator;

namespace ShSoft.Infrastructure.DomainEventBase.Factories
{
    /// <summary>
    /// 领域事件处理者工厂
    /// </summary>
    internal static class DomainEventHandlerFactory
    {
        #region # 获取领域事件处理者实例集合 —— IEnumerable<IDomainEventHandler<T>> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例集合
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="domainEvent">领域事件源</param>
        /// <returns>领域事件处理者集合</returns>
        public static IEnumerable<IDomainEventHandler<T>> GetEventHandlersFor<T>(T domainEvent) where T : class, IDomainEvent
        {
            return ResolveMediator.ResolveAll<IDomainEventHandler<T>>();
        }
        #endregion

        #region # 获取领域事件处理者实例集合 —— IEnumerable<IDomainEventHandler> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例集合
        /// </summary>
        /// <param name="eventType">领域事件类型</param>
        /// <returns>领域事件处理者集合</returns>
        public static IEnumerable<IDomainEventHandler> GetEventHandlersFor(Type eventType)
        {
            #region # 验证类型

            if (!typeof(IDomainEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException(string.Format("类型\"{0}\"不实现领域事件基接口！", eventType.FullName));
            }

            #endregion

            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            IEnumerable<object> handlers = ResolveMediator.ResolveAll(handlerType);

            return handlers.Select(handler => (IDomainEventHandler)handler);
        }
        #endregion
    }
}
