using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.CommandBase.Mediator;
using SD.Infrastructure.CommandBase.Tests.Commands;

namespace SD.Infrastructure.CommandBase.Tests.TestCases
{
    /// <summary>
    /// 测试执行命令
    /// </summary>
    [TestClass]
    public class TestCommand
    {
        /// <summary>
        /// 测试
        /// </summary>
        [TestMethod]
        public void TestSync()
        {
            CreateProductCommand cmd = new CreateProductCommand { Price = 10, ProductName = "商品1", ProductNo = "123" };

            CommandMediator.Execute(cmd);

            Assert.IsTrue(cmd.Price == 20);
        }
    }
}
