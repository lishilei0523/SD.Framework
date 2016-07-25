using System;
using ShSoft.Infrastructure.Constants;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.EventBase
{
    /// <summary>
    /// 领域事件源基类
    /// </summary>
    [Serializable]
    public abstract class Event : IEvent
    {
        #region # 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Event()
        {
            //默认值
            this.Id = Guid.NewGuid();
            this.Handled = false;
            this.AddedTime = DateTime.Now;
            this.SessionId = WebConfigSetting.CurrentSessionId;
            this.TriggerTime = null;
        }
        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; set; }
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
        public string SourceDataStr { get; set; }
        #endregion

        #region 触发时间 —— DateTime? TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime? TriggerTime { get; set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; set; }
        #endregion

        #region 会话Id —— Guid SessionId
        /// <summary>
        /// 会话Id
        /// </summary>
        public Guid SessionId { get; set; }
        #endregion

        #endregion
    }
}
