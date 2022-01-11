using ArxOne.MrAdvice.Advice;
using SD.Infrastructure.WPF.Caliburn.Base;
using SD.Infrastructure.WPF.Extensions;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.WPF.Caliburn.Aspects
{
    /// <summary>
    /// 异步异常AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ExceptionAsyncAspcet : Attribute, IMethodAsyncAdvice
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            try
            {
                await context.ProceedAsync();
            }
            catch
            {
                if (context.Target is ScreenBase screenBase)
                {
                    screenBase.Idle();
                }
                if (context.Target is OneActiveConductorBase oneActiveConductorBase)
                {
                    oneActiveConductorBase.Idle();
                }
                throw;
            }
        }
    }
}