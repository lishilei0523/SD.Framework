using System;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 公共常量
    /// </summary>
    public static class CommonConstants
    {
        #region # 数据库相关

        /// <summary>
        /// MongoDB连接字符串名称
        /// </summary>
        public const string MongoConnectionStringName = "MongoConnection";

        /// <summary>
        /// 数据文件夹名称
        /// </summary>
        public const string DataDirectory = "DataDirectory";

        #endregion

        #region # 身份认证系统相关

        /// <summary>
        /// 超级管理员用户名
        /// </summary>
        public const string AdminLoginId = "admin";

        /// <summary>
        /// 初始密码
        /// </summary>
        public const string InitialPassword = "888888";

        /// <summary>
        /// WCF身份认证消息头名称
        /// </summary>
        public const string WCFAuthenticationHeader = "AuthenticationHeader";

        /// <summary>
        /// 默认应用程序Id
        /// </summary>
        internal const string DefaultApplicationId = "00000000-0000-0000-0000-000000000000";

        #endregion

        #region # 其他

        /// <summary>
        /// 日期格式
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// 时间格式
        /// </summary>
        public const string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// 日期时间格式
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly TimeSpan MinTime = new TimeSpan(00, 00, 00);

        /// <summary>
        /// 最大时间
        /// </summary>
        public static readonly TimeSpan MaxTime = new TimeSpan(23, 59, 59);

        /// <summary>
        /// 最小日期
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(1901, 1, 1);

        /// <summary>
        /// 最大日期
        /// </summary>
        public static readonly DateTime MaxDate = new DateTime(2078, 6, 6);

        /// <summary>
        /// 基元数据类型数组
        /// </summary>
        public static readonly string[] PrimitiveDataTypes =
        {
            typeof(string).FullName,
            typeof(bool).FullName,
            typeof(byte).FullName,
            typeof(short).FullName,
            typeof(int).FullName,
            typeof(long).FullName,
            typeof(float).FullName,
            typeof(double).FullName,
            typeof(decimal).FullName,
            typeof(Guid).FullName,
            typeof(DateTime).FullName,
            typeof(TimeSpan).FullName
        };

        #endregion
    }
}
