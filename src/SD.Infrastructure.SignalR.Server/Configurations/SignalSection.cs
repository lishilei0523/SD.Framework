using SD.Infrastructure.SignalR.Server.Configurations;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.SignalR
{
    /// <summary>
    /// SignalR服务器配置
    /// </summary>
    public class SignalSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly SignalSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static SignalSection()
        {
            _Setting = (SignalSection)ConfigurationManager.GetSection("signalrConfiguration");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("SignalR节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static SignalSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static SignalSection Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 是否需授权 —— bool Authorized
        /// <summary>
        /// 是否需授权
        /// </summary>
        [ConfigurationProperty("authorized", IsRequired = true, IsKey = true)]
        public bool Authorized
        {
            get { return (bool)this["authorized"]; }
            set { this["authorized"] = value; }
        }
        #endregion

        #region # 节点地址列表 —— EndpointElementCollection EndpointElement
        /// <summary>
        /// 节点地址列表
        /// </summary>
        [ConfigurationProperty("hosts")]
        [ConfigurationCollection(typeof(HostElementCollection), AddItemName = "host")]
        public HostElementCollection HostElement
        {
            get
            {
                HostElementCollection collection = this["hosts"] as HostElementCollection;
                return collection ?? new HostElementCollection();
            }
            set { this["hosts"] = value; }
        }
        #endregion
    }
}
