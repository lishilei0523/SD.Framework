using SD.Infrastructure.CrontabBase;
using SD.Infrastructure.SchedulerBase.Tests.StubCrontabs;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.SchedulerBase.Tests.StubCrontabSchedulerss
{
    public class ShowTimeCrontabExecutor : CrontabExecutor<ShowTimeCrontab>
    {
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public override void Execute(ShowTimeCrontab crontab)
        {
            Trace.WriteLine(crontab.Text);
            Console.WriteLine(crontab.Text);

            crontab.Handled = true;
            crontab.Count++;

            if (crontab.Text == "Exception")
            {
                throw new NullReferenceException("测试异常！");
            }
        }
    }
}
