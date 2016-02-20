using System;
using ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator;

namespace ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEventSources
{
    /// <summary>
    /// 商品创建事件
    /// </summary>
    public class ProductCreatedEvent : IDomainEvent.DomainEvent
    {
        /// <summary>
        /// 基础构造器
        /// </summary>
        public ProductCreatedEvent(string productNo, string productName, decimal price, DateTime? triggerTime = null)
            : base(triggerTime)
        {
            this.ProductNo = productNo;
            this.ProductName = productName;
            this.Price = price;
        }

        public string ProductNo { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }


        ///// <summary>
        ///// 处理
        ///// </summary>
        //public override void Handle()
        //{
        //    EventMediator.Handle(this);
        //}
    }
}
