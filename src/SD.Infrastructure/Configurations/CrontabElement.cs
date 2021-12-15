using System.Configuration;

namespace SD.Infrastructure.Configurations
{
    /// <summary>
    /// 定时任务节点
    /// </summary>
    public class CrontabElement : ConfigurationElement
    {
        #region # 定时任务类型 —— string Type
        /// <summary>
        /// 定时任务类型
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }
        #endregion

        #region # 策略类型 —— string StrategyType
        /// <summary>
        /// 策略类型
        /// </summary>
        [ConfigurationProperty("strategyType", IsRequired = true)]
        public string StrategyType
        {
            get { return (string)this["strategyType"]; }
            set { this["strategyType"] = value; }
        }
        #endregion

        #region # 策略 —— string Strategy
        /// <summary>
        /// 策略
        /// </summary>
        [ConfigurationProperty("strategy", IsRequired = true)]
        public string Strategy
        {
            get { return (string)this["strategy"]; }
            set { this["strategy"] = value; }
        }
        #endregion
    }
}
