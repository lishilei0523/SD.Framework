using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.StubDomainEventHandlers;
using SD.Infrastructure.EventBase.Tests.StubEntities;
using SD.Infrastructure.Global;

namespace SD.Infrastructure.EventBase.Tests.TestCases
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
            for (int i = 0; i < 100; i++)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Initializer.InitSessionId();

                    Product product = new Product(i.ToString(), "测试商品" + i, 19);

                    EventMediator.HandleUncompletedEvents();

                    //断言会触发领域事件，并修改目标参数的值
                    Assert.IsTrue(ProductCreatedEventHandler.ProductName == product.Name);
                    Assert.IsTrue(ProductCreatedEvent2Handler.ProductName == product.Name);

                    scope.Complete();
                }
            }
        }
    }
}
