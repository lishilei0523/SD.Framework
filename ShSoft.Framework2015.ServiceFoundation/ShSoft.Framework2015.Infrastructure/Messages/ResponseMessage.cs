using System;
using System.Runtime.Serialization;

namespace ShSoft.Framework2015.Infrastructure.Messages
{
    /// <summary>
    /// 应用程序服务层响应消息模型
    /// </summary>
    [DataContract]
    public class ResponseMessage
    {
        #region 01.响应状态 —— ResponseStatus ResponseStatus
        /// <summary>
        /// 响应状态
        /// </summary>
        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
        #endregion

        #region 02.消息 —— string Message
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        #endregion

        #region 03.错误日志Id —— Guid LogId
        /// <summary>
        /// 错误日志Id
        /// </summary>
        [DataMember]
        public Guid LogId { get; set; }
        #endregion
    }
}
