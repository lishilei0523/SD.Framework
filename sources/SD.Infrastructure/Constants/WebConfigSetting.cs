using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace SD.Infrastructure.Constants
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

        #region # 身份认证过期时间 —— static readonly string AuthenticationTimeout
        /// <summary>
        /// 身份认证过期时间
        /// </summary>
        public static readonly string AuthenticationTimeout =
            ConfigurationManager.AppSettings[CommonConstants.AuthenticationTimeoutAppSettingKey];
        #endregion

        #region # 当前SessionId —— static Guid CurrentSessionId
        /// <summary>
        /// 当前SessionId
        /// </summary>
        public static Guid CurrentSessionId
        {
            get
            {
                object sessionIdCache = CallContext.GetData(CacheConstants.SessionIdKey);

                if (sessionIdCache == null)
                {
                    throw new ApplicationException("SessionId未设置，请检查程序！");
                }

                return (Guid)sessionIdCache;
            }
        }
        #endregion
    }
}
