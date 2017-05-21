using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 聚合根实体基类
    /// </summary>
    [Serializable]
    public abstract class AggregateRootEntity : PlainEntity, INumerable, INameable, ISearchable
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        protected AggregateRootEntity()
        {
            this.SavedTime = DateTime.Now;
            this.Deleted = false;
            this.DeletedTime = null;
        }
        #endregion

        #region # 属性

        #region 编号 —— string Number
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; protected set; }
        #endregion

        #region 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; protected set; }
        #endregion

        #region 关键字 —— string Keywords
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; private set; }
        #endregion

        #region 保存时间 —— DateTime SavedTime
        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime SavedTime { get; protected internal set; }
        #endregion

        #region 逻辑删除标记 —— bool Deleted
        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool Deleted { get; protected internal set; }
        #endregion

        #region 删除时间 —— DateTime? DeletedTime
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedTime { get; protected internal set; }
        #endregion

        #region 创建人账号 —— string CreatorAccount
        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatorAccount { get; protected internal set; }
        #endregion

        #region 操作人账号 —— string OperatorAccount
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperatorAccount { get; protected internal set; }
        #endregion

        #endregion

        #region # 方法

        #region 设置关键字 —— void SetKeywords(string keywords)
        /// <summary>
        /// 设置关键字
        /// </summary>
        /// <param name="keywords">关键字</param>
        public void SetKeywords(string keywords)
        {
            this.Keywords = keywords;
        }
        #endregion

        #endregion
    }
}
