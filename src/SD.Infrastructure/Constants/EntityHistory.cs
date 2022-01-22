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


    /// <summary>
    /// 实体历史
    /// </summary>
    internal class EntityHistory : IEntityHistory
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected EntityHistory()
        {
            this.SavedTime = DateTime.Now;
        }
        #endregion

        #region 01.创建实体历史构造器
        /// <summary>
        /// 创建实体历史构造器
        /// </summary>
        /// <param name="actionType">操作类型</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体对象Id</param>
        /// <param name="beforeSnapshot">操作前快照</param>
        /// <param name="afterSnapshot">操作后快照</param>
        /// <param name="operatorAccount">操作人账号</param>
        /// <param name="operatorName">操作人姓名</param>
        public EntityHistory(ActionType actionType, Type entityType, Guid entityId, IDictionary<string, object> beforeSnapshot, IDictionary<string, object> afterSnapshot, string operatorAccount, string operatorName)
            : this()
        {
            this.ActionType = actionType;
            this.EntityType = entityType;
            this.EntityId = entityId;
            this.BeforeSnapshot = beforeSnapshot;
            this.AfterSnapshot = afterSnapshot;
            this.OperatorAccount = operatorAccount;
            this.OperatorName = operatorName;
        }
        #endregion

        #endregion

        #region # 属性

        #region 操作类型 —— ActionType ActionType
        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType ActionType { get; private set; }
        #endregion

        #region 实体类型 —— Type EntityType
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }
        #endregion

        #region 实体对象Id —— Guid EntityId
        /// <summary>
        /// 实体对象Id
        /// </summary>
        public Guid EntityId { get; private set; }
        #endregion

        #region 操作前快照 —— IDictionary<string, object> BeforeSnapshot
        /// <summary>
        /// 操作前快照
        /// </summary>
        public IDictionary<string, object> BeforeSnapshot { get; set; }
        #endregion

        #region 操作后快照 —— IDictionary<string, object> AfterSnapshot
        /// <summary>
        /// 操作后快照
        /// </summary>
        public IDictionary<string, object> AfterSnapshot { get; set; }
        #endregion

        #region 操作人账号 —— string OperatorAccount
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperatorAccount { get; private set; }
        #endregion

        #region 操作人姓名 —— string OperatorName
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperatorName { get; private set; }
        #endregion

        #region 保存时间 —— DateTime SavedTime
        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime SavedTime { get; set; }
        #endregion  

        #endregion
    }
}
