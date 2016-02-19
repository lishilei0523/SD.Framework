using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using ShSoft.Framework2015.Infrastructure.IIOC;

namespace ShSoft.Framework2015.Infrastructure.IOC.UnityProvider
{
    /// <summary>
    /// Unity实例解析者
    /// </summary>
    internal class UnityInstanceResolver : IInstanceResolver
    {
        /// <summary>
        /// Unity容器
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="container">Unity容器</param>
        public UnityInstanceResolver(IUnityContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// 解析实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例</returns>
        public T Resolve<T>()
        {
            return this._container.Resolve<T>();
        }

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例集</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return this._container.ResolveAll<T>();
        }

        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例</returns>
        public object Resolve(Type type)
        {
            return this._container.Resolve(type);
        }

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例集</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return this._container.ResolveAll(type);
        }
    }
}
