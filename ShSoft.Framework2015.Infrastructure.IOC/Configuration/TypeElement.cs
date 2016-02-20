using System.Configuration;

namespace ShSoft.Framework2015.Infrastructure.IOC.Configuration
{
    /// <summary>
    /// 类型节点
    /// </summary>
    public class TypeElement : ConfigurationElement
    {
        #region # 类型名称 —— string Name
        /// <summary>
        /// 类型名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        #endregion

        #region # 所在程序集 —— string Assembly
        /// <summary>
        /// 所在程序集
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = false, IsKey = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
        #endregion
    }
}
