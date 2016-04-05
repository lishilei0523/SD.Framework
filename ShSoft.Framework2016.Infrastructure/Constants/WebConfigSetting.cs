using System;
using System.Configuration;

namespace ShSoft.Framework2016.Infrastructure.Constants
{
    /// <summary>
    /// WebConfig设置
    /// </summary>
    public static class WebConfigSetting
    {
        #region # 默认连接字符串 —— static string DefaultConnectionString

        /// <summary>
        /// 默认连接字符串
        /// </summary>
        private static readonly string _DefaultConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.DefaultConnectionStringName] == null
            ? null
            : ConfigurationManager.ConnectionStrings[CommonConstants.DefaultConnectionStringName].ConnectionString;

        /// <summary>
        /// 默认连接字符串
        /// </summary>
        public static string DefaultConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DefaultConnectionString))
                {
                    throw new NullReferenceException("默认连接字符串未配置，请联系管理员！");
                }

                return _DefaultConnectionString;
            }
        }

        #endregion

        #region # 事件源连接字符串 —— static string EventSourceConnectionString

        /// <summary>
        /// 事件源连接字符串
        /// </summary>
        private static readonly string _EventSourceConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.EventSourceConnectionStringName] == null
            ? null
            : ConfigurationManager.ConnectionStrings[CommonConstants.EventSourceConnectionStringName].ConnectionString;

        /// <summary>
        /// 事件源连接字符串
        /// </summary>
        public static string EventSourceConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EventSourceConnectionString))
                {
                    throw new NullReferenceException("事件源连接字符串未配置，请联系管理员！");
                }

                return _EventSourceConnectionString;
            }
        }

        #endregion

        #region # 日志连接字符串 —— static string LogConnectionString

        /// <summary>
        /// 日志连接字符串
        /// </summary>
        private static readonly string _LogConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.LogConnectionStringName] == null
            ? null
            : ConfigurationManager.ConnectionStrings[CommonConstants.LogConnectionStringName].ConnectionString;


        /// <summary>
        /// 日志连接字符串
        /// </summary>
        public static string LogConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LogConnectionString))
                {
                    throw new NullReferenceException("日志连接字符串未配置，请联系管理员！");
                }

                return _LogConnectionString;
            }
        }

        #endregion

        #region # 实体所在程序集 —— static readonly string EntityAssembly
        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public static readonly string EntityAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EntityAssemblyAppSettingKey];
        #endregion

        #region # 实体配置所在程序集 —— static readonly string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public static readonly string EntityConfigAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EntityConfigAssemblyAppSettingKey];
        #endregion

        #region # 领域事件源所在程序集 —— static readonly string EventSourceAssembly
        /// <summary>
        /// 领域事件源所在程序集
        /// </summary>
        public static readonly string EventSourceAssembly =
            ConfigurationManager.AppSettings[CommonConstants.EventSourceAssemblyAppSettingKey];
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
