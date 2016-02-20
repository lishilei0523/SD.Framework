using System.Runtime.Serialization;

namespace ShSoft.Framework2015.Infrastructure.Messages
{
    /// <summary>
    /// 响应状态
    /// </summary>
    [DataContract]
    public enum ResponseStatus
    {
        /// <summary>
        /// 响应成功
        /// </summary>
        [EnumMember]
        Success = 0,

        /// <summary>
        /// 客户端错误
        /// </summary>
        [EnumMember]
        ClientError = 1,

        /// <summary>
        /// 服务器错误
        /// </summary>
        [EnumMember]
        ServerError = 2
    }
}
