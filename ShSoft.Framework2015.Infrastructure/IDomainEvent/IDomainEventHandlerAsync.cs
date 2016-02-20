namespace ShSoft.Framework2015.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件异步处理者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IDomainEventHandlerAsync<in T> where T : class, IDomainEvent
    {
        /// <summary>
        /// 领域事件处理方法（异步执行）
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        void HandleAsync(T eventSource);
    }
}
