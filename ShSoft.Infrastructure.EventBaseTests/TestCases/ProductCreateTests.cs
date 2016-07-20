using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventBaseTests.StubEntities;
using ShSoft.Infrastructure.EventBaseTests.StubEventHandlers;
using ShSoft.Infrastructure.EventBaseTests.StubEventSources;
using ShSoft.Infrastructure.Global;

namespace ShSoft.Infrastructure.EventBaseTests.TestCases
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

            EventMediator.Handle((IEvent)eventSource);

            //断言会触发领域事件，并修改目标参数的值
            Assert.IsTrue(ProductCreatedEventHandler.ProductName == eventSource.ProductName);
        }
    }
}
