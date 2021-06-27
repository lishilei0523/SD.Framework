using Microsoft.Owin;
using Owin;
using SD.Infrastructure.WebApi.SelfHost.Server.Middlewares;
using SD.Infrastructure.WebApi.Tests;
using SD.IOC.Integration.WebApi.SelfHost;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]
namespace SD.Infrastructure.WebApi.Tests
{
    public class Startup : StartupBase
    {
        /// <summary>
        /// 配置应用程序
        /// </summary>
        /// <param name="appBuilder">应用程序建造者</param>
        /// <param name="httpConfiguration">Http配置</param>
        protected override void Configuration(IAppBuilder appBuilder, HttpConfiguration httpConfiguration)
        {
            //适用全局中间件
            appBuilder.Use<GlobalMiddleware>();

            //配置路由规则
            httpConfiguration.Routes.MapHttpRoute(
                "DefaultApi",
                "Api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
