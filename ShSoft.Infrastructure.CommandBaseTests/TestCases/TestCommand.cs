using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Infrastructure.CommandBase.Mediator;
using ShSoft.Infrastructure.CommandBaseTests.Commands;

namespace ShSoft.Infrastructure.CommandBaseTests.TestCases
{
    /// <summary>
    /// 测试执行命令
    /// </summary>
    [TestClass]
    public class TestCommand
    {
        /// <summary>
        /// 同步测试
        /// </summary>
        [TestMethod]
        public void TestSync()
        {
            CreateProductCommand cmd = new CreateProductCommand { Asynchronous = false, Price = 10, ProductName = "商品1", ProductNo = "123" };

            cmd.Handle();
            //CommandMediator.Execute(cmd);

            Assert.IsTrue(cmd.Price == 20);
        }

        /// <summary>
        /// 异步测试
        /// </summary>
        [TestMethod]
        public void TestAsync()
        {
            CreateProductCommand cmd = new CreateProductCommand { Asynchronous = true, Price = 10, ProductName = "商品1", ProductNo = "123" };

            CommandMediator.Execute(cmd);
            Thread.Sleep(1000);

            Assert.IsTrue(cmd.Price == 20);
        }
    }
}
