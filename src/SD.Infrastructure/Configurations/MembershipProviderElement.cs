using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// Membership提供者配置节点
    /// </summary>
    public class MembershipProviderElement : ConfigurationElement
    {
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
    }
}
