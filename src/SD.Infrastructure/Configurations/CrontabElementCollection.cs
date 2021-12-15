using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 定时任务节点集合
    /// </summary>
    public class CrontabElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 账号
        /// </summary>
        [ConfigurationProperty("loginId", IsRequired = false)]
        public string LoginId
        {
            get { return (string)this["loginId"]; }
            set { this["loginId"] = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CrontabElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CrontabElement)element).Type;
        }
    }
}
