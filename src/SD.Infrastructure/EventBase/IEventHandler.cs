namespace SD.Infrastructure.EventBase
{
    /// <summary>
    /// 领域事件处理者基接口
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        uint Sort { get; }
    }

    /// <summary>
    /// 领域事件处理者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IEventHandler<in T> : IEventHandler where T : class, IEvent
    {
        /// <summary>
        /// 领域事件处理方法
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        void Handle(T eventSource);
    }
}
