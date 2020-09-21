using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using SD.Infrastructure.CustomExceptions;
using System;

namespace SD.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 仓储层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RepositoryExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            //抛出异常
            throw new RepositoryException(exception.Message, exception);
        }
    }
}