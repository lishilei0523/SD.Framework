namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 可并发接口
    /// </summary>
    public interface IConcurrency
    {
        /// <summary>
        /// 行版本
        /// </summary>
        byte[] RowVersion { get; }
    }
}
