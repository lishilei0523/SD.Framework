using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.Infrastructure.CustomExceptions;
using System;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 领域层异常AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class DomainExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            //抛出异常
            throw new DomainException(exception.Message, exception);
        }
    }
}
