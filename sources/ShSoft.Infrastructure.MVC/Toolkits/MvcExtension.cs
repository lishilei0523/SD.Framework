using System;
using System.Web.Mvc;

namespace ShSoft.Infrastructure.MVC.Toolkits
{
    /// <summary>
    /// MVC扩展工具类
    /// </summary>
    public static class MvcExtension
    {
        #region # Controller/Action是否有某特性标签 —— static bool HasAttr<T>(this...
        /// <summary>
        /// Controller/Action是否有某特性标签
        /// </summary>
        /// <typeparam name="T">特性标签类型</typeparam>
        /// <param name="action">ActionDescriptor</param>
        /// <returns>是否拥有该特性</returns>
        public static bool HasAttr<T>(this ActionDescriptor action)
        {
            Type type = typeof(T);
            return action.IsDefined(type, false) || action.ControllerDescriptor.IsDefined(type, false);
        }
        #endregion
    }
}
