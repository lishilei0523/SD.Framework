using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void TestConfigurations()
        {
            FrameworkSection setting = FrameworkSection.Setting;

            Trace.WriteLine(setting.ServiceName.Value);
            Trace.WriteLine(setting.ServiceDisplayName.Value);
            Trace.WriteLine(setting.ServiceDescription.Value);
            Trace.WriteLine(setting.EntityAssembly.Value);
            Trace.WriteLine(setting.EntityConfigAssembly.Value);
            Trace.WriteLine(setting.EventSourceAssembly.Value);
            Trace.WriteLine(setting.CrontabAssembly.Value);
            Trace.WriteLine(setting.WorkflowAssembly.Value);
            Trace.WriteLine(setting.TablePrefix.Value);
            Trace.WriteLine(setting.AuthenticationTimeout.Value);

            Assert.IsNotNull(setting);
        }
    }
}
