using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 审核状态
    /// </summary>
    [DataContract]
    public enum CheckStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [EnumMember]
        [Description("未审核")]
        Unchecked = 0,

        /// <summary>
        /// 待审核
        /// </summary>
        [EnumMember]
        [Description("待审核")]
        Checking = 1,

        /// <summary>
        /// 审核通过
        /// </summary>
        [EnumMember]
        [Description("审核通过")]
        Passed = 2,

        /// <summary>
        /// 审核未通过
        /// </summary>
        [EnumMember]
        [Description("审核未通过")]
        Rejected = 3
    }
}
