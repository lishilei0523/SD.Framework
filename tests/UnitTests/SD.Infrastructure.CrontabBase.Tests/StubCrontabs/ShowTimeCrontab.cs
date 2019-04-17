using System;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 报时任务
    /// </summary>
    public class ShowTimeCrontab : Crontab
    {
        public ShowTimeCrontab(string text)
        {
            this.Text = text;
        }

        public int Count { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 获取执行策略
        /// </summary>
        protected override ExecutionStrategy GetExecutionStrategy()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            TimeSpanStrategy strategy = new TimeSpanStrategy(timeSpan);

            return strategy;
        }
    }
}
