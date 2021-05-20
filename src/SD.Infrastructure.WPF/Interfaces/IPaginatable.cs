namespace SD.Infrastructure.WPF.Interfaces
{
    /// <summary>
    /// 可分页接口
    /// </summary>
    public interface IPaginatable
    {
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 页容量
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        int RowCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; set; }
    }
}
