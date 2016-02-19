using System;
using ShSoft.Framework2015.Infrastructure.Web.Base;

namespace ShSoft.Framework2015.Infrastructure.MVC
{
    /// <summary>
    /// MVC全局应用程序类基类
    /// </summary>
    public abstract class BaseMvcApplication : BaseHttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            //注册IOC配置
            IocConfig.Register();
        }

        /// <summary>
        /// 请求结束事件
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }
    }
}
