using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// Cron表达式策略
    /// </summary>
    [DataContract]
    public sealed class CronExpressionStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public CronExpressionStrategy() { }
        #endregion

        #region 01.创建Cron表达式策略构造器
        /// <summary>
        /// 创建Cron表达式策略构造器
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        public CronExpressionStrategy(string cronExpression)
            : this()
        {
            this.CronExpression = cronExpression;
        }
        #endregion

        #endregion

        #region # 属性

        #region Cron表达式 —— string CronExpression
        /// <summary>
        /// Cron表达式
        /// </summary>
        [DataMember]
        public string CronExpression { get; set; }
        #endregion

        #endregion
    }
}
