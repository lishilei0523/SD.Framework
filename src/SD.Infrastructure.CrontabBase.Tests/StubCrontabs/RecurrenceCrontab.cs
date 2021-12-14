namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 轮询任务
    /// </summary>
    public class RecurrenceCrontab : Crontab
    {
        /// <summary>
        /// 创建定时任务构造器
        /// </summary>
        /// <param name="strategy">执行策略</param>
        public RecurrenceCrontab(ExecutionStrategy strategy)
            : base(strategy)
        {

        }
    }
}
