using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    internal class RabbitConfiguration : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly RabbitConfiguration _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RabbitConfiguration()
        {
            _Setting = (RabbitConfiguration)ConfigurationManager.GetSection("rabbitConfiguration");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("RabbitMQ节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static RabbitMQConfiguration Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static RabbitConfiguration Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 主机名称 —— string HostName
        /// <summary>
        /// 主机名称
        /// </summary>
        [ConfigurationProperty("hostName", IsRequired = true)]
        public string HostName
        {
            get { return this["hostName"].ToString(); }
            set { this["hostName"] = value; }
        }
        #endregion

        #region # 用户名 —— string UserName
        /// <summary>
        /// 用户名
        /// </summary>
        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get { return this["userName"].ToString(); }
            set { this["userName"] = value; }
        }
        #endregion

        #region # 密码 —— string Password
        /// <summary>
        /// 密码
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"].ToString(); }
            set { this["password"] = value; }
        }
        #endregion
    }
}
