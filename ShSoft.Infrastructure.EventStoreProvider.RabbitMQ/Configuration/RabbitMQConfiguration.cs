using System;
using System.Configuration;

namespace ShSoft.Infrastructure.EventStoreProvider.RabbitMQ.Configuration
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    internal class RabbitMQConfiguration : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly RabbitMQConfiguration _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RabbitMQConfiguration()
        {
            _Setting = (RabbitMQConfiguration)ConfigurationManager.GetSection("rabbitMQConfiguration");

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
        public static RabbitMQConfiguration Setting
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
