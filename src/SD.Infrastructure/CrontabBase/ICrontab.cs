using System;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务基接口
    /// </summary>
    public interface ICrontab
    {
        #region 标识 —— string Id
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; }
        #endregion

        #region 执行策略 —— ExecutionStrategy ExecutionStrategy
        /// <summary>
        /// 执行策略
        /// </summary>
        ExecutionStrategy ExecutionStrategy { get; }
        #endregion

        #region 状态 —— CrontabStatus Status
        /// <summary>
        /// 状态
        /// </summary>
        CrontabStatus Status { get; set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        DateTime AddedTime { get; }
        #endregion
    }
}
