namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 消息基接口
    /// </summary>
    public interface IMessage
    {
        #region 标题 —— string Title
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }
        #endregion

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        string Content { get; }
        #endregion

        #region 发送人 —— string SenderAccount
        /// <summary>
        /// 发送人
        /// </summary>
        string SenderAccount { get; }
        #endregion

        #region 接收人 —— string[] ReceiverAccounts
        /// <summary>
        /// 接收人
        /// </summary>
        string[] ReceiverAccounts { get; }
        #endregion
    }
}
