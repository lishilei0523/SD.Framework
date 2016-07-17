using System;
using System.Runtime.Remoting.Messaging;
using ShSoft.Infrastructure.Constants;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.DomainEventBase
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
            this.Id = Guid.NewGuid();
            this.Handled = false;
            this.AddedTime = DateTime.Now;

            #region # SessionId处理

            object sessionIdCache = CallContext.GetData(CacheConstants.SessionIdKey);

            if (sessionIdCache == null)
            {
                throw new ApplicationException("SessionId未设置，请检查程序！");
            }

            this.SessionId = (Guid)sessionIdCache;

            #endregion
        }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        protected DomainEvent(DateTime? triggerTime = null)
            : this()
        {
            this.TriggerTime = triggerTime;
        }
        #endregion

        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; protected set; }
        #endregion

        #region 是否已处理 —— bool Handled
        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool Handled { get; set; }
        #endregion

        #region 事件源数据序列化字符串 —— string SourceDataStr
        /// <summary>
        /// 事件源数据序列化字符串
        /// </summary>
        public string SourceDataStr { get; protected set; }
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

        #region 会话Id —— Guid SessionId
        /// <summary>
        /// 会话Id
        /// </summary>
        public Guid SessionId { get; private set; }
        #endregion

        #endregion
    }
}
