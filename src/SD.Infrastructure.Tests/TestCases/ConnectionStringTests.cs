using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Constants;
using System.Diagnostics;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConnectionStringTests
    {
        [TestMethod]
        public void TestConnectionString()
        {
            string connectionString = GlobalSetting.DefaultConnectionString;
            Trace.WriteLine(connectionString);

            Assert.IsNotNull(connectionString);
        }
    }
}
