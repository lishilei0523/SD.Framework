using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ShSoft.Framework2015.LogSite.Model.Base;

namespace ShSoft.Framework2015.LogSite
{
    /// <summary>
    /// 全局应用程序类
    /// </summary>

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //初始化菜单
            DbSession.Current.InitMenu();
        }
    }
}