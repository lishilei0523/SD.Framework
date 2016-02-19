using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShSoft.Framework2015.Infrastructure.IIOC;

namespace ShSoft.Framework2015.Infrastructure.IOC.AutofacProvider
{
    /// <summary>
    /// Autofac实例解析者
    /// </summary>
    internal class AutofacInstanceResolver : IInstanceResolver
    {
        /// <summary>
        /// Autofac容器
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="container">Autofac容器</param>
        public AutofacInstanceResolver(IContainer container)
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
            return this._container.Resolve<IEnumerable<T>>();
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
            Type typ = typeof(IEnumerable<>).MakeGenericType(type);

            object instance;
            if (this._container.TryResolve(typ, out instance))
            {
                return ((IEnumerable)instance).Cast<object>();
            }
            return Enumerable.Empty<object>();
        }
    }
}
