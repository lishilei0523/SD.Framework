using System;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务基类
    /// </summary>
    [Serializable]
    public abstract class Crontab : ICrontab
    {
        #region # 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Crontab()
        {
            //默认值
            this.Id = Guid.NewGuid();
            this.AddedTime = DateTime.Now;
            this.ExecutionStrategy = this.GetExecutionStrategy();
        }
        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 执行策略 —— ExecutionStrategy ExecutionStrategy
        /// <summary>
        /// 执行策略
        /// </summary>
        public ExecutionStrategy ExecutionStrategy { get; set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 获取执行策略 —— abstract ExecutionStrategy GetExecutionStrategy()
        /// <summary>
        /// 获取执行策略
        /// </summary>
        protected abstract ExecutionStrategy GetExecutionStrategy();
        #endregion

        #endregion
    }
}
