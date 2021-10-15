using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.Infrastructure.CustomExceptions;
using SD.Toolkits.Json;
using System;
#if NET46_OR_GREATER
using System.ServiceModel;
#endif
#if NETSTANDARD2_0_OR_GREATER
using CoreWCF;
#endif

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 应用程序服务层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AppServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            if (OperationContext.Current != null)
            {
                throw new FaultException(base._exceptionMessage.ToJson());
            }

            throw new AppServiceException(base._exceptionMessage.ToJson());
        }
    }
}