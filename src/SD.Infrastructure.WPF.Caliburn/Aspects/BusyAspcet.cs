using System;
using ArxOne.MrAdvice.Advice;
using SD.Infrastructure.WPF.Caliburn.Base;
using SD.Infrastructure.WPF.Extensions;

namespace SD.Infrastructure.WPF.Caliburn.Aspects
{
    /// <summary>
    /// 繁忙AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class BusyAttribute : Attribute, IMethodAdvice
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
            if (context.Target is ScreenBase screenBase)
            {
                screenBase.Busy();
                context.Proceed();
                screenBase.Idle();
            }
            if (context.Target is OneActiveConductorBase oneActiveConductorBase)
            {
                oneActiveConductorBase.Busy();
                context.Proceed();
                oneActiveConductorBase.Idle();
            }
        }
    }
}