using System;
using ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventSources;
using ShSoft.Framework2016.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventHandlers
{
    public class ProductCreatedEventHandler : IDomainEventHandler<ProductCreatedEvent>
    {
        /// <summary>
        /// 领域事件处理方法（同步执行）
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        public void Handle(ProductCreatedEvent eventSource)
        {
            Console.WriteLine(eventSource.ProductCreatedEventData.Name);
            Console.WriteLine(eventSource.ProductCreatedEventData.Price);
        }

        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        public uint Sort { get { return uint.MaxValue; } }
    }
}
