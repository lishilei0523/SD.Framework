namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 可分区接口
    /// </summary>
    public interface IPartible
    {
        /// <summary>
        /// 分区索引
        /// </summary>
        int PartitionIndex { get; }
    }
}
