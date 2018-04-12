using System;

namespace SD.Infrastructure.EventBase
{
    /// <summary>
    /// 领域事件源基接口
    /// </summary>
    public interface IEvent
    {
        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        Guid Id { get; }
        #endregion

        #region 是否已处理 —— bool Handled
        /// <summary>
        /// 是否已处理
        /// </summary>
        bool Handled { get; }
        #endregion

        #region 会话Id —— Guid SessionId
        /// <summary>
        /// 会话Id
        /// </summary>
        Guid SessionId { get; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        DateTime AddedTime { get; }
        #endregion
    }
}
