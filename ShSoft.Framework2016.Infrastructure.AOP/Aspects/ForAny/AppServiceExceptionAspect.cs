using System;
using PostSharp.Aspects;
using SD.AOP.Core.Aspects.ForAny;
using SD.AOP.Core.Toolkits;
using ShSoft.Framework2016.Infrastructure.CustomExceptions;

namespace ShSoft.Framework2016.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// 应用程序服务层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AppServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //调用基类方法
            base.OnException(eventArgs);

            //抛出异常
            throw new AppServiceException(base._exceptionMessage.ToJson());
        }
    }
}