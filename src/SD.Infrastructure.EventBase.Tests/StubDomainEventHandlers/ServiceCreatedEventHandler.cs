using SD.Infrastructure.EventBase.Tests.StubEventSources;
using System.Threading;

namespace SD.Infrastructure.EventBase.Tests.StubDomainEventHandlers
{
    /// <summary>
    /// 服务已创建事件处理者
    /// </summary>
    public class ServiceCreatedEventHandler : IEventHandler<ServiceCreatedEvent>
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <remarks>
        /// 测试用例参数，默认为null，事件触发后会将其赋值为服务名称
        /// </remarks>
        public static readonly ThreadLocal<string> ServiceName = new ThreadLocal<string>();

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
        public void Handle(ServiceCreatedEvent eventSource)
        {
            ServiceName.Value = eventSource.ServiceName;
        }
    }
}
