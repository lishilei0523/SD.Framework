using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SD.Infrastructure.AspNetCore.Toolkits;
using System;
using System.Net;
using System.Text;

namespace SD.Infrastructure.AspNetCore.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        #region # 发生异常事件 —— void OnException(ExceptionContext filterContext)
        /// <summary>
        /// 发生异常事件
        /// </summary>
        public void OnException(ExceptionContext filterContext)
        {
            string errorMessage/*异常消息*/ = string.Empty;
            errorMessage = GetErrorMessage(filterContext.Exception.Message, ref errorMessage);
            errorMessage = FilterErrorMessage(errorMessage);

            int statusCode/*状态码500*/ = (int)HttpStatusCode.InternalServerError;

            filterContext.HttpContext.Response.StatusCode = statusCode;

            //Ajax请求
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //响应
                filterContext.HttpContext.Response.WriteAsync(errorMessage).Wait();
            }
            else
            {
                //构造脚本
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.Append("<script type=\"text/javascript\">");
                scriptBuilder.Append("(function (){ ");
                scriptBuilder.Append("window.top.location.href=");
                scriptBuilder.AppendFormat("\"{0}?message={1}\";", OperationContext.ErrorPage, errorMessage);
                scriptBuilder.Append(" })();");
                scriptBuilder.Append("</script>");

                //跳转至错误页
                filterContext.HttpContext.Response.WriteAsync(scriptBuilder.ToString()).Wait();
            }

            //异常已处理
            filterContext.ExceptionHandled = true;
        }
        #endregion

        #region # 递归获取错误消息 —— static string GetErrorMessage(string exceptionMessage...
        /// <summary>
        /// 递归获取错误消息
        /// </summary>
        /// <param name="exceptionMessage">异常消息</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>错误消息</returns>
        private static string GetErrorMessage(string exceptionMessage, ref string errorMessage)
        {
            try
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(exceptionMessage);

                if (jObject != null && jObject.TryGetValue("ErrorMessage", StringComparison.OrdinalIgnoreCase, out JToken jToken))
                {
                    errorMessage = jToken.Value<string>();
                }
                else
                {
                    errorMessage = exceptionMessage;
                }

                GetErrorMessage(errorMessage, ref errorMessage);

                return errorMessage;
            }
            catch
            {

                return exceptionMessage;
            }
        }
        #endregion

        #region # 过滤错误消息 —— static string FilterErrorMessage(string errorMessage)
        /// <summary>
        /// 过滤错误消息
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>错误消息</returns>
        private static string FilterErrorMessage(string errorMessage)
        {
            const string ignoreChar1 = "\r";
            const string ignoreChar2 = "\n";

            errorMessage = errorMessage.Replace(ignoreChar1, string.Empty);
            errorMessage = errorMessage.Replace(ignoreChar2, string.Empty);

            return errorMessage;
        }
        #endregion
    }
}
