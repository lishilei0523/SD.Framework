using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.Infrastructure.IOC.Mediator;
using ShSoft.Framework2015.Infrastructure.StubIAppService.Interfaces;

namespace ShSoft.Framework2015.Infrastructure.IOCTests
{
    /// <summary>
    /// 测试依赖注入
    /// </summary>
    [TestClass]
    public class TestIOC
    {
        /// <summary>
        /// 测试解析实例方法
        /// </summary>
        [TestMethod]
        public void TestResolveType()
        {
            object productContract = ResolverMediator.Resolve(typeof(IProductContract));
        }

        /// <summary>
        /// 测试解析实例方法
        /// </summary>
        [TestMethod]
        public void TestResolveOptionalType()
        {
            object productContract = ResolverMediator.ResolveOptional(typeof(IProductContract));
            Assert.IsNotNull(productContract);
        }

        /// <summary>
        /// 测试解析实例泛型方法
        /// </summary>
        [TestMethod]
        public void TestResolveGeneric()
        {
            IProductContract productContract = ResolverMediator.Resolve<IProductContract>();
        }

        /// <summary>
        /// 测试解析实例泛型方法
        /// </summary>
        [TestMethod]
        public void TestResolveOptionalGeneric()
        {
            IProductContract productContract = ResolverMediator.ResolveOptional<IProductContract>();
            Assert.IsNotNull(productContract);
        }
    }
}
