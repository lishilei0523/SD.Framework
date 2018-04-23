using System;
using PostSharp.Aspects;
using SD.AOP.Core.Aspects.ForAny;
using SD.Infrastructure.CustomExceptions;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 领域层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class DomainExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //抛出异常
            throw new DomainException(eventArgs.Exception.Message, eventArgs.Exception);
        }
    }
}