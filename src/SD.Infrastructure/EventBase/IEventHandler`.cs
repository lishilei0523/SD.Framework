namespace SD.Infrastructure.EventBase
{
    /// <summary>
    /// 领域事件处理者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IEventHandler<in T> : IEventHandler where T : class, IEvent
    {
        #region # 处理领域事件 —— void Handle(T eventSource)
        /// <summary>
        /// 处理领域事件
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        void Handle(T eventSource);
        #endregion
    }
}
