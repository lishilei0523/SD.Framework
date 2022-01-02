using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Constants;
using System.Diagnostics;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConnectionStringTests
    {
        [TestInitialize]
        public void Initialize()
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(assembly);
            //FrameworkSection.Initialize(configuration);
        }

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
