using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 实体历史
    /// </summary>
    public interface IEntityHistory
    {
        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        Guid Id { get; }
        #endregion

        #region 实体类型 —— string EntityType
        /// <summary>
        /// 实体类型
        /// </summary>
        string EntityType { get; }
        #endregion

        #region 操作前快照 —— string BeforeSnapshot
        /// <summary>
        /// 操作前快照
        /// </summary>
        string BeforeSnapshot { get; }
        #endregion

        #region 操作后快照 —— string AfterSnapshot
        /// <summary>
        /// 操作后快照
        /// </summary>
        string AfterSnapshot { get; }
        #endregion

        #region 保存时间 —— DateTime SavedTime
        /// <summary>
        /// 保存时间
        /// </summary>
        DateTime SavedTime { get; }
        #endregion 
    }
}
