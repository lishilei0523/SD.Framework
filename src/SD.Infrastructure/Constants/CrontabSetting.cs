using SD.Infrastructure.Configurations;
using SD.Infrastructure.CrontabBase;
using System;
using System.Collections.Generic;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 定时任务设置
    /// </summary>
    public static class CrontabSetting
    {
        #region # 字段及构造器

        /// <summary>
        /// 定时任务策略字典
        /// </summary>
        private static readonly IDictionary<string, ExecutionStrategy> _CrontabStrategies;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CrontabSetting()
        {
            _CrontabStrategies = new Dictionary<string, ExecutionStrategy>();
            foreach (CrontabElement element in FrameworkSection.Setting.CrontabElements)
            {
                ExecutionStrategy strategy = GetExecutionStrategy(element.StrategyType, element.Strategy);
                _CrontabStrategies.Add(element.Type, strategy);
            }
        }

        #endregion

        #region # 执行策略字典 —— static IDictionary<string, ExecutionStrategy> CrontabStrategies
        /// <summary>
        /// 执行策略字典
        /// </summary>
        public static IDictionary<string, ExecutionStrategy> CrontabStrategies
        {
            get { return _CrontabStrategies; }
        }
        #endregion

        #region # 获取执行策略 —— static ExecutionStrategy GetExecutionStrategy(string strategyType...
        /// <summary>
        /// 获取执行策略
        /// </summary>
        /// <param name="strategyType">策略类型</param>
        /// <param name="strategy">策略</param>
        /// <returns>执行策略</returns>
        public static ExecutionStrategy GetExecutionStrategy(string strategyType, string strategy)
        {
            if (strategyType == nameof(FixedTimeStrategy))
            {
                DateTime triggerTime = DateTime.Parse(strategy);
                FixedTimeStrategy fixedTimeStrategy = new FixedTimeStrategy(triggerTime);

                return fixedTimeStrategy;
            }
            if (strategyType == nameof(RecurrenceStrategy))
            {
                TimeSpan interval = TimeSpan.Parse(strategy);
                RecurrenceStrategy recurrenceStrategy = new RecurrenceStrategy(interval);

                return recurrenceStrategy;
            }
            if (strategyType == nameof(CronExpressionStrategy))
            {
                CronExpressionStrategy cronExpressionStrategy = new CronExpressionStrategy(strategy);

                return cronExpressionStrategy;
            }

            throw new NotSupportedException("无此类型执行策略");
        }
        #endregion
    }
}
