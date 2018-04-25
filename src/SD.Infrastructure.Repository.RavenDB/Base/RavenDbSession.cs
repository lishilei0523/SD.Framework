using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.RavenDB.Toolkits;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace SD.Infrastructure.Repository.RavenDB.Base
{
    /// <summary>
    /// RavenDB会话
    /// </summary>
    internal static class RavenDbSession
    {
        #region # 常量

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
        /// Raven（写）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<IDocumentSession> _CommandInstanceCall;

        /// <summary>
        /// Raven（读）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<IAsyncDocumentSession> _QueryInstanceCall;

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
            _CommandInstanceCall = new AsyncLocal<IDocumentSession>();
            _QueryInstanceCall = new AsyncLocal<IAsyncDocumentSession>();

            #region # 验证

            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (connectionString == null)
            {
                throw new ApplicationException("Raven连接字符串未配置，请联系管理员！");
            }

            #endregion

            ConnectionStringParser parser = new ConnectionStringParser(connectionString.ConnectionString);
            RavenConnectionStringOptions options = parser.Parse();

            IDocumentStore documentStore = new DocumentStore
            {
                Urls = options.Urls,
                Database = options.DefaultDatabase
            };

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
                    IDocumentSession dbSession = _CommandInstanceCall.Value;

                    if (dbSession == null)
                    {
                        dbSession = _Store.OpenSession();
                        _CommandInstanceCall.Value = dbSession;
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
                    IAsyncDocumentSession dbSession = _QueryInstanceCall.Value;

                    if (dbSession == null)
                    {
                        dbSession = _Store.OpenAsyncSession();
                        _QueryInstanceCall.Value = dbSession;
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
            if (_CommandInstanceCall.Value != null)
            {
                _CommandInstanceCall.Value.Dispose();
            }

            _CommandInstanceCall.Value = null;
        }
        #endregion

        #region 释放Raven（读）上下文对象 —— static void FreeQueryInstanceCall()
        /// <summary>
        /// 释放Raven（读）上下文对象
        /// </summary>
        public static void FreeQueryInstanceCall()
        {
            if (_QueryInstanceCall.Value != null)
            {
                _QueryInstanceCall.Value.Dispose();
            }

            _QueryInstanceCall.Value = null;
        }
        #endregion

        #endregion
    }
}
