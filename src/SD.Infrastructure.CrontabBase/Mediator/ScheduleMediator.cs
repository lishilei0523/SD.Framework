using Quartz;
using Quartz.Impl;
using SD.Infrastructure.CrontabBase.Factories;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;

namespace SD.Infrastructure.CrontabBase.Mediator
{
    /// <summary>
    /// 调度中介者
    /// </summary>
    public static class ScheduleMediator
    {
        #region # 字段及构造器

        /// <summary>
        /// 调度者单例
        /// </summary>
        private static readonly IScheduler _Scheduler;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ScheduleMediator()
        {
            ScheduleMediator._Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        }

        #endregion

        #region # 调度任务 —— static void Schedule(ICrontab crontab)
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public static void Schedule(ICrontab crontab)
        {
            //调度任务
            IEnumerable<ICrontabExecutor> crontabSchedulers = CrontabExecutorFactory.GetCrontabExecutorsFor(crontab);
            foreach (ICrontabExecutor scheduler in crontabSchedulers)
            {
                JobKey jobKey = new JobKey(crontab.Id.ToString());

                if (!ScheduleMediator._Scheduler.CheckExists(jobKey).Result)
                {
                    Type jobType = scheduler.GetType();
                    JobBuilder jobBuilder = JobBuilder.Create(jobType);

                    //设置任务数据
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add(crontab.Id.ToString(), crontab);
                    jobBuilder.SetJobData(new JobDataMap(dictionary));

                    //创建任务明细
                    IJobDetail jobDetail = jobBuilder.WithIdentity(jobKey).Build();

                    //创建触发器
                    ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(crontab.CronExpression).Build();

                    //为调度者添加任务明细与触发器
                    ScheduleMediator._Scheduler.ScheduleJob(jobDetail, trigger);

                    //开始调度
                    ScheduleMediator._Scheduler.Start().Wait();
                }
                else
                {
                    ScheduleMediator._Scheduler.ResumeJob(jobKey);
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 调度任务 —— static void Schedule<T>(T crontab)
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontab">定时任务</param>
        public static void Schedule<T>(T crontab) where T : class, ICrontab
        {
            //调度任务
            IEnumerable<ICrontabExecutor<T>> crontabSchedulers = CrontabExecutorFactory.GetCrontabExecutorsFor(crontab);
            foreach (ICrontabExecutor<T> scheduler in crontabSchedulers)
            {
                JobKey jobKey = new JobKey(crontab.Id.ToString());

                if (!ScheduleMediator._Scheduler.CheckExists(jobKey).Result)
                {
                    Type jobType = scheduler.GetType();
                    JobBuilder jobBuilder = JobBuilder.Create(jobType);

                    //设置任务数据
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add(crontab.Id.ToString(), crontab);
                    jobBuilder.SetJobData(new JobDataMap(dictionary));

                    //创建任务明细
                    IJobDetail jobDetail = jobBuilder.WithIdentity(jobKey).Build();

                    //创建触发器
                    ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(crontab.CronExpression).Build();

                    //为调度者添加任务明细与触发器
                    ScheduleMediator._Scheduler.ScheduleJob(jobDetail, trigger);

                    //开始调度
                    ScheduleMediator._Scheduler.Start().Wait();
                }
                else
                {
                    ScheduleMediator._Scheduler.ResumeJob(jobKey);
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 删除任务 —— static void Remove<T>(T crontab)
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public static void Remove<T>(T crontab) where T : class, ICrontab
        {
            JobKey jobKey = new JobKey(crontab.Id.ToString());

            if (ScheduleMediator._Scheduler.CheckExists(jobKey).Result)
            {
                ScheduleMediator._Scheduler.DeleteJob(jobKey);
            }

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Remove(crontab);
            }
        }
        #endregion

        #region # 恢复全部任务 —— static void ResumeAll()
        /// <summary>
        /// 恢复全部任务
        /// </summary>
        public static void ResumeAll()
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore != null)
                {
                    IList<ICrontab> crontabs = crontabStore.FindAll();

                    foreach (ICrontab crontab in crontabs)
                    {
                        ScheduleMediator.Schedule(crontab);
                    }
                }
            }
        }
        #endregion

        #region # 清空任务 —— static void Clear()
        /// <summary>
        /// 清空任务
        /// </summary>
        public static void Clear()
        {
            ScheduleMediator._Scheduler.Clear();

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Clear();
            }
        }
        #endregion
    }
}
