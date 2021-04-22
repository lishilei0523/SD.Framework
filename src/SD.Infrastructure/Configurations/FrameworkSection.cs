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
        [ConfigurationProperty("serviceName", IsRequired = false)]
        public TextElement ServiceName
        {
            get { return (TextElement)this["serviceName"]; }
            set { this["serviceName"] = value; }
        }
        #endregion

        #region # 服务显示名称节点 —— TextElement ServiceDisplayName
        /// <summary>
        /// 服务显示名称节点
        /// </summary>
        [ConfigurationProperty("serviceDisplayName", IsRequired = false)]
        public TextElement ServiceDisplayName
        {
            get { return (TextElement)this["serviceDisplayName"]; }
            set { this["serviceDisplayName"] = value; }
        }
        #endregion

        #region # 服务描述节点 —— TextElement ServiceDescription
        /// <summary>
        /// 服务描述节点
        /// </summary>
        [ConfigurationProperty("serviceDescription", IsRequired = false)]
        public TextElement ServiceDescription
        {
            get { return (TextElement)this["serviceDescription"]; }
            set { this["serviceDescription"] = value; }
        }
        #endregion

        #region # 连接字符串节点 —— TextElement ConnectionString
        /// <summary>
        /// 连接字符串节点
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = false)]
        public TextElement ConnectionString
        {
            get { return (TextElement)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }
        #endregion

        #region # 实体所在程序集节点 —— TextElement EntityAssembly
        /// <summary>
        /// 实体所在程序集节点
        /// </summary>
        [ConfigurationProperty("entityAssembly", IsRequired = false)]
        public TextElement EntityAssembly
        {
            get { return (TextElement)this["entityAssembly"]; }
            set { this["entityAssembly"] = value; }
        }
        #endregion

        #region # 实体配置所在程序集节点 —— TextElement EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集节点
        /// </summary>
        [ConfigurationProperty("entityConfigAssembly", IsRequired = false)]
        public TextElement EntityConfigAssembly
        {
            get { return (TextElement)this["entityConfigAssembly"]; }
            set { this["entityConfigAssembly"] = value; }
        }
        #endregion

        #region # 领域事件源所在程序集节点 —— TextElement EventSourceAssembly
        /// <summary>
        /// 领域事件源所在程序集节点
        /// </summary>
        [ConfigurationProperty("eventSourceAssembly", IsRequired = false)]
        public TextElement EventSourceAssembly
        {
            get { return (TextElement)this["eventSourceAssembly"]; }
            set { this["eventSourceAssembly"] = value; }
        }
        #endregion

        #region # 定时任务所在程序集节点 —— TextElement CrontabAssembly
        /// <summary>
        /// 定时任务所在程序集节点
        /// </summary>
        [ConfigurationProperty("crontabAssembly", IsRequired = false)]
        public TextElement CrontabAssembly
        {
            get { return (TextElement)this["crontabAssembly"]; }
            set { this["crontabAssembly"] = value; }
        }
        #endregion

        #region # 工作流所在程序集节点 —— TextElement WorkflowAssembly
        /// <summary>
        /// 工作流所在程序集节点
        /// </summary>
        [ConfigurationProperty("workflowAssembly", IsRequired = false)]
        public TextElement WorkflowAssembly
        {
            get { return (TextElement)this["workflowAssembly"]; }
            set { this["workflowAssembly"] = value; }
        }
        #endregion

        #region # 数据表名前缀节点 —— TextElement TablePrefix
        /// <summary>
        /// 数据表名前缀节点
        /// </summary>
        [ConfigurationProperty("tablePrefix", IsRequired = false)]
        public TextElement TablePrefix
        {
            get { return (TextElement)this["tablePrefix"]; }
            set { this["tablePrefix"] = value; }
        }
        #endregion

        #region # 身份过期时间节点 —— NumericElement AuthenticationTimeout
        /// <summary>
        /// 身份过期时间节点
        /// </summary>
        [ConfigurationProperty("authenticationTimeout", IsRequired = false)]
        public NumericElement AuthenticationTimeout
        {
            get { return (NumericElement)this["authenticationTimeout"]; }
            set { this["authenticationTimeout"] = value; }
        }
        #endregion
    }
}
