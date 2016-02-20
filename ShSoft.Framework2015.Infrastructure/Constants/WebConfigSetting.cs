using System.Configuration;

namespace ShSoft.Framework2015.Infrastructure.Constants
{
    /// <summary>
    /// WebConfig设置
    /// </summary>
    public sealed class WebConfigSetting
    {
        #region # 默认连接字符串 —— static readonly string DefaultConnectionString
        /// <summary>
        /// 默认连接字符串
        /// </summary>
        public static readonly string DefaultConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.DefaultConnectionStringName].ConnectionString;
        #endregion

        #region # 事件源连接字符串 —— static readonly string EventSourceConnectionString
        /// <summary>
        /// 事件源连接字符串
        /// </summary>
        public static readonly string EventSourceConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.EventSourceConnectionStringName].ConnectionString;
        #endregion

        #region # 日志连接字符串 —— static readonly string LogConnectionString
        /// <summary>
        /// 日志连接字符串
        /// </summary>
        public static readonly string LogConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.LogConnectionStringName].ConnectionString;
        #endregion

        #region # 实体所在程序集 —— static readonly string EntityAssembly
        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public static readonly string EntityAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EntityAssemblyAppSettingKey];
        #endregion

        #region # 领域事件源所在程序集 —— static readonly string EventSourceAssembly
        /// <summary>
        /// 领域事件源所在程序集
        /// </summary>
        public static readonly string EventSourceAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EventSourceAssemblyAppSettingKey];
        #endregion

        #region # 实体配置所在程序集 —— static readonly string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public static readonly string EntityConfigAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EntityConfigAssemblyAppSettingKey];
        #endregion

        #region # 领域事件源配置所在程序集 —— static readonly string EventSourceConfigAssembly
        /// <summary>
        /// 领域事件源配置所在程序集
        /// </summary>
        public static readonly string EventSourceConfigAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EventSourceConfigAssemblyAppSettingKey];
        #endregion

        #region # 数据表名前缀 —— static readonly string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public static readonly string TablePrefix =
            ConfigurationManager.AppSettings[CommonConstants.TablePrefixAppSettingKey];
        #endregion
    }
}
