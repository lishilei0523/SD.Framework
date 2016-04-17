using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using ShSoft.Framework2016.Infrastructure.Constants;
using ShSoft.Framework2016.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2016.Infrastructure.DomainEvent.EFStorer.Provider
{
    /// <summary>
    /// 领域事件存储者 - EF提供者
    /// </summary>
    public abstract class EntityFrameworkStorerProvider : DbContext, IDomainEventStorer
    {
        #region # 构造器

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        protected EntityFrameworkStorerProvider()
            : base(CommonConstants.EventDbContextConstructArg)
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
            modelBuilder.RegisterEntityType(typeof(IDomainEvent.DomainEvent));

            //设置Id、非自增长
            modelBuilder.Entity<IDomainEvent.DomainEvent>().HasKey(x => x.Id);
            modelBuilder.Entity<IDomainEvent.DomainEvent>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //配置事件源表名
            modelBuilder.Entity<IDomainEvent.DomainEvent>().ToTable(string.Format("{0}{1}", this.TablePrefix, typeof(IDomainEvent.DomainEvent).Name));

            //加载模型所在程序集查询出所有符合条件的实体类型
            IEnumerable<Type> types = Assembly.Load(this.EventSourceAssembly).GetTypes().Where(x => !x.IsInterface && x.IsSubclassOf(typeof(IDomainEvent.DomainEvent)));

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

        #region 初始化存储 —— virtual void InitStore()
        /// <summary>
        /// 初始化存储
        /// </summary>
        public virtual void InitStore()
        {

        }
        #endregion

        #region 挂起领域事件 —— void Suspend<T>(T domainSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="domainSource">领域事件源</param>
        public void Suspend<T>(T domainSource) where T : class, IDomainEvent.IDomainEvent
        {
            this.Set<T>().Add(domainSource);
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

            Expression<Func<IDomainEvent.DomainEvent, bool>> condition =
                x =>
                    !x.Handled &&
                    x.SessionId == sessionId;

            IOrderedQueryable<IDomainEvent.DomainEvent> eventSources = this.Set<IDomainEvent.DomainEvent>().Where(condition).OrderBy(x => x.AddedTime);

            //如果有未处理的
            if (eventSources.Any())
            {
                foreach (IDomainEvent.DomainEvent eventSource in eventSources)
                {
                    eventSource.Handle();
                }
                this.SaveChanges();
            }

            //递归
            if (this.Set<IDomainEvent.DomainEvent>().Any(condition))
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
            List<IDomainEvent.DomainEvent> eventSources = this.Set<IDomainEvent.DomainEvent>().Where(x => !x.Handled).ToList();

            foreach (IDomainEvent.DomainEvent eventSource in eventSources)
            {
                this.Set<IDomainEvent.DomainEvent>().Remove(eventSource);
            }

            this.SaveChanges();
        }
        #endregion

        #endregion
    }
}
