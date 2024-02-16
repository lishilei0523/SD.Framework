using System;
using System.Collections.Generic;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 实体历史接口
    /// </summary>
    public interface IEntityHistory
    {
        #region 操作类型 —— ActionType ActionType
        /// <summary>
        /// 操作类型
        /// </summary>
        ActionType ActionType { get; }
        #endregion

        #region 实体类型 —— Type EntityType
        /// <summary>
        /// 实体类型
        /// </summary>
        Type EntityType { get; }
        #endregion

        #region 实体对象Id —— Guid EntityId
        /// <summary>
        /// 实体对象Id
        /// </summary>
        Guid EntityId { get; }
        #endregion

        #region 操作前快照 —— IDictionary<string, object> BeforeSnapshot
        /// <summary>
        /// 操作前快照
        /// </summary>
        IDictionary<string, object> BeforeSnapshot { get; }
        #endregion

        #region 操作后快照 —— IDictionary<string, object> AfterSnapshot
        /// <summary>
        /// 操作后快照
        /// </summary>
        IDictionary<string, object> AfterSnapshot { get; }
        #endregion

        #region 操作人账号 —— string OperatorAccount
        /// <summary>
        /// 操作人账号
        /// </summary>
        string OperatorAccount { get; }
        #endregion

        #region 操作人姓名 —— string OperatorName
        /// <summary>
        /// 操作人姓名
        /// </summary>
        string OperatorName { get; }
        #endregion

        #region 保存时间 —— DateTime SavedTime
        /// <summary>
        /// 保存时间
        /// </summary>
        DateTime SavedTime { get; }
        #endregion 
    }
}
