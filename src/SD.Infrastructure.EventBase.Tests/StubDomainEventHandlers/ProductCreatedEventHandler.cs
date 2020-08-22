using SD.Infrastructure.EventBase.Tests.StubEntities;
using SD.Infrastructure.EventBase.Tests.StubEventSources;
using System.Diagnostics;
using System.Threading;

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
        public static ThreadLocal<string> ProductName = new ThreadLocal<string>();

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
                ProductName.Value = eventSource.ProductName;

                Service service = new Service(eventSource.ProductNo, eventSource.ProductName, eventSource.Price);
                Trace.WriteLine(service);
            }
        }
    }
}
