using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 动作类型
    /// </summary>
    [Serializable]
    [DataContract]
    public enum ActionType
    {
        /// <summary>
        /// 创建
        /// </summary>
        [EnumMember]
        [Description("创建")]
        Create = 4,

        /// <summary>
        /// 修改
        /// </summary>
        [EnumMember]
        [Description("修改")]
        Update = 16,

        /// <summary>
        /// 删除
        /// </summary>
        [EnumMember]
        [Description("删除")]
        Delete = 8
    }
}
