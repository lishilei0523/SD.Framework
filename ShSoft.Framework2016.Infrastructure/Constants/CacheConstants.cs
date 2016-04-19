namespace ShSoft.Framework2016.Infrastructure.Constants
{
    /// <summary>
    /// 缓存常量，
    /// 定义缓存键
    /// </summary>
    public static class CacheConstants
    {
        /// <summary>
        /// 验证码Session键
        /// </summary>
        public const string ValidCodeKey = "ValidCode";

        /// <summary>
        /// 当前登录用户Session键
        /// </summary>
        public const string CurrentUserKey = "CurrentUser";

        /// <summary>
        /// 会话Id缓存键
        /// </summary>
        public const string SessionIdKey = "SessionIdKey";

        /// <summary>
        /// 公钥Session键
        /// </summary>
        public const string PublishKeySessionKey = "PublishKeySessionKey";

        /// <summary>
        /// WCF身份认证消息头名称
        /// </summary>
        public const string WcfAuthHeaderName = "WcfAuthHeaderName";

        /// <summary>
        /// WCF身份认证消息头Ns
        /// </summary>
        public const string WcfAuthHeaderNs = "WcfAuthHeaderNs";
    }
}
