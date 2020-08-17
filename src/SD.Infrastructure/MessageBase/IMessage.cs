namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息基接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 内容
        /// </summary>
        string Content { get; }

        /// <summary>
        /// 发送人
        /// </summary>
        string SenderAccount { get; }

        /// <summary>
        /// 接收人
        /// </summary>
        string[] ReceiverAccounts { get; }
    }
}
