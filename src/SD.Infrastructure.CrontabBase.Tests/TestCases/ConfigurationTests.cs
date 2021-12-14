using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.CrontabBase.Configurations;
using SD.Infrastructure.CrontabBase.Constants;
using System;
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
            Type type = typeof(DateTime);
            Trace.WriteLine(type.Name);

            foreach (CrontabElement element in CrontabSection.Setting.CrontabElements)
            {
                Trace.WriteLine(element.Type);
                Trace.WriteLine(element.StrategyType);
                Trace.WriteLine(element.Strategy);
            }
        }

        [TestMethod]
        public void TestCrontabSetting()
        {
            foreach (KeyValuePair<string, ExecutionStrategy> kv in CrontabSetting.CrontabStrategies)
            {
                Trace.WriteLine(kv.Key);
                Trace.WriteLine(kv.Value);
            }
        }
    }
}
