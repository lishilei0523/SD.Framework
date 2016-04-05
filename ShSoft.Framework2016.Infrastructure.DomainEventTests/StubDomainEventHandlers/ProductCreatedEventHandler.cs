using ShSoft.Framework2016.Infrastructure.DomainEventTests.StubEventSources;
using ShSoft.Framework2016.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2016.Infrastructure.DomainEventTests.StubDomainEventHandlers
{
    /// <summary>
    /// 商品已创建事件处理者
    /// </summary>
    public class ProductCreatedEventHandler : IDomainEventHandler<ProductCreatedEvent>
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <remarks>
        /// 测试用例参数，默认为null，事件触发后会将其赋值为商品名称
        /// </remarks>
        public static string ProductName;

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
