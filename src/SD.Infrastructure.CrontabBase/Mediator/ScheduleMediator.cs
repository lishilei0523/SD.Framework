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
            _Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
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

                if (!_Scheduler.CheckExists(jobKey).Result)
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
                    _Scheduler.ScheduleJob(jobDetail, trigger).Wait();

                    //开始调度
                    _Scheduler.Start().Wait();
                }
                else
                {
                    _Scheduler.ResumeJob(jobKey).Wait();
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

                if (!_Scheduler.CheckExists(jobKey).Result)
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
                    _Scheduler.ScheduleJob(jobDetail, trigger).Wait();

                    //开始调度
                    _Scheduler.Start().Wait();
                }
                else
                {
                    _Scheduler.ResumeJob(jobKey).Wait();
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 暂停任务 —— static void Pause(Guid crontabId)
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static void Pause(Guid crontabId)
        {
            JobKey jobKey = new JobKey(crontabId.ToString());

            if (_Scheduler.CheckExists(jobKey).Result)
            {
                _Scheduler.PauseJob(jobKey).Wait();
            }
            else
            {
                throw new NullReferenceException($"Id为\"{crontabId}\"的任务不存在！");
            }
        }
        #endregion

        #region # 恢复任务 —— static void Resume(Guid crontabId)
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static void Resume(Guid crontabId)
        {
            JobKey jobKey = new JobKey(crontabId.ToString());

            if (_Scheduler.CheckExists(jobKey).Result)
            {
                _Scheduler.ResumeJob(jobKey).Wait();
            }
            else
            {
                throw new NullReferenceException($"Id为\"{crontabId}\"的任务不存在！");
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

            if (_Scheduler.CheckExists(jobKey).Result)
            {
                _Scheduler.DeleteJob(jobKey).Wait();
            }

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Remove(crontab);
            }
        }
        #endregion

        #region # 删除任务 —— static void Remove(Guid crontabId)
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static void Remove(Guid crontabId)
        {
            JobKey jobKey = new JobKey(crontabId.ToString());

            if (_Scheduler.CheckExists(jobKey).Result)
            {
                _Scheduler.DeleteJob(jobKey).Wait();
            }

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Remove(crontabId);
            }
        }
        #endregion

        #region # 恢复全部任务 —— static void ResumeAll()
        /// <summary>
        /// 恢复全部任务
        /// </summary>
        /// <remarks>需要CrontabStore持久化支持</remarks>
        public static void ResumeAll()
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore == null)
                {
                    throw new NotImplementedException("未注册持久化存储提供者！");
                }

                IList<ICrontab> crontabs = crontabStore.FindAll();

                foreach (ICrontab crontab in crontabs)
                {
                    ScheduleMediator.Schedule(crontab);
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
            _Scheduler.Clear().Wait();

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Clear();
            }
        }
        #endregion

        #region # 获取任务 —— T GetCrontab<T>(Guid crontabId)
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontabId">定时任务Id</param>
        /// <returns>定时任务</returns>
        /// <remarks>需要CrontabStore持久化支持</remarks>
        public static T GetCrontab<T>(Guid crontabId) where T : ICrontab
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore == null)
                {
                    throw new NotImplementedException("未注册持久化存储提供者！");
                }

                T crontab = crontabStore.Get<T>(crontabId);

                return crontab;
            }
        }
        #endregion

        #region # 获取全部任务列表 —— static IList<ICrontab> FindAll()
        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        /// <returns>定时任务列表</returns>
        /// <remarks>需要CrontabStore持久化支持</remarks>
        public static IList<ICrontab> FindAll()
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore == null)
                {
                    throw new NotImplementedException("未注册持久化存储提供者！");
                }

                IList<ICrontab> crontabs = crontabStore.FindAll();

                return crontabs;
            }
        }
        #endregion
    }
}
