using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 持久化消息基类
    /// </summary>
    public abstract class PermanentMessage : AggregateRootEntity, IMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public string SenderAccount { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string[] ReceiverAccounts { get; set; }
    }
}
