using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Repository.MongoDB.Tests.Entities;
using SD.Infrastructure.Repository.MongoDB.Tests.IRepositories;
using SD.Infrastructure.Repository.MongoDB.Tests.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.Repository.MongoDB.Tests.TestCases
{
    /// <summary>
    /// MongoDB仓储测试
    /// </summary>
    [TestClass]
    public class MongoRepositoryTests
    {
        #region # 测试初始化

        /// <summary>
        /// 商品仓储接口
        /// </summary>
        private IProductRepository _productRep;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._productRep = new ProductRepository();
        }

        #endregion

        #region # 测试清理 —— void CleanUp()
        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            this._productRep.Dispose();
        }
        #endregion

        #region # 测试创建 —— void TestCreate()
        /// <summary>
        /// 测试创建
        /// </summary>
        [TestMethod]
        public void TestCreate()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            Product currentProduct = this._productRep.SingleOrDefault(product.Id);

            Assert.IsTrue(currentProduct != null);
        }
        #endregion

        #region # 测试删除 —— void TestRemove()
        /// <summary>
        /// 测试删除
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            this._productRep.Remove(product.Id);

            Assert.IsTrue(!this._productRep.Exists(product.Id));
        }
        #endregion

        #region # 测试更新 —— void TestUpdate()
        /// <summary>
        /// 测试更新
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            Product product = new Product("商品2", 15);

            this._productRep.Add(product);

            Product currentProduct = this._productRep.Single(product.Id);

            currentProduct.UpdateInfo("商品3", 20);

            this._productRep.Save(currentProduct);

            Product finalProduct = this._productRep.Single(product.Id);

            Assert.IsTrue(finalProduct.Name == currentProduct.Name);
            Assert.IsTrue(finalProduct.Price == currentProduct.Price);
        }
        #endregion

        #region # 测试查询 —— void TestFind()
        /// <summary>
        /// 测试查询
        /// </summary>
        [TestMethod]
        public void TestFind()
        {
            int count = 10000;

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
