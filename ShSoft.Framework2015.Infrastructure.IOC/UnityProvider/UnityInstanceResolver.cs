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
        #region # 字段及构造器

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

        #endregion

        #region # 解析实例 —— T Resolve<T>()
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例</returns>
        public T Resolve<T>()
        {
            return this._container.Resolve<T>();
        }
        #endregion

        #region # 解析实例 —— object Resolve(Type type)
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例</returns>
        public object Resolve(Type type)
        {
            return this._container.Resolve(type);
        }
        #endregion

        #region # 解析实例 —— T ResolveOptional<T>()
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例，如未注册则返回null</returns>
        public T ResolveOptional<T>() where T : class
        {
            return !this._container.IsRegistered<T>() ? null : this._container.Resolve<T>();
        }

        #endregion

        #region # 解析实例 —— object ResolveOptional(Type type)
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例，如未注册则返回null</returns>
        public object ResolveOptional(Type type)
        {
            return !this._container.IsRegistered(type) ? null : this._container.Resolve(type);
        }
        #endregion

        #region # 解析实例集 —— IEnumerable<T> ResolveAll<T>()
        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例集</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return this._container.ResolveAll<T>();
        }
        #endregion

        #region # 解析实例集 —— IEnumerable<object> ResolveAll(Type type)
        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例集</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return this._container.ResolveAll(type);
        }
        #endregion
    }
}
