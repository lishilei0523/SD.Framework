using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 循环策略
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class RecurrenceStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public RecurrenceStrategy() { }
        #endregion

        #region 01.创建循环策略构造器
        /// <summary>
        /// 创建循环策略构造器
        /// </summary>
        /// <param name="recurrenceTimeInterval">循环时间间隔</param>
        public RecurrenceStrategy(TimeSpan recurrenceTimeInterval)
            : this()
        {
            this.RecurrenceTimeInterval = recurrenceTimeInterval;
        }
        #endregion

        #endregion

        #region # 属性

        #region 循环时间间隔 —— TimeSpan RecurrenceTimeInterval
        /// <summary>
        /// 循环时间间隔
        /// </summary>
        [DataMember]
        public TimeSpan RecurrenceTimeInterval { get; set; }
        #endregion 

        #endregion
    }
}
