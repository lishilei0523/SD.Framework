using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Repository.Redis.Tests.Entities;
using SD.Infrastructure.Repository.Redis.Tests.IRepositories;
using SD.Infrastructure.Repository.Redis.Tests.Repositories;

namespace SD.Infrastructure.Repository.Redis.Tests.TestCases
{
    /// <summary>
    /// Redis仓储测试
    /// </summary>
    [TestClass]
    public class RedisRepositoryTests
    {
        #region # 初始化部分

        /// <summary>
        /// 商品仓储
        /// </summary>
        private IProductRepository _productRep;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            this._productRep = new ProductRepository();
            this._productRep.RemoveAll();
        }

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            this._productRep.RemoveAll();
            this._productRep.Dispose();
        }

        #endregion

        #region # 用例部分

        /// <summary>
        /// 创建
        /// </summary>
        [TestMethod]
        public void TestCreate()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            Product currentProduct = this._productRep.SingleOrDefault(product.Id);

            Assert.IsTrue(currentProduct != null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            this._productRep.Remove(product.Id);

            Assert.IsTrue(!this._productRep.Exists(product.Id));
        }

        /// <summary>
        /// 更新
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            Product product = new Product("商品2", 15);

            this._productRep.Add(product);

            Product currentProduct = this._productRep.Single(product.Id);

            currentProduct.UpdateInfo("商品2", 20);

            this._productRep.Save(currentProduct);

            Product finalProduct = this._productRep.Single(product.Id);

            Assert.IsTrue(finalProduct.Name == currentProduct.Name);
            Assert.IsTrue(finalProduct.Price == currentProduct.Price);
        }

        /// <summary>
        /// 查询
        /// </summary>
        [TestMethod]
        public void TestFind()
        {
            int count = 100;

            IList<Product> products = new List<Product>();

            for (int index = 0; index < count; index++)
            {
                Product product = new Product("商品" + index, 10 + index);
                products.Add(product);
            }

            this._productRep.AddRange(products);

            IEnumerable<Product> specProducts = this._productRep.FindAll();
            Assert.IsTrue(specProducts.Count() == count);
        }

        #endregion
    }
}
