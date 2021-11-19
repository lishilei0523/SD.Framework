﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Trace.WriteLine(setting.ApplicationName.Value);
            Trace.WriteLine(setting.ApplicationVersion.Value);
            Trace.WriteLine(setting.ServiceName.Value);
            Trace.WriteLine(setting.ServiceDisplayName.Value);
            Trace.WriteLine(setting.ServiceDescription.Value);
            Trace.WriteLine(setting.DatabaseReadConnectionName.Value);
            Trace.WriteLine(setting.DatabaseWriteConnectionName.Value);
            Trace.WriteLine(setting.EntityAssembly.Value);
            Trace.WriteLine(setting.EntityConfigAssembly.Value);
            Trace.WriteLine(setting.EntityTablePrefix.Value);
            Trace.WriteLine(setting.EventSourceAssembly.Value);
            Trace.WriteLine(setting.CrontabAssembly.Value);
            Trace.WriteLine(setting.WorkflowAssembly.Value);
            Trace.WriteLine(setting.WorkflowConnectionName.Value);
            Trace.WriteLine(setting.WorkflowPersistenceMode.Value);
            Trace.WriteLine(setting.WorkflowMaxInstanceLockedRetriesCount.Value);
            Trace.WriteLine(setting.WindowsUpdateService.Value);
            Trace.WriteLine(setting.FileService.Value);
            Trace.WriteLine(setting.MessageService.Value);
            Trace.WriteLine(setting.OpcService.Value);
            Trace.WriteLine(setting.CrontabService.Value);
            Trace.WriteLine(setting.AuthenticationTimeout.Value);
            Trace.WriteLine(setting.AuthorizationEnabled.Value);
            Trace.WriteLine(setting.PartitionsCount.Value);

            Assert.IsNotNull(setting);
        }
    }
}
