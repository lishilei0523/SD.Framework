using System;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ShSoft.Infrastructure.MVC.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 错误页AppSetting键
        /// </summary>
        private const string ErrorPageAppSettingKey = "ErrorPage";

        /// <summary>
        /// 错误页地址
        /// </summary>
        private static readonly string _ErrorPage;

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
            string errorPage = ConfigurationManager.AppSettings[ErrorPageAppSettingKey];

            #region # 验证

            if (string.IsNullOrWhiteSpace(errorPage))
            {
                throw new ApplicationException("默认错误页未配置，请联系管理员！");
            }

            #endregion

            _ErrorPage = errorPage;
        }

        #endregion

        #region # 发生异常事件 —— void OnException(ExceptionContext filterContext)
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        public void OnException(ExceptionContext filterContext)
        {
            #region # 处理异常消息

            string errorMessage;

            try
            {
                IDictionary json = _JsonSerializer.DeserializeObject(filterContext.Exception.Message) as IDictionary;

                if (json != null && json.Contains("ErrorMessage"))
                {
                    errorMessage = json["ErrorMessage"].ToString();
                }
                else
                {
                    errorMessage = filterContext.Exception.Message;
                }
            }
            catch
            {
                errorMessage = filterContext.Exception.Message;
            }

            #endregion

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
                scriptBuilder.Append(string.Format("\"{0}?message={1}\"", _ErrorPage, errorMessage));
                scriptBuilder.Append("</script>");

                //跳转至错误页
                filterContext.HttpContext.Response.Write(scriptBuilder.ToString());
            }

            //异常已处理
            filterContext.ExceptionHandled = true;
        }
        #endregion
    }
}