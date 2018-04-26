using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SD.Infrastructure.AspNetCore.Toolkits;
using System;
using System.Configuration;
using System.Text;

namespace SD.Infrastructure.AspNetCore.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 授权过滤器
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            #region # 读取开启身份认证配置

            string enableAuthStr = ConfigurationManager.AppSettings[OperationContext.EnableAuthAppSettingKey];

            if (!bool.TryParse(enableAuthStr, out bool enableAuth))
            {
                enableAuth = false;
            }

            #endregion

            //判断是否开启身份认证，用户是否登录，Action上是否贴有无需过滤标签，
            if (enableAuth &&
                !OperationContext.Logined &&
                !filterContext.ActionDescriptor.HasAttr<AllowAnonymousAttribute>())
            {
                //是不是Ajax请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("未登录，请重新登录！");
                }

                //构造脚本
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.Append("<script type=\"text/javascript\">");
                scriptBuilder.Append("(function (){ ");
                scriptBuilder.Append("window.top.location.href=");
                scriptBuilder.AppendFormat("\"{0}\";", OperationContext.LoginPage);
                scriptBuilder.Append(" })();");
                scriptBuilder.Append("</script>");

                //跳转至登录页
                filterContext.HttpContext.Response.WriteAsync(scriptBuilder.ToString()).Wait();
            }
        }
    }
}
