using SD.Infrastructure.CrontabBase.Toolkits;

namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 报时任务
    /// </summary>
    public class ShowTimeCrontab : Crontab
    {
        protected ShowTimeCrontab()
        {
            this.CronExpression = Cron.Secondly(2);
        }

        public ShowTimeCrontab(string text)
            : this()
        {
            this.Text = text;
        }

        public bool Handled { get; set; }
        public int Count { get; set; }
        public string Text { get; set; }
    }
}
