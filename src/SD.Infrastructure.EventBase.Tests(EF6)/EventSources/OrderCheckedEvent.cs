using System;

namespace SD.Infrastructure.EventBase.Tests.EventSources
{
    /// <summary>
    /// 单据已审核建事件
    /// </summary>
    [Serializable]
    public class OrderCheckedEvent : Event
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected OrderCheckedEvent() { }

        /// <summary>
        /// 基础构造器
        /// </summary>
        public OrderCheckedEvent(string orderNo)
            : this()
        {
            this.OrderNo = orderNo;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string OrderNo { get; private set; }
    }
}
