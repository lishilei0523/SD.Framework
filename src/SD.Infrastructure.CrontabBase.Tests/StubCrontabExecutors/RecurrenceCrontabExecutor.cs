using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabExecutors
{
    /// <summary>
    /// 轮询任务执行者
    /// </summary>
    public class RecurrenceCrontabExecutor : CrontabExecutor<RecurrenceCrontab>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public override void Execute(RecurrenceCrontab crontab)
        {
            Trace.WriteLine(DateTime.Now);
        }
    }
}
