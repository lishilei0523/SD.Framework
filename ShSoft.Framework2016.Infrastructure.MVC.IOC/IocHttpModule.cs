using System.Web;
using System.Web.Mvc;
using SD.IOC.Integration.MVC;

namespace ShSoft.Framework2016.Infrastructure.MVC.IOC
{
    /// <summary>
    /// 依赖注入HttpModule
    /// </summary>
    internal class IocHttpModule : IHttpModule
    {
        /// <summary>
        /// 初始化，
        /// 相当于Application_Start事件
        /// </summary>
        /// <param name="context">应用程序上下文</param>
        public void Init(HttpApplication context)
        {
            DependencyResolver.SetResolver(new MvcDependencyResolver());
        }

        /// <summary>
        /// 释放资源，
        /// 相当于Application_End事件
        /// </summary>
        public void Dispose() { }
    }
}
