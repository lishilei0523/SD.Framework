using System;

namespace ShSoft.Framework2016.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件源基接口
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// 是否已处理
        /// </summary>
        bool Handled { get; }

        /// <summary>
        /// 触发时间
        /// </summary>
        DateTime? TriggerTime { get; }

        /// <summary>
        /// 处理
        /// </summary>
        void Handle();
    }
}
