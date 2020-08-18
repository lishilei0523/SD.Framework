using System.Configuration;

namespace SD.Infrastructure.SignalR.Server.Configurations
{
    /// <summary>
    /// 节点
    /// </summary>
    public class HostElement : ConfigurationElement
    {
        #region # 链接地址 —— string Url
        /// <summary>
        /// 链接地址
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true, IsKey = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
        #endregion
    }
}
