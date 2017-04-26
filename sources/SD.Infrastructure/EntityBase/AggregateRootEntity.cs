using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 聚合根实体基类
    /// </summary>
    [Serializable]
    public abstract class AggregateRootEntity : PlainEntity
    {
        #region 操作人账号 —— string OperatorAccount
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperatorAccount { get; protected internal set; }
        #endregion
    }
}
