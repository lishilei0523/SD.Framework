using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using SD.Infrastructure.Configurations;
using SD.Infrastructure.Constants;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigurationTests
    {
        #region # 测试初始化 —— void Initialize()
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
#if NET8_0_OR_GREATER
            Assembly assembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(assembly);
            FrameworkSection.Initialize(configuration);
#endif
        }
        #endregion

        #region # 测试配置文件 —— void TestConfiguration()
        /// <summary>
        /// 测试配置文件
        /// </summary>
        [TestMethod]
        public void TestConfiguration()
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

            Trace.WriteLine(setting.MessageAssembly.Value);
            foreach (ViewModelAssemblyElement element in setting.ViewModelAssemblyElements)
            {
                Trace.WriteLine(element.Name);
            }

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
        #endregion

        #region # 测试应用程序Id —— void TestApplicationId()
        /// <summary>
        /// 测试应用程序Id
        /// </summary>
        [TestMethod]
        public void TestApplicationId()
        {
            Trace.WriteLine(GlobalSetting.ApplicationId);

            GlobalSetting.ApplicationId = Guid.Empty.ToString();

            Trace.WriteLine(GlobalSetting.ApplicationId);

            GlobalSetting.ApplicationId = Guid.NewGuid().ToString();

            Trace.WriteLine(GlobalSetting.ApplicationId);
        }
        #endregion

        #region # 测试连接字符串 —— void TestConnectionString()
        /// <summary>
        /// 测试连接字符串
        /// </summary>
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
        #endregion
    }
}
