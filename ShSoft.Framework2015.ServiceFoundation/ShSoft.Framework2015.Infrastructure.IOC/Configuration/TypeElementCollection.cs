using System.Configuration;

namespace ShSoft.Framework2015.Infrastructure.IOC.Configuration
{
    /// <summary>
    /// 类型节点集合
    /// </summary>
    public class TypeElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns>新配置节点</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeElement)element).Name;
        }
    }
}
