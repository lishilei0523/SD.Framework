namespace ShSoft.Framework2016.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件同步处理者基接口
    /// </summary>
    public interface IDomainEventHandler
    {
        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        uint Sort { get; }
    }

    /// <summary>
    /// 领域事件同步处理者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IDomainEventHandler<in T> : IDomainEventHandler where T : class, IDomainEvent
    {
        /// <summary>
        /// 领域事件处理方法（同步执行）
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        void Handle(T eventSource);
    }
}
