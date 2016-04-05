using SD.IOC.Core.Mediator;
using ShSoft.Framework2016.Infrastructure.IDomainEvent;
using ShSoft.Framework2016.Infrastructure.IRepository;

namespace ShSoft.Framework2016.Infrastructure.Global
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public static class InitDatabase
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
            IDataInitializer initializer = ResolveMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化领域事件存储
        /// </summary>
        private static void InitDomainEventStore()
        {
            IDomainEventStorer eventStorer = ResolveMediator.Resolve<IDomainEventStorer>();
            eventStorer.InitStore();
        }
    }
}
