using System;
using System.Configuration;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.RabbitMQStorer.Configuration
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public class RabbitMQConfiguration : ConfigurationSection
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

        #region # 类型 —— string Type
        /// <summary>
        /// 类型
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
        #endregion

        #region # 程序集 —— string Assembly
        /// <summary>
        /// 程序集
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return this["assembly"].ToString(); }
            set { this["assembly"] = value; }
        }
        #endregion
    }
}
