using SD.Infrastructure.Configurations;
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
        private static readonly FrameworkSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static FrameworkSection()
        {
            _Setting = (FrameworkSection)ConfigurationManager.GetSection("sd.framework");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("SD.Framework节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static FrameworkSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static FrameworkSection Setting
        {
            get { return _Setting; }
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
            set { this["service.Name"] = value; }
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

        #region # 服务连接名称节点 —— TextElement ServiceConnectionName
        /// <summary>
        /// 服务连接名称节点
        /// </summary>
        [ConfigurationProperty("service.connectionName", IsRequired = false)]
        public TextElement ServiceConnectionName
        {
            get { return (TextElement)this["service.connectionName"]; }
            set { this["service.connectionName"] = value; }
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

        #region # 领域事件源所在程序集节点 —— TextElement EventSourceAssembly
        /// <summary>
        /// 领域事件源所在程序集节点
        /// </summary>
        [ConfigurationProperty("eventSource.assembly", IsRequired = false)]
        public TextElement EventSourceAssembly
        {
            get { return (TextElement)this["eventSource.assembly"]; }
            set { this["eventSource.assembly"] = value; }
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
    }
}
