using System;
using System.Web.Http;
using ShSoft.Framework2015.Infrastructure.Web.Base;

namespace ShSoft.Framework2015.Infrastructure.WebApi.Base
{
    /// <summary>
    /// WebApi全局应用程序类基类
    /// </summary>
    public abstract class BaseWebApiApplication : BaseHttpApplication
    {
        /// <summary>
        /// 应用程序开始事件
        /// </summary>
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            //注册WebApi配置
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
