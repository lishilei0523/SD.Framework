using System.Configuration;

namespace ShSoft.Framework2015.Infrastructure.IOC.Configuration
{
    /// <summary>
    /// 程序集节点
    /// </summary>
    public class AssemblyElement : ConfigurationElement
    {
        #region # 程序集名称 —— string Name
        /// <summary>
        /// 程序集名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        #endregion
    }
}
