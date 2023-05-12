using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using SD.Infrastructure.Configurations;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigurationTests
    {
        /// <summary>
        /// 初始化测试
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //初始化配置文件
            Assembly assembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(assembly);
            FrameworkSection.Initialize(configuration);
        }

        /// <summary>
        /// 测试配置文件
        /// </summary>
        [TestMethod]
        public void TestConfigurations()
        {
            FrameworkSection setting = FrameworkSection.Setting;

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
            }

            Trace.WriteLine(setting.AutoUpdateService.Value);
            Trace.WriteLine(setting.FileService.Value);
            Trace.WriteLine(setting.MessageService.Value);
            Trace.WriteLine(setting.OpcService.Value);
            Trace.WriteLine(setting.CrontabService.Value);

            Assert.IsNotNull(setting);
        }
    }
}
