using System;

namespace SD.Infrastructure.PresentationBase
{
    /// <summary>
    /// 模型基类
    /// </summary>
    [Serializable]
    public abstract class Model : ModelBase
    {
        #region 是否选中 —— bool? IsSelected
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? IsSelected { get; set; }
        #endregion

        #region 是否勾选 —— bool? IsChecked
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool? IsChecked { get; set; }
        #endregion
    }
}
