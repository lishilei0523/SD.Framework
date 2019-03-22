using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabExecutors
{
    public class AlarmCrontabExecutor : CrontabExecutor<AlarmCrontab>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public override void Execute(AlarmCrontab crontab)
        {
            Trace.WriteLine(crontab.Word);

            crontab.Rung = true;
            crontab.Count++;
        }
    }
}
