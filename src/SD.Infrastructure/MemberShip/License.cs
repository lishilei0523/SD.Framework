using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.MemberShip
{
    /// <summary>
    /// 许可证
    /// </summary>
    [Serializable]
    [DataContract]
    public struct License
    {
        /// <summary>
        /// 创建许可证构造器
        /// </summary>
        /// <param name="enterpriseName">企业名称</param>
        /// <param name="uniqueCode">唯一码</param>
        /// <param name="expiredDate">过期日期</param>
        public License(string enterpriseName, string uniqueCode, DateTime expiredDate)
            : this()
        {
            this.EnterpriseName = enterpriseName;
            this.UniqueCode = uniqueCode;
            this.ExpiredDate = expiredDate;
        }

        /// <summary>
        /// 企业名称
        /// </summary>
        [DataMember]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        [DataMember]
        public string UniqueCode { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [DataMember]
        public DateTime ExpiredDate { get; set; }
    }
}
