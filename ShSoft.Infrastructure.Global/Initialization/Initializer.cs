using System;
using System.Runtime.Remoting.Messaging;
using SD.IOC.Core.Mediator;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.DomainEventBase;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Global
{
    /// <summary>
    /// 初始化器
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDataBase()
        {
            IDataInitializer initializer = ResolveMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化事件存储
        /// </summary>
        public static void InitDomainEventStore()
        {
            IDomainEventStore eventStore = ResolveMediator.Resolve<IDomainEventStore>();
            eventStore.Init();
        }

        /// <summary>
        /// 初始化会话Id
        /// </summary>
        public static void InitSessionId()
        {
            CallContext.FreeNamedDataSlot(CacheConstants.SessionIdKey);
            CallContext.SetData(CacheConstants.SessionIdKey, Guid.NewGuid());
        }
    }
}
