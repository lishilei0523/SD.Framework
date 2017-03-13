using System;
using System.Web.Mvc;

namespace SD.Infrastructure.MVC.Toolkits
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
        public static bool HasAttr<T>(this ActionDescriptor action) where T : Attribute
        {
            Type type = typeof(T);

            //Action方法上定义了
            if (action.IsDefined(type, false))
            {
                return true;
            }
            //Controller上定义了
            if (action.ControllerDescriptor.IsDefined(type, false))
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
