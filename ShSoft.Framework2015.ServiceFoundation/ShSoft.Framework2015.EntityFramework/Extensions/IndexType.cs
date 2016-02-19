using System;

namespace ShSoft.Framework2015.EntityFramework.Extensions
{
    /// <summary>
    /// 索引类型
    /// </summary>
    [Flags]
    public enum IndexType
    {
        /// <summary>
        /// 非聚集索引
        /// </summary>
        Nonclustered = 0,

        /// <summary>
        /// 聚集索引
        /// </summary>
        Clustered = 1,

        /// <summary>
        /// 唯一键
        /// </summary>
        Unique = 2,

        /// <summary>
        /// 聚集并唯一键
        /// </summary>
        ClusteredUnique = Clustered | Unique
    }
}