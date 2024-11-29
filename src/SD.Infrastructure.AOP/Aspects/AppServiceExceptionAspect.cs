using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.Infrastructure.AOP.Exceptions;
using SD.Toolkits.Json;
using System;

namespace SD.Infrastructure.AOP.Aspects
{
    /// <summary>
    /// 应用程序服务层异常AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AppServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            throw new AppServiceException(base._exceptionMessage.ToJson());
        }
    }
}
