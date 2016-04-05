using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ShSoft.Framework2016.Infrastructure.MVC;

namespace ShSoft.Framework2016.Infrastructure.MVCTests
{
    /// <summary>
    /// 全局应用程序类
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //注册依赖注入配置
            IocConfig.Register();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}