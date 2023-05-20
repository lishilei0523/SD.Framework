using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.Repository.MongoDB.Tests.Entities
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product : AggregateRootEntity
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Product() { }
        #endregion

        #region 01.创建商品构造器
        /// <summary>
        /// 创建商品构造器
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="price">价格</param>
        public Product(string productName, decimal price)
            : this()
        {
            this.Name = productName;
            this.Price = price;
        }
        #endregion

        #endregion

        #region # 属性

        #region 价格 —— decimal Price
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改商品 —— void UpdateInfo(string productName...
        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="price">价格</param>
        public void UpdateInfo(string productName, decimal price)
        {
            this.Name = productName;
            this.Price = price;
        }
        #endregion 

        #endregion
    }
}
