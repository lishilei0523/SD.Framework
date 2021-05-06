using Quartz;
using Quartz.Spi;
using SD.IOC.Core.Mediators;
using System;

namespace SD.Infrastructure.CrontabBase.Factories
{
    /// <summary>
    /// Quartz任务工厂
    /// </summary>
    internal sealed class QuartzJobFactory : IJobFactory
    {
        /// <summary>
        /// 创建Quartz任务
        /// </summary>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobType = bundle.JobDetail.JobType;
            IJob job = (IJob)ResolveMediator.Resolve(jobType);

            return job;
        }

        /// <summary>
        /// 清理Quartz任务
        /// </summary>
        public void ReturnJob(IJob job)
        {
            ResolveMediator.Dispose();
        }
    }
}
