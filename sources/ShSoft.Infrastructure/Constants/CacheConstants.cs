namespace ShSoft.Infrastructure.Constants
{
    /// <summary>
    /// 缓存常量，
    /// 定义缓存键
    /// </summary>
    public static class CacheConstants
    {
        /// <summary>
        /// 会话Id缓存键
        /// </summary>
        public const string SessionIdKey = "SessionIdKey";

        /// <summary>
        /// 验证码Session键
        /// </summary>
        public const string ValidCodeKey = "ValidCodeKey";

        /// <summary>
        /// 当前用户信息Session键
        /// </summary>
        public const string CurrentUserKey = "CurrentUserKey";

        /// <summary>
        /// 当前用户权限Session键
        /// </summary>
        public const string CurrentAuthoritiesKey = "CurrentUserAuthoritiesKey";

        /// <summary>
        /// 当前用户菜单Session键
        /// </summary>
        public const string CurrentMenusKey = "CurrentUserAuthoritiesKey";
    }
}
