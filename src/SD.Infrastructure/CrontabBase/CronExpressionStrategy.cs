namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// Cron表达式策略
    /// </summary>
    public class CronExpressionStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public CronExpressionStrategy() { }
        #endregion

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        public CronExpressionStrategy(string cronExpression)
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
        public string CronExpression { get; set; }
        #endregion

        #endregion
    }
}
