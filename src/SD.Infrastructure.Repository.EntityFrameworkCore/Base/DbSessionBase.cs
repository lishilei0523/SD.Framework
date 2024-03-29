﻿using Microsoft.EntityFrameworkCore;
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
    public abstract class DbSessionBase : DbContextBase
    {
        #region # 构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// EF（写）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<DbSessionBase> _CommandInstanceCall;

        /// <summary>
        /// EF（读）上下文对象访问器线程缓存
        /// </summary>
        private static readonly AsyncLocal<DbSessionBase> _QueryInstanceCall;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DbSessionBase()
        {
            _Sync = new object();
            _CommandInstanceCall = new AsyncLocal<DbSessionBase>();
            _QueryInstanceCall = new AsyncLocal<DbSessionBase>();
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        protected DbSessionBase()
        {
            //默认超时时间
            base.Database.SetCommandTimeout(120);
        }

        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //开启代理类延迟加载
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        #endregion

        #region # 访问器

        #region EF（写）上下文对象 —— static DbSessionBase CommandInstance
        /// <summary>
        /// EF（写）上下文对象
        /// </summary>
        public static DbSessionBase CommandInstance
        {
            get
            {
                lock (_Sync)
                {
                    DbSessionBase dbContext = _CommandInstanceCall.Value;
                    if (dbContext == null || dbContext.Disposed)
                    {
                        dbContext = ResolveMediator.Resolve<DbSessionBase>();
                        dbContext.Database.SetConnectionString(dbContext.WriteConnectionString);
                        _CommandInstanceCall.Value = dbContext;
                    }

                    return dbContext;
                }
            }
        }
        #endregion

        #region EF（读）上下文对象 —— static DbSessionBase QueryInstance
        /// <summary>
        /// EF（读）上下文对象
        /// </summary>
        public static DbSessionBase QueryInstance
        {
            get
            {
                lock (_Sync)
                {
                    DbSessionBase dbContext = _QueryInstanceCall.Value;
                    if (dbContext == null || dbContext.Disposed)
                    {
                        dbContext = ResolveMediator.Resolve<DbSessionBase>();
                        dbContext.Database.SetConnectionString(dbContext.ReadConnectionString);
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

        #region 读连接字符串 —— virtual string ReadConnectionString
        /// <summary>
        /// 读连接字符串
        /// </summary>
        public virtual string ReadConnectionString
        {
            get { return GlobalSetting.ReadConnectionString; }
        }
        #endregion

        #region 写连接字符串 —— virtual string WriteConnectionString
        /// <summary>
        /// 写连接字符串
        /// </summary>
        public virtual string WriteConnectionString
        {
            get { return GlobalSetting.WriteConnectionString; }
        }
        #endregion

        #region 实体所在程序集 —— override string EntityAssembly
        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public override string EntityAssembly
        {
            get { return FrameworkSection.Setting.EntityAssembly.Value; }
        }
        #endregion

        #region 实体配置所在程序集 —— override string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public override string EntityConfigAssembly
        {
            get { return FrameworkSection.Setting.EntityConfigAssembly.Value; }
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
            get { return FrameworkSection.Setting.EntityTablePrefix.Value; }
        }
        #endregion

        #endregion
    }
}
