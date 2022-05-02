using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace SD.Infrastructure.AspNet.Tests
{
    /// <summary>
    /// 全局应用程序类
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// 应用程序启动事件
        /// </summary>
        protected void Application_Start()
        {
            //初始化配置文件
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            FrameworkSection.Initialize(configuration);

            //注册区域
            AreaRegistration.RegisterAllAreas();

            //注册路由
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}