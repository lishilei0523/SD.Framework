using System;
using System.Collections.Generic;
using System.Linq;
using SD.IOC.Core.Mediator;

namespace SD.Infrastructure.EventBase.Factories
{
    /// <summary>
    /// 领域事件处理者工厂
    /// </summary>
    internal static class EventHandlerFactory
    {
        #region # 获取领域事件处理者实例集合 —— IEnumerable<IEventHandler<T>> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例集合
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        /// <returns>领域事件处理者集合</returns>
        public static IEnumerable<IEventHandler<T>> GetEventHandlersFor<T>(T eventSource) where T : class, IEvent
        {
            return ResolveMediator.ResolveAll<IEventHandler<T>>();
        }
        #endregion

        #region # 获取领域事件处理者实例集合 —— IEnumerable<IEventHandler> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例集合
        /// </summary>
        /// <param name="eventType">领域事件类型</param>
        /// <returns>领域事件处理者集合</returns>
        public static IEnumerable<IEventHandler> GetEventHandlersFor(Type eventType)
        {
            #region # 验证类型

            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException(string.Format("类型\"{0}\"不实现领域事件基接口！", eventType.FullName));
            }

            #endregion

            Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            IEnumerable<object> handlers = ResolveMediator.ResolveAll(handlerType);

            return handlers.Select(handler => (IEventHandler)handler);
        }
        #endregion
    }
}
