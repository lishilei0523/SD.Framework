using Quartz;
using Quartz.Spi;
using SD.Infrastructure.Global;
using SD.IOC.Core.Mediators;
using System;

namespace SD.Infrastructure.CrontabBase.Factories
{
    /// <summary>
    /// Quartz任务工厂
    /// </summary>
    internal sealed class QuartzJobFactory : IJobFactory
    {
        #region # 创建Quartz任务 —— IJob NewJob(TriggerFiredBundle bundle...
        /// <summary>
        /// 创建Quartz任务
        /// </summary>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobType = bundle.JobDetail.JobType;
            IJob job = (IJob)ResolveMediator.Resolve(jobType);

            return job;
        }
        #endregion

        #region # 清理Quartz任务 —— void ReturnJob(IJob job)
        /// <summary>
        /// 清理Quartz任务
        /// </summary>
        public void ReturnJob(IJob job)
        {
            //清理数据库
            Finalizer.CleanDb();

            //清理依赖注入范围容器
            ResolveMediator.Dispose();
        }
        #endregion
    }
}
