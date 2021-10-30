using SD.Infrastructure.EntityBase;
using System;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 持久化消息基类
    /// </summary>
    [Serializable]
    public abstract class PermanentMessage : AggregateRootEntity, IMessage
    {
        #region 标题 —— string Title
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        #endregion

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        #endregion

        #region 发送人 —— string SenderAccount
        /// <summary>
        /// 发送人
        /// </summary>
        public string SenderAccount { get; set; }
        #endregion

        #region 接收人 —— string[] ReceiverAccounts
        /// <summary>
        /// 接收人
        /// </summary>
        public abstract string[] ReceiverAccounts { get; }
        #endregion
    }
}
