using System;
using System.Runtime.Serialization;

namespace SD.Infrastructure.CrontabBase
{
    /// <summary>
    /// 执行策略基类
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(FixedTimeStrategy))]
    [KnownType(typeof(RecurrenceStrategy))]
    [KnownType(typeof(CronExpressionStrategy))]
    public abstract class ExecutionStrategy
    {

    }
}
