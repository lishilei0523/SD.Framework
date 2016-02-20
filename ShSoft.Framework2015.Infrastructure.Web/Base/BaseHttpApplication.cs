using System;
using System.Web;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;
using ShSoft.Framework2015.Infrastructure.IOC.Mediator;
using ShSoft.Framework2015.Infrastructure.IRepository;

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
            //初始化数据库
            this.InitDataBase();

            //初始化领域事件存储
            this.InitDomainEventStore();
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void InitDataBase()
        {
            IDataInitializer initializer = ResolverMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化领域事件存储
        /// </summary>
        private void InitDomainEventStore()
        {
            IDomainEventStorer eventStorer = ResolverMediator.Resolve<IDomainEventStorer>();
            eventStorer.InitStore();
        }
    }
}
