using System;
using PostSharp.Aspects;
using SD.AOP.Core.Aspects.ForAny;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 不抛出异常AOP特性类
    /// </summary>
    /// <remarks>发生异常时记录日志</remarks>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NonThrowExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //调用基类方法
            base.OnException(eventArgs);

            //记录日志，不抛出异常
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }
}