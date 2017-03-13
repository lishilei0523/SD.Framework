using SD.Infrastructure.Constants;
using System;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.EventBase
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
