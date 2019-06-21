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

            base._logAppender.Append("［测试日志］" + 1
             + Environment.NewLine + "［测试日志］" + 2
             + Environment.NewLine + "［测试日志］" + 3
             + Environment.NewLine + "［测试日志］" + 4
            );

            if (crontab.Text == "Exception")
            {
                throw new NullReferenceException("测试异常！");
            }
        }
    }
}
