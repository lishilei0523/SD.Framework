using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务状态
    /// </summary>
    [Serializable]
    [DataContract]
    public enum CrontabStatus
    {
        /// <summary>
        /// 待定
        /// </summary>
        [EnumMember]
        [Description("待定")]
        Pending = 0,

        /// <summary>
        /// 已调度
        /// </summary>
        [EnumMember]
        [Description("已调度")]
        Scheduled = 1,

        /// <summary>
        /// 已暂停
        /// </summary>
        [EnumMember]
        [Description("已暂停")]
        Paused = 2
    }
}
