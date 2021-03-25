using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.Constants;
using SD.Infrastructure.EntityBase;
using SD.IOC.Core.Mediators;
using SD.Toolkits.EntityFrameworkCore.Base;
using System;
using System.Threading;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Base
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
        /// EF（写）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<BaseDbSession> _CommandInstanceCall;

        /// <summary>
        /// EF（读）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<BaseDbSession> _QueryInstanceCall;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BaseDbSession()
        {
            _Sync = new object();
            _CommandInstanceCall = new AsyncLocal<BaseDbSession>();
            _QueryInstanceCall = new AsyncLocal<BaseDbSession>();
        }

        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }

        #endregion

        #region # 访问器

        #region EF（写）上下文对象 —— static BaseDbSession CommandInstance
        /// <summary>
        /// EF（写）上下文对象
        /// </summary>
        public static BaseDbSession CommandInstance
        {
            get
            {
                lock (_Sync)
                {
                    BaseDbSession dbContext = _CommandInstanceCall.Value;

                    if (dbContext == null || dbContext.Disposed)
                    {
                        dbContext = ResolveMediator.Resolve<BaseDbSession>();
                        _CommandInstanceCall.Value = dbContext;
                    }

                    return dbContext;
                }
            }
        }
        #endregion

        #region EF（读）上下文对象 —— static BaseDbSession QueryInstance
        /// <summary>
        /// EF（读）上下文对象
        /// </summary>
        public static BaseDbSession QueryInstance
        {
            get
            {
                lock (_Sync)
                {
                    BaseDbSession dbContext = _QueryInstanceCall.Value;

                    if (dbContext == null || dbContext.Disposed)
                    {
                        dbContext = ResolveMediator.Resolve<BaseDbSession>();
                        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;/*关闭自动跟踪实体变化状态*/

                        _QueryInstanceCall.Value = dbContext;
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
            _CommandInstanceCall.Value?.Dispose();
            _CommandInstanceCall.Value = null;
        }
        #endregion

        #region 释放EF（读）上下文对象 —— static void FreeQueryInstanceCall()
        /// <summary>
        /// 释放EF（读）上下文对象
        /// </summary>
        public static void FreeQueryInstanceCall()
        {
            _QueryInstanceCall.Value?.Dispose();
            _QueryInstanceCall.Value = null;
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
