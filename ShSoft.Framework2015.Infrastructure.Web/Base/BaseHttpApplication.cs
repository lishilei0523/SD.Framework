using System;
using System.Web;

namespace ShSoft.Framework2015.Infrastructure.Web.Base
{
    /// <summary>
    /// ASP.NET全局应用程序类基类
    /// </summary>
    public abstract class BaseHttpApplication : HttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            //注册初始化数据库
            InitDbConfig.Register();
        }
    }
}
