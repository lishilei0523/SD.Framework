using SD.Infrastructure.CrontabBase.Toolkits;

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
        /// 获取Cron表达式
        /// </summary>
        protected override string GetCronExpression()
        {
            return Cron.Secondly(2);
        }
    }
}
