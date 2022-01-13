using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 重复时间策略
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class RepeatedTimeStrategy : ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public RepeatedTimeStrategy() { }
        #endregion

        #region 01.创建重复时间策略构造器
        /// <summary>
        /// 创建重复时间策略构造器
        /// </summary>
        /// <param name="triggerTime">触发时间</param>
        public RepeatedTimeStrategy(TimeSpan triggerTime)
            : this()
        {
            this.TriggerTime = triggerTime;
        }
        #endregion

        #endregion

        #region # 属性

        #region 触发时间 —— TimeSpan TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        [DataMember]
        public TimeSpan TriggerTime { get; set; }
        #endregion

        #endregion
    }
}
