using SD.Infrastructure.EntityBase;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.StubEventSources;

namespace SD.Infrastructure.EventBase.Tests.StubEntities
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : AggregateRootEntity
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Product() { }

        /// <summary>
        /// 创建商品构造器
        /// </summary>
        /// <param name="productNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="price">价格</param>
        public Product(string productNo, string productName, decimal price)
            : this()
        {
            base.Number = productNo;
            base.Name = productName;
            this.Price = price;

            //发起领域事件
            EventMediator.Suspend(new ProductCreatedEvent(this.Number, this.Name, this.Price));
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; private set; }
    }
}
