using System;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 时间间隔策略
    /// </summary>
    public class TimeSpanStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public TimeSpanStrategy() { }
        #endregion

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="timeSpan">时间间隔</param>
        public TimeSpanStrategy(TimeSpan timeSpan)
        {
            this.TimeSpan = timeSpan;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 时间间隔 —— TimeSpan TimeSpan
        /// <summary>
        /// 时间间隔
        /// </summary>
        public TimeSpan TimeSpan { get; set; }
        #endregion 

        #endregion
    }
}
