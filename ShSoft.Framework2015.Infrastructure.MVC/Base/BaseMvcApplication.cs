using System;
using System.Web;

namespace ShSoft.Framework2015.Infrastructure.MVC
{
    /// <summary>
    /// MVC全局应用程序类基类
    /// </summary>
    public abstract class BaseMvcApplication : HttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            //注册IOC配置
            IocConfig.Register();
        }
    }
}
