using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventStoreProvider.EntityFrameworkTests.EventSources;

namespace ShSoft.Infrastructure.EventStoreProvider.EntityFrameworkTests.Entities
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
