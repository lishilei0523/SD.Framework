using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务基类
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class Crontab : ICrontab
    {
        #region # 构造器

        #region 01.创建定时任务构造器
        /// <summary>
        /// 创建定时任务构造器
        /// </summary>
        /// <param name="strategy">执行策略</param>
        protected Crontab(ExecutionStrategy strategy)
        {
            //默认值
            this.Id = Guid.NewGuid().ToString();
            this.AddedTime = DateTime.Now;
            this.ExecutionStrategy = strategy;
        }
        #endregion

        #endregion

        #region # 属性

        #region 标识 —— string Id
        /// <summary>
        /// 标识
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        #endregion

        #region 执行策略 —— ExecutionStrategy ExecutionStrategy
        /// <summary>
        /// 执行策略
        /// </summary>
        [DataMember]
        public ExecutionStrategy ExecutionStrategy { get; set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddedTime { get; set; }
        #endregion

        #endregion
    }
}
