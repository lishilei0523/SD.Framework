using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.Toolkits.Json;
using System;
using System.ServiceModel;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// WCF服务层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class WCFServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            //抛出异常
            throw new FaultException(base._exceptionMessage.ToJson());
        }
    }
}
