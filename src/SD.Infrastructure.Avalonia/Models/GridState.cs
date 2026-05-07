using Avalonia;

namespace SD.Infrastructure.Avalonia.Models
{
    /// <summary>
    /// Grid状态
    /// </summary>
    public class GridState
    {
        /// <summary>
        /// 行索引
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 跨行数
        /// </summary>
        public int RowSpan { get; set; }

        /// <summary>
        /// 跨列数
        /// </summary>
        public int ColumnSpan { get; set; }

        /// <summary>
        /// 边距
        /// </summary>
        public Thickness Margin { get; set; }

        /// <summary>
        /// Z索引
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// 是否已扩张
        /// </summary>
        public bool IsExpanded { get; set; }
    }
}
