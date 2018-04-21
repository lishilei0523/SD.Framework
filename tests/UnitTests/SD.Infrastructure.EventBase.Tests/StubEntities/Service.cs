using SD.Infrastructure.EntityBase;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.StubEventSources;

namespace SD.Infrastructure.EventBase.Tests.StubEntities
{
    /// <summary>
    /// 服务
    /// </summary>
    public class Service : AggregateRootEntity
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Service() { }

        /// <summary>
        /// 创建服务构造器
        /// </summary>
        /// <param name="serviceNo">服务编号</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="price">价格</param>
        public Service(string serviceNo, string serviceName, decimal price)
            : this()
        {
            base.Number = serviceNo;
            base.Name = serviceName;
            this.Price = price;

            //挂起领域事件
            EventMediator.Suspend(new ServiceCreatedEvent(this.Number, this.Name, this.Price));
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; private set; }
    }
}
