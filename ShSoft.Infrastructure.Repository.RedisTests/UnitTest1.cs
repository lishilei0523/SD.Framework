using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Infrastructure.Repository.RedisTests.Entities;
using ShSoft.Infrastructure.Repository.RedisTests.IRepositories;
using ShSoft.Infrastructure.Repository.RedisTests.Repositories;

namespace ShSoft.Infrastructure.Repository.RedisTests
{
    [TestClass]
    public class UnitTest1
    {
        private IProductRepository _productRep;

        [TestInitialize]
        public void Init()
        {
            this._productRep = new ProductRepository();
            this._productRep.RemoveAll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this._productRep.RemoveAll();
        }


        [TestMethod]
        public void TestCreate()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            IEnumerable<Product> products = this._productRep.FindAll();

            Assert.IsTrue(products.Any());
        }

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

        [TestMethod]
        public void TestRemove()
        {
            Product product = new Product("商品1", 16);

            this._productRep.Add(product);

            this._productRep.Remove(product.Id);

            Assert.IsTrue(!this._productRep.Exists(product.Id));
        }
    }
}
