using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 执行策略基类
    /// </summary>
    [DataContract]
    [KnownType(typeof(FixedTimeStrategy))]
    [KnownType(typeof(RecurrenceStrategy))]
    [KnownType(typeof(CronExpressionStrategy))]
    public abstract class ExecutionStrategy
    {

    }
}
