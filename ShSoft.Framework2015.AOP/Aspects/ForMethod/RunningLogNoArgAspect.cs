using System;
using PostSharp.Aspects;
using ShSoft.Framework2015.AOP.Toolkits;

namespace ShSoft.Framework2015.AOP.Aspects.ForMethod
{
    /// <summary>
    /// 程序日志AOP特性类，
    /// 不记录方法参数信息
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RunningLogNoArgAspect : RunningLogAspect
    {
        /// <summary>
        /// 执行方法开始事件
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            base._runningLog.BuildRuningInfo(eventArgs);
            base._runningLog.BuildBasicInfo(eventArgs);
        }
    }
}