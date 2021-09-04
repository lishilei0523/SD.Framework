using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : AggregateRootEntity
    {
        #region 密码 —— string Password
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        #endregion

        #region 私钥 —— string PrivateKey
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
        #endregion

        #region 是否启用 —— bool Enabled
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        #endregion
    }
}
