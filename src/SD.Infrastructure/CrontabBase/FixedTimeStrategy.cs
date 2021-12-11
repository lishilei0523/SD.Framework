using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 固定时间策略
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class FixedTimeStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public FixedTimeStrategy() { }
        #endregion

        #region 01.创建固定时间策略构造器
        /// <summary>
        /// 创建固定时间策略构造器
        /// </summary>
        /// <param name="triggerTime">触发时间</param>
        public FixedTimeStrategy(DateTime triggerTime)
            : this()
        {
            this.TriggerTime = triggerTime;
        }
        #endregion

        #endregion

        #region # 属性

        #region 触发时间 —— string TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        [DataMember]
        public DateTime TriggerTime { get; set; }
        #endregion

        #endregion
    }
}
