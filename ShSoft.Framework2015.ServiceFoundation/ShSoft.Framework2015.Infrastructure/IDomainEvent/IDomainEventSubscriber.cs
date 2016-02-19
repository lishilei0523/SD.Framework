namespace ShSoft.Framework2015.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件订阅者基接口
    /// </summary>
    /// <typeparam name="T">领域事件源类型</typeparam>
    public interface IDomainEventSubscriber<in T> where T : class, IDomainEvent
    {

    }
}
