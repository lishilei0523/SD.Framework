using SD.Infrastructure.CrontabBase.Toolkits;
using System;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 闹钟任务
    /// </summary>
    public class AlarmCrontab : Crontab
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public AlarmCrontab(string word)
        {
            this.Word = word;
        }


        public int Count { get; set; }
        public bool Rung { get; set; }
        public string Word { get; set; }

        /// <summary>
        /// 获取执行策略
        /// </summary>
        protected override ExecutionStrategy GetExecutionStrategy()
        {
            string cronExpression = DateTime.Now.AddSeconds(2).ToCronExpression();
            CronExpressionStrategy strategy = new CronExpressionStrategy(cronExpression);

            return strategy;
        }
    }
}
