using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ShSoft.Infrastructure.Constants;

namespace ShSoft.Infrastructure.Repository.RavenDB.Base
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
        /// RavenDB文档存储延迟加载字段
        /// </summary>
        private static readonly Lazy<IDocumentStore> _Store;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RavenDbSession()
        {
            _Store = new Lazy<IDocumentStore>(CreateStore);
        }

        /// <summary>
        /// 创建RavenDB文档存储
        /// </summary>
        /// <returns>RavenDB文档存储</returns>
        private static IDocumentStore CreateStore()
        {
            IDocumentStore documentStore = new DocumentStore { ConnectionStringName = ConnectionStringName };
            documentStore.Initialize();

            if (!string.IsNullOrWhiteSpace(WebConfigSetting.EntityConfigAssembly))
            {
                Assembly assembly = Assembly.Load(WebConfigSetting.EntityConfigAssembly);
                IndexCreation.CreateIndexes(assembly, documentStore);
            }

            return documentStore;
        }

        #endregion

        #region # 访问器

        #region Raven（写）上下文对象 —— static IDocumentSession CommandInstance
        /// <summary>
        /// Raven（写）上下文对象
        /// </summary>
        public static IDocumentSession CommandInstance
        {
            get
            {
                IDocumentSession dbSession = CallContext.GetData(CommandInstanceKey) as IDocumentSession;
                if (dbSession == null)
                {
                    dbSession = _Store.Value.OpenSession();
                    CallContext.SetData(CommandInstanceKey, dbSession);
                }
                return dbSession;
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
                IAsyncDocumentSession dbSession = CallContext.GetData(QueryInstanceKey) as IAsyncDocumentSession;
                if (dbSession == null)
                {
                    dbSession = _Store.Value.OpenAsyncSession();
                    CallContext.SetData(QueryInstanceKey, dbSession);
                }
                return dbSession;
            }
        }
        #endregion

        #endregion
    }
}
