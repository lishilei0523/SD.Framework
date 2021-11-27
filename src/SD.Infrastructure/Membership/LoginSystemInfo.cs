using SD.Infrastructure.Constants;
using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Membership
{
    /// <summary>
    /// 登录信息系统信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class LoginSystemInfo
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LoginSystemInfo() { }

        /// <summary>
        /// 创建登录信息系统信息
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="systemName">信息系统名称</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="index">首页</param>
        public LoginSystemInfo(string systemNo, string systemName, ApplicationType applicationType, string index)
            : this()
        {
            this.Number = systemNo;
            this.Name = systemName;
            this.ApplicationType = applicationType;
            this.Index = index;
        }


        /// <summary>
        /// 信息系统编号
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// 信息系统名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        [DataMember]
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// 首页
        /// </summary>
        [DataMember]
        public string Index { get; set; }
    }
}
