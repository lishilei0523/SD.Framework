using System;

namespace ShSoft.Framework2016.Infrastructure.DomainEventTests.StubEventSources
{
    /// <summary>
    /// 商品已创建事件
    /// </summary>
    [Serializable]
    public class ProductCreatedEvent : IDomainEvent.DomainEvent
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ProductCreatedEvent() { }

        /// <summary>
        /// 基础构造器
        /// </summary>
        public ProductCreatedEvent(string productNo, string productName, decimal price, DateTime? triggerTime = null)
            : base(triggerTime)
        {
            this.ProductNo = productNo;
            this.ProductName = productName;
            this.Price = price;
        }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductNo { get; private set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; private set; }
    }
}
