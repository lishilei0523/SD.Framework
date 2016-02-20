using System;
using PostSharp.Aspects;
using ShSoft.Framework2015.AOP.Aspects.ForAny;
using ShSoft.Framework2015.Common.PoweredByLee;
using ShSoft.Framework2015.Infrastructure.CustomExceptions;

namespace ShSoft.Framework2015.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// UI层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class UIExceptionAspect : ExceptionAspect
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
            throw new UIException(base._exceptionMessage.ToJson());
        }
    }
}