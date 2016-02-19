using System;
using System.Collections.Generic;
using System.Reflection;
using PostSharp.Aspects;
using ShSoft.Framework2015.AOP.Models.Entities;
using ShSoft.Framework2015.AOP.Models.ValueObjects;
using ShSoft.Framework2015.Common.PoweredByLee;

namespace ShSoft.Framework2015.AOP.Toolkits
{
    /// <summary>
    /// 日志建造者
    /// </summary>
    internal static class LogBuilder
    {
        #region # 建造基本信息 —— static void BuildBasicInfo(this BaseLog log, MethodExecutionArgs eventArgs)
        /// <summary>
        /// 建造基本信息
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="eventArgs">方法元数据</param>
        public static void BuildBasicInfo(this BaseLog log, MethodExecutionArgs eventArgs)
        {
            log.Namespace = eventArgs.Method.DeclaringType.Namespace;
            log.ClassName = eventArgs.Method.DeclaringType.Name;
            log.MethodName = eventArgs.Method.Name;
            log.MethodType = eventArgs.Method.IsStatic ? "静态" : "实例";
            log.IPAddress = CommonExtension.GetLocalIPAddress();
        }
        #endregion

        #region # 建造方法参数信息 —— static void BuildMethodArgsInfo(this BaseLog log, MethodExecutionArgs eventArgs)
        /// <summary>
        /// 建造方法参数信息
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="eventArgs">方法元数据</param>
        public static void BuildMethodArgsInfo(this BaseLog log, MethodExecutionArgs eventArgs)
        {
            Arguments arguments = eventArgs.Arguments;
            ParameterInfo[] parameters = eventArgs.Method.GetParameters();

            List<MethodArg> argList = new List<MethodArg>();

            for (int i = 0; arguments != null && i < arguments.Count; i++)
            {
                string argTypeName = string.Format("{0}.{1}", parameters[i].ParameterType.Namespace, parameters[i].ParameterType.Name);
                MethodArg arg = new MethodArg(parameters[i].Name, argTypeName, arguments[i].ToJson());

                argList.Add(arg);
            }

            log.ArgsJson = argList.ToJson();
        }
        #endregion

        #region # 建造异常信息 —— static void BuildExceptionInfo(this ExceptionLog log...
        /// <summary>
        /// 建造异常信息
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <param name="eventArgs">方法元数据</param>
        public static void BuildExceptionInfo(this ExceptionLog log, MethodExecutionArgs eventArgs)
        {
            log.ExceptionType = eventArgs.Exception.GetType().Name;
            log.ExceptionMessage = eventArgs.Exception.Message;
            log.ExceptionInfo = eventArgs.Exception.ToString();
            log.InnerException = eventArgs.Exception.InnerException == null ? null : eventArgs.Exception.InnerException.ToString();
            log.OccurredTime = DateTime.Now;
        }
        #endregion

        #region # 建造程序运行信息 —— static void BuildRuningInfo(this RunningLog log, MethodExecutionArgs eventArgs)
        /// <summary>
        /// 建造程序运行信息
        /// </summary>
        /// <param name="log">程序运行日志</param>
        /// <param name="eventArgs">方法元数据</param>
        public static void BuildRuningInfo(this RunningLog log, MethodExecutionArgs eventArgs)
        {
            log.StartTime = DateTime.Now;
            log.UserId = Guid.Empty;
            log.UserName = null;
        }
        #endregion

        #region # 建造返回值信息 —— static void BuildReturnValueInfo(this RunningLog log, MethodExecutionArgs eventArgs)
        /// <summary>
        /// 建造返回值信息
        /// </summary>
        /// <param name="log">程序运行日志</param>
        /// <param name="eventArgs">方法元数据</param>
        public static void BuildReturnValueInfo(this RunningLog log, MethodExecutionArgs eventArgs)
        {
            log.EndTime = DateTime.Now;

            if (eventArgs.Exception == null)
            {
                if (eventArgs.ReturnValue == null)
                {
                    log.ReturnValue = null;
                    log.ReturnValueType = "void";
                }
                else
                {
                    log.ReturnValue = eventArgs.ReturnValue.ToJson();
                    log.ReturnValueType = string.Format("{0}.{1}", eventArgs.ReturnValue.GetType().Namespace, eventArgs.ReturnValue.GetType().Name);
                }
            }
            else
            {
                log.ReturnValue = eventArgs.Exception.ToString();
            }
        }
        #endregion
    }
}
