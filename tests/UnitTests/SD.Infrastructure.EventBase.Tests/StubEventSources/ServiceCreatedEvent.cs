using System;

namespace SD.Infrastructure.EventBase.Tests.StubEventSources
{
    /// <summary>
    /// 服务已创建事件
    /// </summary>
    [Serializable]
    public class ServiceCreatedEvent : Event
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ServiceCreatedEvent() { }

        /// <summary>
        /// 基础构造器
        /// </summary>
        public ServiceCreatedEvent(string serviceNo, string serviceName, decimal price)
        {
            this.ServiceNo = serviceNo;
            this.ServiceName = serviceName;
            this.Price = price;
        }

        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServiceNo { get; private set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; private set; }

        /// <summary>
        /// 服务价格
        /// </summary>
        public decimal Price { get; private set; }
    }
}
