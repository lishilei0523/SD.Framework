using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : AggregateRootEntity
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected User()
        {
            this.Enabled = true;
        }
        #endregion

        #region 01.创建用户构造器
        /// <summary>
        /// 创建用户构造器
        /// </summary>
        public User(string loginId, string realName, string password, string privateKey, decimal? age)
            : this()
        {
            this.Password = password;
            this.PrivateKey = privateKey;
            this.Age = age;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 密码 —— string Password
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
        #endregion

        #region 私钥 —— string PrivateKey
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; private set; }
        #endregion

        #region 年龄 —— decimal? Age
        /// <summary>
        /// 年龄
        /// </summary>
        public decimal? Age { get; private set; }
        #endregion

        #region 是否启用 —— bool Enabled
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; private set; }
        #endregion 

        #endregion
    }
}
