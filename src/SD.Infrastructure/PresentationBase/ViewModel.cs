using System;

namespace SD.Infrastructure.PresentationBase
{
    /// <summary>
    /// 视图模型基类
    /// </summary>
    [Serializable]
    public abstract class ViewModel
    {
        #region # 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region # 编号 —— string Number
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
        #endregion

        #region # 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region # 添加时间 —— DateTime AddedTime
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddedTime { get; set; }
        #endregion
    }
}
