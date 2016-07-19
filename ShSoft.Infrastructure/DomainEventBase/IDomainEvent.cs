using ShSoft.Infrastructure.MessageBase;

namespace ShSoft.Infrastructure.DomainEventBase
{
    /// <summary>
    /// 领域事件源基接口
    /// </summary>
    public interface IDomainEvent : IMessage
    {
        #region 事件源数据序列化字符串 —— string SourceDataStr
        /// <summary>
        /// 事件源数据序列化字符串
        /// </summary>
        string SourceDataStr { get; }
        #endregion
    }
}
