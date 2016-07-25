﻿namespace ShSoft.Infrastructure.Constants
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
    }
}
