﻿using System;
using PostSharp.Aspects;
using SD.AOP.Core.Aspects.ForAny;
using SD.AOP.Core.Toolkits;
using ShSoft.Infrastructure.CustomExceptions;

namespace ShSoft.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// UI层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class UIExceptionAspect : ExceptionAspect
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