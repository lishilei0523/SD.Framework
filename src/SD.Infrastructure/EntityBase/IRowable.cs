namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 可分行接口
    /// </summary>
    public interface IRowable
    {
        /// <summary>
        /// 行号
        /// </summary>
        long RowNo { get; }
    }
}
