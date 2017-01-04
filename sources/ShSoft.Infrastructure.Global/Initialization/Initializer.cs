using SD.IOC.Core.Mediator;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Runtime.Remoting.Messaging;

namespace ShSoft.Infrastructure.Global
{
    /// <summary>
    /// 初始化器
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDataBase()
        {
            IDataInitializer initializer = ResolveMediator.Resolve<IDataInitializer>();
            initializer.Initialize();
        }

        /// <summary>
        /// 初始化会话Id
        /// </summary>
        public static void InitSessionId()
        {
            lock (_Sync)
            {
                CallContext.FreeNamedDataSlot(CacheConstants.SessionIdKey);
                CallContext.SetData(CacheConstants.SessionIdKey, Guid.NewGuid());
            }
        }
    }
}
