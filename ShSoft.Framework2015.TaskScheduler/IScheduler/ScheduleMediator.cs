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

        #region # 调度任务（定时） —— static void Schedule(IJobDetail jobDetail, DateTime executeTime)
        /// <summary>
        /// 调度任务（定时），
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

        #region # 调度任务（每小时） —— static void ScheduleByHour(IJobDetail jobDetail, byte hours)
        /// <summary>
        /// 调度任务（每小时），
        /// 循环执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="hours">执行时间间隔（小时）</param>
        /// <remarks>小时最大值为23，超过23按23计算</remarks>
        public static void ScheduleByHour(IJobDetail jobDetail, byte hours)
        {
            hours = hours > (byte)23 ? (byte)23 : hours;

            //构造cron表达式
            string cron = string.Format("0 * 0/{0} * * ? ", hours);

            //开始调度
            Schedule(jobDetail, cron);
        }
        #endregion

        #region # 调度任务（每分钟） —— static void ScheduleByMinite(IJobDetail jobDetail, byte minutes)
        /// <summary>
        /// 调度任务（每分钟），
        /// 循环执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="minutes">执行时间间隔（分钟）</param>
        /// <remarks>分钟最大值为59，超过59按59计算</remarks>
        public static void ScheduleByMinite(IJobDetail jobDetail, byte minutes)
        {
            minutes = minutes > (byte)59 ? (byte)59 : minutes;

            //构造cron表达式
            string cron = string.Format("0 0/{0} * * * ? ", minutes);

            //开始调度
            Schedule(jobDetail, cron);
        }
        #endregion

        #region # 调度任务（每秒） —— static void ScheduleBySecond(IJobDetail jobDetail, byte seconds)
        /// <summary>
        /// 调度任务（每秒），
        /// 循环执行
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        /// <param name="seconds">执行时间间隔（秒）</param>
        /// <remarks>秒最大值为59，超过59按59计算</remarks>
        public static void ScheduleBySecond(IJobDetail jobDetail, byte seconds)
        {
            seconds = seconds > (byte)59 ? (byte)59 : seconds;

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
            else
            {
                _Scheduler.ResumeJob(jobDetail.Key);
            }
        }
        #endregion

        #region # 移除调度任务 —— static void Remove(IJobDetail jobDetail)
        /// <summary>
        /// 移除调度任务
        /// </summary>
        /// <param name="jobDetail">任务明细</param>
        public static void Remove(IJobDetail jobDetail)
        {
            if (_Scheduler.CheckExists(jobDetail.Key))
            {
                _Scheduler.DeleteJob(jobDetail.Key);
            }
        }
        #endregion

        #region # 清空调度任务 —— static void Clear()
        /// <summary>
        /// 清空调度任务
        /// </summary>
        public static void Clear()
        {
            _Scheduler.Clear();
        }
        #endregion
    }
}
