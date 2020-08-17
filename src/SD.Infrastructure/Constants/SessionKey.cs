namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// Session键
    /// </summary>
    public static class SessionKey
    {
        /// <summary>
        /// 验证码Session键
        /// </summary>
        public const string ValidCode = "ValidCode";

        /// <summary>
        /// 当前登录用户Session键
        /// </summary>
        public const string CurrentUser = "CurrentUser";

        /// <summary>
        /// 当前公钥Session键
        /// </summary>
        public const string CurrentPublishKey = "CurrentPublishKey";

        /// <summary>
        /// 当前用户菜单树Session键
        /// </summary>
        public const string CurrentMenuTree = "CurrentMenus";

        /// <summary>
        /// 当前用户权限集Session键
        /// </summary>
        public const string CurrentAuthorities = "CurrentAuthorities";
    }
}
