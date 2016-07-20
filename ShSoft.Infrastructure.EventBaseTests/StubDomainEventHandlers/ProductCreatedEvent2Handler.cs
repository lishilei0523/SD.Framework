using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBaseTests.StubEventSources;

namespace ShSoft.Infrastructure.EventBaseTests.StubDomainEventHandlers
{
    /// <summary>
    /// 商品已创建事件处理者
    /// </summary>
    public class ProductCreatedEvent2Handler : IEventHandler<ProductCreatedEvent2>
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
        public void Handle(ProductCreatedEvent2 eventSource)
        {
            ProductName = eventSource.ProductName;
        }
    }
}
