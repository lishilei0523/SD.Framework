using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Configurations;
using System.Diagnostics;

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

            Trace.WriteLine(setting.ApplicationId.Value);
            Trace.WriteLine(setting.ApplicationName.Value);
            Trace.WriteLine(setting.ApplicationVersion.Value);

            Trace.WriteLine(setting.ServiceName.Value);
            Trace.WriteLine(setting.ServiceDisplayName.Value);
            Trace.WriteLine(setting.ServiceDescription.Value);

            Trace.WriteLine(setting.DatabaseReadConnectionName.Value);
            Trace.WriteLine(setting.DatabaseWriteConnectionName.Value);
            Trace.WriteLine(setting.DatabasePartitionsCount.Value);

            Trace.WriteLine(setting.EntityAssembly.Value);
            Trace.WriteLine(setting.EntityConfigAssembly.Value);
            Trace.WriteLine(setting.EntityTablePrefix.Value);

            Trace.WriteLine(setting.WorkflowAssembly.Value);
            Trace.WriteLine(setting.WorkflowConnectionName.Value);
            Trace.WriteLine(setting.WorkflowPersistenceMode.Value);
            Trace.WriteLine(setting.WorkflowMaxInstanceLockedRetriesCount.Value);

            Trace.WriteLine(setting.AuthenticationTimeout.Value);
            Trace.WriteLine(setting.AuthorizationEnabled.Value);

            Trace.WriteLine(setting.CrontabAssembly.Value);
            Trace.WriteLine(setting.CrontabAccountLoginId.Value);
            Trace.WriteLine(setting.CrontabAccountPassword.Value);
            foreach (CrontabStrategyElement element in setting.CrontabStrategyElements)
            {
                Trace.WriteLine(element.Type);
                Trace.WriteLine(element.StrategyType);
                Trace.WriteLine(element.Strategy);
                Trace.WriteLine(element.Enabled);
            }

            Trace.WriteLine(setting.AutoUpdateService.Value);
            Trace.WriteLine(setting.FileService.Value);
            Trace.WriteLine(setting.MessageService.Value);
            Trace.WriteLine(setting.OpcService.Value);
            Trace.WriteLine(setting.CrontabService.Value);

            Trace.WriteLine(setting.MembershipProvider.Type);

            Assert.IsNotNull(setting);
        }
    }
}
