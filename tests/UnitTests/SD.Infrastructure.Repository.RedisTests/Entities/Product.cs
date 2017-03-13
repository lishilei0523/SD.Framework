using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.Repository.Redis.Tests.Entities
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : AggregateRootEntity
    {
        protected Product() { }

        public Product(string productName, decimal price)
        {
            this.Name = productName;
            this.Price = price;
        }

        public decimal Price { get; private set; }

        public void UpdateInfo(string productName, decimal price)
        {
            this.Name = productName;
            this.Price = price;
        }
    }
}
