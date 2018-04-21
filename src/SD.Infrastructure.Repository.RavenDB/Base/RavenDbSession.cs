using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using SD.Infrastructure.Constants;
using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace SD.Infrastructure.Repository.RavenDB.Base
{
    /// <summary>
    /// RavenDB会话
    /// </summary>
    internal static class RavenDbSession
    {
        #region # 常量

        /// <summary>
        /// Raven（写）上下文对象缓存键
        /// </summary>
        internal const string CommandInstanceKey = "RavenCommandInstance";

        /// <summary>
        /// Raven（读）上下文对象缓存键
        /// </summary>
        internal const string QueryInstanceKey = "RavenQueryInstance";

        /// <summary>
        /// RavenDB连接字符串名称
        /// </summary>
        private const string ConnectionStringName = "RavenConnection";

        #endregion

        #region # 字段及构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// RavenDB文档存储延迟加载字段
        /// </summary>
        private static readonly IDocumentStore _Store;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RavenDbSession()
        {
            _Sync = new object();

            #region # 验证

            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (connectionString == null)
            {
                throw new ApplicationException("Raven连接字符串未配置，请联系管理员！");
            }

            #endregion

            IDocumentStore documentStore = new DocumentStore { ConnectionStringName = ConnectionStringName };
            documentStore.Initialize();

            if (!string.IsNullOrWhiteSpace(GlobalSetting.EntityConfigAssembly))
            {
                Assembly assembly = Assembly.Load(GlobalSetting.EntityConfigAssembly);
                IndexCreation.CreateIndexes(assembly, documentStore);
            }

            _Store = documentStore;
        }

        #endregion

        #region # 访问器

        #region Raven（写）上下文对象访问器 —— static object CommandInstanceCall
        /// <summary>
        /// Raven（写）上下文对象访问器
        /// </summary>
        private static object CommandInstanceCall
        {
            get { return CallContext.LogicalGetData(CommandInstanceKey); }
            set { CallContext.LogicalSetData(CommandInstanceKey, value); }
        }
        #endregion

        #region Raven（读）上下文对象访问器 —— static object QueryInstanceCall
        /// <summary>
        /// Raven（读）上下文对象访问器
        /// </summary>
        private static object QueryInstanceCall
        {
            get { return CallContext.LogicalGetData(QueryInstanceKey); }
            set { CallContext.LogicalSetData(QueryInstanceKey, value); }
        }
        #endregion

        #region Raven（写）上下文对象 —— static IDocumentSession CommandInstance
        /// <summary>
        /// Raven（写）上下文对象
        /// </summary>
        public static IDocumentSession CommandInstance
        {
            get
            {
                lock (_Sync)
                {
                    IDocumentSession dbSession = CommandInstanceCall as IDocumentSession;

                    if (dbSession == null)
                    {
                        dbSession = _Store.OpenSession();
                        CommandInstanceCall = dbSession;
                    }

                    return dbSession;
                }
            }
        }
        #endregion

        #region Raven（读）上下文对象 —— static IAsyncDocumentSession QueryInstance
        /// <summary>
        /// Raven（读）上下文对象
        /// </summary>
        public static IAsyncDocumentSession QueryInstance
        {
            get
            {
                lock (_Sync)
                {
                    IAsyncDocumentSession dbSession = QueryInstanceCall as IAsyncDocumentSession;

                    if (dbSession == null)
                    {
                        dbSession = _Store.OpenAsyncSession();
                        QueryInstanceCall = dbSession;
                    }

                    return dbSession;
                }
            }
        }
        #endregion

        #region 释放Raven（写）上下文对象 —— static void FreeCommandInstanceCall()
        /// <summary>
        /// 释放Raven（写）上下文对象
        /// </summary>
        public static void FreeCommandInstanceCall()
        {
            if (CommandInstance != null)
            {
                CommandInstance.Dispose();
            }
            CallContext.FreeNamedDataSlot(CommandInstanceKey);
        }
        #endregion

        #region 释放Raven（读）上下文对象 —— static void FreeQueryInstanceCall()
        /// <summary>
        /// 释放Raven（读）上下文对象
        /// </summary>
        public static void FreeQueryInstanceCall()
        {
            if (QueryInstance != null)
            {
                QueryInstance.Dispose();
            }
            CallContext.FreeNamedDataSlot(QueryInstanceKey);
        }
        #endregion

        #endregion
    }
}
