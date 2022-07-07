using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 执行策略基类
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(FixedTimeStrategy))]
    [KnownType(typeof(RepeatedTimeStrategy))]
    [KnownType(typeof(RecurrenceStrategy))]
    [KnownType(typeof(CronExpressionStrategy))]
    public abstract class ExecutionStrategy
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ExecutionStrategy()
        {
            this.Enabled = true;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 是否启用 —— bool Enabled
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        #endregion 

        #endregion
    }
}
