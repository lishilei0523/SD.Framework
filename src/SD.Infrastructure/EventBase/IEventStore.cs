using System;

namespace SD.Infrastructure.EventBase
{
    /// <summary>
    /// 领域事件存储接口
    /// </summary>
    public interface IEventStore : IDisposable
    {
        #region # 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        void Suspend<T>(T eventSource) where T : class, IEvent;
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        void HandleUncompletedEvents();
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        void ClearUncompletedEvents();
        #endregion
    }
}
