using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.AOP.Core.Toolkits;
using SD.Infrastructure.CustomExceptions;
using System;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 应用程序服务层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AppServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>发生异常事件</summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            //抛出异常
            throw new AppServiceException(base._exceptionMessage.ToJson());
        }
    }
}