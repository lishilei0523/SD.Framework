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
        /// 默认连接字符串名称
        /// </summary>
        public const string DefaultConnectionStringName = "DefaultConnection";

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
        /// 超级管理员登录名
        /// </summary>
        public const string AdminLoginId = "admin";

        /// <summary>
        /// 系统管理员角色编号
        /// </summary>
        public const string ManagerRoleNo = "SystemAdmin";

        /// <summary>
        /// 初始密码
        /// </summary>
        public const string InitialPassword = "888888";

        /// <summary>
        /// WCF身份认证消息头名称
        /// </summary>
        public const string WcfAuthHeaderName = "WcfAuthHeaderName";

        /// <summary>
        /// WCF身份认证消息头命名空间
        /// </summary>
        public const string WcfAuthHeaderNamespace = "WcfAuthHeaderNamespace";

        #endregion

        #region # 其他

        /// <summary>
        /// 消息交换者后缀
        /// </summary>
        public const string HubSuffix = "Hub";

        /// <summary>
        /// 交换消息方法名称
        /// </summary>
        public const string ExchangeMethodName = "Exchange";

        /// <summary>
        /// 日期格式
        /// </summary>
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1901, 1, 1);

        /// <summary>
        /// 最大时间
        /// </summary>
        public static readonly DateTime MaxDateTime = new DateTime(2078, 6, 6);

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
            typeof(DateTime).FullName
        };

        #endregion
    }
}
