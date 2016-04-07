using ShSoft.Framework2016.Common.PoweredByLee;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorerTests.EventSources
{
    /// <summary>
    /// 商品已创建事件
    /// </summary>
    public class ProductCreatedEvent : IDomainEvent.DomainEvent
    {
        protected ProductCreatedEvent()
        {


        }

        public ProductCreatedEvent(ProductCreatedEventData eventData)
        {
            this.SourceDataStr = eventData.ToJson();
        }


        public ProductCreatedEventData ProductCreatedEventData
        {
            get { return this.SourceDataStr.JsonToObject<ProductCreatedEventData>(); }
        }
    }


    public class ProductCreatedEventData
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
