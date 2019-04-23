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
        protected Crontab(ExecutionStrategy executionStrategy)
        {
            //默认值
            this.Id = Guid.NewGuid().ToString();
            this.AddedTime = DateTime.Now;
            this.ExecutionStrategy = executionStrategy;
        }
        #endregion

        #region # 属性

        #region 标识 —— string Id
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }
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
    }
}
