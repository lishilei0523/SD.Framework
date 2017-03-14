using System.Runtime.Serialization;

namespace SD.Infrastructure.CommandBase.Tests.Commands
{
    /// <summary>
    /// 创建商品命令
    /// </summary>
    [DataContract]
    public class CreateProductCommand : Command
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        [DataMember]
        public string ProductNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
    }
}
