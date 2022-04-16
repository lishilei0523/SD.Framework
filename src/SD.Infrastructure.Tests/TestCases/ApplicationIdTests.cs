using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Constants;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 应用程序Id测试
    /// </summary>
    [TestClass]
    public class ApplicationIdTests
    {
        [TestMethod]
        public void TestConfigurations()
        {
            Trace.WriteLine(GlobalSetting.ApplicationId);

            GlobalSetting.ApplicationId = Guid.Empty.ToString();

            Trace.WriteLine(GlobalSetting.ApplicationId);

            GlobalSetting.ApplicationId = Guid.NewGuid().ToString();

            Trace.WriteLine(GlobalSetting.ApplicationId);
        }
    }
}
