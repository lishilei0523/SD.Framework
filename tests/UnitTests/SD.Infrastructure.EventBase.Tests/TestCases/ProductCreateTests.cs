using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.EventBase.Mediator;
using SD.Infrastructure.EventBase.Tests.StubDomainEventHandlers;
using SD.Infrastructure.EventBase.Tests.StubEntities;
using SD.Infrastructure.Global;
using System.Threading.Tasks;
using System.Transactions;

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
            Parallel.For(0, 50, i =>
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Initializer.InitSessionId();

                    Product product = new Product(i.ToString(), "测试商品" + i, 19);

                    EventMediator.HandleUncompletedEvents();

                    //断言会触发领域事件，并修改目标参数的值
                    Assert.IsTrue(ProductCreatedEventHandler.ProductName.Value == product.Name);
                    Assert.IsTrue(ServiceCreatedEventHandler.ServiceName.Value == product.Name);

                    scope.Complete();
                }
            });
        }
    }
}
