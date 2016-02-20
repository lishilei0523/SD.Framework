using ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEventSources;
using ShSoft.Framework2015.Infrastructure.IEntity;

namespace ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEntities
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : AggregateRootEntity
    {
        /// <summary>
        /// 创建商品构造器
        /// </summary>
        /// <param name="productNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="price">价格</param>
        public Product(string productNo, string productName, decimal price)
        {
            base.Number = productNo;
            base.Name = productName;
            this.Price = price;

            EventMediator.Handle(new ProductCreatedEvent(this.Number, this.Name, this.Price) as IDomainEvent.IDomainEvent);
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; private set; }
    }
}
