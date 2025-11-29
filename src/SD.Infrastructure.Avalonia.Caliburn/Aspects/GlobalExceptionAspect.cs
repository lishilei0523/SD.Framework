using ArxOne.MrAdvice.Advice;
using IconPacks.Avalonia.MaterialDesign;
using SD.Infrastructure.Avalonia.Caliburn.Extensions;
using SD.Infrastructure.Avalonia.CustomControls;
using SD.Infrastructure.Avalonia.Enums;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.Caliburn.Aspects
{
    /// <summary>
    /// Avalonia全局异常AOP特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public class GlobalExceptionAspect : Attribute, IMethodAsyncAdvice
    {
        /// <summary>
        /// 未处理异常事件
        /// </summary>
        public static event Func<MethodBase, Exception, Task> UnhandledTaskException;

        /// <summary>
        /// 异步拦截方法
        /// </summary>
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            try
            {
                await context.ProceedAsync();
            }
            catch (Exception exception)
            {
                if (context.IsTargetMethodAsync)
                {
                    if (UnhandledTaskException != null)
                    {
                        await UnhandledTaskException.Invoke(context.TargetMethod, exception);
                    }
                    await MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, PackIconMaterialDesignKind.Error);
                }
                else
                {
                    if (UnhandledTaskException != null)
                    {
                        UnhandledTaskException.Invoke(context.TargetMethod, exception);
                    }
                    MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, PackIconMaterialDesignKind.Error);
                }

                BusyExtension.GlobalIdle();
            }
        }
    }
}
