using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.EventBase.Factories
{
    /// <summary>
    /// 领域事件处理者工厂
    /// </summary>
    internal static class EventHandlerFactory
    {
        #region # 获取领域事件处理者实例列表 —— IEnumerable<IEventHandler<T>> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例列表
        /// </summary>
        /// <typeparam name="T">领域事件类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        /// <returns>领域事件处理者列表</returns>
        public static IEnumerable<IEventHandler<T>> GetEventHandlersFor<T>(T eventSource) where T : class, IEvent
        {
            IEnumerable<IEventHandler<T>> handlers = ResolveMediator.ResolveAll<IEventHandler<T>>();

            return handlers;
        }
        #endregion

        #region # 获取领域事件处理者实例列表 —— IEnumerable<IEventHandler> GetEventHandlers...
        /// <summary>
        /// 获取领域事件处理者实例列表
        /// </summary>
        /// <param name="eventType">领域事件类型</param>
        /// <returns>领域事件处理者列表</returns>
        public static IEnumerable<IEventHandler> GetEventHandlersFor(Type eventType)
        {
            #region # 验证类型

            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException($"类型\"{eventType.FullName}\"不实现领域事件基接口！");
            }

            #endregion

            Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            IEnumerable<object> handlerInstances = ResolveMediator.ResolveAll(handlerType);
            IEnumerable<IEventHandler> handlers = handlerInstances.Select(handler => (IEventHandler)handler);

            return handlers;
        }
        #endregion
    }
}
