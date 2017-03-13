using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Constants
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
        public LoginInfo() { }

        /// <summary>
        /// 创建登录信息构造器
        /// </summary>
        /// <param name="loginId">登录名</param>
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
        /// 登录名
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
    }
}
