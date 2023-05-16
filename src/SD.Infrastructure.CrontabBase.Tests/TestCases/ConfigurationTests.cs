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
        #region # 测试配置文件 —— void TestConfigurations()
        /// <summary>
        /// 测试配置文件
        /// </summary>
        [TestMethod]
        public void TestConfigurations()
        {
            Trace.WriteLine(FrameworkSection.Setting.CrontabAssembly.Value);
            Trace.WriteLine(FrameworkSection.Setting.CrontabAccountLoginId.Value);
            Trace.WriteLine(FrameworkSection.Setting.CrontabAccountPassword.Value);
            foreach (CrontabStrategyElement element in FrameworkSection.Setting.CrontabStrategyElements)
            {
                Trace.WriteLine(element.Type);
                Trace.WriteLine(element.StrategyType);
                Trace.WriteLine(element.Strategy);
                Trace.WriteLine(element.Enabled);
            }
        }
        #endregion

        #region # 测试定时任务配置 —— void TestCrontabSetting()
        /// <summary>
        /// 测试定时任务配置
        /// </summary>
        [TestMethod]
        public void TestCrontabSetting()
        {
            Trace.WriteLine(CrontabSetting.CrontabAssembly);
            Trace.WriteLine(CrontabSetting.CrontabLoginId);
            Trace.WriteLine(CrontabSetting.CrontabPassword);
            foreach (KeyValuePair<string, ExecutionStrategy> kv in CrontabSetting.CrontabStrategies)
            {
                Trace.WriteLine(kv.Key);
                Trace.WriteLine(kv.Value);
            }
        }
        #endregion
    }
}
