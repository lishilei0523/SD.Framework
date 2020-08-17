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
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public string SenderAccount { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public string[] ReceiverAccounts { get; set; }
    }
}
