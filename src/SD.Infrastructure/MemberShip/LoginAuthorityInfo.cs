using SD.Infrastructure.Constants;
using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.MemberShip
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
        /// <param name="authorityId">权限Id</param>
        /// <param name="authorityName">权限名称</param>
        /// <param name="authorityPath">权限路径</param>
        /// <param name="englishName">英文名称</param>
        public LoginAuthorityInfo(string systemNo, ApplicationType applicationType, Guid authorityId, string authorityName, string authorityPath, string englishName)
            : this()
        {
            this.SystemNo = systemNo;
            this.ApplicationType = applicationType;
            this.Id = authorityId;
            this.Name = authorityName;
            this.Path = authorityPath;
            this.EnglishName = englishName;
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
        /// 权限Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

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

        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string EnglishName { get; set; }
    }
}
