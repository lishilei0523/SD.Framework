using System.Configuration;
using System.Xml;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 布尔节点
    /// </summary>
    public class BooleanElement : ConfigurationElement
    {
        /// <summary>
        /// 数值
        /// </summary>
        [ConfigurationProperty("data", IsRequired = true)]
        public bool? Value
        {
            get { return (bool?)this["data"]; }
            set { this["data"] = value; }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            string content = reader.ReadElementContentAs(typeof(string), null)?.ToString();
            if (bool.TryParse(content, out bool value))
            {
                this.Value = value;
            }
            else
            {
                this.Value = null;
            }
        }
    }
}
