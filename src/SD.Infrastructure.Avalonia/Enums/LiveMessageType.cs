using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Avalonia.Enums
{
    /// <summary>
    /// 实时消息类型
    /// </summary>
    [DataContract]
    public enum LiveMessageType
    {
        /// <summary>
        /// 信息
        /// </summary>
        [EnumMember]
        [Description("信息")]
        Info = 0,

        /// <summary>
        /// 提示
        /// </summary>
        [EnumMember]
        [Description("提示")]
        Alert = 1,

        /// <summary>
        /// 警告
        /// </summary>
        [EnumMember]
        [Description("警告")]
        Warning = 2,

        /// <summary>
        /// 错误
        /// </summary>
        [EnumMember]
        [Description("错误")]
        Error = 3
    }
}
