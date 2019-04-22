namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 报时任务
    /// </summary>
    public class ShowTimeCrontab : Crontab
    {
        public ShowTimeCrontab(string text, ExecutionStrategy executionStrategy)
            : base(executionStrategy)
        {
            this.Text = text;
        }

        public int Count { get; set; }
        public string Text { get; set; }
    }
}
