using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 视图模型程序集节点
    /// </summary>
    public class ViewModelAssemblyElement : ConfigurationElement
    {
        #region # 程序集名称 —— string Name
        /// <summary>
        /// 程序集名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        #endregion
    }
}
