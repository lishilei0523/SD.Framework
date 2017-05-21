using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    [Serializable]
    public abstract class PlainEntity
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        protected PlainEntity()
        {
            this.Id = Guid.NewGuid();
            this.AddedTime = DateTime.Now;
        }
        #endregion

        #region # 属性

        #region 标识 —— Guid Id
        /// <summary>
        /// 标识
        /// </summary>
        public Guid Id { get; protected set; }
        #endregion

        #region 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; private set; }
        #endregion

        #endregion

        #region # 方法

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
