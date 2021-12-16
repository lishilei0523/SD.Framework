using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Concurrent;

namespace SD.Infrastructure.CrontabBase.Factories
{
    /// <summary>
    /// Quartz任务工厂
    /// </summary>
    internal sealed class QuartzJobFactory : IJobFactory
    {
        #region # 字段及构造器

        /// <summary>
        /// 任务工厂单例
        /// </summary>
        private static readonly IJobFactory _Instance;

        /// <summary>
        /// 服务提供器
        /// </summary>
        private static readonly IServiceProvider _ServiceProvider;

        /// <summary>
        /// 任务范围容器字典
        /// </summary>
        private static readonly ConcurrentDictionary<object, IServiceScope> _RunningJobs;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static QuartzJobFactory()
        {
            _Instance = new QuartzJobFactory();
            _ServiceProvider = ResolveMediator.GetServiceProvider();
            _RunningJobs = new ConcurrentDictionary<object, IServiceScope>();
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
        private QuartzJobFactory()
        {

        }

        #endregion

        #region # Quartz任务工厂实例 —— static IJobFactory Instance
        /// <summary>
        /// Quartz任务工厂实例
        /// </summary>
        public static IJobFactory Instance
        {
            get { return _Instance; }
        }
        #endregion

        #region # 创建Quartz任务 —— IJob NewJob(TriggerFiredBundle bundle...
        /// <summary>
        /// 创建Quartz任务
        /// </summary>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IServiceScope serviceScope = _ServiceProvider.CreateScope();
            IJob job;
            try
            {
                Type jobType = bundle.JobDetail.JobType;
                job = (IJob)serviceScope.ServiceProvider.GetService(jobType);
                _RunningJobs[job] = serviceScope;
            }
            catch
            {
                serviceScope?.Dispose();
                throw;
            }

            return job;
        }
        #endregion

        #region # 清理Quartz任务 —— void ReturnJob(IJob job)
        /// <summary>
        /// 清理Quartz任务
        /// </summary>
        public void ReturnJob(IJob job)
        {
            if (job == null)
            {
                return;
            }
            if (_RunningJobs.TryRemove(job, out IServiceScope serviceScope))
            {
                serviceScope?.Dispose();
            }

            IDisposable disposable = job as IDisposable;
            disposable?.Dispose();
        }
        #endregion
    }
}
