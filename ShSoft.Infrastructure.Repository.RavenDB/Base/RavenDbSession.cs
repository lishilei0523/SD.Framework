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
        #region # 字段及构造器

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

        /// <summary>
        /// RavenDB文档存储
        /// </summary>
        private static readonly IDocumentStore _Store;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RavenDbSession()
        {
            _Store = new DocumentStore { ConnectionStringName = ConnectionStringName };
            _Store.Initialize();

            Assembly assembly = Assembly.Load(WebConfigSetting.EntityConfigAssembly);
            IndexCreation.CreateIndexes(assembly, _Store);
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
                    dbSession = _Store.OpenSession();
                    CallContext.SetData(CommandInstanceKey, dbSession);
                }
                return dbSession;
            }
        }
        #endregion

        #region Raven（读）上下文对象 —— static IDocumentSession QueryInstance
        /// <summary>
        /// Raven（读）上下文对象
        /// </summary>
        public static IDocumentSession QueryInstance
        {
            get
            {
                IDocumentSession dbSession = CallContext.GetData(QueryInstanceKey) as IDocumentSession;
                if (dbSession == null)
                {
                    dbSession = _Store.OpenSession();
                    CallContext.SetData(QueryInstanceKey, dbSession);
                }
                return dbSession;
            }
        }
        #endregion

        #endregion

        #region # 析构器
        /// <summary>
        /// 释放RavenDB文档存储
        /// </summary>
        public static void FlushStore()
        {
            _Store.Dispose();
        }
        #endregion
    }
}
