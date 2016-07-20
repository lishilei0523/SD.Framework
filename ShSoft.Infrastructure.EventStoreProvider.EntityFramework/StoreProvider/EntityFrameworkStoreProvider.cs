using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.EventBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.Infrastructure.EventStoreProvider.EntityFramework.Migrations;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventStoreProvider
{
    /// <summary>
    /// 领域事件存储 - EF提供者
    /// </summary>
    public class EntityFrameworkStoreProvider : DbContext, IEventStore
    {
        #region # 构造器

        #region 00.静态构造器
        /// <summary>
        /// 静态构造器
        /// </summary>
        static EntityFrameworkStoreProvider()
        {
            //数据迁移
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityFrameworkStoreProvider, Configuration>());
        }
        #endregion

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        public EntityFrameworkStoreProvider()
            : base(CommonConstants.DbSessionConstructorArg)
        {
            this.Database.CreateIfNotExists();
        }
        #endregion

        #endregion

        #region # 属性

        #region 事件源所在程序集 —— string EventSourceAssembly
        /// <summary>
        /// 事件源所在程序集
        /// </summary>
        public string EventSourceAssembly
        {
            get { return WebConfigSetting.EventSourceAssembly; }
        }
        #endregion

        #region 数据表名前缀 —— string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public string TablePrefix
        {
            get { return WebConfigSetting.TablePrefix; }
        }
        #endregion

        #endregion

        #region # 方法

        #region 模型映射事件 —— override void OnModelCreating(DbModelBuilder...
        /// <summary>
        /// 模型映射事件
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region # 验证程序集

            if (string.IsNullOrWhiteSpace(this.EventSourceAssembly))
            {
                throw new ApplicationException("事件源所在程序集未配置！");
            }

            #endregion

            //注册事件源基类
            modelBuilder.RegisterEntityType(typeof(Event));

            //设置Id、非自增长
            modelBuilder.Entity<Event>().HasKey(x => x.Id);
            modelBuilder.Entity<Event>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //配置事件源表名
            modelBuilder.Entity<Event>().ToTable(string.Format("{0}{1}", this.TablePrefix, typeof(Event).Name));

            //加载模型所在程序集查询出所有符合条件的实体类型
            IEnumerable<Type> types = Assembly.Load(this.EventSourceAssembly).GetTypes().Where(x => !x.IsInterface && x.IsSubclassOf(typeof(Event)));

            //注册实体配置
            this.RegisterEntityTypes(modelBuilder, types);
        }
        #endregion

        #region 注册实体类型 —— void RegisterEntityTypes(DbModelBuilder modelBuilder...
        /// <summary>
        /// 注册实体类型
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        /// <param name="entityTypes">实体类型集</param>
        private void RegisterEntityTypes(DbModelBuilder modelBuilder, IEnumerable<Type> entityTypes)
        {
            foreach (Type entityType in entityTypes)
            {
                modelBuilder.RegisterEntityType(entityType);
            }
        }
        #endregion

        #region 挂起领域事件 —— void Suspend<T>(T eventSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="eventSource">领域事件源</param>
        public void Suspend<T>(T eventSource) where T : class, IEvent
        {
            this.Set<T>().Add(eventSource);
            this.SaveChanges();
        }
        #endregion

        #region 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            #region # SessionId处理

            object sessionIdCache = CallContext.GetData(CacheConstants.SessionIdKey);

            if (sessionIdCache == null)
            {
                throw new ApplicationException("SessionId未设置，请检查程序！");
            }

            Guid sessionId = (Guid)sessionIdCache;

            #endregion

            Expression<Func<Event, bool>> condition =
                x =>
                    !x.Handled &&
                    x.SessionId == sessionId;

            IOrderedQueryable<Event> eventSources = this.Set<Event>().Where(condition).OrderBy(x => x.AddedTime);

            //如果有未处理的
            if (eventSources.Any())
            {
                foreach (Event eventSource in eventSources)
                {
                    EventMediator.Handle((IEvent)eventSource);
                    eventSource.Handled = true;
                }
                this.SaveChanges();
            }

            //递归
            if (this.Set<Event>().Any(condition))
            {
                this.HandleUncompletedEvents();
            }
        }
        #endregion

        #region 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            List<Event> eventSources = this.Set<Event>().Where(x => !x.Handled).ToList();

            foreach (Event eventSource in eventSources)
            {
                this.Set<Event>().Remove(eventSource);
            }

            this.SaveChanges();
        }
        #endregion

        #endregion
    }
}
