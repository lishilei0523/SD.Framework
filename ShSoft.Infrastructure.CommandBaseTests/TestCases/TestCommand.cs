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
