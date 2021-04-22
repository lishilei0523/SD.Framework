using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class GlobalSetting
    {
        #region # 默认连接字符串 —— static string DefaultConnectionString

        /// <summary>
        /// 默认连接字符串
        /// </summary>
        private static readonly string _DefaultConnectionString =
            ConfigurationManager.ConnectionStrings[CommonConstants.DefaultConnectionStringName]?.ConnectionString;

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

        #region # 定时任务所在程序集 —— static readonly string CrontabAssembly
        /// <summary>
        /// 定时任务所在程序集
        /// </summary>
        public static readonly string CrontabAssembly =
            ConfigurationManager.AppSettings[CommonConstants.CrontabAssemblyAppSettingKey];
        #endregion

        #region # 工作流所在程序集 —— static readonly string WorkflowAssembly
        /// <summary>
        /// 工作流所在程序集
        /// </summary>
        public static readonly string WorkflowAssembly =
            ConfigurationManager.AppSettings[CommonConstants.WorkflowAssemblyAppSettingKey];
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

        #region # SessionId缓存键 —— const string SessionIdKey
        /// <summary>
        /// SessionId缓存键
        /// </summary>
        public const string SessionIdKey = "SessionIdKey";
        #endregion

        #region # 当前SessionId —— static Guid CurrentSessionId
        /// <summary>
        /// 当前SessionId
        /// </summary>
        public static Guid CurrentSessionId
        {
            get
            {
                object sessionIdCache = CallContext.LogicalGetData(SessionIdKey);

                if (sessionIdCache == null)
                {
                    throw new ApplicationException("SessionId未设置，请检查程序！");
                }

                return (Guid)sessionIdCache;
            }
        }
        #endregion

        #region # 置空当前SessionId —— static void FreeCurrentSessionId()
        /// <summary>
        /// 置空当前SessionId
        /// </summary>
        public static void FreeCurrentSessionId()
        {
            CallContext.FreeNamedDataSlot(SessionIdKey);
        }
        #endregion

        #region # 初始化当前SessionId —— static void InitCurrentSessionId()
        /// <summary>
        /// 初始化当前SessionId
        /// </summary>
        public static void InitCurrentSessionId()
        {
            Guid sessionId = Guid.NewGuid();
            CallContext.LogicalSetData(SessionIdKey, sessionId);
        }
        #endregion

        #region # 初始化数据文件夹 —— static void InitDataDirectory()
        /// <summary>
        /// 初始化数据文件夹
        /// </summary>
        public static void InitDataDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData(CommonConstants.DataDirectory, baseDirectory);
        }
        #endregion
    }
}
