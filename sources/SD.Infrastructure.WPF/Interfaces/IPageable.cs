namespace SD.Infrastructure.WPF.Interfaces
{
    /// <summary>
    /// 可分页接口
    /// </summary>
    public interface IPageable
    {
        #region # 属性

        #region 页码 —— int PageIndex
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }
        #endregion

        #region 页容量 —— int PageSize
        /// <summary>
        /// 页容量
        /// </summary>
        int PageSize { get; set; }
        #endregion

        #region 总记录数 —— int RowCount
        /// <summary>
        /// 总记录数
        /// </summary>
        int RowCount { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 刷新数据 —— void RefreshData()
        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData();
        #endregion

        #endregion
    }
}
