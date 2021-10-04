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
            string readConnectionString = GlobalSetting.ReadConnectionString;
            string writeConnectionString = GlobalSetting.WriteConnectionString;
            Trace.WriteLine(readConnectionString);
            Trace.WriteLine(writeConnectionString);

            Assert.IsNotNull(readConnectionString);
            Assert.IsNotNull(writeConnectionString);
        }
    }
}
