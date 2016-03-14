using ShSoft.Framework2015.Infrastructure.IDomainEvent;
using ShSoft.Framework2015.Infrastructure.IOC.Mediator;
using ShSoft.Framework2015.Infrastructure.IRepository;

namespace ShSoft.Framework2015.Infrastructure.Web
{
    /// <summary>
    /// 初始化数据库配置
    /// </summary>
    public static class InitDbConfig
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            InitDataBase();
            InitDomainEventStore();
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private static void InitDataBase()
        {
            IDataInitializer initializer = ResolverMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化领域事件存储
        /// </summary>
        private static void InitDomainEventStore()
        {
            IDomainEventStorer eventStorer = ResolverMediator.Resolve<IDomainEventStorer>();
            eventStorer.InitStore();
        }
    }
}
