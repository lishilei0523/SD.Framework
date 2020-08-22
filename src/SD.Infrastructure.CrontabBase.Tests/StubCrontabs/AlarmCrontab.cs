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
        public AlarmCrontab(string word, ExecutionStrategy executionStrategy)
            : base(executionStrategy)
        {
            this.Word = word;
        }


        public int Count { get; set; }
        public bool Rung { get; set; }
        public string Word { get; set; }
    }
}
