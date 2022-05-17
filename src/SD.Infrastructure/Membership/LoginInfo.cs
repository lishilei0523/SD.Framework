using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Membership
{
    /// <summary>
    /// 登录信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class LoginInfo
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LoginInfo()
        {
            this.LoginSystemInfos = new HashSet<LoginSystemInfo>();
            this.LoginMenuInfos = new HashSet<LoginMenuInfo>();
            this.LoginAuthorityInfos = new HashSet<LoginAuthorityInfo>();
        }

        /// <summary>
        /// 创建登录信息构造器
        /// </summary>
        /// <param name="loginId">用户名</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="publicKey">公钥</param>
        public LoginInfo(string loginId, string realName, Guid publicKey)
            : this()
        {
            this.LoginId = loginId;
            this.RealName = realName;
            this.PublicKey = publicKey;
        }

        /// <summary>
        /// 创建登录信息构造器
        /// </summary>
        /// <param name="loginId">用户名</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="tenantNo">租户编号</param>
        /// <param name="tenantConnectionString">租户连接字符串</param>
        public LoginInfo(string loginId, string realName, Guid publicKey, string tenantNo, string tenantConnectionString)
            : this(loginId, realName, publicKey)
        {
            this.TenantNo = tenantNo;
            this.TenantConnectionString = tenantConnectionString;
        }

        /// <summary>
        /// 创建登录信息构造器
        /// </summary>
        /// <param name="loginId">用户名</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="tenantNo">租户编号</param>
        /// <param name="tenantConnectionString">租户连接字符串</param>
        /// <param name="tenantSpareConnectionString">租户备用连接字符串</param>
        public LoginInfo(string loginId, string realName, Guid publicKey, string tenantNo, string tenantConnectionString, string tenantSpareConnectionString)
            : this(loginId, realName, publicKey, tenantNo, tenantConnectionString)
        {
            this.TenantSpareConnectionString = tenantSpareConnectionString;
        }


        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string LoginId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [DataMember]
        public string RealName { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        [DataMember]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// 租户编号
        /// </summary>
        [DataMember]
        public string TenantNo { get; set; }

        /// <summary>
        /// 租户连接字符串
        /// </summary>
        [DataMember]
        public string TenantConnectionString { get; set; }

        /// <summary>
        /// 租户备用连接字符串
        /// </summary>
        [DataMember]
        public string TenantSpareConnectionString { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// 信息系统列表
        /// </summary>
        [DataMember]
        public ICollection<LoginSystemInfo> LoginSystemInfos { get; set; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        [DataMember]
        public ICollection<LoginMenuInfo> LoginMenuInfos { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        [DataMember]
        public ICollection<LoginAuthorityInfo> LoginAuthorityInfos { get; set; }
    }
}
