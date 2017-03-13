using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.StubEventSources;

namespace SD.Infrastructure.EventBase.Tests.StubDomainEventHandlers
{
    /// <summary>
    /// 商品已创建事件处理者
    /// </summary>
    public class ProductCreatedEventHandler : IEventHandler<ProductCreatedEvent>
    {
        private static readonly object _Sync = new object();

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
        /// 领域事件处理方法
        /// </summary>
        /// <param name="eventSource">领域事件源</param>
        public void Handle(ProductCreatedEvent eventSource)
        {
            lock (_Sync)
            {
                ProductName = eventSource.ProductName;

                EventMediator.Suspend(new ProductCreatedEvent2(eventSource.ProductNo, eventSource.ProductName, eventSource.Price));
            }
        }
    }
}
