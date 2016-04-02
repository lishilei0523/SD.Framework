using System;
using System.Collections.Generic;
using System.Linq;
using SD.Toolkits.EntityFramework.Base;
using ShSoft.Framework2015.Infrastructure.Constants;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.Providers
{
    /// <summary>
    /// 领域事件存储者 - EF提供者
    /// </summary>
    public abstract class EFDomainEventStorerProvider : BaseDbContext, IDomainEventStorer
    {
        #region # 构造器

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        protected EFDomainEventStorerProvider()
            : base(CommonConstants.EventDbContextConstructArg)
        {

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
            get { return WebConfigSetting.EventSourceAssembly; }
        }
        #endregion

        #region 实体配置所在程序集 —— override string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public override string EntityConfigAssembly
        {
            get { return WebConfigSetting.EventSourceConfigAssembly; }
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
                return type => type.IsSubclassOf(typeof(IDomainEvent.DomainEvent));
            }
        }
        #endregion

        #region 单独注册的类型集 —— override IEnumerable<Type> TypesToRegister
        /// <summary>
        /// 单独注册的类型集
        /// </summary>
        public override IEnumerable<Type> TypesToRegister
        {
            get { return new[] { typeof(IDomainEvent.DomainEvent) }; }
        }
        #endregion

        #region 数据表名前缀 —— override string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public override string TablePrefix
        {
            get { return WebConfigSetting.TablePrefix; }
        }
        #endregion

        #endregion

        #region # 方法

        #region # 初始化存储 —— virtual void InitStore()
        /// <summary>
        /// 初始化存储
        /// </summary>
        public virtual void InitStore()
        {

        }
        #endregion

        #region # 挂起领域事件 —— void Suspend<T>(T domainSource)
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

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            IOrderedQueryable<IDomainEvent.DomainEvent> eventSources = this.Set<IDomainEvent.DomainEvent>().Where(x => !x.Handled).OrderBy(x => x.AddedTime);

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
            if (this.Set<IDomainEvent.DomainEvent>().Any(x => !x.Handled))
            {
                this.HandleUncompletedEvents();
            }
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
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
