using SD.Infrastructure.Configurations;
using SD.Infrastructure.CrontabBase;
using System;
using System.Collections.Concurrent;
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
        /// 定时任务账号
        /// </summary>
        private static readonly string _CrontabLoginId;

        /// <summary>
        /// 定时任务密码
        /// </summary>
        private static readonly string _CrontabPassword;

        /// <summary>
        /// 执行策略字典
        /// </summary>
        private static readonly IDictionary<string, ExecutionStrategy> _CrontabStrategies;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CrontabSetting()
        {
            _CrontabLoginId = FrameworkSection.Setting.CrontabAccountLoginId.Value;
            _CrontabPassword = FrameworkSection.Setting.CrontabAccountPassword.Value;
            _CrontabStrategies = new ConcurrentDictionary<string, ExecutionStrategy>();
            foreach (CrontabStrategyElement element in FrameworkSection.Setting.CrontabStrategyElements)
            {
                ExecutionStrategy strategy = GetExecutionStrategy(element.StrategyType, element.Strategy);
                _CrontabStrategies.Add(element.Type, strategy);
            }
        }

        #endregion


        //属性

        #region # 定时任务所在程序集 —— static string CrontabAssembly
        /// <summary>
        /// 定时任务所在程序集
        /// </summary>
        public static string CrontabAssembly
        {
            get { return FrameworkSection.Setting.CrontabAssembly.Value; }
        }
        #endregion

        #region # 定时任务账号 —— static string CrontabLoginId
        /// <summary>
        /// 定时任务账号
        /// </summary>
        public static string CrontabLoginId
        {
            get { return _CrontabLoginId; }
        }
        #endregion

        #region # 定时任务密码 —— static string CrontabPassword
        /// <summary>
        /// 定时任务密码
        /// </summary>
        public static string CrontabPassword
        {
            get { return _CrontabPassword; }
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


        //方法

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
