using System;
using System.Collections;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SD.Infrastructure.MVC.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        #region # 字段及构造器

        /// <summary>
        /// JSON序列化器
        /// </summary>
        private static readonly JavaScriptSerializer _JsonSerializer;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ExceptionFilterAttribute()
        {
            _JsonSerializer = new JavaScriptSerializer();
        }

        #endregion

        #region # 发生异常事件 —— void OnException(ExceptionContext filterContext)
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        public override void OnException(ExceptionContext filterContext)
        {
            //处理异常消息
            string errorMessage = string.Empty;
            errorMessage = GetErrorMessage(filterContext.Exception.Message, ref errorMessage);

            //设置状态码为500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //IIS处理
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            //Ajax请求
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //响应
                filterContext.HttpContext.Response.Write(errorMessage);
            }
            else
            {
                //构造脚本
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.Append("<script type=\"text/javascript\">");
                scriptBuilder.Append("window.top.location.href=");
                scriptBuilder.AppendFormat("\"{0}?message={1}\"", OperationContext.ErrorPage, errorMessage);
                scriptBuilder.Append("</script>");

                //跳转至错误页
                filterContext.HttpContext.Response.Write(scriptBuilder.ToString());
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
                IDictionary json = _JsonSerializer.DeserializeObject(exceptionMessage) as IDictionary;

                if (json != null && json.Contains("ErrorMessage"))
                {
                    errorMessage = json["ErrorMessage"].ToString();
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
    }
}