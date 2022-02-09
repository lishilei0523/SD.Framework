using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 可删除接口
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletedTime { get; }
    }
}
