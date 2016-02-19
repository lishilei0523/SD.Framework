using System;

namespace ShSoft.Framework2015.AOP.Aspects.ForAny
{
    /// <summary>
    /// 不抛出异常AOP特性类，
    /// 只记录日志
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NotThrowExceptionAspect : ExceptionAspect
    {

    }
}