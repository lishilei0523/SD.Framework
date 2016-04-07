using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventSources;
using ShSoft.Framework2016.Infrastructure.DomainEvent.Mediator;
using ShSoft.Framework2016.Infrastructure.IEntity;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.Entities
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : AggregateRootEntity
    {
        public Product()
        {
            EventMediator.Suspend(new ProductCreatedEvent(new ProductCreatedEventData { Name = "商品", Price = 28 }));
        }
    }
}
