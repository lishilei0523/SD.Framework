using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Configurations;
using SD.Infrastructure.Constants;
using System.Collections.Generic;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.TestCases
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
            Trace.WriteLine(FrameworkSection.Setting.CrontabElements.LoginId);
            Trace.WriteLine(FrameworkSection.Setting.CrontabElements.Password);
            foreach (CrontabElement element in FrameworkSection.Setting.CrontabElements)
            {
                Trace.WriteLine(element.Type);
                Trace.WriteLine(element.StrategyType);
                Trace.WriteLine(element.Strategy);
            }
        }

        [TestMethod]
        public void TestCrontabSetting()
        {
            Trace.WriteLine(CrontabSetting.CrontabLoginId);
            Trace.WriteLine(CrontabSetting.CrontabPassword);
            foreach (KeyValuePair<string, ExecutionStrategy> kv in CrontabSetting.CrontabStrategies)
            {
                Trace.WriteLine(kv.Key);
                Trace.WriteLine(kv.Value);
            }
        }
    }
}
