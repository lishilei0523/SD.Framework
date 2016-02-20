using System;
using Quartz;
using Quartz.Impl;

namespace ShSoft.Framework2015.TaskScheduler.IScheduler
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
        private static readonly Quartz.IScheduler _Scheduler;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ScheduleMediator()
        {
            _Scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        #endregion

        #region # 调度任务 —— static void Schedule(IJobDetail jobDetail, DateTime executeTime)
        /// <summary>
        /// 调度任务，
        /// 定时执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="executeTime">执行时间</param>
        public static void Schedule(IJobDetail jobDetail, DateTime executeTime)
        {
            //构造cron表达式
            string cron = string.Format("{0} {1} {2} {3} {4} ? {5}", executeTime.Second, executeTime.Minute, executeTime.Hour, executeTime.Day, executeTime.Month, executeTime.Year);

            //开始调度
            Schedule(jobDetail, cron);
        }
        #endregion

        #region # 调度任务 —— static void Schedule(IJobDetail jobDetail, byte timeSpan)
        /// <summary>
        /// 调度任务，
        /// 循环执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="minutes">执行时间间隔（分钟）</param>
        public static void Schedule(IJobDetail jobDetail, byte minutes)
        {
            //构造cron表达式
            string cron = string.Format("0 0/{0} * * * ? ", minutes);
            //string cron = string.Format("0/{0} * * * * ? ", minutes);

            //开始调度
            Schedule(jobDetail, cron);
        }
        #endregion

        #region # 调度任务 —— static void SchedulePerSecond(IJobDetail jobDetail, byte seconds)
        /// <summary>
        /// 调度任务，
        /// 循环执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="seconds">执行时间间隔（秒）</param>
        public static void SchedulePerSecond(IJobDetail jobDetail, byte seconds)
        {
            //构造cron表达式
            string cron = string.Format("0/{0} * * * * ? ", seconds);

            //开始调度
            Schedule(jobDetail, cron);
        }
        #endregion

        #region # 调度任务 —— static void Schedule(IJobDetail jobDetail, string cron)
        /// <summary>
        /// 调度任务，
        /// 根据cron表达式执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="cron">cron表达式</param>
        public static void Schedule(IJobDetail jobDetail, string cron)
        {
            if (!_Scheduler.CheckExists(jobDetail.Key))
            {
                //创建触发器
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cron).Build();

                //为调度者添加任务与触发器
                _Scheduler.ScheduleJob(jobDetail, trigger);

                //开始调度
                _Scheduler.Start();
            }
        }
        #endregion
    }
}
