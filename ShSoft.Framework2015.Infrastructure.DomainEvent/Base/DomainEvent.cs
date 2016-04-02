using System;
using ShSoft.Framework2015.Infrastructure.DomainEvent.Mediator;

// ReSharper disable once CheckNamespace
namespace ShSoft.Framework2015.Infrastructure.IDomainEvent
{
    /// <summary>
    /// 领域事件源基类
    /// </summary>
    [Serializable]
    public class DomainEvent : IDomainEvent
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DomainEvent()
        {
            //默认值
            this.Handled = false;
            this.AddedTime = DateTime.Now;
        }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        protected DomainEvent(DateTime? triggerTime)
            : this()
        {
            this.TriggerTime = triggerTime;
        }
        #endregion

        #endregion

        #region # 属性

        #region 是否已处理 —— bool Handled
        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool Handled { get; private set; }
        #endregion

        #region 触发时间 —— DateTime? TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime? TriggerTime { get; private set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 处理 —— void Handle()
        /// <summary>
        /// 处理
        /// </summary>
        public void Handle()
        {
            EventMediator.Handle((IDomainEvent)this);
            this.Handled = true;
        }
        #endregion

        #endregion
    }
}
