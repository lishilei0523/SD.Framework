using SD.Infrastructure.CrontabBase.Tests.StubCrontabs;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabExecutors
{
    /// <summary>
    /// 报时任务执行者
    /// </summary>
    public class ShowTimeCrontabExecutor : CrontabExecutor<ShowTimeCrontab>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void Execute(ShowTimeCrontab crontab)
        {
            Trace.WriteLine(crontab.Text);
            crontab.Count++;

            base._logAppender.Append("［测试日志］" + 1 + Environment.NewLine +
                                     "［测试日志］" + 2 + Environment.NewLine +
                                     "［测试日志］" + 3 + Environment.NewLine +
                                     "［测试日志］" + 4);

            if (crontab.Text == nameof(Exception))
            {
                throw new NullReferenceException("测试异常！");
            }
        }
    }
}
