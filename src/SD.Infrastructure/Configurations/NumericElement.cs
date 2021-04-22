using System.Configuration;
using System.Xml;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 数值节点
    /// </summary>
    public class NumericElement : ConfigurationElement
    {
        /// <summary>
        /// 数值
        /// </summary>
        [ConfigurationProperty("data", IsRequired = true)]
        public int Value
        {
            get { return (int)this["data"]; }
            set { this["data"] = value; }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            this.Value = (int)reader.ReadElementContentAs(typeof(int), null);
        }
    }
}
