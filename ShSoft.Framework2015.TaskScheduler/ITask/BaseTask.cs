using System;
using Quartz;

namespace ShSoft.Framework2015.TaskScheduler.ITask
{
    /// <summary>
    /// 定时任务基类
    /// </summary>
    /// <typeparam name="T">任务类型</typeparam>
    public abstract class BaseTask<T> : IJob where T : IJob
    {
        #region # 字段及构造器

        /// <summary>
        /// 任务明细只读字段
        /// </summary>
        public static readonly IJobDetail Detail;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BaseTask()
        {
            //任务明细初始化
            string id = Guid.NewGuid().ToString();
            Detail = JobBuilder.Create<T>().WithIdentity(id).Build();
        }

        #endregion

        #region # 执行任务 —— abstract void Execute(IJobExecutionContext context)
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">执行上下文</param>
        public abstract void Execute(IJobExecutionContext context);
        #endregion
    }
}
