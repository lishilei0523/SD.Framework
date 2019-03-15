using System;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务基接口
    /// </summary>
    public interface ICrontab
    {
        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        Guid Id { get; }
        #endregion

        #region Cron表达式 —— string CronExpression
        /// <summary>
        /// Cron表达式
        /// </summary>
        string CronExpression { get; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        DateTime AddedTime { get; }
        #endregion
    }
}
