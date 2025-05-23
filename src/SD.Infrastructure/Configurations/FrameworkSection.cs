﻿using SD.Infrastructure.Configurations;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure
{
    /// <summary>
    /// SD.Framework配置
    /// </summary>
    public class FrameworkSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static FrameworkSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static FrameworkSection()
        {
            _Setting = null;
        }

        #endregion

        #region # 初始化 —— static void Initialize(Configuration configuration)
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration">配置</param>
        public static void Initialize(Configuration configuration)
        {
            #region # 验证

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "配置不可为空！");
            }

            #endregion

            _Setting = (FrameworkSection)configuration.GetSection("sd.framework");
        }
        #endregion

        #region # 访问器 —— static FrameworkSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static FrameworkSection Setting
        {
            get
            {
                if (_Setting == null)
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    _Setting = (FrameworkSection)configuration.GetSection("sd.framework");
                }
                if (_Setting == null)
                {
                    throw new ApplicationException("SD.Framework节点未配置，请检查程序！");
                }

                return _Setting;
            }
        }
        #endregion

        #region # 应用程序Id节点 —— TextElement ApplicationId
        /// <summary>
        /// 应用程序Id节点
        /// </summary>
        [ConfigurationProperty("application.id", IsRequired = false)]
        public TextElement ApplicationId
        {
            get { return (TextElement)this["application.id"]; }
            set { this["application.id"] = value; }
        }
        #endregion

        #region # 应用程序名称节点 —— TextElement ApplicationName
        /// <summary>
        /// 应用程序名称节点
        /// </summary>
        [ConfigurationProperty("application.name", IsRequired = false)]
        public TextElement ApplicationName
        {
            get { return (TextElement)this["application.name"]; }
            set { this["application.name"] = value; }
        }
        #endregion

        #region # 应用程序版本节点 —— TextElement ApplicationVersion
        /// <summary>
        /// 应用程序版本节点
        /// </summary>
        [ConfigurationProperty("application.version", IsRequired = false)]
        public TextElement ApplicationVersion
        {
            get { return (TextElement)this["application.version"]; }
            set { this["application.version"] = value; }
        }
        #endregion

        #region # 服务名称节点 —— TextElement ServiceName
        /// <summary>
        /// 服务名称节点
        /// </summary>
        [ConfigurationProperty("service.name", IsRequired = false)]
        public TextElement ServiceName
        {
            get { return (TextElement)this["service.name"]; }
            set { this["service.name"] = value; }
        }
        #endregion

        #region # 服务显示名称节点 —— TextElement ServiceDisplayName
        /// <summary>
        /// 服务显示名称节点
        /// </summary>
        [ConfigurationProperty("service.displayName", IsRequired = false)]
        public TextElement ServiceDisplayName
        {
            get { return (TextElement)this["service.displayName"]; }
            set { this["service.displayName"] = value; }
        }
        #endregion

        #region # 服务描述节点 —— TextElement ServiceDescription
        /// <summary>
        /// 服务描述节点
        /// </summary>
        [ConfigurationProperty("service.description", IsRequired = false)]
        public TextElement ServiceDescription
        {
            get { return (TextElement)this["service.description"]; }
            set { this["service.description"] = value; }
        }
        #endregion

        #region # 数据库读连接名称节点 —— TextElement DatabaseReadConnectionName
        /// <summary>
        /// 数据库读连接名称节点
        /// </summary>
        [ConfigurationProperty("database.read.connectionName", IsRequired = false)]
        public TextElement DatabaseReadConnectionName
        {
            get { return (TextElement)this["database.read.connectionName"]; }
            set { this["database.read.connectionName"] = value; }
        }
        #endregion

        #region # 数据库写连接名称节点 —— TextElement DatabaseWriteConnectionName
        /// <summary>
        /// 数据库写连接名称节点
        /// </summary>
        [ConfigurationProperty("database.write.connectionName", IsRequired = false)]
        public TextElement DatabaseWriteConnectionName
        {
            get { return (TextElement)this["database.write.connectionName"]; }
            set { this["database.write.connectionName"] = value; }
        }
        #endregion

        #region # 数据库分区数量节点 —— NumericElement DatabasePartitionsCount
        /// <summary>
        /// 数据库分区数量节点
        /// </summary>
        [ConfigurationProperty("database.partitions.count", IsRequired = false)]
        public NumericElement DatabasePartitionsCount
        {
            get { return (NumericElement)this["database.partitions.count"]; }
            set { this["database.partitions.count"] = value; }
        }
        #endregion

        #region # 实体所在程序集节点 —— TextElement EntityAssembly
        /// <summary>
        /// 实体所在程序集节点
        /// </summary>
        [ConfigurationProperty("entity.assembly", IsRequired = false)]
        public TextElement EntityAssembly
        {
            get { return (TextElement)this["entity.assembly"]; }
            set { this["entity.assembly"] = value; }
        }
        #endregion

        #region # 实体配置所在程序集节点 —— TextElement EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集节点
        /// </summary>
        [ConfigurationProperty("entity.config.assembly", IsRequired = false)]
        public TextElement EntityConfigAssembly
        {
            get { return (TextElement)this["entity.config.assembly"]; }
            set { this["entity.config.assembly"] = value; }
        }
        #endregion

        #region # 实体数据表名前缀节点 —— TextElement EntityTablePrefix
        /// <summary>
        /// 实体数据表名前缀节点
        /// </summary>
        [ConfigurationProperty("entity.tablePrefix", IsRequired = false)]
        public TextElement EntityTablePrefix
        {
            get { return (TextElement)this["entity.tablePrefix"]; }
            set { this["entity.tablePrefix"] = value; }
        }
        #endregion

        #region # 消息所在程序集节点 —— TextElement MessageAssembly
        /// <summary>
        /// 消息所在程序集节点
        /// </summary>
        [ConfigurationProperty("message.assembly", IsRequired = false)]
        public TextElement MessageAssembly
        {
            get { return (TextElement)this["message.assembly"]; }
            set { this["message.assembly"] = value; }
        }
        #endregion

        #region # 视图模型程序集节点列表 —— ViewModelAssemblyElementCollection ViewModelAssemblyElements
        /// <summary>
        /// 视图模型程序集节点列表
        /// </summary>
        [ConfigurationProperty("viewModel.assemblies")]
        [ConfigurationCollection(typeof(CrontabStrategyElementCollection), AddItemName = "assembly")]
        public ViewModelAssemblyElementCollection ViewModelAssemblyElements
        {
            get
            {
                ViewModelAssemblyElementCollection collection = this["viewModel.assemblies"] as ViewModelAssemblyElementCollection;
                return collection ?? new ViewModelAssemblyElementCollection();
            }
            set { this["viewModel.assemblies"] = value; }
        }
        #endregion

        #region # 工作流所在程序集节点 —— TextElement WorkflowAssembly
        /// <summary>
        /// 工作流所在程序集节点
        /// </summary>
        [ConfigurationProperty("workflow.assembly", IsRequired = false)]
        public TextElement WorkflowAssembly
        {
            get { return (TextElement)this["workflow.assembly"]; }
            set { this["workflow.assembly"] = value; }
        }
        #endregion

        #region # 工作流持久化连接名称节点 —— TextElement WorkflowConnectionName
        /// <summary>
        /// 工作流持久化连接名称节点
        /// </summary>
        [ConfigurationProperty("workflow.connectionName", IsRequired = false)]
        public TextElement WorkflowConnectionName
        {
            get { return (TextElement)this["workflow.connectionName"]; }
            set { this["workflow.connectionName"] = value; }
        }
        #endregion

        #region # 工作流持久化模式节点 —— TextElement WorkflowPersistenceMode
        /// <summary>
        /// 工作流持久化模式节点
        /// </summary>
        [ConfigurationProperty("workflow.persistenceMode", IsRequired = false)]
        public TextElement WorkflowPersistenceMode
        {
            get { return (TextElement)this["workflow.persistenceMode"]; }
            set { this["workflow.persistenceMode"] = value; }
        }
        #endregion

        #region # 工作流最大实例锁定重试次数节点 —— NumericElement WorkflowMaxInstanceLockedRetriesCount
        /// <summary>
        /// 工作流最大实例锁定重试次数节点
        /// </summary>
        [ConfigurationProperty("workflow.maxInstanceLockedRetriesCount", IsRequired = false)]
        public NumericElement WorkflowMaxInstanceLockedRetriesCount
        {
            get { return (NumericElement)this["workflow.maxInstanceLockedRetriesCount"]; }
            set { this["workflow.maxInstanceLockedRetriesCount"] = value; }
        }
        #endregion

        #region # 身份过期时间节点 —— NumericElement AuthenticationTimeout
        /// <summary>
        /// 身份过期时间节点
        /// </summary>
        [ConfigurationProperty("authentication.timeout", IsRequired = false)]
        public NumericElement AuthenticationTimeout
        {
            get { return (NumericElement)this["authentication.timeout"]; }
            set { this["authentication.timeout"] = value; }
        }
        #endregion

        #region # 授权是否启用节点 —— BooleanElement AuthorizationEnabled
        /// <summary>
        /// 授权是否启用节点
        /// </summary>
        [ConfigurationProperty("authorization.enabled", IsRequired = false)]
        public BooleanElement AuthorizationEnabled
        {
            get { return (BooleanElement)this["authorization.enabled"]; }
            set { this["authorization.enabled"] = value; }
        }
        #endregion

        #region # 定时任务所在程序集节点 —— TextElement CrontabAssembly
        /// <summary>
        /// 定时任务所在程序集节点
        /// </summary>
        [ConfigurationProperty("crontab.assembly", IsRequired = false)]
        public TextElement CrontabAssembly
        {
            get { return (TextElement)this["crontab.assembly"]; }
            set { this["crontab.assembly"] = value; }
        }
        #endregion

        #region # 定时任务账户账号节点 —— TextElement CrontabAccountLoginId
        /// <summary>
        /// 定时任务账户账号节点
        /// </summary>
        [ConfigurationProperty("crontab.account.loginId", IsRequired = false)]
        public TextElement CrontabAccountLoginId
        {
            get { return (TextElement)this["crontab.account.loginId"]; }
            set { this["crontab.account.loginId"] = value; }
        }
        #endregion

        #region # 定时任务账户密码节点 —— TextElement CrontabAccountPassword
        /// <summary>
        /// 定时任务账户密码节点
        /// </summary>
        [ConfigurationProperty("crontab.account.password", IsRequired = false)]
        public TextElement CrontabAccountPassword
        {
            get { return (TextElement)this["crontab.account.password"]; }
            set { this["crontab.account.password"] = value; }
        }
        #endregion

        #region # 定时任务策略节点列表 —— CrontabStrategyElementCollection CrontabStrategyElements
        /// <summary>
        /// 定时任务策略节点列表
        /// </summary>
        [ConfigurationProperty("crontab.strategies")]
        [ConfigurationCollection(typeof(CrontabStrategyElementCollection), AddItemName = "strategy")]
        public CrontabStrategyElementCollection CrontabStrategyElements
        {
            get
            {
                CrontabStrategyElementCollection collection = this["crontab.strategies"] as CrontabStrategyElementCollection;
                return collection ?? new CrontabStrategyElementCollection();
            }
            set { this["crontab.strategies"] = value; }
        }
        #endregion

        #region # 外部服务 - 自动更新服务节点 —— TextElement AutoUpdateService
        /// <summary>
        /// 外部服务 - 自动更新服务节点
        /// </summary>
        [ConfigurationProperty("external.service.autoUpdate", IsRequired = false)]
        public TextElement AutoUpdateService
        {
            get { return (TextElement)this["external.service.autoUpdate"]; }
            set { this["external.service.autoUpdate"] = value; }
        }
        #endregion

        #region # 外部服务 - 文件服务节点 —— TextElement FileService
        /// <summary>
        /// 外部服务 - 文件服务节点
        /// </summary>
        [ConfigurationProperty("external.service.file", IsRequired = false)]
        public TextElement FileService
        {
            get { return (TextElement)this["external.service.file"]; }
            set { this["external.service.file"] = value; }
        }
        #endregion

        #region # 外部服务 - 消息服务节点 —— TextElement MessageService
        /// <summary>
        /// 外部服务 - 消息服务节点
        /// </summary>
        [ConfigurationProperty("external.service.message", IsRequired = false)]
        public TextElement MessageService
        {
            get { return (TextElement)this["external.service.message"]; }
            set { this["external.service.message"] = value; }
        }
        #endregion

        #region # 外部服务 - OPC服务节点 —— TextElement OpcService
        /// <summary>
        /// 外部服务 - OPC服务节点
        /// </summary>
        [ConfigurationProperty("external.service.opc", IsRequired = false)]
        public TextElement OpcService
        {
            get { return (TextElement)this["external.service.opc"]; }
            set { this["external.service.opc"] = value; }
        }
        #endregion

        #region # 外部服务 - 定时任务服务节点 —— TextElement CrontabService
        /// <summary>
        /// 外部服务 - 定时任务服务节点
        /// </summary>
        [ConfigurationProperty("external.service.crontab", IsRequired = false)]
        public TextElement CrontabService
        {
            get { return (TextElement)this["external.service.crontab"]; }
            set { this["external.service.crontab"] = value; }
        }
        #endregion

        #region # Membership提供者节点 —— MembershipProviderElement MembershipProvider
        /// <summary>
        /// Membership提供者节点
        /// </summary>
        [ConfigurationProperty("membership.provider", IsRequired = false)]
        public MembershipProviderElement MembershipProvider
        {
            get { return (MembershipProviderElement)this["membership.provider"]; }
            set { this["membership.provider"] = value; }
        }
        #endregion
    }
}
