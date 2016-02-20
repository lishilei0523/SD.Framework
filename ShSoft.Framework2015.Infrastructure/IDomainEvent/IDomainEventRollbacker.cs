namespace ShSoft.Framework2015.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件回滚者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IDomainEventRollbacker<in T> where T : class, IDomainEvent
    {
        /// <summary>
        /// 领域事件回滚方法
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        void Rollback(T eventSource);
    }
}
