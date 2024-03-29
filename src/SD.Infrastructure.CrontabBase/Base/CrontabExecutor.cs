﻿using Quartz;
using SD.Infrastructure.CrontabBase.Aspects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务执行者基类
    /// </summary>
    public abstract class CrontabExecutor<T> : ICrontabExecutor<T>, IJob where T : Crontab
    {
        #region # 字段及构造器

        /// <summary>
        /// 日志附着器
        /// </summary>
        protected readonly StringBuilder _logAppender;

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected CrontabExecutor()
        {
            this._logAppender = new StringBuilder();
        }

        #endregion

        #region # 执行任务 —— Task Execute(IJobExecutionContext context)
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">执行上下文</param>
        public Task Execute(IJobExecutionContext context)
        {
            this.ExecuteInternally(context);

            return Task.CompletedTask;
        }
        #endregion

        #region # 执行任务 —— abstract void Execute(T crontab)
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        public abstract void Execute(T crontab);
        #endregion


        //private

        #region # 执行任务 —— void ExecuteInternally(IJobExecutionContext context)
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">执行上下文</param>
        [RunningAspect]
        private void ExecuteInternally(IJobExecutionContext context)
        {
            KeyValuePair<string, object> keyValuePair = context.MergedJobDataMap.Single();
            T crontab = (T)keyValuePair.Value;

            this.Execute(crontab);
        }
        #endregion
    }
}
