using SD.Infrastructure.Constants;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.WCF.Tests.Stubs
{
    /// <summary>
    /// 数据库清理者桩
    /// </summary>
    public class StubDbCleaner : IDbCleaner
    {
        /// <summary>
        /// 清理
        /// </summary>
        public void Clean()
        {
            Trace.WriteLine($"会话\"{GlobalSetting.CurrentSessionId}\"数据库清理已执行..");
            Console.WriteLine($"会话\"{GlobalSetting.CurrentSessionId}\"数据库清理已执行..");
        }
    }
}