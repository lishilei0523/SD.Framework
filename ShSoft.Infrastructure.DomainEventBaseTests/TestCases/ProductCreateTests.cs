using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Infrastructure.DomainEventBase.Mediator;
using ShSoft.Infrastructure.DomainEventBaseTests.StubDomainEventHandlers;
using ShSoft.Infrastructure.DomainEventBaseTests.StubEntities;
using ShSoft.Infrastructure.DomainEventBaseTests.StubEventSources;
using ShSoft.Infrastructure.Global;

namespace ShSoft.Infrastructure.DomainEventBaseTests.TestCases
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
            Initializer.InitSessionId();

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
            Initializer.InitSessionId();

            ProductCreatedEvent eventSource = new ProductCreatedEvent("001", "测试商品1", 19);
            eventSource.Handle();

            //断言会触发领域事件，并修改目标参数的值
            Assert.IsTrue(ProductCreatedEventHandler.ProductName == eventSource.ProductName);
        }
    }
}
