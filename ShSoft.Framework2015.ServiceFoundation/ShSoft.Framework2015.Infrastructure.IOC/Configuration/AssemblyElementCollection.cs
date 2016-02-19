using System.Configuration;

namespace ShSoft.Framework2015.Infrastructure.IOC.Configuration
{
    /// <summary>
    /// 程序集节点集合
    /// </summary>
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Name;
        }
    }
}
