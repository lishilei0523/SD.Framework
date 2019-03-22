using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabExecutors
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
            crontab.Count++;

            if (crontab.Text == "Exception")
            {
                throw new NullReferenceException("测试异常！");
            }
        }
    }
}
