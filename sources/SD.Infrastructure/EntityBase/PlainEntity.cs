using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    [Serializable]
    public abstract class PlainEntity : INumerable, INameable, ISearchable
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        protected PlainEntity()
        {
            this.Id = Guid.NewGuid();
            this.AddedTime = DateTime.Now;
            this.SavedTime = DateTime.Now;
            this.Deleted = false;
            this.DeletedTime = null;
        }
        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; protected set; }
        #endregion

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

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; private set; }
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

        #region 设置标识Id，慎用！ —— void SetId(Guid id)
        /// <summary>
        /// 设置标识Id，慎用！
        /// </summary>
        /// <param name="id">标识Id</param>
        public void SetId(Guid id)
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", @"Id不可为空！");
            }

            #endregion

            this.Id = id;
        }
        #endregion

        #endregion
    }
}
