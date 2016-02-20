using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubDomainEventHandlers;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEntities;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEventSources;

namespace ShSoft.Framework2015.Infrastructure.DomainEventTests.TestCases
{
    [TestClass]
    public class ProductCreateTests
    {
        [TestMethod]
        public void CreateProduct()
        {

            Product product = new Product("001", "测试商品1", 19);

            Assert.IsTrue(ProductCreatedEventHandler.ProductName == product.Name);
        }

        [TestMethod]
        public void CreateProductCreatedEvent()
        {

            ProductCreatedEvent eventSource = new ProductCreatedEvent("001", "测试商品1", 19);
            eventSource.Handle();

            Assert.IsTrue(ProductCreatedEventHandler.ProductName == eventSource.ProductName);
        }
    }
}
