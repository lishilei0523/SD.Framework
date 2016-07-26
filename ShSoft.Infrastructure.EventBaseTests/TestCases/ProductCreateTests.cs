using System.Threading.Tasks;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventBaseTests.StubDomainEventHandlers;
using ShSoft.Infrastructure.EventBaseTests.StubEntities;
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
            Parallel.For(0, 2000, index =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Initializer.InitSessionId();

                    Product product = new Product("001", "测试商品1", 19);

                    EventMediator.HandleUncompletedEvents();

                    //断言会触发领域事件，并修改目标参数的值
                    Assert.IsTrue(ProductCreatedEventHandler.ProductName == product.Name);
                    Assert.IsTrue(ProductCreatedEvent2Handler.ProductName == product.Name);

                    scope.Complete();
                }
            });
        }
    }
}
