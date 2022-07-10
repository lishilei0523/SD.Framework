namespace SD.Infrastructure.CrontabBase.Tests.StubCrontabs
{
    /// <summary>
    /// 测试任务
    /// </summary>
    public class HelloWorldCrontab : Crontab
    {
        public HelloWorldCrontab(ExecutionStrategy executionStrategy)
            : base(executionStrategy)
        {

        }
    }
}
