using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 视图模型程序集节点集合
    /// </summary>
    public class ViewModelAssemblyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns>配置节点</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewModelAssemblyElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewModelAssemblyElement)element).Name;
        }
    }
}
