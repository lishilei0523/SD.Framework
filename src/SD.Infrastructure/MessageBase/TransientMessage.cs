using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.MessageBase
{
    /// <summary>
    /// 瞬时消息基类
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class TransientMessage : IMessage
    {
        #region 标题 —— string Title
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        #endregion

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
        #endregion

        #region 发送人 —— string SenderAccount
        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public string SenderAccount { get; set; }
        #endregion

        #region 接收人 —— string[] ReceiverAccounts
        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public string[] ReceiverAccounts { get; set; }
        #endregion
    }
}
