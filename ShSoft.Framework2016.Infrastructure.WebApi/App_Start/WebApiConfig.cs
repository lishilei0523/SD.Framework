using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using ShSoft.Framework2016.Infrastructure.WebApi.DependencyResolvers;

namespace ShSoft.Framework2016.Infrastructure.WebApi
{
    /// <summary>
    /// WebApi配置
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 注册WebApi配置
        /// </summary>
        /// <param name="config">配置</param>
        public static void Register(HttpConfiguration config)
        {
            FieldInfo suffix = typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null)
            {
                suffix.SetValue(new DefaultHttpControllerSelector(config), "Contract");
            }

            //WebApi中有两套文档传输格式，XML与JSON，默认为XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //重新设置路由规则
            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{action}/{id}", new { id = RouteParameter.Optional }
            );

            config.DependencyResolver = new WebApiDependencyResolver();
        }
    }
}
