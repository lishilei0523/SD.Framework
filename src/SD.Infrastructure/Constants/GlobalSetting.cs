using System;
using System.Configuration;
using System.Threading;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class GlobalSetting
    {
        #region # 字段及静态构造器

        /// <summary>
        /// SeesionId线程静态字段
        /// </summary>
        private static readonly AsyncLocal<Guid> _SessionId;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GlobalSetting()
        {
            _SessionId = new AsyncLocal<Guid>();
        }

        #endregion

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

        #region # 当前SessionId —— static Guid CurrentSessionId
        /// <summary>
        /// 当前SessionId
        /// </summary>
        public static Guid CurrentSessionId
        {
            get
            {
                if (_SessionId.Value == default(Guid))
                {
                    throw new ApplicationException("SessionId未设置，请检查程序！");
                }

                return _SessionId.Value;
            }
        }
        #endregion

        #region # 置空当前SessionId —— static void FreeCurrentSessionId()
        /// <summary>
        /// 置空当前SessionId
        /// </summary>
        public static void FreeCurrentSessionId()
        {
            _SessionId.Value = default(Guid);
        }
        #endregion

        #region # 初始化当前SessionId —— static void InitCurrentSessionId()
        /// <summary>
        /// 初始化当前SessionId
        /// </summary>
        public static void InitCurrentSessionId()
        {
            _SessionId.Value = Guid.NewGuid();
        }
        #endregion
    }
}
