using System;
using Quartz;
using ShSoft.Framework2015.TaskScheduler.ITask;

namespace ShSoft.Framework2015.TaskSchedulerTests.StubTasks
{
    /// <summary>
    /// 闹钟任务
    /// </summary>
    public class StubAlarmTask : BaseTask<StubAlarmTask>
    {
        /// <summary>
        /// 参考参数
        /// </summary>
        public static string ReferenceParam;

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">执行上下文</param>
        public override void Execute(IJobExecutionContext context)
        {
            ReferenceParam = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
