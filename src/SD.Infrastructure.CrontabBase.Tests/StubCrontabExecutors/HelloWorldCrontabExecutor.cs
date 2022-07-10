using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabExecutors
{
    /// <summary>
    /// 测试任务执行者
    /// </summary>
    public class HelloWorldCrontabExecutor : CrontabExecutor<HelloWorldCrontab>
    {
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public override void Execute(HelloWorldCrontab crontab)
        {
            Trace.WriteLine(crontab.ExecutionStrategy.Enabled);
        }
    }
}
