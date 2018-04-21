using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using SD.IOC.Core.Mediator;
using SD.Toolkits.EntityFramework.Base;
using System;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace SD.Infrastructure.Repository.EntityFramework.Base
{
    /// <summary>
    /// DbSession基类
    /// </summary>
    public abstract class BaseDbSession : BaseDbContext
    {
        #region # 常量与构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BaseDbSession()
        {
            _Sync = new object();
        }

        /// <summary>
        /// EF（写）上下文对象缓存键
        /// </summary>
        internal const string CommandInstanceKey = "CommandInstance";

        /// <summary>
        /// EF（读）上下文对象缓存键
        /// </summary>
        internal const string QueryInstanceKey = "QueryInstance";

        /// <summary>
        /// 基础构造器
        /// </summary>
        protected BaseDbSession()
            : base(GlobalSetting.DefaultConnectionString)
        {
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion

        #region # 访问器

        #region EF（写）上下文对象访问器 —— static object CommandInstanceCall
        /// <summary>
        /// EF（写）上下文对象访问器
        /// </summary>
        private static object CommandInstanceCall
        {
            get { return CallContext.LogicalGetData(CommandInstanceKey); }
            set { CallContext.LogicalSetData(CommandInstanceKey, value); }
        }
        #endregion

        #region EF（读）上下文对象访问器 —— static object QueryInstanceCall
        /// <summary>
        /// EF（读）上下文对象访问器
        /// </summary>
        private static object QueryInstanceCall
        {
            get { return CallContext.LogicalGetData(QueryInstanceKey); }
            set { CallContext.LogicalSetData(QueryInstanceKey, value); }
        }
        #endregion

        #region EF（写）上下文对象 —— static DbContext CommandInstance
        /// <summary>
        /// EF（写）上下文对象
        /// </summary>
        public static DbContext CommandInstance
        {
            get
            {
                lock (_Sync)
                {
                    DbContext dbContext = CommandInstanceCall as DbContext;

                    if (dbContext == null)
                    {
                        dbContext = ResolveMediator.Resolve<BaseDbSession>();
                        CommandInstanceCall = dbContext;
                    }

                    return dbContext;
                }
            }
        }
        #endregion

        #region EF（读）上下文对象 —— static DbContext QueryInstance
        /// <summary>
        /// EF（读）上下文对象
        /// </summary>
        public static DbContext QueryInstance
        {
            get
            {
                lock (_Sync)
                {
                    DbContext dbContext = QueryInstanceCall as DbContext;

                    if (dbContext == null)
                    {
                        dbContext = ResolveMediator.Resolve<BaseDbSession>();
                        dbContext.Configuration.AutoDetectChangesEnabled = false;/*关闭自动跟踪实体变化状态*/

                        QueryInstanceCall = dbContext;
                    }

                    return dbContext;
                }
            }
        }
        #endregion

        #region 释放EF（写）上下文对象 —— static void FreeCommandInstanceCall()
        /// <summary>
        /// 释放EF（写）上下文对象
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

        #region 释放EF（读）上下文对象 —— static void FreeQueryInstanceCall()
        /// <summary>
        /// 释放EF（读）上下文对象
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

        #region # 属性

        #region 实体所在程序集 —— override string EntityAssembly
        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public override string EntityAssembly
        {
            get { return GlobalSetting.EntityAssembly; }
        }
        #endregion

        #region 实体配置所在程序集 —— override string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public override string EntityConfigAssembly
        {
            get { return GlobalSetting.EntityConfigAssembly; }
        }
        #endregion

        #region 类型查询条件 —— override Func<Type, bool> TypeQuery
        /// <summary>
        /// 类型查询条件
        /// </summary>
        public override Func<Type, bool> TypeQuery
        {
            get
            {
                return type =>
                    type != typeof(PlainEntity) &&
                    type != typeof(AggregateRootEntity) &&
                    type.IsSubclassOf(typeof(PlainEntity));
            }
        }
        #endregion

        #region 数据表名前缀 —— override string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public override string TablePrefix
        {
            get { return GlobalSetting.TablePrefix; }
        }
        #endregion

        #endregion
    }
}
