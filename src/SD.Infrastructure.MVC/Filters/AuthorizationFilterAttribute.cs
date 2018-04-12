using SD.Infrastructure.MVC.Toolkits;
using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace SD.Infrastructure.MVC.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        #region 授权过滤器 —— override void OnAuthorization(AuthorizationContext filterContext)
        /// <summary>
        /// 授权过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            #region # 读取开启身份认证配置

            string enableAuthStr = ConfigurationManager.AppSettings[OperationContext.EnableAuthAppSettingKey];
            bool enableAuth;

            if (!bool.TryParse(enableAuthStr, out enableAuth))
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
                scriptBuilder.Append("window.top.location.href=");
                scriptBuilder.Append(string.Format("\"{0}\"", OperationContext.LoginPage));
                scriptBuilder.Append("</script>");

                //跳转至登录页
                filterContext.HttpContext.Response.Write(scriptBuilder.ToString());
            }
        }
        #endregion
    }
}
