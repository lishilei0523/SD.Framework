using System.Web.Mvc;
using ShSoft.Framework2016.Infrastructure.MVC.DependencyResolvers;

namespace ShSoft.Framework2016.Infrastructure.MVC
{
    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public static class IocConfig
    {
        /// <summary>
        /// 注册依赖注入配置
        /// </summary>
        public static void Register()
        {
            DependencyResolver.SetResolver(new MvcDependencyResolver());
        }
    }
}
