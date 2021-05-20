using ArxOne.MrAdvice.Advice;
using Caliburn.Micro;
using System;

namespace SD.Infrastructure.WPF.Caliburn.Aspects
{
    /// <summary>
    /// WPF依赖属性AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DependencyPropertyAttribute : Attribute, IPropertyAdvice
    {
        /// <summary>
        /// 拦截属性
        /// </summary>
        /// <param name="context">属性上下文</param>
        public void Advise(PropertyAdviceContext context)
        {
            if (context.IsGetter)
            {
                context.Proceed();
            }
            else
            {
                context.Proceed();
                if (context.Target is INotifyPropertyChangedEx dataContext)
                {
                    dataContext.NotifyOfPropertyChange(context.TargetName);
                }
            }
        }
    }
}
