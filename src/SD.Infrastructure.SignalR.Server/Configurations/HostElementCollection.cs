using System.Configuration;

namespace SD.Infrastructure.SignalR.Server.Configurations
{
    /// <summary>
    /// 节点集合
    /// </summary>
    public class HostElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new HostElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HostElement)element).Url;
        }
    }
}
