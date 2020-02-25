using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.WorkflowBase
{
    /// <summary>
    /// 持久化模式
    /// </summary>
    [Serializable]
    [DataContract]
    public enum PersistenceMode
    {
        /// <summary>
        /// 持久存储
        /// </summary>
        [EnumMember]
        [Description("持久存储")]
        Permanent = 0,

        /// <summary>
        /// 临时存储
        /// </summary>
        [EnumMember]
        [Description("临时存储")]
        Temporary = 1
    }
}
