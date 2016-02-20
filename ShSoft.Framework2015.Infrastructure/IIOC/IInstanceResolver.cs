using System;
using System.Collections.Generic;

namespace ShSoft.Framework2015.Infrastructure.IIOC
{
    /// <summary>
    /// 实例解析者接口
    /// </summary>
    public interface IInstanceResolver
    {
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例</returns>
        T Resolve<T>();

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns>实例集</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例</returns>
        object Resolve(Type type);

        /// <summary>
        /// 解析实例集
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <returns>实例集</returns>
        IEnumerable<object> ResolveAll(Type type);
    }
}
