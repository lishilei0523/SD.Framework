using System;
using System.Collections.Generic;
using ShSoft.Framework2015.Infrastructure.IIOC;
using ShSoft.Framework2015.Infrastructure.IOC.Factory;

namespace ShSoft.Framework2015.Infrastructure.IOC.Mediator
{
    /// <summary>
    /// 解析者中介者
    /// </summary>
    public static class ResolverMediator
    {
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例</returns>
        public static T Resolve<T>()
        {
            IInstanceResolver instanceResolver = ResolverFactory.GetDefaultInstanceResolver();
            return instanceResolver.Resolve<T>();
        }

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例集</returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            IInstanceResolver instanceResolver = ResolverFactory.GetDefaultInstanceResolver();
            return instanceResolver.ResolveAll<T>();
        }

        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例</returns>
        public static object Resolve(Type type)
        {
            IInstanceResolver instanceResolver = ResolverFactory.GetDefaultInstanceResolver();
            return instanceResolver.Resolve(type);
        }

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例集</returns>
        public static IEnumerable<object> ResolveAll(Type type)
        {
            IInstanceResolver instanceResolver = ResolverFactory.GetDefaultInstanceResolver();
            return instanceResolver.ResolveAll(type);
        }
    }
}
