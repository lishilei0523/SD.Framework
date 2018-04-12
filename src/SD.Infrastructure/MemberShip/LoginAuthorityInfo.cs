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
        /// <param name="authorityName">权限名称</param>
        /// <param name="englishName">权限英文名称</param>
        /// <param name="authorityPath">权限路径</param>
        public LoginAuthorityInfo(string authorityName, string englishName, string authorityPath)
            : this()
        {
            this.AuthorityName = authorityName;
            this.EnglishName = englishName;
            this.AuthorityPath = authorityPath;
        }


        /// <summary>
        /// 权限名称
        /// </summary>
        [DataMember]
        public string AuthorityName { get; set; }

        /// <summary>
        /// 权限英文名称
        /// </summary>
        [DataMember]
        public string EnglishName { get; set; }

        /// <summary>
        /// 权限路径
        /// </summary>
        [DataMember]
        public string AuthorityPath { get; set; }
    }
}
