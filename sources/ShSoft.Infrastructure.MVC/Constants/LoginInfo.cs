using System;

namespace ShSoft.Infrastructure.MVC.Constants
{
    /// <summary>
    /// 登录信息
    /// </summary>
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
        public string LoginId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public Guid PublicKey { get; set; }
    }
}
