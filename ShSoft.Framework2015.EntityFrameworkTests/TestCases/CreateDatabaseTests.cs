using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.EntityFrameworkTests.Entities;

namespace ShSoft.Framework2015.EntityFrameworkTests.TestCases
{
    /// <summary>
    /// 创建数据库测试
    /// </summary>
    [TestClass]
    public class CreateDatabaseTests
    {
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            //删除数据库
            DbSession dbSession = new DbSession();
            dbSession.Database.Delete();
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        [TestMethod]
        public void CreateDataBase()
        {
            //初始化
            DbSession dbSession = new DbSession();

            //断言数据库已创建成功
            Assert.IsTrue(dbSession.Database.Exists());
        }
    }
}
