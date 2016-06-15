using System;
using PostSharp.Aspects;
using SD.AOP.Core.Aspects.ForAny;
using ShSoft.Framework2016.Common.PoweredByLee;

namespace ShSoft.Framework2016.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// Windows服务异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class WinServiceExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //日志记录到文件中
            var file = AppDomain.CurrentDomain.BaseDirectory + "Logs\\Log_" + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt";
            FileAssistant.WriteFile(file, "=============================发生异常, 详细信息如下==================================="
                                          + Environment.NewLine + "［异常时间］" + DateTime.Now
                                          + Environment.NewLine + "［异常消息］" + eventArgs.Exception.Message
                                          + Environment.NewLine + "［内部异常］" + eventArgs.Exception.InnerException
                                          + Environment.NewLine + "［应用程序］" + eventArgs.Exception.Source
                                          + Environment.NewLine + "［当前方法］" + eventArgs.Exception.TargetSite
                                          + Environment.NewLine + "［堆栈信息］" + eventArgs.Exception.StackTrace
                                          + Environment.NewLine, true);
            //记录日志，不抛出异常
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }

}