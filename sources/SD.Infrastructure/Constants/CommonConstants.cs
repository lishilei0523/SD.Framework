using System;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 公共常量
    /// </summary>
    public static class CommonConstants
    {
        #region # Config文件 AppSetting键

        /// <summary>
        /// 实体所在程序集AppSetting键
        /// </summary>
        public const string EntityAssemblyAppSettingKey = "EntityAssembly";

        /// <summary>
        /// 领域事件源所在程序集AppSetting键
        /// </summary>
        public const string EventSourceAssemblyAppSettingKey = "EventSourceAssembly";

        /// <summary>
        /// 实体配置所在程序集AppSetting键
        /// </summary>
        public const string EntityConfigAssemblyAppSettingKey = "EntityConfigAssembly";

        /// <summary>
        /// 数据表名前缀AppSetting键
        /// </summary>
        public const string TablePrefixAppSettingKey = "TablePrefix";

        /// <summary>
        /// 开启数据自动迁移AppSetting键
        /// </summary>
        public const string AutoMigrationAppSettingKey = "AutoMigration";

        /// <summary>
        /// 身份认证过期时间AppSetting键
        /// </summary>
        public const string AuthenticationTimeoutAppSettingKey = "AuthenticationTimeout";

        #endregion

        #region # 数据库相关

        /// <summary>
        /// DbSession构造函数默认参数
        /// </summary>
        public static readonly string DbSessionConstructorArg = string.Format("name={0}", DefaultConnectionStringName);

        /// <summary>
        /// 默认连接字符串名称
        /// </summary>
        public const string DefaultConnectionStringName = "DefaultConnection";

        #endregion

        #region # 统一身份认证系统相关

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
        /// 日期 格式化 字符串
        /// </summary>
        public const string TimeFormat = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// 最小时间
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1901, 1, 1);

        /// <summary>
        /// 最大时间
        /// </summary>
        public static readonly DateTime MaxDateTime = new DateTime(2078, 6, 6);

        #endregion
    }
}
