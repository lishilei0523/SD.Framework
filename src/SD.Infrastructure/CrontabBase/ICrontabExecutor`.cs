namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 定时任务执行者基接口
    /// </summary>
    public interface ICrontabExecutor<in T> : ICrontabExecutor where T : ICrontab
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="crontab">定时任务</param>
        void Execute(T crontab);
    }
}
