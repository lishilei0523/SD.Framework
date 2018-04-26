using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace SD.Infrastructure.AspNetCore.Toolkits
{
    /// <summary>
    /// MVC扩展工具类
    /// </summary>
    public static class MvcExtension
    {
        #region # 是否是Ajax请求 —— static bool IsAjaxRequest(this HttpRequest request)
        /// <summary>
        /// 是否是Ajax请求
        /// </summary>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            const string ajaxHeaderKey = "x-requested-with";
            const string ajaxHeaderValue = "XMLHttpRequest";

            if (request.Headers.ContainsKey(ajaxHeaderKey))
            {
                bool isAjax = request.Headers[ajaxHeaderKey] == ajaxHeaderValue;
                return isAjax;
            }

            return false;
        }
        #endregion

        #region # Controller/Action是否有某特性标签 —— static bool HasAttr<T>(this...
        /// <summary>
        /// Controller/Action是否有某特性标签
        /// </summary>
        /// <typeparam name="T">特性标签类型</typeparam>
        /// <param name="actionDescriptor">Action方法元数据</param>
        /// <returns>是否拥有该特性</returns>
        public static bool HasAttr<T>(this ActionDescriptor actionDescriptor) where T : Attribute
        {
            Type type = typeof(T);

            if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                bool actionDefined = controllerActionDescriptor.MethodInfo.IsDefined(type, true);

                if (actionDefined)
                {
                    return true;
                }

                bool controllerDefined = controllerActionDescriptor.ControllerTypeInfo.IsDefined(type, true);

                if (controllerDefined)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }
        #endregion
    }
}
