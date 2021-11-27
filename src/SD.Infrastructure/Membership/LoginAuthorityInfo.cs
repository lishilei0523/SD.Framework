using SD.Infrastructure.Constants;
using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Membership
{
    /// <summary>
    /// 登录权限信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class LoginAuthorityInfo
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LoginAuthorityInfo() { }

        /// <summary>
        /// 创建登录权限信息构造器
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorityPath">权限路径</param>
        public LoginAuthorityInfo(string systemNo, ApplicationType applicationType, string authorityName, string authorityPath)
            : this()
        {
            this.SystemNo = systemNo;
            this.ApplicationType = applicationType;
            this.Name = authorityName;
            this.Path = authorityPath;
        }


        /// <summary>
        /// 信息系统编号
        /// </summary>
        [DataMember]
        public string SystemNo { get; set; }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        [DataMember]
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 权限路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }
}
