using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubDomainEventHandlers;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEntities;
using ShSoft.Framework2015.Infrastructure.DomainEventTests.StubEventSources;

namespace ShSoft.Framework2015.Infrastructure.DomainEventTests.TestCases
{
    /// <summary>
    /// 商品创建测试
    /// </summary>
    [TestClass]
    public class ProductCreateTests
    {
        /// <summary>
        /// 测试商品创建
        /// </summary>
        [TestMethod]
        public void CreateProduct()
        {
            Product product = new Product("001", "测试商品1", 19);

            EventMediator.HandleUncompletedEvents();

            //断言会触发领域事件，并修改目标参数的值
            Assert.IsTrue(ProductCreatedEventHandler.ProductName == product.Name);
        }

        /// <summary>
        /// 测试商品已创建事件源的Handle
        /// </summary>
        [TestMethod]
        public void CreateProductCreatedEvent()
        {
            ProductCreatedEvent eventSource = new ProductCreatedEvent("001", "测试商品1", 19);
            eventSource.Handle();

            //断言会触发领域事件，并修改目标参数的值
            Assert.IsTrue(ProductCreatedEventHandler.ProductName == eventSource.ProductName);
        }
    }
}
