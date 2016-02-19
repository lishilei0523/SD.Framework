using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEventSources;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2015.Infrastructure.DomainEventTests.StubDomainEventHandlers
{
    public class ProductCreatedEventHandler : IDomainEventHandler<ProductCreatedEvent>
    {
        public static string ProductName = null;


        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        public uint Sort
        {
            get { return uint.MaxValue; }
        }

        /// <summary>
        /// 领域事件处理方法（同步执行）
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        public void Handle(ProductCreatedEvent eventSource)
        {
            ProductName = eventSource.ProductName;
        }
    }
}
