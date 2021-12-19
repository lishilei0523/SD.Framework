using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 定时任务策略节点集合
    /// </summary>
    public class CrontabStrategyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CrontabStrategyElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CrontabStrategyElement)element).Type;
        }
    }
}
