using ArxOne.MrAdvice.Advice;
using SD.Infrastructure.WPF.Caliburn.Base;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.WPF.Caliburn.Aspects
{
    /// <summary>
    /// 繁忙AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BusyAttribute : Attribute, IMethodAsyncAdvice
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            if (context.Target is ScreenBase screenBase)
            {
                screenBase.Busy();

                await context.ProceedAsync();

                screenBase.Idle();
            }
            if (context.Target is OneActiveConductorBase oneActiveConductorBase)
            {
                oneActiveConductorBase.Busy();

                await context.ProceedAsync();

                oneActiveConductorBase.Idle();
            }
        }
    }
}
