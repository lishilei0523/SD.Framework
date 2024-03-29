﻿using Quartz;
using Quartz.Impl;
using SD.Infrastructure.Constants;
using SD.Infrastructure.CrontabBase.Factories;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.CrontabBase.Mediators
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
            _Scheduler.JobFactory = QuartzJobFactory.Instance;
        }

        #endregion


        //Public

        #region # 保存任务 —— static void Store(ICrontab crontab)
        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public static void Store(ICrontab crontab)
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore == null)
                {
                    throw new NotImplementedException("未注册持久化存储提供者！");
                }
                crontabStore.Store(crontab);
            }
        }
        #endregion

        #region # 调度任务 —— static void Schedule<T>(...
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <typeparam name="T">任务类型</typeparam>
        /// <param name="delay">延迟时间(毫秒)</param>
        /// <remarks>适用于没有参数，只有策略的任务</remarks>
        public static async void Schedule<T>(int delay = 1000) where T : class, ICrontab
        {
            //调度任务
            Type crontabType = typeof(T);

            #region # 验证

            if (!CrontabSetting.CrontabStrategies.ContainsKey(crontabType.Name))
            {
                throw new InvalidOperationException($"没有为定时任务\"{crontabType.Name}\"配置执行策略");
            }

            #endregion

            ExecutionStrategy executionStrategy = CrontabSetting.CrontabStrategies[crontabType.Name];

            #region # 验证

            if (!executionStrategy.Enabled)
            {
                return;
            }

            #endregion

            ICrontab crontab = CrontabFactory.CreateCrontab(crontabType, executionStrategy);

            IEnumerable<ICrontabExecutor> crontabExecutors = CrontabExecutorFactory.GetCrontabExecutorsFor(crontabType);
            foreach (ICrontabExecutor crontabExecutor in crontabExecutors)
            {
                JobKey jobKey = new JobKey(crontab.Id);
                if (!_Scheduler.CheckExists(jobKey).Result)
                {
                    Type jobType = crontabExecutor.GetType();
                    JobBuilder jobBuilder = JobBuilder.Create(jobType);

                    //设置任务数据
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add(crontab.Id, crontab);
                    jobBuilder.SetJobData(new JobDataMap(dictionary));

                    //创建任务明细
                    IJobDetail jobDetail = jobBuilder.WithIdentity(jobKey).Build();

                    //创建触发器
                    ITrigger trigger = GetTrigger(crontab.ExecutionStrategy);

                    //为调度者添加任务明细与触发器
                    await _Scheduler.ScheduleJob(jobDetail, trigger);

                    //开始调度
                    await _Scheduler.StartDelayed(TimeSpan.FromMilliseconds(delay));
                }
                else
                {
                    await _Scheduler.ResumeJob(jobKey);
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontab.Status = CrontabStatus.Scheduled;
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 调度任务 —— static void Schedule(ICrontab crontab...
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        /// <param name="delay">延迟时间(毫秒)</param>
        public static async void Schedule(ICrontab crontab, int delay = 1000)
        {
            #region # 验证

            if (crontab.ExecutionStrategy == null)
            {
                throw new InvalidOperationException("执行策略不可为空！");
            }

            #endregion

            //调度任务
            Type crontabType = crontab.GetType();
            IEnumerable<ICrontabExecutor> crontabExecutors = CrontabExecutorFactory.GetCrontabExecutorsFor(crontabType);
            foreach (ICrontabExecutor crontabExecutor in crontabExecutors)
            {
                JobKey jobKey = new JobKey(crontab.Id);
                if (!_Scheduler.CheckExists(jobKey).Result)
                {
                    Type jobType = crontabExecutor.GetType();
                    JobBuilder jobBuilder = JobBuilder.Create(jobType);

                    //设置任务数据
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add(crontab.Id, crontab);
                    jobBuilder.SetJobData(new JobDataMap(dictionary));

                    //创建任务明细
                    IJobDetail jobDetail = jobBuilder.WithIdentity(jobKey).Build();

                    //创建触发器
                    ITrigger trigger = GetTrigger(crontab.ExecutionStrategy);

                    //为调度者添加任务明细与触发器
                    await _Scheduler.ScheduleJob(jobDetail, trigger);

                    //开始调度
                    await _Scheduler.StartDelayed(TimeSpan.FromMilliseconds(delay));
                }
                else
                {
                    await _Scheduler.ResumeJob(jobKey);
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontab.Status = CrontabStatus.Scheduled;
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 调度任务 —— static void ScheduleGenerally<T>(T crontab...
        /// <summary>
        /// 调度任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontab">定时任务</param>
        /// <param name="delay">延迟时间(毫秒)</param>
        public static async void ScheduleGenerally<T>(T crontab, int delay = 1000) where T : class, ICrontab
        {
            #region # 验证

            if (crontab.ExecutionStrategy == null)
            {
                throw new InvalidOperationException("执行策略不可为空！");
            }

            #endregion

            //调度任务
            IEnumerable<ICrontabExecutor<T>> crontabSchedulers = CrontabExecutorFactory.GetCrontabExecutorsFor(crontab);
            foreach (ICrontabExecutor<T> scheduler in crontabSchedulers)
            {
                JobKey jobKey = new JobKey(crontab.Id);
                if (!_Scheduler.CheckExists(jobKey).Result)
                {
                    Type jobType = scheduler.GetType();
                    JobBuilder jobBuilder = JobBuilder.Create(jobType);

                    //设置任务数据
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add(crontab.Id, crontab);
                    jobBuilder.SetJobData(new JobDataMap(dictionary));

                    //创建任务明细
                    IJobDetail jobDetail = jobBuilder.WithIdentity(jobKey).Build();

                    //创建触发器
                    ITrigger trigger = GetTrigger(crontab.ExecutionStrategy);

                    //为调度者添加任务明细与触发器
                    await _Scheduler.ScheduleJob(jobDetail, trigger);

                    //开始调度
                    await _Scheduler.StartDelayed(TimeSpan.FromMilliseconds(delay));
                }
                else
                {
                    await _Scheduler.ResumeJob(jobKey);
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontab.Status = CrontabStatus.Scheduled;
                crontabStore?.Store(crontab);
            }
        }
        #endregion

        #region # 暂停任务 —— static void Pause(string crontabId)
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static async void Pause(string crontabId)
        {
            JobKey jobKey = new JobKey(crontabId);
            if (_Scheduler.CheckExists(jobKey).Result)
            {
                await _Scheduler.PauseJob(jobKey);
            }
            else
            {
                throw new NullReferenceException($"Id为\"{crontabId}\"的任务不存在！");
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore != null)
                {
                    ICrontab crontab = crontabStore.Get<ICrontab>(crontabId);
                    crontab.Status = CrontabStatus.Paused;
                    crontabStore.Store(crontab);
                }
            }
        }
        #endregion

        #region # 恢复任务 —— static void Resume(string crontabId)
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static async void Resume(string crontabId)
        {
            JobKey jobKey = new JobKey(crontabId);
            if (_Scheduler.CheckExists(jobKey).Result)
            {
                await _Scheduler.ResumeJob(jobKey);
            }
            else
            {
                using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
                {
                    if (crontabStore != null)
                    {
                        ICrontab crontab = crontabStore.Get<ICrontab>(crontabId);
                        if (crontab != null)
                        {
                            Schedule(crontab);
                        }
                        else
                        {
                            throw new NullReferenceException($"Id为\"{crontabId}\"的任务不存在！");
                        }
                    }
                }
            }

            //保存任务
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore != null)
                {
                    ICrontab crontab = crontabStore.Get<ICrontab>(crontabId);
                    crontab.Status = CrontabStatus.Scheduled;
                    crontabStore.Store(crontab);
                }
            }
        }
        #endregion

        #region # 删除任务 —— static void Remove<T>(T crontab)
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public static async void Remove<T>(T crontab) where T : class, ICrontab
        {
            JobKey jobKey = new JobKey(crontab.Id);
            if (_Scheduler.CheckExists(jobKey).Result)
            {
                await _Scheduler.DeleteJob(jobKey);
            }

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Remove(crontab);
            }
        }
        #endregion

        #region # 删除任务 —— static void Remove(string crontabId)
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="crontabId">定时任务Id</param>
        public static async void Remove(string crontabId)
        {
            JobKey jobKey = new JobKey(crontabId);
            if (_Scheduler.CheckExists(jobKey).Result)
            {
                await _Scheduler.DeleteJob(jobKey);
            }

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Remove(crontabId);
            }
        }
        #endregion

        #region # 异常恢复 —— static void Recover()
        /// <summary>
        /// 异常恢复
        /// </summary>
        /// <remarks>需要CrontabStore持久化支持</remarks>
        public static void Recover()
        {
            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                if (crontabStore == null)
                {
                    throw new NotImplementedException("未注册持久化存储提供者！");
                }

                IList<ICrontab> crontabs = crontabStore.FindAll();
                IEnumerable<ICrontab> scheduledCrontabs = crontabs.Where(x => x.Status == CrontabStatus.Scheduled);
                foreach (ICrontab crontab in scheduledCrontabs)
                {
                    Schedule(crontab);
                }
            }
        }
        #endregion

        #region # 清空任务 —— static void Clear()
        /// <summary>
        /// 清空任务
        /// </summary>
        public static async void Clear()
        {
            await _Scheduler.Clear();

            using (ICrontabStore crontabStore = ResolveMediator.ResolveOptional<ICrontabStore>())
            {
                crontabStore?.Clear();
            }
        }
        #endregion

        #region # 获取任务 —— T GetCrontab<T>(string crontabId)
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <typeparam name="T">定时任务类型</typeparam>
        /// <param name="crontabId">定时任务Id</param>
        /// <returns>定时任务</returns>
        /// <remarks>需要CrontabStore持久化支持</remarks>
        public static T GetCrontab<T>(string crontabId) where T : ICrontab
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


        //Private

        #region # 获取触发器 —— static ITrigger GetTrigger(ExecutionStrategy strategy)
        /// <summary>
        /// 获取触发器
        /// </summary>
        /// <param name="strategy">执行策略</param>
        /// <returns>触发器</returns>
        private static ITrigger GetTrigger(ExecutionStrategy strategy)
        {
            if (strategy is FixedTimeStrategy fixedTimeStrategy)
            {
                string cronExpression = fixedTimeStrategy.TriggerTime.ToCronExpression();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();

                return trigger;
            }
            if (strategy is RepeatedTimeStrategy repeatedTimeStrategy)
            {
                string cronExpression = repeatedTimeStrategy.TriggerTime.ToCronExpression();
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();

                return trigger;
            }
            if (strategy is RecurrenceStrategy recurrenceStrategy)
            {
                ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithInterval(recurrenceStrategy.RecurrenceTimeInterval).RepeatForever()).Build();

                return trigger;
            }
            if (strategy is CronExpressionStrategy cronExpressionStrategy)
            {
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpressionStrategy.CronExpression).Build();

                return trigger;
            }

            throw new NotSupportedException("无当前类型执行策略！");
        }
        #endregion
    }
}
